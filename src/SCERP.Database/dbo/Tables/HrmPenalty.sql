CREATE TABLE [dbo].[HrmPenalty] (
    [PenaltyId]      INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeId]     UNIQUEIDENTIFIER NOT NULL,
    [EmployeeCardId] NVARCHAR (100)   NOT NULL,
    [PenaltyTypeId]  INT              NOT NULL,
    [Penalty]        NUMERIC (18, 2)  NOT NULL,
    [PenaltyDate]    DATETIME         NOT NULL,
    [Reason]         NVARCHAR (MAX)   NULL,
    [ClaimerId]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]    DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [EditedDate]     DATETIME         NULL,
    [EditedBy]       UNIQUEIDENTIFIER NULL,
    [IsActive]       BIT              NOT NULL,
    CONSTRAINT [PK_HrmPenalty] PRIMARY KEY CLUSTERED ([PenaltyId] ASC)
);

