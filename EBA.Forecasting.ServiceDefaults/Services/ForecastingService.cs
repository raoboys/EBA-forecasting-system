using EBA.Forecasting.ServiceDefaults.Services.Models;

using Microsoft.Data.SqlClient;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;

namespace EBA.Forecasting.ServiceDefaults.Services
{
    public class ForecastingService
    {
        private readonly MLContext _context;
        private ITransformer _model;
        private readonly string _modelPath = "eba-model.zip";

        public ForecastingService(MLContext context)
        {
            _context = context;
            LoadModel();
        }

        // Load Training Data
        public IDataView LoadTrainingData(IEnumerable<EnhancedPredictionData>? enhancedPredictions)
        {
            Console.WriteLine("Loading data for training model");
            if (enhancedPredictions != null)
            {
                // Load data into ML.NET IDataView
                var dataView = _context.Data.LoadFromEnumerable(enhancedPredictions);
                Console.WriteLine("Loading data for training model is successfull..");
                return dataView;
            }
            else
            {
                var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EBA.Forecasting.DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"; // Update with your connection string
                var query = "SELECT * FROM EnhancedPredictionData"; // Your SQL query here

                // Load data from SQL
                DatabaseLoader loader = _context.Data.CreateDatabaseLoader<EnhancedPredictionData>();
                DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, query);

                // Load data into ML.NET IDataView
                var dataView = loader.Load(dbSource);
                Console.WriteLine("Loading data for training model is successfull..");
                return dataView;
            }
        }

