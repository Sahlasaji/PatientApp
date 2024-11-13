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
    public partial class AppointmentConfirmationControl : UserControl
    {
        private PatientViewModel _viewModel;
        public event EventHandler NavigateToDashboard;
        public event EventHandler AppointmentCompleted;

        public AppointmentConfirmationControl(PatientViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            //_viewModel.PatientRegistered += OnPatientRegistered; 
            LoadPatients();
            this.Unloaded += (s, e) => UnsubscribeEvents();
           
        }

        private void UnsubscribeEvents()
        {
            NavigateToDashboard=null;
            AppointmentCompleted=null;
        }

        //private void OnPatientRegistered(object sender, string message)
        //{
        //    Dispatcher.Invoke(() =>
        //    {
        //        RegistrationTextBox.Text = message;
        //    });

        //}

        private void LoadPatients()
        {
            foreach (var patient in _viewModel.Patients)
            {
                var checkBox = new CheckBox
                {
                    Content = $"{patient.Name} (Age: {patient.Age}, DOB: {patient.DateOfBirth}, Address: {patient.Address}, BookingDate:{patient.BookingDate})",
                    Tag = patient
                };
                PatientsListBox.Items.Add(checkBox);
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var selectedPatients = new List<Patient>();
            for (int i = PatientsListBox.Items.Count - 1; i >= 0; i--)
            {
                CheckBox checkBox = PatientsListBox.Items[i] as CheckBox;
                if (checkBox != null && checkBox.IsChecked == true && checkBox.Tag is Patient patient)
                {
                    selectedPatients.Add(patient);
                    PatientsListBox.Items.RemoveAt(i);
                }
            }

            foreach (var confirmedPatient in selectedPatients)
            {
                _viewModel.Patients.Remove(confirmedPatient);
            }

            _viewModel.ConfirmPatients(selectedPatients);
            //MessageBox.Show("Appointment confirmed successfully.");
            AppointmentCompleted?.Invoke(this,EventArgs.Empty);
            NavigateToDashboard?.Invoke(this,EventArgs.Empty);
        }
    }
}