ALTER TABLE [Quotation]
ADD FOREIGN KEY (QuotationStatusId) REFERENCES [QuotationStatus](Id)
