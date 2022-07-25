using System.Xml.Serialization;

namespace ProductShop.DTO.OutputDTOs
{
    [XmlType("Category")]
    public class CategoryOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("count")]
        public int NumberOfProducts { get; set; }

        [XmlElement("averagePrice")]
        public decimal AverageProductsPrice { get; set; }

        [XmlElement("totalRevenue")]
        public decimal TotalRevenue { get; set; }

    }
}
