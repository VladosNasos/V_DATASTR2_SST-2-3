using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace OrganizationApp.Models
{
    // Department class representing a department in the organization
    public class Department
    {
        public string Name { get; set; }
        public string ManagerName { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Department> SubDepartments { get; set; }

        [JsonConstructor]
        public Department(string name, string managerName)
        {
            Name = name;
            ManagerName = managerName;
            Employees = new List<Employee>();
            SubDepartments = new List<Department>();
        }

        // Add a sub-department
        public void AddSubDepartment(Department subDept)
        {
            SubDepartments.Add(subDept);
        }

        // Remove a sub-department by name
        public bool RemoveSubDepartment(string subDeptName)
        {
            var subDept = SubDepartments.FirstOrDefault(d => d.Name.Equals(subDeptName, System.StringComparison.OrdinalIgnoreCase));
            if (subDept != null)
            {
                SubDepartments.Remove(subDept);
                return true;
            }
            return false;
        }

        // Add an employee
        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
        }

        // Remove an employee by name
        public bool RemoveEmployee(string employeeName)
        {
            var employee = Employees.FirstOrDefault(e => e.Name.Equals(employeeName, System.StringComparison.OrdinalIgnoreCase));
            if (employee != null)
            {
                Employees.Remove(employee);
                return true;
            }
            return false;
        }

        // Move an employee to another department
        public bool MoveEmployee(string employeeName, Department targetDept)
        {
            var employee = Employees.FirstOrDefault(e => e.Name.Equals(employeeName, System.StringComparison.OrdinalIgnoreCase));
            if (employee != null)
            {
                Employees.Remove(employee);
                targetDept.AddEmployee(employee);
                return true;
            }
            return false;
        }

        // Move a sub-department to another department
        public bool MoveSubDepartment(string subDeptName, Department targetDept)
        {
            var subDept = SubDepartments.FirstOrDefault(d => d.Name.Equals(subDeptName, System.StringComparison.OrdinalIgnoreCase));
            if (subDept != null)
            {
                SubDepartments.Remove(subDept);
                targetDept.AddSubDepartment(subDept);
                return true;
            }
            return false;
        }

        // Calculate the total number of employees, including sub-departments
        public int GetTotalEmployees()
        {
            int count = Employees.Count;
            foreach (var subDept in SubDepartments)
            {
                count += subDept.GetTotalEmployees();
            }
            return count;
        }

        // Find a department by name (recursive)
        public Department FindDepartment(string deptName)
        {
            if (Name.Equals(deptName, System.StringComparison.OrdinalIgnoreCase))
                return this;

            foreach (var subDept in SubDepartments)
            {
                var found = subDept.FindDepartment(deptName);
                if (found != null)
                    return found;
            }
            return null;
        }

        // Find parent department of a given department
        public Department FindParentDepartment(Department targetDept)
        {
            foreach (var subDept in SubDepartments)
            {
                if (subDept == targetDept)
                    return this;

                var parent = subDept.FindParentDepartment(targetDept);
                if (parent != null)
                    return parent;
            }
            return null;
        }

        // Check if the current department is a sub-department of the given department
        public bool IsSubDepartmentOf(Department potentialParent)
        {
            if (SubDepartments.Contains(potentialParent))
                return true;

            foreach (var subDept in SubDepartments)
            {
                if (subDept.IsSubDepartmentOf(potentialParent))
                    return true;
            }
            return false;
        }
    }
}
