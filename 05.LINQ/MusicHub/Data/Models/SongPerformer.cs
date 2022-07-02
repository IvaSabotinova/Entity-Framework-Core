using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
    public class SongPerformer
    {


        public int SongId { get; set; }

        [Required]
        public Song Song { get; set; }
        public int PerformerId { get; set; }

        [Required]
        public Performer Performer { get; set; }    


    }
}
