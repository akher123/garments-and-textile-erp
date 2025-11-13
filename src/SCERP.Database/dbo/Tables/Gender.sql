CREATE TABLE [dbo].[Gender] (
    [GenderId]       TINYINT          IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (100)   NOT NULL,
    [TitleInBengali] NVARCHAR (100)   NOT NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_Gender] PRIMARY KEY CLUSTERED ([GenderId] ASC)
);

