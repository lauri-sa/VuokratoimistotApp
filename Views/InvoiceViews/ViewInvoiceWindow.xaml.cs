using Ohtu1Project.ViewModels.InvoiceViewModels;
using System.Windows;

namespace Ohtu1Project.Views.InvoiceViews
{
    /// <summary>
    /// Interaction logic for ViewInvoiceWindow.xaml
    /// </summary>
    public partial class ViewInvoiceWindow : Window
    {
        public ViewInvoiceWindow()
        {
            InitializeComponent();

            this.DataContext = new ViewInvoiceWindowViewModel();
        }
    }
}