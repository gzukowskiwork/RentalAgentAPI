using AutoMapper;
using Contracts;
using EmailService;
using Entities.DataTransferObjects.Photo;
using Entities.DataTransferObjects.Property;
using Entities.Models;
using Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleFakeRent.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;


namespace FakeRentTests
{
    public class PropertyControllerTests
    {
        private readonly PropertyController _sut;
        private readonly Mock<IRepositoryWrapper> mockRepo = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
        private readonly Mock<IPropertyRepository> _propertyRepoMock = new Mock<IPropertyRepository>();

        public PropertyControllerTests()
        {
            _sut = new PropertyController(mockRepo.Object, mockMapper.Object, mockLogger.Object);
        }


        [Fact]
        public async Task GetAllPropertiesShouldReturnOKIfPropertiesInDb()
        {
            // Arrange

            var property = new Property()
            {
                Id = 1,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false
            };


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetAllProperties()).ReturnsAsync(new[] { property });


            // Act
            var result = await _sut.GetAllProperties();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
            Assert.Equal(1, property.Id);
        }

        [Fact]
        public async Task GetAllPropertiesShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange

            var property = new Property()
            {
                Id = 1,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false
            };

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetAllProperties()).ReturnsAsync(new[] { property });

            // Act
            var result = await _sut.GetAllProperties();

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned all 'Properties' from db. Total number of Properties found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetAllPropertiesShouldReturnNotFoundIfNoProperties()
        {
            // Arrange

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetAllProperties()).ReturnsAsync(() => null);

           

                // Act
                var result = await _sut.GetAllProperties();

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllPropertiesShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetAllProperties()).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetAllProperties();

            // Assert

            mockLogger.Verify(x => x.LogInfo("No 'Property' has been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetAllPropertiesExceptionCatch()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetAllProperties()).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetAllProperties();

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAllProperties() action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPropertyByIdShouldReturnOKIfPropertiesInDb()
        {
            // Arrange

            var id = 13;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyById(id)).ReturnsAsync(new Property{
                Id = id,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false
            });


            // Act
           // var result = await _sut.GetPropertyById(12);
            var result = await _sut.GetPropertyById(id);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertyByIdShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange

            var id = 13;

            var property = new Property()
            {
                Id = id,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false
            };


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyById(id)).ReturnsAsync(property);


            // Act
            var result = await _sut.GetPropertyById(id);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Property with id: 13"), Times.Once);

        }

    
    [Fact]
    public async Task GetPropertyByIdShouldReturnNotFoundIfNoProperties()
    {
        // Arrange

        var id = 13;


        mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
        _propertyRepoMock.Setup(x => x.GetPropertyById(id)).ReturnsAsync(() => null);



        // Act
        var result = await _sut.GetPropertyById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        Assert.IsNotType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetPropertyByIdShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
    {
            // Arrange

            var id = 13;


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyById(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertyById(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Property with id: 13, hasn't been found in db."), Times.Once);

    }

    [Fact]
    public async Task GetPropertyByIdExceptionCatch()
    {
            // Arrange
            var id = 13;


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyById(id)).ThrowsAsync(new Exception());


        // Act
        var result = await _sut.GetPropertyById(id);

        // Assert

        mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertyById(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
        Assert.IsType<ObjectResult>(result);

    }

        [Fact]
        public async Task GetPropertyByIdLimitedShouldReturnOKIfPropertiesInDb()
        {
            // Arrange

            var id = 13;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyByIdLimited(id)).ReturnsAsync(new Property
            {
                Id = id,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false
            });


            // Act
            // var result = await _sut.GetPropertyById(12);
            var result = await _sut.GetPropertyByIdLimited(id);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertyByIdLimitedShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange

            var id = 13;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyByIdLimited(id)).ReturnsAsync(new Property
            {
                Id = id,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false
            });


            // Act
            // var result = await _sut.GetPropertyById(12);
            var result = await _sut.GetPropertyByIdLimited(id);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Property with id: 13"), Times.Once);

        }

        [Fact]
        public async Task GetPropertyByIdLimitedShouldReturnNotFoundIfNoProperties()
        {
            // Arrange

            var id = 13;


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyByIdLimited(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertyByIdLimited(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertyByIdLimitedShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange

            var id = 13;


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyByIdLimited(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertyByIdLimited(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Property with id: 13, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetPropertyByIdLimitedExceptionCatch()
        {
            // Arrange
            var id = 13;


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyByIdLimited(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPropertyByIdLimited(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertyByIdLimited(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPropertiesByLandlordIdShouldReturnOKIfPropertiesInDb()
        {
            // Arrange

            var landlordId = 13;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByLandlordId(landlordId)).ReturnsAsync(new[] {new Property
            {
                Id = 13,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = landlordId,
                AddressId = 1

            }});


            // Act
            var result = await _sut.GetPropertiesByLandlordId(landlordId);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertiesByLandlordIdShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange

            var landlordId = 13;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByLandlordId(landlordId)).ReturnsAsync(new[] {new Property
            {
                Id = 13,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = landlordId,
                AddressId = 1

            }});


            // Act
            var result = await _sut.GetPropertiesByLandlordId(landlordId);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Property with landlord_id: 13"), Times.Once);

        }

        [Fact]
        public async Task GetPropertiesByLandlordIdShouldReturnNotFoundIfNoProperties()
        {
            // Arrange

            var landlordId = 13;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByLandlordId(landlordId)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertiesByLandlordId(landlordId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertiesByLandlordIdShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange

            var landlordId = 13;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByLandlordId(landlordId)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertiesByLandlordId(landlordId);

            // Assert

            mockLogger.Verify(x => x.LogError("Property with landlord_id: 13, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetPropertiesByLandlordIdExceptionCatch()
        {
            // Arrange
            var landlordId = 13;


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByLandlordId(landlordId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPropertiesByLandlordId(landlordId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertiesByLandlordId(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPropertiesWithAddressByLandlordIdShouldReturnOKIfPropertiesInDb()
        {
            // Arrange

            var landlordId = 69;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesWithAddressByLandlordId(landlordId)).ReturnsAsync(new[] {new Property
            {
                Id = 13,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = landlordId,
                AddressId = 1,
                Address = new Address()
                {
                Id = 21,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
                }

            }});


            // Act
            var result = await _sut.GetPropertiesWithAddressByLandlordId(landlordId);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertiesWithAddressByLandlordIdShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange

            var landlordId = 69;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesWithAddressByLandlordId(landlordId)).ReturnsAsync(new[] {new Property
            {
                Id = 13,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = landlordId,
                AddressId = 1,
                Address = new Address()
                {
                Id = 21,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
                }

            }});


            // Act
            var result = await _sut.GetPropertiesWithAddressByLandlordId(landlordId);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Property with its address by landlord_id: 69"), Times.Once);

        }

        [Fact]
        public async Task GetPropertiesWithAddressByLandlordIdShouldReturnNotFoundIfNoProperties()
        {
            // Arrange

            var landlordId = 13;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesWithAddressByLandlordId(landlordId)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertiesWithAddressByLandlordId(landlordId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertiesWithAddressByLandlordIdShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange

            var landlordId = 13;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesWithAddressByLandlordId(landlordId)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertiesWithAddressByLandlordId(landlordId);

            // Assert

            mockLogger.Verify(x => x.LogError("Property with its address, by landlord_id: 13, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetPropertiesWithAddressByLandlordIdExceptionCatch()
        {
            // Arrange
            var landlordId = 13;


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesWithAddressByLandlordId(landlordId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPropertiesWithAddressByLandlordId(landlordId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertiesWithAddressByLandlordId(landlord_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPropertyWithAddressShouldReturnOKIfPropertiesInDb()
        {
            // Arrange

            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithAddress(id)).ReturnsAsync(new Property
            {
                Id = id,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Address = new Address()
                {
                Id = 21,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
                }

            });


            // Act
            var result = await _sut.GetPropertyWithAddress(id);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertyWithAddressShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange

            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithAddress(id)).ReturnsAsync(new Property
            {
                Id = id,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Address = new Address()
                {
                    Id = 21,
                    Country = "Polska",
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }

            });


            // Act
            var result = await _sut.GetPropertyWithAddress(id);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Property with id: 3 and its address."), Times.Once);

        }


        [Fact]
        public async Task GetPropertyWithAddressShouldReturnNotFoundIfNoProperties()
        {
            // Arrange

            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithAddress(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertyWithAddress(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertyWithAddressShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange

            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithAddress(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertyWithAddress(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Property with id: 3, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetPropertyWithAddressExceptionCatch()
        {
            // Arrange
            var id = 3;


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithAddress(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPropertyWithAddress(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertyWithAddress(property_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPropertyWithRateShouldReturnOKIfPropertiesInDb()
        {
            // Arrange
            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRate(id)).ReturnsAsync(new Property
            {
                Id = id,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Rate = new Rate()
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
                }

            });

            // Act
            var result = await _sut.GetPropertyWithRate(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertyWithRateShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange
            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRate(id)).ReturnsAsync(new Property
            {
                Id = id,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Rate = new Rate()
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
                }

            });

            // Act
            var result = await _sut.GetPropertyWithRate(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Property with id: 3 and its rate."), Times.Once);

        }
        [Fact]
        public async Task GetPropertyWithRateShouldReturnNotFoundIfNoProperties()
        {
            // Arrange

            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRate(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertyWithRate(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertyWithRateShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange

            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRate(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertyWithRate(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Property with id: 3, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetPropertyWithRateExceptionCatch()
        {
            // Arrange
            var id = 3;


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRate(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPropertyWithRate(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertyWithRate(property_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPropertyWithRentsShouldReturnOKIfPropertiesInDb()
        {
            // Arrange
            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRents(id)).ReturnsAsync(new Property
            {
                Id = id,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Rents = new [] {new Rent()
                {
                    Id = 1,
                    PropertyId = id,
                    RentPurpose = "work",
                    StartRent = DateTime.Now,
                    EndRent = DateTime.Today,
                    TenantCount = 3,
                    RentDeposit = 1500,
                    PayDayDelay = 7,
                    SendStateDay = 12,
                    DisplayOnWeb = false,
                    PhotoRequired = false
                } }

            } );

            // Act
            var result = await _sut.GetPropertyWithRents(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertyWithRentsShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange
            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRents(id)).ReturnsAsync(new Property
            {
                Id = id,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Rents = new[] {new Rent()
                {
                    Id = 1,
                    PropertyId = id,
                    RentPurpose = "work",
                    StartRent = DateTime.Now,
                    EndRent = DateTime.Today,
                    TenantCount = 3,
                    RentDeposit = 1500,
                    PayDayDelay = 7,
                    SendStateDay = 12,
                    DisplayOnWeb = false,
                    PhotoRequired = false
                }, new Rent() }

            });

            // Act
            var result = await _sut.GetPropertyWithRents(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Property with id: 3 and its Rents, Rents count: 2."), Times.Once);

        }
        [Fact]
        public async Task GetPropertyWithRentsShouldReturnNotFoundIfNoProperties()
        {
            // Arrange

            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRents(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertyWithRents(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertyWithRentsShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange

            var id = 3;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRents(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertyWithRents(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Property with id: 3, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetPropertyWithRentsExceptionCatch()
        {
            // Arrange
            var id = 3;


            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRents(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPropertyWithRents(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertyWithRents(property_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }
        [Fact]
        public async Task GetPropertiesByCountryShouldReturnOKIfPropertiesInDb()
        {
            // Arrange

            var country = "Polska";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByCountry(country)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Address = new Address()
                {
                    Id = 21,
                    Country = country,
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }

            }});

            // Act
            var result = await _sut.GetPropertiesByCountry(country);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertiesByCountryShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange

            var country = "Polska";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByCountry(country)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Address = new Address()
                {
                    Id = 21,
                    Country = country,
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }

            }});

            // Act
            var result = await _sut.GetPropertiesByCountry(country);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Property with Country: Polska, properties found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetPropertiesByCountryShouldReturnNotFoundIfNoProperties()
        {
            // Arrange
            var country = "Polska";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByCountry(country)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertiesByCountry(country);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertiesByCountryShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange
            var country = "Polska";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByCountry(country)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPropertiesByCountry(country);

            // Assert

            mockLogger.Verify(x => x.LogError("Property with Country: Polska, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetPropertiesByCountryExceptionCatch()
        {
            // Arrange
            var country = "Polska";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByCountry(country)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPropertiesByCountry(country);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertiesByCountry(country) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPropertiesByCityShouldReturnOKIfPropertiesInDb()
        {
            // Arrange
            var city = "Sopot";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByCity(city)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Address = new Address()
                {
                    Id = 21,
                    Country = "Polska",
                    City = city,
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            }});

            // Act
            var result = await _sut.GetPropertiesByCity(city);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertiesByCityShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange
            var city = "Sopot";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByCity(city)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Address = new Address()
                {
                    Id = 21,
                    Country = "Polska",
                    City = city,
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            }});

            // Act
            var result = await _sut.GetPropertiesByCity(city);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Properties with city: Sopot, number of properties found: 1."), Times.Once);
        }
        [Fact]
        public async Task GetPropertiesByCityShouldReturnNotFoundIfNoProperties()
        {
            // Arrange
            var city = "Sopot";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByCity(city)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByCity(city);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertiesByCityShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange
            var city = "Sopot";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByCity(city)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByCity(city);

            // Assert
            mockLogger.Verify(x => x.LogError("Properties with city: Sopot, hasn't been found in db."), Times.Once);
        }

        [Fact]
        public async Task GetPropertiesByCityExceptionCatch()
        {
            // Arrange
            var city = "Sopot";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByCity(city)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetPropertiesByCity(city);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertiesByCity(city) action: Exception of type 'System.Exception' was thrown.."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetPropertiesByStreetShouldReturnOKIfPropertiesInDb()
        {
            // Arrange
            var street = "Ogrodowa";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByStreet(street)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Address = new Address()
                {
                    Id = 21,
                    Country = "Polska",
                    City = "Sopot",
                    Street = street,
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            }});

            // Act
            var result = await _sut.GetPropertiesByStreet(street);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertiesByStreetShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange
            var street = "Ogrodowa";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByStreet(street)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Address = new Address()
                {
                    Id = 21,
                    Country = "Polska",
                    City = "Sopot",
                    Street = street,
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            }});

            // Act
            var result = await _sut.GetPropertiesByStreet(street);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Properties with street: Ogrodowa. Property with street count: 1."), Times.Once);
        }
        [Fact]
        public async Task GetPropertiesByStreetShouldReturnNotFoundIfNoProperties()
        {
            // Arrange
            var street = "Ogrodowa";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByStreet(street)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByStreet(street);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertiesByStreetShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange
            var street = "Ogrodowa";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByStreet(street)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByStreet(street);

            // Assert
            mockLogger.Verify(x => x.LogError("Property on street : Ogrodowa, hasn't been found in db."), Times.Once);
        }

        [Fact]
        public async Task GetPropertiesByStreetExceptionCatch()
        {
            // Arrange
            var street = "Ogrodowa";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByStreet(street)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetPropertiesByStreet(street);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertiesByStreet(street) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetPropertiesByZipCodeShouldReturnOKIfPropertiesInDb()
        {
            // Arrange
            var zip = "80-765";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByZipCode(zip)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Address = new Address()
                {
                    Id = 21,
                    Country = "Polska",
                    City = "Sopot",
                    Street = "Ogrodowa",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = zip
                }
            }});

            // Act
            var result = await _sut.GetPropertiesByZipCode(zip);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertiesByZipCodeShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange
            var zip = "80-765";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByZipCode(zip)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1,
                Address = new Address()
                {
                    Id = 21,
                    Country = "Polska",
                    City = "Sopot",
                    Street = "Ogrodowa",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = zip
                }
            }});

            // Act
            var result = await _sut.GetPropertiesByZipCode(zip);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Properties under ZIP: 80-765. Property count: 1."), Times.Once);
        }
        [Fact]
        public async Task GetPropertiesByZipCodeShouldReturnNotFoundIfNoProperties()
        {
            // Arrange
            var zip = "80-765";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByZipCode(zip)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByZipCode(zip);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertiesByZipCodeShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange
            var zip = "80-765";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByZipCode(zip)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByZipCode(zip);

            // Assert
            mockLogger.Verify(x => x.LogError("Property under ZIP : 80-765, hasn't been found in db."), Times.Once);
        }

        [Fact]
        public async Task GetPropertiesByZipCodeExceptionCatch()
        {
            // Arrange
            var zip = "80-765";

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByZipCode(zip)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetPropertiesByZipCode(zip);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertiesByZipCode(ZIP) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetPropertiesByFlatSizeeShouldReturnOKIfPropertiesInDb()
        {
            // Arrange
            var flatSize = 36;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByFlatSize(flatSize)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = flatSize,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1
            }});

            // Act
            var result = await _sut.GetPropertiesByFlatSize(flatSize);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertiesByFlatSizeShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange
            var flatSize = 36;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByFlatSize(flatSize)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = flatSize,
                HasGas = false,
                HasHW = true,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1
            }});

            // Act
            var result = await _sut.GetPropertiesByFlatSize(flatSize);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Property with size: 36[m^2]. Number of properties with specified size: 1."), Times.Once);
        }
        [Fact]
        public async Task GetPropertiesByFlatSizeShouldReturnNotFoundIfNoProperties()
        {
            // Arrange
            var flatSize = 36;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByFlatSize(flatSize)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByFlatSize(flatSize);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertiesByFlatSizeShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange
            var flatSize = 36;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByFlatSize(flatSize)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByFlatSize(flatSize);

            // Assert
            mockLogger.Verify(x => x.LogError("Property of size:: 36[m^2], hasn't been found in db."), Times.Once);
        }

        [Fact]
        public async Task GetPropertiesByFlatSizeExceptionCatch()
        {
            // Arrange
            var flatSize = 36;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByFlatSize(flatSize)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetPropertiesByFlatSize(flatSize);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertiesByFlatSize(property_size[m^2]) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetPropertiesByHasGasShouldReturnOKIfPropertiesInDb()
        {
            // Arrange
            var hasGas = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasGas(hasGas)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = hasGas,
                HasHW = false,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1
            }});

            // Act
            var result = await _sut.GetPropertiesByHasGas(hasGas);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertiesByHasGasShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange
            var hasGas = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasGas(hasGas)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = hasGas,
                HasHW = false,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1
            }});

            // Act
            var result = await _sut.GetPropertiesByHasGas(hasGas);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Properties with field hasGas set to: False. Properties with gas count: 1."), Times.Once);
        }
        [Fact]
        public async Task GetPropertiesByHasGasShouldReturnNotFoundIfNoProperties()
        {
            // Arrange
            var hasGas = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasGas(hasGas)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByHasGas(hasGas);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertiesByHasGasShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange
            var hasGas = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasGas(hasGas)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByHasGas(hasGas);

            // Assert
            mockLogger.Verify(x => x.LogError("Property has Gas: False, hasn't been found in db."), Times.Once);
        }

        [Fact]
        public async Task GetPropertiesByHasGasExceptionCatch()
        {
            // Arrange
            var hasGas = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasGas(hasGas)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetPropertiesByHasGas(hasGas);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertiesByHasGas(bool: hasGas) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetPropertiesByHasHotWaterShouldReturnOKIfPropertiesInDb()
        {
            // Arrange
            var hasHW = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasHotWater(hasHW)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = hasHW,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1
            }});

            // Act
            var result = await _sut.GetPropertiesByHasHotWater(hasHW);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertiesByHasHotWaterShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange
            var hasHW = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasHotWater(hasHW)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = hasHW,
                HasHeat = false,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1
            }});

            // Act
            var result = await _sut.GetPropertiesByHasHotWater(hasHW);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Properties with field 'hasHW' set to: False. Properties with hasHotWater 1."), Times.Once);
        }
        [Fact]
        public async Task GetPropertiesByHasHotWaterShouldReturnNotFoundIfNoProperties()
        {
            // Arrange
            var hasHW = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasHotWater(hasHW)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByHasHotWater(hasHW);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertiesByHasHotWaterShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange
            var hasHW = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasHotWater(hasHW)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByHasHotWater(hasHW);

            // Assert
            mockLogger.Verify(x => x.LogError("Property 'hasHW': False, hasn't been found in db."), Times.Once);
        }

        [Fact]
        public async Task GetPropertiesByHasHotWaterExceptionCatch()
        {
            // Arrange
            var hasHW = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasHotWater(hasHW)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetPropertiesByHasHotWater(hasHW);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertiesByHasHotWater(bool: hasHW) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetPropertiesByHasHeatShouldReturnOKIfPropertiesInDb()
        {
            // Arrange
            var hasHeat = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasHeat(hasHeat)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = false,
                HasHeat = hasHeat,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1
            }});

            // Act
            var result = await _sut.GetPropertiesByHasHeat(hasHeat);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetPropertiesByHasHeatShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange
            var hasHeat = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasHeat(hasHeat)).ReturnsAsync(new[] {new Property
            {
                Id = 32,
                FlatLabel = "Apartament1",
                RoomCount = 3,
                FlatSize = 36,
                HasGas = false,
                HasHW = false,
                HasHeat = hasHeat,
                DisplayOnWeb = false,
                LandlordId = 13,
                AddressId = 1
            }});

            // Act
            var result = await _sut.GetPropertiesByHasHeat(hasHeat);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Properties with field 'hasHeat' set to: False. Properties with hasHeat 1."), Times.Once);
        }
        [Fact]
        public async Task GetPropertiesByHasHeatShouldReturnNotFoundIfNoProperties()
        {
            // Arrange
            var hasHeat = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasHeat(hasHeat)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByHasHeat(hasHeat);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertiesByHasHeatShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {
            // Arrange
            var hasHeat = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasHeat(hasHeat)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertiesByHasHeat(hasHeat);

            // Assert
            mockLogger.Verify(x => x.LogError("Property 'hasHeat': False, hasn't been found in db."), Times.Once);
        }

        [Fact]
        public async Task GetPropertiesByHasHeatExceptionCatch()
        {
            // Arrange
            var hasHeat = false;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertiesByHasHeat(hasHeat)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetPropertiesByHasHeat(hasHeat);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertiesByHasHeat(bool: hasHeat) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreatePropertyExceptionCatch()
        {
            // Arrange
            var id = 45;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.CreateProperty(new Property())).Throws(new Exception());

            // Act
            var result = await _sut.CreateProperty(new PropertyForCreationDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside CreateProperty(propertForCreationDto) action: Object reference not set to an instance of an object."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreatePropertyShouldReturnBadRequestIfPropertyIsNull()
        {
            // Arrange


            // Act
            var result = await _sut.CreateProperty(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
        [Fact]
        public async Task CreatePropertyshouldLogAppropriateMessageIfPropertyIsNull()
        {
            // Arrange


            // Act
            var result = await _sut.CreateProperty(null);

            // Assert
            mockLogger.Verify(x => x.LogError("Property received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task UpdatePropertyExceptionCatch()
        {
            /// Arrange
            var id = 45;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyById(id)).Throws(new Exception());

            // Act
            var result = await _sut.UpdateProperty(id, new PropertyForUpdateDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside UpdateProperty(id, property) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task UpdatePropertyShouldReturnBadRequestIfPropertyIsNull()
        {
            /// Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdateProperty(id, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdatePropertyShouldLogAppropriateMessageIfPropertyIsNull()
        {
            // Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdateProperty(id, null);

            // Assert
            mockLogger.Verify(x => x.LogError("Property received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task DeletePropertyExceptionCatch()
        {
            /// Arrange
            var id = 45;
            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRentsAndAddress(id)).Throws(new Exception());

            // Act
            var result = await _sut.DeleteProperty(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside DeleteProperty(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task DeletePropertyShouldLogAppropriateMessageWhenNoPhotosInDatabase()
        {
            // Arrange

            var id = 45;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRentsAndAddress(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.DeleteProperty(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Not deleted. Property with given id: 45 was not found"), Times.Once);
        }
        [Fact]
        public async Task UnDeletePropertyExceptionCatch()
        {
            /// Arrange
            var id = 45;
            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRentsAndAddress(id)).Throws(new Exception());

            // Act
            var result = await _sut.UnDeleteProperty(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside UnDeleteProperty(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task UnDeletePropertyShouldLogAppropriateMessageWhenNoPhotosInDatabase()
        {
            // Arrange

            var id = 45;

            mockRepo.Setup(x => x.Property).Returns(_propertyRepoMock.Object);
            _propertyRepoMock.Setup(x => x.GetPropertyWithRentsAndAddress(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.UnDeleteProperty(id);

            // Assert
        
            mockLogger.Verify(x => x.LogError("Not undeleted. Property with given id: 45 was not found"), Times.Once);
        }

        [Fact]
        public async Task GetPropertyWithAddressByRentIdShouldLogAppropriateMessageWhenPropertyInDatabase()
        {
            // Arrange
            var rentId = 1;
            Mock<IRentRepository> _rentRepoMock = new Mock<IRentRepository>();
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithPropertyAndAddress(rentId)).ReturnsAsync( new Rent {} );

            // Act
            var result = await _sut.GetPropertyWithAddressByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Property for rent with id: 1 hasn't been found in db."), Times.Once);
        }

        [Fact]
        public async Task GetPropertyWithAddressByRentIdShouldReturnNotFoundIfNoProperties()
        {


            // Arrange
            var rentId = 1;
            Mock<IRentRepository> _rentRepoMock = new Mock<IRentRepository>();
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithPropertyAndAddress(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertyWithAddressByRentId(rentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPropertyWithAddressByRentIdShouldLogAppropriateMessageWhenNoPropertiesInDatabase()
        {

            // Arrange
            var rentId = 1;
            Mock<IRentRepository> _rentRepoMock = new Mock<IRentRepository>();
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithPropertyAndAddress(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPropertyWithAddressByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Rent with id: 1, hasn't been found in db."), Times.Once);
        }

        [Fact]
        public async Task GetPropertyWithAddressByRentIdExceptionCatch()
        {
            // Arrange
            var rentId = 1;

            Mock<IRentRepository> _rentRepoMock = new Mock<IRentRepository>();
            mockRepo.Setup(x => x.Rent).Returns(_rentRepoMock.Object);
            _rentRepoMock.Setup(x => x.GetRentWithPropertyAndAddress(rentId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetPropertyWithAddressByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPropertyWithAddressByRentId(rent_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
    }

}
