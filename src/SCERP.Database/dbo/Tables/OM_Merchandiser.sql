CREATE TABLE [dbo].[OM_Merchandiser] (
    [MerchandiserId] INT            IDENTITY (1, 1) NOT NULL,
    [CompId]         VARCHAR (3)    NULL,
    [EmpId]          VARCHAR (4)    NOT NULL,
    [EmpName]        NVARCHAR (50)  NOT NULL,
    [Address1]       NVARCHAR (100) NULL,
    [Address2]       NVARCHAR (100) NULL,
    [Address3]       NVARCHAR (100) NULL,
    [TeamLdrId]      VARCHAR (5)    NULL,
    [TeamId]         INT            NOT NULL,
    [Email]          NVARCHAR (50)  NULL,
    [Phone]          NVARCHAR (50)  NULL,
    CONSTRAINT [PK_OM_Merchandiser] PRIMARY KEY CLUSTERED ([MerchandiserId] ASC)
);

