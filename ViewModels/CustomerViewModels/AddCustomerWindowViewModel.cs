using System;
using System.Linq;
using Ohtu1Project.Views;
using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using Ohtu1Project.Repositories;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.CustomerViewModels
{
    /// <summary>
    /// Datacontext class for AddCustomerWindow. Inherits MainViewModel class.
    /// </summary>
    internal class AddCustomerWindowViewModel : MainViewModel
    {
        private CustomerModel _customerModel = new();
        public CustomerModel CustomerModel { get { return _customerModel; } set { _customerModel = value; OnPropertyChanged(); } }

        private string _firstNameError;
        public string FirstNameError { get { return _firstNameError; } set { _firstNameError = value; OnPropertyChanged(); } }

        private string _lastNameError;
        public string LastNameError { get { return _lastNameError; } set { _lastNameError = value; OnPropertyChanged(); } }

        private string _streetAddressError;
        public string StreetAddressError { get { return _streetAddressError; } set { _streetAddressError = value; OnPropertyChanged(); } }

        private string _postalCodeError;
        public string PostalCodeError { get { return _postalCodeError; } set { _postalCodeError = value; OnPropertyChanged(); } }

        private string _cityError;
        public string CityError { get { return _cityError; } set { _cityError = value; OnPropertyChanged(); } }

        private string _phoneNumberError;
        public string PhoneNumberError { get { return _phoneNumberError; } set { _phoneNumberError = value; OnPropertyChanged(); } }

        private string _emailError;
        public string EmailError { get { return _emailError; } set { _emailError = value; OnPropertyChanged(); } }


        /// <summary>
        /// Command that is responsible for adding new customer.
        /// </summary>
        public ICommand AddButtonCommand => new DelegateCommand(AddButton);

        /// <summary>
        /// Command that is responsible for closing the window.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);


        /// <summary>
        /// Validates the inputs for adding a customer and sets error messages for any invalid input fields.
        /// </summary>
        /// <returns>True if all input fields are valid, otherwise false.</returns>
        private bool InputValidation()
        {
            bool validInput = true;

            FirstNameError = string.Empty;
            LastNameError = string.Empty;
            StreetAddressError = string.Empty;
            PostalCodeError = string.Empty;
            CityError = string.Empty;
            PhoneNumberError = string.Empty;
            EmailError = string.Empty;

            if (string.IsNullOrWhiteSpace(CustomerModel.FirstName))
            {
                FirstNameError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(CustomerModel.LastName))
            {
                LastNameError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(CustomerModel.StreetAddress))
            {
                StreetAddressError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(CustomerModel.PostalCode))
            {
                PostalCodeError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }
            else if (!CustomerModel.PostalCode.All(char.IsNumber) || CustomerModel.PostalCode.Length != 5)
            {
                PostalCodeError = "Postinumeron tulee koostua viidestä numerosta";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(CustomerModel.City))
            {
                CityError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(CustomerModel.PhoneNumber))
            {
                PhoneNumberError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }
            else if (!CustomerModel.PhoneNumber.All(char.IsNumber) || CustomerModel.PhoneNumber.Length != 10)
            {
                PhoneNumberError = "Puhelinnumeron tulee koostua kymmenestä numerosta";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(CustomerModel.Email))
            {
                EmailError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }

            return validInput;
        }

        /// <summary>
        /// Adds new customer to the database via the CustomerRepository class and closes the current window if successful.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's retry method to itself.
        /// </summary>
        public void AddCustomerToDatabase()
        {
            try
            {
                CustomerRepository.AddCustomer(CustomerModel);
                WindowManager.CloseWindow();
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.RetryMethod = () => AddCustomerToDatabase();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        ///  Handles the click event of the "Add" button. If the input provided by the user is valid,
        ///  new customer is added to the database by calling the AddCustomerToDatabase() method.
        /// </summary>
        private void AddButton()
        {
            if (InputValidation())
            {
                AddCustomerToDatabase();
            }
        }

        /// <summary>
        /// Handles the click event of the "Return" button. Closes the current window
        /// by calling CloseWindow() method in WindowManager class.
        /// </summary>
        private void ReturnButton()
        {
            WindowManager.CloseWindow();
        }
    }
}