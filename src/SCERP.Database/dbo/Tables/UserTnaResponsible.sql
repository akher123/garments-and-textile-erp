CREATE TABLE [dbo].[UserTnaResponsible] (
    [UserTnaResponsibleId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]           UNIQUEIDENTIFIER NOT NULL,
    [Responsible]          VARCHAR (150)    NOT NULL,
    [IsActive]             BIT              NOT NULL,
    CONSTRAINT [PK_UserTnaResponsible] PRIMARY KEY CLUSTERED ([UserTnaResponsibleId] ASC)
);

