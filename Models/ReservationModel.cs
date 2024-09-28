using System;
using System.Collections.ObjectModel;

namespace Ohtu1Project.Models
{
    /// <summary>
    /// A model class for reservation.
    /// </summary>
    internal class ReservationModel
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int OfficeID { get; set; }
        public int SpaceID { get; set; }
        public string? CustomerName { get; set; }
        public float OfficeSpacePrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? ReservationDay { get; set; }
        public ObservableCollection<ServiceModel> ReservationServices { get; set; }
    }
}