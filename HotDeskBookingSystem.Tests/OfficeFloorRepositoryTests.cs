using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces.Repositories;
using Moq;

namespace HotDeskBookingSystem.Tests
{
    public class OfficeFloorRepositoryTests
    {
        private readonly Mock<IOfficeFloorRepository> _repositoryMock;

        public OfficeFloorRepositoryTests()
        {
            _repositoryMock = new Mock<IOfficeFloorRepository>();
        }

        [Fact]
        public async Task GetOfficeFloorByIdAsync_Test()
        {
            int floorId = 1;
            var expectedFloor = new OfficeFloor { OfficeFloorId = floorId };

            _repositoryMock
                .Setup(repo => repo.GetOfficeFloorByIdAsync(floorId))
                .ReturnsAsync(expectedFloor);

            var result = await _repositoryMock.Object.GetOfficeFloorByIdAsync(floorId);

            Assert.NotNull(result);
            Assert.Equal(floorId, result.OfficeFloorId);
        }
    }
}
