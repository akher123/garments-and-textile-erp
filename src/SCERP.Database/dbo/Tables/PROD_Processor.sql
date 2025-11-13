CREATE TABLE [dbo].[PROD_Processor] (
    [ProcessorId]    INT            IDENTITY (1, 1) NOT NULL,
    [ProcessRefId]   VARCHAR (3)    NULL,
    [CompId]         VARCHAR (3)    NULL,
    [ProcessorRefId] VARCHAR (3)    NULL,
    [ProcessorName]  NVARCHAR (100) NULL,
    CONSTRAINT [PK_PROD_Processor] PRIMARY KEY CLUSTERED ([ProcessorId] ASC)
);

