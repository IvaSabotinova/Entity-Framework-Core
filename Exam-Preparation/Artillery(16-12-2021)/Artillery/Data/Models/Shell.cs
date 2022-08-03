using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Artillery.Data.Models
{
    public class Shell
    {
        public int Id { get; set; } 

        public double ShellWeight { get; set; }

        [Required]
        public string Caliber { get; set; } 
        public ICollection<Gun> Guns { get; set; }

    }
}
