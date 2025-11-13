CREATE TABLE [dbo].[PLAN_TNAXL] (
    [TnaRowId]          INT            IDENTITY (1, 1) NOT NULL,
    [CompId]            NVARCHAR (3)   NULL,
    [OrderStyleRefId]   NVARCHAR (7)   NULL,
    [ActivityName]      NVARCHAR (200) NULL,
    [SerialId]          INT            NULL,
    [LeadTime]          INT            NULL,
    [PSDate]            DATETIME       NULL,
    [PEDate]            DATETIME       NULL,
    [ActualStartDate]   DATETIME       NULL,
    [ActualEndDate]     DATETIME       NULL,
    [ResponsiblePerson] NVARCHAR (30)  NULL,
    [Remarks]           VARCHAR (100)  NULL,
    CONSTRAINT [PK_PLAN_TNAXL] PRIMARY KEY CLUSTERED ([TnaRowId] ASC)
);

