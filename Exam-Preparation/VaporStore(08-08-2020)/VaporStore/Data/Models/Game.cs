using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VaporStore.Data.Models
{
    public class Game
    {
        public Game()
        {
            Purchases = new HashSet<Purchase>();
            GameTags = new HashSet<GameTag>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }    
        public decimal Price { get; set; }    
        public DateTime ReleaseDate { get; set; }

        [ForeignKey(nameof(Developer))]
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }

        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public ICollection<Purchase> Purchases { get; set; }    

        public ICollection<GameTag> GameTags { get; set; }
    }

}
