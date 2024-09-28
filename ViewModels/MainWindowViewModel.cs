using Ohtu1Project.Views;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using Ohtu1Project.Views.ReservationViews;
using Ohtu1Project.Views.ReportViews;
using Ohtu1Project.Views.InvoiceViews;

namespace Ohtu1Project.ViewModels
{
    /// <summary>
    /// Datacontext class for MainWindow. Inherits MainViewModel class.
    /// </summary>
    internal class MainWindowViewModel : MainViewModel
    {
        /// <summary>
        /// Command that is responsible for opening the NewReservationWindow.
        /// </summary>
        public ICommand NewReservationButtonCommand => new DelegateCommand(NewReservationButton);

        /// <summary>
        /// Command that is responsible for opening the ReservationsWindow.
        /// </summary>
        public ICommand ReservationsButtonCommand => new DelegateCommand(ReservationsButton);

        /// <summary>
        /// Command that is responsible for opening the OfficesWindow.
        /// </summary>
        public ICommand OfficesButtonCommand => new DelegateCommand(OfficesButton);

        /// <summary>
        /// Command that is responsible for opening the ServicesWindow.
        /// </summary>
        public ICommand ServicesButtonCommand => new DelegateCommand(ServicesButton);

        /// <summary>
        /// Command that is responsible for opening the CustomersWindow.
        /// </summary>
        public ICommand CustomersButtonCommand => new DelegateCommand(CustomersButton);

        /// <summary>
        /// Command that is responsible for opening the InvoicesWindow.
        /// </summary>
        public ICommand InvoicesButtonCommand => new DelegateCommand(InvoicesButton);

        /// <summary>
        /// Command that is responsible for opening the ReportsWindow.
        /// </summary>
        public ICommand ReportsButtonCommand => new DelegateCommand(ReportsButton);

        /// <summary>
        /// A event handler for the new reservation button.
        /// Opens the NewReservationWindow
        /// </summary>
        private void NewReservationButton()
        {
            WindowManager.OpenWindow(new AddReservationWindow());
        }

        /// <summary>
        /// A event handler for the reservations button.
        /// Opens the ReservationsWindow
        /// </summary>
        private void ReservationsButton()
        {
            WindowManager.OpenWindow(new ReservationsWindow());
        }

        /// <summary>
        /// A event handler for the offices button.
        /// Opens the OfficesWindow
        /// </summary>
        private void OfficesButton()
        {
            WindowManager.OpenWindow(new OfficeWindow());
        }

        /// <summary>
        /// A event handler for the services button.
        /// Opens the ServicesWindow
        /// </summary>
        private void ServicesButton()
        {
            WindowManager.OpenWindow(new ServiceWindow());
        }

        /// <summary>
        /// A event handler for the customers button.
        /// Opens the CustomerWindow
        /// </summary>
        private void CustomersButton()
        {
            WindowManager.OpenWindow(new CustomerWindow());
        }

        /// <summary>
        /// Event handler for the invoices button.
        /// Opens the InvoicesWindow
        /// </summary>
        private void InvoicesButton()
        {
            WindowManager.OpenWindow(new InvoicesWindow());
        }

        /// <summary>
        /// Event handler for the reports button.
        /// Opens the ReportsWindow
        /// </summary>
        private void ReportsButton()
        {
            WindowManager.OpenWindow(new ReportsWindow());
        }
    }
}