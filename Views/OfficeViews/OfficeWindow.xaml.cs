using Ohtu1Project.ViewModels.OfficeViewModels;
using System.Windows;

namespace Ohtu1Project.Views
{
    /// <summary>
    /// Interaction logic for OfficeWindow.xaml
    /// </summary>
    public partial class OfficeWindow : Window
    {
        public OfficeWindow()
        {
            InitializeComponent();

            this.DataContext = new OfficeWindowViewModel();
        }
    }
}