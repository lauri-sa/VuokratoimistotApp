using System;
using System.Collections.ObjectModel;

namespace Ohtu1Project.Models
{
    /// <summary>
    /// A model class for office reports.
    /// </summary>
    internal class OfficeReportModel
    {
        public int ReservationID { get; set; }
        public string? ReservationDate { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Period { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set;}
        public string? Email { get; set; }
        public string? OfficeName { get; set; }
        public string? OfficeSpaceName { get; set;}
        public ObservableCollection<ServiceModel> ReservationServices { get; set; }
    }
}