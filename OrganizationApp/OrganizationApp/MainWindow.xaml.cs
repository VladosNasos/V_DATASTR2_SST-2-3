using System;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using OrganizationApp.Models;
using System.IO;

namespace OrganizationApp
{
    public partial class MainWindow : Window
    {
        private Company company;
        private readonly string dataFilePath = "Data/companyData.txt";

        public MainWindow()
        {
            InitializeComponent();

            // Ensure the Data directory exists
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            // Load data if exists, else initialize
            if (File.Exists(dataFilePath))
            {
                LoadData();
            }
            else
            {
                company = new Company();
                PopulateTreeView();
            }
        }

        // Populate the TreeView with the department hierarchy
        private void PopulateTreeView()
        {
            DepartmentTreeView.Items.Clear();
            TreeViewItem rootItem = CreateTreeViewItem(company.RootDepartment);
            DepartmentTreeView.Items.Add(rootItem);
        }

        // Recursively create TreeViewItems for departments
        private TreeViewItem CreateTreeViewItem(Department dept)
        {
            TreeViewItem item = new TreeViewItem
            {
                Header = $"{dept.Name} (Manager: {dept.ManagerName})",
                Tag = dept
            };
            foreach (var subDept in dept.SubDepartments)
            {
                item.Items.Add(CreateTreeViewItem(subDept));
            }
            return item;
        }

        // When a department is selected, display its employees
        private void DepartmentTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DepartmentTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Department dept)
            {
                EmployeeListView.Items.Clear();
                foreach (var emp in dept.Employees)
                {
                    EmployeeListView.Items.Add(emp);
                }
            }
        }

        // Add Department Button Click
        private void AddDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Department parentDept)
            {
                AddDepartmentWindow addDeptWindow = new AddDepartmentWindow();
                addDeptWindow.Owner = this;
                if (addDeptWindow.ShowDialog() == true)
                {
                    string deptName = addDeptWindow.DepartmentName;
                    string managerName = addDeptWindow.ManagerName;

                    if (parentDept.FindDepartment(deptName) != null)
                    {
                        MessageBox.Show("A department with this name already exists under the selected parent.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Department newDept = new Department(deptName, managerName);
                    parentDept.AddSubDepartment(newDept);
                    selectedItem.Items.Add(CreateTreeViewItem(newDept));
                    selectedItem.IsExpanded = true;

                    SaveData();
                }
            }
            else
            {
                MessageBox.Show("Please select a parent department to add a sub-department.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Remove Department Button Click
        private void RemoveDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Department dept && dept != company.RootDepartment)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove the department '{dept.Name}' and all its sub-departments?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Department parentDept = company.RootDepartment.FindParentDepartment(dept);
                    if (parentDept != null)
                    {
                        parentDept.RemoveSubDepartment(dept.Name);
                        TreeViewItem parentItem = selectedItem.Parent as TreeViewItem;
                        if (parentItem != null)
                        {
                            parentItem.Items.Remove(selectedItem);
                        }
                        else
                        {
                            DepartmentTreeView.Items.Remove(selectedItem);
                        }
                        EmployeeListView.Items.Clear();

                        SaveData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a sub-department to remove.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Add Employee Button Click
        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Department dept)
            {
                AddEmployeeWindow addEmpWindow = new AddEmployeeWindow();
                addEmpWindow.Owner = this;
                if (addEmpWindow.ShowDialog() == true)
                {
                    string empName = addEmpWindow.EmployeeName;

                    if (dept.Employees.Any(emp => emp.Name.Equals(empName, StringComparison.OrdinalIgnoreCase)))
                    {
                        MessageBox.Show("An employee with this name already exists in the selected department.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Employee newEmp = new Employee(empName);
                    dept.AddEmployee(newEmp);
                    EmployeeListView.Items.Add(newEmp);

                    SaveData();
                }
            }
            else
            {
                MessageBox.Show("Please select a department to add an employee.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Remove Employee Button Click
        private void RemoveEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Department dept)
            {
                if (EmployeeListView.SelectedItem is Employee emp)
                {
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove employee '{emp.Name}' from department '{dept.Name}'?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        dept.RemoveEmployee(emp.Name);
                        EmployeeListView.Items.Remove(emp);

                        SaveData();
                    }
                }
                else
                {
                    MessageBox.Show("Please select an employee to remove.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a department.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Move Employee Button Click
        private void MoveEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Department currentDept)
            {
                if (EmployeeListView.SelectedItem is Employee emp)
                {
                    MoveEmployeeWindow moveEmpWindow = new MoveEmployeeWindow(company, currentDept, emp);
                    moveEmpWindow.Owner = this;
                    if (moveEmpWindow.ShowDialog() == true)
                    {
                        Department targetDept = moveEmpWindow.TargetDepartment;
                        if (targetDept != null && targetDept != currentDept)
                        {
                            currentDept.MoveEmployee(emp.Name, targetDept);
                            EmployeeListView.Items.Remove(emp);
                            MessageBox.Show($"Employee '{emp.Name}' has been moved to department '{targetDept.Name}'.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            SaveData();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select an employee to move.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a department.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Move Department Button Click
        private void MoveDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Department dept && dept != company.RootDepartment)
            {
                MoveDepartmentWindow moveDeptWindow = new MoveDepartmentWindow(company, dept);
                moveDeptWindow.Owner = this;
                if (moveDeptWindow.ShowDialog() == true)
                {
                    Department targetParent = moveDeptWindow.TargetDepartment;
                    if (targetParent != null && targetParent != dept && !dept.IsSubDepartmentOf(targetParent))
                    {
                        Department oldParent = company.RootDepartment.FindParentDepartment(dept);
                        if (oldParent != null)
                        {
                            oldParent.MoveSubDepartment(dept.Name, targetParent);
                            // Refresh TreeView
                            PopulateTreeView();
                            MessageBox.Show($"Department '{dept.Name}' has been moved under '{targetParent.Name}'.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            SaveData();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid target department selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a sub-department to move.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Total Employees Button Click
        private void TotalEmployees_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Department dept)
            {
                int total = dept.GetTotalEmployees();
                MessageBox.Show($"Total employees in '{dept.Name}' (including sub-departments): {total}", "Total Employees", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a department.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Save Data Button Click
        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            MessageBox.Show("Data has been saved successfully.", "Save Data", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Load Data Button Click
        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            PopulateTreeView();
            EmployeeListView.Items.Clear();
            MessageBox.Show("Data has been loaded successfully.", "Load Data", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Save data to a text file using JSON serialization
        private void SaveData()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonData = JsonSerializer.Serialize(company, options);
            File.WriteAllText(dataFilePath, jsonData);
        }

        // Load data from a text file using JSON deserialization
        private void LoadData()
        {
            try
            {
                string jsonData = File.ReadAllText(dataFilePath);
                company = JsonSerializer.Deserialize<Company>(jsonData);
                if (company == null)
                {
                    company = new Company();
                }
                PopulateTreeView();
                EmployeeListView.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                company = new Company();
                PopulateTreeView();
            }
        }
    }
}
