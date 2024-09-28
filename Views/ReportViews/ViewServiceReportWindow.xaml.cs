using Ohtu1Project.ViewModels.ReportViewModels;
using System.Windows;

namespace Ohtu1Project.Views.ReportViews
{
    /// <summary>
    /// Interaction logic for ViewServiceReportWindow.xaml
    /// </summary>
    public partial class ViewServiceReportWindow : Window
    {
        public ViewServiceReportWindow()
        {
            InitializeComponent();

            this.DataContext = new ViewServiceReportWindowViewModel();
        }
    }
}