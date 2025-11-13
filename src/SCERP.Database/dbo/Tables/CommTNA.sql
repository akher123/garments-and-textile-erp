CREATE TABLE [dbo].[CommTNA] (
    [CommTnaRowId]   INT            IDENTITY (1, 1) NOT NULL,
    [LCRefId]        INT            NOT NULL,
    [CompId]         NVARCHAR (3)   NULL,
    [SerialId]       INT            NULL,
    [Activity]       NVARCHAR (200) NULL,
    [PlanDate]       NVARCHAR (50)  NULL,
    [ActualDate]     NVARCHAR (50)  NULL,
    [ActivityStatus] NVARCHAR (15)  NULL,
    [Remarks]        NVARCHAR (200) NULL,
    CONSTRAINT [PK_CommTNA] PRIMARY KEY CLUSTERED ([CommTnaRowId] ASC)
);

