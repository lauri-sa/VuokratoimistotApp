using Ohtu1Project.ViewModels.ReservationViewModels;
using System.Windows;

namespace Ohtu1Project.Views.ReservationViews
{
    /// <summary>
    /// Interaction logic for UpdateReservationWindow.xaml
    /// </summary>
    public partial class UpdateReservationWindow : Window
    {
        public UpdateReservationWindow()
        {
            InitializeComponent();

            this.DataContext = new UpdateReservationWindowViewModel();
        }
    }
}