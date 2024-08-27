ALTER TABLE [dbo].[Beds]
ADD CONSTRAINT UC_Bed UNIQUE (HospitalID, BedNumber);
