CREATE TABLE [dbo].[PLAN_ResponsiblePerson] (
    [Id]                           INT              IDENTITY (1, 1) NOT NULL,
    [ResponsiblePersonId]          UNIQUEIDENTIFIER NOT NULL,
    [ResponsiblePersonName]        NVARCHAR (100)   NOT NULL,
    [ResponsiblePersonDisplayName] NVARCHAR (50)    NOT NULL,
    [CreatedDate]                  DATETIME         NULL,
    [CreatedBy]                    UNIQUEIDENTIFIER NULL,
    [EditedDate]                   DATETIME         NULL,
    [EditedBy]                     UNIQUEIDENTIFIER NULL,
    [IsActive]                     BIT              NOT NULL,
    CONSTRAINT [PK_PLAN_ResponsiblePerson] PRIMARY KEY CLUSTERED ([ResponsiblePersonId] ASC)
);

