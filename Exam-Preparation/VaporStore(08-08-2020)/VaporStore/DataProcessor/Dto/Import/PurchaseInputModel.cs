using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.Dto.Import
{
    [XmlType("Purchase")]
    public class PurchaseInputModel
    {
        [Required]
        [XmlAttribute("title")]
        public string GameName { get; set; }

        [Required]
        [EnumDataType(typeof(PurchaseType))]
        [XmlElement("Type")]
        public string Type { get; set; }

        [Required]
        [RegularExpression("^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        [XmlElement("Key")]
        public string ProductKey { get; set; }

        [Required]
        [RegularExpression("^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$")]
        [XmlElement("Card")]
        public string CardNumber { get; set; }


        [Required]
        [XmlElement("Date")]
        public string Date { get; set; }
            
    }

  }
