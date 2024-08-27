CREATE TABLE [dbo].[EmergencyCases] (
    CaseID INT PRIMARY KEY,
    PatientID INT NOT NULL,
    HospitalID INT NOT NULL,
    BedID INT NOT NULL,
    AdmissionTime DATETIME NOT NULL,
    EmergencyType NVARCHAR(100) NOT NULL,
    FOREIGN KEY (PatientID) REFERENCES [dbo].[Patients](PatientID),
    FOREIGN KEY (HospitalID) REFERENCES [dbo].[Hospitals](HospitalID),
    FOREIGN KEY (BedID) REFERENCES [dbo].[Beds](BedID)
);
