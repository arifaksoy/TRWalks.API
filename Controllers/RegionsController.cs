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
        public IActionResult GetAll()
        {
            //Get Data From Database to DTOs

            var regionsDomain = dbContext.Regions.ToList();

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

        public IActionResult GetById(Guid id) 
        {
           // Get Data From Database to DTOs

            var regionDomain = dbContext.Regions.FirstOrDefault(r => r.Id == id);

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
    }
}
