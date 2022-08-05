using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmentCellInputModel
    {
        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }
        public CellInputModel[] Cells { get; set; }
    }

    public class CellInputModel
    {
        [Range(1,1000)]
        public int CellNumber { get; set; }

        public bool HasWindow { get; set; }
    }

 }
