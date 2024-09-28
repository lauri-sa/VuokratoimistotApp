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
    /// Datacontext class for AddServiceWindow. Inherits MainViewModel class.
    /// </summary>
    internal class AddServiceWindowViewModel : MainViewModel
    {
        public string OfficeName { get; set; }

        public static OfficeModel OfficeModel { get; set; }

        private int _id;
        public int ID { get { return _id; } set { _id = value; OnPropertyChanged(); } }

        private string _nameError;
        public string NameError { get { return _nameError; } set { _nameError = value; OnPropertyChanged(); } }

        private string _priceError;
        public string PriceError { get { return _priceError; } set { _priceError = value; OnPropertyChanged(); } }

        private bool _enableAddButton;
        public bool EnableAddButton { get { return _enableAddButton; } set { _enableAddButton = value; OnPropertyChanged(); } }

        private bool _enableOfficeSpacesComboBox;
        public bool EnableOfficeSpacesComboBox { get { return _enableOfficeSpacesComboBox; } set { _enableOfficeSpacesComboBox = value; OnPropertyChanged(); } }

        private ServiceModel _serviceModel = new();
        public ServiceModel ServiceModel { get { return  _serviceModel; } set { _serviceModel = value; OnPropertyChanged(); } }

        private ObservableCollection<OfficeSpaceModel> _officeSpaces;
        public ObservableCollection<OfficeSpaceModel> OfficeSpaces { get { return _officeSpaces; } set { _officeSpaces = value; OnPropertyChanged(); } }

        public AddServiceWindowViewModel()
        {
            OfficeName = OfficeModel.OfficeName;
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
        /// Calls GetCustomers() method asynchronously.
        /// </summary>
        private async void Loaded()
        {
            await GetOfficeSpaces();
        }

        /// <summary>
        /// Fetches all the office spaces associated with the currently selected office asynchronously.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetOfficeSpaces()
        {
            try
            {
                OfficeSpaces = await OfficeSpaceRepository.FetchAllOfficeSpacesMinorDetail(OfficeModel.ID);
                EnableOfficeSpacesComboBox = OfficeSpaces.Count > 0 ? true : false;
                EnableAddButton = OfficeSpaces.Count > 0 ? true : false;
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
        /// Adds new service to the database via the ServiceRepository class and closes the current window if successful.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's retry method to itself.
        /// </summary>
        public void AddServiceToDatabase()
        {
            try
            {
                ServiceRepository.AddService(ID, ServiceModel);
                WindowManager.CloseWindow();
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.RetryMethod = () => AddServiceToDatabase();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Validates the inputs for adding a service and sets error messages for any invalid input fields.
        /// </summary>
        /// <returns>True if all input fields are valid, otherwise false.</returns>
        private bool InputValidation()
        {
            bool validInput = true;

            NameError = string.Empty;
            PriceError = string.Empty;

            if (string.IsNullOrWhiteSpace(ServiceModel.Name))
            {
                NameError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(ServiceModel.Price))
            {
                PriceError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }
            else if (!float.TryParse(ServiceModel.Price, out float result))
            {
                PriceError = "Syötteen tulee olla numeerinen";
                validInput = false;
            }

            return validInput;
        }

        /// <summary>
        ///  Handles the click event of the "Add" button. If the input provided by the user is valid,
        ///  new service is added to the database by calling the AddServiceToDatabase() method.
        /// </summary>
        private void AddButton()
        {
            if (InputValidation())
            {
                AddServiceToDatabase();
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