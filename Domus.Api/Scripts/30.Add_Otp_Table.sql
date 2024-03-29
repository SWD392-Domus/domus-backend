﻿CREATE TABLE [Otp] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [Code] NVARCHAR(256) NOT NULL,
    [CreatedAt] DATETIME NOT NULL,
    [Used] BIT DEFAULT 0,
    [UserId] NVARCHAR(450) FOREIGN KEY REFERENCES [DomusUser]([Id])
)