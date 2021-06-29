using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Customer")]
    public class ImportTicketCustomerXmlModel
    {
        [Required]
        [StringLength(20,MinimumLength =3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string LastName { get; set; }
        [Range(12,110)]
        public int Age { get; set; }
        [Range(0.01,double.MaxValue)]
        public decimal Balance { get; set; }
        [XmlArray("Tickets")]
        public ImportTicketXmlModel[] Tickets { get; set; }
    }
}
//•	Id – integer, Primary Key
//•	FirstName – text with length [3, 20] (required)
//•	LastName – text with length [3, 20] (required)
//•	Age – integer in the range[12, 110] (required)
//•	Balance - decimal(non - negative, minimum value: 0.01)(required)
//•	Tickets - collection of type Ticket


//<Customer>
//    <FirstName>Randi</FirstName>
//    <LastName>Ferraraccio</LastName>
//    <Age>20</Age>
//    <Balance>59.44</Balance>
//    <Tickets>
//      <Ticket>
//        <ProjectionId>1</ProjectionId>
//        <Price>7</Price>
//      </Ticket>
//      <Ticket>
//        <ProjectionId>1</ProjectionId>
//        <Price>15</Price>
//      </Ticket>
//      <Ticket>
//        <ProjectionId>1</ProjectionId>
//        <Price>12.13</Price>
//      </Ticket>
//      <Ticket>
//        <ProjectionId>1</ProjectionId>
//        <Price>11</Price>
//      </Ticket>
//      <Ticket>
//        <ProjectionId>1</ProjectionId>
//        <Price>9.13</Price>
//      </Ticket>
//      <Ticket>
//        <ProjectionId>1</ProjectionId>
//        <Price>9.13</Price>
//      </Ticket>
//    </Tickets>
//  </Customer>
