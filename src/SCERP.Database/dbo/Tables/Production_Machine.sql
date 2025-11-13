CREATE TABLE [dbo].[Production_Machine] (
    [MachineId]      INT              IDENTITY (1, 1) NOT NULL,
    [CompId]         VARCHAR (3)      NULL,
    [MachineRefId]   VARCHAR (3)      NULL,
    [ProcessorRefId] VARCHAR (3)      NULL,
    [ProcessRefId]   VARCHAR (3)      NULL,
    [Name]           NVARCHAR (100)   NOT NULL,
    [Description]    NVARCHAR (MAX)   NULL,
    [NoMachine]      INT              NULL,
    [EfficiencyPer]  NUMERIC (18, 5)  NULL,
    [IdelPer]        NUMERIC (18, 5)  NULL,
    [RatedCapacity]  NUMERIC (18, 5)  NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              CONSTRAINT [DF_Production_Machin_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Production_Machin] PRIMARY KEY CLUSTERED ([MachineId] ASC)
);

