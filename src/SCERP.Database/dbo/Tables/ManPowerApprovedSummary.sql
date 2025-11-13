CREATE TABLE [dbo].[ManPowerApprovedSummary] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [UnitName]      NVARCHAR (100) NULL,
    [SectionName]   NVARCHAR (100) NULL,
    [SectionId]     INT            NULL,
    [Designation]   NVARCHAR (100) NULL,
    [DesignationId] INT            NULL,
    [Regular]       INT            NULL,
    [NewJoin]       INT            NULL,
    [Total]         INT            NULL,
    [Present]       INT            NULL,
    [Absent]        INT            NULL,
    [Leave]         INT            NULL,
    [Approved]      INT            NULL,
    [Male]          INT            NULL,
    [Female]        INT            NULL,
    [AbsentPercent] INT            NULL,
    CONSTRAINT [PK_ManPowerApprovedSummary] PRIMARY KEY CLUSTERED ([Id] ASC)
);

