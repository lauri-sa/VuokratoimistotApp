using Ohtu1Project.ViewModels.OfficeViewModels;
using System.Windows;

namespace Ohtu1Project.Views.OfficeViews
{
    /// <summary>
    /// Interaction logic for AddOfficeWindow.xaml
    /// </summary>
    public partial class AddOfficeWindow : Window
    {
        public AddOfficeWindow()
        {
            InitializeComponent();

            this.DataContext = new AddOfficeWindowViewModel();
        }
    }
}