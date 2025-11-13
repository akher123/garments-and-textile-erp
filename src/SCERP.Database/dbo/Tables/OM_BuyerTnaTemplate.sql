CREATE TABLE [dbo].[OM_BuyerTnaTemplate] (
    [TemplateId]     INT              IDENTITY (1, 1) NOT NULL,
    [CompId]         CHAR (3)         NOT NULL,
    [BuyerRefId]     CHAR (3)         NOT NULL,
    [TemplateTypeId] INT              NOT NULL,
    [ActivityId]     INT              NOT NULL,
    [Duration]       FLOAT (53)       NOT NULL,
    [Remarks]        VARCHAR (200)    NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [CreatedDate]    DATE             NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATE             NULL,
    [SerialNo]       INT              NOT NULL,
    [RSerialNo]      INT              NULL,
    [RType]          CHAR (1)         NULL,
    [FDuration]      FLOAT (53)       NULL,
    CONSTRAINT [PK_OM_BuerTnaTemplate] PRIMARY KEY CLUSTERED ([TemplateId] ASC)
);

