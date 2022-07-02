using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
    public class Producer
    {

        public Producer()
        {
            Albums = new HashSet<Album>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }  
        
        public string Pseudonym { get; set; }   

        public string PhoneNumber { get; set; } 

        public ICollection<Album> Albums { get; set; }

    }
}
