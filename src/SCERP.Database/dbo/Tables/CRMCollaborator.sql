CREATE TABLE [dbo].[CRMCollaborator] (
    [Id]                      INT              IDENTITY (1, 1) NOT NULL,
    [CollaboratorId]          UNIQUEIDENTIFIER NOT NULL,
    [CollaboratorName]        NVARCHAR (100)   NOT NULL,
    [CollaboratorDisplayName] NVARCHAR (50)    NOT NULL,
    [CreatedDate]             DATETIME         NULL,
    [CreatedBy]               UNIQUEIDENTIFIER NULL,
    [EditedDate]              DATETIME         NULL,
    [EditedBy]                UNIQUEIDENTIFIER NULL,
    [IsActive]                BIT              NOT NULL,
    CONSTRAINT [PK_Collaborator] PRIMARY KEY CLUSTERED ([CollaboratorId] ASC)
);

