CREATE TABLE [dbo].[ReceivePON] (
    [CompID]  CHAR (3)   NULL,
    [Ref]     CHAR (10)  NULL,
    [MRRNo]   CHAR (15)  NULL,
    [MRRDate] DATE       NULL,
    [PONo]    CHAR (15)  NULL,
    [PODate]  DATE       NULL,
    [VHNo]    CHAR (50)  NULL,
    [ChlnNo]  CHAR (15)  NULL,
    [ChDate]  DATE       NULL,
    [GENo]    CHAR (15)  NULL,
    [GEDate]  DATE       NULL,
    [SupID]   CHAR (3)   NULL,
    [Rmks]    CHAR (100) NULL,
    [EmpID]   CHAR (5)   NULL,
    [STID]    CHAR (2)   NULL,
    [xType]   CHAR (1)   NULL,
    [PayRef]  CHAR (8)   NULL,
    [TAmt]    FLOAT (53) NULL,
    [TDis]    FLOAT (53) NULL,
    [NAmt]    FLOAT (53) NULL
);

