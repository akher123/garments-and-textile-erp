CREATE TABLE [dbo].[SMSInformation] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [UserName]    NVARCHAR (50)    NULL,
    [Password]    NVARCHAR (50)    NULL,
    [Sender]      NVARCHAR (50)    NULL,
    [CreatedDate] DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EditedDate]  DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              NULL,
    CONSTRAINT [PK_SMSInformation] PRIMARY KEY CLUSTERED ([Id] ASC)
);

