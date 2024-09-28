using System;
using Ohtu1Project.Views;
using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using System.Threading.Tasks;
using Ohtu1Project.Repositories;
using System.Collections.ObjectModel;
using Ohtu1Project.Views.ReservationViews;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.ReservationViewModels
{
    /// <summary>
    /// Datacontext class for ReservationsWindow. Inherits MainViewModel class.
    /// </summary>
    internal class ReservationsWindowViewModel : MainViewModel
    {
        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                EnableButtons = _selectedIndex < 0 ? false : true;
                OnPropertyChanged();
            }
        }

        private bool _enableButtons;
        public bool EnableButtons { get { return _enableButtons; } set { _enableButtons = value; OnPropertyChanged(); } }

        private bool _enableSearchButton;
        public bool EnableSearchButton { get { return _enableSearchButton; } set { _enableSearchButton = value; OnPropertyChanged(); } }

        private bool _enableCustomersComboBox;
        public bool EnableCustomersComboBox { get { return _enableCustomersComboBox; } set { _enableCustomersComboBox = value; OnPropertyChanged(); } }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                EndDate = StartDate.AddDays(1);
                EndDateDisplayDateStart = EndDate;
                OnPropertyChanged();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate { get { return _endDate; } set { _endDate = value; OnPropertyChanged(); } }

        public DateTime StartDateDisplayDateStart { get; set; }

        private DateTime _endDateDisplayDateStart;
        public DateTime EndDateDisplayDateStart { get { return _endDateDisplayDateStart; } set { _endDateDisplayDateStart = value; OnPropertyChanged(); } }

        private CustomerModel _customerModel;
        public CustomerModel CustomerModel
        {
            get
            {
                return _customerModel;
            }
            set
            {
                _customerModel = value;
                ReservationsCollection?.Clear();
                OnPropertyChanged();
            }
        }

        private ReservationModel _reservationModel;
        public ReservationModel ReservationModel { get { return _reservationModel; } set { _reservationModel = value; OnPropertyChanged(); } }

        private ObservableCollection<CustomerModel> _customersCollection;
        public ObservableCollection<CustomerModel> CustomersCollection { get { return _customersCollection; } set { _customersCollection = value; OnPropertyChanged(); } }

        private ObservableCollection<ReservationModel> _reservationsCollection;
        public ObservableCollection<ReservationModel> ReservationsCollection { get { return _reservationsCollection; } set { _reservationsCollection = value; OnPropertyChanged(); } }

        /// <summary>
        /// Command that is responsible for ContentRendered event.
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand(Loaded);

        /// <summary>
        /// Command that is responsible for lose focus from datagrid.
        /// </summary>
        public ICommand LostFocusCommand => new DelegateCommand(LostFocus);

        /// <summary>
        /// A command for the modify button.
        /// </summary>
        public ICommand SearchButtonCommand => new DelegateCommand(SearchButton);

        /// <summary>
        /// A command for the modify button.
        /// </summary>
        public ICommand ModifyButtonCommand => new DelegateCommand(ModifyButton);

        /// <summary>
        /// A command for the delete button.
        /// </summary>
        public ICommand DeleteButtonCommand => new DelegateCommand(DeleteButton);

        /// <summary>
        /// A command for the return button.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);

        public ReservationsWindowViewModel()
        {
            StartDate = DateTime.Today;
            StartDateDisplayDateStart = DateTime.Today;
        }

        /// <summary>
        /// Calls GetCustomers() method asynchronously.
        /// </summary>
        private async void Loaded()
        {
            await GetCustomers();
        }

        /// <summary>
        /// Sets SelectedIndex property value to -1.
        /// </summary>
        private void LostFocus()
        {
            SelectedIndex = -1;
        }

        /// <summary>
        /// Asynchronously fetches all customers from the database via CustomerRepository
        /// and assigns the resulting collection to the CustomersCollection property.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetCustomers()
        {
            try
            {
                CustomersCollection = await CustomerRepository.FetchAllCustomersMinorDetails();
                EnableCustomersComboBox = CustomersCollection.Count > 0 ? true : false;
                EnableSearchButton = CustomersCollection.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.AsyncRetryMethod = async () => await GetCustomers();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Asynchronously fetches reservations from the ReservationRepository for the selected customer within the specified date range,
        /// and populates the ReservationsCollection property with the results.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task GetReservations()
        {
            try
            {
                ReservationsCollection = await ReservationRepository.FetchReservations(CustomerModel.ID, StartDate, EndDate);
                await ReservationRepository.FetchReservationServices(ReservationsCollection);
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.AsyncRetryMethod = async () => await GetReservations();
            }
        }

        /// <summary>
        /// Event handler for the search button. Calls GetReservations() method asynchronously.
        /// </summary>
        private async void SearchButton()
        {
            await GetReservations();
        }

        /// <summary>
        /// Event handler for the modify button. Opens the UpdateReservationWindow with the current ReservationModel data
        /// and refreshes the ReservationsCollection by calling GetReservations() method asynchronously.
        /// </summary>
        private async void ModifyButton()
        {
            UpdateReservationWindowViewModel.ReservationModel = ReservationModel;
            WindowManager.OpenWindow(new UpdateReservationWindow());
            await GetReservations();
        }

        /// <summary>
        /// Event handler for the delete button. Opens a delete confirmation window and
        /// sets the delete action in DeleteConfirmationWindowViewModel
        /// to DeleteReservation() method in ReservationRepository class.
        /// </summary>
        private async void DeleteButton()
        {
            DeleteConfirmationWindowViewModel.DeleteAction = () => ReservationRepository.DeleteReservation(ReservationModel.ID);
            WindowManager.OpenWindow(new DeleteConfirmationWindow());
            await GetReservations();
        }

        /// <summary>
        /// Event handler for the return button. Closes the window.
        /// </summary>
        private void ReturnButton()
        {
            WindowManager.CloseWindow();
        }
    }
}