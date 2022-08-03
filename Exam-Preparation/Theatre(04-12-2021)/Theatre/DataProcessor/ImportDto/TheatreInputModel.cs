using System;
using System.ComponentModel.DataAnnotations;

namespace Theatre.DataProcessor.ImportDto
{
    public class TheatreInputModel
    {
        [Required]
        [StringLength(30, MinimumLength =4)]
        public string Name { get; set; }

        [Range(0,10)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Director { get; set; }
        public TicketInputModel[] Tickets { get; set; }

    }

    public class TicketInputModel
    {
        [Range(typeof(decimal), "1.00", "100.00")]
        public decimal Price { get; set; }

        [Range(1,10)]
        public sbyte RowNumber { get; set; }

        public int PlayId { get; set; }
    }
   


}
