﻿ALTER TABLE [Quotation]
ADD [PackageId] UNIQUEIDENTIFIER

ALTER TABLE [Quotation]
ADD FOREIGN KEY (PackageId) REFERENCES [Package](Id)