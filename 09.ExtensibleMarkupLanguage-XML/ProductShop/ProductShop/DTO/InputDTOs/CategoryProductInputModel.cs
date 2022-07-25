using System.Xml.Serialization;

namespace ProductShop.DTO.InputDTOs
{
    [XmlType("CategoryProduct")]
    public class CategoryProductInputModel
    {
        [XmlElement]
        public int CategoryId { get; set; }

        [XmlElement]
        public int ProductId { get; set; }

       
    }

}
