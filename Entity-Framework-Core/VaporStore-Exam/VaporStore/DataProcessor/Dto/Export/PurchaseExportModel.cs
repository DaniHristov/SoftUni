using System;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Purchase")]
    public class PurchaseExportModel
    {
        public string Card { get; set; }

        public string Cvc { get; set; }

        public DateTime Date { get; set; }

        public GameExportModel Game { get; set; }

    }
}
//        <Card>7991 7779 5123 9211</Card>
//        <Cvc>340</Cvc>
//        <Date>2017-08-31 17:09</Date>
//        <Game title="Counter-Strike: Global Offensive">
//          <Genre>Action</Genre>
//          <Price>12.49</Price>
//        </Game>