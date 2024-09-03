using EBA.Forecasting.ServiceDefaults.Services;
using EBA.Forecasting.ServiceDefaults.Services.Models;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Filters;

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
        public IActionResult TrainModel()
        {
            //  loading data for training the model.
            var data = _forecastingService.LoadTrainingData();

            _forecastingService.TrainModel(data);
            return Ok("Model trained and saved successfully.");
        }

        [HttpPost("predict")]
        [SwaggerRequestExample(typeof(BedAvailabilityParameters), typeof(BedAvailabilityParametersExample))]
        [SwaggerResponseExample(200, typeof(BedAvailabilityExample))]
        public IActionResult Predict([FromBody] BedAvailabilityParameters input)
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
