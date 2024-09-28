using System;
using Ohtu1Project.Views;
using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using System.Threading.Tasks;
using Ohtu1Project.Repositories;
using System.Collections.ObjectModel;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.ServiceViewModels
{
    /// <summary>
    /// Datacontext class for ServiceWindow. Inherits MainViewModel class.
    /// </summary>
    internal class ServiceWindowViewModel : MainViewModel
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

        private bool _enableAddButton;
        public bool EnableAddButton { get { return _enableAddButton; } set { _enableAddButton = value; OnPropertyChanged(); } }

        private bool _enableOfficesComboBox;
        public bool EnableOfficesComboBox { get { return _enableOfficesComboBox; } set { _enableOfficesComboBox = value; OnPropertyChanged(); } }

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
                _=GetServices();
                OnPropertyChanged();
            }
        }

        private ServiceModel _serviceModel;
        public ServiceModel ServiceModel { get { return _serviceModel; } set { _serviceModel = value; OnPropertyChanged(); } }

        private ObservableCollection<OfficeModel> _officesCollection;
        public ObservableCollection<OfficeModel> OfficesCollection { get { return _officesCollection; } set { _officesCollection = value; OnPropertyChanged(); } }

        private ObservableCollection<ServiceModel> _servicesCollection;
        public ObservableCollection<ServiceModel> ServicesCollection { get { return _servicesCollection; } set { _servicesCollection = value; OnPropertyChanged(); } }

        /// <summary>
        /// Command that is responsible for ContentRendered event.
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand(Loaded);

        /// <summary>
        /// Command that is responsible for lose focus from datagrid.
        /// </summary>
        public ICommand LostFocusCommand => new DelegateCommand(LostFocus);

        /// <summary>
        /// A command for the add button.
        /// </summary>
        public ICommand AddButtonCommand => new DelegateCommand(AddButton);

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

        /// <summary>
        /// Calls GetCustomers() method asynchronously.
        /// </summary>
        private async void Loaded()
        {
            await GetOffices();
        }

        /// <summary>
        /// Asynchronously fetches all offices from the database via OfficeRepository
        /// and assigns the resulting collection to the OfficesCollection property.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetOffices()
        {
            try
            {
                OfficesCollection = await OfficeRepository.FetchAllOfficesMinorDetails();
                EnableOfficesComboBox = OfficesCollection.Count > 0 ? true : false;
                EnableAddButton = OfficesCollection.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.AsyncRetryMethod = async () => await GetOffices();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Asynchronously fetches all services for specific office from the database via ServiceRepository
        /// and assigns the resulting collection to the ServicesCollection property.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetServices()
        {
            try
            {
                ServicesCollection = await ServiceRepository.FetchAllOfficeServices(OfficeModel.ID);
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
        /// Sets SelectedIndex property value to -1.
        /// </summary>
        private void LostFocus()
        {
            SelectedIndex = -1;
        }

        /// <summary>
        /// Event handler for the add button. Opens the AddServiceWindow
        /// and refreshes the ServicesCollection by calling GetServices method asynchronously.
        /// </summary>
        private async void AddButton()
        {
            if (OfficeModel != null)
            {
                AddServiceWindowViewModel.OfficeModel = OfficeModel;
                WindowManager.OpenWindow(new AddServiceWindow());
                await GetServices();
                LostFocus();
            }
        }

        /// <summary>
        /// Event handler for the modify button. Opens the UpdateServiceWindow with the current ServiceModel data
        /// and refreshes the ServicesCollection by calling GetServices() method asynchronously.
        /// </summary>
        private async void ModifyButton()
        {
            UpdateServiceWindowViewModel.OfficeName = OfficeModel.OfficeName;
            UpdateServiceWindowViewModel.ServiceModel = ServiceModel;
            WindowManager.OpenWindow(new UpdateServiceWindow());
            await GetServices();
            LostFocus();
        }

        /// <summary>
        /// Event handler for the delete button. Opens a delete confirmation window,
        /// sets the delete action in DeleteConfirmationWindowViewModel
        /// to DeleteService() method in ServiceRepository class and refreshes the ServicesCollection
        /// by calling GetServices() method asynchronously.
        /// </summary>
        private async void DeleteButton()
        {
            DeleteConfirmationWindowViewModel.DeleteAction = () => ServiceRepository.DeleteService(ServiceModel.ID);
            WindowManager.OpenWindow(new DeleteConfirmationWindow());
            await GetServices();
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