using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class CastInputModel
    {
        [XmlElement("FullName")]
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string FullName { get; set; }

        [XmlElement("IsMainCharacter")]
        public bool IsMainCharacter { get; set; }

        [Required]
        [XmlElement("PhoneNumber")]
        [RegularExpression(@"\+44-[0-9]{2}-[0-9]{3}-[0-9]{4}")]
        public string PhoneNumber { get; set; }

        [XmlElement("PlayId")]
        public int PlayId { get; set; }
    }
}
