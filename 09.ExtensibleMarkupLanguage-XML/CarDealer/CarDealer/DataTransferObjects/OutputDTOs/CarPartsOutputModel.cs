using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.OutputDTOs
{
    [XmlType("part")]
    public class CarPartsOutputModel
    {

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
