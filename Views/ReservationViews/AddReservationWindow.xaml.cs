using Ohtu1Project.ViewModels.ReservationViewModels;
using System.Windows;

namespace Ohtu1Project.Views.ReservationViews
{
    /// <summary>
    /// Interaction logic for AddReservation.xaml
    /// </summary>
    public partial class AddReservationWindow : Window
    {
        public AddReservationWindow()
        {
            InitializeComponent();

            this.DataContext = new AddReservationWindowViewModel();
        }
    }
}