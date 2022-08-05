using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class CountryInputModel
    {
        [StringLength(60, MinimumLength = 4)]
        [Required]
        [XmlElement("CountryName")]
        public string CountryName { get; set; }

        [Range(50_000, 10_000_000)]
        [XmlElement("ArmySize")]
        public int ArmySize { get; set; }
    }
 
}
