CREATE TABLE [dbo].[Beds] (
    BedID INT PRIMARY KEY,
    HospitalID INT NOT NULL,
    BedNumber INT NOT NULL,
    BedType NVARCHAR(50) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    FOREIGN KEY (HospitalID) REFERENCES [dbo].[Hospitals](HospitalID)
);
