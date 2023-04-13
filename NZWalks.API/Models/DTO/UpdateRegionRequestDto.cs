using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be minimum of 3 chars")]
        [MaxLength(3, ErrorMessage = "Code has to be maximum of 3 chars")]
        public string Code { get; set; }

        // Name for region.
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be maximum of 100 chars")]
        public string Name { get; set; }

        // RegionImageUrl for region.
        public string? RegionImageUrl { get; set; }
    }
}
