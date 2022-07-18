using System.Xml.Serialization;

namespace ProductShop.DTO.InputDTOs
{

    [XmlType("Category")]
    public class CategoryInputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

    }
}
