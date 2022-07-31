using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_SalesDatabase.Data.Models
{
    public class Customer
    {
        public Customer()
        {
            Sales = new HashSet<Sale>();
        }
        public int CustomerId { get; set; }

        [MaxLength(Constants.CustomerNameMaxLength)]
        public string Name { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string Email { get; set; } 
        public string CreditCardNumber { get; set; } 
        public ICollection<Sale> Sales { get; set; }
       
    }

}
