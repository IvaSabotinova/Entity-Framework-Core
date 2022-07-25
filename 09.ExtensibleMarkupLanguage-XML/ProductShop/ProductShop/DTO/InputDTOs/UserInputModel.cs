using System.Xml.Serialization;

namespace ProductShop.DTO.InputDTOs
{
    [XmlType("User")]
    public class UserInputModel
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

    }
}
