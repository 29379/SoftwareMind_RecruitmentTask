using HotDeskBookingSystem.Controllers;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotDeskBookingSystem.Tests
{
    public class OfficeFloorControllerTests
    {
        private Mock<IOfficeFloorRepository> _repositoryMock;
        private OfficeFloorController _controller;

        public OfficeFloorControllerTests()
        {
            _repositoryMock = new Mock<IOfficeFloorRepository>();
            _controller = new OfficeFloorController(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetOfficeFloorByIdAsync_Test()
        {
            int floorId = 1;
            var expectedFloor = new OfficeFloor { OfficeFloorId = floorId };
            _repositoryMock
                .Setup(r => r.GetOfficeFloorByIdAsync(floorId))
                .ReturnsAsync(expectedFloor);

            var result = await _controller.GetOfficeFloorByIdAsync(floorId);
            var okResult = result as OkObjectResult;    // successful http response
            
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedFloor, okResult.Value);
        }

    }
}
