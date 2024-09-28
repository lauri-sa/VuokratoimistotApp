using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;

namespace Ohtu1Project.ViewModels.ReportViewModels
{
    /// <summary>
    /// Datacontext class for ViewReportWindow. Inherits MainViewModel class.
    /// </summary>
    internal class ViewReportWindowViewModel : MainViewModel
    {
        public static OfficeReportModel OfficeReportModel { get; set; }

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