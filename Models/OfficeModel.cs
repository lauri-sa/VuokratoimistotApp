namespace Ohtu1Project.Models
{
    /// <summary>
    /// A model class for office.
    /// </summary>
    internal class OfficeModel
    {
        public int ID { get; set; }
        public string? OfficeName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}