CREATE TABLE [dbo].[SystemLogs] (
    LogID INT PRIMARY KEY,
    Timestamp DATETIME NOT NULL,
    EventType NVARCHAR(50) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL
);
