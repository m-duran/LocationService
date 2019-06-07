using System;
using System.Collections.Generic;
using System.Linq;
using StatlerWaldorfCorp.LocationService.Controllers;
using StatlerWaldorfCorp.LocationService.Models;
using StatlerWaldorfCorp.LocationService.Persistence;
using Xunit;

namespace StatlerWaldorfCorp.LocationService.Tests
{
    public class LocationRecordControllerTest
    {
        [Fact]
        public void ShouldAdd()
        {
            ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
            LocationRecordController controller = new LocationRecordController(repository);
            Guid memberGuid = Guid.NewGuid();

            controller.AddLocation(memberGuid, new LocationRecord()
            {
                Id = Guid.NewGuid(),
                MemberId = memberGuid,
                Timestamp = 1
            });
            controller.AddLocation(memberGuid, new LocationRecord()
            {
                Id = Guid.NewGuid(),
                MemberId = memberGuid,
                Timestamp = 2
            });

            Assert.Equal(2, repository.AllForMember(memberGuid).Count());
        }

        [Fact]
        public void ShouldReturnEmtpyListForNewMember()
        {
            ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
            LocationRecordController controller = new LocationRecordController(repository);
            Guid memberGuid = Guid.NewGuid();

            IEnumerable<LocationRecord> locationRecords =
                controller.GetLocationsForMember(memberGuid).Value;

            Assert.Equal(0, locationRecords.Count());
        }

        [Fact]
        public void ShouldTrackAllLocationsForMember()
        {
            ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
            LocationRecordController controller = new LocationRecordController(repository);
            Guid memberGuid = Guid.NewGuid();

            controller.AddLocation(memberGuid, new LocationRecord()
            {
                Id = Guid.NewGuid(),
                Timestamp = 1,
                MemberId = memberGuid,
                Latitude = 12.3f
            });
            controller.AddLocation(memberGuid, new LocationRecord()
            {
                Id = Guid.NewGuid(),
                Timestamp = 2,
                MemberId = memberGuid,
                Latitude = 23.4f
            });
            controller.AddLocation(Guid.NewGuid(), new LocationRecord()
            {
                Id = Guid.NewGuid(),
                Timestamp = 3,
                MemberId = Guid.NewGuid(),
                Latitude = 23.4f
            });

            IEnumerable<LocationRecord> locationRecords =
                controller.GetLocationsForMember(memberGuid).Value;

            Assert.Equal(2, locationRecords.Count());
        }

        [Fact]
        public void ShouldTrackNullLatestForNewMember()
        {
            ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
            LocationRecordController controller = new LocationRecordController(repository);
            Guid memberGuid = Guid.NewGuid();

            LocationRecord latest = controller.GetLatestForMember(memberGuid).Value;

            Assert.Null(latest);
        }

        [Fact]
        public void ShouldTrackLatestLocationsForMember()
        {
            ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
            LocationRecordController controller = new LocationRecordController(repository);
            Guid memberGuid = Guid.NewGuid();

            Guid latestId = Guid.NewGuid();
            controller.AddLocation(memberGuid, new LocationRecord()
            {
                Id = Guid.NewGuid(),
                Timestamp = 1,
                MemberId = memberGuid,
                Latitude = 12.3f
            });
            controller.AddLocation(memberGuid, new LocationRecord()
            {
                Id = latestId,
                Timestamp = 3,
                MemberId = memberGuid,
                Latitude = 23.4f
            });
            controller.AddLocation(memberGuid, new LocationRecord()
            {
                Id = Guid.NewGuid(),
                Timestamp = 2,
                MemberId = memberGuid,
                Latitude = 23.4f
            });
            controller.AddLocation(Guid.NewGuid(), new LocationRecord()
            {
                Id = Guid.NewGuid(),
                Timestamp = 4,
                MemberId = Guid.NewGuid(),
                Latitude = 23.4f
            });

            LocationRecord latest = controller.GetLatestForMember(memberGuid).Value;

            Assert.NotNull(latest);
            Assert.Equal(latestId, latest.Id);
        }
    }
}
