using AutoMapper;
using Contracts;
using EmailService;
using Entities.DataTransferObjects.Landlord;
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
    public class LandlordControllerTests
    {
        private readonly LandlordController _sut;
        private readonly Mock<IRepositoryWrapper> mockRepo = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
        private readonly Mock<IIdentityService> mockIdentity = new Mock<IIdentityService>();
        private readonly Mock<IEmailEmmiter> mockEmail = new Mock<IEmailEmmiter>();
        private readonly Mock<ILandlordRepository> _landlordRepoMock = new Mock<ILandlordRepository>();

        public LandlordControllerTests()
        {
            _sut = new LandlordController(mockRepo.Object, mockMapper.Object, mockLogger.Object, mockIdentity.Object, mockEmail.Object);
        }
        [Fact]
        public async Task GetAllLandlordsShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var landlord = new Landlord()
            {
                Id = 1,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456"
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetAllLandlords()).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetAllLandlords();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetAllLandlordsShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var landlord = new Landlord()
            {
                Id = 1,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456"
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetAllLandlords()).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetAllLandlords();

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned all 'Landlords' from db. Number of Landords found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetAllLandlordsShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetAllLandlords()).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllLandlords();

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllLandlordsShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetAllLandlords()).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllLandlords();

            // Assert
            mockLogger.Verify(x => x.LogInfo("No 'Landlord' has been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetAllLandlordsxceptionCatch()
        {
            // Arrange
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetAllLandlords()).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetAllLandlords();

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAllLandlords() action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLandlordByIdShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var id = 5;
            var landlord = new Landlord()
            {
                Id = id,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456"
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordById(id)).ReturnsAsync(landlord);

            // Act
            var result = await _sut.GetLandlordById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordByIdShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var id = 5;
            var landlord = new Landlord()
            {
                Id = id,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456"
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordById(id)).ReturnsAsync(landlord);

            // Act
            var result = await _sut.GetLandlordById(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with id: 5"), Times.Once);

        }
        [Fact]
        public async Task GetLandlordByIdShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var id = 5;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordByIdShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var id = 5;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordById(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with id: 5, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordByIdExceptionCatch()
        {
            // Arrange
            var id = 5;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordById(id)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordById(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordById(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLandlordWithPropertiesReturnOKIfLandlordsInDb()
        {
            // Arrange
            var id = 5;
            var landlord = new Landlord()
            {
                Id = id,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                Properties = new[] { new Property() { Id = 1,
                    FlatLabel = "Apartament1",
                    RoomCount = 3,
                    FlatSize = 36,
                    HasGas = false,
                    HasHW = true,
                    HasHeat = false,
                    DisplayOnWeb = false,
                    LandlordId = id
                } }
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithProperties(id)).ReturnsAsync(landlord);

            // Act
            var result = await _sut.GetLandlordWithProperties(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordWithPropertiesShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var id = 5;
            var landlord = new Landlord()
            {
                Id = id,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                Properties = new[] { new Property() { Id = 1,
                    FlatLabel = "Apartament1",
                    RoomCount = 3,
                    FlatSize = 36,
                    HasGas = false,
                    HasHW = true,
                    HasHeat = false,
                    DisplayOnWeb = false,
                    LandlordId = id
                } }
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithProperties(id)).ReturnsAsync(landlord);

            // Act
            var result = await _sut.GetLandlordWithProperties(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with id: 5 and its properties. Number of Properties of this landlord: 1."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordWithPropertiesShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var id = 5;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithProperties(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordWithProperties(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordWithPropertiesShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var id = 5;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithProperties(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordWithProperties(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with id: 5, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordWithPropertiesExceptionCatch()
        {
            // Arrange
            var id = 5;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithProperties(id)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordWithProperties(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordWithProperties(landlord_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordsByCityReturnOKIfLandlordsInDb()
        {
            // Arrange
            var city = "Sopot";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                Address = new Address()
                {
                    Id = 12,
                    Country = "Polska",
                    City = city,
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByCity(city)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByCity(city);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByCityShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var city = "Sopot";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                Address = new Address()
                {
                    Id = 12,
                    Country = "Polska",
                    City = city,
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByCity(city)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByCity(city);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with city: Sopot, landlords found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordsByCityShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var city = "Sopot";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByCity(city)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByCity(city);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordsByCityShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var city = "Sopot";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByCity(city)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByCity(city);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlords with city: Sopot, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordsByCityExceptionCatch()
        {
            // Arrange
            var city = "Sopot";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByCity(city)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordsByCity(city);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordsByCity(city) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByStreetReturnOKIfLandlordsInDb()
        {
            // Arrange
            var street = "Ciasna";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                Address = new Address()
                {
                    Id = 12,
                    Country = "Polska",
                    City = "Sopot",
                    Street = street,
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByStreet(street)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByStreet(street);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByStreetShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var street = "Ciasna";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                Address = new Address()
                {
                    Id = 12,
                    Country = "Polska",
                    City = "Sopot",
                    Street = street,
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByStreet(street)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByStreet(street);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with Street: Ciasna, landlords found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordsByStreetShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var street = "Ciasna";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByStreet(street)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByStreet(street);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordsByStreetShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var street = "Ciasna";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByStreet(street)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByStreet(street);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with Street: Ciasna, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordsByStreetExceptionCatch()
        {
            // Arrange
            var street = "Ciasna";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByStreet(street)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordsByStreet(street);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordsByStreet(street) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByCountryShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var country = "Polska";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                Address = new Address()
                {
                    Id = 12,
                    Country = country,
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByCountry(country)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByCountry(country);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByCountryShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var country = "Polska";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                Address = new Address()
                {
                    Id = 12,
                    Country = country,
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByCountry(country)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByCountry(country);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with Country: Polska, landlords found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordsByCountryShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var country = "Polska";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByCountry(country)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByCountry(country);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordsByCountryShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var country = "Polska";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByCountry(country)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByCountry(country);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with Country: Polska, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordsByCountryExceptionCatch()
        {
            // Arrange
            var country = "Polska";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByCountry(country)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordsByCountry(country);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordsByCountry(country) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByZipCodeShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var zip = "80-765";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                Address = new Address()
                {
                    Id = 12,
                    Country = "Polska",
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = zip
                }
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByZipCode(zip)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByZipCode(zip);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByZipCodeShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var zip = "80-765";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                Address = new Address()
                {
                    Id = 12,
                    Country = "Polska",
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = zip
                }
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByZipCode(zip)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByZipCode(zip);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with ZipCode: 80-765, landlords found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordsByZipCodeShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var zip = "80-765";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByZipCode(zip)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByZipCode(zip);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordsByZipCodeShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var zip = "80-765";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByZipCode(zip)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByZipCode(zip);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with ZipCode: 80-765, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordsByZipCodeExceptionCatch()
        {
            // Arrange
            var zip = "80-765";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByZipCode(zip)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordsByZipCode(zip);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordsByZipCode(zipCode) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLandlordByEmailShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var email = "landlord1@wp.pl";

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByEmail(email)).ReturnsAsync(new Landlord());

            // Act
            var result = await _sut.GetLandlordByEmail(email);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordByEmailShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var email = "landlord1@wp.pl";

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByEmail(email)).ReturnsAsync(new Landlord());

            // Act
            var result = await _sut.GetLandlordByEmail(email);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with e-mail: landlord1@wp.pl."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordByEmailShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var email = "landlord1@wp.pl";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByEmail(email)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordByEmail(email);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordByEmailShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var email = "landlord1@wp.pl";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByEmail(email)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordByEmail(email);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with e-mail: landlord1@wp.pl, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordByEmailExceptionCatch()
        {
            // Arrange
            var email = "landlord1@wp.pl";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByEmail(email)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordByEmail(email);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordByEmail(email) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordByAspUserIdShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var aspId = 4;
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = aspId
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByAspNetUserId(aspId)).ReturnsAsync(landlord);

            // Act
            var result = await _sut.GetLandlordByAspUserId(aspId);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordByAspUserIdShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var aspId = 4;
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = aspId
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByAspNetUserId(aspId)).ReturnsAsync(landlord);

            // Act
            var result = await _sut.GetLandlordByAspUserId(aspId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with aspId: 4."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordByAspUserIdShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var aspId = 4;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByAspNetUserId(aspId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordByAspUserId(aspId);

            // Assert
            ///toCheckLater
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordByAspUserIdShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var aspId = 4;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByAspNetUserId(aspId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordByAspUserId(aspId);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with aspId: 4, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordByAspUserIdExceptionCatch()
        {
            // Arrange
            var aspId = 4;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByAspNetUserId(aspId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordByAspUserId(aspId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordByAspUserId(aspNetUserId) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLandlordsBySurnameShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var surname = "Stonoga";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = surname,
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = 4
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsBySurname(surname)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsBySurname(surname);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordsBySurnameShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var surname = "Stonoga";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = surname,
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = 4
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsBySurname(surname)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsBySurname(surname);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with Surname: Stonoga, landlords found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordsBySurnameShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var surname = "Stonoga";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsBySurname(surname)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsBySurname(surname);

            // Assert

            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordsBySurnameShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var surname = "Stonoga";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsBySurname(surname)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsBySurname(surname);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with Surname: Stonoga, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordsBySurnameExceptionCatch()
        {
            // Arrange
            var surname = "Stonoga";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsBySurname(surname)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordsBySurname(surname);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordsBySurname(landlord_surname) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByCompanyShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var companyName = "Szlachta nie Pracuje";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = true,
                CompanyName = companyName,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = 4
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByCompanyName(companyName)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByCompany(companyName);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByCompanyShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var companyName = "Szlachta nie Pracuje";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = true,
                CompanyName = companyName,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = 4
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByCompanyName(companyName)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByCompany(companyName);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with CompanyName: Szlachta nie Pracuje, landlords found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordsByCompanyShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var companyName = "Szlachta nie Pracuje";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByCompanyName(companyName)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByCompany(companyName);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordsByCompanyShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var companyName = "Szlachta nie Pracuje";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByCompanyName(companyName)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByCompany(companyName);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with CompanyName: Szlachta nie Pracuje, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordsByCompanyExceptionCatch()
        {
            // Arrange
            var companyName = "Szlachta nie Pracuje";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByCompanyName(companyName)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordsByCompany(companyName);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordsByCompany(company_name) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByNipShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var nip = "9876500011";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                NIP = nip,
                PESEL = "83876500011",
                REGON = "123456789",
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = 4
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByNip(nip)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByNip(nip);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByNipShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var nip = "9876500011";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                NIP = nip,
                PESEL = "83876500011",
                REGON = "123456789",
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = 4
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByNip(nip)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByNip(nip);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with NIP: 9876500011, landlords found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordsByNipShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var nip = "9876500011";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByNip(nip)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByNip(nip);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordsByNipShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var nip = "9876500011";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByNip(nip)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByNip(nip);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with NIP: 9876500011, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordsByNipExceptionCatch()
        {
            // Arrange
            var nip = "9876500011";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByNip(nip)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordsByNip(nip);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordByNip(NIP) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByPeselShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var pesel = "83876500011";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                NIP = "9876500011",
                PESEL = pesel,
                REGON = "123456789",
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = 4
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByPesel(pesel)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByPesel(pesel);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByPeselShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var pesel = "83876500011";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                NIP = "9876500011",
                PESEL = pesel,
                REGON = "123456789",
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = 4
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByPesel(pesel)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByPesel(pesel);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with PESEL: 83876500011, landlords found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordsByPeselShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var pesel = "83876500011";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByPesel(pesel)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByPesel(pesel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordsByPeselShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var pesel = "83876500011";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByPesel(pesel)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByPesel(pesel);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with PESEL: 83876500011, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordsByPeselExceptionCatch()
        {
            // Arrange
            var pesel = "83876500011";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordsByPesel(pesel)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordsByPesel(pesel);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordByPesel(PESEL) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByRegonShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var regon = "123456789";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                NIP = "9876500011",
                PESEL = "83876500011",
                REGON = regon,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = 4
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByRegon(regon)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByRegon(regon);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetLandlordsByRegonShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var regon = "123456789";
            var landlord = new Landlord()
            {
                Id = 7,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                NIP = "9876500011",
                PESEL = "83876500011",
                REGON = regon,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                AddressId = 12,
                AspNetUsersId = 4
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByRegon(regon)).ReturnsAsync(new[] { landlord });

            // Act
            var result = await _sut.GetLandlordsByRegon(regon);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord with NIP: 123456789, landlords found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordsByRegonShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var regon = "123456789";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByRegon(regon)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByRegon(regon);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordsByRegonShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var regon = "123456789";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByRegon(regon)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordsByRegon(regon);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with REGON: 123456789, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordsByRegonExceptionCatch()
        {
            // Arrange
            var regon = "123456789";
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordByRegon(regon)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordsByRegon(regon);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordsByRegon(regon) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreateLandlordExceptionCatch()
        {
            // Arrange

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.CreateLandlord(new Landlord())).Throws(new Exception());

            // Act
            var result = await _sut.CreateLandlord(new LandlordForCreationDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside CreateLandlord() action: Object reference not set to an instance of an object."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreateLandlordShouldReturnBadRequestIfLandlordIsNull()
        {
            // Arrange

            // mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            // _landlordRepoMock.Setup(x => x.CreateLandlord(null));

            // Act
            var result = await _sut.CreateLandlord(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
        [Fact]
        public async Task CreateLandlordShouldLogAppropriateMessageIfLandlordIsNull()
        {
            // Arrange

            // mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            // _landlordRepoMock.Setup(x => x.CreateLandlord(null));

            // Act
            var result = await _sut.CreateLandlord(null);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task UpdateLandlordExceptionCatch()
        {
            /// Arrange
            var id = 3;

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordById(id)).Throws(new Exception());

            // Act
            var result = await _sut.UpdateLandlord(id, new LandlordForUpdateDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside UpdateLandlord(id, landlordForUpdateDto) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task UpdateLandlordShouldReturnBadRequestIfLandlordIsNull()
        {
            /// Arrange
            var id = 3;

            // mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            // _landlordRepoMock.Setup(x => x.GetLandlordById(id)).Throws(new Exception());

            // Act
            var result = await _sut.UpdateLandlord(id, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateLandlordShouldLogAppropriateMessageIfLandlordIsNull()
        {
            // Arrange
            var id = 3;

            // mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            // _landlordRepoMock.Setup(x => x.CreateLandlord(null));

            // Act
            var result = await _sut.UpdateLandlord(id, null);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task DeleteLandlordExceptionCatch()
        {
            /// Arrange
            var id = 3;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithAddress(id)).Throws(new Exception());

            // Act
            var result = await _sut.DeleteLandlord(id);

            // Assert
            //toCheck
            mockLogger.Verify(x => x.LogError("Something went wrong inside DeleteLandlord(id) action: System.Exception: Exception of type 'System.Exception' was thrown."), Times.Never);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task DeleteLandlordShouldLogAppropriateMessageWhenNoLandlordWithGivenIdInDatabase()
        {
            // Arrange
            var id = 3;

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithAddress(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.DeleteLandlord(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Not deleted. Landlord with given id: 3 was not found"), Times.Once);
        }

        [Fact]
        public async Task GetLandlordWithAddressShouldReturnOKIfLandlordsInDb()
        {
            // Arrange
            var id = 5;
            var landlord = new Landlord()
            {
                Id = id,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                Address = new Address()
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithAddress(id)).ReturnsAsync(landlord);

            // Act
            var result = await _sut.GetLandlordWithAddress(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetLandlordWithAddressShouldLogAppropriateMessageWhenLandlordsInDatabase()
        {
            // Arrange
            var id = 5;
            var landlord = new Landlord()
            {
                Id = id,
                Name = "Janusz",
                Surname = "Stonoga",
                IsCompany = false,
                IsVATPayer = false,
                PhoneNumber = "948387397",
                BankAccount = "67843186627845100000123456",
                Address = new Address()
            };

            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithAddress(id)).ReturnsAsync(landlord);

            // Act
            var result = await _sut.GetLandlordWithAddress(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Landlord and its address."), Times.Once);

        }
        [Fact]
        public async Task GetLandlordWithAddressShouldReturnNotFoundIfNoLandlordsInDb()
        {
            // Arrange
            var id = 5;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithAddress(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordWithAddress(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLandlordWithAddressShouldLogAppropriateMessageWhenNoLandlordsInDatabase()
        {
            // Arrange
            var id = 5;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithAddress(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetLandlordWithAddress(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Landlord with id: 5, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetLandlordWithAddressExceptionCatch()
        {
            // Arrange
            var id = 5;
            mockRepo.Setup(x => x.Landlord).Returns(_landlordRepoMock.Object);
            _landlordRepoMock.Setup(x => x.GetLandlordWithAddress(id)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetLandlordWithAddress(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetLandlordWithAddress(landlord_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
    }
}
