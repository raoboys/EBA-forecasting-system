using Swashbuckle.AspNetCore.Filters;

namespace EBA.Forecasting.ServiceDefaults.Services.Models
{
    public class BedAvailability
    {
        public BedAvailabilityParameters? Data { get; set; }

        public Forecast? Forecast { get; set; }
    }

    public class BedAvailabilityParameters
    {
        public float BedNumber { get; set; }
        public float StaffAvailable { get; set; }
        public float Age { get; set; }
        public float AvailableBeds { get; set; }
        public float OccupiedBeds { get; set; }
        public string BedType { get; set; }
        public string BedStatus { get; set; }
        public string HospitalName { get; set; }
        public string HospitalLocation { get; set; }
        public string Gender { get; set; }
        public string AdmissionStatus { get; set; }
        public string EmergencyType { get; set; }
        public string Season { get; set; }
        public string WeatherConditions { get; set; }
        public string DiseaseOutbreak { get; set; }
    }

    public class BedAvailabilityParametersExample : IExamplesProvider<BedAvailabilityParameters>
    {
        public BedAvailabilityParameters GetExamples()
        {
            return new BedAvailabilityParameters
            {
                BedNumber = 101,
                StaffAvailable = 5,
                Age = 65,
                AvailableBeds = 10,
                OccupiedBeds = 5,
                BedType = "ICU",
                BedStatus = "Occupied",
                HospitalName = "AIIMS",
                HospitalLocation = "Delhi",
                Gender = "M",
                AdmissionStatus = "IN",
                EmergencyType = "Cardiac Arrest",
                Season = "Winter",
                WeatherConditions = "Snowy",
                DiseaseOutbreak = "None"
            };
        }
    }

    public class BedAvailabilityExample : IExamplesProvider<BedAvailability>
    {
        public BedAvailability GetExamples()
        {
            return new BedAvailability
            {
                Data = new BedAvailabilityParameters
                {
                    BedNumber = 101,
                    StaffAvailable = 5,
                    Age = 65,
                    AvailableBeds = 10,
                    OccupiedBeds = 5,
                    BedType = "ICU",
                    BedStatus = "Occupied",
                    HospitalName = "AIIMS",
                    HospitalLocation = "Delhi",
                    Gender = "Male",
                    AdmissionStatus = "IN",
                    EmergencyType = "Cardiac Arrest",
                    Season = "Winter",
                    WeatherConditions = "Snowy",
                    DiseaseOutbreak = "None"
                },
                Forecast = new Forecast
                {
                    ForecastedAvailableBeds = new float[] { 8, 9, 10 },
                    ForecastedMinimumAvailableBeds = new float[] { 7, 8, 9 },
                    ForecastedMaximumAvailableBeds = new float[] { 9, 10, 11 },
                    ForecastedOccupiedBeds = new float[] { 5, 6, 7 },
                    ForecastedMinimumOccupiedBeds = new float[] { 4, 5, 6 },
                    ForecastedMaximumOccupiedBeds = new float[] { 6, 7, 8 }
                }
            };
        }
    }
}
