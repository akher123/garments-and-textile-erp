CREATE TABLE [dbo].[Checkinout2] (
    [Logid]        INT          IDENTITY (1, 1) NOT NULL,
    [Userid]       VARCHAR (20) NOT NULL,
    [CheckTime]    DATETIME     NOT NULL,
    [CheckType]    VARCHAR (2)  NOT NULL,
    [Sensorid]     VARCHAR (10) NULL,
    [Checked]      BIT          NULL,
    [WorkType]     INT          NULL,
    [AttFlag]      INT          NULL,
    [OpenDoorFlag] BIT          NULL
);

