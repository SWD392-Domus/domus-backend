-- doi ten attribute
EXEC sp_rename 'Service.isDeleted', 'IsDeleted', 'COLUMN';
EXEC sp_rename 'Package.isDeleted', 'IsDeleted', 'COLUMN';
