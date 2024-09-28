using System;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using System.Threading.Tasks;

namespace Ohtu1Project.ViewModels.PopupViewModels
{
    /// <summary>
    /// Datacontext class for ErrorWindowViewModel. Inherits MainViewModel class.
    /// </summary>
    internal class ErrorWindowViewModel : MainViewModel
    {
        public static Action RetryMethod { get; set; }

        public static Func<Task> AsyncRetryMethod { get; set; }

        /// <summary>
        /// A command for the retry button.
        /// </summary>
        public ICommand RetryButtonCommand => new DelegateCommand(RetryButton);

        /// <summary>
        /// A command for the cancel button.
        /// </summary>
        public ICommand CancelButtonCommand => new DelegateCommand(CancelButton);

        /// <summary>
        /// Clears the delegates of the ErrorWindowViewModel class, resetting the retry methods to null.
        /// </summary>
        public static void ClearDelegates()
        {
            RetryMethod = null;
            AsyncRetryMethod = null;
        }

        /// <summary>
        /// Event handler for the retry button. Invokes one of the retry methods.
        /// If RetryMethod is not null, it will invoke the method synchronously.
        /// If AsyncRetryMethod is not null, it will invoke the method asynchronously.
        /// Finally, it closes the error window.
        /// </summary>
        private void RetryButton()
        {
            if (RetryMethod != null)
            {
                RetryMethod.Invoke();
            }
            else if (AsyncRetryMethod != null)
            {
                AsyncRetryMethod.Invoke();
            }

            WindowManager.CloseWindow();
        }

        /// <summary>
        /// Event handler for the cancel button.
        /// Returns back to main window by calling ReturnToMainMenu() method in WindowManager class.
        /// </summary>
        private void CancelButton()
        {
            WindowManager.ReturnToMainWindow();
        }
    }
}