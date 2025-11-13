CREATE TABLE [dbo].[Inventory_RedyeingFabricIssue] (
    [RedyeingFabricIssueId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [RefNo]                 VARCHAR (6)      NOT NULL,
    [CompId]                VARCHAR (3)      NOT NULL,
    [PartyId]               BIGINT           NOT NULL,
    [ChallanNo]             VARCHAR (50)     NULL,
    [ChallanDate]           DATETIME         NOT NULL,
    [DriverName]            VARCHAR (150)    NULL,
    [DriverPhone]           VARCHAR (50)     NULL,
    [VehicleType]           VARCHAR (150)    NULL,
    [Remarks]               VARCHAR (150)    NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NOT NULL,
    [EditedBy]              UNIQUEIDENTIFIER NULL,
    [IsApproved]            BIT              NOT NULL,
    [ApprovedBy]            UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Inventory_RedyeingFabricIssue] PRIMARY KEY CLUSTERED ([RedyeingFabricIssueId] ASC),
    CONSTRAINT [FK_Inventory_RedyeingFabricIssue_Party] FOREIGN KEY ([PartyId]) REFERENCES [dbo].[Party] ([PartyId])
);

