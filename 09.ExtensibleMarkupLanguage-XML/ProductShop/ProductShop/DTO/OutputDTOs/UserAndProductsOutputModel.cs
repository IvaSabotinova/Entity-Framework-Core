using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.DTO.OutputDTOs
{
    [XmlType("User")]
    public class UserAndProductsOutputModel
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public List<SoldProductOutputModel> SoldProducts { get; set; }

    }


}
