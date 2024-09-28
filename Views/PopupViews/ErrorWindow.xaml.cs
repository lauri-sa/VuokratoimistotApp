using Ohtu1Project.ViewModels.PopupViewModels;
using System.Windows;

namespace Ohtu1Project.Views
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow()
        {
            InitializeComponent();

            this.DataContext = new ErrorWindowViewModel();
        }
    }
}