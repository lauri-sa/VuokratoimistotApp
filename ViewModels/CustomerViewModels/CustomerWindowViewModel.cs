using System;
using Ohtu1Project.Views;
using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using System.Threading.Tasks;
using Ohtu1Project.Repositories;
using System.Collections.ObjectModel;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.CustomerViewModels
{
    /// <summary>
    /// Datacontext class for CustomerWindow. Inherits MainViewModel class.
    /// </summary>
    internal class CustomerWindowViewModel : MainViewModel
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

        private CustomerModel _customerModel;
        public CustomerModel CustomerModel { get { return _customerModel; } set { _customerModel = value; OnPropertyChanged(); } }

        private ObservableCollection<CustomerModel> _customersCollection;
        public ObservableCollection<CustomerModel> CustomersCollection { get { return _customersCollection; } set { _customersCollection = value; OnPropertyChanged(); } }

        /// <summary>
        /// Command that is responsible for ContentRendered event.
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand(Loaded);

        /// <summary>
        /// Command that is responsible for lose focus from datagrid.
        /// </summary>
        public ICommand LostFocusCommand => new DelegateCommand(LostFocus);

        /// <summary>
        /// Command that is responsible for opening the AddCustomerWindow.
        /// </summary>
        public ICommand AddButtonCommand => new DelegateCommand(AddButton);

        /// <summary>
        /// Command that is responsible for opening the UpdateCustomerWindow.
        /// </summary>
        public ICommand ModifyButtonCommand => new DelegateCommand(ModifyButton);

        /// <summary>
        /// Command that is responsible for opening the DeleteConfirmationWindow.
        /// </summary>
        public ICommand DeleteButtonCommand => new DelegateCommand(DeleteButton);

        /// <summary>
        /// Command that is responsible for closing the window.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);

        /// <summary>
        /// Calls GetCustomers() method asynchronously.
        /// </summary>
        private async void Loaded()
        {
            await GetCustomers();
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
                CustomersCollection = await CustomerRepository.FetchAllCustomers();
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
        /// Sets SelectedIndex property value to -1.
        /// </summary>
        private void LostFocus()
        {
            SelectedIndex = -1;
        }

        /// <summary>
        /// Event handler for the add button. Opens the AddCustomerWindow
        /// and refreshes the CustomersCollection by calling GetCustomers method asynchronously.
        /// </summary>
        private async void AddButton()
        {
            WindowManager.OpenWindow(new AddCustomerWindow());
            await GetCustomers();
            LostFocus();
        }

        /// <summary>
        /// Event handler for the modify button. Opens the UpdateCustomerWindow with the current CustomerModel data
        /// and refreshes the CustomersCollection by calling GetCustomers method asynchronously.
        /// </summary>
        private async void ModifyButton()
        {
            UpdateCustomerWindowViewModel.CustomerModel = CustomerModel;
            WindowManager.OpenWindow(new UpdateCustomerWindow());
            await GetCustomers();
            LostFocus();
        }

        /// <summary>
        /// Event handler for the delete button. Opens a delete confirmation window,
        /// sets the delete action in DeleteConfirmationWindowViewModel
        /// to DeleteCustomer() method in CustomerRepository class and refreshes the CustomersCollection
        /// by calling GetCustomers method asynchronously.
        /// </summary>
        private async void DeleteButton()
        {
            DeleteConfirmationWindowViewModel.DeleteAction = () => CustomerRepository.DeleteCustomer(CustomerModel.ID);
            WindowManager.OpenWindow(new DeleteConfirmationWindow());
            await GetCustomers();
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