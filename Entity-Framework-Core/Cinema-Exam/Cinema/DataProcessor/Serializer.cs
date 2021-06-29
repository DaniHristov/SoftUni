namespace Cinema.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var topMovies = context.Movies.Where(x => (int)x.Rating >= rating && x.Projections.Any(x => x.Tickets.Count > 0)).ToList().Select(x => new
            {
                MovieName = x.Title,
                Rating = x.Rating.ToString("f2"),
                TotalIncomes = x.Projections.Select(x=>x.Tickets.Select(x=>x.Customer.Tickets.Sum(x=>x.Price))),
                Customers = x.Projections.ToList().Select(x => x.Tickets.Select(x => new
                {
                    FirstName = x.Customer.FirstName,
                    LastName = x.Customer.LastName,
                    Balance = x.Customer.Balance.ToString("F2")
                }).ToList()
                .OrderByDescending(x => x.Balance)
                .ThenBy(x => x.FirstName).ThenBy(x => x.LastName)
                )

            }).Take(10).OrderByDescending(x => x.Rating).ThenBy(x => x.TotalIncomes).ToList();

                


            var result = JsonConvert.SerializeObject(topMovies, Formatting.Indented);

            return result;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context.Customers.Where(x => x.Age >= age).Select(x => new ExportTopCustomersXmlModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                SpentMoney = x.Tickets.Sum(x => x.Price),
            })
                .ToList();


            var result = XmlConverter.Serialize(customers, "Customers");

            return result;
        }


    }
}
//: "hh\:mm\:ss"). 