-- ====================================================================================================================
-- Author	   :  Yasir
-- Create date :  2017-11-05
-- Description :  EXEC [SPGetSalarySummaryTopSheet] 1, 1, 2020, 05, '2020-04-26', '2020-05-25'
-- ====================================================================================================================

CREATE PROCEDURE [dbo].[SPGetSalarySummaryTopSheet] 


									 @CompanyId			INT
									,@BranchId			INT	
									,@Year				INT				
									,@Month				INT			
									,@FromDate			DATETIME
									,@ToDate			DATETIME					


AS
BEGIN

	SET NOCOUNT ON;

							
									UPDATE [EmployeeSalarySummary]    -- Empty Table
									   SET [EmployeeSalarySummary].NetAmount = 0
										  ,[EmployeeSalarySummary].SumEmployee = 0
										  ,[EmployeeSalarySummary].ExtraOT = 0
										  ,[EmployeeSalarySummary].RegularOT = 0
										  ,[EmployeeSalarySummary].ExtraOTHours = 0
										  ,[EmployeeSalarySummary].RegularOTHours = 0


									UPDATE [EmployeeSalarySummary]    -- Line

									SET    [EmployeeSalarySummary].NetAmount = SalaryLine.NetAmount
										  ,[EmployeeSalarySummary].SumEmployee = SalaryLine.SumOfEmployee
										  ,[EmployeeSalarySummary].ExtraOT = SalaryLine.ExtraOT
										  ,[EmployeeSalarySummary].RegularOT= SalaryLine.RegularOT
										  ,[EmployeeSalarySummary].ExtraOTHours = SalaryLine.ExtraOTHours
										  ,[EmployeeSalarySummary].RegularOTHours = SalaryLine.RegularOTHours

									FROM   [EmployeeSalarySummary] INNER JOIN 
									(	   
										SELECT    LineId										 AS LineId
												 ,ISNULL(SUM(NetAmount), 0)						 AS NetAmount
												 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
												 ,ISNULL(SUM(TotalExtraOTAmount), 0) + ISNULL(SUM(TotalWeekendOTAmount), 0) + ISNULL(SUM(TotalHolidayOTAmount), 0) AS ExtraOT
												 ,ISNULL(SUM(TotalOTAmount), 0) AS RegularOT
												 ,ISNULL(SUM(TotalExtraOTHours), 0) + ISNULL(SUM(TotalWeekendOTHours), 0) + ISNULL(SUM(TotalHolidayOTHours), 0) AS ExtraOTHours
												 ,ISNULL(SUM(OTHours), 0) AS RegularOTHours

												  FROM [dbo].[EmployeeProcessedSalary]
												  WHERE Year = @Year AND Month = @Month AND FromDate = @FromDate AND EmployeeTypeId NOT IN(1,2,3)
												  AND EmployeeCardId NOT IN
												  (
													  SELECT EmployeeCardId FROM [EmployeeProcessedSalary] 
													  WHERE CAST(JoiningDate AS DATE) BETWEEN @FromDate AND @ToDate AND QuitDate IS NULL
													  OR CAST(QuitDate AS DATE) BETWEEN @FromDate AND @ToDate AND CAST(JoiningDate AS DATE) < @FromDate
													  OR CAST(JoiningDate AS DATE) BETWEEN @FromDate AND @ToDate AND CAST(QuitDate AS DATE) BETWEEN @FromDate AND @ToDate
												  )
												  GROUP BY LineId

									) AS SalaryLine ON [EmployeeSalarySummary].LineId = SalaryLine.LineId


									UPDATE [EmployeeSalarySummary]   -- Section

									SET    [EmployeeSalarySummary].NetAmount = SalarySection.NetAmount
										  ,[EmployeeSalarySummary].SumEmployee = SalarySection.SumOfEmployee
										  ,[EmployeeSalarySummary].ExtraOT = SalarySection.ExtraOT
										  ,[EmployeeSalarySummary].RegularOT= SalarySection.RegularOT
										  ,[EmployeeSalarySummary].ExtraOTHours = SalarySection.ExtraOTHours
										  ,[EmployeeSalarySummary].RegularOTHours = SalarySection.RegularOTHours

									FROM   [EmployeeSalarySummary] INNER JOIN 
									(	   
										SELECT    SectionId										 AS SectionId
												 ,ISNULL(SUM(NetAmount), 0)						 AS NetAmount
												 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
												 ,ISNULL(SUM(TotalExtraOTAmount), 0) + ISNULL(SUM(TotalWeekendOTAmount), 0) + ISNULL(SUM(TotalHolidayOTAmount), 0) AS ExtraOT
												 ,ISNULL(SUM(TotalOTAmount), 0) AS RegularOT
												 ,ISNULL(SUM(TotalExtraOTHours), 0) + ISNULL(SUM(TotalWeekendOTHours), 0) + ISNULL(SUM(TotalHolidayOTHours), 0) AS ExtraOTHours
												 ,ISNULL(SUM(OTHours), 0) AS RegularOTHours

												  FROM [dbo].[EmployeeProcessedSalary]
												  WHERE Year = @Year AND Month = @Month AND FromDate = @FromDate AND EmployeeTypeId NOT IN(1,2,3)
												  AND EmployeeCardId NOT IN
												  (
													  SELECT EmployeeCardId FROM [EmployeeProcessedSalary] 
													  WHERE CAST(JoiningDate AS DATE) BETWEEN @FromDate AND @ToDate AND QuitDate IS NULL
													  OR CAST(QuitDate AS DATE) BETWEEN @FromDate AND @ToDate AND CAST(JoiningDate AS DATE) < @FromDate
													  OR CAST(JoiningDate AS DATE) BETWEEN @FromDate AND @ToDate AND CAST(QuitDate AS DATE) BETWEEN @FromDate AND @ToDate
												  )

												  GROUP BY SectionId

									) AS SalarySection ON CAST([EmployeeSalarySummary].SectionId AS INT) = SalarySection.SectionId


									UPDATE [EmployeeSalarySummary]  -- Department

									SET    [EmployeeSalarySummary].NetAmount = SalaryDepartment.NetAmount
										  ,[EmployeeSalarySummary].SumEmployee = SalaryDepartment.SumOfEmployee
										  ,[EmployeeSalarySummary].ExtraOT = SalaryDepartment.ExtraOT
										  ,[EmployeeSalarySummary].RegularOT= SalaryDepartment.RegularOT
										  ,[EmployeeSalarySummary].ExtraOTHours = SalaryDepartment.ExtraOTHours
										  ,[EmployeeSalarySummary].RegularOTHours = SalaryDepartment.RegularOTHours

									FROM   [EmployeeSalarySummary] INNER JOIN 
									(	   
										SELECT    DepartmentId									 AS DepartmentId
												 ,ISNULL(SUM(NetAmount), 0)						 AS NetAmount
												 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
												 ,ISNULL(SUM(TotalExtraOTAmount), 0) + ISNULL(SUM(TotalWeekendOTAmount), 0) + ISNULL(SUM(TotalHolidayOTAmount), 0) AS ExtraOT
												 ,ISNULL(SUM(TotalOTAmount), 0) AS RegularOT
												 ,ISNULL(SUM(TotalExtraOTHours), 0) + ISNULL(SUM(TotalWeekendOTHours), 0) + ISNULL(SUM(TotalHolidayOTHours), 0) AS ExtraOTHours
												 ,ISNULL(SUM(OTHours), 0) AS RegularOTHours

												  FROM [dbo].[EmployeeProcessedSalary]
												  WHERE Year = @Year AND Month = @Month AND FromDate = @FromDate AND EmployeeTypeId NOT IN(1,2,3)
												  AND EmployeeCardId NOT IN
												  (
													  SELECT EmployeeCardId FROM [EmployeeProcessedSalary] 
													  WHERE CAST(JoiningDate AS DATE) BETWEEN @FromDate AND @ToDate AND QuitDate IS NULL
													  OR CAST(QuitDate AS DATE) BETWEEN @FromDate AND @ToDate AND CAST(JoiningDate AS DATE) < @FromDate
													  OR CAST(JoiningDate AS DATE) BETWEEN @FromDate AND @ToDate AND CAST(QuitDate AS DATE) BETWEEN @FromDate AND @ToDate
												  )
												  GROUP BY DepartmentId

									) AS SalaryDepartment ON CAST([EmployeeSalarySummary].DepartmentId AS INT) = SalaryDepartment.DepartmentId


									UPDATE [EmployeeSalarySummary]  -- EmployeeType

									SET    [EmployeeSalarySummary].NetAmount = SalaryEmployeeType.NetAmount
										  ,[EmployeeSalarySummary].SumEmployee = SalaryEmployeeType.SumOfEmployee
										  ,[EmployeeSalarySummary].ExtraOT = SalaryEmployeeType.ExtraOT
										  ,[EmployeeSalarySummary].RegularOT= SalaryEmployeeType.RegularOT
										  ,[EmployeeSalarySummary].ExtraOTHours = SalaryEmployeeType.ExtraOTHours
										  ,[EmployeeSalarySummary].RegularOTHours = SalaryEmployeeType.RegularOTHours

									FROM   [EmployeeSalarySummary] INNER JOIN 
									(	   
										SELECT    EmployeeTypeId							     AS EmployeeTypeId
												 ,ISNULL(SUM(NetAmount), 0)						 AS NetAmount
												 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
												 ,ISNULL(SUM(TotalExtraOTAmount), 0) + ISNULL(SUM(TotalWeekendOTAmount), 0) + ISNULL(SUM(TotalHolidayOTAmount), 0) AS ExtraOT
												 ,ISNULL(SUM(TotalOTAmount), 0) AS RegularOT
												 ,ISNULL(SUM(TotalExtraOTHours), 0) + ISNULL(SUM(TotalWeekendOTHours), 0) + ISNULL(SUM(TotalHolidayOTHours), 0) AS ExtraOTHours
												 ,ISNULL(SUM(OTHours), 0) AS RegularOTHours

												  FROM [dbo].[EmployeeProcessedSalary]
												  WHERE Year = @Year AND Month = @Month AND FromDate = @FromDate
												  AND EmployeeCardId NOT IN
												  (
													  SELECT EmployeeCardId FROM [EmployeeProcessedSalary] 
													  WHERE CAST(JoiningDate AS DATE) BETWEEN @FromDate AND @ToDate AND QuitDate IS NULL
													  OR CAST(QuitDate AS DATE) BETWEEN @FromDate AND @ToDate AND CAST(JoiningDate AS DATE) < @FromDate
													  OR CAST(JoiningDate AS DATE) BETWEEN @FromDate AND @ToDate AND CAST(QuitDate AS DATE) BETWEEN @FromDate AND @ToDate
												  )
												  GROUP BY EmployeeTypeId

									) AS SalaryEmployeeType ON CAST([EmployeeSalarySummary].EmployeeTypeId AS INT) = SalaryEmployeeType.EmployeeTypeId


									UPDATE [EmployeeSalarySummary]  -- New Join Employee

									SET    [EmployeeSalarySummary].NetAmount = SalaryEmployeeNewJoin.NetAmount
										  ,[EmployeeSalarySummary].SumEmployee = SalaryEmployeeNewJoin.SumOfEmployee
										  ,[EmployeeSalarySummary].ExtraOT = SalaryEmployeeNewJoin.ExtraOT
										  ,[EmployeeSalarySummary].RegularOT= SalaryEmployeeNewJoin.RegularOT
										  ,[EmployeeSalarySummary].ExtraOTHours = SalaryEmployeeNewJoin.ExtraOTHours
										  ,[EmployeeSalarySummary].RegularOTHours = SalaryEmployeeNewJoin.RegularOTHours

									FROM   [EmployeeSalarySummary] INNER JOIN 
									(	   
										 SELECT	  14										     AS SLNO			
												 ,ISNULL(SUM(NetAmount), 0)						 AS NetAmount
												 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
												 ,ISNULL(SUM(TotalExtraOTAmount), 0) + ISNULL(SUM(TotalWeekendOTAmount), 0) + ISNULL(SUM(TotalHolidayOTAmount), 0) AS ExtraOT
												 ,ISNULL(SUM(TotalOTAmount), 0) AS RegularOT
												 ,ISNULL(SUM(TotalExtraOTHours), 0) + ISNULL(SUM(TotalWeekendOTHours), 0) + ISNULL(SUM(TotalHolidayOTHours), 0) AS ExtraOTHours
												 ,ISNULL(SUM(OTHours), 0) AS RegularOTHours

												  FROM [dbo].[EmployeeProcessedSalary]
												  WHERE YEAR = @Year AND MONTH = @Month AND FromDate = @FromDate AND CAST(JoiningDate AS DATE) BETWEEN @FromDate AND @ToDate AND QuitDate IS NULL
												  GROUP BY MONTH

									) AS SalaryEmployeeNewJoin ON CAST([EmployeeSalarySummary].SLNO AS INT) = SalaryEmployeeNewJoin.SLNO


									UPDATE [EmployeeSalarySummary]  -- Quit Employee

									SET    [EmployeeSalarySummary].NetAmount = SalaryEmployeeQuit.NetAmount
										  ,[EmployeeSalarySummary].SumEmployee = SalaryEmployeeQuit.SumOfEmployee
										  ,[EmployeeSalarySummary].ExtraOT = SalaryEmployeeQuit.ExtraOT
										  ,[EmployeeSalarySummary].RegularOT= SalaryEmployeeQuit.RegularOT
										  ,[EmployeeSalarySummary].ExtraOTHours = SalaryEmployeeQuit.ExtraOTHours
										  ,[EmployeeSalarySummary].RegularOTHours = SalaryEmployeeQuit.RegularOTHours

									FROM   [EmployeeSalarySummary] INNER JOIN 
									(	   
										  SELECT	
												  15											 AS SLNO	
												  ,ISNULL(SUM(NetAmount), 0)					 AS NetAmount
												 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
												 ,ISNULL(SUM(TotalExtraOTAmount), 0) + ISNULL(SUM(TotalWeekendOTAmount), 0) + ISNULL(SUM(TotalHolidayOTAmount), 0) AS ExtraOT
												 ,ISNULL(SUM(TotalOTAmount), 0) AS RegularOT
												 ,ISNULL(SUM(TotalExtraOTHours), 0) + ISNULL(SUM(TotalWeekendOTHours), 0) + ISNULL(SUM(TotalHolidayOTHours), 0) AS ExtraOTHours
												 ,ISNULL(SUM(OTHours), 0) AS RegularOTHours

												  FROM [dbo].[EmployeeProcessedSalary]
												  WHERE YEAR = @Year AND MONTH = @Month AND FromDate = @FromDate AND CAST(QuitDate AS DATE) BETWEEN @FromDate AND @ToDate AND CAST(JoiningDate AS DATE) < @FromDate
												  GROUP BY MONTH

									) AS SalaryEmployeeQuit ON CAST([EmployeeSalarySummary].SLNO AS INT) = SalaryEmployeeQuit.SLNO


									UPDATE [EmployeeSalarySummary]  -- New Join and Quit Employee

									SET    [EmployeeSalarySummary].NetAmount = SalaryEmployeeQuit.NetAmount
										  ,[EmployeeSalarySummary].SumEmployee = SalaryEmployeeQuit.SumOfEmployee
										  ,[EmployeeSalarySummary].ExtraOT = SalaryEmployeeQuit.ExtraOT
										  ,[EmployeeSalarySummary].RegularOT= SalaryEmployeeQuit.RegularOT
										  ,[EmployeeSalarySummary].ExtraOTHours = SalaryEmployeeQuit.ExtraOTHours
										  ,[EmployeeSalarySummary].RegularOTHours = SalaryEmployeeQuit.RegularOTHours

									FROM   [EmployeeSalarySummary] INNER JOIN 
									(	   
										  SELECT	
												  16											 AS SLNO	
												 ,ISNULL(SUM(NetAmount), 0)						 AS NetAmount
												 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
												 ,ISNULL(SUM(TotalExtraOTAmount), 0) + ISNULL(SUM(TotalWeekendOTAmount), 0) + ISNULL(SUM(TotalHolidayOTAmount), 0) AS ExtraOT
												 ,ISNULL(SUM(TotalOTAmount), 0) AS RegularOT
												 ,ISNULL(SUM(TotalExtraOTHours), 0) + ISNULL(SUM(TotalWeekendOTHours), 0) + ISNULL(SUM(TotalHolidayOTHours), 0) AS ExtraOTHours
												 ,ISNULL(SUM(OTHours), 0) AS RegularOTHours

												  FROM [dbo].[EmployeeProcessedSalary]
												  WHERE YEAR = @Year AND MONTH = @Month AND FromDate = @FromDate AND CAST(JoiningDate AS DATE) BETWEEN @FromDate AND @ToDate AND CAST(QuitDate AS DATE) BETWEEN @FromDate AND @ToDate
												  GROUP BY MONTH

									) AS SalaryEmployeeQuit ON CAST([EmployeeSalarySummary].SLNO AS INT) = SalaryEmployeeQuit.SLNO


									SELECT [Id]
										  ,[SLNO]
										  ,[EmployeeTypeId]
										  ,[EmployeeType]
										  ,[DepartmentId]
										  ,[Department]
										  ,[SectionId]
										  ,[Section]
										  ,[LineId]
										  ,[Line]
										  ,[SumEmployee]
										  ,[NetAmount]  
										  ,[RegularOT]    
										  ,[ExtraOT]
										  ,ExtraOTHours
										  ,RegularOTHours										    
									  FROM [dbo].[EmployeeSalarySummary]
									  ORDER BY [SLNO],[Id]
      
									--SELECT COUNT(1) AS SumOfEmployee        
									--	  FROM [dbo].[EmployeeProcessedSalary]
									--	  WHERE Year = @Year AND Month = @Month AND FromDate = @FromDate
	
									--SELECT SUM(SumEmployee) AS SumOfEmployee        
									--	  FROM [dbo].employeeSalarySummary

									  			  														  													
END