using Microsoft.ML.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBA.Forecasting.ServiceDefaults.Services.Models
{
    public class EnhancedPredictionData
    {
        [LoadColumn(0)]
        public int PredictionID { get; set; }

        [LoadColumn(1)]
        public int HospitalID { get; set; }

        [LoadColumn(2)]
        public DateTime PredictionDate { get; set; }

        [LoadColumn(3)]
        public int PredictedAvailableBeds { get; set; }

        [LoadColumn(4)]
        public int PredictedOccupiedBeds { get; set; }

        [LoadColumn(5)]
        public int BedID { get; set; }

        [LoadColumn(6)]
        public int BedNumber { get; set; }

        [LoadColumn(7)]
        public string BedType { get; set; }

        [LoadColumn(8)]
        public string BedStatus { get; set; }

        [LoadColumn(9)]
        public string HospitalName { get; set; }

        [LoadColumn(10)]
        public string HospitalLocation { get; set; }

        [LoadColumn(11)]
        public int PatientID { get; set; }

        [LoadColumn(12)]
        public int Age { get; set; }

        [LoadColumn(13)]
        public string Gender { get; set; }

        [LoadColumn(14)]
        public DateTime AdmissionDate { get; set; }

        [LoadColumn(15)]
        public string AdmissionStatus { get; set; }

        [LoadColumn(16)]
        public DateTime DischargeDate { get; set; }

        [LoadColumn(17)]
        public int CaseID { get; set; }

        [LoadColumn(18)]
        public string EmergencyType { get; set; }

        [LoadColumn(19)]
        public DateTime EmergencyAdmissionTime { get; set; }
    }

    public class Prediction
    {
        [ColumnName("Score")]
        public float PredictedAvailableBeds { get; set; }
    }
}
