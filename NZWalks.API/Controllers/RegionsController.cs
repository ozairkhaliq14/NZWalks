using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _db;

        public RegionsController(NZWalksDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get data from DB - Domain Models
            var regionsDomain = _db.Regions.ToList();

            //Map domain models to DTOs
            var regionsDto = new List<RegionDto>();
            foreach(var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id= regionDomain.Id,
                    Name= regionDomain.Name,
                    Code= regionDomain.Code,
                    RegionImageUrl= regionDomain.RegionImageUrl
                });
            }

            //return DTOs
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute]Guid id)
        {
            //Get Region from DB
            //var region = _db.Regions.FirstOrDefault(u => u.Id == id);
            var regionDomain = _db.Regions.Find(id);

            

            
            //return region DTO
            if(regionDomain == null)
            {
                return NotFound();
            }

            //Map Domain models to DTOs
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };


            return Ok(regionDto);
        }

        //POST to create new region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //map or convert dto to domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            //use domain model to create region
            _db.Regions.Add(regionDomainModel);
            _db.SaveChanges();

            //map domain model back to dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new {id = regionDto.Id}, regionDto);

        }


        //Update Region
        // Put: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id
            ,[FromBody] UpdateRegionRequest updateRegionRequest)
        {

            //Get existing Domain Model from Database
            var regionDomainModel = _db.Regions.Find(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            regionDomainModel.Name = updateRegionRequest.Name;
            regionDomainModel.Code = updateRegionRequest.Code;
            regionDomainModel.RegionImageUrl = updateRegionRequest.RegionImageUrl;

            _db.Regions.Update(regionDomainModel);
            _db.SaveChanges();
            

            //map domain model back to dto
            var regionDto = new RegionDto
            {
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }


        //Delete Region
        // Delete: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = _db.Regions.Find(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            _db.Regions.Remove(regionDomainModel);
            _db.SaveChanges();

            // optional if you want to return deleted region back
            // convert domain region to dto region

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
