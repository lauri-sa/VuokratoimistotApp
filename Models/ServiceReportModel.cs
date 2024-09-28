using System;

namespace Ohtu1Project.Models
{
    /// <summary>
    /// A model class for service reports.
    /// </summary>
    internal class ServiceReportModel
    {
        public int ReservationID { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Period { get; set; }
        public string? CustomerName { get; set; }
        public string? OfficeName { get; set;}
        public string? OfficeSpaceName { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceDescription { get; set;}
        public string? ServicePrice { get; set; }
    }
}