using Ohtu1Project.ViewModels.ReportViewModels;
using System.Windows;

namespace Ohtu1Project.Views.ReportViews
{
    /// <summary>
    /// Interaction logic for ViewReportWindow.xaml
    /// </summary>
    public partial class ViewReportWindow : Window
    {
        public ViewReportWindow()
        {
            InitializeComponent();

            this.DataContext = new ViewReportWindowViewModel();
        }
    }
}