CREATE TABLE [Package] (
	[Id]						UNIQUEIDENTIFIER					NOT NULL,
	[Name]						NVARCHAR(256)						NOT NULL
)

CREATE TABLE [Package_ProductDetail] (
	[PackageId]					UNIQUEIDENTIFIER					NOT NULL,
	[ProductDetailId]			UNIQUEIDENTIFIER					NOT NULL
)

CREATE TABLE [Package_Service] (
	[PackageId]					UNIQUEIDENTIFIER					NOT NULL,
	[ServiceId]					UNIQUEIDENTIFIER					NOT NULL
)

ALTER TABLE [Package]
ADD CONSTRAINT "PK_Package"		PRIMARY KEY ("Id")

ALTER TABLE [Package_Service]
ADD FOREIGN KEY (PackageId)		REFERENCES [Package](Id)

ALTER TABLE [Package_Service]
ADD FOREIGN KEY (ServiceId)		REFERENCES [Service](Id)

ALTER TABLE [Package_ProductDetail]
ADD FOREIGN KEY (PackageId)		REFERENCES [Package](Id)

ALTER TABLE [Package_ProductDetail]
ADD FOREIGN KEY (ProductDetailId)	REFERENCES [ProductDetail](Id)

ALTER TABLE [Package_Service]
ADD CONSTRAINT "PK_PackageService" PRIMARY KEY ("PackageId", "ServiceId")

ALTER TABLE [Package_ProductDetail]
ADD CONSTRAINT "PK_PackageProductDetail" PRIMARY KEY ("PackageId", "ProductDetailId")
