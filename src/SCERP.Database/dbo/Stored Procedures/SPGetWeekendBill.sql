-- =================================================
-- Author:		<Md. Yasir Arafat>
-- Create date: <2016-08-14>
-- Description: EXEC [SPGetWeekendBill] '2016-08-05'
-- =================================================

CREATE PROCEDURE [dbo].[SPGetWeekendBill]


					@FromDate DateTime = NULL

AS

BEGIN
				
		SET NOCOUNT ON;
			
				SELECT [WeekendBillId]
					  ,CONVERT(VARCHAR(15),[Date], 105) AS Date
					  ,[EmployeeId]
					  ,[EmployeeCardId]
					  ,[EmployeeName]
					  ,[SectionName]
					  ,[EmployeeType]
					  ,[Designation]
					  ,CONVERT(VARCHAR(5), [InTime], 108)  AS InTime
					  ,CONVERT(VARCHAR(5), [OutTime], 108) AS OutTime
					  ,[BasicSalary]
					  ,CEILING([Allowance]) AS [Allowance]
					  ,[CreatedDate]
					  ,[CreatedBy]
					  ,[EditedDate]
					  ,[EditedBy]
					  ,[IsActive]
			  FROM [dbo].[WeekendBill]
			  WHERE CAST([Date] AS DATE) = @FromDate
			  ORDER BY [EmployeeCardId] 	 
END

