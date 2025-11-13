CREATE TABLE [dbo].[Weekends] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [DayName]  NVARCHAR (50) NOT NULL,
    [IsActive] BIT           NOT NULL,
    CONSTRAINT [PK_Weekends] PRIMARY KEY CLUSTERED ([Id] ASC)
);

