CREATE TABLE [dbo].[Mrc_StyleSize] (
    [StyleSizeId]          INT              IDENTITY (1, 1) NOT NULL,
    [StyleSizeTitle]       NVARCHAR (50)    NOT NULL,
    [StyleCode]            NVARCHAR (50)    NULL,
    [StyleSizeDescription] NVARCHAR (MAX)   NULL,
    [CreatedDate]          DATETIME         NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [EditedDate]           DATETIME         NULL,
    [EditedBy]             UNIQUEIDENTIFIER NULL,
    [IsActive]             BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_StyleSize] PRIMARY KEY CLUSTERED ([StyleSizeId] ASC)
);

