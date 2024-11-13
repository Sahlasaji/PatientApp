using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp
{
    public class PatientViewModel : IPatient
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<Patient> OnPatientRegistered;
        public event EventHandler<Patient> OnAppointmentConfirmed;

        public event EventHandler<string> PatientRegistered;

        public ObservableCollection<Patient> Patients { get; private set; }
        public ObservableCollection<Patient> ConfirmedPatients { get; private set; }

        private string _registrationMessage;
        public string RegistrationMessage
        {
            get => _registrationMessage;
            set
            {
                _registrationMessage = value;
                OnPropertyChanged(nameof(RegistrationMessage));
            }

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PatientViewModel()
        {
            Patients = new ObservableCollection<Patient>();
            ConfirmedPatients = new ObservableCollection<Patient>();
        }

        public void RegisterPatient(Patient patient)
        {
            Patients.Add(patient);
            
            // Raise an event for notification
            RegistrationMessage = $"Patient {patient.Name} Registered";
            OnPatientRegistered?.Invoke(this, patient);
            PatientRegistered?.Invoke(this, "Registration Completed");
        }

        public void ConfirmPatients(List<Patient> selectedPatients)
        {
            //ConfirmedPatients.Clear();
            foreach (var patient in selectedPatients)
            {
                ConfirmedPatients.Add(patient);
            }
            OnAppointmentConfirmed?.Invoke(this,null);
        }
    }
}
