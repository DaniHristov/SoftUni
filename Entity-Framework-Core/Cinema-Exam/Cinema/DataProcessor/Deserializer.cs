namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";

        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";

        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesDto = JsonConvert.DeserializeObject<IEnumerable<ImportMovieJsonModel>>(jsonString);
            var sb = new StringBuilder();
            var movies = new List<Movie>();

            foreach (var movie in moviesDto)
            {
                if (!IsValid(movie))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (movies.Any(x=>x.Title == movie.Title))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newMovie = new Movie()
                {
                    Title = movie.Title,
                    Genre = movie.Genre,
                    Duration = TimeSpan.ParseExact(movie.Duration, "c", CultureInfo.InvariantCulture),
                    Rating = movie.Rating,
                    Director = movie.Director

                };

                movies.Add(newMovie);
                sb.AppendLine($"Successfully imported {newMovie.Title} with genre {newMovie.Genre} and rating {newMovie.Rating:F2}!");
            }
            context.Movies.AddRange(movies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportProjectionsXmlModel[]), new XmlRootAttribute("Projections"));
            var projectionsDto = (ImportProjectionsXmlModel[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            foreach (var projection in projectionsDto)
            {
                if (!IsValid(projection))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movieTitle = context.Movies.Where(x => x.Id == projection.MovieId).FirstOrDefault();
                if (movieTitle == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newProjection = new Projection()
                {
                    MovieId = projection.MovieId,
                    DateTime = DateTime.ParseExact(projection.DateTime,"yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture,DateTimeStyles.None)
                };

                context.Projections.Add(newProjection);
                context.SaveChanges();
                //var projectionDateParsed = DateTime.ParseExact(newProjection.DateTime.ToString(), "MM/dd/yyyy", null);
                sb.AppendLine($"Successfully imported projection {movieTitle.Title} on {newProjection.DateTime:MM/dd/yyyy}!");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportTicketCustomerXmlModel[]), new XmlRootAttribute("Customers"));
            var customersDto = (ImportTicketCustomerXmlModel[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            foreach (var customer in customersDto)
            {
                if (!IsValid(customer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newCustomer = new Customer()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Age = customer.Age,
                    Balance = customer.Balance,
                };

                foreach (var ticket in customer.Tickets)
                {
                    if (!IsValid(ticket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var ticketNew = new Ticket()
                    {
                        ProjectionId = ticket.ProjectionId,
                        Price = ticket.Price
                    };

                    newCustomer.Tickets.Add(ticketNew);
                    
                }
                context.Customers.Add(newCustomer);
                context.SaveChanges();
                sb.AppendLine($"Successfully imported customer {newCustomer.FirstName} {newCustomer.LastName} with bought tickets: {newCustomer.Tickets.Count}!");
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}