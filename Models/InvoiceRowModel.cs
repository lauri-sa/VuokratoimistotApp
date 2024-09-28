namespace Ohtu1Project.Models
{
    /// <summary>
    /// A model class for invoice rows.
    /// </summary>
    internal class InvoiceRowModel
    {
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public string Amount { get; set; }
        public string TotalSum { get; set; }
    }
}