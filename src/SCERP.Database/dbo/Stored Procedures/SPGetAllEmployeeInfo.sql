-- =====================================================================================================================================================
-- Author:		Golam Rabbi
-- Create date: 2015.04.18
-- Description:	To Get All Employee Info
-- Exec SPGetAllEmployeeInfo null, null, null, null, null, null, null, null, null, null, null, null,null, null, 'superadmin', null,1, 2000, 1, 1, 10
-- =====================================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetAllEmployeeInfo]
	-- Add the parameters for the stored procedure here
	 @CompanyID INT = NULL,
	 @BranchID INT = NULL,
	 @BranchUnitID INT = NULL,
	 @BranchUnitDepartmentID INT = NULL,
	 @DepartmentSectionId INT = NULL,
	 @DepartmentLineId INT = NULL,
	 @EmployeeTypeID INT = NULL,
	 @EmployeeGradeID INT = NULL,
	 @EmployeeDesignationID INT = NULL,
	 @GenderId INT = NULL,
	 @EmployeeCardId NVARCHAR(100) = NULL,
	 @EmployeeName NVARCHAR(100) = NULL,
	 @EmployeeMobilePhone NVARCHAR(100) = NULL,
	 @EmployeeStatus INT = NULL,
	 @UserName NVARCHAR(100),
	 @FromDate DATETIME = NULL,

	 @StartRowIndex INT = NULL,
	 @MaxRows INT = NULL,
	 @SortField INT = NULL,
	 @SortDiriection INT = NULL,
	 @RowCount INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SQL AS NVARCHAR(1000);
	DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
    DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
    DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
    DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);
    DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);

    -- Insert statements for procedure here
	IF(@CompanyID IS NULL)
	BEGIN
	   INSERT INTO @ListOfCompanyIds
	   SELECT DISTINCT CompanyId FROM UserPermissionForDepartmentLevel
	   WHERE UserName = @UserName;
	END  
	ELSE
	BEGIN
	   INSERT INTO @ListOfCompanyIds VALUES (@CompanyID)
	END


	IF(@BranchID IS NULL)
	BEGIN
	   INSERT INTO @ListOfBranchIds
	   SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
	   WHERE UserName = @UserName;
	END  
	ELSE
	BEGIN
	   INSERT INTO @ListOfBranchIds VALUES (@BranchID)
	END


	IF(@BranchUnitID IS NULL)
	BEGIN
	   INSERT INTO @ListOfBranchUnitIds
	   SELECT DISTINCT BranchUnitId FROM UserPermissionForDepartmentLevel
	   WHERE UserName = @UserName;
	END  
	ELSE
	BEGIN
	   INSERT INTO @ListOfBranchUnitIds VALUES (@BranchUnitID)
	END


	IF(@BranchUnitDepartmentID IS NULL)
	BEGIN
	   INSERT INTO @ListOfBranchUnitDepartmentIds
	   SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
	   WHERE UserName = @UserName;
	END  
	ELSE
	BEGIN
	   INSERT INTO @ListOfBranchUnitDepartmentIds VALUES (@BranchUnitDepartmentID)
	END


	IF(@EmployeeTypeID IS NULL)
	BEGIN
	   INSERT INTO @ListOfEmployeeTypeIds
	   SELECT DISTINCT EmployeeTypeId FROM UserPermissionForEmployeeLevel
	   WHERE UserName = @UserName;
	END  
	ELSE
	BEGIN
	   INSERT INTO @ListOfEmployeeTypeIds VALUES (@EmployeeTypeID)
	END


	DECLARE @ReturnTable TABLE (
								RowID INT,
								EmployeeId UNIQUEIDENTIFIER NULL,
								EmployeeCardNo NVARCHAR(100) NULL,
								Name NVARCHAR(100) NULL,
								DOB DATETIME NULL,
								Gender NVARCHAR(100) NULL,
								EmployeeStatus INT NULL,
								Mobile NVARCHAR(100) NULL,
								Company NVARCHAR(100) NULL,
								Branch NVARCHAR(100) NULL,
								Unit NVARCHAR(100) NULL,
								Department NVARCHAR(100) NULL,
								Section NVARCHAR(100) NULL,
								Line NVARCHAR(100) NULL,
								Designation NVARCHAR(100) NULL,
								EffectiveFrom DATETIME NULL
								);

	INSERT INTO @ReturnTable
	SELECT DISTINCT
					ROW_NUMBER() OVER (ORDER BY
											 CASE WHEN (@SortField = 1 AND @SortDiriection = 1)
												       THEN employee.EmployeeCardId
											 END ASC, 
											 CASE WHEN (@SortField = 1 AND @SortDiriection = 2)
												       THEN employee.EmployeeCardId
											 END DESC,
											 CASE WHEN (@SortField = 2 AND @SortDiriection = 1)
												       THEN employee.Name 
											 END ASC, 
											 CASE WHEN (@SortField = 2 AND @SortDiriection = 2)
												       THEN employee.Name
											 END DESC,
											 CASE WHEN (@SortField = 3 AND @SortDiriection = 1)
												       THEN gender.Title
											 END ASC, 
											 CASE WHEN (@SortField = 3 AND @SortDiriection = 2)
												       THEN gender.Title 
											 END DESC,
											  CASE WHEN (@SortField = 4 AND @SortDiriection = 1)
												       THEN company.Name
											 END ASC, 
											 CASE WHEN (@SortField =4 AND @SortDiriection = 2)
												       THEN company.Name
											 END DESC,
											  CASE WHEN (@SortField = 5 AND @SortDiriection = 1)
												       THEN branch.Name
											 END ASC, 
											 CASE WHEN (@SortField =5 AND @SortDiriection = 2)
												       THEN branch.Name
											 END DESC,
											  CASE WHEN (@SortField =6 AND @SortDiriection = 1)
												       THEN unit.Name
											 END ASC, 
											 CASE WHEN (@SortField =6 AND @SortDiriection = 2)
												       THEN unit.Name
											 END DESC,
											  CASE WHEN (@SortField = 7 AND @SortDiriection = 1)
												       THEN department.Name
											 END ASC, 
											 CASE WHEN (@SortField =7 AND @SortDiriection = 2)
												       THEN department.Name
											 END DESC,
											  CASE WHEN (@SortField = 8 AND @SortDiriection = 1)
												       THEN section.Name
											 END ASC, 
											 CASE WHEN (@SortField = 8 AND @SortDiriection = 2)
												       THEN section.Name
											 END DESC,
											  CASE WHEN (@SortField = 9 AND @SortDiriection = 1)
												       THEN line.Name
											 END ASC, 
											 CASE WHEN (@SortField = 9 AND @SortDiriection = 2)
												       THEN line.Name
											 END DESC,
											 CASE WHEN (@SortField = 10 AND @SortDiriection = 1)
												       THEN employeeDesignation.Title
											 END ASC, 
											 CASE WHEN (@SortField =10 AND @SortDiriection = 2)
												       THEN employeeDesignation.Title
											 END DESC,
											 CASE WHEN (@SortField = 11 AND @SortDiriection = 1)
												       THEN employee.[Status]
											 END ASC, 
											 CASE WHEN (@SortField =11 AND @SortDiriection = 2)
												       THEN employee.[Status]
											 END DESC
									  ) AS RowNumber,										
					employee.EmployeeId AS EmployeeId,
					employee.EmployeeCardId AS [Employee Card No],
					employee.Name AS Name,
					employee.DateOfBirth AS [DOB],
					gender.Title AS [Gender],
					employee.[Status] AS [Status],
					employeePresentAddress.MobilePhone AS [Mobile],
					company.Name AS [Company],
					branch.Name AS [Branch],
					unit.Name AS [Unit],
					department.Name AS [Department],
					section.Name AS [Section],
					line.Name AS [Line],
					employeeDesignation.Title AS [Designation],
					employeeCompanyInfo.FromDate AS [Effective From]
					FROM Employee AS  employee
					LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
					ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
					FROM EmployeeCompanyInfo AS employeeCompanyInfo 
					WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
					ON employee.EmployeeId = employeeCompanyInfo.EmployeeId
					LEFT JOIN EmployeePresentAddress AS employeePresentAddress  ON employee.EmployeeId = employeePresentAddress.EmployeeId
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
					LEFT JOIN Gender gender ON employee.GenderId = gender.GenderId
					WHERE (employee.IsActive = 1) 
					AND employeeCompanyInfo.rowNum = 1 
					AND ((employeePresentAddress.[Status] = 1) AND (employeePresentAddress.IsActive=1))
					AND company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds)
					AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
					AND branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
					AND branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
					AND ((departmentSection.DepartmentSectionId = @DepartmentSectionId) OR (@DepartmentSectionId IS NULL))
					AND ((departmentLine.DepartmentLineId = @DepartmentLineId) OR (@DepartmentLineId IS NULL))
					AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
					AND ((employeeDesignation.Id = @EmployeeDesignationID) OR (@EmployeeDesignationID IS NULL))
					AND ((employeeGrade.Id = @EmployeeGradeID) OR (@EmployeeGradeID IS NULL))
					AND ((employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId IS NULL))
					AND ((employee.Name LIKE '%' + @EmployeeName + '%') OR (@EmployeeName IS NULL))
					AND ((employeePresentAddress.MobilePhone LIKE '%' + @EmployeeMobilePhone + '%') OR (@EmployeeMobilePhone IS NULL))
					AND ((gender.GenderId = @GenderId) OR (@GenderId IS NULL)) 
					AND ((employee.[Status] = @EmployeeStatus) OR (@EmployeeStatus IS NULL))
	
	SELECT @RowCount = COUNT(*) FROM @ReturnTable

	SELECT * FROM @ReturnTable
	WHERE RowID BETWEEN (@StartRowIndex * @MaxRows) + 1 AND ((@StartRowIndex+1) * @MaxRows)

END




