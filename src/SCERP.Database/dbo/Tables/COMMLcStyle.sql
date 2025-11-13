CREATE TABLE [dbo].[COMMLcStyle] (
    [LcStyleId]       INT              IDENTITY (1, 1) NOT NULL,
    [LcRefId]         INT              NULL,
    [OrderNo]         NVARCHAR (12)    NULL,
    [OrderStyleRefId] NVARCHAR (7)     NULL,
    [StyleQuantity]   DECIMAL (18, 2)  NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_COMMLcStyle] PRIMARY KEY CLUSTERED ([LcStyleId] ASC)
);

