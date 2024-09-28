using System;

namespace Ohtu1Project.Models
{
    /// <summary>
    /// A model class for invoice.
    /// </summary>
    internal class InvoiceModel
    {
        public int ID { get; set; }
        public int ReservationID { get; set; }
        public int CustomerID { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public float TotalSum { get; set; }
    }
}