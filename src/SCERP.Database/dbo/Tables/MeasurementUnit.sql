CREATE TABLE [dbo].[MeasurementUnit] (
    [UnitId]      INT              IDENTITY (1, 1) NOT NULL,
    [UnitName]    NVARCHAR (100)   NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [CreatedDate] DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [EditedDate]  DATETIME         NULL,
    [EditedBy]    UNIQUEIDENTIFIER NULL,
    [IsActive]    BIT              CONSTRAINT [DF_MeasurementUnit_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_MeasurementUnit] PRIMARY KEY CLUSTERED ([UnitId] ASC)
);

