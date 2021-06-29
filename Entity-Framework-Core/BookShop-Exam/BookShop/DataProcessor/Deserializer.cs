namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportBooksXmlModel[]), new XmlRootAttribute("Books"));
            var books = (ImportBooksXmlModel[])xmlSerializer.Deserialize(new StringReader(xmlString));
            var sb = new StringBuilder();

            foreach (var book in books)
            {
                if (!IsValid(book))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }
                var newBook = new Book()
                {
                    Name = book.Name,
                    Genre = Enum.Parse<Genre>(book.Genre),
                    Price = book.Price,
                    Pages = book.Pages,
                    PublishedOn = DateTime.ParseExact(book.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                };
                context.Books.Add(newBook);
                context.SaveChanges();
                sb.AppendLine($"Successfully imported book {newBook.Name} for {newBook.Price:F2}.");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorsDto = JsonConvert.DeserializeObject<IEnumerable<ImportAuthorsJsonModel>>(jsonString);
            var sb = new StringBuilder();
            var authors = new List<Author>();

            foreach (var author in authorsDto)
            {
                if (!IsValid(author))   
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                if (authors.Any(x=>x.Email == author.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var newAuthor = new Author()
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Phone = author.Phone,
                    Email = author.Email,
                };
                foreach (var bookDto in author.Books)
                {
                    if (!bookDto.Id.HasValue)
                    {
                        continue;
                    }

                    var book = context.Books.FirstOrDefault(x => x.Id == bookDto.Id);

                    if (book == null)
                    {
                        continue;
                    }

                    newAuthor.AuthorsBooks.Add(new AuthorBook
                    {
                        Author = newAuthor,
                        Book = book
                    });
                }
            
                if (newAuthor.AuthorsBooks.Count ==0)
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                authors.Add(newAuthor);
                sb.AppendLine($"Successfully imported author - {newAuthor.FirstName + " " + newAuthor.LastName} with {newAuthor.AuthorsBooks.Count} books.");
            }
            context.Authors.AddRange(authors);
            context.SaveChanges();
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