CREATE TABLE [dbo].[TrackVehicle] (
    [VehicleId]   INT              IDENTITY (1, 1) NOT NULL,
    [CompanyId]   NVARCHAR (3)     NOT NULL,
    [VehicheType] NVARCHAR (50)    NOT NULL,
    [Remarks]     NVARCHAR (MAX)   NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [CreatedDate] DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [EditedDate]  DATETIME         NULL,
    [IsActive]    BIT              NOT NULL,
    CONSTRAINT [PK_Vehicle_1] PRIMARY KEY CLUSTERED ([VehicleId] ASC)
);

