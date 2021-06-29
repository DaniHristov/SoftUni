using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Ticket")]
    public class ImportTicketXmlModel
    {
        public int ProjectionId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
//      <Ticket>
//        <ProjectionId>1</ProjectionId>
//        <Price>7</Price>
//      </Ticket>