using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Rate;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleFakeRent.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FakeRentTests
{
    public class RateControllerTests
    {
        private readonly RateController _sut;
        private readonly Mock<IRepositoryWrapper> mockRepo = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
        private readonly Mock<IRateRepository> _rateRepoMock = new Mock<IRateRepository>();

        public RateControllerTests()
        {
            _sut = new RateController(mockRepo.Object, mockMapper.Object, mockLogger.Object);
        }
        [Fact]
        public async Task GetAllRatesShouldReturnOKIfRatesInDb()
        {
            // Arrange
            var rate = new Rate()
            {
                Id = 1,
                LandlordRent = 1000,
                HousingRent = 350,
                ColdWaterPrice = 10,
                HotWaterPrice = 30,
                GasPrice = 3,
                EnergyPrice = 1,
                HeatPrice = 70,
                GasSubscription = 12,
                EnergySubscription = 7,
                HeatSubscription = 10,
                LandlordRentVAT = 0,
                HousingRentVAT = 23,
                WaterVAT = 23,
                GasVAT = 23,
                EnergyVAT = 23,
                PropertyId = 3
            };

            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetAllRates()).ReturnsAsync(new[] { rate });

            // Act
            var result = await _sut.GetAllRates();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetAllRatesShouldLogAppropriateMessageWhenRatesInDatabase()
        {
            // Arrange
            var rate = new Rate()
            {
                Id = 1,
                LandlordRent = 1000,
                HousingRent = 350,
                ColdWaterPrice = 10,
                HotWaterPrice = 30,
                GasPrice = 3,
                EnergyPrice = 1,
                HeatPrice = 70,
                GasSubscription = 12,
                EnergySubscription = 7,
                HeatSubscription = 10,
                LandlordRentVAT = 0,
                HousingRentVAT = 23,
                WaterVAT = 23,
                GasVAT = 23,
                EnergyVAT = 23,
                PropertyId = 3
            };

            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetAllRates()).ReturnsAsync(new[] { rate });

            // Act
            var result = await _sut.GetAllRates();

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned all rows from table Rate."), Times.Once);
        }

        [Fact]
        public async Task GetAllRatesShouldReturnNotFoundIfNoRatesInDB()
        {
            // Arrange
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetAllRates()).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllRates();

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetAllRatesShouldLogAppropriateMessageWhenNoRatesInDatabase()
        {
            // Arrange
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetAllRates()).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllRates();

            // Assert
            mockLogger.Verify(x => x.LogError("Rates have not been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetAllRatesExceptionCatch()
        {
            // Arrange
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetAllRates()).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetAllRates();

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAllRates() action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetRateByIdShouldReturnOKIfRateInDb()
        {
            // Arrange
            var id = 4;
            var rate = new Rate()
            {
                Id = id,
                LandlordRent = 1000,
                HousingRent = 350,
                ColdWaterPrice = 10,
                HotWaterPrice = 30,
                GasPrice = 3,
                EnergyPrice = 1,
                HeatPrice = 70,
                GasSubscription = 12,
                EnergySubscription = 7,
                HeatSubscription = 10,
                LandlordRentVAT = 0,
                HousingRentVAT = 23,
                WaterVAT = 23,
                GasVAT = 23,
                EnergyVAT = 23,
                PropertyId = 3
            };

            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateById(id)).ReturnsAsync( rate );

            // Act
            var result = await _sut.GetRateById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetRateByIdShouldLogAppropriateMessageWhenRateInDatabase()
        {
            // Arrange
            var id = 4;
            var rate = new Rate()
            {
                Id = id,
                LandlordRent = 1000,
                HousingRent = 350,
                ColdWaterPrice = 10,
                HotWaterPrice = 30,
                GasPrice = 3,
                EnergyPrice = 1,
                HeatPrice = 70,
                GasSubscription = 12,
                EnergySubscription = 7,
                HeatSubscription = 10,
                LandlordRentVAT = 0,
                HousingRentVAT = 23,
                WaterVAT = 23,
                GasVAT = 23,
                EnergyVAT = 23,
                PropertyId = 3
            };

            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateById(id)).ReturnsAsync(rate);

            // Act
            var result = await _sut.GetRateById(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rate with id: 4"), Times.Once);
        }
        [Fact]
        public async Task GetRateByIdShouldReturnNotFoundIfNoRatesInDB()
        {
            // Arrange
            var id = 4;
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRateById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRateByIdShouldLogAppropriateMessageWhenNoRatesInDatabase()
        {
            // Arrange
            var id = 4;
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRateById(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Rate with id: 4, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRateByIdExceptionCatch()
        {
            // Arrange
            var id = 4;
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateById(id)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRateById(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRateById(rate_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetRateByPropertyIdShouldReturnOKIfRateInDb()
        {
            // Arrange
            var propertyId = 4;
            var rate = new Rate()
            {
                Id = 4,
                LandlordRent = 1000,
                HousingRent = 350,
                ColdWaterPrice = 10,
                HotWaterPrice = 30,
                GasPrice = 3,
                EnergyPrice = 1,
                HeatPrice = 70,
                GasSubscription = 12,
                EnergySubscription = 7,
                HeatSubscription = 10,
                LandlordRentVAT = 0,
                HousingRentVAT = 23,
                WaterVAT = 23,
                GasVAT = 23,
                EnergyVAT = 23,
                PropertyId = propertyId
            };

            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateByPropertyId(propertyId)).ReturnsAsync(rate);

            // Act
            var result = await _sut.GetRateByPropertyId(propertyId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetRateByPropertyIdShouldLogAppropriateMessageWhenRateInDatabase()
        {
            // Arrange
            var propertyId = 4;
            var rate = new Rate()
            {
                Id = 4,
                LandlordRent = 1000,
                HousingRent = 350,
                ColdWaterPrice = 10,
                HotWaterPrice = 30,
                GasPrice = 3,
                EnergyPrice = 1,
                HeatPrice = 70,
                GasSubscription = 12,
                EnergySubscription = 7,
                HeatSubscription = 10,
                LandlordRentVAT = 0,
                HousingRentVAT = 23,
                WaterVAT = 23,
                GasVAT = 23,
                EnergyVAT = 23,
                PropertyId = propertyId
            };

            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateByPropertyId(propertyId)).ReturnsAsync(rate);

            // Act
            var result = await _sut.GetRateByPropertyId(propertyId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rate with PropertyId: 4"), Times.Once);
        }
        [Fact]
        public async Task GetRateByPropertyIdShouldReturnNotFoundIfNoRatesInDB()
        {
            // Arrange
            var propertyId = 4;
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateByPropertyId(propertyId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRateByPropertyId(propertyId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRateByPropertyIdShouldLogAppropriateMessageWhenNoRatesInDatabase()
        {
            // Arrange
            var propertyId = 4;
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateByPropertyId(propertyId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRateByPropertyId(propertyId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rate with PropertyId: 4, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRateByPropertyIdExceptionCatch()
        {
            // Arrange
            var propertyId = 4;
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateByPropertyId(propertyId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRateByPropertyId(propertyId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRateByPropertyId(property_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetRateWithPropertyByRateIdShouldReturnOKIfRateInDb()
        {
            // Arrange
            var id = 3;
            var rate = new Rate()
            {
                Id = id,
                LandlordRent = 1000,
                HousingRent = 350,
                ColdWaterPrice = 10,
                HotWaterPrice = 30,
                GasPrice = 3,
                EnergyPrice = 1,
                HeatPrice = 70,
                GasSubscription = 12,
                EnergySubscription = 7,
                HeatSubscription = 10,
                LandlordRentVAT = 0,
                HousingRentVAT = 23,
                WaterVAT = 23,
                GasVAT = 23,
                EnergyVAT = 23,
                PropertyId = 4,
                Property = new Property()
                {
                    Id = 4,
                    FlatLabel = "Apartament1",
                    RoomCount = 3,
                    FlatSize = 36,
                    HasGas = false,
                    HasHW = true,
                    HasHeat = false,
                    DisplayOnWeb = false,
                    LandlordId = 13,
                    AddressId = 1                
                }
            };

            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateWithProperty(id)).ReturnsAsync( rate );

            // Act
            var result = await _sut.GetRateWithPropertyByRateId(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetRateWithPropertyByRateIdShouldLogAppropriateMessageWhenRateInDatabase()
        {
            // Arrange
            var id = 3;
            var rate = new Rate()
            {
                Id = id,
                LandlordRent = 1000,
                HousingRent = 350,
                ColdWaterPrice = 10,
                HotWaterPrice = 30,
                GasPrice = 3,
                EnergyPrice = 1,
                HeatPrice = 70,
                GasSubscription = 12,
                EnergySubscription = 7,
                HeatSubscription = 10,
                LandlordRentVAT = 0,
                HousingRentVAT = 23,
                WaterVAT = 23,
                GasVAT = 23,
                EnergyVAT = 23,
                PropertyId = 4,
                Property = new Property()
                {
                    Id = 4,
                    FlatLabel = "Apartament1",
                    RoomCount = 3,
                    FlatSize = 36,
                    HasGas = false,
                    HasHW = true,
                    HasHeat = false,
                    DisplayOnWeb = false,
                    LandlordId = 13,
                    AddressId = 1
                }
            };

            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateWithProperty(id)).ReturnsAsync(rate);

            // Act
            var result = await _sut.GetRateWithPropertyByRateId(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rate with id: 3 with related property."), Times.Once);
        }
        [Fact]
        public async Task GetRateWithPropertyByRateIdShouldReturnNotFoundIfNoRatesInDB()
        {
            // Arrange
            var id = 3;
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateWithProperty(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRateWithPropertyByRateId(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRateWithPropertyByRateIdShouldLogAppropriateMessageWhenNoRatesInDatabase()
        {
            // Arrange
            var id = 3;
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateWithProperty(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRateWithPropertyByRateId(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Rate with id: 3, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRateWithPropertyByRateIdExceptionCatch()
        {
            // Arrange
            var id = 3;
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateWithProperty(id)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRateWithPropertyByRateId(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRateWithProperty(rate_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreateRateExceptionCatch()
        {
            // Arrange

            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.CreateRate(new Rate())).Throws(new Exception());

            // Act
            var result = await _sut.CreateRate(new RateForCreationDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside CreateRate(RateForCreationDto) action: Object reference not set to an instance of an object."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreateRateShouldReturnBadRequestIfRateIsNull()
        {
            // Arrange


            // Act
            var result = await _sut.CreateRate(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
        [Fact]
        public async Task CreateAddressShouldLogAppropriateMessageIfRateIsNull()
        {
            // Arrange

            // Act
            var result = await _sut.CreateRate(null);

            // Assert
            mockLogger.Verify(x => x.LogError("Rate received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task UpdateRateExceptionCatch()
        {
            /// Arrange
            var id = 45;

            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateById(id)).Throws(new Exception());

            // Act
            var result = await _sut.UpdateRate(id, new RateForUpdateDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside UpdateRate(id, RateForUpdateDto) action: Object reference not set to an instance of an object."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task UpdateRateShouldReturnBadRequestIfRateIsNull()
        {
            /// Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdateRate(id, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateRateShouldLogAppropriateMessageIfRateIsNull()
        {
            // Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdateRate(id, null);

            // Assert
            mockLogger.Verify(x => x.LogError("Rate received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task DeletePropertyExceptionCatch()
        {
            /// Arrange
            var id = 45;
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateById(id)).Throws(new Exception());

            // Act
            var result = await _sut.DeleteRate(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside DeleteRate(rate_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task DeleteRateShouldLogAppropriateMessageWhenNoRatesInDatabase()
        {
            // Arrange

            var id = 45;

            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.DeleteRate(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Rate Id not found, cannot delete."), Times.Once);
        }
        [Fact]
        public async Task DeleteRateByPropertyIdShouldLogAppropriateMessageWhenNoRatesInDatabase()
        {
            /// Arrange
            var id = 45;
            mockRepo.Setup(x => x.Rate).Returns(_rateRepoMock.Object);
            _rateRepoMock.Setup(x => x.GetRateByPropertyId(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.DeleteRate(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Rate Id not found, cannot delete."), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
        }

      
    }
}
