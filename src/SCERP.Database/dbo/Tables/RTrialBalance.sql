CREATE TABLE [dbo].[RTrialBalance] (
    [SectorId]    INT             NULL,
    [AccountCode] NUMERIC (18)    NULL,
    [AccountName] NVARCHAR (500)  NULL,
    [OpAmt]       NUMERIC (38, 2) NULL,
    [DrAmt]       NUMERIC (38, 2) NULL,
    [CrAmt]       NUMERIC (38, 2) NULL,
    [GrID]        VARCHAR (1)     NULL
);

