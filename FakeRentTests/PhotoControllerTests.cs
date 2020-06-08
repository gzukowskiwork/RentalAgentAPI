using AutoMapper;
using Contracts;
using EmailService;
using Entities.DataTransferObjects.Photo;
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
    public class PhotoControllerTests
    {
        private readonly PhotoController _sut;
        private readonly Mock<IRepositoryWrapper> mockRepo = new Mock<IRepositoryWrapper>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
        private readonly Mock<IPhotoRepository> _photoRepoMock = new Mock<IPhotoRepository>();

        public PhotoControllerTests()
        {
            _sut = new PhotoController(mockRepo.Object, mockMapper.Object, mockLogger.Object);
        }
        [Fact]
        public async Task GetPhotoByIdShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var photoId = 1;

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(photoId)).ReturnsAsync(new Photo());

            // Act
            var result = await _sut.GetPhotoById(photoId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetPhotoByIdShouldLogAppropriateMessageWhenPhotoInDatabase()
        {
            // Arrange
            var photoId = 1;


            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(photoId)).ReturnsAsync(new Photo());


            // Act
            var result = await _sut.GetPhotoById(photoId);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned table Photo, full one row with id: 1"), Times.Once);

        }
        [Fact]
        public async Task GetPhotoByIdShouldReturnNotFoundIfNoPhotosInDB()
        {
            // Arrange
            var photoId = 1;


            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(photoId)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPhotoById(photoId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPhotoByIdShouldLogAppropriateMessageWhenNoPhotoInDatabase()
        {
            // Arrange
            var photoId = 1;


            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(photoId)).ReturnsAsync(() => null);



            // Act
            var result = await _sut.GetPhotoById(photoId);

            // Assert

            mockLogger.Verify(x => x.LogError("Photo with id: 1, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetAdressByIdExceptionCatch()
        {
            // Arrange
            var photoId = 1;


            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(photoId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPhotoById(photoId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPhotoById(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPhotoByStateIdShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var stateId = 45;

            var photo = new Photo()
            {
                Id = 1,
                StateId = stateId
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoByStateId(stateId)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByStateId(stateId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(45, photo.StateId);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetPhotoByStateIdShouldLogAppropriateMessageWhenPhotoInDatabase()
        {
            // Arrange
            var stateId = 45;

            var photo = new Photo()
            {
                Id = 1,
                StateId = stateId
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoByStateId(stateId)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByStateId(stateId);

            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned table Photo, full one row with id: 45"), Times.Once);

        }

        [Fact]
        public async Task GetPhotoByStateIdShouldReturnNotFoundIfNoPhotosInDB()
        {
            // Arrange
            var stateId = 45;

            var photo = new Photo()
            {
                Id = 1,
                StateId = stateId
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoByStateId(stateId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByStateId(stateId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPhotoByStateIdShouldLogAppropriateMessageWhenNoPhotoInDatabase()
        {
            // Arrange
            var stateId = 45;

            var photo = new Photo()
            {
                Id = 1,
                StateId = stateId
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoByStateId(stateId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByStateId(stateId);
            // Assert

            mockLogger.Verify(x => x.LogError("Photo with state_id: 45, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetPhotoByStateIdExceptionCatch()
        {
            // Arrange
            var photoId = 1;


            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoByStateId(photoId)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPhotoByStateId(photoId);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPhotoByStateId(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPhotoByIdAndAnyPhotoColumnNameShouldReturnNotFound()
        {
            // Arrange
            var id = 45;
            string coldwater = "coldwater";
           //  hotwater, gas, energy, heat;

      

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoByIdOneColumn(id, coldwater)).ReturnsAsync(()=>null);

            // Act
            var result = await _sut.GetPhotoByIdAndAnyPhotoColumnName(id, coldwater);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetPhotoByIdAndAnyPhotoColumnNameShouldLogAppropriateMessageWhenNoPhotoInDatabase()
        {
            // Arrange
            var id = 45;
            string coldwater = "coldwater";
            //  hotwater, gas, energy, heat;



            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoByIdOneColumn(id, coldwater)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByIdAndAnyPhotoColumnName(id, coldwater);
            // Assert

            mockLogger.Verify(x => x.LogError("Photo with id: 45, hasn't been found in db."), Times.Once);

        }

        [Fact]
        public async Task GetPhotoByIdAndAnyPhotoColumnNameExceptionCatch()
        {
            // Arrange
            var id = 45;
            string coldwater = "coldwater";
            //  hotwater, gas, energy, heat;

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoByIdOneColumn(id, coldwater)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPhotoByIdAndAnyPhotoColumnName(id, coldwater);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPhotoByIdAndAnyPhotoColumnName(photo_id, photo_column_name) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPhotoByIdOnlyColdWaterShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByIdOnlyColdWater(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(45, photo.Id);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetPhotoByIdOnlyColdWaterShouldLogAppropriateMessageWhenPhotoInDatabase()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByIdOnlyColdWater(id);


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned row Photo, just ColdWater Photo with id: 45"), Times.Once);

        }

        [Fact]
        public async Task GetPhotoByIdOnlyColdWaterShouldReturnNotFoundIfNoPhotosInDB()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByIdOnlyColdWater(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPhotoByIdOnlyColdWaterShouldLogAppropriateMessageWhenNoPhotoInDatabase()
        {

            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByIdOnlyColdWater(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Photo with id: 45, hasn't been found in db."), Times.Once);

        }
        [Fact]
        public async Task GetPhotoByIdOnlyColdWaterExceptionCatch()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };


            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPhotoByIdOnlyColdWater(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPhotoByIdOnlyColdWater(photo_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPhotoByIdOnlyHotWaterShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByIdOnlyHotWater(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(id, photo.Id);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetPhotoByIdOnlyHotWaterShouldLogAppropriateMessageWhenPhotoInDatabase()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByIdOnlyHotWater(id);


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned row Photo, just HotWater Photo with id: 45"), Times.Once);

        }
        [Fact]
        public async Task GetPhotoByIdOnlyHotWaterShouldReturnNotFoundIfNoPhotosInDB()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByIdOnlyHotWater(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPhotoByIdOnlyHotWaterShouldLogAppropriateMessageWhenNoPhotoInDatabase()
        {

            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByIdOnlyHotWater(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Photo with id: 45, hasn't been found in db."), Times.Once);

        }
        [Fact]
        public async Task GetPhotoByIdOnlyHotWaterExceptionCatch()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };


            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPhotoByIdOnlyHotWater(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPhotoByIdOnlyHotWater(photo_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPhotoByIdOnlyGasShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByIdOnlyGas(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(id, photo.Id);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetPhotoByIdOnlyGasShouldLogAppropriateMessageWhenPhotoInDatabase()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByIdOnlyGas(id);


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned row Photo, just Gas Photo with id: 45"), Times.Once);

        }
        [Fact]
        public async Task GetPhotoByIdOnlyGasShouldReturnNotFoundIfNoPhotosInDB()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByIdOnlyGas(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPhotoByIdOnlyGasShouldLogAppropriateMessageWhenNoPhotoInDatabase()
        {

            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByIdOnlyGas(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Photo with id: 45, hasn't been found in db."), Times.Once);

        }
        [Fact]
        public async Task GetPhotoByIdOnlyGasExceptionCatch()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };


            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPhotoByIdOnlyGas(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPhotoByIdOnlyGas(photo_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async Task GetPhotoByIdOnlyEnergyShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByIdOnlyEnergy(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(id, photo.Id);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetPhotoByIdOnlyEnergyShouldLogAppropriateMessageWhenPhotoInDatabase()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByIdOnlyEnergy(id);


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned row Photo, just Energy Photo with id: 45"), Times.Once);

        }
        [Fact]
        public async Task GetPhotoByIdOnlyEnergyShouldReturnNotFoundIfNoPhotosInDB()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByIdOnlyEnergy(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPhotoByIdOnlyEnergyShouldLogAppropriateMessageWhenNoPhotoInDatabase()
        {

            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByIdOnlyEnergy(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Photo with id: 45, hasn't been found in db."), Times.Once);

        }
        [Fact]
        public async Task GetPhotoByIdOnlyEnergyExceptionCatch()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };


            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPhotoByIdOnlyEnergy(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPhotoByIdOnlyEnergy(photo_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }


        [Fact]
        public async Task GetPhotoByIdOnlyHeatShouldReturnOkIfFoundInDatabase()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByIdOnlyHeat(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(id, photo.Id);
            Assert.IsNotType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetPhotoByIdOnlyHeatShouldLogAppropriateMessageWhenPhotoInDatabase()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(photo);

            // Act
            var result = await _sut.GetPhotoByIdOnlyHeat(id);


            // Assert

            mockLogger.Verify(x => x.LogInfo("Returned row Photo, just Heat Photo with id: 45"), Times.Once);

        }
        [Fact]
        public async Task GetPhotoByIdOnlyHeatShouldReturnNotFoundIfNoPhotosInDB()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByIdOnlyHeat(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPhotoByIdOnlyHeatShouldLogAppropriateMessageWhenNoPhotoInDatabase()
        {

            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetPhotoByIdOnlyHeat(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Photo with id: 45, hasn't been found in db."), Times.Once);

        }
        [Fact]
        public async Task GetPhotoByIdOnlyHeatExceptionCatch()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };


            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ThrowsAsync(new Exception());


            // Act
            var result = await _sut.GetPhotoByIdOnlyHeat(id);

            // Assert

            mockLogger.Verify(x => x.LogError("Something went wrong inside GetPhotoByIdOnlyHeat(photo_id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);

        }
        [Fact]
        public async Task CreatePhotoExceptionCatch()
        {
            // Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.CreatePhoto(photo)).Throws(new Exception());

            // Act
            var result = await _sut.CreatePhoto(new PhotoForCreationDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside CreatePhoto(photoForCreationDto) action: Object reference not set to an instance of an object."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task CreatePhotoShouldReturnBadRequestIfPhotoIsNull()
        {
            // Arrange


            // Act
            var result = await _sut.CreatePhoto(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
        [Fact]
        public async Task CreatePhotoShouldLogAppropriateMessageIfPhotoIsNull()
        {
            // Arrange


            // Act
            var result = await _sut.CreatePhoto(null);

            // Assert
            mockLogger.Verify(x => x.LogError("Photo received is a Null Object."), Times.Once);
        }

        [Fact]
        public async Task UpdatePhotoExceptionCatch()
        {
            /// Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };


            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).Throws(new Exception());

            // Act
            var result = await _sut.UpdatePhoto(id, new PhotoForUpdateDto());

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside UpdatePhoto(id, photoForUpdateDto) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task UpdatePhotoShouldReturnBadRequestIfPhotoIsNull()
        {
            /// Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdatePhoto(id, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdatePhotoShouldLogAppropriateMessageIfPhotoIsNull()
        {
            // Arrange
            var id = 3;

            // Act
            var result = await _sut.UpdatePhoto(id, null);

            // Assert
            mockLogger.Verify(x => x.LogError("Photo received is a Null Object."), Times.Once);
        }
        [Fact]
        public async Task DeletePhotoExceptionCatch()
        {
            /// Arrange
            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).Throws(new Exception());

            // Act
            var result = await _sut.DeletePhoto(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Something went wrong inside DeletePhoto(id) action: Exception of type 'System.Exception' was thrown."), Times.Once);
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task DeletePhotoShouldLogAppropriateMessageWhenNoPhotosInDatabase()
        {
            // Arrange

            var id = 45;

            var photo = new Photo()
            {
                Id = id,
                StateId = 69
            };

            mockRepo.Setup(x => x.Photo).Returns(_photoRepoMock.Object);
            _photoRepoMock.Setup(x => x.GetPhotoById(id)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.DeletePhoto(id);

            // Assert
            mockLogger.Verify(x => x.LogError("Photo Id not found, cannot delete."), Times.Once);
        }
    }
}
