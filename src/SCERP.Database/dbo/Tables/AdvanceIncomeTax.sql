CREATE TABLE [dbo].[AdvanceIncomeTax] (
    [AdvanceTaxId]   INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]     UNIQUEIDENTIFIER NOT NULL,
    [EmployeeCardId] NVARCHAR (10)    NOT NULL,
    [Amount]         DECIMAL (18, 2)  NULL,
    [FromDate]       DATE             NULL,
    [ToDate]         DATE             NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              CONSTRAINT [DF_AdvanceIncomeTax_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_AdvanceIncomeTax] PRIMARY KEY CLUSTERED ([AdvanceTaxId] ASC)
);

