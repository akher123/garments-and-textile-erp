CREATE TABLE [dbo].[AuthorizationType] (
    [Id]              INT              IDENTITY (1, 1) NOT NULL,
    [ProcessKeyId]    INT              NOT NULL,
    [AuthorizationId] INT              NOT NULL,
    [TypeName]        NVARCHAR (100)   NOT NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [EditedDate]      DATETIME         NULL,
    [EditedBy]        UNIQUEIDENTIFIER NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_AuthorizedType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

