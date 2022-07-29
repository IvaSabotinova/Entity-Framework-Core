using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("User")]
    public class UserWithPurchasesOutputModel
    {
        [XmlAttribute("username")]
        public string Username { get; set; }


        [XmlArray("Purchases")]
        public PurchaseOutputModel[] Purchases { get; set; }

        [XmlElement("TotalSpent")]
        public decimal MoneySpent { get; set; }
    }

    [XmlType("Purchase")]
    public class PurchaseOutputModel
    {
        [XmlElement("Card")]
        public string CardNumber { get; set; }

        [XmlElement("Cvc")]
        public string Cvc { get; set; }

        [XmlElement("Date")]
        public string DateTime { get; set; }

        [XmlElement("Game")]
        public XmlGameOutputModel XmlGame { get; set; }

      
    }

    [XmlType("Game")]
    public class XmlGameOutputModel
    {
        [XmlAttribute("title")]
        public string GameName { get; set; }

        [XmlElement("Genre")]
        public string GenreName { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
