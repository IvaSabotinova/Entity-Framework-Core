using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Department
    {
        public Department()
        {
            Cells = new HashSet<Cell>();
        }
        public int Id { get; set; }

        [Required]
     
        public string Name { get; set; }

        public virtual ICollection<Cell> Cells { get; set; }

    }

}