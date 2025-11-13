CREATE TABLE [dbo].[HrmPenaltyType] (
    [PenaltyTypeId] INT              IDENTITY (1, 1) NOT NULL,
    [Type]          NVARCHAR (50)    NOT NULL,
    [Description]   NVARCHAR (MAX)   NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              NOT NULL,
    CONSTRAINT [PK_HrmPenaltyType] PRIMARY KEY CLUSTERED ([PenaltyTypeId] ASC)
);