        // Train the model
        public void TrainModel(IDataView data)
        {
            if (data == null) {  throw new ArgumentNullException("data"); }

            //int AnomalyDetected = _context.AnomalyDetection.DetectSeasonality(data, "Seasonality", seasonalityWindowSize: 12);

            // Data processing pipeline
            var dataProcessPipeline = _context.Transforms.DetectAnomalyBySrCnn("Anomaly", "PredictedOccupiedBeds", threshold: 0.3)
                //.Append(_context.Transforms.DetectTrend("Trend", "Value", trendWindowSize: 12))
                .Append(_context.Transforms.Concatenate("NumericFeatures",
                    nameof(BedAvailabilityData.EnhancedPredictionData.BedNumber),
                    nameof(BedAvailabilityData.EnhancedPredictionData.StaffAvailable),
                    nameof(BedAvailabilityData.EnhancedPredictionData.Age),
                    nameof(BedAvailabilityData.EnhancedPredictionData.PredictedAvailableBeds),
                    nameof(BedAvailabilityData.EnhancedPredictionData.PredictedOccupiedBeds)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.EnhancedPredictionData.BedType)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.EnhancedPredictionData.BedStatus)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.EnhancedPredictionData.HospitalName)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.EnhancedPredictionData.HospitalLocation)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.EnhancedPredictionData.Gender)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.EnhancedPredictionData.AdmissionStatus)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.EnhancedPredictionData.EmergencyType)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.EnhancedPredictionData.Season)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.EnhancedPredictionData.WeatherConditions)))
                .Append(_context.Transforms.Categorical.OneHotEncoding(nameof(BedAvailabilityData.EnhancedPredictionData.DiseaseOutbreak)))
                .Append(_context.Transforms.Concatenate("CategoricalFeatures",
                    nameof(BedAvailabilityData.EnhancedPredictionData.BedType),
                    nameof(BedAvailabilityData.EnhancedPredictionData.BedStatus),
                    nameof(BedAvailabilityData.EnhancedPredictionData.HospitalName),
                    nameof(BedAvailabilityData.EnhancedPredictionData.HospitalLocation),
                    nameof(BedAvailabilityData.EnhancedPredictionData.Gender),
                    nameof(BedAvailabilityData.EnhancedPredictionData.AdmissionStatus),
                    nameof(BedAvailabilityData.EnhancedPredictionData.EmergencyType),
                    nameof(BedAvailabilityData.EnhancedPredictionData.Season),
                    nameof(BedAvailabilityData.EnhancedPredictionData.WeatherConditions),
                    nameof(BedAvailabilityData.EnhancedPredictionData.DiseaseOutbreak)))
                .Append(_context.Transforms.Concatenate("Features", "NumericFeatures", "CategoricalFeatures"))
                .Append(_context.Transforms.NormalizeMinMax("Features")
                .Append(_context.Transforms.NormalizeBinning("Features")));


            //Define Beds Availability and Occupied models
            var availabilityPipeline = _context.Forecasting.ForecastBySsa(
               outputColumnName: nameof(BedAvailabilityData.BedAvailability.ForecastedAvailableBeds),
               inputColumnName: nameof(BedAvailabilityData.EnhancedPredictionData.PredictedAvailableBeds),
               windowSize: 7,
               seriesLength: 30,
               trainSize: 100,
               horizon: 7,
               confidenceLevel: 0.95f,
               confidenceLowerBoundColumn: nameof(BedAvailabilityData.BedAvailability.ForecastedMinimumAvailableBeds),
               confidenceUpperBoundColumn: nameof(BedAvailabilityData.BedAvailability.ForecastedMaximumAvailableBeds)
               );

             var occupancyPipeline = _context.Forecasting.ForecastBySsa(
               outputColumnName: nameof(BedAvailabilityData.BedAvailability.ForecastedOccupiedBeds),
               inputColumnName: nameof(BedAvailabilityData.EnhancedPredictionData.PredictedAvailableBeds),
               windowSize: 7,
               seriesLength: 30,
               trainSize: 100,
               horizon: 7,
               confidenceLevel: 0.95f,
               confidenceLowerBoundColumn: nameof(BedAvailabilityData.BedAvailability.ForecastedMinimumOccupiedBeds),
               confidenceUpperBoundColumn: nameof(BedAvailabilityData.BedAvailability.ForecastedMaximumOccupiedBeds)
               );

            // Combine pipelines
            var trainingPipeline = dataProcessPipeline
                .Append(availabilityPipeline)
                .Append(occupancyPipeline);
                //.Append(_context.Regression.Trainers.Sdca(labelColumnName: "PredictedAvailableBeds", maximumNumberOfIterations: 100));

            // Train the model
            var model = trainingPipeline.Fit(data);

            // Evaluate the model
            var predictions = model.Transform(data);
            var metrics = _context.Regression.Evaluate(predictions, labelColumnName: nameof(BedAvailabilityData.BedAvailability.ForecastedAvailableBeds), nameof(BedAvailabilityData.BedAvailability.ForecastedAvailableBeds));

            Console.WriteLine($"R^2 Score: {metrics.RSquared}");
            Console.WriteLine($"Root Mean Squared Error: {metrics.RootMeanSquaredError}");
            Console.WriteLine($"Mean Absolute Error: {metrics.MeanAbsoluteError}");
            Console.WriteLine($"Mean Squared Error: { metrics.MeanSquaredError}");

            _model = model;

            SaveModel();
        }

        // CreatePipeline
        private object CreateTrainingPipeline(IDataView data)
        {
            // Data processing pipeline
            //var dataProcessPipeline = _context.Transforms.DetectSeasonality("Seasonality", "Value", seasonalityWindowSize: 12)
            //    .Append(_context.Transforms.DetectTrend("Trend", "Value", trendWindowSize: 12))
            //    .Append(_context.Transforms.DetectAnomalyBySrCnn("Anomaly", "Value", threshold: 0.3));
            //_context.Transforms.Concatenate("NumericFeatures",
            //        "HospitalID", "BedID", "BedNumber", "PatientID", "Age", "CaseID", "PredictedOccupiedBeds", "PredictionID")
            //    //.Append(_context.Transforms.Conversion.MapValueToKey("BedType"))
            //    //.Append(_context.Transforms.Conversion.MapValueToKey("BedStatus"))
            //    //.Append(_context.Transforms.Conversion.MapValueToKey("HospitalName"))
            //    //.Append(_context.Transforms.Conversion.MapValueToKey("HospitalLocation"))
            //    //.Append(_context.Transforms.Conversion.MapValueToKey("Gender"))
            //    //.Append(_context.Transforms.Conversion.MapValueToKey("AdmissionStatus"))
            //    //.Append(_context.Transforms.Conversion.MapValueToKey("EmergencyType"))
            //    //.Append(_context.Transforms.Concatenate("CategoricalFeatures", "BedType", "BedStatus", "HospitalName", "HospitalLocation", "Gender", "AdmissionStatus", "EmergencyType"))
            //    .Append(_context.Transforms.Concatenate("Features", "NumericFeatures"))
            //    .Append(_context.Transforms.NormalizeMinMax("Features"));

            //// Define training pipeline
            //var trainingPipeline = dataProcessPipeline
            //    .Append(_context.Regression.Trainers.Sdca(labelColumnName: "PredictedAvailableBeds", maximumNumberOfIterations: 100));

            //// Train the model
            //_model = trainingPipeline.Fit(data);


            // Data processing pipeline
            var dataProcessPipeline = _context.Transforms.DetectAnomalyBySrCnn("Anomaly", "PredictedOccupiedBeds", threshold: 0.3)
                //.Append(_context.Transforms.DetectTrend("Trend", "Value", trendWindowSize: 12))
                .Append(_context.Transforms.Concatenate("NumericFeatures", "HospitalID", "BedID", "BedNumber", "PatientID", "Age", "CaseID", "PredictedOccupiedBeds", "PredictionID"))
                .Append(_context.Transforms.Concatenate("CategoricalFeatures", "BedType", "BedStatus", "HospitalName", "HospitalLocation", "Gender", "AdmissionStatus", "EmergencyType"))
                .Append(_context.Transforms.Concatenate("Features", "NumericFeatures", "CategoricalFeatures"))
                .Append(_context.Transforms.NormalizeMinMax("Features"));

            // Define STL and ARIMA models
            //var stlPipeline = _context.Transforms.TimeSeries.SsaForecasting(
            //    outputColumnName: "STLForecastedValue",
            //    inputColumnName: "Value",
            //    windowSize: 12,
            //    seriesLength: 24,
            //    trainSize: 48,
            //    horizon: 12);

            //var arimaPipeline = _context.Transforms.TimeSeries.SsaForecasting(
            //    outputColumnName: "ARIMAForecastedValue",
            //    inputColumnName: "Value",
            //    windowSize: 12,
            //    seriesLength: 24,
            //    trainSize: 48,
            //    horizon: 12);

            // Combine pipelines
            var trainingPipeline = dataProcessPipeline
                //.Append(stlPipeline)
                //.Append(arimaPipeline)
                .Append(_context.Regression.Trainers.Sdca(labelColumnName: "PredictedAvailableBeds", maximumNumberOfIterations: 100));

            return trainingPipeline;
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
                var data = LoadTrainingData(null);
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
        public BedAvailabilityData PredictBedsAvailability(EnhancedPredictionData input)
        {
            // Create prediction engine
            var forecastingEngine = _model.CreateTimeSeriesEngine<EnhancedPredictionData, BedAvailabilityForecast>(_context);

            // Make predictions
            var forecast = forecastingEngine.Predict(input);

            // Output predictions
            Console.WriteLine("Forecast");

            return new BedAvailabilityData { EnhancedPredictionData = input, BedAvailability = forecast };
            //var predictionFunction = _context.Model.CreatePredictionEngine<EnhancedPredictionData, Prediction>(_model);
            //var result = predictionFunction.Predict(input);
            //return result.PredictedAvailableBeds;
        }
    }
}
