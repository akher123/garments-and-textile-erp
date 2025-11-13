CREATE TABLE [dbo].[TmReportImageInfo] (
    [ReportImageId]    INT            IDENTITY (1, 1) NOT NULL,
    [SubjectId]        INT            NOT NULL,
    [ReportName]       NVARCHAR (100) NOT NULL,
    [ReportNo]         NVARCHAR (50)  NOT NULL,
    [ReportImageUrl]   VARCHAR (250)  NOT NULL,
    [CompId]           VARCHAR (3)    NOT NULL,
    [Remarks]          VARCHAR (200)  NULL,
    [ProjectReportUrl] VARCHAR (200)  NULL,
    CONSTRAINT [PK_TmReportImageInfo] PRIMARY KEY CLUSTERED ([ReportImageId] ASC)
);

