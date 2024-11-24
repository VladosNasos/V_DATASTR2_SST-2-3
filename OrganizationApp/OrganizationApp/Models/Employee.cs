using System.Text.Json.Serialization;

namespace OrganizationApp.Models
{
    // Employee class representing an employee
    public class Employee
    {
        public string Name { get; set; }

        [JsonConstructor]
        public Employee(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
