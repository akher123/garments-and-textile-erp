CREATE TABLE [dbo].[ManPowerApprovedBySection] (
    [ManPowerApprovedBySectionId] INT              IDENTITY (1, 1) NOT NULL,
    [SectionId]                   INT              NULL,
    [SectionName]                 NVARCHAR (100)   NULL,
    [ApprovedManPower]            INT              NULL,
    [CreatedDate]                 DATETIME         NULL,
    [CreatedBy]                   UNIQUEIDENTIFIER NULL,
    [EditedDate]                  DATETIME         NULL,
    [EditedBy]                    UNIQUEIDENTIFIER NULL,
    [IsActive]                    BIT              NOT NULL,
    CONSTRAINT [PK_ManPowerApprovedBySection] PRIMARY KEY CLUSTERED ([ManPowerApprovedBySectionId] ASC)
);

