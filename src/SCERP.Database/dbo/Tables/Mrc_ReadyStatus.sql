CREATE TABLE [dbo].[Mrc_ReadyStatus] (
    [ReadyStatusId] INT              IDENTITY (1, 1) NOT NULL,
    [Status]        NVARCHAR (50)    NOT NULL,
    [Description]   NVARCHAR (MAX)   NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_ReadyStatus] PRIMARY KEY CLUSTERED ([ReadyStatusId] ASC)
);

