CREATE TABLE [dbo].[PROD_Production] (
    [ProductionId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [CompId]          VARCHAR (3)      NULL,
    [ProductionRefId] VARCHAR (10)     NULL,
    [ProdDate]        DATE             NULL,
    [ProcessRefId]    VARCHAR (3)      NULL,
    [ProrgramRefId]   VARCHAR (10)     NULL,
    [ProcessorRefId]  VARCHAR (3)      NULL,
    [MachineRefId]    VARCHAR (3)      NULL,
    [UserId]          UNIQUEIDENTIFIER NULL,
    [Rmks]            NVARCHAR (100)   NULL,
    [SlipNo]          NVARCHAR (15)    NULL,
    [PType]           VARCHAR (1)      NULL,
    CONSTRAINT [PK_PROD_Production] PRIMARY KEY CLUSTERED ([ProductionId] ASC)
);

