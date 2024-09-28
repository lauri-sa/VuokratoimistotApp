namespace Ohtu1Project.Models
{
    /// <summary>
    /// A model class for service.
    /// </summary>
    internal class ServiceModel
    {
        public int ID { get; set; }
        public int SpaceID { get; set; }
        public string? SpaceName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
    }
}