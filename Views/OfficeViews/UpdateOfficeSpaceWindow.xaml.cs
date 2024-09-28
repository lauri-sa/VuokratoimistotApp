using Ohtu1Project.ViewModels.OfficeViewModels;
using System.Windows;

namespace Ohtu1Project.Views.OfficeViews
{
    /// <summary>
    /// Interaction logic for UpdateOfficeSpaceWindow.xaml
    /// </summary>
    public partial class UpdateOfficeSpaceWindow : Window
    {
        public UpdateOfficeSpaceWindow()
        {
            InitializeComponent();

            this.DataContext = new UpdateOfficeSpaceWindowViewModel();
        }
    }
}