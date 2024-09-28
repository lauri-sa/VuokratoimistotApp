using System;
using Ohtu1Project.Views;
using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using Ohtu1Project.Repositories;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.OfficeViewModels
{
    /// <summary>
    /// Datacontext class for AddOfficeSpaceWindow. Inherits MainViewModel class.
    /// </summary>
    internal class AddOfficeSpaceWindowViewModel : MainViewModel
    {
        private static OfficeSpaceModel _officeSpaceModel;
        public static OfficeSpaceModel OfficeSpaceModel { get { return _officeSpaceModel; } set { _officeSpaceModel = value; OnStaticPropertyChanged(); } }

        private string _nameError;
        public string NameError { get { return _nameError; } set { _nameError = value; OnPropertyChanged(); } }

        private string _sizeError;
        public string SizeError { get { return _sizeError; } set { _sizeError = value; OnPropertyChanged(); } }

        private string _capacityError;
        public string CapacityError { get { return _capacityError; } set { _capacityError = value; OnPropertyChanged(); } }

        private string _priceError;
        public string PriceError { get { return _priceError; } set { _priceError = value; OnPropertyChanged(); } }

        /// <summary>
        /// A command for the add button.
        /// </summary>
        public ICommand AddButtonCommand => new DelegateCommand(AddButton);

        /// <summary>
        /// A command for the return button.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);

        /// <summary>
        /// Validates the inputs for adding a office space and sets error messages for any invalid input fields.
        /// </summary>
        /// <returns>True if all input fields are valid, otherwise false.</returns>
        private bool InputValidation()
        {
            bool validInput = true;

            NameError = string.Empty;
            SizeError = string.Empty;
            CapacityError = string.Empty;
            PriceError = string.Empty;

            if (string.IsNullOrWhiteSpace(OfficeSpaceModel.Name))
            {
                NameError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(OfficeSpaceModel.Size))
            {
                SizeError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }
            else if (!int.TryParse(OfficeSpaceModel.Size, out int result))
            {
                SizeError = "Syötteen tulee olla kokonaisluku";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(OfficeSpaceModel.Capacity))
            {
                CapacityError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }
            else if (!int.TryParse(OfficeSpaceModel.Capacity, out int result))
            {
                CapacityError = "Syötteen tulee olla kokonaisluku";
                validInput = false;
            }

            if (string.IsNullOrWhiteSpace(OfficeSpaceModel.Price))
            {
                PriceError = "Kenttä ei voi olla tyhjä";
                validInput = false;
            }
            else if (!float.TryParse(OfficeSpaceModel.Price, out float result))
            {
                PriceError = "Syötteen tulee olla numeerinen";
                validInput = false;
            }

            return validInput;
        }

        /// <summary>
        /// Adds new office space to the database via the OfficeSpaceRepository class and closes the current window if successful.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's retry method to itself.
        /// </summary>
        public void AddOfficeSpaceToDatabase()
        {
            try
            {
                OfficeSpaceRepository.AddOfficeSpace(OfficeSpaceModel);
                WindowManager.CloseWindow();
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.RetryMethod = () => AddOfficeSpaceToDatabase();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        ///  Handles the click event of the "Add" button. If the input provided by the user is valid,
        ///  new office space is added to the database by calling the AddOfficeSpaceToDatabase() method.
        /// </summary>
        private void AddButton()
        {
            if (InputValidation())
            {
                AddOfficeSpaceToDatabase();
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