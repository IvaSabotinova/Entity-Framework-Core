using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("User")]
    public class UserPurchaseOutputModel
    {
        [XmlAttribute("username")]
        public string Username { get; set; }

        [XmlArray(nameof(Purchases))]
        public PurchaseOutputModel[] Purchases { get; set; }

        [XmlElement(nameof(TotalSpent))]
        public decimal TotalSpent { get; set; }


    }
    [XmlType("Purchase")]
    public class PurchaseOutputModel
    {
        [XmlElement("Card")]
        public string CardNumber { get; set; }

        [XmlElement(nameof(Cvc))]
        public string Cvc { get; set; }

        [XmlElement(nameof(Date))]
        public string Date { get; set; }

        [XmlElement(nameof(Game))]
        public GameOfPurchaseOutputModel Game { get; set; }

       
    }

    [XmlType("Game")]
    public class GameOfPurchaseOutputModel
    {
        [XmlAttribute("title")]
        public string Name { get; set; }

        [XmlElement(nameof(Genre))]
        public string Genre { get; set; }

        [XmlElement(nameof(Price))]
        public decimal Price { get; set; }

    }
}