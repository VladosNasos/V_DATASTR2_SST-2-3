using System.Windows;
using System.Windows.Controls;
using OrganizationApp.Models;

namespace OrganizationApp
{
    public partial class MoveEmployeeWindow : Window
    {
        private Company company;
        private Department currentDept;
        public Department TargetDepartment { get; private set; }

        public MoveEmployeeWindow(Company company, Department currentDept, Employee employee)
        {
            InitializeComponent();
            this.company = company;
            this.currentDept = currentDept;
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
