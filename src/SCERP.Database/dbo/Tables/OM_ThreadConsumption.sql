CREATE TABLE [dbo].[OM_ThreadConsumption] (
    [ThreadConsumptionId] INT              IDENTITY (1, 1) NOT NULL,
    [BuyerRefId]          VARCHAR (4)      NOT NULL,
    [OrderNo]             VARCHAR (12)     NOT NULL,
    [OrderStyleRefId]     VARCHAR (7)      NOT NULL,
    [SizeRefId]           VARCHAR (4)      NOT NULL,
    [EntryDate]           DATETIME         NOT NULL,
    [Remarks]             VARCHAR (150)    NULL,
    [CompId]              VARCHAR (4)      NOT NULL,
    [CreatedBy]           UNIQUEIDENTIFIER NOT NULL,
    [ApprovedBy]          UNIQUEIDENTIFIER NULL,
    [IsApproved]          BIT              NOT NULL,
    CONSTRAINT [PK_OM_ThreadConsumption] PRIMARY KEY CLUSTERED ([ThreadConsumptionId] ASC)
);

