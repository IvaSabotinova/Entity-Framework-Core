using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class PlayInputModel
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("Duration")]
        public string Duration { get; set; }

        [XmlElement("Rating")]
        [Range(0.00, 10.00)]
        public float Rating { get; set; }

        [XmlElement("Genre")]
        [Required]
        [EnumDataType(typeof(Genre))]
        public string Genre { get; set; }

        [Required]
        [MaxLength(700)]
        [XmlElement("Description")]
        public string Description { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Screenwriter { get; set; }

    }
    
}
