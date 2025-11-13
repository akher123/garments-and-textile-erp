CREATE TABLE [dbo].[MarketingInstitute] (
    [InstituteId]     INT            IDENTITY (1, 1) NOT NULL,
    [InstituteName]   NVARCHAR (100) NOT NULL,
    [District]        NVARCHAR (50)  NULL,
    [Address]         NVARCHAR (250) NULL,
    [DecisionMaker]   NVARCHAR (150) NULL,
    [Designation]     NVARCHAR (100) NULL,
    [Mobile]          NVARCHAR (100) NULL,
    [Telephone]       NVARCHAR (50)  NULL,
    [Email]           NVARCHAR (100) NULL,
    [WebSite]         NVARCHAR (100) NULL,
    [IsAvailable]     BIT            NOT NULL,
    [Remarks]         NVARCHAR (MAX) NULL,
    [StatusId]        INT            NULL,
    [ClientEntryDate] DATE           NULL,
    [IsActive]        BIT            NULL,
    CONSTRAINT [PK_Institute2] PRIMARY KEY CLUSTERED ([InstituteId] ASC)
);

