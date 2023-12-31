﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using TRWalks.API.Data;
using TRWalks.API.Models.Domain;
using TRWalks.API.Models.DTO;

namespace TRWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
       private readonly TRWalksDbContext dbContext;
        public RegionsController(TRWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data From Database to DTOs

            var regionsDomain = await dbContext.Regions.ToListAsync();

            //Map domain models to DTOS

            var regionsDTO = new List<RegionDto>();

            foreach (var regionDomain in regionsDomain)
            {
                regionsDTO.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }


            //Return DTOs

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetById(Guid id) 
        {
           // Get Data From Database to DTOs

            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map domain model to DTO

            var regionDto = new RegionDto()
            { 
                Id=regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            //return domain
            return Ok(regionDto);
        }

        //Post To Create new Region
        //Post:https://localhost:portnumber/api/regions
        
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto) 
        {
            //Map or Convert Dto to Domain Model

            var regionDomainModel = new Region 
            {
                Name = addRegionRequestDto.Name,
                Code = addRegionRequestDto.Code,    
                RegionImageUrl=addRegionRequestDto.RegionImageUrl
            };

            //Use Domain Model to create Region
           await dbContext.Regions.AddAsync(regionDomainModel);
           await dbContext.SaveChangesAsync();

            //Map Domainmodel back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id= regionDto.Id }, regionDto);
        }

        //Put to update region

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Get the region that will be updated
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
            
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //update the region

            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
           await dbContext.SaveChangesAsync();

            //map the domain model to dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(regionDto);

        }

        //delete region 

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id) 
        {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);  

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //delete region

            dbContext.Regions.Remove(regionDomainModel);
            await dbContext.SaveChangesAsync();

            //return deleted region back
            //map domain to DTO

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            return Ok(regionDto);
        }
    }
}
