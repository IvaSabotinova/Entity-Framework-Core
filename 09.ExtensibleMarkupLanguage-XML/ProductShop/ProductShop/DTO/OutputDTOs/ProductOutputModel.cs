using System.Xml.Serialization;

namespace ProductShop.DTO.OutputDTOs
{
    [XmlType("Product")]
    public class ProductOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }  

    }
}