CREATE TABLE [dbo].[Mrc_SampleType] (
    [SampleTypeId] INT              IDENTITY (1, 1) NOT NULL,
    [SampleName]   NVARCHAR (100)   NOT NULL,
    [Description]  NVARCHAR (MAX)   NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL,
    CONSTRAINT [PK_SampleSettings] PRIMARY KEY CLUSTERED ([SampleTypeId] ASC)
);

