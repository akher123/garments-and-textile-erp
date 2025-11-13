CREATE TABLE [dbo].[PLAN_StyleUF] (
    [Id]      INT       IDENTITY (1, 1) NOT NULL,
    [CompID]  CHAR (3)  NULL,
    [FLD]     CHAR (10) NULL,
    [FDes]    CHAR (25) NULL,
    [OTypeID] CHAR (2)  NULL,
    CONSTRAINT [PK_PLAN_StyleUF_2] PRIMARY KEY CLUSTERED ([Id] ASC)
);

