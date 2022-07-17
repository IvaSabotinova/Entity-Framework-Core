using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.InputDTOs
{
    [XmlType("partId")]
    public class CarPartsInputModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }


    }
}