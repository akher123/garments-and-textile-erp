CREATE TABLE [dbo].[OM_TnAAlert] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [PSDate]     VARCHAR (20)   NULL,
    [DDiff]      INT            NULL,
    [BeforeDays] FLOAT (53)     NOT NULL,
    [Receiver]   VARCHAR (50)   NULL,
    [Msg]        NVARCHAR (160) NULL,
    CONSTRAINT [PK_OM_TnAAlert] PRIMARY KEY CLUSTERED ([Id] ASC)
);

