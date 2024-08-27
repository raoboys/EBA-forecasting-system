ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT CK_Age CHECK ([Patients].[Age] >= 0);
