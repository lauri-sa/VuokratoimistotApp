using Ohtu1Project.ViewModels.OfficeViewModels;
using System.Windows;

namespace Ohtu1Project.Views.OfficeViews
{
    /// <summary>
    /// Interaction logic for UpdateOfficeWindow.xaml
    /// </summary>
    public partial class UpdateOfficeWindow : Window
    {
        public UpdateOfficeWindow()
        {
            InitializeComponent();

            this.DataContext = new UpdateOfficeWindowViewModel();
        }
    }
}