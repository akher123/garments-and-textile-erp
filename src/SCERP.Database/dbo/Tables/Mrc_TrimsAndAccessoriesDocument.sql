CREATE TABLE [dbo].[Mrc_TrimsAndAccessoriesDocument] (
    [TrimsAndAccessoriesDocumentId]    INT              IDENTITY (1, 1) NOT NULL,
    [TrimsAndAccessoriesDevelopmentId] INT              NOT NULL,
    [Title]                            NVARCHAR (100)   NOT NULL,
    [Description]                      NVARCHAR (MAX)   NULL,
    [DocumentPath]                     NVARCHAR (MAX)   NOT NULL,
    [CreatedDate]                      DATETIME         NULL,
    [CreatedBy]                        UNIQUEIDENTIFIER NULL,
    [EditedDate]                       DATETIME         NULL,
    [EditedBy]                         UNIQUEIDENTIFIER NULL,
    [IsActive]                         BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_TrimAndAccessoriesDocument] PRIMARY KEY CLUSTERED ([TrimsAndAccessoriesDocumentId] ASC),
    CONSTRAINT [FK_Mrc_TrimsAndAccessoriesDocument_Mrc_TrimsAndAccessoriesDevelopment] FOREIGN KEY ([TrimsAndAccessoriesDevelopmentId]) REFERENCES [dbo].[Mrc_TrimsAndAccessoriesDevelopment] ([TrimsAndAccessoriesDevelopmentId])
);

