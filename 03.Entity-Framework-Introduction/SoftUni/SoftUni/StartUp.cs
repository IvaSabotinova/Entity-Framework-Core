using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public static class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext softUniContext = new SoftUniContext();

            //Console.WriteLine(GetEmployeesFullInformation(softUniContext));  //T03Employees Full Information

            //Console.WriteLine(GetEmployeesWithSalaryOver50000(softUniContext)); //T04EmployeesWithSalaryOver50000

            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(softUniContext));   //T05 Employees from Research and Development

            //Console.WriteLine(AddNewAddressToEmployee(softUniContext)); //T06 Adding a New Address and Updating Employee

            //Console.WriteLine(GetEmployeesInPeriod(softUniContext));  //T07 Employees and Projects

            //Console.WriteLine(GetAddressesByTown(softUniContext)); //T08 Addresses by Town

            //Console.WriteLine(GetEmployee147(softUniContext)); //T09 Employee 147

            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(softUniContext)); //T10 Departments with More Than 5 Employees
            //Console.WriteLine(GetLatestProjects(softUniContext));  //T11 Find Latest 10 Projects

            //Console.WriteLine(IncreaseSalaries(softUniContext));  //T12 Increase Salaries

            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(softUniContext)); //T13 Find Employees by First Name Starting with "Sa"

            //Console.WriteLine(DeleteProjectById(softUniContext)); //T14 Delete Project by Id

            Console.WriteLine(RemoveTown(softUniContext)); //T15 Remove Town

        }

        //T03 Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees.Select(x => new
            {
                x.EmployeeId,
                x.FirstName,
                x.MiddleName,
                x.LastName,
                x.JobTitle,
                x.Salary

            }).OrderBy(x => x.EmployeeId).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        //T04 Employees with Salary Over 50 000

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees.Where(x => x.Salary > 50000).Select(x => new
            {
                x.FirstName,
                x.Salary

            }).OrderBy(x => x.FirstName).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        //T05 Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees.Where(x => x.Department.Name == "Research and Development").Select(x => new
            {
                x.FirstName,
                x.LastName,
                departmentName = x.Department.Name,
                x.Salary

            }).OrderBy(x => x.Salary).ThenByDescending(x => x.FirstName).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.departmentName} - ${employee.Salary:f2}");
            }
            return sb.ToString().TrimEnd();

        }

        //T06 Adding a New Address and Updating Employee

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {

            Address nakovAddress = new Address { AddressText = "Vitoshka 15", TownId = 4 };

            Employee employeeNakov = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");

            context.Addresses.Add(nakovAddress);

            employeeNakov.Address = nakovAddress;
            context.SaveChanges();

            var employees = context.Employees.OrderByDescending(x => x.AddressId).Select(x => new
            {
                x.Address.AddressText,

            }).Take(10).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine(employee.AddressText);
            }
            return sb.ToString().TrimEnd();

        }

        //T07 Employees and Projects

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {

            var employees = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeesProjects.Any(x => x.Project.StartDate.Year >= 2001 && x.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    ManagerName = x.Manager.FirstName + " " + x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name,
                        ProjectStartDate = p.Project.StartDate,
                        ProjectEndDate = p.Project.EndDate
                    })
                }).Take(10);

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerName}");
                foreach (var project in employee.Projects)
                {
                    string endDate = project.ProjectEndDate.HasValue 
                        ? project.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) 
                        : "not finished";

                    sb.AppendLine($"--{project.ProjectName} - {project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {endDate}");

                }
            }
            return sb.ToString().TrimEnd();
        }

        //T08 Addresses by Town

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses.Include(x => x.Employees).Include(x => x.Town).Select(x => new
            {
                NumberOfEmployees = x.Employees.Count(),
                TownName = x.Town.Name,
                x.AddressText
            }).OrderByDescending(x => x.NumberOfEmployees).ThenBy(x => x.TownName).ThenBy(x => x.AddressText).Take(10).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.NumberOfEmployees} employees");

            }
            return sb.ToString().TrimEnd();
        }

        //T09 Employee 147

        public static string GetEmployee147(SoftUniContext context)
        {

            var employeeId147 = context.Employees.Where(x => x.EmployeeId == 147).Select(x => new
            {
                EmployeeFullName = x.FirstName + " " + x.LastName,
                EmployeeJobTitle = x.JobTitle,
                Projects = x.EmployeesProjects.Select(x => new
                {
                    ProjectName = x.Project.Name
                }).OrderBy(x => x.ProjectName).ToList()
            })
                .FirstOrDefault();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{employeeId147.EmployeeFullName} - {employeeId147.EmployeeJobTitle}");
            foreach (var project in employeeId147.Projects)
            {
                sb.AppendLine($"{project.ProjectName}");
            }

            return sb.ToString().TrimEnd();
        }

        //T10 Departments with More Than 5 Employees

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {

            var departmentsWithMoreThan5Employees = context.Departments.Where(x => x.Employees.Count > 5)
                 .OrderBy(x => x.Employees.Count).ThenBy(x => x.Name)
                .Select(x => new
                {
                    DepartmentName = x.Name,
                    DeptManagerName = x.Manager.FirstName + " " + x.Manager.LastName,
                    DepartmentEmployees = x.Employees.Select(e => new
                    {
                        EmployeeFirstName = e.FirstName,
                        EmployeeLastName = e.LastName,
                        EmployeeJobTitle = e.JobTitle
                    })
                    .OrderBy(e => e.EmployeeFirstName).ThenBy(e => e.EmployeeLastName).ToList()
                })

                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var department in departmentsWithMoreThan5Employees)
            {
                sb.AppendLine($"{department.DepartmentName} - {department.DeptManagerName}");
                foreach (var employee in department.DepartmentEmployees)
                {
                    sb.AppendLine($"{employee.EmployeeFirstName} {employee.EmployeeLastName} - {employee.EmployeeJobTitle}");
                }
            }
            return sb.ToString().TrimEnd();

        }

        //T11 Find Latest 10 Projects

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects.OrderByDescending(p => p.StartDate).Take(10).Select(p => new
            {
                p.Name,
                p.StartDate,
                p.Description
            }).OrderBy(x => x.Name).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}\n{project.Description}\n{project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }
            return sb.ToString().TrimEnd();

        }

        //T12 Increase Salaries

        public static string IncreaseSalaries(SoftUniContext context)
        {
            List<Employee> employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || 
                e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (Employee employee in employees)
            {
                employee.Salary *= 1.12M;
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        //T13 Find Employees by First Name Starting with "Sa"

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            List<Employee> employees = context.Employees.Where(x => x.FirstName.ToLower().StartsWith("sa")).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (Employee employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }
            return sb.ToString().TrimEnd();
        }

        //T14 Delete Project by Id

        public static string DeleteProjectById(SoftUniContext context)
        {

            List<EmployeeProject> employeesProject = context.EmployeesProjects.Where(x => x.ProjectId == 2).ToList();

            context.EmployeesProjects.RemoveRange(employeesProject);

            Project projectToDelete = context.Projects.Find(2);

            context.Projects.Remove(projectToDelete);

            context.SaveChanges();

            List<Project> projectList = context.Projects.Take(10).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (Project project in projectList)
            {
                sb.AppendLine(project.Name);
            }
            return sb.ToString().TrimEnd();
        }

        //T15 Remove Town
        public static string RemoveTown(SoftUniContext context)
        {
            Town town = context.Towns.Where(x => x.Name == "Seattle").FirstOrDefault();

            List<int> IdsOfAddressesToDelete = context.Addresses.Where(x => x.Town.Name == "Seattle").Select(x => x.AddressId).ToList();
            List<int> employeesAddressesForNulling = context.Employees
                .Where(x => x.AddressId.HasValue && IdsOfAddressesToDelete.Contains(x.AddressId.Value))
                .Select(x => x.EmployeeId)
                .ToList();

            foreach (Employee emp in context.Employees)
            {
                if (employeesAddressesForNulling.Contains(emp.EmployeeId))
                {
                    emp.AddressId = null;
                }
            }
            context.SaveChanges();

            context.Addresses.RemoveRange(context.Addresses.Where(x => x.Town.Name == "Seattle").ToList());

            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{IdsOfAddressesToDelete.Count} addresses in Seattle were deleted";
        }
    }
}
