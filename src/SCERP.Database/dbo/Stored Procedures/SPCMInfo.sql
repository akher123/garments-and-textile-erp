-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/11/2016>
-- Description:	<> EXEC SPCMInfo  '2016-11-01', '2016-11-05', ''
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCMInfo]
											
						   @FromDate		DATETIME
						  ,@ToDate			DATETIME
						  ,@UserName		NVARCHAR(100)

AS
BEGIN
	
			SET NOCOUNT ON;

					DECLARE    		
						@Days INT
					   ,@Count INT
					   ,@StartDate DATE
					   ,@TotalWorkingHours DECIMAL(18,2) = 0
					   ,@TotalWorkerSalary DECIMAL(18,2) = 0
					   ,@TotalStaffSalary  DECIMAL(18,2) = 0

					   ,@PerDayTotalSalary DECIMAL(18,2) = 0

DECLARE     @WorkingMinutes TABLE( 
								 TransactionDate Date
								,LineId INT 
								,LineName NVARCHAR(100) 
								,NoOfEmployee INT
								,WorkingHours DECIMAL(10,2)
								,OTHours DECIMAL(10,2) 	
								,ExtraOTHours DECIMAL(10,2)		
								,WeekEndOTHours DECIMAL(10,2)	
								,HolidayOTHours DECIMAL(10,2)	
								,TotalWorkingHours DECIMAL(10,2)	
								,AverageWorkingHours DECIMAL(10,2)
								,PerDayTotalSalary DECIMAL(10,2)	
								,TotalOTAmount DECIMAL(10,2)
								,CostOfManufacturing DECIMAL(10,2)				
							   )


							SELECT @PerDayTotalSalary = SUM(employeeSalary.GrossSalary)
							FROM Employee 
							LEFT JOIN 
							(SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,
													  employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,
													  employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, 
													  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
													  FROM EmployeeSalary AS employeeSalary 
													  WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @toDate) AND EmployeeSalary.IsActive = 1) employeeSalary 
													  ON Employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1 

							LEFT JOIN (SELECT EmployeeId, FromDate, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
									ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
									FROM EmployeeCompanyInfo AS employeeCompanyInfo 
									WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
									ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

									LEFT JOIN BranchUnitDepartment AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
									LEFT JOIN BranchUnit AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId

									WHERE Employee.IsActive = 1
									AND employee.QuitDate IS NULL
									AND BranchUnit.BranchUnitId = 1



  							SET @Days = DATEDIFF(DAY, @FromDate, @ToDate) + 1
							SET @StartDate = @FromDate
							WHILE @Days > 0
							BEGIN							 

							INSERT INTO @WorkingMinutes

							SELECT  @StartDate
								   ,[DepartmentLineId] 
								   ,[LineName]
								   ,COUNT(EmployeeInOut.EmployeeId)
								   ,COUNT(EmployeeInOut.EmployeeId) * 8
								   ,SUM(OTHours)
								   ,SUM(ExtraOTHours)
								   ,SUM(WeekendOTHours)
								   ,SUM(HolidayOTHours)
								   ,COUNT(EmployeeInOut.EmployeeId) * 8 + SUM(OTHours) + SUM(ExtraOTHours) + SUM(WeekendOTHours) + SUM(HolidayOTHours)
								   ,(COUNT(EmployeeInOut.EmployeeId) * 8 + SUM(OTHours) + SUM(ExtraOTHours) + SUM(WeekendOTHours) + SUM(HolidayOTHours))/COUNT(EmployeeInOut.EmployeeId)
								   ,@PerDayTotalSalary/26
								   ,SUM(OTHours * employeeSalary.BasicSalary/104) + SUM(ExtraOTHours * employeeSalary.BasicSalary/104) + SUM(WeekendOTHours * employeeSalary.BasicSalary/104) + SUM(HolidayOTHours * employeeSalary.BasicSalary/104)
				   				   ,((@PerDayTotalSalary/26) + SUM(OTHours * employeeSalary.BasicSalary/104) + SUM(ExtraOTHours * employeeSalary.BasicSalary/104) + SUM(WeekendOTHours * employeeSalary.BasicSalary/104) + SUM(HolidayOTHours * employeeSalary.BasicSalary/104)) / (COUNT(EmployeeInOut.EmployeeId) * 8 + SUM(OTHours) + SUM(ExtraOTHours) + SUM(WeekendOTHours) + SUM(HolidayOTHours)) / 60
								   				   
									FROM EmployeeInOut
									LEFT JOIN
									(SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,
													  employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,
													  employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, 
													  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
													  FROM EmployeeSalary AS employeeSalary 
													  WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @toDate) AND EmployeeSalary.IsActive = 1) employeeSalary 
													  ON EmployeeInOut.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1 

									WHERE CAST(EmployeeInOut.TransactionDate AS DATE) = CAST(@StartDate AS DATE) 
									AND	Status IN('Present', 'Late')
									AND [DepartmentLineId] IS NOT NULL
									AND EmployeeDesignation LIKE '%Operator%' 
									AND EmployeeDesignation NOT LIKE '%Assistant%'
									AND DepartmentLineId NOT IN (1, 23)
									GROUP BY [DepartmentLineId],[LineName]								  							
						
									SET @StartDate = DATEADD (day, 1, @StartDate)
									SET @Days = @Days - 1;	
								END	


								DELETE FROM CMInfo			-- delete duplicate data
								WHERE Date IN (
										SELECT 
										TransactionDate								
										FROM @WorkingMinutes
								)


								INSERT INTO [CMInfo]
										   (	
												[Date]
											   ,[LineId]
											   ,[LineName]
											   ,[NoOfEmployee]
											   ,[TotalWorkingHours]
											   ,[Salary]
											   ,[CostOfMinute]											 
											)
								SELECT 
										TransactionDate
									   ,LineId
									   ,LineName
									   ,NoOfEmployee	
									   ,TotalWorkingHours							
									   ,TotalOTAmount
									   ,CostOfManufacturing
								FROM @WorkingMinutes	
												  														  					  														  						  											  							
END