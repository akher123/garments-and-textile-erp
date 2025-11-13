
-- =====================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <29/11/2017>
-- Description:	<> EXEC SPMISInsertSalarySummaryDaily '2017-12-20'
-- ====================================================================

CREATE PROCEDURE [dbo].[SPMISInsertSalarySummaryDaily]
				

						   @Date  DATE  
						  																				 
AS

BEGIN
	
	SET NOCOUNT ON;

					   DELETE FROM MIS_SalarySummaryDaily
					   WHERE TransactionDate = @Date 


					   INSERT INTO MIS_SalarySummaryDaily
					   (    TransactionDate
						   ,[DepartmentId]
						   ,[DepartmentName]
						   ,[NetAmount]
						   ,[ExtraOTAmount]
						   ,[WeekendOTAmount]
						   ,[HolidayOTAmount]
						   ,[Percentage]
						   ,[GroupCode]
					    )	
											   						
					   SELECT 
					   @Date
					  ,BranchUnitDepartmentId
					  ,DepartmentName
					  ,SUM(CAST(employeeSalary.GrossSalary/26 AS decimal(18,2))) + SUM(EmployeeInOut.OTHours * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@Date, @Date))) AS DECIMAL(18,2))) AS NetAmount
					  ,SUM(EmployeeInOut.ExtraOTHours * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@Date, @Date))) AS DECIMAL(18,2))) AS ExtraOTAmount
					  ,SUM(EmployeeInOut.WeekendOTHours * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@Date, @Date))) AS DECIMAL(18,2))) AS WeekendOTAmount
					  ,SUM(EmployeeInOut.HolidayOTHours * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@Date, @Date))) AS DECIMAL(18,2))) AS HolidayOTAmount
					  ,(SELECT Percentage FROM MIS_DepartmentPercent WHERE MIS_DepartmentPercent.DepartmentId = EmployeeInOut.BranchUnitDepartmentId) 
					  ,'D'

					   FROM [dbo].[EmployeeInOut]
						  LEFT JOIN (SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,
						  employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,
						  employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, 
						  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
						  FROM EmployeeSalary AS employeeSalary 
						  WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @Date) AND EmployeeSalary.IsActive = 1) employeeSalary 
						  ON [EmployeeInOut].EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1 

					  WHERE CAST(EmployeeInOut.TransactionDate AS DATE) = @Date
					  AND EmployeeInOut.Status <> 'Absent'
					  AND EmployeeInOut.BranchUnitDepartmentId NOT IN(1, 5, 8, 11, 13, 14)
					  GROUP BY EmployeeInOut.BranchUnitDepartmentId, EmployeeInOut.DepartmentName
					 	
						
										  		
														 	
					   INSERT INTO MIS_SalarySummaryDaily
					   (    TransactionDate
						   ,[DepartmentId]
						   ,[DepartmentName]
						   ,[NetAmount]
						   ,[ExtraOTAmount]
						   ,[WeekendOTAmount]
						   ,[HolidayOTAmount]
						   ,[Percentage]
						   ,[GroupCode]
					    )	
											   						
					   SELECT 
					   @Date
					  ,BranchUnitDepartmentId
					  ,DepartmentName
					  ,SUM(CAST(employeeSalary.GrossSalary/26 AS decimal(18,2))) + SUM(EmployeeInOut.OTHours * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@Date, @Date))) AS DECIMAL(18,2))) AS NetAmount
					  ,SUM(EmployeeInOut.ExtraOTHours * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@Date, @Date))) AS DECIMAL(18,2))) AS ExtraOTAmount
					  ,SUM(EmployeeInOut.WeekendOTHours * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@Date, @Date))) AS DECIMAL(18,2))) AS WeekendOTAmount
					  ,SUM(EmployeeInOut.HolidayOTHours * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@Date, @Date))) AS DECIMAL(18,2))) AS HolidayOTAmount
					  ,(SELECT Percentage FROM MIS_DepartmentPercent WHERE MIS_DepartmentPercent.DepartmentId = EmployeeInOut.BranchUnitDepartmentId) 
					  ,'D'

					   FROM [dbo].[EmployeeInOut]
						  LEFT JOIN (SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,
						  employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,
						  employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, 
						  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
						  FROM EmployeeSalary AS employeeSalary 
						  WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @Date) AND EmployeeSalary.IsActive = 1) employeeSalary 
						  ON [EmployeeInOut].EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1 

					  WHERE CAST(EmployeeInOut.TransactionDate AS DATE) = @Date
					  AND EmployeeInOut.Status <> 'Absent'
					  AND EmployeeInOut.BranchUnitDepartmentId IN(1, 5, 8, 11, 13)
					  GROUP BY EmployeeInOut.BranchUnitDepartmentId, EmployeeInOut.DepartmentName
					

					  	
					  INSERT INTO MIS_SalarySummaryDaily
					   (
							TransactionDate
						   ,[DepartmentId]
						   ,[DepartmentName]
						   ,[NetAmount]
						   ,[ExtraOTAmount]
						   ,[WeekendOTAmount]
						   ,[HolidayOTAmount]
						   ,[Percentage]
						   ,[GroupCode]
					   ) 						   						
					   SELECT 
					   @Date
					  ,98
					  ,'Security'
					  ,0 AS NetAmount
					  ,0 AS ExtraOTAmount
					  ,0 AS WeekendOTAmount
					  ,0 AS HolidayOTAmount
					  ,100
					  ,'I'


					  UPDATE MIS_SalarySummaryDaily				-- Perday OverHead Add
					  SET [NetAmount] = CAST(16000 AS DECIMAL(18,2))
					  WHERE DepartmentId = 98

									  						 												
					  INSERT INTO MIS_SalarySummaryDaily
					   (
							TransactionDate
						   ,[DepartmentId]
						   ,[DepartmentName]
						   ,[NetAmount]
						   ,[ExtraOTAmount]
						   ,[WeekendOTAmount]
						   ,[HolidayOTAmount]
						   ,[Percentage]
						   ,[GroupCode]
					   ) 						   						
					   SELECT 
					   @Date
					  ,99
					  ,'Total Other Overhead'
					  ,0 AS NetAmount
					  ,0 AS ExtraOTAmount
					  ,0 AS WeekendOTAmount
					  ,0 AS HolidayOTAmount
					  ,100
					  ,'I'

					  					
					  UPDATE MIS_SalarySummaryDaily				-- Perday OverHead Add
					  SET [NetAmount] = CAST(948480 AS DECIMAL(18,2))
					  WHERE DepartmentId = 99

			
																-- Production Manager Salary shift
					  DECLARE @Amount DECIMAL(18,2) 

					  SELECT @Amount = NetAmount 
					  FROM MIS_SalarySummaryDaily 
					  WHERE DepartmentId = 16 
					  AND TransactionDate = @Date

					  UPDATE MIS_SalarySummaryDaily
					  SET NetAmount = NetAmount + ISNULL(@Amount, 0)
					  WHERE DepartmentId = 6
					  
					  DELETE MIS_SalarySummaryDaily WHERE DepartmentId = 16 AND TransactionDate = @Date	

					  DELETE FROM MIS_SalarySummaryDaily WHERE [Percentage] IS NULL
					  
					  

					  DECLARE @Result INT = 1;

					  IF (@@ERROR <> 0)
						  SET @Result = 0;
		
					  SELECT @Result;
					  					 					  																																																
END