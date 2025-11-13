CREATE TABLE [dbo].[PROD_BundleCuttingSeqChar] (
    [SeqCharId] INT         IDENTITY (1, 1) NOT NULL,
    [XSC]       INT         NULL,
    [XSCName]   VARCHAR (2) NULL,
    CONSTRAINT [PK_PROD_BundleCuttingSeqChar] PRIMARY KEY CLUSTERED ([SeqCharId] ASC)
);

