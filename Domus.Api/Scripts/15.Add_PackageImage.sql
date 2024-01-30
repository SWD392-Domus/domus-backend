IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PackageImage')
BEGIN

CREATE TABLE [PackageImage] (
    [Id]						UNIQUEIDENTIFIER						NOT NULL,
    [PackageId]			        UNIQUEIDENTIFIER						NOT NULL,
    [ImageUrl]					NVARCHAR(MAX)							NOT NULL,
    [Width]						INT,
    [Height]					INT
    )
END

ALTER TABLE [PackageImage]
    ADD CONSTRAINT "PK_PackageImage" 			PRIMARY KEY ("Id")

ALTER TABLE [PackageImage]
    ADD FOREIGN KEY (PackageId)					REFERENCES [Package](Id)