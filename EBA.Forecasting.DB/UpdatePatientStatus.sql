CREATE TRIGGER [dbo].[UpdatePatientStatus]
ON [dbo].[Patients]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
        -- Update the AdmissionStatus based on the DischargeDate
        UPDATE p SET p.AdmissionStatus = CASE WHEN i.DischargeDate IS NOT NULL THEN 'OUT' ELSE 'IN' 
    END
    FROM [dbo].[Patients] p INNER JOIN inserted i ON p.PatientID = i.PatientID;
END
