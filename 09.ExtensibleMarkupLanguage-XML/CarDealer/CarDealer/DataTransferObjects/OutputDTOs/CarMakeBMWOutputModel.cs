using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.OutputDTOs
{
    [XmlType("car")]
    public class CarMakeBMWOutputModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }


    }
 
}
