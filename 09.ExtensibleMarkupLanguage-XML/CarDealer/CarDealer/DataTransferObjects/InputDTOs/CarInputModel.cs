using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.InputDTOs
{
    [XmlType("Car")]
    public class CarInputModel
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("TraveledDistance")]

        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public List<CarPartsInputModel> PartsIds { get; set; }


    }
}
