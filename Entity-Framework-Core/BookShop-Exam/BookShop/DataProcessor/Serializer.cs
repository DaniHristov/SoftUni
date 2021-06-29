namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors.Select(x => new
            {
                AuthorName = x.FirstName + " " + x.LastName,
                Books = x.AuthorsBooks.OrderByDescending(s => s.Book.Price)
                .Select(ab => new
                {
                    BookName = ab.Book.Name,
                    BookPrice = ab.Book.Price.ToString("F2"),
                }).ToArray() 
                
            })
                .ToArray()
                .OrderByDescending(a => a.Books.Count())
                .ThenBy(a => a.AuthorName)
                .ToArray();

            var result = JsonConvert.SerializeObject(authors, Formatting.Indented);
            return result;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {


            var books = context.Books.Where(x => x.Genre == Genre.Science /*Enum.Parse<Genre>("Science"*/ && x.PublishedOn <= date).ToArray().OrderByDescending(x => x.Pages).ThenByDescending(x => x.PublishedOn).Take(10).Select(x => new BookExportXmlModel
            {
                Pages = x.Pages,
                Name = x.Name,
                Date = x.PublishedOn.ToString("d",CultureInfo.InvariantCulture)
            }).ToArray();

            var result = XmlConverter.Serialize(books, "Books");
            return result;
        }

    }
}