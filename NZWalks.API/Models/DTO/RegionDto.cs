namespace NZWalks.API.Models.DTO
{
    public class RegionDto
    {

        // Id for region.
        public Guid Id { get; set; }

        // Code for region.
        public string Code { get; set; }

        // Code for region.
        public string Name { get; set; }

        // RegionImageUrl for region.
        public string? RegionImageUrl { get; set; }
    }
}
