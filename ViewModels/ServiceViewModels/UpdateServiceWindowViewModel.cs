using System;
using Ohtu1Project.Views;
using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using Ohtu1Project.Repositories;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.ServiceViewModels
{
    /// <summary>
    /// Datacontext class for UpdateServiceWindow. Inherits MainViewModel class.
    /// </summary>
    internal class UpdateServiceWindowViewModel : MainViewModel
    {
        public static string OfficeName { get; set; }

        private string _nameError;
        public string NameError { get { return _nameError; } set { _nameError = value; OnPropertyChanged(); } }

        private string _priceError;
        public string PriceError { get { return _priceError; } set { _priceError = value; OnPropertyChanged(); } }

        private static ServiceModel _serviceModel;
        public static ServiceModel ServiceModel { get { return _serviceModel; } set { _serviceModel = value; OnStaticPropertyChanged(); } }

        /// <summary>
        /// A command for the add button.
        /// </summary>
        public ICommand AddButtonCommand => new DelegateCommand(AddButton);

        /// <summary>
        /// A command for the return button.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);

        /// <summary>
        /// Validates the inputs for updating a service and sets error messages for any invalid input fields.
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
                PriceError = "Hinnan tulee numeerinen";
                validInput = false;
            }

            return validInput;
        }

        /// <summary>
        /// Updates a service to the database via the ServiceRepository class and closes the current window if successful.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's retry method to itself.
        /// </summary>
        public void UpdateServiceToDatabase()
        {
            try
            {
                ServiceRepository.UpdateService(ServiceModel);
                WindowManager.CloseWindow();
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.RetryMethod = () => UpdateServiceToDatabase();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// A event handler for the add button.
        /// If the input provided by the user is valid,
        /// service is updated to the database by calling the UpdateServiceToDatabase() method.
        /// </summary>
        private void AddButton()
        {
            if (InputValidation())
            {
                UpdateServiceToDatabase();
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