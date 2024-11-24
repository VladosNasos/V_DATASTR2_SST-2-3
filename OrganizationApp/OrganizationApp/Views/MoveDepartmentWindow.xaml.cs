using System.Windows;
using System.Windows.Controls;
using OrganizationApp.Models;

namespace OrganizationApp
{
    public partial class MoveDepartmentWindow : Window
    {
        private Company company;
        private Department departmentToMove;
        public Department TargetDepartment { get; private set; }

        public MoveDepartmentWindow(Company company, Department deptToMove)
        {
            InitializeComponent();
            this.company = company;
            this.departmentToMove = deptToMove;
            PopulateTreeView();
        }

        private void PopulateTreeView()
        {
            TargetDeptTreeView.Items.Clear();
            TreeViewItem rootItem = CreateTreeViewItem(company.RootDepartment);
            TargetDeptTreeView.Items.Add(rootItem);
        }

        private TreeViewItem CreateTreeViewItem(Department dept)
        {
            // Prevent moving a department under itself or its sub-departments
            if (dept == departmentToMove || departmentToMove.IsSubDepartmentOf(dept))
                return null;

            TreeViewItem item = new TreeViewItem
            {
                Header = $"{dept.Name} (Manager: {dept.ManagerName})",
                Tag = dept
            };
            foreach (var subDept in dept.SubDepartments)
            {
                var childItem = CreateTreeViewItem(subDept);
                if (childItem != null)
                    item.Items.Add(childItem);
            }
            return item;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (TargetDeptTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Department dept)
            {
                TargetDepartment = dept;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a target department.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
