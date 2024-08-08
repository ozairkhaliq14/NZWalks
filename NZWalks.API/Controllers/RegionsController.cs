using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _db;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(NZWalksDbContext db, IRegionRepository regionRepository,
            IMapper mapper)
        {
            _db = db;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        // Get Region
        // Get all Regions from Database
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from DB - Domain Models
            var regionsDomain = await _regionRepository.GetAllAsync();

            //Map domain models to DTOs
            //return DTOs
            return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
        }

        // GetById Region
        // Get one Region by id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            //Get Region from DB
            //var region = _db.Regions.FirstOrDefault(u => u.Id == id);
            var regionDomain = await _regionRepository.GetByIdAsync(id);

            
            if(regionDomain == null)
            {
                return NotFound();
            }
            //return region DTO
            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }

        // Post Region
        //POST to create new region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //map or convert dto to domain model
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

            //use domain model to create region
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

            //map domain model back to dto
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new {id = regionDto.Id}, regionDto);

        }


        // Update Region
        // Put: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id
            ,[FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);

            //Get existing Domain Model from Database
            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //map domain model back to dto
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }


        //Delete Region
        // Delete: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // convert domain region to dto region
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
