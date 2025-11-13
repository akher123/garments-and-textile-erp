
-- ==========================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2016-08-29>
-- Description:	<> EXEC [SPProcessBonusSummary] '2016-07-26','2016-08-25','superadmin'
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessBonusSummary]

								
								   @FromDate DATETIME = NULL
								  ,@ToDate	 DATETIME = TIME
								  ,@UserName NVARCHAR(100)
AS
BEGIN
			
				BEGIN TRAN
								SET XACT_ABORT ON;
								SET NOCOUNT ON;
		 										
								IF(@FromDate IS NULL)
								BEGIN
									SET @FromDate = CAST(CURRENT_TIMESTAMP AS DATE)
								END
								ELSE
								BEGIN
									SET @FromDate = CAST(@FromDate AS DATE)
								END									
						
						SET FMTONLY OFF;
														
						DECLARE @UserID UNIQUEIDENTIFIER;
						SELECT @UserID = EmployeeID FROM [User] WHERE UserName = @UserName;
	
						UPDATE [EmployeeSalarySummary]    -- Empty Table
						   SET [EmployeeSalarySummary].NetAmount = 0
							  ,[EmployeeSalarySummary].SumEmployee = 0
							  ,[EmployeeSalarySummary].ExtraOT = 0


						UPDATE [EmployeeSalarySummary]    -- Line

						SET    [EmployeeSalarySummary].NetAmount = SalaryLine.NetAmount
							  ,[EmployeeSalarySummary].SumEmployee = SalaryLine.SumOfEmployee
							  ,[EmployeeSalarySummary].ExtraOT = 0

						FROM   [EmployeeSalarySummary] INNER JOIN 
						(	   
							SELECT    LineId										 AS LineId
									 ,SUM(NetAmount)								 AS NetAmount
									 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
									 ,SUM(TotalExtraOTAmount) + SUM(TotalWeekendOTAmount) AS ExtraOT

									  FROM [dbo].[EmployeeProcessedSalary]
									  WHERE Year = 2016 AND Month = 7 AND EmployeeTypeId NOT IN(1,2,3)
									  AND EmployeeCardId NOT IN
									  (
										  SELECT EmployeeCardId FROM [EmployeeProcessedSalary] 
										  WHERE CAST(JoiningDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND QuitDate IS NULL
										  OR CAST(QuitDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND CAST(JoiningDate AS DATE) < '2016-06-26'
										  OR CAST(JoiningDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND CAST(QuitDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25'
									  )
									  GROUP BY LineId

						) AS SalaryLine ON [EmployeeSalarySummary].LineId = SalaryLine.LineId


--UPDATE [EmployeeSalarySummary]   -- Section

--SET    [EmployeeSalarySummary].NetAmount = SalarySection.NetAmount
--	  ,[EmployeeSalarySummary].SumEmployee = SalarySection.SumOfEmployee
--	  ,[EmployeeSalarySummary].ExtraOT = SalarySection.ExtraOT

--FROM   [EmployeeSalarySummary] INNER JOIN 
--(	   
--	SELECT    SectionId										 AS SectionId
--			 ,SUM(NetAmount)								 AS NetAmount
--			 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
--			 ,SUM(TotalExtraOTAmount) + SUM(TotalWeekendOTAmount) AS ExtraOT

--			  FROM [dbo].[EmployeeProcessedSalary]
--			  WHERE Year = 2016 AND Month = 7 AND EmployeeTypeId NOT IN(1,2,3)
--			  AND EmployeeCardId NOT IN
--			  (
--				  SELECT EmployeeCardId FROM [EmployeeProcessedSalary] 
--				  WHERE CAST(JoiningDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND QuitDate IS NULL
--				  OR CAST(QuitDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND CAST(JoiningDate AS DATE) < '2016-06-26'
--				  OR CAST(JoiningDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND CAST(QuitDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25'
--			  )

--			  GROUP BY SectionId

--) AS SalarySection ON CAST([EmployeeSalarySummary].SectionId AS INT) = SalarySection.SectionId


--UPDATE [EmployeeSalarySummary]  -- Department

--SET    [EmployeeSalarySummary].NetAmount = SalaryDepartment.NetAmount
--	  ,[EmployeeSalarySummary].SumEmployee = SalaryDepartment.SumOfEmployee
--	  ,[EmployeeSalarySummary].ExtraOT = SalaryDepartment.ExtraOT

--FROM   [EmployeeSalarySummary] INNER JOIN 
--(	   
--	SELECT    DepartmentId									 AS DepartmentId
--			 ,SUM(NetAmount)								 AS NetAmount
--			 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
--			 ,SUM(TotalExtraOTAmount) + SUM(TotalWeekendOTAmount) AS ExtraOT

--			  FROM [dbo].[EmployeeProcessedSalary]
--			  WHERE Year = 2016 AND Month = 7 AND EmployeeTypeId NOT IN(1,2,3)
--			  AND EmployeeCardId NOT IN
--			  (
--				  SELECT EmployeeCardId FROM [EmployeeProcessedSalary] 
--				  WHERE CAST(JoiningDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND QuitDate IS NULL
--				  OR CAST(QuitDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND CAST(JoiningDate AS DATE) < '2016-06-26'
--				  OR CAST(JoiningDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND CAST(QuitDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25'
--			  )
--			  GROUP BY DepartmentId

--) AS SalaryDepartment ON CAST([EmployeeSalarySummary].DepartmentId AS INT) = SalaryDepartment.DepartmentId


--UPDATE [EmployeeSalarySummary]  -- EmployeeType

--SET    [EmployeeSalarySummary].NetAmount = SalaryEmployeeType.NetAmount
--	  ,[EmployeeSalarySummary].SumEmployee = SalaryEmployeeType.SumOfEmployee
--	  ,[EmployeeSalarySummary].ExtraOT = SalaryEmployeeType.ExtraOT

--FROM   [EmployeeSalarySummary] INNER JOIN 
--(	   
--	SELECT    EmployeeTypeId							     AS EmployeeTypeId
--			 ,SUM(NetAmount)								 AS NetAmount
--			 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
--			 ,SUM(TotalExtraOTAmount) + SUM(TotalWeekendOTAmount) AS ExtraOT

--			  FROM [dbo].[EmployeeProcessedSalary]
--			  WHERE Year = 2016 AND Month = 7
--			  AND EmployeeCardId NOT IN
--			  (
--				  SELECT EmployeeCardId FROM [EmployeeProcessedSalary] 
--				  WHERE CAST(JoiningDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND QuitDate IS NULL
--				  OR CAST(QuitDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND CAST(JoiningDate AS DATE) < '2016-06-26'
--				  OR CAST(JoiningDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND CAST(QuitDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25'
--			  )
--			  GROUP BY EmployeeTypeId

--) AS SalaryEmployeeType ON CAST([EmployeeSalarySummary].EmployeeTypeId AS INT) = SalaryEmployeeType.EmployeeTypeId


--UPDATE [EmployeeSalarySummary]  -- New Join Employee

--SET    [EmployeeSalarySummary].NetAmount = SalaryEmployeeNewJoin.NetAmount
--	  ,[EmployeeSalarySummary].SumEmployee = SalaryEmployeeNewJoin.SumOfEmployee
--	  ,[EmployeeSalarySummary].ExtraOT = SalaryEmployeeNewJoin.ExtraOT

--FROM   [EmployeeSalarySummary] INNER JOIN 
--(	   
--	 SELECT	  14												  AS SLNO			
--			 ,SUM(NetAmount)								      AS NetAmount
--			 ,COUNT(EmployeeCardId)							      AS SumOfEmployee
--			 ,SUM(TotalExtraOTAmount) + SUM(TotalWeekendOTAmount) AS ExtraOT

--			  FROM [dbo].[EmployeeProcessedSalary]
--			  WHERE YEAR = 2016 AND MONTH = 7 AND CAST(JoiningDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND QuitDate IS NULL
--			  GROUP BY MONTH

--) AS SalaryEmployeeNewJoin ON CAST([EmployeeSalarySummary].SLNO AS INT) = SalaryEmployeeNewJoin.SLNO


--UPDATE [EmployeeSalarySummary]  -- Quit Employee

--SET    [EmployeeSalarySummary].NetAmount = SalaryEmployeeQuit.NetAmount
--	  ,[EmployeeSalarySummary].SumEmployee = SalaryEmployeeQuit.SumOfEmployee
--	  ,[EmployeeSalarySummary].ExtraOT = SalaryEmployeeQuit.ExtraOT

--FROM   [EmployeeSalarySummary] INNER JOIN 
--(	   
--	  SELECT	
--			  15												  AS SLNO	
--			 ,SUM(NetAmount)									  AS NetAmount
--			 ,COUNT(EmployeeCardId)								  AS SumOfEmployee
--			 ,SUM(TotalExtraOTAmount) + SUM(TotalWeekendOTAmount) AS ExtraOT

--			  FROM [dbo].[EmployeeProcessedSalary]
--			  WHERE YEAR = 2016 AND MONTH = 7 AND CAST(QuitDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND CAST(JoiningDate AS DATE) < '2016-06-26'
--			  GROUP BY MONTH

--) AS SalaryEmployeeQuit ON CAST([EmployeeSalarySummary].SLNO AS INT) = SalaryEmployeeQuit.SLNO


--UPDATE [EmployeeSalarySummary]  -- New Join and Quit Employee

--SET    [EmployeeSalarySummary].NetAmount = SalaryEmployeeQuit.NetAmount
--	  ,[EmployeeSalarySummary].SumEmployee = SalaryEmployeeQuit.SumOfEmployee
--	  ,[EmployeeSalarySummary].ExtraOT = SalaryEmployeeQuit.ExtraOT

--FROM   [EmployeeSalarySummary] INNER JOIN 
--(	   
--	  SELECT	
--			  16												  AS SLNO	
--			 ,SUM(NetAmount)									  AS NetAmount
--			 ,COUNT(EmployeeCardId)								  AS SumOfEmployee
--			 ,SUM(TotalExtraOTAmount) + SUM(TotalWeekendOTAmount) AS ExtraOT

--			  FROM [dbo].[EmployeeProcessedSalary]
--			  WHERE YEAR = 2016 AND MONTH = 7 AND CAST(JoiningDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25' AND CAST(QuitDate AS DATE) BETWEEN '2016-06-26' AND '2016-07-25'
--			  GROUP BY MONTH

--) AS SalaryEmployeeQuit ON CAST([EmployeeSalarySummary].SLNO AS INT) = SalaryEmployeeQuit.SLNO


--SELECT [Id]
--      ,[SLNO]
--      ,[EmployeeTypeId]
--      ,[EmployeeType]
--      ,[DepartmentId]
--      ,[Department]
--      ,[SectionId]
--      ,[Section]
--      ,[LineId]
--      ,[Line]
--      ,[NetAmount]
--      ,[SumEmployee]
--      ,[ExtraOT]
--  FROM [dbo].[EmployeeSalarySummary]
--  ORDER BY [SLNO], [Id]

--SELECT       
--      COUNT(EmployeeCardId)		AS SumOfEmployee        
--	  FROM [dbo].[EmployeeProcessedSalary]
--	  WHERE Year = 2016 AND Month = 7
	
--  SELECT SUM(SumEmployee)	  AS SumOfEmployee        
--	  FROM [dbo].employeeSalarySummary	
									



				COMMIT TRAN
														
END






