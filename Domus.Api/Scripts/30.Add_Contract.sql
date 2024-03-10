EXEC sp_rename 'Contract.CreatedBy', 'ClientId', 'COLUMN';
EXEC sp_rename 'Contract.LastUpdatedBy', 'ContractorId', 'COLUMN';

ALTER TABLE [Contract] 
    DROP COLUMN LastUpdatedAt;
ALTER TABLE [Contract] 
    DROP COLUMN CreatedAt;
ALTER TABLE [Contract]
    DROP COLUMN Status;
ALTER TABLE [Contract] 
    DROP COLUMN SignedAt;
ALTER TABLE [Contract] 
    DROP COLUMN EndDate;

ALTER TABLE [Contract] 
    ADD Description NVARCHAR(MAX);
ALTER TABLE [Contract] 
    ADD Name NVARCHAR(MAX);
ALTER TABLE [Contract] 
    ADD Signature NVARCHAR(MAX);

