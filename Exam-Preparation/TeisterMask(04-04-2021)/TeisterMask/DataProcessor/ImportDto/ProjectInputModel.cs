using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ProjectInputModel
    {
        [Required]
        [XmlElement(nameof(Name))]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [XmlElement(nameof(OpenDate))]
        public string OpenDate { get; set; }

        [XmlElement(nameof(DueDate))]
        public string DueDate { get; set; }

        [XmlArray(nameof(Tasks))]
        public TaskOfProjectInputModel[] Tasks { get; set; }
    }
  
    [XmlType("Task")]
    public class TaskOfProjectInputModel
    {
        [Required]
        [XmlElement(nameof(Name))]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [XmlElement(nameof(OpenDate))]
        public string OpenDate { get; set; }

        [Required]
        [XmlElement(nameof(DueDate))]
        public string DueDate { get; set; }

        [EnumDataType(typeof(ExecutionType))]
        public string ExecutionType { get; set; }
              
        [EnumDataType(typeof(LabelType))]
        public string LabelType { get; set; }

    }
}

