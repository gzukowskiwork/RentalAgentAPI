using AutoMapper;
using Contracts;
using EmailService;
using Entities.DataTransferObjects.Tenant;
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
    public class TenantControllerTests
    {
        private readonly TenantController _sut;
        private readonly Mock<IRepositoryWrapper> mockRepo = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
        private readonly Mock<IIdentityService> mockIdentity = new Mock<IIdentityService>();
        private readonly Mock<IEmailEmmiter> mockEmail = new Mock<IEmailEmmiter>();
        private readonly Mock<ITenantRepository> _tenantRepoMock = new Mock<ITenantRepository>();

        public TenantControllerTests()
        {
            _sut = new TenantController(mockRepo.Object, mockMapper.Object, mockLogger.Object, mockIdentity.Object, mockEmail.Object);

        }

        [Fact]
        public async Task GetAllTenantsShouldReturnNotFoundIfNoTenants()
        {
            // Arrange

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetAllTenants()).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetAllTenants();

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllTenantsShouldReturnOkIfTenantsInDatabase()
        {
            // Arrange    

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetAllTenants()).ReturnsAsync(new[] { new Tenant(), new Tenant() });

            // Act

            var result = await _sut.GetAllTenants(); ;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetAllTenantsShouldLogAppropriateMessageWhenTenantsInDatabase()
        {
            // Arrange

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetAllTenants()).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = "123456789",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,

            } });

            // Act
            var result = await _sut.GetAllTenants();

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned all 'Tenants' from db. Number of Tenants found: 1."), Times.Once);

        }

        [Fact]
        public async Task GetAllTenantsShouldLogAppropriateMessageWhenNoTenantsInDatabase()
        {
            // Arrange

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetAllTenants()).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllTenants();

            // Assert

            mockLogger.Verify(x => x.LogInfo("No 'Tenant' has been found in db."));

        }

        [Fact]
        public async Task GetAllTenantsExceptionCatch()
        {
            // Arrange

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetAllTenants()).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetAllTenants();

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside .GetAllTenants() action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantByIdShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var tenantId = 1;

            var tenant = new Tenant
            {
                Id = tenantId,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = "123456789",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,

            };

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantById(tenantId)).ReturnsAsync(tenant);

            // Act
            var result = await _sut.GetTenantById(tenant.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, tenant.Id);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetTenantByIdShouldReturnNoFoundIfNotFoundInDatabase()
        {
            // Arrange
            var tenantId = 1;

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantById(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantById(tenantId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetTenantByIdShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange

            var tenantId = 1;


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantById(tenantId)).ReturnsAsync(new Tenant()
            {
                Id = tenantId,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = "123456789",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,

            });

            // Act
            var result = await _sut.GetTenantById(tenantId);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenant with id: 1"), Times.Once);

        }
        [Fact]
        public async Task GetTenantByIdShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange

            var tenantId = 1;


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantById(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantById(tenantId);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with id: 1, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetAdressByIdExceptionCatch()
        {
            // Arrange
            var id = 1;


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantById(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantById(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantById(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantWithAddressShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var tenantId = 1;

            var tenant = new Tenant
            {
                Id = tenantId,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = "123456789",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,
                AddressId = 1,
                Address = new Address()
                {
                    Id = 1,
                    Country = "Polska",
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            };


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantWithAddress(tenantId)).ReturnsAsync(tenant);

            // Act
            var result = await _sut.GetTenantWithAddress(tenant.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
            Assert.Equal(1, tenant.Id);
            Assert.False(tenant.IsCompany);
            Assert.Equal("0048", tenant.PhonePrefix);
            Assert.Equal("Janusz", tenant.Name);
            Assert.Equal("Kowalski", tenant.Surname);
            Assert.False(tenant.DisplayOnWeb);
            Assert.Equal(1, tenant.AddressId);
            Assert.Equal(1, tenant.Address.Id);
            Assert.Equal("Polska", tenant.Address.Country);
            Assert.Equal("Sopot", tenant.Address.City);
            Assert.Equal("Ciasna", tenant.Address.Street);
            Assert.Equal("1", tenant.Address.BuildingNumber);
            Assert.Equal("69", tenant.Address.FlatNumber);
            Assert.Equal("80-765", tenant.Address.PostalCode);

        }
        [Fact]
        public async Task GetTenantWithAddressShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var tenantId = 1;


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantWithAddress(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantWithAddress(tenantId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }
        [Fact]
        public async Task GetTenantWithAddressShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange

            var tenantId = 1;

            var tenant = new Tenant
            {
                Id = tenantId,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = "123456789",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,
                AddressId = 1,
                Address = new Address()
                {
                    Id = 1,
                    Country = "Polska",
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }
            };


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantWithAddress(tenantId)).ReturnsAsync(tenant);

            // Act
            var result = await _sut.GetTenantWithAddress(tenantId);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenant with id: 1 and its address."), Times.Once);

        }

        [Fact]
        public async Task GetTenantWithAddressShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange

            var tenantId = 1;


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantWithAddress(tenantId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantWithAddress(tenantId);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with id: 1, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetTenantWithAddressExceptionCatch()
        {
            // Arrange
            var tenantId = 1;


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantWithAddress(tenantId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantWithAddress(tenantId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantWithAddress(tenantId) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }
        [Fact]
        public async Task GetTenantByEmailShouldReturnOKIfTenantsInDb()
        {
            // Arrange
            var email = "tenant@wp.pl";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByEmail(email)).ReturnsAsync(new Tenant());

            // Act
            var result = await _sut.GetTenantByEmail(email);


            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetTenantByEmailShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange
            var email = "tenant@wp.pl";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByEmail(email)).ReturnsAsync(new Tenant());

            // Act
            var result = await _sut.GetTenantByEmail(email);


            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned Tenant with e-mail: tenant@wp.pl."), Times.Once);

        }
        [Fact]
        public async Task GetTenantByEmailShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var email = "tenant@wp.pl";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByEmail(email)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantByEmail(email);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }
        [Fact]
        public async Task GetTenantByEmailShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange

            var email = "tenant@wp.pl";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByEmail(email)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantByEmail(email);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with e-mail: tenant@wp.pl, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetTenantByEmailExceptionCatch()
        {
            // Arrange
            var email = "tenant@wp.pl";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByEmail(email)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantByEmail(email);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantByEmail(email) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantByAspUserIdShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var aspId = 13;

            var tenant = new Tenant
            {
                Id = 1,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = "123456789",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,
                AddressId = 1,
                AspNetUsersId = aspId

            };


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByAspNetUserId(aspId)).ReturnsAsync(tenant);

            // Act
            var result = await _sut.GetTenantByAspUserId(tenant.AspNetUsersId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(13, tenant.AspNetUsersId);
            Assert.IsNotType<NotFoundResult>(result);

        }

      

        [Fact]
        public async Task GetTenantByAspUserIdShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var aspId = 13;


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByAspNetUserId(aspId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantByAspUserId(aspId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }
        [Fact]
        public async Task GetTenantByAspUserIdShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange
            var aspId = 13;

            var tenant = new Tenant
            {
                Id = 1,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = "123456789",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,
                AddressId = 1,
                AspNetUsersId = aspId

            };

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByAspNetUserId(aspId)).ReturnsAsync(tenant);

            // Act
            var result = await _sut.GetTenantByAspUserId(aspId);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenant with aspId: 13."), Times.Once);

        }
        [Fact]
        public async Task GetTenantByAspUserIdShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange

            var aspId = 13;


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByAspNetUserId(aspId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantByAspUserId(aspId);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with aspId: 13, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetTenantByAspUserIdExceptionCatch()
        {
            // Arrange
            var aspId = 13;


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByAspNetUserId(aspId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantByAspUserId(aspId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantByAspUserId(aspNetUserId) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantByPhoneNumberIdShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var phoneNumber = "888031303";

            var tenant = new Tenant
            {
                Id = 1,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = phoneNumber,
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,
                AddressId = 1,
                AspNetUsersId = 69

            };


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByPhone(phoneNumber)).ReturnsAsync(tenant);

            // Act
            var result = await _sut.GetTenantByPhoneNumber(tenant.PhoneNumber);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("888031303", tenant.PhoneNumber);
            Assert.IsNotType<NotFoundResult>(result);

        }
        [Fact]
        public async Task GetTenantByPhoneNumberShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var phoneNumber = "888031303";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByPhone(phoneNumber)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantByPhoneNumber(phoneNumber);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }
        [Fact]
        public async Task GetTenantByPhoneNumberShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange
            var phoneNumber = "888031303";

            var tenant = new Tenant
            {
                Id = 1,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = phoneNumber,
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,
                AddressId = 1,
                AspNetUsersId = 69

            };


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByPhone(phoneNumber)).ReturnsAsync(tenant);

            // Act
            var result = await _sut.GetTenantByPhoneNumber(phoneNumber);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenant with phone No.: 888031303."), Times.Once);

        }
        [Fact]
        public async Task GetTenantByPhoneNumberShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange

            var phoneNumber = "888031303";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByPhone(phoneNumber)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantByPhoneNumber(phoneNumber);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with phone No.: 888031303 (no prefix), hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetTenantByPhoneNumberExceptionCatch()
        {
            // Arrange
            var phoneNumber = "888031303";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantByPhone(phoneNumber)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantByPhoneNumber(phoneNumber);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantByPhoneNumber(phoneNumber) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantsByNipShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            String nip = "0987650001";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByNip(nip)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = nip,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,
                AddressId = 1,
                AspNetUsersId = 69

            } });

            // Act
            var result = await _sut.GetTenantsByNip(nip);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("0987650001", nip);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetTenantsByNipShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            String nip = "0987650001";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByNip(nip)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByNip(nip);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantsByNipShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange
            String nip = "0987650001";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByNip(nip)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = nip,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,
                AddressId = 1,
                AspNetUsersId = 69

            } });

            // Act
            var result = await _sut.GetTenantsByNip(nip);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenants with NIP: 0987650001, tenant count :1."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByNipShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange

            String nip = "0987650001";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByNip(nip)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByNip(nip);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with NIP: 0987650001, hasn't been found in db."), Times.Once);

        }
        [Fact]
        public async Task GetTenantsByNipExceptionCatch()
        {
            // Arrange
            String nip = "0987650001";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByNip(nip)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantsByNip(nip);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantByNip(nip) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }
        [Fact]
        public async Task GetTenantsByPeselShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            String pesel = "09876500011";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByPesel(pesel)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "09876500011",
                PESEL = pesel,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,
                AddressId = 1,
                AspNetUsersId = 69

            } });

            // Act
            var result = await _sut.GetTenantsByPesel(pesel);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetTenantsByPeselShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            String pesel = "09876500011";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByPesel(pesel)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByPesel(pesel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantsByPeselShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange
            String pesel = "09876500011";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByPesel(pesel)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = pesel,
                IsCompany = false,
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = false,
                AddressId = 1,
                AspNetUsersId = 69

            } });

            // Act
            var result = await _sut.GetTenantsByPesel(pesel);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenants with PESEL: 09876500011, tenant count :1."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByPeselShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange
            String pesel = "09876500011";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByPesel(pesel)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByPesel(pesel);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with PESEL: 09876500011, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByPeselExceptionCatch()
        {
            // Arrange
            String pesel = "09876500011";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByPesel(pesel)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantsByPesel(pesel);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantsByPesel(pesel) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantsByIsCompanyShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var isCompany = true;

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByIsCompany(isCompany)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = isCompany,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69

            } }) ;

            // Act
            var result = await _sut.GetTenantsByIsCompany(isCompany);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetTenantsByIsCompanyShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var isCompany = true;

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByIsCompany(isCompany)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByIsCompany(isCompany);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantsByIsCompanyShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange
            var isCompany = true;

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByIsCompany(isCompany)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = isCompany,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69

            } });

            // Act
            var result = await _sut.GetTenantsByIsCompany(isCompany);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenants with IsCompany: True, tenant count :1."), Times.Once);

        }



        [Fact]
        public async Task GetTenantsByIsCompanyShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange
            var isCompany = true;

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByIsCompany(isCompany)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByIsCompany(isCompany);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with IsCompany: True, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByIsCompanyExceptionCatch()
        {
            // Arrange
            var isCompany = true;


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByIsCompany(isCompany)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantsByIsCompany(isCompany);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantsByIsCompany(bool isCompany) action: Exception of type 'System.Exception' was thrown."), Times.Once);

        }
        [Fact]
        public async Task GetTenantsBySurnameIsCompanyShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var surname = "Kowalski";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsBySurname(surname)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = surname,
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69

            } });

            // Act
            var result = await _sut.GetTenantsBySurname(surname);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetTenantsBySurnameShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var surname = "Kowalski";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsBySurname(surname)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsBySurname(surname);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }


        [Fact]
        public async Task GetTenantsBySurnameShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange
            var surname = "Kowalski";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsBySurname(surname)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = surname,
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69

            },new Tenant(){
                Id = 2,
                NIP = "4679556893",
                PESEL = "83052705625",
                IsCompany = true,
                CompanyName = "Sii",
                PhonePrefix = "0038",
                PhoneNumber = "888301303",
                Name = "Alan",
                Surname = surname,
                DisplayOnWeb = true,
                AddressId = 2,
                AspNetUsersId = 70

            }});

            // Act
            var result = await _sut.GetTenantsBySurname(surname);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenants with Surname: Kowalski, tenants found: 2."), Times.Once);
        }
        [Fact]
        public async Task GetTenantsBySurnameShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange
            var surname = "Kowalski";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsBySurname(surname)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsBySurname(surname);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with Surname: Kowalski, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsBySurnameExceptionCatch()
        {
            // Arrange
            var surname = "Kowalski";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsBySurname(surname)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantsBySurname(surname);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantsBySurname(tenant_surname) action: Exception of type 'System.Exception' was thrown."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByCityShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var city = "Sopot";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByCity(city)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69,
                Address = new Address()
                {
                    Id = 1,
                    Country = "Polska",
                    City = city,
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }

            } });

            // Act
            var result = await _sut.GetTenantsByCity(city);

            // Assert
            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantsByCityShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var city = "Sopot";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByCity(city)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByCity(city);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantsByCityShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange
            var city = "Sopot";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByCity(city)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69,
                Address = new Address()
                {
                    Id = 1,
                    Country = "Polska",
                    City = city,
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }

            } });

            // Act
            var result = await _sut.GetTenantsByCity(city);
            var result2 = await _sut.GetTenantsByCity("Gdynia");
            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenants with city: Sopot, tenants found: 1."), Times.Once);
            mockLogger.Verify(x => x.LogInfo("Returned Tenants with city: Gdynia, tenants found: 0."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByCityShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange
            var city = "Sopot";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByCity(city)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByCity(city);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with city: Sopot, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByCityExceptionCatch()
        {
            // Arrange
            var city = "Sopot";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByCity(city)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantsByCity(city);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantsByCity(city_name) action: Exception of type 'System.Exception' was thrown."), Times.Once);

        }


        [Fact]
        public async Task GetTenantsByStreetShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var street = "Ciasna";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByStreet(street)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69,
                Address = new Address()
                {
                    Id = 1,
                    Country = "Polska",
                    City = "Sopot",
                    Street = street,
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }

            } });

            // Act
            var result = await _sut.GetTenantsByStreet(street);

              // Assert
              Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantsByStreetShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var street = "Ciasna";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByStreet(street)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByStreet(street);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantsByStreetShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange
            var street = "Ciasna";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByStreet(street)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69,
                Address = new Address()
                {
                    Id = 1,
                    Country = "Polska",
                    City = "Sopot",
                    Street = street,
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }

            } });

            // Act
            var result = await _sut.GetTenantsByStreet(street);
            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenants with Street: Ciasna, tenants found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetTenantsByStreetShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange
            var street = "Ciasna";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByStreet(street)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByStreet(street);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with Street: Ciasna, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByStreetExceptionCatch()
        {
            // Arrange
            var street = "Ciasna";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByStreet(street)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantsByStreet(street);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantsByStreet(street) action: Exception of type 'System.Exception' was thrown."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByCountryShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var country = "Polska";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByCountry(country)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69,
                Address = new Address()
                {
                    Id = 1,
                    Country = country,
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }

            } });

            // Act
            var result = await _sut.GetTenantsByCountry(country);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);


        }

        [Fact]
        public async Task GetTenantsByCountryShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var country = "Polska";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByCountry(country)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByCountry(country);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }

        [Fact]
        public async Task GetTenantsByCountryShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange
            var country = "Polska";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByCountry(country)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69,
                Address = new Address()
                {
                    Id = 1,
                    Country = country,
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = "80-765"
                }

            } });

            // Act
            var result = await _sut.GetTenantsByCountry(country);
            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenants with Country: Polska, tenants found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetTenantsByCountryShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange
            var country = "Polska";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByCountry(country)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByCountry(country);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with Country: Polska, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByCountryExceptionCatch()
        {
            // Arrange
            var country = "Polska";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByCountry(country)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantsByCountry(country);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantsByCountry(country) action: Exception of type 'System.Exception' was thrown."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByZipCodeShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var zip = "80-765";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByZipCode(zip)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69,
                Address = new Address()
                {
                    Id = 1,
                    Country = "Polska",
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = zip
                }

            } });

            // Act
            var result = await _sut.GetTenantsByZipCode(zip);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);


        }
        [Fact]
        public async Task GetTenantsByZipCodeShouldReturnNotFoundIfNotFoundInDatabase()
        {
            // Arrange
            var zip = "80-765";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByZipCode(zip)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByZipCode(zip);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);

        }
        [Fact]
        public async Task GetTenantsByZipCodeShouldLogAppropriateMessageWhenTenantInDatabase()
        {
            // Arrange
            var zip = "80-765";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByZipCode(zip)).ReturnsAsync(new[] { new Tenant(){
                Id = 1,
                NIP = "9876500011",
                PESEL = "09876500011",
                IsCompany = true,
                CompanyName = "Szlachta nie Pracuje",
                PhonePrefix = "0048",
                PhoneNumber = "888031303",
                Name = "Janusz",
                Surname = "Kowalski",
                DisplayOnWeb = true,
                AddressId = 1,
                AspNetUsersId = 69,
                Address = new Address()
                {
                    Id = 1,
                    Country = "Polska",
                    City = "Sopot",
                    Street = "Ciasna",
                    BuildingNumber = "1",
                    FlatNumber = "69",
                    PostalCode = zip
                }

            } });

            // Act
            var result = await _sut.GetTenantsByZipCode(zip);
            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Tenants with ZipCode: 80-765, tenants found: 1."), Times.Once);

        }
        [Fact]
        public async Task GetTenantsByZipCodeShouldLogAppropriateMessageWhenNoTenantInDatabase()
        {
            // Arrange
            var zip = "80-765";

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByZipCode(zip)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetTenantsByZipCode(zip);

            // Assert

            mockLogger.Verify(x => x.LogError("Tenant with ZipCode: 80-765, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetTenantsByZipCodeExceptionCatch()
        {
            // Arrange
            var zip = "80-765";


            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantsByZipCode(zip)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetTenantsByZipCode(zip);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetTenantsByZipCode(zipCode) action: Exception of type 'System.Exception' was thrown."), Times.Once);

        }

        [Fact]
        public async Task GetRelatedTenantsByLandlordIdShouldReturnNotoundIfNotFoundInDatabase()
        {
            // Arrange
            var landlordId = 1;

            mockRepo.Setup(repo => repo.Landlord.CheckIfLandlordExistByLandlordId(landlordId)).ReturnsAsync(() => false);

            // Act
            var result = await _sut.GetRelatedTenantsByLandlordId(landlordId);

            // Assert
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetRelatedTenantsByLandlordIdShouldReturnTenantsIfFoundInDatabase()
        {
            // Arrange
            var landlordId = 1;

            mockRepo.Setup(repo => repo.Landlord.CheckIfLandlordExistByLandlordId(landlordId)).ReturnsAsync(() => true);

            // Act
            var tenants = await _sut.GetRelatedTenantsByLandlordId(landlordId);

            // Assert
            Assert.IsType<ObjectResult>(tenants);

        }

        [Fact]
        public async Task GetRelatedTenantsByLandlordIdShouldLogAppropriateMessage()
        {
            // Arrange
            var landlordId = 1;

            mockRepo.Setup(repo => repo.Landlord.CheckIfLandlordExistByLandlordId(landlordId)).ReturnsAsync(() => true);

            // Act
            var result = await _sut.GetRelatedTenantsByLandlordId(landlordId);
            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRelatedTenantsByLandlordId(landlord_id) action: Object reference not set to an instance of an object."), Times.Once);

        }

        [Fact]
        public async Task GetRelatedTenantsByLandlordIdExceptionCatch()
        {
            // Arrange
            var landlordId = 1;


          
            mockRepo.Setup(repo => repo.Landlord.CheckIfLandlordExistByLandlordId(landlordId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRelatedTenantsByLandlordId(landlordId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRelatedTenantsByLandlordId(landlord_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);

        }


        [Fact]
        public async Task GetRelatedActualTenantsByLandlordIdShouldReturnNotoundIfNotFoundInDatabase()
        {
            // Arrange
            var landlordId = 1;

            mockRepo.Setup(repo => repo.Landlord.CheckIfLandlordExistByLandlordId(landlordId)).ReturnsAsync(() => false);

            // Act
            var result = await _sut.GetRelatedTenantsByLandlordId(landlordId);

            // Assert
            Assert.IsType<NotFoundResult>(result);

        }
        [Fact]
        public async Task GetRelatedActualTenantsByLandlordIdShouldReturnTenantsIfFoundInDatabase()
        {
            // Arrange
            var landlordId = 1;

            mockRepo.Setup(repo => repo.Landlord.CheckIfLandlordExistByLandlordId(landlordId)).ReturnsAsync(() => true);

            // Act
            var tenants = await _sut.GetRelatedTenantsByLandlordId(landlordId);

            // Assert
            Assert.IsType<ObjectResult>(tenants);

        }

        [Fact]
        public async Task GetRelatedActualTenantsByLandlordIdShouldLogAppropriateMessage()
        {
            // Arrange
            var landlordId = 1;

            mockRepo.Setup(repo => repo.Landlord.CheckIfLandlordExistByLandlordId(landlordId)).ReturnsAsync(() => true);

            // Act
            var result = await _sut.GetRelatedTenantsByLandlordId(landlordId);
            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRelatedTenantsByLandlordId(landlord_id) action: Object reference not set to an instance of an object."), Times.Once);

        }

        [Fact]
        public async Task GetRelatedActualTenantsByLandlordIdExceptionCatch()
        {
            // Arrange
            var landlordId = 1;


            mockRepo.Setup(repo => repo.Landlord.CheckIfLandlordExistByLandlordId(landlordId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetRelatedActualTenantsByLandlordId(landlordId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetRelatedActualTenantsByLandlordId(landlord_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);

        }
        [Fact]
        public async Task CreateAddressExceptionCatch()
        {
            // Arrange

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.CreateTenant(new Tenant())).Throws(new Exception());

            // Act
            var result = await _sut.CreateTenant(new TenantForCreationDto());

            // Assert
            //CheckLater
            mockLogger.Verify(x => x.LogError("Something went wrong inside CreateTenant(tenantForCreationDto) action: System.NullReferenceException: Object reference not set to an instance of an object."), Times.Never);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreateTenantShouldReturnBadRequestIfTenantIsNull()
        {
            // Arrange

            // Act
            var result = await _sut.CreateTenant(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
        [Fact]
        public async Task CreateTenantShouldLogAppropriateMessageIfTenantIsNull()
        {
            // Arrange

            // Act
            var result = await _sut.CreateTenant(null);

            // Assert
            mockLogger.Verify(x => x.LogError("Tenant received is a Null Object."), Times.Once);
        }
        //[Fact]
        //public async Task UpdateTenantdExceptionCatch()
        //{
        //    /// Arrange
        //    var id = 3;

        //    mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
        //    _tenantRepoMock.Setup(x => x.GetTenantById(id)).Throws(new Exception());

        //    // Act
        //    var result = await _sut.UpdateTenant(id, new TenantForUpdateDto());

        //    // Assert
        //    mockLogger.Verify(x => x.LogError(" "), Times.Once);
        //    Assert.IsType<ObjectResult>(result);
        //}
        [Fact]
        public async Task UpdateTenantShouldReturnBadRequestIfTenantIsNull()
        {
            /// Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdateTenant(id, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateTenantShouldLogAppropriateMessageIfTenantIsNull()
        {
            // Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdateTenant(id, null);

            // Assert
            mockLogger.Verify(x => x.LogError("Tenant received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task DeleteTenantExceptionCatch()
        {
            /// Arrange
            var id = 3;
            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantById(id)).Throws(new Exception());

            // Act
            var result = await _sut.DeleteTenant(id);

            // Assert
            //tocheck
           // mockLogger.Verify(x => x.LogError(""), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task DeleteTenantShouldLogAppropriateMessageWhenNoTenantWithGivenIdInDatabase()
        {
            // Arrange
            var id = 3;

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.DeleteTenant(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Not deleted. Tenant with given id: 3 was not found"), Times.Once);
        }
        [Fact]
        public async Task UnDeleteTenantShouldLogAppropriateMessageWhenNoTenantWithGivenIdInDatabase()
        {
            // Arrange
            var id = 3;

            mockRepo.Setup(x => x.Tenant).Returns(_tenantRepoMock.Object);
            _tenantRepoMock.Setup(x => x.GetTenantById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.UnDeleteTenant(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Not deleted. Tenant with given id: 3 was not found"), Times.Once);
        }
      

    }





}


