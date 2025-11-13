CREATE TABLE [dbo].[TmReport] (
    [ReportId]    INT            NOT NULL,
    [SubjectId]   INT            NOT NULL,
    [ReportName]  NVARCHAR (100) NOT NULL,
    [ReportImage] IMAGE          NOT NULL,
    [ReportNo]    NVARCHAR (50)  NOT NULL,
    [CompId]      VARCHAR (3)    NOT NULL,
    [Remarks]     VARCHAR (200)  NULL,
    CONSTRAINT [PK_TmReport] PRIMARY KEY CLUSTERED ([ReportId] ASC)
);

