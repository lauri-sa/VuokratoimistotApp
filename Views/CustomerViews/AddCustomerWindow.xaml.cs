using Ohtu1Project.ViewModels.CustomerViewModels;
using System.Windows;

namespace Ohtu1Project.Views
{
    /// <summary>
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public AddCustomerWindow()
        {
            InitializeComponent();

            this.DataContext = new AddCustomerWindowViewModel();
        }
    }
}