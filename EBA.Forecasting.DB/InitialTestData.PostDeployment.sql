/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Inserting Bulk Hospital Data
INSERT INTO [dbo].[Hospitals] ([dbo].[Hospitals].[HospitalID], [dbo].[Hospitals].[Name], [dbo].[Hospitals].[Location])
VALUES 
    (1, 'Max Super Specialty Hospital', 'New Delhi'),
    (2, 'Apollo Hospital', 'Chennai'),
    (3, 'Fortis Hospital', 'Bangalore'),
    (4, 'Kokilaben Dhirubhai Ambani Hospital', 'Mumbai'),
    (5, 'AIIMS', 'Delhi'),
    (6, 'Narayana Health', 'Bangalore'),
    (7, 'Medanta - The Medicity', 'Gurgaon'),
    (8, 'Jaslok Hospital', 'Mumbai');

-- Inserting Bulk Bed Data
INSERT INTO [dbo].[Beds] ([dbo].[Beds].[BedID], [dbo].[Beds].[HospitalID], [dbo].[Beds].[BedNumber], [dbo].[Beds].[BedType], [dbo].[Beds].[Status])
VALUES 
    (1, 1, 101, 'General', 'Available'),
    (2, 1, 102, 'ICU', 'Occupied'),
    (3, 1, 103, 'General', 'Available'),
    (4, 1, 104, 'ICU', 'Available'),
    (5, 2, 201, 'General', 'Occupied'),
    (6, 2, 202, 'ICU', 'Available'),
    (7, 2, 203, 'General', 'Available'),
    (8, 2, 204, 'ICU', 'Occupied'),
    (9, 3, 301, 'General', 'Available'),
    (10, 3, 302, 'ICU', 'Occupied'),
    (11, 3, 303, 'General', 'Available'),
    (12, 3, 304, 'ICU', 'Available'),
    (13, 4, 401, 'General', 'Occupied'),
    (14, 4, 402, 'ICU', 'Available'),
    (15, 4, 403, 'General', 'Available'),
    (16, 4, 404, 'ICU', 'Occupied'),
    (17, 5, 501, 'General', 'Available'),
    (18, 5, 502, 'ICU', 'Occupied'),
    (19, 5, 503, 'General', 'Available'),
    (20, 5, 504, 'ICU', 'Available'),
    (21, 6, 601, 'General', 'Occupied'),
    (22, 6, 602, 'ICU', 'Available'),
    (23, 6, 603, 'General', 'Available'),
    (24, 6, 604, 'ICU', 'Occupied'),
    (25, 7, 701, 'General', 'Available'),
    (26, 7, 702, 'ICU', 'Occupied'),
    (27, 7, 703, 'General', 'Available'),
    (28, 7, 704, 'ICU', 'Available'),
    (29, 8, 801, 'General', 'Occupied'),
    (30, 8, 802, 'ICU', 'Available'),
    (31, 8, 803, 'General', 'Available'),
    (32, 8, 804, 'ICU', 'Occupied');

-- Inserting Bulk Patient Data
INSERT INTO [dbo].[Patients] ([dbo].[Patients].[PatientID], [dbo].[Patients].[Name], [dbo].[Patients].[Age], [dbo].[Patients].[Gender], [dbo].[Patients].[AdmissionDate], [dbo].[Patients].[DischargeDate], [dbo].[Patients].[HospitalID], [dbo].[Patients].[BedID], [dbo].[Patients].[Season], [dbo].[Patients].[WeatherConditions], [dbo].[Patients].[DiseaseOutbreak])
VALUES 
    (1, 'Arun Patel', 45, 'M', '2024-08-15', NULL, 1, 2, 'Summer', 'Clear', 'Covid-19'),
    (2, 'Neha Sharma', 34, 'F', '2024-08-16', NULL, 2, 4, 'Winter', 'Rainy', 'None'),
    (3, 'Anita Reddy', 50, 'F', '2024-08-17', NULL, 3, 5, 'Monsoon', 'Cloudy', 'Flu'),
    (4, 'Rajesh Kumar', 29, 'M', '2024-08-18', NULL, 4, 8, 'Autumn', 'Stormy', 'None'),
    (5, 'Suresh Gupta', 60, 'M', '2024-08-20', NULL, 5, 12, 'Summer', 'Sunny', 'Covid-19'),
    (6, 'Pooja Singh', 40, 'F', '2024-08-21', NULL, 6, 16, 'Winter', 'Clear', 'None'),
    (7, 'Amit Verma', 55, 'F', '2024-08-22', NULL, 7, 20, 'Monsoon', 'Foggy', 'None'),
    (8, 'Ravi Iyer', 65, 'M', '2024-08-23', NULL, 8, 24, 'Autumn', 'Windy', 'Flu'),
    (9, 'Sita Nair', 33, 'F', '2024-08-24', NULL, 1, 3, 'Summer', 'Clear', 'None'),
    (10, 'Vikram Mehta', 47, 'M', '2024-08-25', NULL, 2, 7, 'Spring', 'Partly Cloudy', 'None');

