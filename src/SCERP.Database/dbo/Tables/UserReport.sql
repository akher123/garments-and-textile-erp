CREATE TABLE [dbo].[UserReport] (
    [UserReportId]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserName]      NVARCHAR (50) NOT NULL,
    [SqlQuaryRefId] VARCHAR (8)   NOT NULL,
    [IsEnable]      BIT           NOT NULL,
    CONSTRAINT [PK_UserReport] PRIMARY KEY CLUSTERED ([UserReportId] ASC)
);

