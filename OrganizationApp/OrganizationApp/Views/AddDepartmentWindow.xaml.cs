using System.Windows;

namespace OrganizationApp
{
    public partial class AddDepartmentWindow : Window
    {
        public string DepartmentName { get; private set; }
        public string ManagerName { get; private set; }

        public AddDepartmentWindow()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DeptNameTextBox.Text) || string.IsNullOrWhiteSpace(ManagerNameTextBox.Text))
            {
                MessageBox.Show("Please enter both Department Name and Manager Name.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DepartmentName = DeptNameTextBox.Text.Trim();
            ManagerName = ManagerNameTextBox.Text.Trim();
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
