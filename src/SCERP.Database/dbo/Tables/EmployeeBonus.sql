CREATE TABLE [dbo].[EmployeeBonus] (
    [EmployeeBonusId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]      UNIQUEIDENTIFIER NOT NULL,
    [BonusTypeId]     INT              NULL,
    [Amount]          DECIMAL (18, 2)  NOT NULL,
    [EffectiveDate]   DATETIME         NOT NULL,
    [Remarks]         NVARCHAR (MAX)   NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NOT NULL,
    [BonusRuleId]     INT              NOT NULL,
    CONSTRAINT [PK_EmployeeBonus] PRIMARY KEY CLUSTERED ([EmployeeBonusId] ASC),
    CONSTRAINT [FK_EmployeeBonus_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId])
);

