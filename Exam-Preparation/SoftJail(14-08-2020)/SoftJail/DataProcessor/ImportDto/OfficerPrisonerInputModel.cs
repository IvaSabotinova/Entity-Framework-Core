using SoftJail.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class OfficerPrisonerInputModel
    {
        [Required]
        [StringLength(30, MinimumLength =3)]
        [XmlElement("Name")]
        public string FullName { get; set; }

        [XmlElement("Money")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Salary { get; set; }

        [Required]
        [EnumDataType(typeof(Position))]
        [XmlElement(nameof(Position))]
        public string Position { get; set; }

        [Required]
        [EnumDataType(typeof(Weapon))]
        [XmlElement(nameof(Weapon))]
        public string Weapon { get; set; }

        [XmlElement(nameof(DepartmentId))]
        public int DepartmentId { get; set; }

        [XmlArray(nameof(Prisoners))]
        public PrisonerOfOfficerInputModel[] Prisoners { get; set; }
    }

    [XmlType("Prisoner")]
    public class PrisonerOfOfficerInputModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

    }
}
