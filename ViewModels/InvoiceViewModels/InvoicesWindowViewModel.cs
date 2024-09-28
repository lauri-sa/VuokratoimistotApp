using System;
using Ohtu1Project.Views;
using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using System.Threading.Tasks;
using Ohtu1Project.Repositories;
using System.Collections.ObjectModel;
using Ohtu1Project.Views.InvoiceViews;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.InvoiceViewModels
{
    /// <summary>
    /// Datacontext class for InvoicesWindow. Inherits MainViewModel class.
    /// </summary>
    internal class InvoicesWindowViewModel : MainViewModel
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
                EnableViewButton = _selectedIndex < 0 ? false : true;
                OnPropertyChanged();
            }
        }

        private bool _enableSearchButton;
        public bool EnableSearchButton { get { return _enableSearchButton; } set { _enableSearchButton = value; OnPropertyChanged(); } }

        private bool _enableviewButton;
        public bool EnableViewButton { get { return _enableviewButton; } set { _enableviewButton = value; OnPropertyChanged(); } }

        private bool _enableDatePicker;
        public bool EnableDatePicker { get { return _enableDatePicker; } set { _enableDatePicker = value; OnPropertyChanged(); } }

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
                OnPropertyChanged();
            }
        }

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
                InvoicesCollection?.Clear();
                OnPropertyChanged();
            }
        }

        private ViewInvoiceModel _viewInvoiceModel;
        public ViewInvoiceModel ViewInvoiceModel { get { return _viewInvoiceModel; } set { _viewInvoiceModel = value; OnPropertyChanged(); } }

        private ObservableCollection<CustomerModel> _customersCollection;
        public ObservableCollection<CustomerModel> CustomersCollection { get { return _customersCollection; } set { _customersCollection = value; OnPropertyChanged(); } }

        private ObservableCollection<ViewInvoiceModel> _invoicesCollection;
        public ObservableCollection<ViewInvoiceModel> InvoicesCollection { get { return _invoicesCollection; } set { _invoicesCollection = value; OnPropertyChanged(); } }

        /// <summary>
        /// Command that is responsible for ContentRendered event.
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand(Loaded);

        /// <summary>
        /// Command that is responsible for lose focus from datagrid.
        /// </summary>
        public ICommand LostFocusCommand => new DelegateCommand(LostFocus);

        /// <summary>
        /// A command for the search button.
        /// </summary>
        public ICommand SearchButtonCommand => new DelegateCommand(SearchButton);

        /// <summary>
        /// A command for the view button.
        /// </summary>
        public ICommand ViewButtonCommand => new DelegateCommand(ViewButton);

        /// <summary>
        /// A command for the return button.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);

        public InvoicesWindowViewModel()
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
                EnableDatePicker = CustomersCollection.Count > 0 ? true : false;
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
        /// Asynchronously fetches and populates the collection of invoices for the current customer within the specified date range.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetInvoices()
        {
            try
            {
                InvoicesCollection = await InvoiceRepository.FetchInvoices(CustomerModel.ID, StartDate, EndDate);
                await InvoiceRepository.FetchInvoiceRows(InvoicesCollection);
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.AsyncRetryMethod = async () => await GetInvoices();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Event handler for the search button.
        /// </summary>
        private async void SearchButton()
        {
            await GetInvoices();
        }

        /// <summary>
        /// Handles the View button click event. Opens a new window to display details of the selected invoice.
        /// </summary>
        private void ViewButton()
        {
            ViewInvoiceWindowViewModel.ViewInvoiceModel = ViewInvoiceModel;
            WindowManager.OpenWindow(new ViewInvoiceWindow());
            LostFocus();
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