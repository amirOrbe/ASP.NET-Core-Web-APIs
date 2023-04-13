namespace NZWalks.API.Models.Domain
{
    // Class for Region.
    public class Region
    {
        // Id for region.
        public Guid Id { get; set; }

        // Code for region.
        public string Code { get; set; }

        // Name for region.
        public string Name { get; set; }

        // RegionImageUrl for region.
        public string? RegionImageUrl { get; set; }
    }
}
