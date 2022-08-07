namespace TeisterMask.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Projects");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectInputModel[]), xmlRootAttribute);

            using StringReader reader = new StringReader(xmlString);

            ProjectInputModel[] dtoProjects = (ProjectInputModel[])xmlSerializer.Deserialize(reader);

            StringBuilder sb = new StringBuilder();

            List<Project> projects = new List<Project>();

            foreach (ProjectInputModel dtoProject in dtoProjects)
            {
                bool isValidOpenDate = DateTime.TryParseExact(dtoProject.OpenDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime openDate);
                if (!IsValid(dtoProject) || !isValidOpenDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Project newProject = new Project()
                {
                    Name = dtoProject.Name,
                    OpenDate = openDate,
                    DueDate = DateTime.TryParseExact(dtoProject.DueDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate) ? (DateTime?)dueDate : null,

                };
                List<Task> validTasks = new List<Task>();

                foreach (TaskOfProjectInputModel dtoTask in dtoProject.Tasks)
                {
                    bool isTaskOpenDateValid = DateTime.TryParseExact(dtoTask.OpenDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskOpenDate);

                    bool isTaskDueDateValid = DateTime.TryParseExact(dtoTask.DueDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDueDate);

                    if (!IsValid(dtoTask) || !isTaskOpenDateValid || !isTaskDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if(taskOpenDate < newProject.OpenDate || taskDueDate > newProject.DueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Task newTask = new Task()
                    {
                        Name = dtoTask.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = Enum.Parse<ExecutionType>(dtoTask.ExecutionType),
                        LabelType = Enum.Parse<LabelType>(dtoTask.LabelType)

                    };
                    validTasks.Add(newTask);
                }
                newProject.Tasks = validTasks;
                projects.Add(newProject);
                sb.AppendLine(String.Format(SuccessfullyImportedProject, newProject.Name, newProject.Tasks.Count));
            }

            context.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            EmployeeInputModel[] dtoEmployees = JsonConvert.DeserializeObject<EmployeeInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Employee> employees = new List<Employee>();    

            foreach (EmployeeInputModel dtoEmployee in dtoEmployees)
            {
                if (!IsValid(dtoEmployee))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee newEmployee = new Employee()
                {
                    Username = dtoEmployee.Username,
                    Email = dtoEmployee.Email,
                    Phone = dtoEmployee.Phone,
                    
                };

                IEnumerable<int> existingTasksIds = context.Tasks.Select(x => x.Id).ToList();

                foreach (int dtoTaskId in dtoEmployee.Tasks.Distinct())
                {
                    if (!existingTasksIds.Contains(dtoTaskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    newEmployee.EmployeesTasks.Add(new EmployeeTask
                    {
                        TaskId = dtoTaskId
                    });
                }
                employees.Add(newEmployee);
                sb.AppendLine(String.Format(SuccessfullyImportedEmployee, newEmployee.Username, newEmployee.EmployeesTasks.Count));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}