CREATE TABLE [dbo].[Users] (
    UserID INT PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    HospitalID INT,
    Available BIT,
    FOREIGN KEY (HospitalID) REFERENCES [dbo].[Hospitals](HospitalID)
);
