using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ExportDto
{
    [XmlType("Customer")]
    public class ExportTopCustomersXmlModel
    {
        [XmlAttribute("FirstName")]
        public string FirstName { get; set; }

        [XmlAttribute("LastName")]
        public string LastName { get; set; }

        [XmlElement("SpentMoney")]
        public decimal SpentMoney { get; set; }

        [XmlElement("SpentTime")]
        [DisplayFormat(DataFormatString = @"hh\:mm\:ss" , ApplyFormatInEditMode = true)]
        public long SpentTime { get; set; }
    }
}
  //<Customer FirstName="Marjy" LastName="Starbeck">
  //  <SpentMoney>82.65</SpentMoney>
  //  <SpentTime>12:10:00</SpentTime>
  //</Customer>
  //<Customer FirstName="Jerrie" LastName="O\'Carroll">
  //  <SpentMoney>67.13</SpentMoney>
  //  <SpentTime>13:20:00</SpentTime>
  //</Customer>
  //<Customer FirstName="Randi" LastName="Ferraraccio">
  //  <SpentMoney>63.39</SpentMoney>
  //  <SpentTime>06:36:00</SpentTime>
  //</Customer>
