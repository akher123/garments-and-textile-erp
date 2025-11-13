CREATE TABLE [dbo].[TrackConfirmationMedia] (
    [ConfirmationMediaId] INT              IDENTITY (1, 1) NOT NULL,
    [ConfirmationMedia]   NVARCHAR (100)   NOT NULL,
    [CompanyId]           NVARCHAR (3)     NOT NULL,
    [Remarks]             NVARCHAR (MAX)   NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [CreatedDate]         DATETIME         NULL,
    [EditedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]          DATETIME         NULL,
    [IsActive]            BIT              NOT NULL,
    CONSTRAINT [PK_ENV_ConfirmationMedia] PRIMARY KEY CLUSTERED ([ConfirmationMediaId] ASC)
);

