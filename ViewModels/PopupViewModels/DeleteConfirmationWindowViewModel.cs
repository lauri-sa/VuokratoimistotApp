using System;
using Ohtu1Project.Views;
using System.Windows.Input;
using Ohtu1Project.Helpers;

namespace Ohtu1Project.ViewModels.PopupViewModels
{
    /// <summary>
    /// Datacontext class for DeleteConfirmationWindowViewModel. Inherits MainViewModel class.
    /// </summary>
    internal class DeleteConfirmationWindowViewModel : MainViewModel
    {
        public static Action DeleteAction { get; set; }

        /// <summary>
        /// A command for a confirmationbutton.
        /// </summary>
        public ICommand ConfirmationButtonCommand => new DelegateCommand(ConfirmationButton);

        /// <summary>
        /// A command for the cancel button.
        /// </summary>
        public ICommand CancelButtonCommand => new DelegateCommand(CancelButton);

        /// <summary>
        /// Event handler for the confirmation button.
        /// When clicked, it invokes the DeleteAction delegate if it has been set, and closes the window.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's retry method to itself.
        /// </summary>
        private void ConfirmationButton()
        {
            try
            {
                DeleteAction?.Invoke();
                WindowManager.CloseWindow();
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.RetryMethod = () => ConfirmationButton();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Event handler for the cancel button. Closes the current window
        /// by calling CloseWindow() method in WindowManager class.
        /// </summary>
        private void CancelButton()
        {
            WindowManager.CloseWindow();
        }
    }
}