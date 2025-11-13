CREATE TABLE [dbo].[Document] (
    [DocumentId]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompId]      VARCHAR (3)    NOT NULL,
    [Name]        VARCHAR (100)  NOT NULL,
    [Path]        VARCHAR (MAX)  NOT NULL,
    [RefId]       VARCHAR (20)   NOT NULL,
    [SrcType]     INT            NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [IsActive]    BIT            NOT NULL,
    CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED ([DocumentId] ASC)
);

