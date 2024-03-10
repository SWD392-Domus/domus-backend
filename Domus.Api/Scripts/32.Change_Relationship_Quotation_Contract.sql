ALTER TABLE Contract
ADD Status BIT DEFAULT 0

ALTER TABLE Contract
ADD QuotationRevisionId UNIQUEIDENTIFIER NOT NULL

ALTER TABLE Contract 
DROP CONSTRAINT FK__Contract__Quotat__6E01572D

ALTER TABLE Contract
DROP COLUMN QuotationId
    
ALTER TABLE Contract
ADD FOREIGN KEY ("QuotationRevisionId") REFERENCES [QuotationRevision]("Id")