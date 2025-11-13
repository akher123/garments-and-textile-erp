
-- ==================================================================================================================================================================
-- Author	   :	Yasir Arafat
-- Create date :    <26/09/2016>
-- Description :    EXEC [SPGetMaternityLeaveInfo]  1,  1,  1, -1, -1, -1,'0155','2016-06-26','2016-11-25','superadmin'
-- ==================================================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetMaternityLeaveInfo]

				@CompanyId INT = -1,
				@BranchId INT = -1,
				@BranchUnitId INT = -1,
				@BranchUnitDepartmentId INT = -1,
				@DepartmentSectionId INT = -1,
				@DepartmentLineId INT = -1,
				@EmployeeCardId NVARCHAR(100) = '',				
				@FromDate DATETIME,
				@ToDate DATETIME,											
				@UserName NVARCHAR(100)

AS
BEGIN
	
			SET NOCOUNT ON;
		
			DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
			DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
			DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
			DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);


			DECLARE @CompanyName NVARCHAR(100) = ''; 
			DECLARE @BranchName NVARCHAR(100) = ''; 
			DECLARE @UnitName NVARCHAR(100) = ''; 
			DECLARE @DepartmentName NVARCHAR(100) = ''; 
			DECLARE @SectionName NVARCHAR(100) = '';
			DECLARE @LineName NVARCHAR(100) = '';
			DECLARE @CompanyAddress NVARCHAR(100) = ''; 


			IF(@CompanyId = -1)
			BEGIN
				INSERT INTO @ListOfCompanyIds
				SELECT DISTINCT CompanyId FROM UserPermissionForDepartmentLevel
				WHERE UserName = @UserName;

				SET @CompanyName = '';
				SET @CompanyAddress = '';
			END  
			ELSE
			BEGIN
				INSERT INTO @ListOfCompanyIds VALUES (@CompanyId)

				SELECT @CompanyName = company.Name, 
					   @CompanyAddress = company.FullAddress 
					   FROM Company  AS company 
					   WHERE company.Id = @CompanyId 
			END

			IF(@BranchId = -1)
			BEGIN
				INSERT INTO @ListOfBranchIds
				SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
				WHERE UserName = @UserName;

				SET @BranchName = ''
			END  
			ELSE
			BEGIN
				INSERT INTO @ListOfBranchIds VALUES (@BranchId)

				SELECT @BranchName = branch.Name FROM 
									 Branch  AS branch 
									 WHERE branch.Id = @BranchId 
			END

			IF(@BranchUnitId = -1)
			BEGIN
				INSERT INTO @ListOfBranchUnitIds
				SELECT DISTINCT BranchUnitId FROM UserPermissionForDepartmentLevel
				WHERE UserName = @UserName;

				SELECT @UnitName = '';
			END  
			ELSE
			BEGIN
				INSERT INTO @ListOfBranchUnitIds VALUES (@BranchUnitId)

				SELECT @UnitName = unit.Name FROM 
								   Unit  AS unit 
								   LEFT JOIN BranchUnit  AS branchUnit ON unit.UnitId = branchUnit.UnitId
								   WHERE branchUnit.BranchUnitId = @BranchUnitId 
			END

			IF(@BranchUnitDepartmentId = -1)
			BEGIN
				INSERT INTO @ListOfBranchUnitDepartmentIds
				SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
				WHERE UserName = @UserName;

				SELECT @DepartmentName = '';
			END  
			ELSE
			BEGIN
				INSERT INTO @ListOfBranchUnitDepartmentIds VALUES (@BranchUnitDepartmentId)

				SELECT @DepartmentName = department.Name FROM 
										 BranchUnitDepartment  AS branchUnitDepartment 
										 LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
										 LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
										 WHERE branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId 
			END
		
			IF (@DepartmentSectionId  = -1)
			BEGIN
				SET @DepartmentSectionId = NULL
				SET @SectionName = ''
			END
			ELSE
			BEGIN
				Select @SectionName = sec.Name FROM Section sec
										INNER JOIN DepartmentSection dsec
										ON sec.SectionId = dsec.SectionId
										WHERE dsec.DepartmentSectionId = @DepartmentSectionId
			END

			IF (@DepartmentLineId  = -1)
			BEGIN
				SET @DepartmentLineId = NULL
				SET @LineName = ''
			END
			ELSE
			BEGIN
				 Select @LineName = line.Name FROM Line line
									  INNER JOIN DepartmentLine dline
									  ON line.LineId = dline.LineId
									  WHERE dline.DepartmentLineId = @DepartmentLineId
			END
		
			IF (@EmployeeCardId  = '')
			BEGIN
				SET @EmployeeCardId = NULL
			END


			 SELECT									
			 MaternityPaymentId
			,HrmMaternityPayment.EmployeeId
		    ,Employee.EmployeeCardId
			,Employee.Name						AS EmployeeName
			,EmployeeDesignation.Title			AS Designation		
			,Department.Name					AS DepartmentName
			,Section.Name						AS SectionName
			,Line.Name							AS LineName
			,LeaveDayStart
			,LeaveDayEnd
			,FirstPaymentDate
			,FirstPaymentAmount
			,SecondPaymentDate
		    ,SecondPaymentAmount			
		    ,CompId
			  

		    FROM HrmMaternityPayment
		  
		    LEFT JOIN Employee employee ON Employee.EmployeeId = HrmMaternityPayment.EmployeeId
	
			LEFT JOIN (SELECT EmployeeId, FromDate, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, 
			ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
			FROM EmployeeCompanyInfo AS employeeCompanyInfo
			WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @fromDate) OR (@fromDate IS NULL))
			AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
			ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1 
                                            
			LEFT JOIN EmployeeDesignation ON EmployeeCompanyInfo.DesignationId = EmployeeDesignation.Id AND EmployeeDesignation.IsActive = 1 
			LEFT JOIN EmployeeType ON EmployeeDesignation.EmployeeTypeId = EmployeeType.Id AND EmployeeType.IsActive = 1 
			LEFT JOIN EmployeeGrade ON EmployeeDesignation.GradeId = EmployeeGrade.Id AND EmployeeGrade.IsActive = 1  
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
		  											 				 		
		    WHERE 
			HrmMaternityPayment.IsActive = 1
			AND Employee.IsActive = 1 
			--AND Employee.QuitDate IS NULL
			AND (company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds)
			AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
			AND branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
			AND branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
			AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
			AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL))						 			
			AND((employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId IS NULL))
									
END