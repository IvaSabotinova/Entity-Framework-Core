using System.Xml.Serialization;

namespace ProductShop.DTO.OutputDTOs
{
    
    [XmlType("Product")]
    public class SoldProductOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }  

    }
}