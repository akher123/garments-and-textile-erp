
-- ==========================================================================================================================================================================================================
-- Author:		Golam Rabbi
-- Create date: 2015.12.15
-- ==========================================================================================================================================================================================================

-- EXEC spHrmGetEmployeeLeaveDetailReport -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,'1900-01-01','1900-01-01','1900-01-01','1900-01-01', '0835','',-1,'1900-01-01','1900-01-01', -1, '2016-05-30', 'superadmin'

-- ==========================================================================================================================================================================================================

CREATE PROCEDURE [dbo].[spHrmGetEmployeeLeaveDetailReport]
				@CompanyId						INT = -1,
				@BranchId						INT = -1,
				@BranchUnitId					INT = -1,		
				@BranchUnitDepartmentId			INT = -1,
				@DepartmentSectionId			INT = -1,
				@DepartmentLineId				INT = -1,
				@EmployeeTypeId					INT = -1,
				@EmployeeGradeId				INT = -1,
				@EmployeeDesignationId			INT = -1,
				@GenderId						INT = -1,	
				@JoiningDateBegin				DATETIME = '1900-01-01',
				@JoiningDateEnd					DATETIME = '1900-01-01',
				@QuitDateBegin					DATETIME = '1900-01-01',
				@QuitDateEnd					DATETIME = '1900-01-01',	
				@EmployeeCardId					NVARCHAR(100) = '',
				@EmployeeName					NVARCHAR(100) = '',
				@LeaveTypeId					INT = -1,						
				@ConsumedDateBegin				DATETIME = '1900-01-01',
				@ConsumedDateEnd				DATETIME = '1900-01-01',
				@ActiveStatus					INT = -1,
				@UpToDate					    DATETIME = '1900-01-01',
				@UserName						NVARCHAR(100) = ''	
				
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRAN

			DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
			DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
			DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
			DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);
			DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);

			DECLARE @CompanyName NVARCHAR(100) = ''; 
			DECLARE @CompanyAddress NVARCHAR(100) = ''; 
			DECLARE @BranchName NVARCHAR(100) = ''; 
			DECLARE @UnitName NVARCHAR(100) = ''; 
			DECLARE @DepartmentName NVARCHAR(100) = ''; 
			DECLARE @SectionName NVARCHAR(100) = '';
			DECLARE @LineName NVARCHAR(100) = '';
			DECLARE @EmployeeTypeName NVARCHAR(100) = '';

			-- Insert statements for procedure here
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
			   INSERT INTO @ListOfCompanyIds VALUES (@CompanyID)
			   
			   SELECT @CompanyName= comp.Name, 
					  @CompanyAddress = comp.FullAddress
			   FROM Company comp 
			   WHERE comp.Id = @companyID
			END

			IF(@BranchID = -1)
			BEGIN
			   INSERT INTO @ListOfBranchIds
			   SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
			   WHERE UserName = @UserName;

			   SET @BranchName = '';
			END  
			ELSE
			BEGIN
			   INSERT INTO @ListOfBranchIds VALUES (@BranchID)
			   
			   SELECT @BranchName= brnch.Name
			   FROM Branch brnch 
			   WHERE brnch.Id = @branchId
			END

			IF(@BranchUnitId=-1)
			BEGIN
			   INSERT INTO @ListOfBranchUnitIds
			   SELECT DISTINCT BranchUnitId FROM UserPermissionForDepartmentLevel
			   WHERE UserName = @UserName;

			   SET @UnitName = '';
			END  
			ELSE
			BEGIN
			   INSERT INTO @ListOfBranchUnitIds VALUES (@BranchUnitId)
			   
			   SELECT TOP(1) @UnitName= unit.Name
			   FROM Unit unit 
			   INNER JOIN BranchUnit bunit ON unit.UnitId = bunit.UnitId
			   WHERE bunit.BranchUnitId = @branchUnitId
			END

			IF(@BranchUnitDepartmentId =-1)
			BEGIN
			   INSERT INTO @ListOfBranchUnitDepartmentIds
			   SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
			   WHERE UserName = @UserName;

			   SET @DepartmentName = '';
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

			IF(@DepartmentSectionId  = -1)
			BEGIN
				SET @SectionName = ''			  
			END
			ELSE
			BEGIN
					Select @SectionName = sec.Name FROM Section sec
									INNER JOIN DepartmentSection dsec
									ON sec.SectionId = dsec.SectionId
									WHERE dsec.DepartmentSectionId = @DepartmentSectionId
			END
				
			IF(@DepartmentLineId = -1)
			BEGIN
				SET @LineName = ''				  
			END
			ELSE
			BEGIN
					Select @LineName = line.Name FROM Line line
									INNER JOIN DepartmentLine dline
									ON line.LineId = dline.LineId
									WHERE dline.DepartmentLineId = @DepartmentLineId
			END

			IF(@EmployeeTypeID = -1)
			BEGIN
			   INSERT INTO @ListOfEmployeeTypeIds
			   SELECT DISTINCT EmployeeTypeId FROM UserPermissionForEmployeeLevel
			   WHERE UserName = @UserName;

			   SET @EmployeeTypeName = '';
			END  
			ELSE
			BEGIN
			   INSERT INTO @ListOfEmployeeTypeIds VALUES (@EmployeeTypeID)

			   SELECT @EmployeeTypeName = et.Title FROM EmployeeType et
										  WHERE et.Id = @EmployeeTypeId
			END

			IF(CAST(@JoiningDateBegin AS DATE) = '1900-01-01')
				SET @JoiningDateBegin = NULL;

			IF(CAST(@JoiningDateEnd AS DATE) = '1900-01-01')
				SET @JoiningDateEnd =NULL;

			IF(CAST(@QuitDateBegin AS DATE) = '1900-01-01')
				SET @QuitDateBegin = NULL;

			IF(CAST(@QuitDateEnd AS DATE) = '1900-01-01')
				SET @QuitDateEnd =NULL;

			IF(CAST(@ConsumedDateBegin AS DATE) = '1900-01-01')
				SET @ConsumedDateBegin = NULL;

			IF(CAST(@ConsumedDateEnd AS DATE) = '1900-01-01')
				SET @ConsumedDateEnd =NULL;

			IF(CAST(@UpToDate AS DATE) = '1900-01-01')
				SET @UpToDate =NULL;	
			
			SELECT  
					 @CompanyName					AS CompanyName
					,@CompanyAddress				AS CompanyAddress
					,@BranchName					AS BranchName
					,@UnitName						AS UnitName
					,@DepartmentName				AS DepartmentName
					,@SectionName					AS SectionName
					,@LineName						AS LineName
					,@EmployeeTypeName              AS EmployeeTypeName	

				
			
					,employee.EmployeeCardId
					,employee.Name AS EmployeeName
				    ,department.Name				AS Department
					,section.Name					AS Section
					,line.Name						AS Line
					,employeeType.Title				AS EmployeeType
					,employeeDesignation.Title		AS EmployeeDesignation
					,eld.LeaveTypeTitle AS LeaveTitle
					,CAST(eld.ConsumedDate AS DATE) AS LeaveCosumedDate
					,CAST(eld.SubmitDate AS DATE) AS ApplicationSubmitDate
					,el.LeavePurpose
					,el.AddressDuringLeave
					,CAST(el.RecommendationStatusDate AS DATE) AS RecommendationDate
					,(SELECT Name FROM Employee WHERE EmployeeId = el.RecommendationPerson) AS RecommendationPerson
					,CAST(el.ApprovalStatusDate AS DATE) AS ApprovalDate
					,(SELECT Name FROM Employee WHERE EmployeeId = el.ApprovalPerson) AS ApprovalPerson
					
					FROM EmployeeLeaveDetail eld
					INNER JOIN EmployeeLeave el ON el.Id = eld.EmployeeLeaveId
					INNER JOIN Employee employee ON eld.EmployeeId = employee.EmployeeId
					LEFT JOIN
					(SELECT EmployeeId, PunchCardNo, FromDate,DesignationId,BranchUnitDepartmentId,DepartmentSectionId,DepartmentLineId, 
					 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
					FROM EmployeeCompanyInfo AS employeeCompanyInfo
					WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @UpToDate) OR (@UpToDate IS NULL))
					AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
					ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1  
	
					LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id AND employeeDesignation.IsActive = 1 
					LEFT JOIN EmployeeType AS employeeType ON employeeDesignation.EmployeeTypeId = employeeType.Id AND employeeType.IsActive = 1 
					LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id AND employeeGrade.IsActive = 1  
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
					(company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds)
					AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
					AND branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
					AND branchUnitDepartment.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
					AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId = -1)
					AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId = -1)			 							
					AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
					AND ((employeeGrade.Id = @EmployeeGradeID) OR (@EmployeeGradeID = -1))
					AND ((employeeDesignation.Id = @EmployeeDesignationID) OR (@EmployeeDesignationID = -1))
					AND (Employee.GenderId = @GenderId OR @GenderId =-1)
					AND ((CAST(Employee.JoiningDate AS date) >= CAST(@JoiningDateBegin AS date)) OR (@JoiningDateBegin IS NULL))
					AND ((CAST(Employee.JoiningDate AS date) <= CAST(@JoiningDateEnd AS date)) OR (@JoiningDateEnd IS NULL))
					AND ((CAST(Employee.QuitDate AS date) >= CAST(@QuitDateBegin AS date)) OR (@QuitDateBegin IS NULL))
					AND ((CAST(Employee.QuitDate AS date) <= CAST(@QuitDateEnd AS date)) OR (@QuitDateEnd IS NULL))
					AND ((employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId =''))
					AND ((employee.Name LIKE '%' + @EmployeeName + '%') OR (@EmployeeName =''))
					AND ((employee.[Status] = @ActiveStatus) OR (@ActiveStatus = -1))
					AND (employee.IsActive = 1)
					AND (employeeCompanyInfo.rowNum = 1)
					AND (eld.LeaveTypeId = @LeaveTypeId OR @LeaveTypeId = -1)
					AND ((CAST(eld.ConsumedDate AS date) >= CAST(@ConsumedDateBegin AS date)) OR (@ConsumedDateBegin IS NULL))
					AND ((CAST(eld.ConsumedDate AS date) <= CAST(@ConsumedDateEnd AS date)) OR (@ConsumedDateEnd IS NULL))
					AND eld.IsActive = 1)

					ORDER BY employee.EmployeeCardId ASC

	COMMIT TRAN

END
