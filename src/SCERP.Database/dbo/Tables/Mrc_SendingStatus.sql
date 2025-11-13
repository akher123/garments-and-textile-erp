CREATE TABLE [dbo].[Mrc_SendingStatus] (
    [SendingStatusId] INT              IDENTITY (1, 1) NOT NULL,
    [Status]          NVARCHAR (50)    NULL,
    [Description]     NVARCHAR (MAX)   NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_SendingStatus] PRIMARY KEY CLUSTERED ([SendingStatusId] ASC)
);

