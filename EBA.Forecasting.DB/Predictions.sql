CREATE TABLE [dbo].[Predictions] (
    PredictionID INT PRIMARY KEY,
    HospitalID INT NOT NULL,
    Date DATE NOT NULL,
    PredictedAvailableBeds INT NOT NULL,
    PredictedOccupiedBeds INT NOT NULL,
    FOREIGN KEY (HospitalID) REFERENCES [dbo].[Hospitals](HospitalID)
);
