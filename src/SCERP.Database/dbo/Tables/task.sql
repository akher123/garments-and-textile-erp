CREATE TABLE [dbo].[task] (
    [id]               INT          IDENTITY (1, 1) NOT NULL,
    [name]             VARCHAR (50) NULL,
    [start]            DATETIME     NOT NULL,
    [end]              DATETIME     NOT NULL,
    [parent_id]        INT          NULL,
    [ordinal]          INT          NULL,
    [ordinal_priority] DATETIME     NULL,
    [milestone]        BIT          NOT NULL,
    [complete]         INT          NOT NULL,
    [xStatus]          CHAR (1)     NULL,
    CONSTRAINT [PK_task] PRIMARY KEY CLUSTERED ([id] ASC)
);

