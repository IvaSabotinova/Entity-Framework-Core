using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Theatre.Data.Models
{
    public class Theatre
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } 
        public sbyte NumberOfHalls { get; set; }

        [Required]
        public string Director { get; set; } 
        public ICollection<Ticket> Tickets { get; set; }

    }

}
