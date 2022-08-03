using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theatre.Data.Models
{
    public class Cast
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }    

        public bool IsMainCharacter { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; } 
        public Play Play { get; set; }

    }
}