CREATE TABLE [dbo].[Acc_ReportAging] (
    [SerialId]         INT              IDENTITY (1, 1) NOT NULL,
    [PartyName]        NVARCHAR (100)   NULL,
    [PartyGLId]        INT              NULL,
    [PartyGLCode]      NUMERIC (18)     NULL,
    [PartyControlCode] NUMERIC (18)     NULL,
    [CurrentMonth]     DECIMAL (18, 2)  NULL,
    [FirstMonth]       DECIMAL (18, 2)  NULL,
    [SecondMonth]      DECIMAL (18, 2)  NULL,
    [ThirdMonth]       DECIMAL (18, 2)  NULL,
    [TotalAmount]      DECIMAL (18, 2)  NULL,
    [TotalDisbursed]   DECIMAL (18, 2)  NULL,
    [IsChanged]        BIT              NULL,
    [CreatedDate]      DATETIME         NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NULL,
    [EditedDate]       DATETIME         NULL,
    [EditedBy]         UNIQUEIDENTIFIER NULL,
    [IsActive]         BIT              NOT NULL,
    CONSTRAINT [PK_Acc_ReportAging] PRIMARY KEY CLUSTERED ([SerialId] ASC)
);

