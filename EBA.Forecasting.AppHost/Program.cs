var builder = DistributedApplication.CreateBuilder(args);

// please uncomment if you're planning to use caching.
//var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.EBA_Forecasting_ApiService>("EBA-ApiService");

// Provisions a containerized SQL Server database when published
//var sqlpassword = builder.AddParameter("sql-password", secret: true);

//var sqlServerDBInstance = builder.AddSqlServer("EBA-SqlServer", sqlpassword).AddDatabase("EBA-DB");
//builder.AddProject<Projects.EBA_Forecasting_ServiceDefaults>("EBA-DB")
//                       .WithReference(sqlServerDBInstance);

builder.Build().Run();