using Ohtu1Project.ViewModels.ReservationViewModels;
using System.Windows;

namespace Ohtu1Project.Views.ReservationViews
{
    /// <summary>
    /// Interaction logic for ReservationsWindow.xaml
    /// </summary>
    public partial class ReservationsWindow : Window
    {
        public ReservationsWindow()
        {
            InitializeComponent();

            this.DataContext = new ReservationsWindowViewModel();
        }
    }
}