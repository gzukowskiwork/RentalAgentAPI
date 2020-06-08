using AutoMapper;
using Contracts;
using DinkToPdf.Contracts;
using EmailService;
using Entities.DataTransferObjects.Invoice;
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
    public class InvoiceControllerTests
    {
        private readonly InvoiceController _sut;
        private readonly Mock<IRepositoryWrapper> mockRepo = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
        private readonly Mock<IIdentityService> mockIdentity = new Mock<IIdentityService>();
        private readonly Mock<IEmailEmmiter> mockEmail = new Mock<IEmailEmmiter>();
        private readonly Mock<IInvoiceRepository> _invoiceRepoMock = new Mock<IInvoiceRepository>();
        private readonly Mock<IConverter> mockConverter = new Mock<IConverter>();

        public InvoiceControllerTests()
        {
            _sut = new InvoiceController(mockRepo.Object, mockMapper.Object, mockLogger.Object, mockConverter.Object, mockEmail.Object);

        }

        [Fact]
        public async Task GetAllInvoicesShouldReturnNotFoundIfNoInvoicesInDb()
        {
            // Arrange

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetAllInvoices()).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetAllInvoices();

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllInvoicesShouldReturnOKIfInvoicesInDb()
        {
            // Arrange

            var invoice = new Invoice()
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
                EnergyVAT = 23

            };

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetAllInvoices()).ReturnsAsync(new[] { invoice });


            // Act
            var result = await _sut.GetAllInvoices();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllInvoicesShouldLogAppropriateMessageWhenNoinvoicesInDatabase()
        {
            // Arrange

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetAllInvoices()).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllInvoices();

            // Assert

            mockLogger.Verify(x => x.LogInfo("No 'Invoice' has been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetAllInvoicesShouldLogAppropriateMessageWhenInvoicesInDatabase()
        {
            // Arrange

            var invoice = new Invoice()
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
                EnergyVAT = 23

            };

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetAllInvoices()).ReturnsAsync(new[] { invoice });


            // Act
            var result = await _sut.GetAllInvoices();


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned all 'Invoices' from db, result count: 1."), Times.Once);

        }

        [Fact]
        public async Task GetAllInvoicesExceptionCatch()
        {
            // Arrange

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetAllInvoices()).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetAllInvoices();

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAllInvoices() action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetInvoiceByInvoiceIdShouldReturnOKIfInvoicesInDb()
        {
            // Arrange

            var invoiceId = 1;

            var invoice = new Invoice()
            {
                Id = invoiceId,
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
                EnergyVAT = 23

            };

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceById(invoiceId)).ReturnsAsync( invoice );


            // Act
            var result = await _sut.GetInvoiceByInvoiceId(invoiceId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetInvoiceByInvoiceIdShouldLogAppropriateMessageWhenInvoicesInDatabase()
        {
            // Arrange

            var invoiceId = 1;

            var invoice = new Invoice()
            {
                Id = invoiceId,
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
                EnergyVAT = 23

            };

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceById(invoiceId)).ReturnsAsync( invoice );


            // Act
            var result = await _sut.GetInvoiceByInvoiceId(invoiceId);


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Inovice by invoice_id: 1"), Times.Once);

        }

        [Fact]
        public async Task GetInvoiceByInvoiceIdShouldReturnNotFoundIfNoInvoicesInDb()
        {
            // Arrange

            var invoiceId = 1;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceById(invoiceId)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetInvoiceByInvoiceId(invoiceId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetInvoiceByInvoiceIdShouldLogAppropriateMessageWhenNoinvoicesInDatabase()
        {
            // Arrange
            var invoiceId = 1;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceById(invoiceId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetInvoiceByInvoiceId(invoiceId);

            // Assert

            mockLogger.Verify(x => x.LogError("Invoice with id: 1, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetInvoiceByInvoiceIdExceptionCatch()
        {
            // Arrange

            var invoiceId = 1;

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceById(invoiceId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetInvoiceByInvoiceId(invoiceId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetInvoiceById(invoice_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetInvoiceByStateIdShouldReturnOKIfInvoicesInDb()
        {
            // Arrange

            var stateId = 55;

            var invoice = new Invoice()
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
                StateId = stateId

            };

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceByStateId(stateId)).ReturnsAsync(invoice);


            // Act
            var result = await _sut.GetInvoiceByStateId(stateId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetInvoiceByStateIdShouldLogAppropriateMessageWhenInvoicesInDatabase()
        {
            // Arrange

            var stateId = 55;

            var invoice = new Invoice()
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
                StateId = stateId

            };

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceByStateId(stateId)).ReturnsAsync(invoice);


            // Act
            var result = await _sut.GetInvoiceByStateId(stateId);


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Inovice by state_id: 55"), Times.Once);

        }

        [Fact]
        public async Task GetInvoiceByStateIdShouldReturnNotFoundIfNoInvoicesInDb()
        {
            // Arrange

            var stateId = 55;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceByStateId(stateId)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetInvoiceByStateId(stateId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetInvoiceByStateIdShouldLogAppropriateMessageWhenNoinvoicesInDatabase()
        {
            // Arrange
            var stateId = 55;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceByStateId(stateId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetInvoiceByStateId(stateId);

            // Assert

            mockLogger.Verify(x => x.LogError("Invoice with StateId: 55, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetInvoiceByStateIdExceptionCatch()
        {
            // Arrange

            var stateId = 55;

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceByStateId(stateId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetInvoiceByStateId(stateId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetInvoiceByStateId(state_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetInvoicesByRentIdShouldReturnOKIfInvoicesInDb()
        {
            // Arrange

            var rentId = 13;

            var invoice = new Invoice()
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
                StateId = 55,
                RentId = rentId

            };

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoicesByRentId(rentId)).ReturnsAsync(new[] { invoice });


            // Act
            var result = await _sut.GetInvoicesByRentId(rentId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetInvoicesByRentIdShouldLogAppropriateMessageWhenInvoicesInDatabase()
        {
            // Arrange

            var rentId = 13;

            var invoice = new Invoice()
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
                StateId = 55,
                RentId = rentId

            };

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoicesByRentId(rentId)).ReturnsAsync(new[] { invoice });


            // Act
            var result = await _sut.GetInvoicesByRentId(rentId);


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Invoices with rent_id: 13, result count: 1."), Times.Once);

        }

        [Fact]
        public async Task GetInvoicesByRentIdShouldReturnNotFoundIfNoInvoicesInDb()
        {
            // Arrange

            var rentId = 13;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoicesByRentId(rentId)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetInvoicesByRentId(rentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetInvoicesByRentIdShouldLogAppropriateMessageWhenNoinvoicesInDatabase()
        {
            // Arrange

            var rentId = 13;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoicesByRentId(rentId)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetInvoicesByRentId(rentId);

            // Assert

            mockLogger.Verify(x => x.LogError("Invoice with rent_id: 13, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetInvoicesByRentIdExceptionCatch()
        {
            // Arrange

            var rentId = 13;

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoicesByRentId(rentId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetInvoicesByRentId(rentId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetInvoicesByRentId(rent_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetInvoiceWithRentByInvoiceIdShouldReturnOKIfInvoicesInDb()
        {
            // Arrange

            var id = 5;

            var invoice = new Invoice()
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
                StateId = 55,
                RentId = 85

            };

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceWithRentByInvoiceId(id)).ReturnsAsync( invoice );


            // Act
            var result = await _sut.GetInvoiceWithRentByInvoiceId(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetInvoiceWithRentByInvoiceIdShouldLogAppropriateMessageWhenInvoicesInDatabase()
        {
            // Arrange

            var id = 5;

            var invoice = new Invoice()
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
                StateId = 55,
                RentId = 85

            };

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceWithRentByInvoiceId(id)).ReturnsAsync(invoice);


            // Act
            var result = await _sut.GetInvoiceWithRentByInvoiceId(id);



            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Invoice with invoice_id: 5 and related Rent."), Times.Once);

        }


        [Fact]
        public async Task GetInvoiceWithRentByInvoiceIdShouldReturnNotFoundIfNoInvoicesInDb()
        {
            // Arrange

            var id = 4;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceWithRentByInvoiceId(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetInvoiceWithRentByInvoiceId(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetInvoiceWithRentByInvoiceIdShouldLogAppropriateMessageWhenNoinvoicesInDatabase()
        {
            
            // Arrange

            var id = 4;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceWithRentByInvoiceId(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetInvoiceWithRentByInvoiceId(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Invoice with id: 4, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetInvoiceWithRentByInvoiceIdExceptionCatch()
        {
            // Arrange

            var id = 4;

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceWithRentByInvoiceId(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetInvoiceWithRentByInvoiceId(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetInvoiceWithRentByInvoiceId(invoice_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetInvoiceWithStateByInvoiceIdShouldReturnOKIfInvoicesInDb()
        {
            // Arrange

            var id = 5;

            var invoice = new Invoice()
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
                StateId = 55,
                RentId = 85

            };

             mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
             _invoiceRepoMock.Setup(x => x.GetInvoiceWithStateByInvoiceId(id)).ReturnsAsync(invoice);


            // Act
            var result = await _sut.GetInvoiceWithStateByInvoiceId(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetInvoiceWithStateByInvoiceIdShouldLogAppropriateMessageWhenInvoicesInDatabase()
        {
            // Arrange

            var id = 5;

            var invoice = new Invoice()
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
                StateId = 55,
                RentId = 85

            };

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceWithStateByInvoiceId(id)).ReturnsAsync(invoice);


            // Act
            var result = await _sut.GetInvoiceWithStateByInvoiceId(id);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned Invoice with id: 5 and related State."), Times.Once);

        }

        [Fact]
        public async Task GetInvoiceWithStateByInvoiceIdShouldReturnNotFoundIfNoInvoicesInDb()
        {
            // Arrange

            var id = 9;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceWithStateByInvoiceId(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetInvoiceWithStateByInvoiceId(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetInvoiceWithStateByInvoiceIdShouldLogAppropriateMessageWhenNoinvoicesInDatabase()
        {


            // Arrange

            var id = 9;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceWithStateByInvoiceId(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetInvoiceWithStateByInvoiceId(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Invoice with id: 9, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetInvoiceWithStateByInvoiceIdExceptionCatch()
        {
            // Arrange

            var id = 9;

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceWithStateByInvoiceId(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetInvoiceWithStateByInvoiceId(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetInvoiceWithStateByInvoiceId(invoice_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetInvoiceDocumentStoredByInvoiceIdShouldReturnOKIfInvoicesInDb()
        {
            // Arrange

            var id = 5;

            var invoice = new Invoice();
            

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceById(id)).ReturnsAsync(invoice );

            // Act
            var result = await _sut.GetInvoiceDocumentStoredByInvoiceId(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetInvoiceDocumentStoredByInvoiceIdShouldLogAppropriateMessageWhenInvoicesInDatabase()
        {
            // Arrange

            var id = 5;

            var invoice = new Invoice();


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceById(id)).ReturnsAsync(invoice);

            // Act
            var result = await _sut.GetInvoiceDocumentStoredByInvoiceId(id);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned row Invoice, Invoice Document, invoice and tenant id, with id: 5"), Times.Once);

        }
        [Fact]
        public async Task GetInvoiceDocumentStoredByInvoiceIdShouldReturnNotFoundIfNoInvoicesInDb()
        {
            // Arrange

            var id = 9;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceById(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetInvoiceDocumentStoredByInvoiceId(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetInvoiceDocumentStoredByInvoiceIddShouldLogAppropriateMessageWhenNoinvoicesInDatabase()
        {


            // Arrange

            var id = 9;


            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceById(id)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetInvoiceWithStateByInvoiceId(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Invoice with id: 9, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetInvoiceDocumentStoredByInvoiceIdExceptionCatch()
        {
            // Arrange

            var id = 9;

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceWithStateByInvoiceId(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetInvoiceWithStateByInvoiceId(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetInvoiceWithStateByInvoiceId(invoice_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task CreateInvoiceExceptionCatch()
        {
            // Arrange
            var invoice = new Invoice();

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.CreateInvoice(invoice)).Throws(new Exception());

            // Act
            var result = await _sut.CreateInvoice(new InvoiceForCreationDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside CreateInvoice() action: Object reference not set to an instance of an object."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreateInvoiceShouldReturnBadRequestIfInvoiceIsNull()
        {
            // Arrange


            // Act
            var result = await _sut.CreateInvoice(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
        [Fact]
        public async Task CreateInvoiceShouldLogAppropriateMessageIfInvoiceIsNull()
        {
            // Arrange

            // Act
            var result = await _sut.CreateInvoice(null);

            // Assert
            mockLogger.Verify(x => x.LogError("Invoice received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task UpdateInvoiceExceptionCatch()
        {
            // Arrange


            var invoice = new Invoice();

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.UpdateInvoice(invoice)).Throws(new Exception());

            // Act
            var result = await _sut.UpdateInvoice(1, new InvoiceForUpdateDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside UpdateInvoice(id, invoiceForUpdateDto) action: Object reference not set to an instance of an object."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task UpdateInvoiceShouldReturnBadRequestIfInvoiceIsNull()
        {
            /// Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdateInvoice(id, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateInvoiceShouldLogAppropriateMessageIfInvoiceIsNull()
        {
            // Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdateInvoice(id, null);

            // Assert
            mockLogger.Verify(x => x.LogError("Invoice received is a Null Object."), Times.Once);
        }

        [Fact]
        public async Task DeleteInvoiceExceptionCatch()
        {
            // Arrange


            var invoice = new Invoice();

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceById(1)).Throws(new Exception());

            // Act
            var result = await _sut.DeleteInvoice(1);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside DeleteInvoice(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task DeleteInvoiceShouldLogAppropriateMessageWhenNoinvoicesInDatabase()
        {
            // Arrange


            var invoice = new Invoice();

            mockRepo.Setup(x => x.Invoice).Returns(_invoiceRepoMock.Object);
            _invoiceRepoMock.Setup(x => x.GetInvoiceById(1)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.DeleteInvoice(1);

            // Assert
            mockLogger.Verify(x => x.LogError("Invoice Id not found, cannot delete."), Times.Once);
        }
      

    }
}
