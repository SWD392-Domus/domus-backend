CREATE TABLE Notification(
    Id  UNIQUEIDENTIFIER    NOT NULL,
    RecipientID NVARCHAR(450) NOT NULL,
    Content NVARCHAR(MAX),
    SentAt DATETIME,
    RedirectString NVARCHAR(MAX)
)

ALTER TABLE Notification
ADD PRIMARY KEY ("Id")

ALTER TABLE Notification
ADD FOREIGN KEY ("RecipientID") REFERENCES [DomusUser]("Id")