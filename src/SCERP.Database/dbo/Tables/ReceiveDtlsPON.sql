CREATE TABLE [dbo].[ReceiveDtlsPON] (
    [CompID]  CHAR (3)   NULL,
    [Ref]     CHAR (10)  NULL,
    [TypeID]  CHAR (2)   NULL,
    [CatID]   CHAR (2)   NULL,
    [PrdID]   CHAR (4)   NULL,
    [ColorID] CHAR (4)   NULL,
    [SizeID]  CHAR (4)   NULL,
    [UnitID]  CHAR (2)   NULL,
    [Qty]     FLOAT (53) NULL,
    [Rate]    FLOAT (53) NULL,
    [Rmks]    CHAR (100) NULL,
    [Unit]    CHAR (25)  NULL,
    [Dis]     FLOAT (53) NULL
);

