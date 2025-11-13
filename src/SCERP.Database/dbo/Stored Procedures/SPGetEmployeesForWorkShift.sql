
-- ==========================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <22-Sep-15 2:09:40 PM>
-- Description:	<> EXEC [SPGetEmployeesForWorkShift] NULL, 1, 1, 1, NULL, NULL,NULL, NULL, NULL, NULL, NULL,'superadmin'
-- ==========================================================================================================================


CREATE PROCEDURE [dbo].[SPGetEmployeesForWorkShift]
						 @EmployeeCardId			NVARCHAR(100) = NULL
						,@CompanyId		            INT
						,@BranchId	      	        INT
						,@BranchUnitId		        INT
						,@BranchUnitDepartmentId    INT = NULL
						,@DepartmentSectionId		INT = NULL
						,@DepartmentLineId			INT = NULL
						,@WorkGroupId				INT = NULL
						,@BranchUnitWorkShiftId		INT = NULL
						,@EmployeeTypeId			INT = NULL
						,@CheckDate				    DATETIME = NULL
						,@UserName					NVARCHAR(100)

AS
BEGIN
	
	SET NOCOUNT ON;


			DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
			DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
			DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
			DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);
			DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);

			INSERT INTO @ListOfCompanyIds VALUES (@CompanyID)
			INSERT INTO @ListOfBranchIds VALUES (@BranchID)		
			INSERT INTO @ListOfBranchUnitIds VALUES (@BranchUnitID)
	
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


			IF(@CheckDate IS NULL)
			BEGIN
				SET @CheckDate = CAST(CURRENT_TIMESTAMP AS DATE)
			END
			ELSE
			BEGIN
				SET @CheckDate = CAST(@CheckDate AS DATE)
			END


	       DECLARE @EmployeeForWorkShiftTempTable TABLE (
								EmployeeId UNIQUEIDENTIFIER NULL,
								EmployeeCardNo NVARCHAR(100) NULL,
						        EmployeeName NVARCHAR(100) NULL,
								UNIT NVARCHAR(100) NULL,
								Department NVARCHAR(100) NULL,
								Section NVARCHAR(100) NULL,
								Line NVARCHAR(100) NULL,
								EmployeeType NVARCHAR(100) NULL,
								Grade NVARCHAR(100) NULL,
								Designation NVARCHAR(100) NULL,
								JoiningDate DATETIME NULL,
								WorkGroup NVARCHAR(100) NULL,
								WorkGroupAssignedDate DATETIME NULL								
								);
			
			INSERT INTO @EmployeeForWorkShiftTempTable
			SELECT		  
						  Employee.EmployeeId AS EmployeeId
						 ,Employee.EmployeeCardId AS EmployeeCardId
						 ,Employee.Name AS EmployeeName
						 ,Unit.Name AS UNIT
						 ,Department.Name AS Department									
						 ,Section.Name AS Section
						 ,Line.Name AS Line	
						 ,employeeType.Title  AS EmployeeType
						 ,EmployeeGrade.Name AS EmployeeGrade
						 ,EmployeeDesignation.Title AS EmployeeDesignation		
						 ,employee.JoiningDate	
						 ,workGroup.Name AS WorkGroup
						 ,employeeWorkGroup.AssignedDate AS WorkGroupAssignedDate
						
						 					
			FROM		 Employee employee
						 
						 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @CheckDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
						 ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

						 LEFT JOIN (SELECT EmployeeId,  WorkGroupId, AssignedDate,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY AssignedDate DESC) AS rowNumWG
						 FROM EmployeeWorkGroup AS employeeWorkGroup 
					     WHERE (CAST(employeeWorkGroup.AssignedDate AS Date) <= @CheckDate) AND employeeWorkGroup.IsActive=1) employeeWorkGroup 
					     ON employee.EmployeeId = employeeWorkGroup.EmployeeId
						 LEFT JOIN WorkGroup workGroup ON employeeWorkGroup.WorkGroupId = workGroup.WorkGroupId
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

						 WHERE      
								 company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds) 
								 AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
								 AND branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
								 AND branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
								 AND ((departmentSection.DepartmentSectionId = @DepartmentSectionId) OR (@DepartmentSectionId IS NULL))
								 AND ((departmentLine.DepartmentLineId = @DepartmentLineId) OR (@DepartmentLineId IS NULL))
								 AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
								 AND ((Employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId IS NULL))
								 AND ((employeeWorkGroup.WorkGroupId = @WorkGroupId) OR (@WorkGroupId IS NULL))
								 AND (employee.IsActive = 1)
								 AND (employee.[Status] = 1)
						 ORDER BY Employee.EmployeeCardId

						 IF(@CheckDate IS NULL AND @BranchUnitWorkShiftId IS NULL)
						 BEGIN
							SELECT ewsTempTable.EmployeeId, 
								   ewsTempTable.EmployeeCardNo,
							       ewsTempTable.EmployeeName, 
								   ewsTempTable.Department, 
								   ewsTempTable.Section, 
								   ewsTempTable.Line, 
								   ewsTempTable.EmployeeType, 
								   ewsTempTable.Grade, 
								   ewsTempTable.Designation,
								   ewsTempTable.JoiningDate,
								   ewsTempTable.WorkGroup, 
								   ewsTempTable.WorkGroupAssignedDate
							FROM @EmployeeForWorkShiftTempTable ewsTempTable 
							ORDER BY ewsTempTable.EmployeeCardNo ASC
						 END
						 ELSE
						 BEGIN						 
							SELECT ewsTempTable.EmployeeId, 
								   ewsTempTable.EmployeeCardNo,
							       ewsTempTable.EmployeeName, 
								   ewsTempTable.Department, 
								   ewsTempTable.Section, 
								   ewsTempTable.Line, 
								   ewsTempTable.EmployeeType, 
								   ewsTempTable.Grade, 
								   ewsTempTable.Designation,
								   ewsTempTable.JoiningDate,
								   ewsTempTable.WorkGroup, 
								   ewsTempTable.WorkGroupAssignedDate
							FROM @EmployeeForWorkShiftTempTable ewsTempTable 
							WHERE ewsTempTable.EmployeeId NOT IN 
							(
								SELECT EmployeeId FROM EmployeeWorkShift employeeWorkShift
								WHERE ((CAST(employeeWorkShift.ShiftDate AS DATE) = @CheckDate)
								AND  (employeeWorkShift.BranchUnitWorkShiftId = @BranchUnitWorkShiftId)
								AND  (employeeWorkShift.IsActive = 1)
								AND  (employeeWorkShift.[Status] = 1))
							)
							AND CAST(ewsTempTable.JoiningDate AS DATE) <= Cast(@CheckDate AS DATE) 
							ORDER BY ewsTempTable.EmployeeCardNo ASC
						 END
END


