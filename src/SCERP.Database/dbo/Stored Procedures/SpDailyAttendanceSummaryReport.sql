-- =========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <11/05/2019>
-- Description:	<> EXEC SpDailyAttendanceSummaryReport   1, 1, 1, NULL, NULL, NULL, NULL, '', '2019-05-09'
-- =========================================================================================================

CREATE PROCEDURE [dbo].[SpDailyAttendanceSummaryReport]
	
									
										 @CompanyId		            INT = NULL
										,@BranchId	      	        INT = NULL
										,@BranchUnitId		        INT = NULL
										,@BranchUnitDepartmentId    INT = NULL
										,@SectionId					INT = NULL
										,@LineId					INT = NULL
										,@EmployeeTypeId			INT = NULL
										,@employeeCardId			NVARCHAR(100) = NULL
										,@EffectiveDate				DATETIME 
															
AS
								 
BEGIN
	
			    SET NOCOUNT ON;

									DECLARE @PreviousDay DATETIME = DATEADD(DAY, -1, @EffectiveDate)
					
									SELECT [DepartmentName]
											,(SELECT SUM(1) FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @EffectiveDate AND EMP.[DepartmentName] = [EmployeeInOut].[DepartmentName] ) AS TotalEmployee
											,ISNULL((SELECT SUM(1) FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @EffectiveDate AND EMP.[DepartmentName] = [EmployeeInOut].[DepartmentName] AND Status IN( 'Present','Late')),0) AS TotalPresent
											,ISNULL((SELECT SUM(1) FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @EffectiveDate AND EMP.[DepartmentName] = [EmployeeInOut].[DepartmentName] AND Status= 'Late'),0) AS TotalLate
											,ISNULL((SELECT SUM(1) FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @EffectiveDate AND EMP.[DepartmentName] = [EmployeeInOut].[DepartmentName] AND Status= 'Leave'),0) AS TotalLeave
											,ISNULL((SELECT SUM(1) FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @EffectiveDate AND EMP.[DepartmentName] = [EmployeeInOut].[DepartmentName] AND Status= 'Absent'),0) AS TotalAbsent
											,(SELECT SUM(OTHours + ExtraOTHours) FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @PreviousDay AND EMP.[DepartmentName] = [EmployeeInOut].[DepartmentName] ) AS PreviousDayOTHours
											,CAST(@EffectiveDate AS DATE) AS Date
											,'Badon Fashion Ltd.' AS CompanyName
											,'Plot No # A-19,20,22,23, BSCIC I/E, Kanchpur, Sonargaon Narayangonj' AS CompanyAddress

										FROM [dbo].[EmployeeInOut]
										LEFT JOIN Employee employee ON employee.EmployeeId = [EmployeeInOut].EmployeeId										
										LEFT JOIN (SELECT EmployeeId, FromDate, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
										ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
										FROM EmployeeCompanyInfo AS employeeCompanyInfo 
										WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @EffectiveDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
										ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1						 											
										LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
										LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
										LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
										LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
										LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
										LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
										LEFT JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
										LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
										LEFT JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
										LEFT JOIN Company  AS company ON branch.CompanyId = company.Id
										LEFT JOIN DepartmentSection departmentSection ON employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
										LEFT JOIN Section section ON departmentSection.SectionId = section.SectionId
										LEFT JOIN DepartmentLine departmentLine ON employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
										LEFT JOIN Line line ON departmentLine.LineId = line.LineId																							
																			
								WHERE	(company.Id = @CompanyId OR @CompanyId IS NULL)
										AND (branch.Id = @BranchId OR @BranchId IS NULL)
										AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId IS NULL)
										AND ((BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId IS NULL))
										AND ((employeeCompanyInfo.DepartmentSectionId = @SectionId) OR (@SectionId IS NULL))
										AND ((employeeCompanyInfo.DepartmentLineId = @LineId) OR (@LineId IS NULL))
										AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId =''))
										AND ((EmployeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId IS NULL))															
										AND (employee.IsActive = 1)
										AND (employee.QuitDate IS NULL OR employee.QuitDate >= @EffectiveDate)
										AND employee.JoiningDate <= @EffectiveDate															
									    AND CAST([TransactionDate] AS DATE) = @EffectiveDate
									  
								GROUP BY [DepartmentName]
								ORDER BY [DepartmentName]
					 									 													  					  														  						  											  							
END