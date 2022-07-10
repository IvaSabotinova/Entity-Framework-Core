namespace FastFood.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Item
	{
		public int Id { get; set; }

		[StringLength(30, MinimumLength = 3)]
		public string Name { get; set; }

		public int CategoryId { get; set; }

		[Required]
		public Category Category { get; set; }

		[Range(typeof(decimal), "0.01", "79228162514264337593543950335")]

        [Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; }
	}
}