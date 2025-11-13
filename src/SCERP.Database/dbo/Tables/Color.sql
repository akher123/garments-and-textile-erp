CREATE TABLE [dbo].[Color] (
    [ColorId]   BIGINT     IDENTITY (1, 1) NOT NULL,
    [ColorRef]  CHAR (4)   NULL,
    [ColorName] CHAR (100) NULL,
    [ColorCode] CHAR (40)  NULL,
    CONSTRAINT [PK_Color] PRIMARY KEY NONCLUSTERED ([ColorId] ASC)
);

