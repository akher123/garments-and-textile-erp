CREATE TABLE [dbo].[PROD_NonProductiveTime] (
    [NonProductiveTimeId]   INT           IDENTITY (1, 1) NOT NULL,
    [BuyerRefId]            VARCHAR (4)   NOT NULL,
    [OrderNo]               VARCHAR (12)  NOT NULL,
    [OrderStyleRefId]       VARCHAR (7)   NOT NULL,
    [MachineId]             BIGINT        NOT NULL,
    [NptRefId]              CHAR (6)      NOT NULL,
    [StartTime]             DATETIME      NOT NULL,
    [EndTime]               DATETIME      NULL,
    [Solution]              VARCHAR (150) NULL,
    [Supervisor]            VARCHAR (150) NOT NULL,
    [DownTimeCategoryId]    INT           NOT NULL,
    [EntryDate]             DATETIME      NULL,
    [ResponsibleDepartment] VARCHAR (100) NOT NULL,
    [Remarks]               VARCHAR (150) NULL,
    [CompId]                VARCHAR (3)   NULL,
    [Manpower]              INT           NOT NULL,
    CONSTRAINT [PK_PROD_NonProductiveTime] PRIMARY KEY CLUSTERED ([NonProductiveTimeId] ASC)
);

