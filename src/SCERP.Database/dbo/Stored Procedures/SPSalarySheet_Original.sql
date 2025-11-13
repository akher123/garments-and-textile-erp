

-- ==================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <18/03/2015>
-- Description:	<> EXEC [SPSalarySheet] '0180', 2, 0, 0, '2015-09-01','2015-09-30',0
-- ==================================================================================


CREATE PROCEDURE [dbo].[SPSalarySheet_Original]

	   @employeeCardId NVARCHAR(100), 
	   @departmentId INT, 
	   @sectionId INT, 
	   @lineId INT, 
	   @fromDate DateTime,
	   @toDate DateTime ,
	   @employeeTypeId INT 	
AS
BEGIN
	
			SET NOCOUNT ON;
			
			IF(@employeeCardId = '')
			   SET @employeeCardId = NULL;

			IF(@sectionId = 0)
			   SET @sectionId = NULL;

			IF(@lineId = 0)
			   SET @lineId = NULL;

			Declare @StartDate Datetime = @fromDate

					  
			SELECT		  CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY Employee.EmployeeCardId)) AS Row
						 ,Employee.EmployeeId
						 ,Employee.EmployeeCardId
						 ,Company.Name AS CompanyName
						 ,Company.FullAddress AS CompanyAddress
						 ,Employee.Name AS Name
						 ,EmployeeDesignation.Title AS Designation
						 ,EmployeeGrade.Name AS Grade
						 ,CONVERT(VARCHAR(10), Employee.JoiningDate, 103) JoiningDate
						 ,Branch.Name AS Branch
						 ,Unit.Name AS Unit
						 ,Department.Name AS Department
						 ,Section.Name AS Section
						 		
						 ,CONVERT(NVARCHAR(20), DATEDIFF(DAY, @FromDate, @ToDate) + 1) AS TotalDays
						 ,CONVERT(NVARCHAR(20),(DATEDIFF(DAY, @FromDate, @ToDate) + 1) - dbo.fnGetWeekend(Employee.EmployeeId,@fromDate, @toDate) - dbo.fnGetHolidays(Employee.EmployeeId,@fromDate, @toDate)) AS WorkingDays	
						 ,CONVERT(NVARCHAR(20), (dbo.fnGetPresentDays(Employee.EmployeeId, @fromDate, @toDate) + dbo.fnGetWeekend(Employee.EmployeeId,@fromDate, @toDate) + dbo.fnGetHolidays(Employee.EmployeeId,@fromDate, @toDate) + dbo.fnGetTotalLeave(Employee.EmployeeId, @fromDate, @toDate))) AS Paydays				
						 ,CONVERT(NVARCHAR(20), dbo.fnGetWeekend(Employee.EmployeeId,@fromDate, @toDate)) AS WeekendDays  
						 ,CONVERT(NVARCHAR(20), dbo.fnGetHolidays(Employee.EmployeeId,@fromDate, @toDate)) AS HolidayDays 										
						 ,CONVERT(NVARCHAR(20), dbo.fnGetPresentDays(Employee.EmployeeId, @fromDate, @toDate)) AS PresentDays
						 ,CONVERT(NVARCHAR(20), dbo.fnGetAbsentDays(Employee.EmployeeId, @fromDate, @toDate)) AS AbsentDays						 		
						 ,CONVERT(NVARCHAR(20), dbo.fnGetTotalLateDays(Employee.EmployeeId, @fromDate, @toDate)) AS LateDays
						 ,CONVERT(NVARCHAR(20), dbo.fnGetTotalLeave(Employee.EmployeeId, @fromDate, @toDate)) AS LeaveDays
						 ,CONVERT(NVARCHAR(20), dbo.fnGetLWPDays(Employee.EmployeeId, @fromDate, @toDate)) AS LWPDays	
						 ,CONVERT(NVARCHAR(20), dbo.fnGetIndividualLeaveDays(Employee.EmployeeId, @fromDate, @toDate, 1)) AS CasualLeave		
						 ,CONVERT(NVARCHAR(20), dbo.fnGetIndividualLeaveDays(Employee.EmployeeId, @fromDate, @toDate, 2)) AS SickLeave	
					     ,CONVERT(NVARCHAR(20), dbo.fnGetIndividualLeaveDays(Employee.EmployeeId, @fromDate, @toDate, 4)) AS MaternityLeave	
						 ,CONVERT(NVARCHAR(20), dbo.fnGetIndividualLeaveDays(Employee.EmployeeId, @fromDate, @toDate, 6)) AS EarnLeave
																																																																															
						 ,CONVERT(DECIMAL(18,2), EmployeeSalary.BasicSalary) AS BasicSalary
						 ,CONVERT(DECIMAL(18,2), EmployeeSalary.HouseRent) AS HouseRent
						 ,CONVERT(DECIMAL(18,2), EmployeeSalary.MedicalAllowance) AS  MedicalAllowance  
						 ,CONVERT(DECIMAL(18,2), EmployeeSalary.Conveyance) AS Conveyance
						 ,CONVERT(DECIMAL(18,2), EmployeeSalary.FoodAllowance) AS FoodAllowance
						 ,CONVERT(DECIMAL(18,2), EmployeeSalary.EntertainmentAllowance) AS EntertainmentAllowance
						 ,CONVERT(DECIMAL(18,2), EmployeeSalary.GrossSalary) AS GrossSalary

						 ,CONVERT(DECIMAL(18,2), (EmployeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 ) * (dbo.fnGetLWPDays(Employee.EmployeeId, @fromDate, @toDate)))) AS LWPFee
						 ,CONVERT(DECIMAL(18,2), (EmployeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 ) * (dbo.fnGetAbsentDays(Employee.EmployeeId, @fromDate, @toDate)))) AS AbsentFee
						 ,CONVERT(DECIMAL(18,2), dbo.fnGetAdvanceAmount(Employee.EmployeeId, @fromDate, @toDate)) AS Advance
						 ,CONVERT(DECIMAL(18,2), (SELECT TOP(1) StampAmount.Amount FROM StampAmount 
						  WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @fromDate
						  ORDER BY StampAmount.FromDate DESC ))  AS Stamp

						 ,CONVERT(DECIMAL(18,2), (EmployeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 ) * (dbo.fnGetLWPDays(Employee.EmployeeId, @fromDate, @toDate))) + (EmployeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 ) * (dbo.fnGetAbsentDays(Employee.EmployeeId, @fromDate, @toDate))) + dbo.fnGetAdvanceAmount(Employee.EmployeeId, @fromDate, @toDate) +  (SELECT TOP(1) StampAmount.Amount FROM StampAmount 
						  WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @fromDate
						  ORDER BY StampAmount.FromDate DESC )) AS TotalDeduction

						 ,CONVERT(DECIMAL(18,2), dbo.fnGetAttendanceBonusAmount(Employee.EmployeeId, @fromDate, @toDate)) AS AttendanceBonus
						 ,CONVERT(DECIMAL(18,2), 0.00) AS ShiftingBonus	-- Business logic not defined yet
						 ,CONVERT(DECIMAL(18,2), dbo.fnGetAttendanceBonusAmount(Employee.EmployeeId, @fromDate, @toDate))  AS TotalBonus

						 ,CONVERT(DECIMAL(18,2), (EmployeeSalary.GrossSalary - ((EmployeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 ) 
						 * (dbo.fnGetLWPDays(Employee.EmployeeId, @fromDate, @toDate))) + (EmployeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 ) 
						 * (dbo.fnGetAbsentDays(Employee.EmployeeId, @fromDate, @toDate))) + dbo.fnGetAdvanceAmount(Employee.EmployeeId, @fromDate, @toDate) 
						 +  (SELECT TOP(1) StampAmount.Amount FROM StampAmount 
						  WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @fromDate
						  ORDER BY StampAmount.FromDate DESC )) + (dbo.fnGetAttendanceBonusAmount(Employee.EmployeeId, @fromDate, @toDate))))  AS TotalPaid    -- TotalPaid = gross - deduction + att Bonus + Shift Allow

						 ,CONVERT(DECIMAL(18,2), (dbo.fnGetTotalOTHours(Employee.EmployeeId, @fromDate, @toDate) 
						 + dbo.fnGetTotalExtraOTHours(Employee.EmployeeId, @fromDate, @toDate))) AS OTHours

						 ,CASE employeeCompanyInfo.IsEligibleForOvertime WHEN 1 THEN 
						 CAST(((EmployeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@fromDate, @toDate))) AS DECIMAL(18,2)) ELSE 0.00 END AS OTRate
						 												
						 ,CONVERT(DECIMAL(18,2), (CAST((((EmployeeSalary.BasicSalary/208.00)* (dbo.fnGetOverTimeRate(@fromDate, @toDate))) 
						 * (dbo.fnGetTotalOTHours(Employee.EmployeeId, @fromDate, @toDate))) AS DECIMAL(18,2)) 
						 + CAST((((EmployeeSalary.BasicSalary/208.00) * (dbo.fnGetOverTimeRate(@fromDate, @toDate))) 
						 * (dbo.fnGetTotalExtraOTHours(Employee.EmployeeId, @fromDate, @toDate))) AS DECIMAL(18,2)))) AS TotalOTAmount						 													 						 						 			

						 ,CONVERT(DECIMAL(18,2),ROUND(((EmployeeSalary.GrossSalary + 
						  CAST((((EmployeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@fromDate, @toDate))) 
						  * ((dbo.fnGetTotalOTHours(Employee.EmployeeId, @fromDate, @toDate)) + 
						  (dbo.fnGetTotalExtraOTHours(Employee.EmployeeId, @fromDate, @toDate)))) 
						  AS DECIMAL(18,2)) + dbo.fnGetAttendanceBonusAmount(Employee.EmployeeId, @fromDate, @toDate)) - 
						  ((EmployeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 ) 
						  * (dbo.fnGetLWPDays(Employee.EmployeeId, @fromDate, @toDate))) 
						  + (EmployeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 ) 
						  * (dbo.fnGetAbsentDays(Employee.EmployeeId, @fromDate, @toDate))) + 
						  dbo.fnGetAdvanceAmount(Employee.EmployeeId, @fromDate, @toDate) +  (SELECT TOP(1) StampAmount.Amount FROM StampAmount 
						  WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @fromDate
						  ORDER BY StampAmount.FromDate DESC ))),0)) AS NetAmount

						 ,CONVERT(NVARCHAR(20), MONTH(GETDATE())) AS Month
						 ,CONVERT(NVARCHAR(20),YEAR(GETDATE())) AS Year			
						 ,branchUnitDepartment.BranchUnitDepartmentId AS DepartmentId
						 ,departmentSection.DepartmentSectionId AS SectionId
						 ,departmentLine.DepartmentLineId AS LineId
						 ,DATENAME(m,@fromDate)+'-'+CAST(DATEPART(yyyy,@fromDate) AS VARCHAR) AS [MonthName]
						 ,CONVERT(VARCHAR(10), @fromDate, 103) AS FromDate
						 ,CONVERT(VARCHAR(10), @toDate, 103) AS ToDate
						
