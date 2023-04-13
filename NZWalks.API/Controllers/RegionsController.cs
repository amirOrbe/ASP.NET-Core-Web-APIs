namespace NZWalks.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NZWalks.API.CustomActionsFilters;
    using NZWalks.API.Data;
    using NZWalks.API.Models.Domain;
    using NZWalks.API.Models.DTO;
    using NZWalks.API.Repositories;
    using System.Text.Json;

    /// <summary>
    /// Controller class for Regions.
    /// </summary>
    // https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionsController"/> class.
        /// </summary>
        /// <param name="dbContext">DatabseContext.</param>
        /// <param name="regionRepository">Interface Region Repository.</param>
        /// <param name="mapper">Mapper for data.</param>
        /// <returns>Assign the correspondent value.</returns>
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAll action method was invoked");
            logger.LogWarning("This is a warning log");
            logger.LogError("This is a error log");

            // GET DATA FRIN DATABASE - DOMAIN MODELS
            var regionsDomain = await regionRepository.GetAllAsync();
            this.logger.LogInformation($"Finished GetAll request with data: {JsonSerializer.Serialize(regionsDomain)}");


            // MAP DOMAIN MODELS TO DTOs
            // var regionsDto = new List<RegionDto>();
            // foreach (var regionDomain in regionsDomain)
            // {
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl,
            //    });
            // }

            // RETURN DTOs
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        // GET SINGLE REGION BY ID
        // GET: HTTPS://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            // Get region domain model from database
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            // Return DTO back to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        // POST To Create New Region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                // use domain model to create region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                // map domain model back to dto
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // update region
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
                // Map DTO to Domain Model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                // check if region exist
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                // convert domain model to dto
                return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        // delete region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // return deleted region back
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
