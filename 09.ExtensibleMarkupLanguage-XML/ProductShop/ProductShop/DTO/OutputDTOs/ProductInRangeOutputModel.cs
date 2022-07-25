using System.Xml.Serialization;

namespace ProductShop.DTO.OutputDTOs
{
    [XmlType("Product")]
    public class ProductInRangeOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("buyer")]
        public string BuyerName { get; set; }   

    }

}
