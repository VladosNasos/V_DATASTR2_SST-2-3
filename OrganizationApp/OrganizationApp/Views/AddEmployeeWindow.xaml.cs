using System.Windows;

namespace OrganizationApp
{
    public partial class AddEmployeeWindow : Window
    {
        public string EmployeeName { get; private set; }

        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmpNameTextBox.Text))
            {
                MessageBox.Show("Please enter the Employee Name.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            EmployeeName = EmpNameTextBox.Text.Trim();
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
