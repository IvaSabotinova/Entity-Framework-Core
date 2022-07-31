using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P03_SalesDatabase.Data.Models
{
    public class Store
    {
        public Store()
        {
            Sales = new HashSet<Sale>();
        }
        public int StoreId { get; set; }

        [MaxLength(Constants.StoreNameMaxLength)]
        public string Name { get; set; }   

        public ICollection<Sale> Sales { get; set; }

    }

}
