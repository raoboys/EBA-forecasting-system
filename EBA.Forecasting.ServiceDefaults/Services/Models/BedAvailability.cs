using Microsoft.ML.Data;

namespace EBA.Forecasting.ServiceDefaults.Services.Models
{
    public class BedAvailabilityData
    {
        [LoadColumn(0), ColumnName("PredictionID")]
        public int PredictionID { get; set; }

        [LoadColumn(1), ColumnName("HospitalID")]
        public int HospitalID { get; set; }

        [LoadColumn(2), ColumnName("PredictionDate")]
        public DateTime PredictionDate { get; set; }

        [LoadColumn(3), ColumnName("PredictedAvailableBeds")]
        public float PredictedAvailableBeds { get; set; }

        [LoadColumn(4), ColumnName("PredictedOccupiedBeds")]
        public float PredictedOccupiedBeds { get; set; }

        [LoadColumn(5), ColumnName("BedID")]
        public float BedID { get; set; }

        [LoadColumn(6), ColumnName("BedNumber")]
        public float BedNumber { get; set; }

        [LoadColumn(7), ColumnName("BedType")]
        public string BedType { get; set; }

        [LoadColumn(8), ColumnName("BedStatus")]
        public string BedStatus { get; set; }

        [LoadColumn(9), ColumnName("HospitalName")]
        public string HospitalName { get; set; }

        [LoadColumn(10), ColumnName("HospitalLocation")]
        public string HospitalLocation { get; set; }

        [LoadColumn(11), ColumnName("PatientID")]
        public float PatientID { get; set; }

        [LoadColumn(12), ColumnName("Age")]
        public float Age { get; set; }

        [LoadColumn(13), ColumnName("Gender")]
        public string Gender { get; set; }

        [LoadColumn(14), ColumnName("AdmissionDate")]
        public DateTime AdmissionDate { get; set; }

        [LoadColumn(15), ColumnName("AdmissionStatus")]
        public string AdmissionStatus { get; set; }

        [LoadColumn(16), ColumnName("DischargeDate")]
        public DateTime DischargeDate { get; set; }

        [LoadColumn(17), ColumnName("CaseID")]
        public float CaseID { get; set; }

        [LoadColumn(18), ColumnName("EmergencyType")]
        public string EmergencyType { get; set; }

        [LoadColumn(19), ColumnName("EmergencyAdmissionTime")]
        public DateTime EmergencyAdmissionTime { get; set; }

        [LoadColumn(20), ColumnName("Season")]
        public string Season { get; set; }

        [LoadColumn(21), ColumnName("WeatherConditions")]
        public string WeatherConditions { get; set; }

        [LoadColumn(22), ColumnName("DiseaseOutbreak")]
        public string DiseaseOutbreak { get; set; }

        [LoadColumn(23), ColumnName("StaffAvailable")]
        public float StaffAvailable { get; set; }

        public static BedAvailabilityData MapToData(BedAvailabilityParameters parameters)
        {
            return new BedAvailabilityData
            {
                BedNumber = parameters.BedNumber,
                StaffAvailable = parameters.StaffAvailable,
                Age = parameters.Age,
                PredictedAvailableBeds = parameters.AvailableBeds,
                PredictedOccupiedBeds = parameters.OccupiedBeds,
                BedType = parameters.BedType,
                BedStatus = parameters.BedStatus,
                HospitalName = parameters.HospitalName,
                HospitalLocation = parameters.HospitalLocation,
                Gender = parameters.Gender,
                AdmissionStatus = parameters.AdmissionStatus,
                EmergencyType = parameters.EmergencyType,
                Season = parameters.Season,
                WeatherConditions = parameters.WeatherConditions,
                DiseaseOutbreak = parameters.DiseaseOutbreak
            };
        }
    }


    public class Forecast
    {
        [VectorType(3)]
        [ColumnName("BedAvailabilityAnamolyPrediction")]
        public VBuffer<double> BedAvailabilityAnamolyPrediction { get; set; }

        [ColumnName("ForecastedAvailableBeds")]
        public float[] ForecastedAvailableBeds { get; set; } = Array.Empty<float>();

        [ColumnName("ForecastedMinimumAvailableBeds")]
        public float[] ForecastedMinimumAvailableBeds { get; set; } = Array.Empty<float>();

        [ColumnName("ForecastedMaximumAvailableBeds")]
        public float[] ForecastedMaximumAvailableBeds { get; set; } = Array.Empty<float>();

        [ColumnName("ForecastedOccupiedBeds")]
        public float[] ForecastedOccupiedBeds { get; set; } = Array.Empty<float>();

        [ColumnName("ForecastedMinimumOccupiedBeds")]
        public float[] ForecastedMinimumOccupiedBeds { get; set; } = Array.Empty<float>();

        [ColumnName("ForecastedMaximumOccupiedBeds")]
        public float[] ForecastedMaximumOccupiedBeds { get; set; } = Array.Empty<float>();
    }
}
