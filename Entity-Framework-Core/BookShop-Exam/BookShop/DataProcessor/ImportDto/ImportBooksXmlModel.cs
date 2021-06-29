using BookShop.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class ImportBooksXmlModel
    {
        [Required]
        [StringLength(30,MinimumLength =3)]
        public string Name { get; set; }
        [Required]
        [EnumDataType(typeof(Genre))]
        public string Genre { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(50,5000)]
        public int Pages { get; set; }
        [Required]
        public string PublishedOn { get; set; }
    }
}
//•	Id - integer, Primary Key
//•	Name - text with length [3, 30]. (required)
//•	Genre - enumeration of type Genre, with possible values (Biography = 1, Business = 2, Science = 3) (required)
//•	Price - decimal in range between 0.01 and max value of the decimal
//•	Pages – integer in range between 50 and 5000
//•	PublishedOn - date and time (required)
//•	AuthorsBooks - collection of type AuthorBook

//<Books>
//  <Book>
//    <Name>Hairy Torchwood</Name>
//    <Genre>3</Genre>
//    <Price>41.99</Price>
//    <Pages>3013</Pages>
//    <PublishedOn>01/13/2013</PublishedOn>
//  </Book>
//  <Book>
//    <Name>Anise Burnet Saxifrage</Name>
//    <Genre>1</Genre>
//    <Price>-1.51</Price>
//    <Pages>2920</Pages>
//    <PublishedOn>12/02/2015</PublishedOn>
//  </Book>
//  <Book>
//    <Name>Hand Fern</Name>
//    <Genre>2</Genre>
//    <Price>3.57</Price>
//    <Pages>5303</Pages>
//    <PublishedOn>02/23/2018</PublishedOn>
//  </Book>
