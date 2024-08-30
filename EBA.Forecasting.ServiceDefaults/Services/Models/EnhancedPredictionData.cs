using Microsoft.ML.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBA.Forecasting.ServiceDefaults.Services.Models
{
    //public class EnhancedPredictionData
    //{
    //    [LoadColumn(0), ColumnName("")]
    //    public int PredictionID { get; set; }

    //    [LoadColumn(1)]
    //    public int HospitalID { get; set; }

    //    [LoadColumn(17)]
    //    public int CaseID { get; set; }

    //    [LoadColumn(5)]
    //    public int BedID { get; set; }

    //    [LoadColumn(11)]
    //    public int PatientID { get; set; }

    //    [LoadColumn(6)]
    //    public int BedNumber { get; set; }

    //    [LoadColumn(12), ColumnName("Age")]
    //    public int Age { get; set; }

    //    [LoadColumn(13)]
    //    public string Gender { get; set; }

    //    [LoadColumn(2)]
    //    public DateTime PredictionDate { get; set; }

    //    [LoadColumn(3)]
    //    public Single PredictedAvailableBeds { get; set; }

    //    [LoadColumn(4)]
    //    public Single PredictedOccupiedBeds { get; set; }

    //    [LoadColumn(7)]
    //    public string BedType { get; set; }

    //    [LoadColumn(8)]
    //    public string BedStatus { get; set; }

    //    [LoadColumn(9)]
    //    public string HospitalName { get; set; }

    //    [LoadColumn(10)]
    //    public string HospitalLocation { get; set; }

    //    [LoadColumn(14)]
    //    public DateTime AdmissionDate { get; set; }

    //    [LoadColumn(15)]
    //    public string AdmissionStatus { get; set; }

    //    [LoadColumn(16)]
    //    public DateTime DischargeDate { get; set; }

    //    [LoadColumn(18)]
    //    public string EmergencyType { get; set; }

    //    [LoadColumn(19), ColumnName("EmergencyAdmissionTime")]
    //    public DateTime EmergencyAdmissionTime { get; set; }

    //    [LoadColumn(23), ColumnName("StaffAvailable")]
    //    public float StaffAvailable { get; set; }

    //    [LoadColumn(20), ColumnName("Season")]
    //    public string Season { get; set; }

    //    [LoadColumn(21), ColumnName("WeatherConditions")]
    //    public string WeatherConditions { get; set; }

    //    [LoadColumn(22), ColumnName("DiseaseOutbreak")]
    //    public string DiseaseOutbreak { get; set; }
    //}

    public class EnhancedPredictionData
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
    }


    public class BedAvailabilityForecast
    {
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

    public class BedAvailabilityData
    {
        public EnhancedPredictionData? EnhancedPredictionData { get; set; } 

        public BedAvailabilityForecast? BedAvailability { get; set; }
    }
}
