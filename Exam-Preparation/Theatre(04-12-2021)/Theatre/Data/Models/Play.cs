using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } 
        public TimeSpan Duration { get; set; } 
        public float Rating { get; set; }
        public Genre Genre { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Screenwriter { get; set; }

        public ICollection<Cast> Casts { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }

}
