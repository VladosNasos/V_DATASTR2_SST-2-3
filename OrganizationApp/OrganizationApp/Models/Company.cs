using System.Collections.Generic;

namespace OrganizationApp.Models
{
    public class Company
    {
        public Department RootDepartment { get; set; }

        public Company()
        {
            // Initialize the root department (Company)
            RootDepartment = new Department("TechCorp", "Alice Johnson");

            // Level 1 Departments
            var HR = new Department("Human Resources", "Bob Smith");
            var IT = new Department("Information Technology", "Carol Williams");
            var Finance = new Department("Finance", "David Brown");
            var Marketing = new Department("Marketing", "Eva Davis");

            RootDepartment.AddSubDepartment(HR);
            RootDepartment.AddSubDepartment(IT);
            RootDepartment.AddSubDepartment(Finance);
            RootDepartment.AddSubDepartment(Marketing);

            // Level 2 Departments
            var Recruitment = new Department("Recruitment", "Fiona Clark");
            var EmployeeRelations = new Department("Employee Relations", "George Miller");
            HR.AddSubDepartment(Recruitment);
            HR.AddSubDepartment(EmployeeRelations);

            var Development = new Department("Development", "Hannah Wilson");
            var Support = new Department("Support", "Ian Moore");
            IT.AddSubDepartment(Development);
            IT.AddSubDepartment(Support);

            var Accounts = new Department("Accounts", "Jane Taylor");
            var Auditing = new Department("Auditing", "Kevin Anderson");
            Finance.AddSubDepartment(Accounts);
            Finance.AddSubDepartment(Auditing);

            var DigitalMarketing = new Department("Digital Marketing", "Laura Thomas");
            var ContentMarketing = new Department("Content Marketing", "Mike Jackson");
            Marketing.AddSubDepartment(DigitalMarketing);
            Marketing.AddSubDepartment(ContentMarketing);

            // Level 3 Departments
            var Frontend = new Department("Frontend Team", "Nina Martin");
            var Backend = new Department("Backend Team", "Oscar Lee");
            Development.AddSubDepartment(Frontend);
            Development.AddSubDepartment(Backend);

            var ITSupport = new Department("IT Support Team", "Paul Walker");
            Support.AddSubDepartment(ITSupport);

            var Payroll = new Department("Payroll", "Quincy Hall");
            Accounts.AddSubDepartment(Payroll);

            // Level 4 Departments
            var ReactTeam = new Department("React Developers", "Rachel Young");
            Frontend.AddSubDepartment(ReactTeam);

            // Adding employees to departments
            RootDepartment.AddEmployee(new Employee("Alice Johnson"));

            HR.AddEmployee(new Employee("Bob Smith"));
            Recruitment.AddEmployee(new Employee("Fiona Clark"));
            EmployeeRelations.AddEmployee(new Employee("George Miller"));

            IT.AddEmployee(new Employee("Carol Williams"));
            Development.AddEmployee(new Employee("Hannah Wilson"));
            Frontend.AddEmployee(new Employee("Nina Martin"));
            ReactTeam.AddEmployee(new Employee("Rachel Young"));
            Backend.AddEmployee(new Employee("Oscar Lee"));
            Support.AddEmployee(new Employee("Ian Moore"));
            ITSupport.AddEmployee(new Employee("Paul Walker"));

            Finance.AddEmployee(new Employee("David Brown"));
            Accounts.AddEmployee(new Employee("Jane Taylor"));
            Payroll.AddEmployee(new Employee("Quincy Hall"));
            Auditing.AddEmployee(new Employee("Kevin Anderson"));

            Marketing.AddEmployee(new Employee("Eva Davis"));
            DigitalMarketing.AddEmployee(new Employee("Laura Thomas"));
            ContentMarketing.AddEmployee(new Employee("Mike Jackson"));
        }
    }
}
