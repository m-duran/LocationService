
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StatlerWaldorfCorp.LocationService.Models;

namespace StatlerWaldorfCorp.LocationService.Controllers
{
    [Route("locations/{memberId}")]
    [ApiController]
    public class LocationRecordController : ControllerBase
    {
        private ILocationRecordRepository locationRepository;

        public LocationRecordController(ILocationRecordRepository repository)
        {
            locationRepository = repository;
        }

        [HttpPost]
        public ActionResult<LocationRecord> AddLocation(Guid memberId, LocationRecord locationRecord)
        {
            locationRepository.Add(locationRecord);
            return Created($"locations/{memberId}/{locationRecord.Id}", locationRecord);
        }

        [HttpGet]
        public ActionResult<IEnumerable<LocationRecord>> GetLocationsForMember(Guid memberId)
        {
            return locationRepository.AllForMember(memberId).ToList();
        }

        [HttpGet("latest")]
        public ActionResult<LocationRecord> GetLatestForMember(Guid memberId)
        {
            return locationRepository.GetLatestForMember(memberId);
        }
    }
}