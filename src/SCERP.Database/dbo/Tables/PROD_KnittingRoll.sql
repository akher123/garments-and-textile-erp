CREATE TABLE [dbo].[PROD_KnittingRoll] (
    [KnittingRollId]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [RollRefNo]       CHAR (12)     NOT NULL,
    [RollDate]        DATETIME      NOT NULL,
    [PartyId]         BIGINT        NOT NULL,
    [ProgramId]       BIGINT        NOT NULL,
    [MachineId]       INT           NOT NULL,
    [SizeRefId]       VARCHAR (4)   NOT NULL,
    [ColorRefId]      VARCHAR (4)   NOT NULL,
    [FinishSizeRefId] VARCHAR (4)   NOT NULL,
    [GSM]             VARCHAR (15)  NOT NULL,
    [Quantity]        FLOAT (53)    NOT NULL,
    [Rmks]            VARCHAR (150) NULL,
    [CompId]          VARCHAR (3)   NOT NULL,
    [ItemCode]        VARCHAR (10)  NOT NULL,
    [RollLength]      FLOAT (53)    NULL,
    [CharllRollNo]    VARCHAR (100) NULL,
    [StLength]        VARCHAR (150) NULL,
    [ComponentRefId]  CHAR (3)      NULL,
    [RejQuantity]     FLOAT (53)    NULL,
    [IsRejected]      BIT           NULL,
    CONSTRAINT [PK_PROD_KnittingRoll] PRIMARY KEY CLUSTERED ([KnittingRollId] ASC)
);

