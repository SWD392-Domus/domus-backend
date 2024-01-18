-- Create Tables

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DomusUser')
BEGIN

CREATE TABLE [DomusUser] (
[Id]						NVARCHAR(450)							NOT NULL,
[UserName]					NVARCHAR(256),
[NormalizedUserName]		NVARCHAR(256),
[Email]						NVARCHAR(256),
[NormalizedEmail]			NVARCHAR(256),
[EmailConfirmed]			BIT										NOT NULL,
[PasswordHash]				NVARCHAR(MAX),
[SecurityStamp]				NVARCHAR(MAX),
[ConcurrencyStamp]			NVARCHAR(MAX),
[PhoneNumber]				NVARCHAR(MAX),
[PhoneNumberConfirmed]		BIT										NOT NULL,
[TwoFactorEnabled]			BIT										NOT NULL,
[LockoutEnd]				DATETIMEOFFSET,
[LockoutEnabled]			BIT										NOT NULL,
[AccessFailedCount]			INT										NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AspNetUserClaims')
BEGIN

CREATE TABLE [AspNetUserClaims] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[UserId]					NVARCHAR(450)							NOT NULL,
[ClaimType]					NVARCHAR(MAX),
[ClaimValue]				NVARCHAR(MAX)
)

END


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AspNetUserLogins')
BEGIN

