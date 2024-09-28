using Ohtu1Project.ViewModels.ServiceViewModels;
using System.Windows;

namespace Ohtu1Project.Views
{
    /// <summary>
    /// Interaction logic for AddServiceWindow.xaml
    /// </summary>
    public partial class AddServiceWindow : Window
    {
        public AddServiceWindow()
        {
            InitializeComponent();

            this.DataContext = new AddServiceWindowViewModel();
        }
    }
}