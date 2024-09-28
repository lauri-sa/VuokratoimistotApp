using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;

namespace Ohtu1Project.ViewModels.ReportViewModels
{
    /// <summary>
    /// Datacontext class for ViewServiceReportWindow. Inherits MainViewModel class.
    /// </summary>
    internal class ViewServiceReportWindowViewModel : MainViewModel
    {
        public static ServiceReportModel ServiceReportModel { get; set; }

        /// <summary>
        /// A command for the return button.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);

        /// <summary>
        /// Event handler for the return button. Closes the window.
        /// </summary>
        private void ReturnButton()
        {
            WindowManager.CloseWindow();
        }
    }
}