CREATE TABLE [dbo].[PLAN_ProgramDetail] (
    [ProgramDetailId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [ProgramId]       BIGINT          NOT NULL,
    [CompId]          VARCHAR (3)     NOT NULL,
    [PrgramRefId]     VARCHAR (10)    NOT NULL,
    [MType]           CHAR (1)        NULL,
    [ItemCode]        VARCHAR (8)     NOT NULL,
    [ColorRefId]      VARCHAR (4)     NULL,
    [LotRefId]        VARCHAR (4)     NULL,
    [SizeRefId]       VARCHAR (4)     NULL,
    [FinishSizeRefId] VARCHAR (4)     NULL,
    [SleeveLength]    VARCHAR (25)    NULL,
    [Quantity]        DECIMAL (18, 5) NOT NULL,
    [Rate]            FLOAT (53)      NULL,
    [GSM]             VARCHAR (50)    NULL,
    [Remarks]         NVARCHAR (150)  NULL,
    [YRatio]          FLOAT (53)      NOT NULL,
    [NoOfCone]        INT             NULL,
    [ComponentRefId]  VARCHAR (3)     NULL,
    CONSTRAINT [PK_PLAN_ProgramDetail] PRIMARY KEY CLUSTERED ([ProgramDetailId] ASC),
    CONSTRAINT [FK_PLAN_ProgramDetail_PLAN_Program] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[PLAN_Program] ([ProgramId])
);