FROM					Employee AS  employee

						LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,IsEligibleForOvertime,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						--WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @StartDate) OR (@StartDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1 
										
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
						
						LEFT JOIN (SELECT EmployeeId, FromDate,ToDate, BasicSalary, HouseRent, MedicalAllowance, Conveyance, FoodAllowance, EntertainmentAllowance, GrossSalary,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSalary
						FROM EmployeeSalary AS EmployeeSalary 
						WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND EmployeeSalary.IsActive=1) EmployeeSalary 
						ON employee.EmployeeId = EmployeeSalary.EmployeeId AND rowNumSalary = 1
		
						LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
						LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
						LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
						LEFT JOIN Line line on departmentLine.LineId = line.LineId

						WHERE employee.IsActive = 1
						AND employee.[Status] = 1 
						AND (employeeType.Id = @EmployeeTypeId OR @EmployeeTypeId = 0)
						AND (employee.EmployeeCardId = @employeeCardId OR  @employeeCardId IS NULL)
						AND employeeCompanyInfo.BranchUnitDepartmentId = @departmentId
						AND (employeeCompanyInfo.DepartmentSectionId = @sectionId OR @sectionId IS NULL)
						AND (employeeCompanyInfo.DepartmentLineId = @lineId OR @lineId IS NULL)
																	
						ORDER BY EmployeeCardId ASC	
END






