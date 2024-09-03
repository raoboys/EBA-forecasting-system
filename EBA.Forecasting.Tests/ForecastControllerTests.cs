using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using EBA.Forecasting.ServiceDefaults.Services;
using EBA.Forecasting.ServiceDefaults.Services.Models;
using EBA.Forecasting.ApiService.Controllers;
using Microsoft.ML;

namespace EBA.Forecasting.Tests
{
    public class ForecastControllerTests
    {
        private readonly Mock<ForecastingService> _mockForecastingService;
        private readonly ForecastController _controller;

        public ForecastControllerTests()
        {
            _mockForecastingService = new Mock<ForecastingService>();
            _controller = new ForecastController(_mockForecastingService.Object);
        }

        [Fact]
        public void TrainModel_ReturnsOkResult()
        {
            // Arrange
            var mockDataView = new Mock<IDataView>();
            _mockForecastingService.Setup(service => service.LoadTrainingData(null)).Returns(mockDataView.Object);
            _mockForecastingService.Setup(service => service.TrainModel(It.IsAny<IDataView>()));

            // Act
            var result = _controller.TrainModel();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Model trained and saved successfully.", okResult.Value);
        }

        [Fact]
        public void Predict_ReturnsBadRequest_WhenInputIsNull()
        {
            // Act
            var result = _controller.Predict(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid input data", badRequestResult.Value);
        }

        [Fact]
        public void Predict_ReturnsOkResult_WithPrediction()
        {
            // Arrange
            var input = new BedAvailabilityParameters();
            var prediction = new BedAvailability();
            _mockForecastingService.Setup(service => service.PredictBedsAvailability(input)).Returns(prediction);

            // Act
            var result = _controller.Predict(input);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(prediction, okResult.Value);
        }

        [Fact]
        public void Predict_ReturnsBadRequest_WhenModelErrorOccurs()
        {
            // Arrange
            var input = new BedAvailabilityParameters();
            _mockForecastingService.Setup(service => service.PredictBedsAvailability(input)).Throws(new InvalidOperationException("Model error"));

            // Act
            var result = _controller.Predict(input);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Model error: Model error", badRequestResult.Value);
        }
    }
}


