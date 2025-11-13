CREATE TABLE [dbo].[OvertimeSettings] (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [OvertimeHours] DECIMAL (18, 2)  NOT NULL,
    [OvertimeRate]  DECIMAL (18, 2)  NOT NULL,
    [FromDate]      DATETIME         NOT NULL,
    [ToDate]        DATETIME         NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [EditedDate]    DATETIME         NULL,
    [EditedBy]      UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              CONSTRAINT [DF_OvertimeSettings_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_OvertimeSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
);

