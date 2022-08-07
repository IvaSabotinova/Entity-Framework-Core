using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ProjectOutputModel
    {
        [XmlAttribute(nameof(TasksCount))]
        public int TasksCount { get; set; }

        [XmlElement(nameof(ProjectName))]
        public string ProjectName { get; set; }

        [XmlElement(nameof(HasEndDate))]
        public string HasEndDate { get; set; }

        [XmlArray(nameof(Tasks))]
        public TaskOutputModel[] Tasks { get; set; }
    }

    [XmlType("Task")]
    public class TaskOutputModel
    {
        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [XmlElement(nameof(Label))]
        public string Label { get; set; } 

    }

}
