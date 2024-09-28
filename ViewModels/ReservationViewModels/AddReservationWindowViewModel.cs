using System;
using System.Linq;
using System.Windows;
using Ohtu1Project.Views;
using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using System.Threading.Tasks;
using System.Windows.Controls;
using Ohtu1Project.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ohtu1Project.Views.ReservationViews;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.ReservationViewModels
{
    /// <summary>
    /// Datacontext class for AddReservationWindow. Inherits MainViewModel class.
    /// </summary>
    internal class AddReservationWindowViewModel : MainViewModel
    {
        private List<DateTime> blackoutDatesList;

        public DateTime StartDateDisplayDateStart { get; set; }

        private DateTime _endDateDisplayDateStart;
        public DateTime EndDateDisplayDateStart { get { return _endDateDisplayDateStart; } set { _endDateDisplayDateStart = value; OnPropertyChanged(); } }

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
                
                if (OfficeModel != null)
                {
                    EndDate = CheckBlackoutDate() ? StartDate : StartDate.AddDays(1);
                }

                EndDateDisplayDateStart = EndDate;
                OnPropertyChanged();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate { get { return _endDate; } set { _endDate = value; OnPropertyChanged(); } }

        private int _officeSpaceIndex;
        public int OfficeSpaceIndex { get { return _officeSpaceIndex; } set { _officeSpaceIndex = value; OnPropertyChanged(); } }

        private bool _enableButton;
        public bool EnableButton { get { return _enableButton; } set { _enableButton = value; OnPropertyChanged(); } }

        private bool _enableDatePicker;
        public bool EnableDatePicker { get { return _enableDatePicker; } set { _enableDatePicker = value; OnPropertyChanged(); } }

        private bool _enableOfficesComboBox;
        public bool EnableOfficesComboBox
        {
            get
            {
                return _enableOfficesComboBox;
            }
            set
            {
                _enableOfficesComboBox = value;
                OfficesError = EnableOfficesComboBox == true ? string.Empty : "Toimipisteitä ei löydy";
                OnPropertyChanged();
            }
        }

        private bool _enableOfficeSpacesComboBox;
        public bool EnableOfficeSpacesComboBox
        {
            get
            {
                return _enableOfficeSpacesComboBox;
            }
            set
            {
                _enableOfficeSpacesComboBox = value;
                OfficeSpacesError = EnableOfficeSpacesComboBox == true ? string.Empty : "Toimitiloja ei löydy";
                OnPropertyChanged();
            }
        }

        private bool _enableCustomersComboBox;
        public bool EnableCustomersComboBox
        {
            get
            {
                return _enableCustomersComboBox;
            }
            set
            {
                _enableCustomersComboBox = value;
                CustomersError = EnableCustomersComboBox == true ? string.Empty : "Asiakkaita ei löydy";
                OnPropertyChanged();
            }
        }

        private bool _enableServicesComboBox;
        public bool EnableServicesComboBox
        {
            get
            {
                return _enableServicesComboBox;
            }
            set
            {
                _enableServicesComboBox = value;
                ServicesError = EnableServicesComboBox == true ? string.Empty : "Ei valittavia lisäpalveluita";
                OnPropertyChanged();
            }
        }

        private string _officesError;
        public string OfficesError { get { return _officesError; } set { _officesError = value; OnPropertyChanged(); } }

        private string _officeSpacesError;
        public string OfficeSpacesError { get { return _officeSpacesError; } set { _officeSpacesError = value; OnPropertyChanged(); } }

        private string _customersError;
        public string CustomersError { get { return _customersError; } set { _customersError = value; OnPropertyChanged(); } }

        private string _servicesError;
        public string ServicesError { get { return _servicesError; } set { _servicesError = value; OnPropertyChanged(); } }

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
                EnableButton = OfficeSpaceModel == null ? false : true;
                OnPropertyChanged();
            }
        }

        private OfficeModel _officeModel;
        public OfficeModel OfficeModel
        {
            get
            {
                return _officeModel;
            }
            set
            {
                _officeModel = value;
                SelectedServicesCollection.Clear();
                if(OfficeModel != null)
                {
                    _ = GetOfficeSpaces();
                    ServicesCollection?.Clear();
                }
                OnPropertyChanged();
            }
        }

        private OfficeSpaceModel _officeSpaceModel;
        public OfficeSpaceModel OfficeSpaceModel
        {
            get
            {
                return _officeSpaceModel;
            }
            set
            {
                _officeSpaceModel = value;
                SelectedServicesCollection.Clear();
                if (OfficeSpaceModel != null)
                {
                    EnableButton = CustomerModel == null ? false : true;
                    SetBlackOutDates();
                    _ = GetServices();
                }
                else
                {
                    EnableButton = false;
                }
                OnPropertyChanged();
            }
        }

        private ServiceModel _serviceModel;
        public ServiceModel ServiceModel
        {
            get
            {
                return _serviceModel;
            }
            set
            {
                _serviceModel = value;

                if (ServiceModel != null)
                {
                    var model = new ServiceModel
                    {
                        ID = ServiceModel.ID,
                        Name = ServiceModel.Name
                    };

                    if (!SelectedServicesCollection.Any(x => x.ID == model.ID))
                    {
                        SelectedServicesCollection.Add(new ServiceModel
                        {
                            ID = ServiceModel.ID,
                            Name = ServiceModel.Name,
                            Price = ServiceModel.Price
                        });
                    }
                }

                OnPropertyChanged();
            }
        }

        private ServiceModel _selectedServiceModel;
        public ServiceModel SelectedServiceModel
        {
            get
            {
                return _selectedServiceModel;
            }
            set
            {
                _selectedServiceModel = value;
                SelectedServicesCollection.Remove(SelectedServiceModel);
                this.ServiceModel = null;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CustomerModel> _customersCollection;
        public ObservableCollection<CustomerModel> CustomersCollection { get { return _customersCollection; } set { _customersCollection = value; OnPropertyChanged(); } }

        private ObservableCollection<OfficeModel> _officesCollection;
        public ObservableCollection<OfficeModel> OfficesCollection { get { return _officesCollection; } set { _officesCollection = value; OnPropertyChanged(); } }

        private ObservableCollection<OfficeSpaceModel> _officeSpacesCollection;
        public ObservableCollection<OfficeSpaceModel> OfficeSpacesCollection { get { return _officeSpacesCollection; } set { _officeSpacesCollection = value; OnPropertyChanged(); } }

        private ObservableCollection<ServiceModel> _servicesCollection;
        public ObservableCollection<ServiceModel> ServicesCollection { get { return _servicesCollection; } set { _servicesCollection = value; OnPropertyChanged(); } }

        private ObservableCollection<ServiceModel> _selectedServicesCollection = new();
        public ObservableCollection<ServiceModel> SelectedServicesCollection { get { return _selectedServicesCollection; } set { _selectedServicesCollection = value; OnPropertyChanged(); } }

        public AddReservationWindowViewModel()
        {
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(1);
            StartDateDisplayDateStart = DateTime.Today;
        }

        /// <summary>
        /// Command that is responsible for ContentRendered event.
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand(Loaded);

        /// <summary>
        /// A command for the add button.
        /// </summary>
        public ICommand AddButtonCommand => new DelegateCommand(AddButton);

        /// <summary>
        /// A command for the return button.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);

        /// <summary>
        /// Calls GetCustomersAndOffices() method asynchronously.
        /// </summary>
        private async void Loaded()
        {
            await GetCustomersAndOffices();
        }

        /// <summary>
        ///  Sets the blackout dates for the start and end date pickers based on the existing reservations for the currently selected office space.
        /// </summary>
        private void SetBlackOutDates()
        {
            DateTime startDate = DateTime.Today;

            var window = App.Current.Windows.OfType<Window>().FirstOrDefault(window => window is AddReservationWindow) as AddReservationWindow;
            
            var startDatePicker = window.FindName("StartDatePicker") as DatePicker;
            
            var endDatePicker = window.FindName("EndDatePicker") as DatePicker;

            blackoutDatesList = ReservationRepository.FetchReservationDates(OfficeSpaceModel.ID);

            startDatePicker.BlackoutDates.Clear();

            endDatePicker.BlackoutDates.Clear();

            if (blackoutDatesList.Contains(startDate))
            {
                do
                {
                    startDate = startDate.AddDays(1);
                } while (blackoutDatesList.Contains(startDate));
            }

            StartDate = startDate;

            foreach (var reservation in blackoutDatesList)
            {
                startDatePicker.BlackoutDates.Add(new CalendarDateRange(reservation));
                endDatePicker.BlackoutDates.Add(new CalendarDateRange(reservation));
            }
        }

        /// <summary>
        /// Checks if the date that follows the StartDate is in the blackoutDatesList.
        /// </summary>
        /// <returns>Returns true if the date that follows the StartDate is in the blackoutDatesList; otherwise, returns false.</returns>
        private bool CheckBlackoutDate()
        {
            return blackoutDatesList.Contains(StartDate.AddDays(1));
        }

        /// <summary>
        /// Asynchronously fetches all customers and offices from the database via CustomerRepository and OfficeRepository
        /// and assigns the resulting collections to the CustomersCollection OfficesCollection properties.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetCustomersAndOffices()
        {
            try
            {
                CustomersCollection = await CustomerRepository.FetchAllCustomersMinorDetails();
                OfficesCollection = await OfficeRepository.FetchAllOfficesMinorDetails();
                EnableCustomersComboBox = CustomersCollection.Count > 0 ? true : false;
                EnableOfficesComboBox = OfficesCollection.Count > 0 ? true : false;
                EnableOfficeSpacesComboBox = OfficesCollection.Count > 0 ? true : false;
                EnableServicesComboBox = OfficesCollection.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.AsyncRetryMethod = async () => await GetCustomersAndOffices();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Asynchronously fetches all office spaces for specific office from the database via OfficeSpaceRepository
        /// and assigns the resulting collection to the OfficeSpacesCollection property.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetOfficeSpaces()
        {
            try
            {
                OfficeSpacesCollection = await OfficeSpaceRepository.FetchAllOfficeSpacesMinorDetail(OfficeModel.ID);
                EnableOfficeSpacesComboBox = OfficeSpacesCollection.Count > 0 ? true : false;
                EnableServicesComboBox = OfficeSpacesCollection.Count > 0 ? true : false;
                EnableDatePicker = OfficeSpacesCollection.Count > 0 ? true : false;
                OfficeSpaceIndex = 0;
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.AsyncRetryMethod = async () => await GetOfficeSpaces();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Asynchronously fetches all services for specific office space from the database via ServiceRepository
        /// and assigns the resulting collection to the ServicesCollection property.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetServices()
        {
            try
            {
                ServicesCollection = await ServiceRepository.FetchAllServicesMinorDetails(OfficeSpaceModel.ID);
                EnableServicesComboBox = ServicesCollection.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.AsyncRetryMethod = async () => await GetServices();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Initializes a new instance of the ReservationModel class with the data currently stored in the view model.
        /// Calculates the price of each selected service based on the length of the reservation period.
        /// </summary>
        /// <returns>A new instance of the ReservationModel class.</returns>
        private ReservationModel InitReservationModel()
        {
            var reservationModel = new ReservationModel
            {
                CustomerID = CustomerModel.ID,
                SpaceID = OfficeSpaceModel.ID,
                StartDate = StartDate,
                EndDate = EndDate,
                ReservationDay = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                ReservationServices = SelectedServicesCollection
            };

            var amount = (int)reservationModel.EndDate.Date.Subtract(reservationModel.StartDate.Date).TotalDays;

            amount = amount < 1 ? 1 : amount;

            reservationModel.OfficeSpacePrice = (float.Parse(OfficeSpaceModel.Price) * amount);

            foreach (var service in reservationModel.ReservationServices)
            {
                service.Price = (float.Parse(service.Price) * amount).ToString();
            }

            return reservationModel;
        }

        /// <summary>
        /// Initializes an InvoiceModel instance based on the provided ReservationModel instance.
        /// </summary>
        /// <param name="reservationModel">The ReservationModel instance to base the invoice on.</param>
        /// <returns>A new instance of the InvoiceModel class.</returns>
        private InvoiceModel InitInvoiceModel(ReservationModel reservationModel)
        {
            var invoiceModel = new InvoiceModel
            {
                CustomerID = CustomerModel.ID,
                Date = reservationModel.EndDate.AddDays(1),
                DueDate = reservationModel.EndDate.AddDays(31),
                TotalSum = reservationModel.OfficeSpacePrice
            };

            foreach (var service in reservationModel.ReservationServices)
            {
                invoiceModel.TotalSum += float.Parse(service.Price);
            }

            return invoiceModel;
        }

        /// <summary>
        /// Adds a reservation and its corresponding invoice to the database.
        /// If an exception occurs, an error window will be opened´and the operation can be retried.
        /// </summary>
        public void AddReservationToDatabase()
        {
            try
            {
                var reservationModel = InitReservationModel();
                var invoiceModel = InitInvoiceModel(reservationModel);
                ReservationRepository.AddReservation(reservationModel, invoiceModel);
                WindowManager.CloseWindow();
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.RetryMethod = () => AddReservationToDatabase();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        ///  Handles the click event of the "Add" button.
        /// </summary>
        private void AddButton()
        {
            if(CustomerModel != null)
            {
                AddReservationToDatabase();
            }
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