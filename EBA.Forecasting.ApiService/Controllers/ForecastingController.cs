using EBA.Forecasting.ServiceDefaults.Services.Models;
using EBA.Forecasting.ServiceDefaults.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;

namespace EBA.Forecasting.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastController : ControllerBase
    {
        private readonly ForecastingService _forecastingService;

        public ForecastController(ForecastingService forecastingService)
        {
            _forecastingService = forecastingService;
        }

        [HttpPost("train")]
        public IActionResult TrainModel(IEnumerable<EnhancedPredictionData> enhancedPredictions)
        {
            // For demonstration purposes, let's load data from a SQL database or another source.
            // Here we use a placeholder to simulate loading data.
            var data = _forecastingService.LoadTrainingData(enhancedPredictions); // Implement this method to fetch and convert your data into IDataView.

            _forecastingService.TrainModel(data);
            return Ok("Model trained and saved successfully.");
        }

        [HttpPost("predict")]
        public IActionResult Predict([FromBody] EnhancedPredictionData input)
        {
            if (input == null)
            {
                return BadRequest("Invalid input data");
            }

            try
            {
                var prediction = _forecastingService.PredictBedsAvailability(input);
                return Ok(prediction);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Model error: {ex.Message}");
            }
        }
    }
}
