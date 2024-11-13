using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PatientApp
{
    /// <summary>
    /// Interaction logic for PatientRegControl.xaml
    /// </summary>
    public partial class PatientRegControl : UserControl
    {
        private PatientViewModel _viewModel;
        public event EventHandler NavigateToAppointment;
        public event EventHandler RegistrationCompleted;

        public PatientRegControl(PatientViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;

            this.Unloaded += (s, e)=> UnsubscribeEvents();
            
        }

        private void UnsubscribeEvents()
        {
            NavigateToAppointment = null;
            RegistrationCompleted = null;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Input validation
            string name = NameTextBox.Text.Trim();
            string address = AddressTextBox.Text.Trim();
            DateTime? dateOfBirth = DOBPicker.SelectedDate;
            string slot = (SlotComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            
            DateTime? bookingDate = BookingDatePicker.SelectedDate;

            // Validate Name
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a valid name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate Age
            if (!int.TryParse(AgeTextBox.Text, out int age) || age <= 0)
            {
                MessageBox.Show("Please enter a valid age (positive integer).", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate Date of Birth
            if (!dateOfBirth.HasValue)
            {
                MessageBox.Show("Please select a valid date of birth.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate Slot
            if (string.IsNullOrWhiteSpace(slot))
            {
                MessageBox.Show("Please select a time slot.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate Address
            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please enter a valid address.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create Patient object
            var patient = new Patient
            {
                Name = name,
                Age = age,
                BookingDate = DOBPicker.SelectedDate.HasValue ? DOBPicker.SelectedDate.Value.ToString("yyyy-MM-dd") :DateTime.Now.ToString("yyyy-MM-dd"),
                Address = address,
                Slot = slot,
                DateOfBirth = BookingDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd")
            };

            try
            {
                // Register the patient
                _viewModel.RegisterPatient(patient);
                //MessageBox.Show("Patient details entered successfully.", "Registration Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RegistrationCompleted?.Invoke(this,EventArgs.Empty);

                // Trigger navigation to appointment confirmation
                NavigateToAppointment?.Invoke(this,EventArgs.Empty);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during registration
                MessageBox.Show($"An error occurred while registering the patient: {ex.Message}", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}