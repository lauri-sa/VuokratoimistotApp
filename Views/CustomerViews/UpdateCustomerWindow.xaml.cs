using Ohtu1Project.ViewModels.CustomerViewModels;
using System.Windows;

namespace Ohtu1Project.Views
{
    /// <summary>
    /// Interaction logic for UpdateCustomerWindow.xaml
    /// </summary>
    public partial class UpdateCustomerWindow : Window
    {
        public UpdateCustomerWindow()
        {
            InitializeComponent();

            this.DataContext = new UpdateCustomerWindowViewModel();
        }
    }
}