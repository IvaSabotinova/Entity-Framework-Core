using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.OutputDTOs
{
    [XmlType("car")]
    public class CarOutputModel
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("travelled-distance")]
        public long TravelledDistance { get; set; }

    }

}
