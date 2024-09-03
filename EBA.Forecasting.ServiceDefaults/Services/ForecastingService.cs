using EBA.Forecasting.ServiceDefaults.Services.Models;

using Microsoft.Data.SqlClient;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;

using Newtonsoft.Json;

using System.Data;

namespace EBA.Forecasting.ServiceDefaults.Services
{
    public class ForecastingService
    {
        private readonly MLContext _context;
        private ITransformer _model;
        private readonly string _modelPath = "eba-model.zip";

        public ForecastingService()
        {
            _context = new MLContext();
            LoadModel();
        }

        // Load Training Data
        public virtual IDataView LoadTrainingData(IEnumerable<BedAvailabilityData> data = null)
        {
            Console.WriteLine("Loading data for training model");
            if (data != null)
            {
                // Load data into ML.NET IDataView
                var dataView = _context.Data.LoadFromEnumerable(data);
                Console.WriteLine("Loading data for training model is successfull..");
                return dataView;
            }
            else
            {
                var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EBA.Forecasting.DB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"; // Update with your connection string
                var query = "SELECT * FROM BedAvailabilityData"; // Your SQL query here

                // Load data from SQL
                DatabaseLoader loader = _context.Data.CreateDatabaseLoader<BedAvailabilityData>();
                DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, query);

                // Load data into ML.NET IDataView
                var dataView = loader.Load(dbSource);
                Console.WriteLine("Loading data for training model is successfull..");
                return dataView;
            }
        }

