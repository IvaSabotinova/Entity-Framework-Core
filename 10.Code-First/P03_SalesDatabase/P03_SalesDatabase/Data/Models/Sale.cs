using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_SalesDatabase.Data.Models
{
    public class Sale
    {
        public int SaleId { get; set; } 
        public DateTime Date { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }  
        public Product Product { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; } 
        public Customer Customer { get; set; }

        [ForeignKey(nameof(Store))]
        public int StoreId { get; set; }
        public Store Store { get; set; }

    }

}