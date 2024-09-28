using Ohtu1Project.ViewModels.InvoiceViewModels;
using System.Windows;

namespace Ohtu1Project.Views.InvoiceViews
{
    /// <summary>
    /// Interaction logic for InvoicesWindow.xaml
    /// </summary>
    public partial class InvoicesWindow : Window
    {
        public InvoicesWindow()
        {
            InitializeComponent();

            this.DataContext = new InvoicesWindowViewModel();
        }
    }
}