CREATE TABLE [AspNetUserLogins] (
[LoginProvider]				NVARCHAR(450)							NOT NULL,
[ProviderKey]				NVARCHAR(450)							NOT NULL,
[ProviderDisplayName]		NVARCHAR(MAX),
[UserId]					NVARCHAR(450)							NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AspNetUserRoles')
BEGIN

CREATE TABLE [AspNetUserRoles] (
[UserId]					NVARCHAR(450)							NOT NULL,
[RoleId]					NVARCHAR(450)							NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AspNetUserTokens')
BEGIN

CREATE TABLE [AspNetUserTokens] (
[UserId]					NVARCHAR(450)							NOT NULL,
[LoginProvider]				NVARCHAR(450)							NOT NULL,
[Name]						NVARCHAR(450)							NOT NULL,
[Value]						NVARCHAR(MAX)
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AspNetRoles')
BEGIN

CREATE TABLE [AspNetRoles] (
[Id]						NVARCHAR(450)							NOT NULL,
[Name]						NVARCHAR(256),
[NormalizedName]			NVARCHAR(256),
[ConcurrencyStamp]			NVARCHAR(MAX)
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AspNetRoleClaims')
BEGIN

CREATE TABLE [AspNetRoleClaims] (
[Id]						INT										NOT NULL,
[RoleId]					NVARCHAR(450)							NOT NULL,
[ClaimType]					NVARCHAR(MAX),
[ClaimValue]				NVARCHAR(MAX)
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProductCategory')
BEGIN

CREATE TABLE [ProductCategory] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[Name]						NVARCHAR(256)							NOT NULL,
[IsDeleted]					BIT										DEFAULT 0,
[ConcurrencyStamp]			NVARCHAR(MAX)
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Product')
BEGIN

CREATE TABLE [Product] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[ProductCategoryId]			UNIQUEIDENTIFIER						NOT NULL,
[ProductName]				NVARCHAR(256)							NOT NULL,
[Color]						NVARCHAR(256),
[Weight]					FLOAT,
[WeightUnit]				NVARCHAR(256),
[Style]						NVARCHAR(256),
[Brand]						NVARCHAR(256),
[Description]				NVARCHAR(MAX),
[IsDeleted]					BIT										DEFAULT 0,
[ConcurrencyStamp]			NVARCHAR(MAX)
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProductAttribute')
BEGIN

CREATE TABLE [ProductAttribute] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[AttributeName]				NVARCHAR(256)							NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProductAttributeValue')
BEGIN

CREATE TABLE [ProductAttributeValue] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[ProductAttributeId]		UNIQUEIDENTIFIER						NOT NULL,
[ProductDetailId]			UNIQUEIDENTIFIER						NOT NULL,
[Value]						NVARCHAR(256)							NOT NULL,
[ValueType]					NVARCHAR(256)							NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProductDetail')
BEGIN

CREATE TABLE [ProductDetail] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[ProductId]					UNIQUEIDENTIFIER						NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProductPrice')
BEGIN

CREATE TABLE [ProductPrice] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[ProductDetailId]			UNIQUEIDENTIFIER						NOT NULL,
[Price]						FLOAT									NOT NULL,
[MonetaryUnit]				NVARCHAR(256)							NOT NULL,
[Quantity]					FLOAT									NOT NULL,
[QuantityType]				NVARCHAR(256)							NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProductImage')
BEGIN

CREATE TABLE [ProductImage] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[ProductDetailId]			UNIQUEIDENTIFIER						NOT NULL,
[ImageUrl]					NVARCHAR(MAX)							NOT NULL,
[Width]						INT,
[Height]					INT
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Quotation')
BEGIN

CREATE TABLE [Quotation] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[CustomerId]				NVARCHAR(450)							NOT NULL,
[StaffId]					NVARCHAR(450)							NOT NULL,
[QuotationStatusId]			UNIQUEIDENTIFIER						NOT NULL,
[QuotationNegotiationLogId]	UNIQUEIDENTIFIER						NOT NULL,
[ExpireAt]					DATE,
[CreatedAt]					DATE									NOT NULL,
[LastUpdatedAt]				DATE,
[CreatedBy]					NVARCHAR(450)							NOT NULL,
[LastUpdatedBy]				NVARCHAR(450),
[IsDeleted]					BIT										DEFAULT 0,
[ConcurrencyStamp]			NVARCHAR(MAX)
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Article')
BEGIN

CREATE TABLE [Article] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[ArticleCategoryId]			UNIQUEIDENTIFIER						NOT NULL,
[Title] 					NVARCHAR(256)							NOT NULL,
[Content]					NVARCHAR(MAX)							NOT NULL,
[CreatedAt]					DATE									NOT NULL,
[LastUpdatedAt]				DATE,
[CreatedBy]					NVARCHAR(450)							NOT NULL,
[LastUpdatedBy]				NVARCHAR(450),
[IsDeleted]					BIT										DEFAULT 0,
[ConcurrencyStamp]			NVARCHAR(MAX)
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProductDetail_Quotation')
BEGIN

CREATE TABLE [ProductDetail_Quotation] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[QuotationId]				UNIQUEIDENTIFIER						NOT NULL,
[ProductDetailId]			UNIQUEIDENTIFIER						NOT NULL,
[Price]						FLOAT									NOT NULL,
[MonetaryUnit]				NVARCHAR(256)							NOT NULL,
[Quantity]					FLOAT									NOT NULL,
[QuantityType]				NVARCHAR(256)							NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProductDetail_QuotationRevision')
BEGIN

CREATE TABLE [ProductDetail_QuotationRevision] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[ProductDetailQuotationId]	UNIQUEIDENTIFIER						NOT NULL,
[Version]					INT										DEFAULT 0,
[Price]						FLOAT									NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Contract')
BEGIN

CREATE TABLE [Contract] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[QuotationId]				UNIQUEIDENTIFIER						NOT NULL,
[Status]					NVARCHAR(256),
[SignedAt]					DATE,
[StartDate]					DATE,
[EndDate]					DATE,
[Notes]						NVARCHAR(MAX),
[Attachments]				NVARCHAR(MAX),
[CreatedAt]					DATE									NOT NULL,
[LastUpdatedAt]				DATE,
[CreatedBy]					NVARCHAR(450)							NOT NULL,
[LastUpdatedBy]				NVARCHAR(450),
[IsDeleted]					BIT										DEFAULT 0,
[ConcurrencyStamp]			NVARCHAR(MAX)
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'QuotationStatus')
BEGIN

CREATE TABLE [QuotationStatus] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[StatusType]				NVARCHAR(256)							NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'QuotationService')
BEGIN

CREATE TABLE [QuotationService] (
[QuotationId]				UNIQUEIDENTIFIER						NOT NULL,
[ServiceId]					UNIQUEIDENTIFIER						NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Service')
BEGIN

CREATE TABLE [Service] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[Name]						NVARCHAR(256)							NOT NULL,
[Price]						FLOAT									NOT NULL,
[MonetaryUnit]				NVARCHAR(256)							NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'QuotationNegotiationLog')
BEGIN

CREATE TABLE [QuotationNegotiationLog] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[QuotationId]				UNIQUEIDENTIFIER						NOT NULL,
[IsClosed]					BIT										DEFAULT 0,
[StartAt]					DATE									NOT NULL,
[CloseAt]					DATE
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'NegotiationMessage')
BEGIN

CREATE TABLE [NegotiationMessage] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[SentAt]					DATE									NOT NULL,
[IsCustomerMessage]			BIT										NOT NULL,
[Content]					NVARCHAR(MAX)
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ArticleCategory')
BEGIN

CREATE TABLE [ArticleCategory] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[Name]						NVARCHAR(256)							NOT NULL
)

END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ArticleImage')
BEGIN

CREATE TABLE [ArticleImage] (
[Id]						UNIQUEIDENTIFIER						NOT NULL,
[ArticleId]					UNIQUEIDENTIFIER						NOT NULL,
[ImageUrl]					NVARCHAR(MAX)							NOT NULL,
[Width]						INT,
[Height]					INT
)

END

-- End Create Tables

-- Add Primary Keys

ALTER TABLE [DomusUser]
ADD CONSTRAINT "PK_DomusUser"				PRIMARY KEY ("Id")

ALTER TABLE [AspNetUserClaims]
ADD CONSTRAINT "PK_UserClaim"				PRIMARY KEY ("Id")

ALTER TABLE [AspNetUserLogins]
ADD CONSTRAINT "PK_UserLogin"				PRIMARY KEY ("LoginProvider", "ProviderKey")

ALTER TABLE [AspNetRoles]
ADD CONSTRAINT "PK_Role" 					PRIMARY KEY ("Id")

ALTER TABLE [AspNetRoleClaims]
ADD CONSTRAINT "PK_RoleClaim" 				PRIMARY KEY ("Id")

ALTER TABLE [ProductCategory]
ADD CONSTRAINT "PK_ProductCategory"			PRIMARY KEY ("Id")

ALTER TABLE [Product]
ADD CONSTRAINT "PK_Product" 				PRIMARY KEY ("Id")

ALTER TABLE [ProductAttribute]
ADD CONSTRAINT "PK_ProductAttribute" 		PRIMARY KEY ("Id")

ALTER TABLE [ProductAttributeValue]
ADD CONSTRAINT "PK_ProductAttributeValue" 	PRIMARY KEY ("Id")

ALTER TABLE [ProductDetail]
ADD CONSTRAINT "PK_ProductDetail" 			PRIMARY KEY ("Id")

ALTER TABLE [ProductPrice]
ADD CONSTRAINT "PK_ProductPrice" 			PRIMARY KEY ("Id")

ALTER TABLE [ProductImage]
ADD CONSTRAINT "PK_ProductImage" 			PRIMARY KEY ("Id")

ALTER TABLE [Quotation]
ADD CONSTRAINT "PK_Quotation" 				PRIMARY KEY ("Id")

ALTER TABLE [ProductDetail_Quotation]
ADD CONSTRAINT "PK_ProductDetail_Quotation"	PRIMARY KEY ("Id")

ALTER TABLE [Contract]
ADD CONSTRAINT "PK_Contract" 				PRIMARY KEY ("Id")

ALTER TABLE [Article]
ADD CONSTRAINT "PK_Article" 				PRIMARY KEY ("Id")

ALTER TABLE [ArticleCategory]
ADD CONSTRAINT "PK_ArticleCategory"			PRIMARY KEY ("Id")

ALTER TABLE [ArticleImage]
ADD CONSTRAINT "PK_ArticleImage"			PRIMARY KEY ("Id")

ALTER TABLE [Service]
ADD CONSTRAINT "PK_Service"					PRIMARY KEY ("Id")

ALTER TABLE [QuotationStatus]
ADD CONSTRAINT "PK_QuotationStatus"			PRIMARY KEY ("Id")

ALTER TABLE [QuotationNegotiationLog]
ADD CONSTRAINT "PK_QuotationNegotiationLog"	PRIMARY KEY ("Id")

ALTER TABLE [NegotiationMessage]
ADD CONSTRAINT "PK_NegotiationMessage"		PRIMARY KEY ("Id")

ALTER TABLE [ProductDetail_QuotationRevision]
ADD CONSTRAINT "PK_ProductDetail_QuotationRevision"	PRIMARY KEY ("Id")

-- End Add Primary Keys

-- Add Foreign Keys

ALTER TABLE [AspNetRoleClaims]
ADD FOREIGN KEY (RoleId)					REFERENCES [AspNetRoles](Id)

ALTER TABLE [AspNetUserRoles]
ADD FOREIGN KEY (UserId)					REFERENCES [DomusUser](Id)

ALTER TABLE [AspNetUserRoles]
ADD FOREIGN KEY (RoleId)					REFERENCES [AspNetRoles](Id)

ALTER TABLE [AspNetUserClaims]
ADD FOREIGN KEY (UserId) 					REFERENCES [DomusUser](Id)

ALTER TABLE [AspNetUserLogins]
ADD FOREIGN KEY (UserId)					REFERENCES [DomusUser](Id)

ALTER TABLE [AspNetUserTokens]
ADD FOREIGN KEY (UserId)					REFERENCES [DomusUser](Id)

ALTER TABLE [ProductAttributeValue]
ADD FOREIGN KEY (ProductAttributeId)		REFERENCES [ProductAttribute](Id)

ALTER TABLE [ProductAttributeValue]
ADD FOREIGN KEY (ProductDetailId)			REFERENCES [ProductDetail](Id)

ALTER TABLE [Product]
ADD FOREIGN KEY (ProductCategoryId)			REFERENCES [ProductCategory](Id)

ALTER TABLE [ProductDetail]
ADD FOREIGN KEY (ProductId)					REFERENCES [Product](Id)

ALTER TABLE [ProductPrice]
ADD FOREIGN KEY (ProductDetailId)			REFERENCES [ProductDetail](Id)

ALTER TABLE [ProductImage]
ADD FOREIGN KEY (ProductDetailId)			REFERENCES [ProductDetail](Id)

ALTER TABLE [Quotation]
ADD FOREIGN KEY (CustomerId)				REFERENCES [DomusUser](Id)

ALTER TABLE [Quotation]
ADD FOREIGN KEY (StaffId)					REFERENCES [DomusUser](Id)

ALTER TABLE [Quotation]
ADD FOREIGN KEY (CreatedBy)					REFERENCES [DomusUser](Id)

ALTER TABLE [Quotation]
ADD FOREIGN KEY (LastUpdatedBy)				REFERENCES [DomusUser](Id)

ALTER TABLE [Quotation]
ADD FOREIGN KEY (QuotationNegotiationLogId)	REFERENCES [QuotationNegotiationLog](Id)

ALTER TABLE [ProductDetail_Quotation]
ADD FOREIGN KEY (QuotationId)				REFERENCES [Quotation](Id)

ALTER TABLE [ProductDetail_Quotation]
ADD FOREIGN KEY (ProductDetailId)			REFERENCES [ProductDetail](Id)

ALTER TABLE [ProductDetail_QuotationRevision]
ADD FOREIGN KEY (ProductDetailQuotationId)	REFERENCES [ProductDetail_Quotation](Id)

ALTER TABLE [Article]
ADD FOREIGN KEY (CreatedBy)					REFERENCES [DomusUser](Id)

ALTER TABLE [Article]
ADD FOREIGN KEY (LastUpdatedBy)				REFERENCES [DomusUser](Id)

ALTER TABLE [Article]
ADD FOREIGN KEY (ArticleCategoryId)			REFERENCES [ArticleCategory](Id)

ALTER TABLE [ArticleImage]
ADD FOREIGN KEY (ArticleId)					REFERENCES [Article](Id)

ALTER TABLE [QuotationNegotiationLog]
ADD FOREIGN KEY (QuotationId)				REFERENCES [Quotation](Id)

ALTER TABLE [Contract]
ADD FOREIGN KEY (QuotationId)				REFERENCES [Quotation](Id)

ALTER TABLE [Contract]
ADD FOREIGN KEY (CreatedBy)					REFERENCES [DomusUser](Id)

ALTER TABLE [Contract]
ADD FOREIGN KEY (LastUpdatedBy)				REFERENCES [DomusUser](Id)

ALTER TABLE [QuotationService]
ADD FOREIGN KEY (QuotationId)				REFERENCES [Quotation](Id)

ALTER TABLE [QuotationService]
ADD FOREIGN KEY (ServiceId)					REFERENCES [Service](Id)

-- End Add Foreign Keys

-- Add Composite Primary Keys

ALTER TABLE [AspNetUserRoles]
ADD CONSTRAINT "PK_UserRole"				PRIMARY KEY ("UserId", "RoleId")

ALTER TABLE [AspNetUserTokens]
ADD CONSTRAINT "PK_UserToken"				PRIMARY KEY ("UserId", "LoginProvider", "Name")

ALTER TABLE [QuotationService]
ADD CONSTRAINT "PK_QuotationService"		PRIMARY KEY ("QuotationId", "ServiceId")

-- End Add Composite Primary Key
