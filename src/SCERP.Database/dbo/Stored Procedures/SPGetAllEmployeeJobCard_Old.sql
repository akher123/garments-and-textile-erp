
-- ==========================================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> EXEC [SPGetAllEmployeeJobCard] 1, 1, 1, 2, NULL,NULL,NULL,NULL,'09/01/2015','09/30/2015'
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetAllEmployeeJobCard_Old]
				@CompanyId INT,
				@BranchId INT,
				@BranchUnitId INT,
				@BranchUnitDepartmentId INT = NULL,
				@DepartmentSectionId INT = NULL,
				@DepartmentLineId INT = NULL,
				@EmployeeTypeId INT = NULL,
				@EmployeeCardId NVARCHAR(100) = NULL,
				@StartDate DATETIME,
				@EndDate DATETIME


AS
BEGIN
	
	SET NOCOUNT ON;
		
		DECLARE @FromDate Datetime = CURRENT_TIMESTAMP
		

		SELECT    employee.EmployeeId AS EmployeeId, 
				  employee.EmployeeCardId AS EmployeeCardId,
				  employee.Name AS EmployeeName,
				  company.Name AS CompanyName,
				  company.FullAddress AS CompanyAddress,
				  Branch.Name AS Branch,
				  Unit.Name AS Unit,	
				  Department.Name AS Department,
				  Section.Name AS Section,
				  Line.Name AS Line,
				  EmployeeType.Title AS EmployeeType,
				  EmployeeDesignation.Title AS Designation,
				  CONVERT(VARCHAR(10),employee.JoiningDate, 103) AS JoiningDate,
				  CONVERT(NVARCHAR(20), DATEDIFF(DAY, @StartDate, @EndDate) + 1) AS TotalDays,
				  CONVERT(NVARCHAR(20), dbo.fnGetPresentDays(employee.EmployeeId, @StartDate, @EndDate))  AS PresentDays,
				  CONVERT(NVARCHAR(20), dbo.fnGetAbsentDays(Employee.EmployeeId, @StartDate, @EndDate)) AS AbsentDays,
				  CONVERT(NVARCHAR(20), dbo.fnGetWeekend(Employee.EmployeeId,@StartDate, @EndDate)) AS WeekendDays,
				  CONVERT(NVARCHAR(20), dbo.fnGetHolidays(Employee.EmployeeId,@StartDate, @EndDate)) AS Holidays,
				  CONVERT(NVARCHAR(20), dbo.fnGetTotalLeave(employee.EmployeeId, @StartDate, @EndDate)) AS LeaveDays,
				  CONVERT(NVARCHAR(20), dbo.fnGetTotalLateDays(Employee.EmployeeId, @StartDate, @EndDate))  AS LateDays,		
				  CONVERT(NVARCHAR(20), dbo.fnGetTotalOTHours(employee.EmployeeId, @StartDate, @EndDate)) AS TotalOTHours,
				  CONVERT(NVARCHAR(20), dbo.fnGetTotalExtraOTHours(employee.EmployeeId, @StartDate, @EndDate)) AS TotalExtraOTHours,
				  CONVERT(VARCHAR(10),@StartDate, 103) AS FromDate,
				  CONVERT(VARCHAR(10),@EndDate, 103) AS ToDate
				 				 		
		 FROM   Employee 

				LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
				ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
				FROM EmployeeCompanyInfo AS employeeCompanyInfo 
				WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
				ON employee.EmployeeId = employeeCompanyInfo.EmployeeId
				
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
				LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
				LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
				LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
				LEFT JOIN Line line on departmentLine.LineId = line.LineId

				WHERE employee.IsActive = 1 
				AND employee.[Status] = 1 
				AND employeeCompanyInfo.rowNum = 1  
				AND (company.Id = @CompanyId)
				AND (branch.Id = @BranchId)
				AND (unit.UnitId = @BranchUnitId)
				AND (branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId IS NULL)
				AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
				AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)
				AND (employeeType.Id = @EmployeeTypeId OR @EmployeeTypeId IS NULL)
				AND (employee.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId IS NULL)
				ORDER BY EmployeeCardId ASC				
END





