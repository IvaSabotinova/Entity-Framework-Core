using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.OutputDTOs
{
    [XmlType("sale")]
    public class SaleOutputModel
    {
        [XmlElement("car")]
        public SoldCarOutputModel SoldCar { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("price-with-discount")]
        public decimal PriceWithDiscount { get; set; }

    }
 
}
