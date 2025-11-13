
-- ===============================================================================================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-11-22>
-- Description:	<> EXEC [spPayrollGetEmployeesForExcludingEmployeeFromSalaryProcess] -1, -1, -1, -1, -1, -1, -1,'','1900-01-01', '1900-01-01', '1900-01-01', '1900-01-01','2016-03-26','2016-04-25','superadmin'
-- ==============================================================================================================================================================================================================

CREATE PROCEDURE [dbo].[spPayrollGetEmployeesForExcludingEmployeeFromSalaryProcess]
	   @CompanyId INT = -1,
	   @BranchId INT = -1,
	   @BranchUnitId INT = -1,
	   @BranchUnitDepartmentId INT = -1, 
	   @DepartmentSectionId INT = -1, 
	   @DepartmentLineId INT = -1, 
	   @EmployeeTypeId INT = -1,
	   @EmployeeCardId NVARCHAR(100) = '',
	   @JoiningDateBegin DATETIME = NULL,
	   @JoiningDateEnd DATETIME = NULL,
	   @QuitDateBegin DATETIME = NULL,
	   @QuitDateEnd DATETIME = NULL,
	   @FromDate DATETIME,
	   @ToDate DATETIME,	  
	   @UserName NVARCHAR(100)
AS
BEGIN
	
		SET XACT_ABORT ON;
		SET NOCOUNT ON;

		DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
		DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
		DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
		DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);
		DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);
		
		BEGIN TRAN

		IF(@CompanyId = -1)
		BEGIN
			INSERT INTO @ListOfCompanyIds
			SELECT DISTINCT CompanyId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfCompanyIds VALUES (@CompanyId)
		END

		IF(@BranchId = -1)
		BEGIN
			INSERT INTO @ListOfBranchIds
			SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchIds VALUES (@BranchId)
		END

		IF(@BranchUnitId = -1)
		BEGIN
			INSERT INTO @ListOfBranchUnitIds
			SELECT DISTINCT BranchUnitId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchUnitIds VALUES (@BranchUnitId)
		END

		IF(@BranchUnitDepartmentId = -1)
		BEGIN
			INSERT INTO @ListOfBranchUnitDepartmentIds
			SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchUnitDepartmentIds VALUES (@BranchUnitDepartmentId)
		END

		IF(@EmployeeTypeID = -1)
		BEGIN
			INSERT INTO @ListOfEmployeeTypeIds
			SELECT DISTINCT EmployeeTypeId FROM UserPermissionForEmployeeLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfEmployeeTypeIds VALUES (@EmployeeTypeID)
		END

		IF (@DepartmentSectionId  = -1)
		BEGIN
			SET @DepartmentSectionId = NULL
		END

		IF (@DepartmentLineId  = -1)
		BEGIN
			SET @DepartmentLineId = NULL
		END

		
		IF (@EmployeeCardId  = '')
		BEGIN
			SET @EmployeeCardId = NULL
		END

		IF(CAST(@JoiningDateBegin AS DATE) = '1900-01-01')
				SET @JoiningDateBegin = NULL;

		IF(CAST(@JoiningDateEnd AS DATE) = '1900-01-01')
			SET @JoiningDateEnd =NULL;

		IF(CAST(@QuitDateBegin AS DATE) = '1900-01-01')
			SET @QuitDateBegin = NULL;

		IF(CAST(@QuitDateEnd AS DATE) = '1900-01-01')
			SET @QuitDateEnd =NULL;

				  
		SELECT		     
		 company.Name AS Company
		,branch.Name AS Branch
		,unit.Name AS Unit
		,department.Name AS Department
		,section.Name AS Section
		,line.Name AS Line
		,employee.EmployeeId
		,employee.EmployeeCardId
		,employee.Name AS Name
		,employeeType.Title AS EmployeeType
		,employeeDesignation.Title AS Designation
		,employee.JoiningDate
		,employee.QuitDate
						
FROM					
		Employee AS  employee
						
		LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,IsEligibleForOvertime,
		ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
		FROM EmployeeCompanyInfo AS employeeCompanyInfo 
		WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
		ON employee.EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1 

		LEFT JOIN EmployeePresentAddress presentAddress ON Employee.EmployeeId = presentAddress.EmployeeId AND presentAddress.IsActive = 1 
										
		LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
		LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
		LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
		LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
		LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
		LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
		LEFT JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
		LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
		LEFT JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
		LEFT JOIN Company  AS company ON branch.CompanyId=company.Id		
		LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
		LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
		LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
		LEFT JOIN Line line on departmentLine.LineId = line.LineId

		WHERE employee.IsActive = 1
		AND ((employee.[Status] = 1) OR 
		    ((employee.[Status] = 2) AND (employee.QuitDate >= @FromDate) AND (employee.QuitDate <= (DATEADD(DAY, 30, @ToDate)))))
		AND company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds)
		AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
		AND branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
		AND branchUnitDepartment.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
		AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
		AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
		AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
		AND ((employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId IS NULL))	
		AND CAST(employee.JoiningDate AS DATE) <= CAST(@ToDate AS DATE)
		AND (CAST(Employee.JoiningDate AS date) >= CAST(@JoiningDateBegin AS date) OR @JoiningDateBegin IS NULL)
		AND (CAST(Employee.JoiningDate AS date) <= CAST(@JoiningDateEnd AS date) OR @JoiningDateEnd IS NULL)
		AND (CAST(Employee.QuitDate AS date) >= CAST(@QuitDateBegin AS date) OR @QuitDateBegin IS NULL)
		AND (CAST(Employee.QuitDate AS date) <= CAST(@QuitDateEnd AS date) OR @QuitDateEnd IS NULL)				
		ORDER BY EmployeeCardId ASC	


		COMMIT TRAN


END






