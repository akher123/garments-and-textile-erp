CREATE TABLE [dbo].[OM_BulkBookingYarnDetail] (
    [BulkBookingYearnDetailId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [BulkBookingDetailId]      BIGINT         NOT NULL,
    [ItemName]                 NVARCHAR (150) NULL,
    [ColorName]                VARCHAR (50)   NULL,
    [CountName]                VARCHAR (50)   NULL,
    [OrdQty]                   INT            NOT NULL,
    [ConsQty]                  FLOAT (53)     NOT NULL,
    [Remarks]                  NVARCHAR (MAX) NULL,
    [CompId]                   VARCHAR (3)    NULL,
    CONSTRAINT [PK_OM_BulkBookingYarnDetail] PRIMARY KEY CLUSTERED ([BulkBookingYearnDetailId] ASC),
    CONSTRAINT [FK_OM_BulkBookingYarnDetail_OM_BulkBookingDetail] FOREIGN KEY ([BulkBookingDetailId]) REFERENCES [dbo].[OM_BulkBookingDetail] ([BulkBookingDetailId])
);

