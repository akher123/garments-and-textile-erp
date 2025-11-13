CREATE TABLE [dbo].[OM_TnaActivity] (
    [ActivityId]  INT            IDENTITY (1, 1) NOT NULL,
    [SlNo]        INT            NOT NULL,
    [Name]        VARCHAR (100)  NOT NULL,
    [ShortName]   NVARCHAR (20)  NOT NULL,
    [Responsible] NVARCHAR (100) NULL,
    [MaskId]      INT            NULL,
    [XStatus]     CHAR (1)       NOT NULL,
    CONSTRAINT [PK_OM_TnaActivity] PRIMARY KEY CLUSTERED ([ActivityId] ASC)
);

