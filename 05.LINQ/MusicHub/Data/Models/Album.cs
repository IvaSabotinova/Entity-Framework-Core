using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
            Songs = new HashSet<Song>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public decimal Price => Songs.Sum(x => x.Price); 

        [ForeignKey(nameof(Producer))]
        public int? ProducerId { get; set; }

        public Producer Producer { get; set; }  

        public ICollection<Song> Songs { get; set; }
    }
}
