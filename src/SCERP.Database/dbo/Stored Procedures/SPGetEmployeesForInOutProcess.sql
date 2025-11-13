
-- =========================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-12>
-- Description:	<> EXEC [SPGetEmployeesForInOutProcess] -1, -1, -1, -1, -1, -1, '2015-11-01','2015-11-30', -1, '','superadmin'
-- =========================================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeesForInOutProcess]
	   @CompanyId INT = -1,
	   @BranchId INT = -1,
	   @BranchUnitId INT = -1,
	   @BranchUnitDepartmentId INT = -1, 
	   @DepartmentSectionId INT = -1, 
	   @DepartmentLineId INT = -1, 
	   @FromDate DateTime,
	   @ToDate DateTime,
	   @EmployeeTypeId INT = -1,
	   @EmployeeCardId NVARCHAR(100) = '',
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

					  
		SELECT		     
		 company.Id AS CompanyId
		,company.Name AS CompanyName
		,branch.Id AS BranchId
		,branch.Name AS BranchName
		,branchUnit.BranchUnitId 
		,unit.Name AS UnitName
		,branchUnitDepartment.BranchUnitDepartmentId
		,department.Name AS DepartmentName
		,departmentSection.DepartmentSectionId 
		,section.Name AS SectionName
		,departmentLine.DepartmentLineId
		,line.Name AS LineName
		,employee.EmployeeId
		,employee.EmployeeCardId
		,employee.Name AS EmployeeName
		,employeeType.Id AS EmployeeTypeId
		,employeeType.Title AS EmployeeType
		,employeeGrade.Id AS EmployeeGradeId
		,employeeGrade.Name AS EmployeeGradeName
		,employeeDesignation.Id AS EmployeeDesignationId
		,employeeDesignation.Title AS EmployeeDesignation
		,employee.JoiningDate
		,employee.QuitDate
		,presentAddress.MobilePhone
						
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
		ORDER BY EmployeeCardId ASC	


		COMMIT TRAN


END






