using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Rate;
using Entities.DataTransferObjects.Rent;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleFakeRent.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;
namespace FakeRentTests
{
    public class RentControllerTests
    {
        private readonly RentController _sut;
        private readonly Mock<IRepositoryWrapper> mockRepo = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
        private readonly Mock<IRentRepository> _rentRepoMock = new Mock<IRentRepository>();

        public RentControllerTests()
        {
            _sut = new RentController(mockRepo.Object, mockMapper.Object, mockLogger.Object);
        }
        [Fact]
        public async Task GetAllRentsShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 1,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = DateTime.Today,
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetAllRents()).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetAllRents();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllRentsShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 1,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = DateTime.Today,
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetAllRents()).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetAllRents();

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned all rows from table Rent."), Times.Once);
        }
        [Fact]
        public async Task GetAllRentsShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetAllRents()).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllRents();

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetAllRentsShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetAllRents()).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllRents();

            // Assert
            mockLogger.Verify(x => x.LogError("Rents, have not been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetAllRentsExceptionCatch()
        {
            // Arrange
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetAllRents()).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetAllRents();

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAllRents() action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetRentByIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var id = 1;
            var rent = new Rent()
            {
                Id = id,
                PropertyId = 1,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = DateTime.Today,
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentById(id)).ReturnsAsync(rent );

            // Act
            var result = await _sut.GetRentById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentByIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var id = 1;
            var rent = new Rent()
            {
                Id = id,
                PropertyId = 1,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = DateTime.Today,
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentById(id)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentById(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rent with id: 1"), Times.Once);
        }
        [Fact]
        public async Task GetRentByIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentByIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentById(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with id: 1, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentByIdExceptionCatch()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentById(id)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentById(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentById(rent_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetRentsByTenantIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var tenantId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = DateTime.Today,
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantId(tenantId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsByTenantId(tenantId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentsByTenantIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var tenantId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = DateTime.Today,
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantId(tenantId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rents with tenant_id: 5, Rents by tenant_id count: $1"), Times.Once);
        }
        [Fact]
        public async Task GetRentsByTenantIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantId(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsByTenantId(tenantId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentsByTenantIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantId(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with TenantId: 5, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentsByTenantIdExceptionCatch()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantId(tenantId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentsByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside RentByTenantId(tenant_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task RentsByPropertyIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var propertyId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = propertyId,
                TenantId = 3,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = DateTime.Today,
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByPropertyId(propertyId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsByPropertyId(propertyId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task RentsByPropertyIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var propertyId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = propertyId,
                TenantId = 3,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = DateTime.Today,
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByPropertyId(propertyId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsByPropertyId(propertyId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rents with property_id: 5, Rents by property_id count: $1"), Times.Once);
        }
        [Fact]
        public async Task RentsByPropertyIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var propertyId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByPropertyId(propertyId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsByPropertyId(propertyId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task RentsByPropertyIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var propertyId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByPropertyId(propertyId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsByPropertyId(propertyId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with PropertyId: 5, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task RentsByPropertyIdExceptionCatch()
        {
            // Arrange
            var propertyId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByPropertyId(propertyId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentsByPropertyId(propertyId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentsByPropertyId(property_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
    
        [Fact]
        public async Task GetRentsByTenantIdBetweenDatesShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var tenantId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = DateTime.Today,
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantIdBetweenDates(tenantId, new DateTime(2016 - 01 - 01), new DateTime(2020 - 04 - 22))).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsByTenantIdBetweenDates(tenantId, new DateTime(2016 - 01 - 01), new DateTime(2020 - 04 - 22));

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentsByTenantIdBetweenDatesShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var tenantId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = DateTime.Today,
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantIdBetweenDates(tenantId, new DateTime(2016 - 01 - 01), new DateTime(2020 - 04 - 22))).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsByTenantIdBetweenDates(tenantId, new DateTime(2016 - 01 - 01), new DateTime(2020 - 04 - 22));

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rents with tenant_id: 5, Rents between dates count: $1"), Times.Once);
        }
        [Fact]
        public async Task GetRentsByTenantIdBetweenDatesShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantIdBetweenDates(tenantId, new DateTime(2016 - 01 - 01), new DateTime(2020 - 04 - 22))).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsByTenantIdBetweenDates(tenantId, new DateTime(2016 - 01 - 01), new DateTime(2020 - 04 - 22));

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentsByTenantIdBetweenDatesShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantIdBetweenDates(tenantId, new DateTime(2016 - 01 - 01), new DateTime(2020 - 04 - 22))).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsByTenantIdBetweenDates(tenantId, new DateTime(2016 - 01 - 01), new DateTime(2020 - 04 - 22));

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with TenantId: 5, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentsByTenantIdBetweenDatesExceptionCatch()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantIdBetweenDates(tenantId, new DateTime(2016 - 01 - 01), new DateTime(2020 - 04 - 22))).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentsByTenantIdBetweenDates(tenantId, new DateTime(2016 - 01 - 01), new DateTime(2020 - 04 - 22));

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentsByTenantIdBetweenDates(tenant_id, startRentDate, endRentDate) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetRentsThatAreOngoingByTenantIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var tenantId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateByTenantId(tenantId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsThatAreOngoingByTenantId(tenantId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentsThatAreOngoingByTenantIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var tenantId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateByTenantId(tenantId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsThatAreOngoingByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rents with tenant_id: 5, ongoing Rents count: $1"), Times.Once);
        }
        [Fact]
        public async Task GetRentsThatAreOngoingByTenantIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateByTenantId(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsThatAreOngoingByTenantId(tenantId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentsThatAreOngoingByTenantIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateByTenantId(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsThatAreOngoingByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with TenantId: 5, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentsThatAreOngoingByTenantIdExceptionCatch()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateByTenantId(tenantId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentsThatAreOngoingByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentsThatAreOngoingByTenantId(tenant_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }

             [Fact]
        public async Task GetRentsThatAreOngoingWithPropertyByTenantIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var tenantId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Property = new Property()
                {
                    Id = 1,
                    FlatLabel = "Apartament1",
                    RoomCount = 3,
                    FlatSize = 36,
                    HasGas = false,
                    HasHW = true,
                    HasHeat = false,
                    DisplayOnWeb = false,
                    LandlordId = 13,
                    AddressId = 1,
                }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateWithPropertyByTenantId(tenantId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsThatAreOngoingWithPropertyByTenantId(tenantId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentsThatAreOngoingWithPropertyByTenantIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var tenantId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Property = new Property()
                {
                    Id = 1,
                    FlatLabel = "Apartament1",
                    RoomCount = 3,
                    FlatSize = 36,
                    HasGas = false,
                    HasHW = true,
                    HasHeat = false,
                    DisplayOnWeb = false,
                    LandlordId = 13,
                    AddressId = 1,
                }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateWithPropertyByTenantId(tenantId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsThatAreOngoingWithPropertyByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rents with related Property by tenant_id: 5, ongoing Rents count: 1"), Times.Once);
        }
        [Fact]
        public async Task GetRentsThatAreOngoingWithPropertyByTenantIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateWithPropertyByTenantId(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsThatAreOngoingWithPropertyByTenantId(tenantId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentsThatAreOngoingWithPropertyByTenantIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateWithPropertyByTenantId(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsThatAreOngoingWithPropertyByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with TenantId: 5, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentsThatAreOngoingWithPropertyByTenantIdExceptionCatch()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateWithPropertyByTenantId(tenantId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentsThatAreOngoingWithPropertyByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentsThatAreOngoingWithPropertyByTenantId(tenant_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLimitedRentsThatAreOngoingByTenantIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var tenantId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2021 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Property = new Property()
                {
                    Id = 1,
                    FlatLabel = "Apartament1",
                    RoomCount = 3,
                    FlatSize = 36,
                    HasGas = false,
                    HasHW = true,
                    HasHeat = false,
                    DisplayOnWeb = false,
                    LandlordId = 13,
                    AddressId = 1,
                }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateByTenantId(tenantId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetLimitedRentsThatAreOngoingByTenantId(tenantId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetLimitedRentsThatAreOngoingByTenantIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var tenantId = 5;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2021 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Property = new Property()
                {
                    Id = 1,
                    FlatLabel = "Apartament1",
                    RoomCount = 3,
                    FlatSize = 36,
                    HasGas = false,
                    HasHW = true,
                    HasHeat = false,
                    DisplayOnWeb = false,
                    LandlordId = 13,
                    AddressId = 1,
                }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateByTenantId(tenantId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetLimitedRentsThatAreOngoingByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rents with tenant_id: 5, ongoing Rents count: $1"), Times.Once);
        }
        [Fact]
        public async Task GetLimitedRentsThatAreOngoingByTenantIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateByTenantId(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLimitedRentsThatAreOngoingByTenantId(tenantId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetLimitedRentsThatAreOngoingByTenantIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateByTenantId(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLimitedRentsThatAreOngoingByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with TenantId: 5, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetLimitedRentsThatAreOngoingByTenantIdExceptionCatch()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsThatHaveEndDateBeforeActualDateByTenantId(tenantId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLimitedRentsThatAreOngoingByTenantId(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLimitedRentsThatAreOngoingByTenantId(tenant_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetRentsByTenantIdThatAreFinishedShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var tenantId = 3;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2018- 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantIdThatAreFinished(tenantId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsByTenantIdThatAreFinished(tenantId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentsByTenantIdThatAreFinishedShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var tenantId = 3;
            var rent = new Rent()
            {
                Id = 1,
                PropertyId = 2,
                TenantId = tenantId,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2018 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantIdThatAreFinished(tenantId)).ReturnsAsync(new[] { rent });

            // Act
            var result = await _sut.GetRentsByTenantIdThatAreFinished(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rents with tenant_id: 3, finished Rents count: 1"), Times.Once);
        }
        [Fact]
        public async Task GetRentsByTenantIdThatAreFinishedShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var tenantId = 5;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantIdThatAreFinished(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsByTenantIdThatAreFinished(tenantId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentsByTenantIdThatAreFinishedShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var tenantId = 3;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantIdThatAreFinished(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentsByTenantIdThatAreFinished(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with TenantId: 3, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentsByTenantIdThatAreFinishedExceptionCatch()
        {
            // Arrange
            var tenantId = 3;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentsByTenantIdThatAreFinished(tenantId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentsByTenantIdThatAreFinished(tenantId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentsByTenantIdThatAreFinished(tenant_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetRentWithPropertyByRentIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var rentId = 3;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2020 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Property = new Property()
                {
                    Id = 1,
                    FlatLabel = "Apartament1",
                    RoomCount = 3,
                    FlatSize = 36,
                    HasGas = false,
                    HasHW = true,
                    HasHeat = false,
                    DisplayOnWeb = false,
                    LandlordId = 4
                }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithProperty(rentId)).ReturnsAsync( rent );

            // Act
            var result = await _sut.GetRentWithPropertyByRentId(rentId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentWithPropertyByRentIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var rentId = 3;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2020 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Property = new Property()
                {
                    Id = 1,
                    FlatLabel = "Apartament1",
                    RoomCount = 3,
                    FlatSize = 36,
                    HasGas = false,
                    HasHW = true,
                    HasHeat = false,
                    DisplayOnWeb = false,
                    LandlordId = 4
                }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithProperty(rentId)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentWithPropertyByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rent with id: 3 with related property."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithPropertyByRentIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var rentId = 3;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithProperty(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithPropertyByRentId(rentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentWithPropertyByRentIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var rentId = 3;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithProperty(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithPropertyByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with Id: 3, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithPropertyByRentIdExceptionCatch()
        {
            // Arrange
            var rentId = 3;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithProperty(rentId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentWithPropertyByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentWithPropertyByRentId(rent_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetRentWithStatesByRentIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var rentId = 3;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                States = new[] {new State()
                {
                    Id = 13,
                    ColdWaterState = 110,
                    HotWaterState = 0,
                    GasState = 110,
                    EnergyState = 110,
                    HeatState = 0,
                    RentId = rentId

                }
            }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithStates(rentId)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentWithStatesByRentId(rentId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentWithStatesByRentIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var rentId = 3;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                States = new[] {new State()
                {
                    Id = 13,
                    ColdWaterState = 110,
                    HotWaterState = 0,
                    GasState = 110,
                    EnergyState = 110,
                    HeatState = 0,
                    RentId = rentId

                }
            }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithStates(rentId)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentWithStatesByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rent with id: 3 and related states, states count: 1."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithStatesByRentIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var rentId = 3;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithStates(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithStatesByRentId(rentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentWithStatesByRentIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var rentId = 3;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithStates(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithStatesByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with id: 3, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithStatesByRentIdExceptionCatch()
        {
            // Arrange
            var rentId = 3;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithStates(rentId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentWithStatesByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentWithStatesByRentId(rent_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetRentWithInvoicesByRentIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var rentId = 3;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Invoices = new[] {new Invoice()
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
                ColdWaterConsumption = 10,
                HotWaterConsumption = 0,
                GasConsumption = 1,
                EnergyConsumption = 10,
                HeatConsumption = 0,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                LandlordRentVAT = 0,
                HousingRentVAT = 23,
                WaterVAT = 23,
                GasVAT = 23,
                EnergyVAT = 23,
                RentId = rentId

            } }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoices(rentId)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentWithInvoicesByRentId(rentId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentWithInvoicesByRentIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var rentId = 3;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Invoices = new[] {new Invoice()
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
                ColdWaterConsumption = 10,
                HotWaterConsumption = 0,
                GasConsumption = 1,
                EnergyConsumption = 10,
                HeatConsumption = 0,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                LandlordRentVAT = 0,
                HousingRentVAT = 23,
                WaterVAT = 23,
                GasVAT = 23,
                EnergyVAT = 23,
                RentId = rentId

            } }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoices(rentId)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentWithInvoicesByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rent with id: 3 and related invoices, invoices count: $1."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithInvoicesByRentIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var rentId = 6;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoices(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithInvoicesByRentId(rentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentWithInvoicesByRentIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var rentId = 6;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoices(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithInvoicesByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with id: 6, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithInvoicesByRentIdExceptionCatch()
        {
            // Arrange
            var rentId = 6;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoices(rentId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentWithInvoicesByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentWithInvoicesByRentId(rent_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task GetRentWithLandlordByRentIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var rentId = 9;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Landlord = new Landlord()
                {
                    Id = 1,
                    Name = "Janusz",
                    Surname = "Kiełbasa",
                    IsCompany = false,
                    IsVATPayer = false,
                    PhoneNumber = "948387397",
                    BankAccount = "67843186627845100000123456"
                }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithLandlord(rentId)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentWithLandlordByRentId(rentId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentWithLandlordByRentIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var rentId = 9;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Landlord = new Landlord()
                {
                    Id = 1,
                    Name = "Janusz",
                    Surname = "Kiełbasa",
                    IsCompany = false,
                    IsVATPayer = false,
                    PhoneNumber = "948387397",
                    BankAccount = "67843186627845100000123456"
                }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithLandlord(rentId)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentWithLandlordByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rent with id: 9 with related landlord."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithLandlordByRentIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var rentId = 9;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithLandlord(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithLandlordByRentId(rentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentWithLandlordByRentIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var rentId = 9;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithLandlord(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithLandlordByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with id: 9, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithLandlordByRentIdExceptionCatch()
        {
            // Arrange
            var rentId = 6;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithLandlord(rentId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentWithLandlordByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentWithLandlordByRentId(rent_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task GetRentWithTenantByRentIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var rentId = 2;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Tenant = new Tenant()
                {
                    Id = 4,
                    NIP = "9876500011",
                    PESEL = "09876500011",
                    IsCompany = true,
                    CompanyName = "Szlachta nie Pracuje",
                    PhonePrefix = "0048",
                    PhoneNumber = "888031303",
                    Name = "Janusz",
                    Surname = "Kowalski",
                    DisplayOnWeb = true,
                    AspNetUsersId = 69,
                }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithTenant(rentId)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentWithTenantByRentId(rentId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentWithTenantByRentIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var rentId = 2;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Tenant = new Tenant()
                {
                    Id = 4,
                    NIP = "9876500011",
                    PESEL = "09876500011",
                    IsCompany = true,
                    CompanyName = "Szlachta nie Pracuje",
                    PhonePrefix = "0048",
                    PhoneNumber = "888031303",
                    Name = "Janusz",
                    Surname = "Kowalski",
                    DisplayOnWeb = true,
                    AspNetUsersId = 69,
                }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithTenant(rentId)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentWithTenantByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rent with id: 2 with related tenant."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithTenantByRentIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var rentId = 2;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithTenant(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithTenantByRentId(rentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentWithTenantByRentIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var rentId = 2;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithTenant(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithTenantByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with id: 2, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithTenantByRentIdExceptionCatch()
        {
            // Arrange
            var rentId = 2;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithTenant(rentId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentWithTenantByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentWithTenantByRentId(rent_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetRentWithInvoiceAndStateByRentIdShouldReturnOKIfRentsInDb()
        {
            // Arrange
            var rentId = 7;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Invoices = new [] {new Invoice(), new Invoice()},
                States = new[] {new State(), new State(), new State()}
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoiceAndStates(rentId)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentWithInvoiceAndStateByRentId(rentId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRentWithInvoiceAndStateByRentIdShouldLogAppropriateMessageWhenRentsInDatabase()
        {
            // Arrange
            var rentId = 7;
            var rent = new Rent()
            {
                Id = rentId,
                PropertyId = 2,
                TenantId = 4,
                LandlordId = 4,
                RentPurpose = "work",
                StartRent = new DateTime(2017 - 02 - 01),
                EndRent = new DateTime(2022 - 02 - 01),
                TenantCount = 3,
                RentDeposit = 1500,
                PayDayDelay = 7,
                SendStateDay = 12,
                DisplayOnWeb = false,
                PhotoRequired = false,
                Invoices = new[] { new Invoice(), new Invoice() },
                States = new[] { new State(), new State(), new State() }
            };

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoiceAndStates(rentId)).ReturnsAsync(rent);

            // Act
            var result = await _sut.GetRentWithInvoiceAndStateByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Rent with id: 7 with related invoices and states."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithInvoiceAndStateByRentIdShouldReturnNotFoundIfNorentsInDB()
        {
            // Arrange
            var rentId = 7;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoiceAndStates(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithInvoiceAndStateByRentId(rentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetRentWithInvoiceAndStateByRentIdShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange
            var rentId = 7;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoiceAndStates(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetRentWithInvoiceAndStateByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with id: 7, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetRentWithInvoiceAndStateByRentIdExceptionCatch()
        {
            // Arrange
            var rentId = 7;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoiceAndStates(rentId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRentWithInvoiceAndStateByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRentWithInvoiceAndStateByRentId(rent_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreateRentExceptionCatch()
        {
            // Arrange

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.CreateRent(new Rent())).Throws(new Exception());

            // Act
            var result = await _sut.CreateRent(new RentForCreationDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside CreateRent(RentForCreationDto) action: Object reference not set to an instance of an object."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreateRentShouldReturnBadRequestIfRentIsNull()
        {
            // Arrange


            // Act
            var result = await _sut.CreateRent(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
        [Fact]
        public async Task CreateRentShouldLogAppropriateMessageIfRentIsNull()
        {
            // Arrange



            // Act
            var result = await _sut.CreateRent(null);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task UpdateRentExceptionCatch()
        {
            /// Arrange
            var id = 45;

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentById(id)).Throws(new Exception());

            // Act
            var result = await _sut.UpdateRate(id, new RentForUpdateDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside UpdateRent(id, RentForUpdateDto) action: Object reference not set to an instance of an object."), Times.Once);
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
            mockLogger.Verify(x => x.LogError("Rent received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task DeleteRentExceptionCatch()
        {
            /// Arrange
            var id = 45;
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoiceAndStates(id)).Throws(new Exception());

            // Act
            var result = await _sut.DeleteRent(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside DeleteRent(rent_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task DeleteRentShouldLogAppropriateMessageWhenNoRentsInDatabase()
        {
            // Arrange

            var id = 45;

            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithInvoiceAndStates(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.DeleteRent(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Not deleted. Rent with given id: 45 was not found"), Times.Once);
        }
    }
}
