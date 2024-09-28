using Ohtu1Project.ViewModels.PopupViewModels;
using System.Windows;

namespace Ohtu1Project.Views
{
    /// <summary>
    /// Interaction logic for DeleteConfirmationWindow.xaml
    /// </summary>
    public partial class DeleteConfirmationWindow : Window
    {
        public DeleteConfirmationWindow()
        {
            InitializeComponent();

            this.DataContext = new DeleteConfirmationWindowViewModel();
        }
    }
}