using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.DTO.OutputDTOs
{
    [XmlType("Users")]
    public class FinalResult
    {
        [XmlElement("count")]
        public int CountOfUsers { get; set; }

        [XmlArray("users")]
        public List<UserAndProductCountOutputModel> Users { get; set; }
    }
}