CREATE TABLE [dbo].[Mrc_ApprovalStatus] (
    [ApprovalStatusId]          INT              IDENTITY (1, 1) NOT NULL,
    [ApprovalStatusTitle]       NVARCHAR (100)   NOT NULL,
    [ApprovalStatusDescription] NVARCHAR (MAX)   NULL,
    [CreatedDate]               DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [EditedDate]                DATETIME         NULL,
    [EditedBy]                  UNIQUEIDENTIFIER NULL,
    [IsActive]                  BIT              NOT NULL,
    CONSTRAINT [PK_Mrc_ApprovalStatus] PRIMARY KEY CLUSTERED ([ApprovalStatusId] ASC)
);

