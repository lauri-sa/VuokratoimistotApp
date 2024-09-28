using System;
using System.Linq;
using Ohtu1Project.Views;
using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using Ohtu1Project.Repositories;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.OfficeViewModels
{
    /// <summary>
    /// Datacontext class for AddOfficeWindow. Inherits MainViewModel class.
    /// </summary>
    internal class AddOfficeWindowViewModel : MainViewModel
    {
        private OfficeModel _officeModel = new();
        public OfficeModel OfficeModel { get { return _officeModel; } set { _officeModel = value; OnPropertyChanged(); } }

        private string _nameError;
        public string NameError { get { return _nameError; } set { _nameError = value; OnPropertyChanged(); } }

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
        /// A command for the add button.
        /// </summary>
        public ICommand AddButtonCommand => new DelegateCommand(AddButton);

        /// <summary>
        /// A command for the return button.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);

        /// <summary>
        /// Validates the inputs for adding a office and sets error messages for any invalid input fields.
        /// </summary>
        /// <returns>True if all input fields are valid, otherwise false.</returns>
        private bool InputValidation()
        {
            bool validInput = true;

            NameError = string.Empty;
            StreetAddressError = string.Empty;
            PostalCodeError = string.Empty;
            CityError = string.Empty;
            PhoneNumberError = string.Empty;
            EmailError = string.Empty;

            if (string.IsNullOrWhiteSpace(OfficeModel.OfficeName))
            {
                NameError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(OfficeModel.StreetAddress))
            {
                StreetAddressError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(OfficeModel.PostalCode))
            {
                PostalCodeError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }
            else if (!OfficeModel.PostalCode.All(char.IsNumber) || OfficeModel.PostalCode.Length != 5)
            {
                PostalCodeError = "Postinumeron tulee koostua viidestä numerosta";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(OfficeModel.City))
            {
                CityError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(OfficeModel.PhoneNumber))
            {
                PhoneNumberError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }
            else if (!OfficeModel.PhoneNumber.All(char.IsNumber) || OfficeModel.PhoneNumber.Length != 10)
            {
                PhoneNumberError = "Puhelinnumeron tulee koostua kymmenestä numerosta";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(OfficeModel.Email))
            {
                EmailError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }

            return validInput;
        }

        /// <summary>
        /// Adds new office to the database via the OfficeRepository class and closes the current window if successful.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's retry method to itself.
        /// </summary>
        public void AddOfficeToDatabase()
        {
            try
            {
                OfficeRepository.AddOffice(OfficeModel);
                WindowManager.CloseWindow();
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.RetryMethod = () => AddOfficeToDatabase();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        ///  Handles the click event of the "Add" button. If the input provided by the user is valid,
        ///  new office is added to the database by calling the AddOfficeToDatabase() method.
        /// </summary>
        private void AddButton()
        {
            if (InputValidation())
            {
                AddOfficeToDatabase();
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