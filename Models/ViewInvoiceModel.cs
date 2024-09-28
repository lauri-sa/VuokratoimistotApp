using System;
using System.Collections.ObjectModel;

namespace Ohtu1Project.Models
{
    /// <summary>
    /// A model class for viewed invoices.
    /// </summary>
    internal class ViewInvoiceModel
    {
        public int ID { get; set; }
        public int ReservationID { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? StreetAddress { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? Date { get; set; }
        public string? DueDate { get; set; }
        public string? TotalSum { get; set; }
        public ObservableCollection<InvoiceRowModel> InvoiceRows { get; set; }
    }
}