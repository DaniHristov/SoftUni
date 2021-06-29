using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{

    [XmlType("Book")]
    public class BookExportXmlModel
    {
        [XmlAttribute("Pages")]
        public int Pages { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }
        
        public string Date { get; set; }
    }
}
