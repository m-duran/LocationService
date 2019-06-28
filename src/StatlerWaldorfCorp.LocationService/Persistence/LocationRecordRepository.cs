using System;
using System.Linq;
using System.Collections.Generic;
using StatlerWaldorfCorp.LocationService.Models;
using Microsoft.EntityFrameworkCore;

namespace StatlerWaldorfCorp.LocationService.Persistence
{
    public class LocationRecordRepository : ILocationRecordRepository
    {
        private LocationDbContext context;

        public LocationRecordRepository(LocationDbContext context)
        {
            this.context = context;
        }

        public LocationRecord Add(LocationRecord locationRecord)
        {
            context.Add(locationRecord);
            context.SaveChanges();
            return locationRecord;
        }

        public LocationRecord Update(LocationRecord locationRecord)
        {
            context.Entry(locationRecord).State = EntityState.Modified;
            context.SaveChanges();
            return locationRecord;
        }

        public LocationRecord Get(Guid memberId, Guid recordId)
        {
            return context.LocationRecords.Single(lr => lr.MemberId == memberId && lr.Id == recordId);
        }

        public LocationRecord Delete(Guid memberId, Guid recordId)
        {
            LocationRecord locationRecord = Get(memberId, recordId);
            context.Remove(locationRecord);
            context.SaveChanges();
            return locationRecord;
        }

        public LocationRecord GetLatestForMember(Guid memberId)
        {
            LocationRecord locationRecord = context.LocationRecords
                .Where(lr => lr.MemberId == memberId)
                .OrderBy(lr => lr.Timestamp)
                .Last();
            return locationRecord;
        }

        public IEnumerable<LocationRecord> AllForMember(Guid memberId)
        {
            return context.LocationRecords.Where(lr => lr.MemberId == memberId)
                .OrderBy(lr => lr.Timestamp)
                .ToList();
        }
    }
}