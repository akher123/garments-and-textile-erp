CREATE TABLE [dbo].[UserMerchandiser] (
    [UserMerchandiserId] INT              IDENTITY (1, 1) NOT NULL,
    [CompId]             VARCHAR (3)      NOT NULL,
    [MerchandiserRefId]  NCHAR (4)        NOT NULL,
    [EmployeeId]         UNIQUEIDENTIFIER NOT NULL,
    [CreateDate]         DATETIME         NULL,
    [EditedDate]         DATETIME         NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [EditedBy]           UNIQUEIDENTIFIER NULL,
    [IsActive]           BIT              NOT NULL,
    CONSTRAINT [PK_UserMerchandiser] PRIMARY KEY CLUSTERED ([UserMerchandiserId] ASC)
);

