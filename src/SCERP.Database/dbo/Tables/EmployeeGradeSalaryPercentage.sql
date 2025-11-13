CREATE TABLE [dbo].[EmployeeGradeSalaryPercentage] (
    [EmployeeGradeSalaryPercentageId] INT              IDENTITY (1, 1) NOT NULL,
    [EmployeeGradeId]                 INT              NOT NULL,
    [Medical]                         DECIMAL (18, 2)  NOT NULL,
    [Conveyance]                      DECIMAL (18, 2)  NOT NULL,
    [Food]                            DECIMAL (18, 2)  NOT NULL,
    [HouseRentPercentage]             DECIMAL (18, 2)  NOT NULL,
    [BasicPercentageRate]             DECIMAL (18, 2)  NOT NULL,
    [CreatedDate]                     DATETIME         NULL,
    [CreatedBy]                       UNIQUEIDENTIFIER NULL,
    [EditedDate]                      DATETIME         NULL,
    [EditedBy]                        UNIQUEIDENTIFIER NULL,
    [Status]                          INT              NOT NULL,
    [IsActive]                        BIT              NOT NULL,
    CONSTRAINT [PK_EmployeeTypeSalaryPercentage] PRIMARY KEY CLUSTERED ([EmployeeGradeSalaryPercentageId] ASC),
    CONSTRAINT [FK_EmployeeGradeSalaryPercentage_EmployeeGrade] FOREIGN KEY ([EmployeeGradeId]) REFERENCES [dbo].[EmployeeGrade] ([Id])
);

