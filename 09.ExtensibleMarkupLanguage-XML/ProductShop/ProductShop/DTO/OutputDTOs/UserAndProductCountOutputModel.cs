using System.Xml.Serialization;

namespace ProductShop.DTO.OutputDTOs
{
    [XmlType("User")]
    public class UserAndProductCountOutputModel
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlIgnore]
        public bool AgeSpecified { get { return this.Age != null; } }

        [XmlElement("SoldProducts")]
        public SoldProductCountOutputModel SoldProducts { get; set; }


    }
}
