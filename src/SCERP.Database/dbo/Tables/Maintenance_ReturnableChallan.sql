CREATE TABLE [dbo].[Maintenance_ReturnableChallan] (
    [ReturnableChallanId]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [ReturnableChallanRefId] VARCHAR (6)      NOT NULL,
    [Messrs]                 VARCHAR (200)    NULL,
    [Address]                VARCHAR (MAX)    NULL,
    [RefferancePerson]       VARCHAR (100)    NULL,
    [Designation]            VARCHAR (100)    NULL,
    [Department]             VARCHAR (100)    NULL,
    [EmployeeCardId]         VARCHAR (100)    NULL,
    [ChallanDate]            DATETIME         NULL,
    [CompId]                 VARCHAR (3)      NOT NULL,
    [Remarks]                VARCHAR (MAX)    NULL,
    [Phone]                  VARCHAR (25)     NULL,
    [IsApproved]             BIT              NULL,
    [ApprovedBy]             UNIQUEIDENTIFIER NULL,
    [PreparedBy]             UNIQUEIDENTIFIER NULL,
    [ChllanType]             CHAR (1)         NULL,
    CONSTRAINT [PK_Maintenance_ReturnableChallan] PRIMARY KEY CLUSTERED ([ReturnableChallanId] ASC)
);

