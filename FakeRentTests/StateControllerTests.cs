using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.State;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleFakeRent.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FakeRentTests
{
    public class StateControllerTests
    {
        private readonly StateController _sut;
        private readonly Mock<IRepositoryWrapper> mockRepo = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
        private readonly Mock<IStateRepository> _stateRepoMock = new Mock<IStateRepository>();

        public StateControllerTests()
        {
            _sut = new StateController(mockRepo.Object, mockMapper.Object, mockLogger.Object);
        }
        [Fact]
        public async Task GetAllStatesShouldReturnOKIfStatesInDb()
        {
            // Arrange
            var state = new State()
            {
                Id = 13,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetAllStates()).ReturnsAsync(new[] { state });

            // Act
            var result = await _sut.GetAllStates();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetAllStatesShouldLogAppropriateMessageWhenStatesInDatabase()
        {
            // Arrange
            var state = new State()
            {
                Id = 13,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetAllStates()).ReturnsAsync(new[] { state });

            // Act
            var result = await _sut.GetAllStates();

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned all rows from table State."), Times.Once);
        }
        [Fact]
        public async Task GetAllStatesShouldReturnNotFoundIfNoStatesInDB()
        {
            // Arrange
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetAllStates()).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllStates();

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetAllStatesShouldLogAppropriateMessageWhenNoStatesInDatabase()
        {
            // Arrange
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetAllStates()).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllStates();

            // Assert
            mockLogger.Verify(x => x.LogError("No states found in db."), Times.Once);
        }
        [Fact]
        public async Task GetAllStatesExceptionCatch()
        {
            // Arrange
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetAllStates()).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetAllStates();

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetAllStates() action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetStateByIdShouldReturnOKIfStatesInDb()
        {
            // Arrange
            var id = 1;
            var state = new State()
            {
                Id = id,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateById(id)).ReturnsAsync(state);

            // Act
            var result = await _sut.GetStateById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetStateByIdShouldLogAppropriateMessageWhenStatesInDatabase()
        {
            // Arrange
            var id = 1;
            var state = new State()
            {
                Id = id,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateById(id)).ReturnsAsync( state );

            // Act
            var result = await _sut.GetStateById(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned State with id: 1"), Times.Once);
        }
        [Fact]
        public async Task GetStateByIdShouldReturnNotFoundIfNoStatesInDB()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStateById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetStateByIdShouldLogAppropriateMessageWhenNoStatesInDatabase()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStateById(id);

            // Assert
            mockLogger.Verify(x => x.LogError("State with id: 1, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetStateByIdExceptionCatch()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateById(id)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetStateById(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetStateById(state_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetStatesByRentIdShouldReturnOKIfStatesInDb()
        {
            // Arrange
            var rentId = 1;
            var state = new State()
            {
                Id = 1,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = rentId
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesByRentId(rentId)).ReturnsAsync(new[] { state });

            // Act
            var result = await _sut.GetStatesByRentId(rentId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetStatesByRentIdShouldLogAppropriateMessageWhenStatesInDatabase()
        {
            // Arrange
            var rentId = 1;
            var state = new State()
            {
                Id = 1,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = rentId
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesByRentId(rentId)).ReturnsAsync(new[] { state });

            // Act
            var result = await _sut.GetStatesByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned States with rent_id: 1, States by Rent_id count: 1"), Times.Once);
        }
        [Fact]
        public async Task GetStatesByRentIdShouldReturnNotFoundIfNoStatesInDB()
        {
            // Arrange
            var rentId = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesByRentId(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStatesByRentId(rentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetStatesByRentIdShouldLogAppropriateMessageWhenNoStatesInDatabase()
        {
            // Arrange
            var rentId = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesByRentId(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStatesByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("State with RentId: 1, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetStatesByRentIdExceptionCatch()
        {
            // Arrange
            var rentId = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesByRentId(rentId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetStatesByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetStatesByRentId(rent_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetStateWithInvoiceByIdShouldReturnOKIfStatesInDb()
        {
            // Arrange
            var id = 1;
            var state = new State()
            {
                Id = id,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1,
                Invoice = new Invoice()
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
                }
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceByStateId(id)).ReturnsAsync( state );

            // Act
            var result = await _sut.GetStateWithInvoiceById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetStateWithInvoiceByIdShouldLogAppropriateMessageWhenStatesInDatabase()
        {
            // Arrange
            var id = 1;
            var state = new State()
            {
                Id = id,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1,
                Invoice = new Invoice()
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
                }
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceByStateId(id)).ReturnsAsync(state);

            // Act
            var result = await _sut.GetStateWithInvoiceById(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned State with state_id: 1 with related Invoice."), Times.Once);
        }
        [Fact]
        public async Task GetStateWithInvoiceByIdShouldReturnNotFoundIfNoStatesInDB()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceByStateId(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStateWithInvoiceById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetStateWithInvoiceByIdShouldLogAppropriateMessageWhenNoStatesInDatabase()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceByStateId(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStateWithInvoiceById(id);

            // Assert
            mockLogger.Verify(x => x.LogError("State with id: 1, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetStateWithInvoiceByIdExceptionCatch()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceByStateId(id)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetStateWithInvoiceById(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetStateWithInvoiceById(state_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetStatesWithInvoiceByRentIdShouldReturnOKIfStatesInDb()
        {
            // Arrange
            var rentId = 1;
            var state = new State()
            {
                Id = 2,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = rentId,
                Invoice = new Invoice()
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
                }
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesWithInvoiceByRentId(rentId)).ReturnsAsync(new[] { state });

            // Act
            var result = await _sut.GetStatesWithInvoiceByRentId(rentId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetStatesWithInvoiceByRentIdShouldLogAppropriateMessageWhenStatesInDatabase()
        {
            // Arrange
            var rentId = 1;
            var state = new State()
            {
                Id = 2,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = rentId,
                Invoice = new Invoice()
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
                }
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesWithInvoiceByRentId(rentId)).ReturnsAsync(new[] { state });

            // Act
            var result = await _sut.GetStatesWithInvoiceByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned States with RentId: 1 with related Invoice."), Times.Once);
        }
        [Fact]
        public async Task GetStatesWithInvoiceByRentIdShouldReturnNotFoundIfNoStatesInDB()
        {
            // Arrange
            var rentId = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesWithInvoiceByRentId(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStatesWithInvoiceByRentId(rentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetStatesWithInvoiceByRentIdShouldLogAppropriateMessageWhenNoStatesInDatabase()
        {
            // Arrange
            var rentId = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesWithInvoiceByRentId(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStatesWithInvoiceByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("State with rent_id: 1, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetStatesWithInvoiceByRentIdExceptionCatch()
        {
            // Arrange
            var rentId = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesWithInvoiceByRentId(rentId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetStatesWithInvoiceByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside StatesWithInvoiceByRentId(rent_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetStateWithPhotoByStateIdShouldReturnOKIfStatesInDb()
        {
            // Arrange
            var id = 1;
            var state = new State()
            {
                Id = id,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1,
                Photo = new Photo()
                { Id = 1 }
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithPhotoByStateId(id)).ReturnsAsync(state);

            // Act
            var result = await _sut.GetStateWithPhotoByStateId(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetStateWithPhotoByStateIdShouldLogAppropriateMessageWhenStatesInDatabase()
        {
            // Arrange
            var id = 1;
            var state = new State()
            {
                Id = id,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1,
                Photo = new Photo()
                { Id = 1 }
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithPhotoByStateId(id)).ReturnsAsync(state);

            // Act
            var result = await _sut.GetStateWithPhotoByStateId(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned State with state_id: 1 with related Photo."), Times.Once);
        }
        [Fact]
        public async Task GetStateWithPhotoByStateIdShouldReturnNotFoundIfNoStatesInDB()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithPhotoByStateId(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStateWithPhotoByStateId(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetStateWithPhotoByStateIdShouldLogAppropriateMessageWhenNoStatesInDatabase()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithPhotoByStateId(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStateWithPhotoByStateId(id);

            // Assert
            mockLogger.Verify(x => x.LogError("State with id: 1, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetStateWithPhotoByStateIdExceptionCatch()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithPhotoByStateId(id)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetStateWithPhotoByStateId(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetStateWithPhotoByStateId(state_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetStatesWithPhotoByRentIdShouldReturnOKIfStatesInDb()
        {
            // Arrange
            var rentId = 1;
            var state = new State()
            {
                Id = 3,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = rentId,
                Photo = new Photo()
                { Id = 1 }
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesWithPhotoByRentId(rentId)).ReturnsAsync(new[] { state });

            // Act
            var result = await _sut.GetStatesWithPhotoByRentId(rentId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetStatesWithPhotoByRentIdShouldLogAppropriateMessageWhenStatesInDatabase()
        {
            // Arrange
            var rentId = 1;
            var state = new State()
            {
                Id = 3,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = rentId,
                Photo = new Photo()
                { Id = 1 }
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesWithPhotoByRentId(rentId)).ReturnsAsync(new[] { state });

            // Act
            var result = await _sut.GetStatesWithPhotoByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned States with RentId: 1 with related Photo."), Times.Once);
        }
        [Fact]
        public async Task GetStatesWithPhotoByRentIdShouldReturnNotFoundIfNoStatesInDB()
        {
            // Arrange
            var rentId = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesWithPhotoByRentId(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStatesWithPhotoByRentId(rentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetStatesWithPhotoByRentIdShouldLogAppropriateMessageWhenNoStatesInDatabase()
        {
            // Arrange
            var rentId = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesWithPhotoByRentId(rentId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStatesWithPhotoByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("State with rent_id: 1, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetStatesWithPhotoByRentIdExceptionCatch()
        {
            // Arrange
            var rentId = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStatesWithPhotoByRentId(rentId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetStatesWithPhotoByRentId(rentId);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetStatesWithPhotoByRentId(rent_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetStateWithRentByStateIdShouldReturnOKIfStatesInDb()
        {
            // Arrange
            var id = 1;
            var state = new State()
            {
                Id = id,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1,
                Rent = new Rent()
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
                }

            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithRentByStateId(id)).ReturnsAsync(state);

            // Act
            var result = await _sut.GetStateWithRentByStateId(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetStateWithRentByStateIdShouldLogAppropriateMessageWhenStatesInDatabase()
        {
            // Arrange
            var id = 1;
            var state = new State()
            {
                Id = id,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1,
                Rent = new Rent()
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
                }

            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithRentByStateId(id)).ReturnsAsync(state);

            // Act
            var result = await _sut.GetStateWithRentByStateId(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned State with state_id: 1 with related Rent."), Times.Once);
        }
        [Fact]
        public async Task GetStateWithRentByStateIdShouldReturnNotFoundIfNoStatesInDB()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithRentByStateId(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStateWithRentByStateId(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetStateWithRentByStateIdShouldLogAppropriateMessageWhenNoStatesInDatabase()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithRentByStateId(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStateWithRentByStateId(id);

            // Assert
            mockLogger.Verify(x => x.LogError("State with id: 1, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetStateWithRentByStateIdExceptionCatch()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithRentByStateId(id)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetStateWithRentByStateId(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetStateWithRentByStateId(state_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task GetStateWithInvoiceAndPhotoByStateIdShouldReturnOKIfStatesInDb()
        {
            // Arrange
            var id = 1;
            var state = new State()
            {
                Id = id,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1,
                Photo = new Photo(),
                Invoice = new Invoice()
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceAndPhotoByStateId(id)).ReturnsAsync(state);

            // Act
            var result = await _sut.GetStateWithInvoiceAndPhotoByStateId(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetStateWithInvoiceAndPhotoByStateIdShouldLogAppropriateMessageWhenStatesInDatabase()
        {
            // Arrange
            var id = 1;
            var state = new State()
            {
                Id = id,
                ColdWaterState = 110,
                HotWaterState = 0,
                GasState = 110,
                EnergyState = 110,
                HeatState = 0,
                IsInitial = false,
                RentId = 1,
                Photo = new Photo(),
                Invoice = new Invoice()
            };

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceAndPhotoByStateId(id)).ReturnsAsync(state);

            // Act
            var result = await _sut.GetStateWithInvoiceAndPhotoByStateId(id);

            // Assert
            mockLogger.Verify(x => x.LogInfo("Returned State with state_id: 1 with related Rent."), Times.Once);
        }
        [Fact]
        public async Task GetStateWithInvoiceAndPhotoByStateIdShouldReturnNotFoundIfNoStatesInDB()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceAndPhotoByStateId(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStateWithInvoiceAndPhotoByStateId(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetStateWithInvoiceAndPhotoByStateIdShouldLogAppropriateMessageWhenNoStatesInDatabase()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceAndPhotoByStateId(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetStateWithInvoiceAndPhotoByStateId(id);

            // Assert
            mockLogger.Verify(x => x.LogError("State with id: 1, hasn't been found in db."), Times.Once);
        }
        [Fact]
        public async Task GetStateWithInvoiceAndPhotoByStateIdExceptionCatch()
        {
            // Arrange
            var id = 1;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceAndPhotoByStateId(id)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetStateWithInvoiceAndPhotoByStateId(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside GetStateWithRentByStateId(state_Id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreateStateExceptionCatch()
        {
            // Arrange

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.CreateState(new State())).Throws(new Exception());

            // Act
            var result = await _sut.CreateState(new StateForCreationDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside CreateState(StateForCreationDto) action: Object reference not set to an instance of an object."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreateStateShouldReturnBadRequestIfStateIsNull()
        {
            // Arrange

            // Act
            var result = await _sut.CreateState(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
        [Fact]
        public async Task CreateStateShouldLogAppropriateMessageIfStateIsNull()
        {
            // Arrange

            // Act
            var result = await _sut.CreateState(null);

            // Assert
            mockLogger.Verify(x => x.LogError("State received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task UpdateStateExceptionCatch()
        {
            /// Arrange
            var id = 3;

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateById(id)).Throws(new Exception());

            // Act
            var result = await _sut.UpdateState(id, new StateForUpdateDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside UpdateState(id, StateForUpdateDto) action: Object reference not set to an instance of an object."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task UpdateStateShouldReturnBadRequestIfStateIsNull()
        {
            /// Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdateState(id, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateStateShouldLogAppropriateMessageIfStateIsNull()
        {
            // Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdateState(id, null);

            // Assert
            mockLogger.Verify(x => x.LogError("State received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task DeleteStateExceptionCatch()
        {
            /// Arrange
            var id = 3;
            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceAndPhotoByStateId(id)).Throws(new Exception());

            // Act
            var result = await _sut.DeleteState(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside DeleteState(state_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task DeleteStateShouldLogAppropriateMessageWhenNoStatesInDatabase()
        {
            // Arrange

            var id = 3;

            mockRepo.Setup(x => x.State).Returns(_stateRepoMock.Object);
            _stateRepoMock.Setup(x => x.GetStateWithInvoiceAndPhotoByStateId(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.DeleteState(id);

            // Assert
            mockLogger.Verify(x => x.LogError("State Id not found, cannot delete."), Times.Once);
        }
    }
}
