CREATE TABLE [dbo].[CommAccHead] (
    [AccHeadId]    INT              IDENTITY (1, 1) NOT NULL,
    [AccHeadName]  NVARCHAR (100)   NULL,
    [Particulars]  NVARCHAR (100)   NULL,
    [AccHeadType]  NVARCHAR (10)    NULL,
    [CurrencyId]   INT              NULL,
    [Amount]       DECIMAL (18, 4)  NULL,
    [Rate]         DECIMAL (18, 4)  NULL,
    [AmountInTaka] DECIMAL (18, 4)  NULL,
    [CompId]       NVARCHAR (3)     NULL,
    [DisplayOrder] INT              NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]   DATETIME         NULL,
    [EditedBy]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL,
    CONSTRAINT [PK_CommAccHead] PRIMARY KEY CLUSTERED ([AccHeadId] ASC)
);

