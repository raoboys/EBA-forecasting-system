using EBA.Forecasting.ServiceDefaults.Services.Models;

using Microsoft.Data.SqlClient;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

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

            // Data processing pipeline
            //var dataProcessPipeline = _context.Transforms.Concatenate("NumericFeatures",
            //                    "HospitalID", "BedID", "BedNumber", "PatientID", "Age", "CaseID", "PredictedOccupiedBeds", "PredictionID")
            //                .Append(_context.Transforms.Concatenate("CategoricalFeatures", "BedType", "BedStatus", "HospitalName", "HospitalLocation", "Gender", "AdmissionStatus", "EmergencyType"))
            //                .Append(_context.Transforms.Categorical.OneHotEncoding("BedType"))
            //                .Append(_context.Transforms.Categorical.OneHotEncoding("BedStatus"))
            //                .Append(_context.Transforms.Categorical.OneHotEncoding("HospitalName"))
            //                .Append(_context.Transforms.Categorical.OneHotEncoding("HospitalLocation"))
            //                .Append(_context.Transforms.Categorical.OneHotEncoding("Gender"))
            //                .Append(_context.Transforms.Categorical.OneHotEncoding("AdmissionStatus"))
            //                .Append(_context.Transforms.Categorical.OneHotEncoding("EmergencyType"))
            //                .Append(_context.Transforms.Concatenate("Features", "NumericFeatures", "CategoricalFeatures"))
            //                .Append(_context.Transforms.NormalizeMinMax("Features"));

            // Data processing pipeline
            var dataProcessPipeline = _context.Transforms.Concatenate("NumericFeatures",
                    "HospitalID", "BedID", "BedNumber", "PatientID", "Age", "CaseID", "PredictedOccupiedBeds", "PredictionID")
                //.Append(_context.Transforms.Conversion.MapValueToKey("BedType"))
                //.Append(_context.Transforms.Conversion.MapValueToKey("BedStatus"))
                //.Append(_context.Transforms.Conversion.MapValueToKey("HospitalName"))
                //.Append(_context.Transforms.Conversion.MapValueToKey("HospitalLocation"))
                //.Append(_context.Transforms.Conversion.MapValueToKey("Gender"))
                //.Append(_context.Transforms.Conversion.MapValueToKey("AdmissionStatus"))
                //.Append(_context.Transforms.Conversion.MapValueToKey("EmergencyType"))
                //.Append(_context.Transforms.Concatenate("CategoricalFeatures", "BedType", "BedStatus", "HospitalName", "HospitalLocation", "Gender", "AdmissionStatus", "EmergencyType"))
                .Append(_context.Transforms.Concatenate("Features", "NumericFeatures"))
                .Append(_context.Transforms.NormalizeMinMax("Features"));

            // Define training pipeline
            var trainingPipeline = dataProcessPipeline
                .Append(_context.Regression.Trainers.Sdca(labelColumnName: "PredictedAvailableBeds", maximumNumberOfIterations: 100));

            // Train the model
            _model = trainingPipeline.Fit(data);

            // Evaluate the model
            var predictions = _model.Transform(data);
            var metrics = _context.Regression.Evaluate(predictions, labelColumnName: "PredictedAvailableBeds");

            Console.WriteLine($"R^2 Score: {metrics.RSquared}");
            Console.WriteLine($"Root Mean Squared Error: {metrics.RootMeanSquaredError}");

            SaveModel();
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

        // Predict available beds using the trained model
        public float PredictAvailableBeds(EnhancedPredictionData input)
        {
            var predictionFunction = _context.Model.CreatePredictionEngine<EnhancedPredictionData, Prediction>(_model);
            var result = predictionFunction.Predict(input);
            return result.PredictedAvailableBeds;
        }
    }
}
