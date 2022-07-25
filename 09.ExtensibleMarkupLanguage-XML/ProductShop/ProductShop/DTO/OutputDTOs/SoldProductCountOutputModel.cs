using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.DTO.OutputDTOs
{
    [XmlType("SoldProducts")]
    public class SoldProductCountOutputModel
    {
        [XmlElement("count")]
        public int ProductsCount { get; set; }

        [XmlArray("products")]
        public List<ProductOutputModel> Products { get; set; }

    }
}