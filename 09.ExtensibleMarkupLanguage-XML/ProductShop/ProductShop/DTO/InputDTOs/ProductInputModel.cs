using System.Xml.Serialization;

namespace ProductShop.DTO.InputDTOs
{
    [XmlType("Product")]
    public class ProductInputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("sellerId")]
        public int SellerId { get; set; }

        [XmlElement("buyerId")]
        public int? BuyerId { get; set; }


    }
 
}
