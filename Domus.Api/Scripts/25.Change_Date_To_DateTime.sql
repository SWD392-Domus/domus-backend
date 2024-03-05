-- Article
ALTER TABLE [Article]
ALTER COLUMN [CreatedAt] DATETIME

ALTER TABLE [Article]
ALTER COLUMN [LastUpdatedAt] DATETIME

-- Contract
ALTER TABLE [Contract]
ALTER COLUMN [SignedAt] DATETIME

ALTER TABLE [Contract]
ALTER COLUMN [CreatedAt] DATETIME

ALTER TABLE [Contract]
ALTER COLUMN [LastUpdatedAt] DATETIME

-- NegotiationMessage
ALTER TABLE [NegotiationMessage]
ALTER COLUMN [SentAt] DATETIME

-- Quotation
ALTER TABLE [Quotation]
ALTER COLUMN [ExpireAt] DATETIME

ALTER TABLE [Quotation]
ALTER COLUMN [CreatedAt] DATETIME

ALTER TABLE [Quotation]
ALTER COLUMN [LastUpdatedAt] DATETIME

-- QuotationNegotiationLog
ALTER TABLE [QuotationNegotiationLog]
ALTER COLUMN [StartAt] DATETIME

ALTER TABLE [QuotationNegotiationLog]
ALTER COLUMN [CloseAt] DATETIME