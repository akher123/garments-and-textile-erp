CREATE TABLE [dbo].[CommBBLCImageUpload] (
    [BBLCImageId] INT              IDENTITY (1, 1) NOT NULL,
    [BBLCId]      INT              NOT NULL,
    [ImageName]   NVARCHAR (100)   NULL,
    [Url]         NVARCHAR (250)   NULL,
    [SerialId]    INT              NULL,
    [CreatedDate] DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_BBLCImageUpload] PRIMARY KEY CLUSTERED ([BBLCImageId] ASC)
);

