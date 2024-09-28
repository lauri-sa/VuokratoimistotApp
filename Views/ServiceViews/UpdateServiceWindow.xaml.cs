using Ohtu1Project.ViewModels.ServiceViewModels;
using System.Windows;

namespace Ohtu1Project.Views
{
    /// <summary>
    /// Interaction logic for UpdateServiceWindow.xaml
    /// </summary>
    public partial class UpdateServiceWindow : Window
    {
        public UpdateServiceWindow()
        {
            InitializeComponent();

            this.DataContext = new UpdateServiceWindowViewModel();
        }
    }
}