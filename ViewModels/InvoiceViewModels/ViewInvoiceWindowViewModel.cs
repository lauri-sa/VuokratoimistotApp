using System;
using System.IO;
using System.Windows;
using System.Net.Mail;
using Ohtu1Project.Views;
using System.Windows.Data;
using Ohtu1Project.Models;
using System.Windows.Input;
using System.Windows.Media;
using Ohtu1Project.Helpers;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.InvoiceViewModels
{
    /// <summary>
    /// Datacontext class for ViewInvoiceWindow. Inherits MainViewModel class.
    /// </summary>
    internal class ViewInvoiceWindowViewModel : MainViewModel
    {
        public static ViewInvoiceModel ViewInvoiceModel { get; set; }

        /// <summary>
        /// A command for the email button.
        /// </summary>
        public ICommand EmailButtonCommand => new DelegateCommand(EmailButton);

        /// <summary>
        /// A command for the print button.
        /// </summary>
        public ICommand PrintButtonCommand => new DelegateCommand(PrintButton);

        /// <summary>
        /// A command for the return button.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);

        /// <summary>
        /// Creates a temporary WPF window, adds the invoice UI element to it,
        /// and renders the window as a PNG image. The image is then attached to an email,
        /// which is sent to the customer's email address using Gmail SMTP server.
        /// </summary>
        public void SendEmail()
        {
            try
            {
                var tempWindow = new Window
                {
                    Width = 800,
                    Height = 1000,
                    ShowInTaskbar = false,
                    WindowStyle = WindowStyle.None,
                    ResizeMode = ResizeMode.NoResize,
                    Topmost = false,
                    Content = DrawInvoice() // Add the invoice UI element to the temporary window
                };

                tempWindow.Show();

                var visual = new VisualBrush(tempWindow)
                {
                    ViewboxUnits = BrushMappingMode.Absolute,
                    Viewbox = new Rect(0, 0, 800, 1000)
                };

                tempWindow.Close();

                
                var bitmap = new RenderTargetBitmap(800, 1000, 96, 96, PixelFormats.Pbgra32);

                var drawingVisual = new DrawingVisual();
                using (var grid = drawingVisual.RenderOpen())
                {
                    grid.DrawRectangle(visual, null, new Rect(0, 0, 800, 1000));
                }
                bitmap.Render(drawingVisual);

                
                var pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(bitmap));

                
                using (var memoryStream = new MemoryStream())
                {
                    pngEncoder.Save(memoryStream);

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var mail = new MailMessage();
                    var SmtpServer = new SmtpClient("smtp.gmail.com");

                    var attachment = new Attachment(memoryStream, "Lasku.png");

                    mail.Attachments.Add(attachment);

                    mail.From = new MailAddress("AddYourEmailAddressHere");
                    mail.To.Add($"{ViewInvoiceModel.CustomerEmail}");
                    mail.Subject = "Lasku";
                    mail.Body = @$"<html>
                                    <body>
                                        <span><strong>Hei {ViewInvoiceModel.CustomerName}</strong></span>
                                        <br>
                                        <br>
                                        <span>Kiitos, että valitsit VuokraToimistot Oy:n.</span>
                                        <br>
                                        <span>Liitteenä lähetämme laskun tekemästänne varauksesta.</span>
                                        <br>
                                        <br>
                                        <span>Mikäli sinulla on kysyttävää laskustasi,</span>
                                        <br>
                                        <span>ole yhteydessä asiakaspalveluumme sähköpostilla</span>
                                        <br>
                                        <span>AddYourEmailAddressHere.</span>
                                        <br>
                                        <br>
                                        <span>Ethän vastaa suoraan tähän automaattiseen viestiin.</span>
                                    </body>
                                   </html>";

                    mail.IsBodyHtml = true;

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("AddYourEmailAddressHere", "AddYourCredentialsHere");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                } 
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.RetryMethod = () => SendEmail();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Draws an invoice Grid object with all the required details.
        /// </summary>
        /// <returns>Returns a Grid object representing an invoice.</returns>
        public Grid DrawInvoice()
        {
            var grid = new Grid();

            for (int i = 0; i < 5; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = GridLength.Auto;
                grid.RowDefinitions.Add(row);
            }

            for (int i = 0; i < 4; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(col);
            }

            var image = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Logo-dual-colour.png", UriKind.Absolute)),
                Height = 84,
                Width = 264,
                Margin = new Thickness(0, 50, 0, 50)
            };

            Grid.SetColumnSpan(image, 4);
            grid.Children.Add(image);

            var billerStackPanel = new StackPanel
            {
                Width = 400,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var headerLabel = new Label
            {
                Content = "LASKU",
                Height = 50,
                FontWeight = FontWeights.Bold,
                FontSize = 20,
                Margin = new Thickness(100, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var contentLabel1 = new Label
            {
                Content = "Laskuttaja",
                FontSize = 20,
                Margin = new Thickness(100, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var contentLabel2 = new Label
            {
                Content = "VuokraToimistot Oy",
                FontSize = 20,
                Margin = new Thickness(100, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var contentLabel3 = new Label
            {
                Content = "Karjalankatu 3",
                FontSize = 20,
                Margin = new Thickness(100, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var contentLabel4 = new Label
            {
                Content = "80200 Joensuu",
                FontSize = 20,
                Margin = new Thickness(100, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            billerStackPanel.Children.Add(headerLabel);
            billerStackPanel.Children.Add(contentLabel1);
            billerStackPanel.Children.Add(contentLabel2);
            billerStackPanel.Children.Add(contentLabel3);
            billerStackPanel.Children.Add(contentLabel4);

            Grid.SetRow(billerStackPanel, 1);
            Grid.SetColumnSpan(billerStackPanel, 2);
            grid.Children.Add(billerStackPanel);

            var detailStackPanel = new StackPanel
            {
                Width = 400,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var dateLabel = new Label
            {
                Content = $"Päiväys: {ViewInvoiceModel.Date}",
                FontSize = 20,
                Margin = new Thickness(0, 50, 100, 0),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var idLabel = new Label
            {
                Content = $"Laskun numero: {ViewInvoiceModel.ID}",
                FontSize = 20,
                Margin = new Thickness(0, 0, 100, 0),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var dueDateLabel = new Label
            {
                Content = $"Eräpäivä: {ViewInvoiceModel.DueDate}",
                FontSize = 20,
                Margin = new Thickness(0, 0, 100, 0),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            detailStackPanel.Children.Add(dateLabel);
            detailStackPanel.Children.Add(idLabel);
            detailStackPanel.Children.Add(dueDateLabel);

            Grid.SetColumn(detailStackPanel, 2);
            Grid.SetRow(detailStackPanel, 1);
            Grid.SetColumnSpan(detailStackPanel, 2);
            grid.Children.Add(detailStackPanel);

            var customerStackPanel = new StackPanel
            {
                Width = 400,
                Margin = new Thickness(0, 50, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var customerLabel = new Label
            {
                Content = "Asiakas:",
                FontSize = 20,
                Margin = new Thickness(100, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var customerNameLabel = new Label
            {
                Content = $"{ViewInvoiceModel.CustomerName}",
                FontSize = 20,
                Margin = new Thickness(100, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var customerAddressLabel = new Label
            {
                Content = $"{ViewInvoiceModel.StreetAddress}",
                FontSize = 20,
                Margin = new Thickness(100, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var customerCityLabel = new Label
            {
                Content = $"{ViewInvoiceModel.PostalCode} {ViewInvoiceModel.City}",
                FontSize = 20,
                Margin = new Thickness(100, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            customerStackPanel.Children.Add(customerLabel);
            customerStackPanel.Children.Add(customerNameLabel);
            customerStackPanel.Children.Add(customerAddressLabel);
            customerStackPanel.Children.Add(customerCityLabel);

            Grid.SetRow(customerStackPanel, 2);
            Grid.SetColumnSpan(customerStackPanel, 2);
            grid.Children.Add(customerStackPanel);

            var invoiceRowDataGrid = new DataGrid
            {
                Width = 600,
                FontSize = 20,
                Margin = new Thickness(0, 50, 0, 0),
                Background = Brushes.White,
                BorderThickness = new Thickness(0),
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                ItemsSource = ViewInvoiceModel.InvoiceRows,
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                GridLinesVisibility = DataGridGridLinesVisibility.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var columnHeaderStyle = new Style(typeof(DataGridColumnHeader));
            columnHeaderStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.White));
            columnHeaderStyle.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Left));
            invoiceRowDataGrid.ColumnHeaderStyle = columnHeaderStyle;

            var rowStyle = new Style(typeof(DataGridRow));
            rowStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.White));
            rowStyle.Setters.Add(new Setter(Control.MarginProperty, new Thickness(-3, 0, 0, 0)));
            invoiceRowDataGrid.RowStyle = rowStyle;

            var rowHeaderStyle = new Style(typeof(DataGridRowHeader));
            rowHeaderStyle.Setters.Add(new Setter(Control.OverridesDefaultStyleProperty, true));
            invoiceRowDataGrid.RowHeaderStyle = rowHeaderStyle;

            var productNameColumn = new DataGridTextColumn
            {
                Width = 150,
                Header = "Tuote",
                Binding = new Binding("ProductName")
            };

            var productPriceColumn = new DataGridTextColumn
            {
                Width = 150,
                Header = "Hinta / pvä",
                Binding = new Binding("ProductPrice")
            };

            var amountColumn = new DataGridTextColumn
            {
                Width = 150,
                Header = "Kpl",
                Binding = new Binding("Amount")
            };

            var totalSumColumn = new DataGridTextColumn
            {
                Width = 150,
                Header = "Yhteensä",
                Binding = new Binding("TotalSum")
            };

            invoiceRowDataGrid.Columns.Add(productNameColumn);
            invoiceRowDataGrid.Columns.Add(productPriceColumn);
            invoiceRowDataGrid.Columns.Add(amountColumn);
            invoiceRowDataGrid.Columns.Add(totalSumColumn);

            Grid.SetRow(invoiceRowDataGrid, 3);
            Grid.SetColumnSpan(invoiceRowDataGrid, 4);
            grid.Children.Add(invoiceRowDataGrid);

            var totalSumTextBlock = new TextBlock
            {
                Width = 600,
                FontSize = 20,
                Margin = new Thickness(0, 50, 0, 0),
                TextAlignment = TextAlignment.Right,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = $"YHTEENSÄ: {ViewInvoiceModel.TotalSum} €"
            };

            Grid.SetRow(totalSumTextBlock, 4);
            Grid.SetColumnSpan(totalSumTextBlock, 4);
            grid.Children.Add(totalSumTextBlock);

            return grid;
        }

        /// <summary>
        /// Event handler for the email button.
        /// Calls SendEmail() method.
        /// </summary>
        private void EmailButton()
        {
            SendEmail();
        }

        /// <summary>
        /// Event handler for the print button.
        /// Creates a PDF-file from the currently selected invoice.
        /// </summary>
        private void PrintButton()
        {
            var printDialog = new PrintDialog();
            printDialog.PrintVisual(DrawInvoice(), "Invoice");
        }

        /// <summary>
        /// Event handler for the return button. Closes the window.
        /// </summary>
        private void ReturnButton()
        {
            WindowManager.CloseWindow();
        }
    }
}