using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Projection")]
    public class ImportProjectionsXmlModel
    {
        public int MovieId { get; set; }
        [Required]
        public string DateTime { get; set; }
    }
}
//<Projections>
//  <Projection>
//    <MovieId>38</MovieId>
//    <DateTime>2019-04-27 13:33:20</DateTime>
//  </Projection>
