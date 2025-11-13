CREATE TABLE [dbo].[Mrc_Fabrication] (
    [FabricationId] INT              IDENTITY (1, 1) NOT NULL,
    [Fabrication]   NVARCHAR (200)   NOT NULL,
    [Description]   NVARCHAR (MAX)   NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_Fabrication] PRIMARY KEY CLUSTERED ([FabricationId] ASC)
);

