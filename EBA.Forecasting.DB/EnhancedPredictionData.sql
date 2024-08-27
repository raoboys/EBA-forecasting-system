CREATE VIEW [dbo].[EnhancedPredictionData] AS
SELECT
    p.PredictionID,
    p.HospitalID,
    p.Date AS PredictionDate,
    p.PredictedAvailableBeds,
    p.PredictedOccupiedBeds,
    b.BedID,
    b.BedNumber,
    b.BedType,
    b.Status AS BedStatus,
    h.Name AS HospitalName,
    h.Location AS HospitalLocation,
    pa.PatientID,
    pa.Age,
    pa.Gender,
    pa.AdmissionDate,
    pa.AdmissionStatus,
    pa.DischargeDate,
    ec.CaseID,
    ec.EmergencyType,
    ec.AdmissionTime AS EmergencyAdmissionTime
FROM
    [dbo].[Predictions] p
    INNER JOIN [dbo].[Beds] b ON p.HospitalID= b.HospitalID
    INNER JOIN [dbo].[Hospitals] h ON p.HospitalID = h.HospitalID
    LEFT JOIN [dbo].[Patients] pa ON pa.BedID = b.BedID
    LEFT JOIN [dbo].[EmergencyCases] ec ON ec.PatientID = pa.PatientID
