IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Quotation' AND COLUMN_NAME = 'QuotationStatusId')
BEGIN
	DECLARE @ConstraintName AS NVARCHAR(256)
	DECLARE @Sql AS NVARCHAR(MAX)

	SELECT @ConstraintName = fk.name
	FROM sys.foreign_keys fk
	JOIN sys.tables tp ON fk.parent_object_id = tp.object_id
	JOIN sys.tables ref ON fk.referenced_object_id = ref.object_id
	WHERE ref.name = 'QuotationStatus'

	SELECT @Sql = 'ALTER TABLE [Quotation] DROP CONSTRAINT ' + @ConstraintName
	EXEC sp_executesql @Sql

	ALTER TABLE [Quotation]
	DROP COLUMN [QuotationStatusId]

	DROP TABLE [QuotationStatus]
END

IF NOT EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Quotation' AND COLUMN_NAME = 'Status')
BEGIN
	ALTER TABLE [Quotation]
	ADD [Status] NVARCHAR(256) NOT NULL
END
