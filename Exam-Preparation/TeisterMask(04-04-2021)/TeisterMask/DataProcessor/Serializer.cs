namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            ProjectOutputModel[] dtoProjects = context.Projects.Where(x=>x.Tasks.Any()).ToArray()
                .Select(x=> new ProjectOutputModel
                {
                    TasksCount = x.Tasks.Count,  
                    ProjectName = x.Name,
                    HasEndDate = x.DueDate.HasValue ? "Yes" : "No",
                    Tasks = x.Tasks.Select(t=> new TaskOutputModel
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString(),    

                    })
                    .OrderBy(x=>x.Name)
                    .ToArray() 
                })
                .OrderByDescending(x=>x.TasksCount)
                .ThenBy(x=>x.ProjectName)
                .ToArray();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Projects");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectOutputModel[]), xmlRootAttribute);

            StringBuilder sb = new StringBuilder();

            using StringWriter writer = new StringWriter(sb); 

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            xmlSerializer.Serialize(writer, dtoProjects, xmlSerializerNamespaces);  

            return writer.ToString();   
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var dtoEmployees = context.Employees.Where(x => x.EmployeesTasks.Any(t => t.Task.OpenDate >= date)).ToList()
                .Select(x=> new
                {
                Username = x.Username,
                Tasks = x.EmployeesTasks.Where(et=>et.Task.OpenDate >= date)
                .OrderByDescending(x => x.Task.DueDate)
                .ThenBy(x => x.Task.Name)
                .Select( et=> new
                {
                    TaskName = et.Task.Name,
                    OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                    DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                    LabelType = et.Task.LabelType.ToString(),
                    ExecutionType = et.Task.ExecutionType.ToString()
                })
                .ToList(),
            })
            .OrderByDescending(x=>x.Tasks.Count)
            .ThenBy(x=>x.Username)
            .Take(10)
            .ToList();

            return JsonConvert.SerializeObject(dtoEmployees, Formatting.Indented);
        }
    }
}