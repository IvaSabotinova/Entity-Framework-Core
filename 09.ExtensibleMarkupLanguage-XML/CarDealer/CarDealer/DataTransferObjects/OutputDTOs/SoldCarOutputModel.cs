using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.OutputDTOs
{
    [XmlType("car")]
    public class SoldCarOutputModel
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }

    }
}