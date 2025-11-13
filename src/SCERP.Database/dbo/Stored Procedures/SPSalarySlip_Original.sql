-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <15/03/2015>
-- Description:	<> exec SPSalarySlip '06/01/2015','06/30/2015'
-- =============================================
CREATE PROCEDURE [dbo].[SPSalarySlip_Original]

						    @employeeCardId		NVARCHAR(100)
						   ,@departmentId		INT 
						   ,@sectionId			INT
						   ,@lineId				INT
						   ,@fromDate			DateTime
						   ,@toDate				DateTime
						   ,@employeeTypeId		INT 	

AS
BEGIN
	
	SET NOCOUNT ON;
			
			IF(@employeeCardId = '')
			   SET @employeeCardId = NULL;

			IF(@sectionId = 0)
			   SET @sectionId = NULL;

			IF(@lineId = 0)
			   SET @lineId = NULL;

			Declare @StartDate Datetime = CURRENT_TIMESTAMP,
					@EmployeeId uniqueidentifier,
					@Stamp DECIMAL(18,2) = 0.00,
					@Advance DECIMAL(18,2) = 0.00,
					@test DECIMAL(18,2) = 10

			SELECT @EmployeeId = Employee.EmployeeId From Employee WHERE Employee.EmployeeCardId = @employeeCardId AND Employee.IsActive = 1

			SELECT TOP(1) @Stamp = StampAmount.Amount FROM StampAmount 
						  WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @fromDate
						  ORDER BY StampAmount.FromDate DESC 

			SELECT		  Employee.EmployeeId					AS EmployeeId
						 ,Employee.EmployeeCardId				AS EmployeeCardId
						 ,Company.NameInBengali					AS CompanyName
						 ,Employee.NameInBengali				AS Name
						 ,EmployeeDesignation.TitleInBengali	AS Designation
						 ,EmployeeGrade.NameInBengali			AS Grade
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) AS JoiningDate
						 ,Unit.NameInBengali					AS Unit
						 ,Department.NameInBengali				AS Department
						 ,Section.NameInBengali					AS Section

						 ,CONVERT(NVARCHAR(20), DATEDIFF(DAY, @FromDate, @ToDate) + 1) AS TotalDays		
						 ,CONVERT(NVARCHAR(20), (DATEDIFF(DAY, @FromDate, @ToDate) + 1- dbo.fnGetWeekend(Employee.EmployeeId,@fromDate, @toDate)-dbo.fnGetHolidays(Employee.EmployeeId,@fromDate, @toDate))) AS WorkingDays					
						 ,CONVERT(NVARCHAR(20), dbo.fnGetWeekend(Employee.EmployeeId,@fromDate, @toDate)) AS WeekendDays  
						 ,CONVERT(NVARCHAR(20), dbo.fnGetHolidays(Employee.EmployeeId,@fromDate, @toDate)) AS HolidayDays
							
						 ,CONVERT(NVARCHAR(20), dbo.fnGetPresentDays(Employee.EmployeeId, @fromDate, @toDate)) AS PresentDays
						 ,CONVERT(NVARCHAR(20), dbo.fnGetAbsentDays(Employee.EmployeeId, @fromDate, @toDate)) AS AbsentDays						 		
						 ,CONVERT(NVARCHAR(20), dbo.fnGetTotalLateDays(Employee.EmployeeId, @fromDate, @toDate)) AS LateDays
						 ,CONVERT(NVARCHAR(20), dbo.fnGetTotalLeave(Employee.EmployeeId, @fromDate, @toDate)) AS LeaveDays
						 																																	
						 ,CONVERT(NVARCHAR(20), EmployeeSalary.BasicSalary) AS BasicSalary
						 ,CONVERT(NVARCHAR(20), EmployeeSalary.HouseRent) AS HouseRent
						 ,CONVERT(NVARCHAR(20), EmployeeSalary.MedicalAllowance) AS  MedicalAllowance  
						 ,CONVERT(NVARCHAR(20), EmployeeSalary.Conveyance) AS Conveyance
						 ,CONVERT(NVARCHAR(20), EmployeeSalary.FoodAllowance) AS FoodAllowance
						 ,CONVERT(NVARCHAR(20), EmployeeSalary.EntertainmentAllowance) AS EntertainmentAllowance
						 ,CONVERT(NVARCHAR(20), EmployeeSalary.GrossSalary) AS GrossSalary

						 ,CONVERT(NVARCHAR(20), dbo.fnGetAttendanceBonusAmount(@EmployeeId, @FromDate, @ToDate)) AS AttendanceBonus  
						 ,'0' AS ShiftingBonus
						 ,CONVERT(NVARCHAR(20), dbo.fnGetAttendanceBonusAmount(@EmployeeId, @FromDate, @ToDate)) AS TotalBonus -- Add Shift bonus when logic come

						 ,CONVERT(NVARCHAR(20), CAST((EmployeeSalary.BasicSalary * dbo.fnGetOverTimeRate(@fromDate, @toDate)/208) AS DECIMAL(18,2))) AS OTRate					
						 ,CONVERT(NVARCHAR(20), dbo.fnGetTotalOTHours(Employee.EmployeeId, @fromDate, @toDate)) AS OTHours
						 ,CONVERT(NVARCHAR(20), CAST((EmployeeSalary.BasicSalary * dbo.fnGetOverTimeRate(@fromDate, @toDate)/208.00 * dbo.fnGetTotalOTHours(Employee.EmployeeId, @fromDate, @toDate)) AS DECIMAL(18,2))) AS TotalOTAmount				
						 ,CONVERT(NVARCHAR(20), EmployeeSalary.GrossSalary + dbo.fnGetAttendanceBonusAmount(@EmployeeId, @FromDate, @ToDate)+ CAST((EmployeeSalary.BasicSalary * dbo.fnGetOverTimeRate(@fromDate, @toDate)/208.00 * dbo.fnGetTotalOTHours(Employee.EmployeeId, @fromDate, @toDate)) AS DECIMAL(18,2))) AS TotalPaid
						
						 ,CONVERT(NVARCHAR(20), @Stamp) AS Stamp
						 ,CONVERT(NVARCHAR(20), CONVERT(DECIMAL(18,2), (EmployeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 ) * (dbo.fnGetAbsentDays(Employee.EmployeeId, @fromDate, @toDate))))) AS AbsentFee
						 ,CONVERT(NVARCHAR(20), dbo.fnGetAdvanceAmount(@EmployeeId, @fromDate, @toDate)) AS Advance
						 ,CONVERT(NVARCHAR(20), @Stamp + dbo.fnGetAdvanceAmount(@EmployeeId, @fromDate, @toDate) + CONVERT(DECIMAL(18,2), (EmployeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 ) * (dbo.fnGetAbsentDays(Employee.EmployeeId, @fromDate, @toDate))))) AS TotalDeduction

						 ,CONVERT(NVARCHAR(20), EmployeeSalary.GrossSalary + dbo.fnGetAttendanceBonusAmount(@EmployeeId, @FromDate, @ToDate)+ CAST((EmployeeSalary.BasicSalary * dbo.fnGetOverTimeRate(@fromDate, @toDate)/208.00 * dbo.fnGetTotalOTHours(Employee.EmployeeId, @fromDate, @toDate)) AS DECIMAL(18,2)) - @Stamp - dbo.fnGetAdvanceAmount(@EmployeeId, @fromDate, @toDate)-CONVERT(DECIMAL(18,2), (EmployeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 ) * (dbo.fnGetAbsentDays(Employee.EmployeeId, @fromDate, @toDate))))) AS NetAmount
						 ,CONVERT(NVARCHAR(20), MONTH(GETDATE())) AS Month
						 ,CONVERT(NVARCHAR(20), YEAR(GETDATE())) AS Year
						 ,Department.id AS DepartmentId
						 ,CONVERT(VARCHAR(10), @fromDate, 103) AS FromDate	
						 ,CONVERT(VARCHAR(10), @toDate, 103) AS ToDate	
						 ,CONVERT(VARCHAR(10), dbo.fnGetOverTimeRate(@fromDate, @toDate)) AS Rate
						 ,employeeType.id AS EmployeeTypeId

