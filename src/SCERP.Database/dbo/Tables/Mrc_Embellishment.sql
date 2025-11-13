CREATE TABLE [dbo].[Mrc_Embellishment] (
    [EmbellishmentId]     INT              IDENTITY (1, 1) NOT NULL,
    [EmbellishmentTypeId] INT              NOT NULL,
    [Name]                NVARCHAR (100)   NOT NULL,
    [Description]         NVARCHAR (MAX)   NULL,
    [CreatedDate]         DATETIME         NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NULL,
    [EditedDate]          DATETIME         NULL,
    [EditedBy]            UNIQUEIDENTIFIER NULL,
    [IsActive]            BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_Embellishment] PRIMARY KEY CLUSTERED ([EmbellishmentId] ASC),
    CONSTRAINT [FK_Mrc_Embellishment_Mrc_EmbellishmentType] FOREIGN KEY ([EmbellishmentTypeId]) REFERENCES [dbo].[Mrc_EmbellishmentType] ([EmbellishmentTypeId])
);

