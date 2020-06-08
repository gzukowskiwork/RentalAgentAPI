using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Address;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleFakeRent.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FakeRentTests
{
    public class AddressControllerTests
    {
        private readonly AddressController _sut;
        private readonly Mock<IRepositoryWrapper> mockRepo = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
        private readonly Mock<IAddressRepository> _addressRepoMock = new Mock<IAddressRepository>();

        public AddressControllerTests()
        {
            _sut = new AddressController(mockRepo.Object, mockMapper.Object, mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAddressesShouldReturnNotFoundIfNoAddresses()
        {
            // Arrange

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAllAddresses()).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetAllAddresses();

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllAddressesShouldReturnOKIfAddressesInDb()
        {
            // Arrange

            var address = new Address()
            {
                Id = 1,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAllAddresses()).ReturnsAsync(new[] { address });


            // Act
            var result = await _sut.GetAllAddresses();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
            Assert.Equal(1, address.Id);
        }

        [Fact]
        public async Task GetAllAddressesShouldLogAppropriateMessageWhenNoAddressesInDatabase()
        {
            // Arrange

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAllAddresses()).ReturnsAsync(()=>null);

            // Act
            var result = await _sut.GetAllAddresses();

            // Assert

            mockLogger.Verify(x => x.LogInfo("No 'Address' has been found in db."), Times.Once);

        }
        [Fact]
        public async Task GetAllAddressesShouldLogAppropriateMessageWhenAddressInDatabase()
        {
            // Arrange

            var address = new Address()
            {
                Id = 1,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
            };


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAllAddresses()).ReturnsAsync(new[] { address });


            // Act
            var result = await _sut.GetAllAddresses();

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned all 'Address' from db. Total number of Addresses found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetAllAddressesExceptionCatch()
        {
            // Arrange

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAllAddresses()).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetAllAddresses();

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAllAddresses() action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }
        [Fact]
        public async Task GetAdressByIdShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var addressId = 1;

            var address = new Address()
            {
                Id = addressId,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressById(addressId)).ReturnsAsync( address );

            // Act
            var result = await _sut.GetAdressById(addressId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, address.Id);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetAdressByIdShouldReturnNotFoundIfNoAddresses()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressById(addressId)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetAdressById(addressId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetAdressByIdShouldLogAppropriateMessageWhenAddressInDatabase()
        {
            // Arrange
            var addressId = 1;

            var address = new Address()
            {
                Id = addressId,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
            };


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressById(addressId)).ReturnsAsync( address );


            // Act
            var result = await _sut.GetAdressById(addressId);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Address with id: 1"), Times.Once);

        }

        [Fact]
        public async Task GetAdressByIdShouldLogAppropriateMessageWhenNoAddressesInDatabase()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressById(addressId)).ReturnsAsync(() => null);


            // Act
            var result = await _sut.GetAdressById(addressId);

            // Assert

            mockLogger.Verify(x => x.LogError("Address with id: 1, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetAdressByIdExceptionCatch()
        {
            // Arrange
            var id = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressById(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetAdressById(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAddressById(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetAddressWithLandlordShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var addressId = 1;

            var address = new Address()
            {
                Id = addressId,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765",
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

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithLandlord(addressId)).ReturnsAsync(address);

            // Act
            var result = await _sut.GetAddressWithLandlord(addressId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, address.Id);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAddressWithLandlordShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithLandlord(addressId)).ReturnsAsync(()=>null);

            // Act
            var result = await _sut.GetAddressWithLandlord(addressId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetAddressWithLandlordShouldLogAppropriateMessageWhenAddressInDatabase()
        {
            // Arrange
            var addressId = 1;

            var address = new Address()
            {
                Id = addressId,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765",
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

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithLandlord(addressId)).ReturnsAsync(address);

            // Act
            var result = await _sut.GetAddressWithLandlord(addressId);

 
            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Address with id: 1 and its Landlord."), Times.Once);

        }

        [Fact]
        public async Task GetAddressWithLandlordShouldLogAppropriateMessageWhenNoAddressesInDatabase()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithLandlord(addressId)).ReturnsAsync(() => null);


            // Act
            var result = await _sut.GetAddressWithLandlord(addressId);

            // Assert

            mockLogger.Verify(x => x.LogError("Address with id: 1, hasn't been found in db."), Times.Once);

        }
        [Fact]
        public async Task GetAddressWithLandlordExceptionCatch()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithLandlord(addressId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetAddressWithLandlord(addressId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAddressWithLandlord(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }
        [Fact]
        public async Task GetAddressWithPropertyShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var addressId = 1;

            var address = new Address()
            {
                Id = addressId,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765",
                Property = new Property()
                {
                    Id = 1,
                    FlatLabel = "Apartament1",
                    RoomCount = 3,
                    FlatSize = 36,
                    HasGas = false,
                    HasHW = true,
                    HasHeat = false,
                    DisplayOnWeb = false
                }
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithProperty(addressId)).ReturnsAsync(address);

            // Act
            var result = await _sut.GetAddressWithProperty(addressId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAddressWithPropertyShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithProperty(addressId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAddressWithProperty(addressId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAddressWithPropertyShouldLogAppropriateMessageWhenAddressInDatabase()
        {
            // Arrange
            var addressId = 1;

            var address = new Address()
            {
                Id = addressId,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765",
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

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithProperty(addressId)).ReturnsAsync(address);

            // Act
            var result = await _sut.GetAddressWithProperty(addressId);


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Address with id: 1 and its Property."), Times.Once);

        }

        [Fact]
        public async Task GetAddressWithPropertyShouldLogAppropriateMessageWhenNoAddressesInDatabase()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithProperty(addressId)).ReturnsAsync(() => null);


            // Act
            var result = await _sut.GetAddressWithProperty(addressId);

            // Assert

            mockLogger.Verify(x => x.LogError("Address with id: 1, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetAddressWithPropertyExceptionCatch()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithProperty(addressId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetAddressWithProperty(addressId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAddressWithProperty(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetAddressWithTenantShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var addressId = 1;

            var address = new Address()
            {
                Id = addressId,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765",
                Tenant = new Tenant()
                {
                    Id = 1,
                    NIP = "9876500011",
                    PESEL = "09876500011",
                    IsCompany = true,
                    CompanyName = "Szlachta nie Pracuje",
                    PhonePrefix = "0048",
                    PhoneNumber = "888031303",
                    Name = "Janusz",
                    Surname = "Nowak",
                    DisplayOnWeb = true,
                    AddressId = 1,
                    AspNetUsersId = 69
                }
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithTenant(addressId)).ReturnsAsync(address);

            // Act
            var result = await _sut.GetAddressWithTenant(addressId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAddressWithTenantShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithTenant(addressId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAddressWithTenant(addressId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAddressWithTenantShouldLogAppropriateMessageWhenAddressInDatabase()
        {
            // Arrange
            var addressId = 1;

            var address = new Address()
            {
                Id = addressId,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765",
                Tenant = new Tenant()
                {
                    Id = 1,
                    NIP = "9876500011",
                    PESEL = "09876500011",
                    IsCompany = true,
                    CompanyName = "Szlachta nie Pracuje",
                    PhonePrefix = "0048",
                    PhoneNumber = "888031303",
                    Name = "Janusz",
                    Surname = "Nowak",
                    DisplayOnWeb = true,
                    AddressId = 1,
                    AspNetUsersId = 69
                }
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithTenant(addressId)).ReturnsAsync(address);

            // Act
            var result = await _sut.GetAddressWithTenant(addressId);


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Address with id: 1 and its Tenant."), Times.Once);

        }

        [Fact]
        public async Task GetAddressWithTenantShouldLogAppropriateMessageWhenNoAddressesInDatabase()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithTenant(addressId)).ReturnsAsync(() => null);


            // Act
            var result = await _sut.GetAddressWithTenant(addressId);

            // Assert

            mockLogger.Verify(x => x.LogError("Address with id: 1, hasn't been found in db."), Times.Once);

        }


        [Fact]
        public async Task GetAddressWithTenantExceptionCatch()
        {
            // Arrange
            var addressId = 1;


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressWithTenant(addressId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetAddressWithTenant(addressId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAddressWithTenant(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetAddressesByCityShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var city = "Sopot";
            var address = new Address()
            {
                Id = 1,
                Country = "Polska",
                City = city,
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByCity(city)).ReturnsAsync(new[] { address });

            // Act
            var result = await _sut.GetAddressesByCity(city);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetAddressesByCityShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var city = "Sopot";


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByCity(city)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAddressesByCity(city);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAddressesByCityShouldLogAppropriateMessageWhenAddressInDatabase()
        {
            // Arrange
            var city = "Sopot";

            var address = new Address()
            {
                Id = 1,
                Country = "Polska",
                City = city,
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByCity(city)).ReturnsAsync(new[] { address });

            // Act
            var result = await _sut.GetAddressesByCity(city);


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Addresses with city: Sopot, addresses found count: 1."), Times.Once);

        }
        [Fact]
        public async Task GetAddressesByCityShouldLogAppropriateMessageWhenNoAddressesInDatabase()
        {
            // Arrange
            var city = "Sopot";


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByCity(city)).ReturnsAsync(() => null);


            // Act
            var result = await _sut.GetAddressesByCity(city);

            // Assert

            mockLogger.Verify(x => x.LogError("Address with city: Sopot, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetAddressesByCityExceptionCatch()
        {
            // Arrange
            var city = "Sopot";


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByCity(city)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetAddressesByCity(city);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAddressByCity(city) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetAddressesByStreetShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var street = "Ciasna";
            var address = new Address()
            {
                Id = 1,
                Country = "Polska",
                City = "Sopot",
                Street = street,
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByStreet(street)).ReturnsAsync(new[] { address });

            // Act
            var result = await _sut.GetAddressesByStreet(street);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }


        [Fact]
        public async Task GetAddressesByStreetShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var street = "Ciasna";


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByStreet(street)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAddressesByStreet(street);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAddressesByStreetShouldLogAppropriateMessageWhenAddressInDatabase()
        {
            // Arrange
            var street = "Ciasna";
            var address = new Address()
            {
                Id = 1,
                Country = "Polska",
                City = "Sopot",
                Street = street,
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByStreet(street)).ReturnsAsync(new[] { address });

            // Act
            var result = await _sut.GetAddressesByStreet(street);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Addresses with Street: Ciasna, addresses found count: 1."), Times.Once);

        }

        [Fact]
        public async Task GetAddressesByStreetShouldLogAppropriateMessageWhenNoAddressesInDatabase()
        {
            // Arrange
            var street = "Ciasna";


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByStreet(street)).ReturnsAsync(() => null);


            // Act
            var result = await _sut.GetAddressesByStreet(street);

            // Assert

            mockLogger.Verify(x => x.LogError("Address with Street: Ciasna, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetAddressesByStreetExceptionCatch()
        {
            // Arrange
            var street = "Ciasna";


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByStreet(street)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetAddressesByStreet(street);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAddressesByStreet(streetName) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetAddressByCountryShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var country = "Polska";
            var address = new Address()
            {
                Id = 1,
                Country = country,
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByCountry(country)).ReturnsAsync(new[] { address });

            // Act
            var result = await _sut.GetAddressByCountry(country);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetAddressByCountryShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var country = "Polska";


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByCountry(country)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAddressByCountry(country);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAddressByCountryShouldLogAppropriateMessageWhenAddressInDatabase()
        {
            // Arrange
            var country = "Polska";
            var address = new Address()
            {
                Id = 1,
                Country = country,
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = "80-765"
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByCountry(country)).ReturnsAsync(new[] { address });

            // Act
            var result = await _sut.GetAddressByCountry(country);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Addresses with Country: Polska, addresses found count: 1."), Times.Once);

        }
        [Fact]
        public async Task GetAddressByCountryShouldLogAppropriateMessageWhenNoAddressesInDatabase()
        {
            // Arrange
            var country = "Polska";


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByCountry(country)).ReturnsAsync(() => null);


            // Act
            var result = await _sut.GetAddressByCountry(country);

            // Assert

            mockLogger.Verify(x => x.LogError("Address with Country: Polska, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetAddressByCountryExceptionCatch()
        {
            // Arrange
            var country = "Polska";


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByCountry(country)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetAddressByCountry(country);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAddressByCountry(countryName) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetAddressesByZipCodeShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var zip = "80-765";
            var address = new Address()
            {
                Id = 1,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = zip
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByZipCode(zip)).ReturnsAsync(new[] { address });

            // Act
            var result = await _sut.GetAddressesByZipCode(zip);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetAddressesByZipCodeShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var zip = "80-765";


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByZipCode(zip)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAddressesByZipCode(zip);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAddressesByZipCodeShouldLogAppropriateMessageWhenAddressInDatabase()
        {
            // Arrange
            var zip = "80-765";
            var address = new Address()
            {
                Id = 1,
                Country = "Polska",
                City = "Sopot",
                Street = "Ciasna",
                BuildingNumber = "1",
                FlatNumber = "69",
                PostalCode = zip
            };

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByZipCode(zip)).ReturnsAsync(new[] { address });


            // Act
            var result = await _sut.GetAddressesByZipCode(zip);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Addresses with ZipCode: 80-765, addresses found count: 1."), Times.Once);

        }

        [Fact]
        public async Task GetAddressesByZipCodeShouldLogAppropriateMessageWhenNoAddressesInDatabase()
        {
            // Arrange
            var zip = "80-765";


            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByZipCode(zip)).ReturnsAsync(() => null);


            // Act
            var result = await _sut.GetAddressesByZipCode(zip);

            // Assert

            mockLogger.Verify(x => x.LogError("Address with ZipCode: 80-765, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetAddressesByZipCodeExceptionCatch()
        {
            // Arrange
            var zip = "80-765";


             mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressByZipCode(zip)).ThrowsAsync(new Exception());

            
            // Act
            var result = await _sut.GetAddressesByZipCode(zip);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAddressesByZipCode(ZIP) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }
        [Fact]
        public async Task CreateAddressExceptionCatch()
        {
            // Arrange

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.CreateAddress(new Address())).Throws(new Exception());

            // Act
            var result = await _sut.CreateAddress(new AddressForCreationDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside CreateAddress(addressDto) action: Object reference not set to an instance of an object."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreateAddressShouldReturnBadRequestIfAddressIsNull()
        {
            // Arrange

           
            // Act
            var result = await _sut.CreateAddress(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
        [Fact]
        public async Task CreateAddressShouldLogAppropriateMessageIfAddressIsNull()
        {
            // Arrange

            

            // Act
            var result = await _sut.CreateAddress(null);

            // Assert
            mockLogger.Verify(x => x.LogError("Address received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task UpdateAddressdExceptionCatch()
        {
            /// Arrange
            var id = 3;

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressById(id)).Throws(new Exception());

            // Act
            var result = await _sut.UpdateAddress(id, new AddressForUpdateDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside UpdateAddress(id, address) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task UpdateAddressShouldReturnBadRequestIfAddressIsNull()
        {
            /// Arrange
            var id = 3;
      
            // Act
            var result = await _sut.UpdateAddress(id, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateAddressShouldLogAppropriateMessageIfAddressIsNull()
        {
            // Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdateAddress(id, null);

            // Assert
            mockLogger.Verify(x => x.LogError("Address received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task DeleteAddressExceptionCatch()
        {
            /// Arrange
            var id = 3;
            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressById(id)).Throws(new Exception());

            // Act
            var result = await _sut.DeleteAddress(id);

            // Assert
            //tocheck
            mockLogger.Verify(x => x.LogError("Something went wrong inside DeleteLandlord(id) action: System.Exception: Exception of type 'System.Exception' was thrown."), Times.Never);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task DeleteAddressShouldLogAppropriateMessageWhenNoAddressWithGivenIdInDatabase()
        {
            // Arrange
            var id = 3;

            mockRepo.Setup(x => x.Address).Returns(_addressRepoMock.Object);
            _addressRepoMock.Setup(x => x.GetAddressById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.DeleteAddress(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Address with id: 3, hasn't been found in db."), Times.Once);
        }

    }
}
