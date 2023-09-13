----------------------------------------------------
-- Use this script to fix problem with adding Image
-- record into the database after Video record has 
-- been added
----------------------------------------------------

-- Drop constraints to be able to remove Id column
ALTER TABLE [dbo].[Video] DROP CONSTRAINT [FK_Video_Images]
GO

ALTER TABLE [dbo].[Image]
DROP CONSTRAINT [PK_Image]
GO

-- Add new Id column (we need a different name, but we will change it to Id later)
ALTER TABLE [dbo].[Image]
ADD Id2 [int]
GO

-- Set Id2 to have identical values as Id (we'll actually remove Id)
UPDATE [dbo].[Image]
SET Id2=Id
GO

-- Remove Id column
ALTER TABLE [dbo].[Image]
DROP COLUMN Id
GO

-- Rename Id2 to Id
EXEC sp_rename 'dbo.Image.Id2', 'Id', 'COLUMN'
GO

-- Return constraints to new Id column
ALTER TABLE [dbo].[Image] 
ALTER COLUMN [Id] int NOT NULL
GO

ALTER TABLE [dbo].[Image]
ADD CONSTRAINT PK_Image PRIMARY KEY CLUSTERED (Id) 
GO

ALTER TABLE [dbo].[Video]  WITH CHECK ADD CONSTRAINT [FK_Video_Images] FOREIGN KEY([ImageId])
REFERENCES [dbo].[Image] ([Id])
GO

ALTER TABLE [dbo].[Video] CHECK CONSTRAINT [FK_Video_Images]
GO
