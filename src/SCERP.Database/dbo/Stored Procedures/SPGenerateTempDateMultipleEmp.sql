-- ============================================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <07/04/2015>
-- Description:	<> exec SPGenerateTempDateMultipleEmp '02/01/2015','02/28/2015', NULL, NULL, NULL, NULL, '45139A3C-6C12-4C6C-8CE4-E18E9CB1BD53'
-- =============================================================================================================================================
CREATE PROCEDURE [dbo].[SPGenerateTempDateMultipleEmp]
	   
				@StartDate DATETIME,
				@EndDate DATETIME,
				@DepartmentId INT = NULL,
				@LineId INT = NULL,
				@SectionId INT = NULL,
				@EmployeeTypeId INT = NULL,
				@EmployeeId uniqueidentifier = NULL
AS
BEGIN
	
		SET NOCOUNT ON;

		DECLARE @FromDate Datetime = CURRENT_TIMESTAMP
		TRUNCATE TABLE TempDate

		DECLARE	@Days INT,
				@Count INT

	SET @Days = DATEDIFF(DAY, @StartDate, @EndDate)

	--print @Days

WHILE @Days >= 0
	BEGIN
		
				INSERT INTO TempDate (MonthDate, MonthDay, EmployeeId) 
						
				SELECT @StartDate, DateName(dw, @StartDate), Employee.EmployeeId FROM Employee 													 					
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

				WHERE employee.IsActive = 1 AND employeeCompanyInfo.rowNum = 1 AND employee.[Status] = 1						
				AND (employeeCompanyInfo.BranchUnitDepartmentId = @DepartmentId OR @DepartmentId IS NULL)		
				AND (employeeCompanyInfo.DepartmentLineId = @LineId OR @LineId IS NULL)
				AND (employeeCompanyInfo.DepartmentSectionId = @SectionId OR @SectionId IS NULL)	
				AND (employeeType.Id = @EmployeeTypeId OR  @EmployeeTypeId IS NULL)	
				AND (employee.EmployeeId = @EmployeeId OR @EmployeeId IS NULL)
  	    SET @StartDate =  DATEADD (day, 1, @StartDate)
		SET @Days = @Days - 1;	
	END	   
END






