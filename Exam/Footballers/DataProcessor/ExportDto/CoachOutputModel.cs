using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class CoachOutputModel
    {
        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }

        [XmlElement("CoachName")]
        public string CoachName { get; set; }

        [XmlArray("Footballers")]
        public FootballerOfCoachOutputModel[] Footballers { get; set; }

    }

    [XmlType("Footballer")]
    public class FootballerOfCoachOutputModel
    {
        [XmlElement("Name")]
        public string FootballerName { get; set; }

        [XmlElement("Position")]
        public string PositionType { get; set; }

    }
}
