CREATE TABLE [dbo].[GatePassDetail] (
    [GatePassDetailId] INT           IDENTITY (1, 1) NOT NULL,
    [GatePassId]       INT           NOT NULL,
    [Item]             VARCHAR (150) NULL,
    [Description]      VARCHAR (250) NULL,
    [Unit]             VARCHAR (50)  NULL,
    [Quantity]         FLOAT (53)    NULL,
    [Remarks]          VARCHAR (150) NULL,
    [Color]            VARCHAR (150) NULL,
    [Size]             VARCHAR (150) NULL,
    [Wrapper]          VARCHAR (50)  NULL,
    [WrappingQty]      FLOAT (53)    NULL,
    [FColorName]       VARCHAR (150) NULL,
    [ParentId]         INT           NULL,
    CONSTRAINT [PK_GatePassDetail] PRIMARY KEY CLUSTERED ([GatePassDetailId] ASC),
    CONSTRAINT [FK_GatePassDetail_GatePass] FOREIGN KEY ([GatePassId]) REFERENCES [dbo].[GatePass] ([GatePassId])
);

