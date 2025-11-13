-- =========================================================================================================================================
-- Author:		Golam Rabbi
-- Create date: 2016.05.30
-- Description:	To select employee's company
-- EXEC [SCERPDB_DEV].[dbo].[spHrmGetEmployeesForAssigingJobType]  -1, -1, -1, -1, -1, -1, -1, '', NULL, NULL, '2015-12-31', 'superadmin' 
-- =========================================================================================================================================

CREATE PROCEDURE [dbo].[spHrmGetEmployeesForAssigingJobType]
	   
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
	   @ExistingJobTypeId INT = -1,
	   @UpToDate DateTime,	   
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

		IF (@ExistingJobTypeId  = -1)
		BEGIN
			SET @ExistingJobTypeId = NULL
		END
		


		SELECT DISTINCT		
		employeeCompanyInfo.EmployeeCompanyInfoId AS EmployeeCompanyInfoId,
		employee.EmployeeId AS EmployeeId,
		employee.EmployeeCardId AS EmployeeCardId, 
		employee.Name AS EmployeeName,		
		employee.JoiningDate AS JoiningDate,
		department.Name AS DepartmentName,
		section.Name AS SectionName,
		line.Name AS LineName,	
		employeeType.Title AS EmployeeType,
		employeeGrade.Name AS EmployeeGrade,
		employeeDesignation.Title AS Designation,
		skillSet.Title AS JobType,
		employeeCompanyInfo.PunchCardNo AS PunchCardNo,
		employeeCompanyInfo.IsEligibleForOvertime AS IsEligibleForOvertime,
		employeeCompanyInfo.FromDate AS FromDate,
		employeeCompanyInfo.ToDate AS ToDate
		FROM Employee AS  employee
		LEFT JOIN (SELECT EmployeeId, EmployeeCompanyInfoId, PunchCardNo, DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, JobTypeId,
						  IsEligibleForOvertime, FromDate, ToDate, 
						  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
					FROM EmployeeCompanyInfo AS empCompanyInfo 
					WHERE ((CAST(empCompanyInfo.FromDate AS Date) <= @UpToDate) OR (@UpToDate IS NULL)) AND empCompanyInfo.IsActive=1) employeeCompanyInfo 
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
		LEFT JOIN SkillSet skillSet ON skillSet.Id = employeeCompanyInfo.JobTypeId
		WHERE 
		(company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds)
		AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
		AND branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
		AND branchUnitDepartment.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
		AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
		AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
		AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
		AND ((employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId IS NULL))
		AND (CAST(employee.JoiningDate AS date) >= CAST(@JoiningDateBegin AS date) OR @JoiningDateBegin IS NULL)
		AND (CAST(employee.JoiningDate AS date) <= CAST(@JoiningDateEnd AS date) OR @JoiningDateEnd IS NULL)
		AND ((skillSet.Id = @ExistingJobTypeId) OR (@ExistingJobTypeId IS NULL))
		AND employee.IsActive = 1
		AND employee.[Status] = 1)


		ORDER BY employee.EmployeeCardId ASC
		
		
		COMMIT TRAN
END



