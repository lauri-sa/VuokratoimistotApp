using Ohtu1Project.ViewModels.CustomerViewModels;
using System.Windows;

namespace Ohtu1Project.Views
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public CustomerWindow()
        {
            InitializeComponent();

            this.DataContext = new CustomerWindowViewModel();
        }
    }
}