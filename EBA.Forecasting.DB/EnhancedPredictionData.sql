CREATE VIEW [dbo].[EnhancedPredictionData] AS
SELECT
    CAST(p.PredictionID As Real) As PredictionID,
    CAST(p.HospitalID As Real) As HospitalID,
    p.Date AS PredictionDate,
    CAST( p.PredictedAvailableBeds As Real) As PredictedAvailableBeds,
    CAST(p.PredictedOccupiedBeds As Real) As PredictedOccupiedBeds,
    CAST( b.BedID As Real) As BedID,
    CAST(b.BedNumber As Real) As BedNumber,
    b.BedType,
    b.Status AS BedStatus,
    h.Name AS HospitalName,
    h.Location AS HospitalLocation,
    CAST(pa.PatientID As Real) As PatientID,
    CAST(pa.Age As Real) As Age,
    pa.Gender,
    pa.AdmissionDate,
    pa.AdmissionStatus,
    pa.DischargeDate,
   CAST( ec.CaseID As Real) As CaseID,
    ec.EmergencyType,
    ec.AdmissionTime AS EmergencyAdmissionTime
FROM
    [dbo].[Predictions] p
    INNER JOIN [dbo].[Beds] b ON p.HospitalID= b.HospitalID
    INNER JOIN [dbo].[Hospitals] h ON p.HospitalID = h.HospitalID
    LEFT JOIN [dbo].[Patients] pa ON pa.BedID = b.BedID
    LEFT JOIN [dbo].[EmergencyCases] ec ON ec.PatientID = pa.PatientID
