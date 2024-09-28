using Ohtu1Project.ViewModels.ReportViewModels;
using System.Windows;

namespace Ohtu1Project.Views.ReportViews
{
    /// <summary>
    /// Interaction logic for ReportsWindow.xaml
    /// </summary>
    public partial class ReportsWindow : Window
    {
        public ReportsWindow()
        {
            InitializeComponent();

            this.DataContext = new ReportsWindowViewModel();
        }
    }
}