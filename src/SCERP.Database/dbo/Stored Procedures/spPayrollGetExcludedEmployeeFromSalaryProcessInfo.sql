
-- ==================================================================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-11-22>
-- Description:	<> EXEC [spPayrollGetExcludedEmployeeFromSalaryProcessInfo] 1, 1, 1, NULL, NULL, NULL, NULL, '', 2016, 4, '2016-03-26', '2016-04-25', 'superadmin', 0, 10, 1, 1, 10
-- ==================================================================================================================================================================================

CREATE PROCEDURE [dbo].[spPayrollGetExcludedEmployeeFromSalaryProcessInfo]

	   @CompanyId INT = NULL,
	   @BranchId INT = NULL,
	   @BranchUnitId INT = NULL,
	   @BranchUnitDepartmentId INT = NULL, 
	   @DepartmentSectionId INT = NULL, 
	   @DepartmentLineId INT = NULL, 
	   @EmployeeTypeId INT = NULL,
	   @EmployeeCardId NVARCHAR(100) = '',
	   @Year INT,
	   @Month INT,
	   @FromDate DateTime,
	   @ToDate DateTime,	   
	   @UserName NVARCHAR(100),

	   @StartRowIndex INT = NULL,
	   @MaxRows INT = NULL,
	   @SortField INT = NULL,
	   @SortDiriection INT = NULL,
	   @RowCount INT OUTPUT

	   
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

		IF(@CompanyId IS NULL)
		BEGIN
			INSERT INTO @ListOfCompanyIds
			SELECT DISTINCT CompanyId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfCompanyIds VALUES (@CompanyId)
		END

		IF(@BranchId IS NULL)
		BEGIN
			INSERT INTO @ListOfBranchIds
			SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchIds VALUES (@BranchId)
		END

		IF(@BranchUnitId IS NULL)
		BEGIN
			INSERT INTO @ListOfBranchUnitIds
			SELECT DISTINCT BranchUnitId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchUnitIds VALUES (@BranchUnitId)
		END

		IF(@BranchUnitDepartmentId IS NULL)
		BEGIN
			INSERT INTO @ListOfBranchUnitDepartmentIds
			SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchUnitDepartmentIds VALUES (@BranchUnitDepartmentId)
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

		IF (@DepartmentSectionId  IS NULL)
		BEGIN
			SET @DepartmentSectionId = NULL
		END

		IF (@DepartmentLineId  IS NULL)
		BEGIN
			SET @DepartmentLineId = NULL
		END

		
		DECLARE @EmployeeId UNIQUEIDENTIFIER = NULL;
		IF (@EmployeeCardId  = '')
		BEGIN
			SET @EmployeeCardId = NULL
			SET @EmployeeId = NULL
		END
		ELSE
		BEGIN
			SELECT @EmployeeId = EmployeeId FROM Employee
			WHERE EmployeeCardId = @EmployeeCardId
			AND IsActive = 1
		END

		DECLARE @ReturnTable TABLE (
								     RowID BIGINT NULL
									,[ExcludedEmployeeFromSalaryProcessId] INT
									,[EmployeeCardId] NVARCHAR(100)
									,[EmployeeId] UNIQUEIDENTIFIER
									,[Name] NVARCHAR(100) NULL
									,[JoiningDate] DATETIME NULL
									,[QuitDate] DATETIME NULL
									,[Company] NVARCHAR(100) NULL
									,[Branch] NVARCHAR(100) NULL
									,[Unit] NVARCHAR(100) NULL
									,[Department] NVARCHAR(100) NULL
									,[Section] NVARCHAR(100) NULL
									,[Line] NVARCHAR(100) NULL
									,[EmployeeType] NVARCHAR(100) NULL
									,[Designation] NVARCHAR(100) NULL
									,[Month] INT
									,[Year] INT
									,[FromDate] DATETIME NULL
									,[ToDate] DATETIME NULL
									,[Remarks] NVARCHAR(MAX)
									,[CreatedDate] DATETIME  NULL
									,[CreatedBy] UNIQUEIDENTIFIER NULL
									,[IsActive] BIT
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
												       THEN company.Name
											 END ASC, 
											 CASE WHEN (@SortField = 3 AND @SortDiriection = 2)
												       THEN company.Name
											 END DESC,
											  CASE WHEN (@SortField = 4 AND @SortDiriection = 1)
												       THEN branch.Name
											 END ASC, 
											 CASE WHEN (@SortField = 4 AND @SortDiriection = 2)
												       THEN branch.Name
											 END DESC,
											  CASE WHEN (@SortField = 5 AND @SortDiriection = 1)
												       THEN unit.Name
											 END ASC, 
											 CASE WHEN (@SortField = 5 AND @SortDiriection = 2)
												       THEN unit.Name
											 END DESC,
											  CASE WHEN (@SortField = 6 AND @SortDiriection = 1)
												       THEN department.Name
											 END ASC, 
											 CASE WHEN (@SortField = 6 AND @SortDiriection = 2)
												       THEN department.Name
											 END DESC,
											  CASE WHEN (@SortField = 7 AND @SortDiriection = 1)
												       THEN section.Name
											 END ASC, 
											 CASE WHEN (@SortField = 7 AND @SortDiriection = 2)
												       THEN section.Name
											 END DESC,
											  CASE WHEN (@SortField = 8 AND @SortDiriection = 1)
												       THEN line.Name
											 END ASC, 
											 CASE WHEN (@SortField = 8 AND @SortDiriection = 2)
												       THEN line.Name
											 END DESC,
											 CASE WHEN (@SortField = 9 AND @SortDiriection = 1)
												       THEN employeeDesignation.Title
											 END ASC, 
											 CASE WHEN (@SortField = 9 AND @SortDiriection = 2)
												       THEN employeeDesignation.Title
											 END DESC
											
									  ) AS RowNumber	
						,[ExcludedEmployeeFromSalaryProcessId]							
						,peefsp.[EmployeeCardId] 
						,peefsp.[EmployeeId] 
						,employee.Name
						,employee.JoiningDate
						,employee.QuitDate
						,company.Name
						,branch.Name 
						,unit.Name
						,department.Name 
						,section.Name
						,line.Name
						,employeeType.Title
						,employeeDesignation.Title
						,peefsp.[Month] 
						,peefsp.[Year] 
						,peefsp.[FromDate] 
						,peefsp.[ToDate] 
						,peefsp.[Remarks]
						,peefsp.[CreatedDate]
						,peefsp.[CreatedBy] 
						,peefsp.[IsActive]
						
			FROM	[dbo].[PayrollExcludedEmployeeFromSalaryProcess] peefsp
					LEFT JOIN  Employee AS  employee ON peefsp.EmployeeId = employee.EmployeeId
					LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
					ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
					FROM EmployeeCompanyInfo AS employeeCompanyInfo 
					WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @ToDate) OR (@ToDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
					ON employee.EmployeeId = employeeCompanyInfo.EmployeeId
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
		    WHERE   peefsp.[Year] = @Year
					AND peefsp.[Month] = @Month
					AND ((CAST(peefsp.FromDate AS DATE) = CAST(@FromDate AS DATE)) OR (@FromDate IS NULL))
					AND ((CAST(peefsp.ToDate AS DATE) = CAST(@ToDate AS DATE)) OR (@ToDate IS NULL))
					AND (employee.IsActive = 1) 
					AND employeeCompanyInfo.rowNum = 1 
					AND company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds)
					AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
					AND branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
					AND branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
					AND ((departmentSection.DepartmentSectionId = @DepartmentSectionId) OR (@DepartmentSectionId IS NULL))
					AND ((departmentLine.DepartmentLineId = @DepartmentLineId) OR (@DepartmentLineId IS NULL))
					AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
					AND ((employee.EmployeeId = @EmployeeId) OR (@EmployeeId IS NULL))
					AND ((employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId IS NULL))
					AND peefsp.IsActive = 1

					ORDER BY ExcludedEmployeeFromSalaryProcessId ASC	

				SELECT @RowCount = COUNT(*) FROM @ReturnTable

				SELECT * FROM @ReturnTable
				WHERE RowID BETWEEN (@StartRowIndex * @MaxRows) + 1 AND ((@StartRowIndex+1) * @MaxRows)

		COMMIT TRAN


END






