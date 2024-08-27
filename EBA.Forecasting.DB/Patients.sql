CREATE TABLE [dbo].[Patients] (
    PatientID INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Gender CHAR(1) CHECK ([Patients].[Gender] IN ('M', 'F')),
    AdmissionDate DATE NOT NULL,
    AdmissionStatus CHAR(3) CHECK ([Patients].[AdmissionStatus] IN ('IN', 'OUT')),
    DischargeDate DATE NULL,
    HospitalID INT NOT NULL,
    BedID INT NOT NULL,
    FOREIGN KEY (HospitalID) REFERENCES [dbo].[Hospitals](HospitalID),
    FOREIGN KEY (BedID) REFERENCES [dbo].[Beds](BedID)
);
