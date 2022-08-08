using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class CoachWithFootballersInputModel
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [XmlElement("Nationality")]
        [Required]
        public string Nationality { get; set; }

        [XmlArray("Footballers")]
        public FootballerOfCoachInputModel[] Footballers { get; set; }
    }
    [XmlType("Footballer")]
    public class FootballerOfCoachInputModel
    {
        [Required]
        [StringLength(40, MinimumLength =2)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [XmlElement("ContractStartDate")]
        public string ContractStartDate { get; set; }

        [Required]
        [XmlElement("ContractEndDate")]
        public string ContractEndDate { get; set; }


        [Required]
        [XmlElement("BestSkillType")]
        [EnumDataType(typeof(BestSkillType))]
        public string BestSkillType { get; set; }

        [Required]
        [XmlElement("PositionType")]
        [EnumDataType(typeof(PositionType))]
        public string PositionType { get; set; }
       
    }
  }
