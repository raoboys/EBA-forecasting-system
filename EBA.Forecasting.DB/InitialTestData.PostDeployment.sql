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
INSERT INTO [dbo].[Hospitals] ([HospitalID], [Name], [Location])
VALUES 
    (1, 'City Hospital', 'Downtown'),
    (2, 'Westside Clinic', 'Westville'),
    (3, 'Eastside Medical', 'Easttown'),
    (4, 'Central Hospital', 'Midcity'),
    (5, 'Northview Hospital', 'Northside'),
    (6, 'Southtown Medical Center', 'Southside'),
    (7, 'Riverdale Clinic', 'Riverside'),
    (8, 'Hilltop Hospital', 'Hilltown');

-- Inserting Bulk Bed Data
INSERT INTO [dbo].[Beds] ([BedID], [HospitalID], [BedNumber], [BedType], [Status])
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
INSERT INTO [dbo].[Patients] ([PatientID], [Name], [Age], [Gender], [AdmissionDate], [DischargeDate], [HospitalID], [BedID])
VALUES 
    (1, 'John Doe', 45, 'M', '2024-08-15', NULL, 1, 2),
    (2, 'Jane Smith', 34, 'F', '2024-08-16', NULL, 2, 4),
    (3, 'Alice Johnson', 50, 'F', '2024-08-17', NULL, 3, 5),
    (4, 'Bob Brown', 29, 'M', '2024-08-18', NULL, 4, 8),
    (5, 'Charlie Davis', 60, 'M', '2024-08-20', NULL, 5, 12),
    (6, 'Diana Evans', 40, 'F', '2024-08-21', NULL, 6, 16),
    (7, 'Emily Clark', 55, 'F', '2024-08-22', NULL, 7, 20),
    (8, 'Frank Green', 65, 'M', '2024-08-23', NULL, 8, 24),
    (9, 'Grace Lee', 33, 'F', '2024-08-24', NULL, 1, 3),
    (10, 'Henry Adams', 47, 'M', '2024-08-25', NULL, 2, 7);

-- Inserting Bulk Prediction Data
INSERT INTO [dbo].[Predictions] ([PredictionID], [HospitalID], [Date], [PredictedAvailableBeds], [PredictedOccupiedBeds])
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
INSERT INTO [dbo].[EmergencyCases] ([CaseID], [PatientID], [HospitalID], [BedID], [AdmissionTime], [EmergencyType])
VALUES 
    (1, 2, 2, 4, '2024-08-21 14:00', 'Cardiac Arrest'),
    (2, 4, 4, 8, '2024-08-21 16:00', 'Stroke'),
    (3, 5, 5, 12, '2024-08-22 09:00', 'Accident'),
    (4, 6, 6, 16, '2024-08-22 10:30', 'Diabetes'),
    (5, 7, 7, 20, '2024-08-22 12:00', 'Asthma'),
    (6, 8, 8, 24, '2024-08-22 14:00', 'Injury');

-- Inserting Bulk System Logs Data
INSERT INTO [dbo].[SystemLogs] ([LogID], [Timestamp], [EventType], [Description])
VALUES 
    (1, '2024-08-21 14:30:00', 'Login', 'User admin logged in.'),
    (2, '2024-08-21 14:32:00', 'Data Update', 'Bed status updated for Hospital 1.'),
    (3, '2024-08-21 14:45:00', 'Prediction', 'Bed availability prediction generated for Hospital 2.'),
    (4, '2024-08-21 15:00:00', 'Error', 'Error accessing database for Bed 302.'),
    (5, '2024-08-22 08:00:00', 'Login', 'User nurse_jane logged in.'),
    (6, '2024-08-22 08:15:00', 'Data Update', 'Patient records updated for Hospital 3.'),
    (7, '2024-08-22 08:30:00', 'Prediction', 'Bed availability prediction generated for Hospital 4.'),
    (8, '2024-08-22 09:00:00', 'Error', 'Failed to connect to database.');

-- Inserting Bulk User Data
INSERT INTO [dbo].[Users] ([UserID], [Username], [PasswordHash], [Role], [HospitalID])
VALUES 
    (1, 'admin', '[hashed_pw]', 'Admin', NULL),
    (2, 'nurse_jane', '[hashed_pw]', 'Nurse', 1),
    (3, 'doctor_smith', '[hashed_pw]', 'Doctor', 2),
    (4, 'admin_clark', '[hashed_pw]', 'Admin', NULL),
    (5, 'nurse_lisa', '[hashed_pw]', 'Nurse', 3),
    (6, 'doctor_brown', '[hashed_pw]', 'Doctor', 4),
    (7, 'admin_jones', '[hashed_pw]', 'Admin', 5),
    (8, 'nurse_lee', '[hashed_pw]', 'Nurse', 6);
