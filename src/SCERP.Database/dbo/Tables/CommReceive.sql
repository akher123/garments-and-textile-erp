CREATE TABLE [dbo].[CommReceive] (
    [ReceiveId]         INT              IDENTITY (1, 1) NOT NULL,
    [ReceiveRefNo]      NVARCHAR (100)   NULL,
    [BbLcId]            INT              NULL,
    [ReceiveDate]       DATETIME         NULL,
    [PassBookPageNo]    NVARCHAR (50)    NULL,
    [MushakChallanNo]   NVARCHAR (50)    NULL,
    [MushakChallanDate] DATETIME         NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]        DATETIME         NULL,
    [EditedBy]          UNIQUEIDENTIFIER NULL,
    [IsActive]          BIT              NOT NULL,
    CONSTRAINT [PK_CommItemReceive] PRIMARY KEY CLUSTERED ([ReceiveId] ASC)
);

