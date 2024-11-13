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
    public partial class PatientDashboardControl : UserControl
    {
        private PatientViewModel _viewModel;
        public event EventHandler DashboardCompleted;

        public PatientDashboardControl(PatientViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.PatientRegistered += OnPatientRegistered;
            grdPatients.ItemsSource = _viewModel.ConfirmedPatients;
            DashboardCompleted?.Invoke(this, EventArgs.Empty);
            this.Unloaded += (s, e) => UnsubscribeEvents();

        }

        private void UnsubscribeEvents()
        {
            DashboardCompleted=null;
        }

        private void OnPatientRegistered(object sender, string message)
        {
            Dispatcher.Invoke(() =>
            {
                RegistrationStatusTextBox.Text = message;
            });

        }


    }
}