FROM					Employee AS  employee

						LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @StartDate) OR (@StartDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1 	
								
						INNER JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
						INNER JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
						INNER JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
						INNER JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
						INNER JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
						INNER JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
						INNER JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
						INNER JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
						INNER JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
						INNER JOIN Company  AS company ON branch.CompanyId=company.Id
						
						LEFT JOIN (SELECT EmployeeId, FromDate, BasicSalary, HouseRent, MedicalAllowance, Conveyance, FoodAllowance,EntertainmentAllowance, GrossSalary,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSalary
						FROM EmployeeSalary AS EmployeeSalary 
						WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @fromDate) OR (@fromDate IS NULL)) AND EmployeeSalary.IsActive=1) EmployeeSalary 
						ON employee.EmployeeId = EmployeeSalary.EmployeeId AND EmployeeSalary.rowNumSalary = 1		

						LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
						LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
						LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
						LEFT JOIN Line line on departmentLine.LineId = line.LineId

						WHERE employee.IsActive = 1
						AND employee.[Status] = 1 	
						AND (employee.EmployeeCardId = @employeeCardId OR  @employeeCardId IS NULL)
						AND employeeCompanyInfo.BranchUnitDepartmentId = @departmentId
						AND (employeeCompanyInfo.DepartmentSectionId = @sectionId OR @sectionId IS NULL)
						AND (employeeCompanyInfo.DepartmentLineId = @lineId OR @lineId IS NULL)
						AND (employeeType.Id = @EmployeeTypeId OR @EmployeeTypeId = 0)
						ORDER BY EmployeeCardId ASC	
																	
END






