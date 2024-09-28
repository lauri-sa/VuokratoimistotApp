using Ohtu1Project.ViewModels.ServiceViewModels;
using System.Windows;

namespace Ohtu1Project.Views
{
    /// <summary>
    /// Interaction logic for ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window
    {
        public ServiceWindow()
        {
            InitializeComponent();

            this.DataContext = new ServiceWindowViewModel();
        }
    }
}