namespace Ohtu1Project.Models
{
    /// <summary>
    /// A model class for office space.
    /// </summary>
    internal class OfficeSpaceModel
    {
        public int ID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Capacity { get; set; }
        public string Size { get; set; }
    }
}