-- Inserting Bulk Prediction Data
INSERT INTO [dbo].[Predictions] ([dbo].[Predictions].[PredictionID], [dbo].[Predictions].[HospitalID], [dbo].[Predictions].[Date], [dbo].[Predictions].[PredictedAvailableBeds], [dbo].[Predictions].[PredictedOccupiedBeds])
VALUES 
    (1, 1, '2024-08-22', 5, 3),
    (2, 2, '2024-08-22', 7, 2),
    (3, 3, '2024-08-22', 4, 2),
    (4, 4, '2024-08-22', 6, 3),
    (5, 5, '2024-08-22', 8, 1),
    (6, 6, '2024-08-22', 4, 5),
    (7, 7, '2024-08-22', 6, 3),
    (8, 8, '2024-08-22', 5, 4),
    (9, 1, '2024-08-23', 6, 2),
    (10, 2, '2024-08-23', 7, 3);

-- Inserting Bulk Emergency Cases Data
INSERT INTO [dbo].[EmergencyCases] ([dbo].[EmergencyCases].[CaseID], [dbo].[EmergencyCases].[PatientID], [dbo].[EmergencyCases].[HospitalID], [dbo].[EmergencyCases].[BedID], [dbo].[EmergencyCases].[AdmissionTime], [dbo].[EmergencyCases].[EmergencyType])
VALUES 
    (1, 2, 2, 4, '2024-08-21 14:00', 'Cardiac Arrest'),
    (2, 4, 4, 8, '2024-08-21 16:00', 'Stroke'),
    (3, 5, 5, 12, '2024-08-22 09:00', 'Accident'),
    (4, 6, 6, 16, '2024-08-22 10:30', 'Diabetes'),
    (5, 7, 7, 20, '2024-08-22 12:00', 'Asthma'),
    (6, 8, 8, 24, '2024-08-22 14:00', 'Injury');

-- Inserting Bulk System Logs Data
INSERT INTO [dbo].[SystemLogs] ([dbo].[SystemLogs].[LogID], [dbo].[SystemLogs].[Timestamp], [dbo].[SystemLogs].[EventType], [dbo].[SystemLogs].[Description])
VALUES 
    (1, '2024-08-21 14:30:00', 'Login', 'User Ajay Kumar logged in.'),
    (2, '2024-08-21 14:32:00', 'Data Update', 'Bed status updated for Max Super Specialty Hospital.'),
    (3, '2024-08-21 14:45:00', 'Prediction', 'Bed availability prediction generated for Apollo Hospital.'),
    (4, '2024-08-21 15:00:00', 'Error', 'Error accessing database for Bed 302 at Fortis Hospital.'),
    (5, '2024-08-22 08:00:00', 'Login', 'User Sita Sharma logged in.'),
    (6, '2024-08-22 08:15:00', 'Data Update', 'Patient records updated for Kokilaben Dhirubhai Ambani Hospital.'),
    (7, '2024-08-22 08:30:00', 'Prediction', 'Bed availability prediction generated for AIIMS.'),
    (8, '2024-08-22 09:00:00', 'Error', 'Failed to connect to database for Narayana Health.');

-- Inserting Bulk User Data
INSERT INTO [dbo].[Users] ([dbo].[Users].[UserID], [dbo].[Users].[Username], [dbo].[Users].[PasswordHash], [dbo].[Users].[Role], [dbo].[Users].[HospitalID], [dbo].[Users].[Available])
VALUES 
    (1, 'admin_ajay', '[hashed_pw]', 'Admin', NULL, 1),
    (2, 'nurse_sita', '[hashed_pw]', 'Nurse', 1, 1),
    (3, 'doctor_ravi', '[hashed_pw]', 'Doctor', 2, 1),
    (4, 'admin_priya', '[hashed_pw]', 'Admin', NULL, 1),
    (5, 'nurse_anita', '[hashed_pw]', 'Nurse', 3, 1),
    (6, 'doctor_vikram', '[hashed_pw]', 'Doctor', 4, 1),
    (7, 'admin_sunil', '[hashed_pw]', 'Admin', 5, 1),
    (8, 'nurse_maya', '[hashed_pw]', 'Nurse', 6, 1);