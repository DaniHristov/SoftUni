using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Product")]
    public class SoldProductsCountDto
    {
        [XmlElement("count")]
        public int Count { get; set; }
        [XmlElement("products")]
        public SoldProductsOutput[] SoldProducts { get; set; }
    }
}