        // Train the model
        public virtual void TrainModel(IDataView data)
        {
            if (data == null) { throw new ArgumentNullException("data"); }

            // Split data into train and test sets
            var trainTestData = _context.Data.TrainTestSplit(data, testFraction: 0.2);

            // Data processing pipeline
            var dataProcessPipeline = _context.Transforms.Concatenate("NumericFeatures",
                    nameof(BedAvailabilityData.BedNumber),
                    nameof(BedAvailabilityData.StaffAvailable),
                    nameof(BedAvailabilityData.Age),
                    nameof(BedAvailabilityData.PredictedAvailableBeds),
                    nameof(BedAvailabilityData.PredictedOccupiedBeds))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.BedType)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.BedStatus)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.HospitalName)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.HospitalLocation)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.Gender)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.AdmissionStatus)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.EmergencyType)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.Season)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.WeatherConditions)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.DiseaseOutbreak)))
                .Append(_context.Transforms.Concatenate("CategoricalFeatures",
                    nameof(BedAvailabilityData.BedType),
                    nameof(BedAvailabilityData.BedStatus),
                    nameof(BedAvailabilityData.HospitalName),
                    nameof(BedAvailabilityData.HospitalLocation),
                    nameof(BedAvailabilityData.Gender),
                    nameof(BedAvailabilityData.AdmissionStatus),
                    nameof(BedAvailabilityData.EmergencyType),
                    nameof(BedAvailabilityData.Season),
                    nameof(BedAvailabilityData.WeatherConditions),
                    nameof(BedAvailabilityData.DiseaseOutbreak)))
                .Append(_context.Transforms.Concatenate("Features", "NumericFeatures", "CategoricalFeatures"))
                .Append(_context.Transforms.NormalizeMinMax("Features")
                .Append(_context.Transforms.NormalizeBinning("Features")));

            //Define Beds Availability and Occupied models
            var availabilityPipeline = _context.Forecasting.ForecastBySsa(
               outputColumnName: nameof(BedAvailability.Forecast.ForecastedAvailableBeds),
               inputColumnName: nameof(BedAvailabilityData.PredictedAvailableBeds),
               windowSize: 7,
               seriesLength: 30,
               trainSize: 100,
               horizon: 7,
               confidenceLevel: 0.95f,
               confidenceLowerBoundColumn: nameof(BedAvailability.Forecast.ForecastedMinimumAvailableBeds),
               confidenceUpperBoundColumn: nameof(BedAvailability.Forecast.ForecastedMaximumAvailableBeds)
               );

            var occupancyPipeline = _context.Forecasting.ForecastBySsa(
              outputColumnName: nameof(BedAvailability.Forecast.ForecastedOccupiedBeds),
              inputColumnName: nameof(BedAvailabilityData.PredictedAvailableBeds),
              windowSize: 7,
              seriesLength: 30,
              trainSize: 100,
              horizon: 7,
              confidenceLevel: 0.95f,
              confidenceLowerBoundColumn: nameof(BedAvailability.Forecast.ForecastedMinimumOccupiedBeds),
              confidenceUpperBoundColumn: nameof(BedAvailability.Forecast.ForecastedMaximumOccupiedBeds)
              );

            var detectAnomalyPipeline = _context.Transforms.DetectAnomalyBySrCnn(
            outputColumnName: nameof(BedAvailability.Forecast.BedAvailabilityAnamolyPrediction),
            inputColumnName: nameof(BedAvailabilityData.PredictedAvailableBeds),
            windowSize: 12,
            backAddWindowSize: 5,
            lookaheadWindowSize: 7, // Horizon of 7
            averagingWindowSize: 3,
            judgementWindowSize: 12,
            threshold: 0.3);

            // Combine pipelines
            var trainingPipeline = dataProcessPipeline
                .Append(availabilityPipeline)
                .Append(occupancyPipeline)
                .Append(detectAnomalyPipeline);

            // Train the model
            var model = trainingPipeline.Fit(trainTestData.TrainSet);

            // Evaluate the model
            var predictions = model.Transform(trainTestData.TestSet);

            // Extract forecasted values
            var forecastedAvailableBeds = predictions.GetColumn<float[]>(nameof(BedAvailability.Forecast.ForecastedAvailableBeds)).ToArray();
            var forecastedOccupiedBeds = predictions.GetColumn<float[]>(nameof(BedAvailability.Forecast.ForecastedOccupiedBeds)).ToArray();

            // Evaluate each forecasted value
            for (int i = 0; i < forecastedAvailableBeds[0].Length; i++)
            {
                var availableBeds = forecastedAvailableBeds.Select(f => f[i]).ToArray();
                var occupiedBeds = forecastedOccupiedBeds.Select(f => f[i]).ToArray();

                var availableBedsData = _context.Data.LoadFromEnumerable(availableBeds.Select((value, index) => new { PredictedAvailableBeds = value }));
                var occupiedBedsData = _context.Data.LoadFromEnumerable(occupiedBeds.Select((value, index) => new { PredictedAvailableBeds = value }));

                var metricsAvailableBeds = _context.Regression.Evaluate(availableBedsData, labelColumnName: "PredictedAvailableBeds", scoreColumnName: "PredictedAvailableBeds");

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine($"==============================Horizon {i + 1}==============================");

                Console.WriteLine($"R^2 for Available Beds (Horizon {i + 1}): {metricsAvailableBeds.RSquared}");
                Console.WriteLine($"Mean Absolute Error for Available Beds (Horizon {i + 1}): {metricsAvailableBeds.MeanAbsoluteError}");
                Console.WriteLine($"Mean Squared Error for Available Beds (Horizon {i + 1}): {metricsAvailableBeds.MeanSquaredError}");
                Console.WriteLine($"Root Mean Squared Error for Available Beds (Horizon {i + 1}): {metricsAvailableBeds.RootMeanSquaredError}");
                Console.WriteLine(Environment.NewLine);
                var metricsOccupiedBeds = _context.Regression.Evaluate(occupiedBedsData, labelColumnName: "PredictedAvailableBeds", scoreColumnName: "PredictedAvailableBeds");
                Console.WriteLine($"R^2 for Occupied Beds (Horizon {i + 1}): {metricsOccupiedBeds.RSquared}");
                Console.WriteLine($"Mean Absolute Error for Occupied Beds (Horizon {i + 1}): {metricsOccupiedBeds.MeanAbsoluteError}");
                Console.WriteLine($"Mean Squared Error for Occupied Beds (Horizon {i + 1}): {metricsOccupiedBeds.MeanSquaredError}");
                Console.WriteLine($"Root Mean Squared Error for Occupied Beds (Horizon {i + 1}): {metricsOccupiedBeds.RootMeanSquaredError}");

                Console.WriteLine($"=====================================================================");
            }

            _model = model;

            SaveModel();
        }

        // Load the trained model
        private void LoadModel()
        {
            if (File.Exists(_modelPath))
            {
                using var fileStream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                _model = _context.Model.Load(fileStream, out _);
            }
            else
            {
                //throw new InvalidOperationException("Model has not been trained yet.");
                Console.WriteLine("Model has not been trained yet.");
                var data = LoadTrainingData();
                TrainModel(data);
            }
        }

        // Save the trained model
        private void SaveModel()
        {
            if (_model == null)
                throw new InvalidOperationException("Model has not been trained yet.");

            using var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write);
            _context.Model.Save(_model, null, fileStream);

            Console.WriteLine("Trained Model Saved successfully..");
        }

        // Predict available beds using the trained model
        public virtual BedAvailability PredictBedsAvailability(BedAvailabilityParameters input)
        {
            if (input == null) throw new ArgumentNullException("input");

            Console.WriteLine("Request Data : {0}", JsonConvert.SerializeObject(input));
            var predictionInput = BedAvailabilityData.MapToData(input);

            // Create prediction engine
            var forecastingEngine = _model.CreateTimeSeriesEngine<BedAvailabilityData, Forecast>(_context);

            // Make predictions
            var forecast = forecastingEngine.Predict(predictionInput);

            // Output predictions
            Console.WriteLine("Forecast Availability : {0}", JsonConvert.SerializeObject(forecast));

            return new BedAvailability { Data = input, Forecast = forecast };
        }
    }
}
