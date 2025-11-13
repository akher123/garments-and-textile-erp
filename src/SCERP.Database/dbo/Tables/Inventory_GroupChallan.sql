CREATE TABLE [dbo].[Inventory_GroupChallan] (
    [GroupChallanId] INT              IDENTITY (1, 1) NOT NULL,
    [RefId]          CHAR (6)         NOT NULL,
    [GType]          INT              NOT NULL,
    [Name]           NVARCHAR (50)    NOT NULL,
    [GDate]          DATE             NOT NULL,
    [Remarks]        NVARCHAR (250)   NULL,
    [CreatedDate]    DATE             NOT NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NOT NULL,
    [EditedDate]     DATE             NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    [CompId]         CHAR (3)         NOT NULL,
    [PartyId]        BIGINT           NOT NULL,
    CONSTRAINT [PK_GroupChallan] PRIMARY KEY CLUSTERED ([GroupChallanId] ASC)
);

