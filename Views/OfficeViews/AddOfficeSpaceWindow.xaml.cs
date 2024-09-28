using Ohtu1Project.ViewModels.OfficeViewModels;
using System.Windows;

namespace Ohtu1Project.Views.OfficeViews
{
    /// <summary>
    /// Interaction logic for AddOfficeSpaceWindow.xaml
    /// </summary>
    public partial class AddOfficeSpaceWindow : Window
    {
        public AddOfficeSpaceWindow()
        {
            InitializeComponent();

            this.DataContext = new AddOfficeSpaceWindowViewModel();
        }
    }
}