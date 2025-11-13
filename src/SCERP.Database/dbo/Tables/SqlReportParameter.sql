CREATE TABLE [dbo].[SqlReportParameter] (
    [ReportParameterId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [CustomSqlQuaryId]  INT            NOT NULL,
    [Pname]             NVARCHAR (50)  NOT NULL,
    [Pvalue]            NVARCHAR (50)  NOT NULL,
    [LabelFor]          NVARCHAR (50)  NOT NULL,
    [Querystring]       NVARCHAR (MAX) NULL,
    [ControlType]       NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_SqlReportParameter] PRIMARY KEY CLUSTERED ([ReportParameterId] ASC),
    CONSTRAINT [FK_SqlReportParameter_CustomSqlQuary] FOREIGN KEY ([CustomSqlQuaryId]) REFERENCES [dbo].[CustomSqlQuary] ([CustomSqlQuaryId])
);

