CREATE TABLE [dbo].[BranchUnit] (
    [BranchUnitId] INT              IDENTITY (1, 1) NOT NULL,
    [BranchId]     INT              NOT NULL,
    [UnitId]       INT              NOT NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL,
    CONSTRAINT [PK_BranchUnit] PRIMARY KEY CLUSTERED ([BranchUnitId] ASC),
    CONSTRAINT [FK_BranchUnit_Branch] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BranchUnit_Unit] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[Unit] ([UnitId])
);

