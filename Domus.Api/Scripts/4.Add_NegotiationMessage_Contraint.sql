ALTER TABLE [NegotiationMessage]
ADD [QuotationNegotiationLogId] UNIQUEIDENTIFIER NOT NULL

ALTER TABLE [NegotiationMessage]
ADD FOREIGN KEY (QuotationNegotiationLogId) REFERENCES [QuotationNegotiationLog](Id)
