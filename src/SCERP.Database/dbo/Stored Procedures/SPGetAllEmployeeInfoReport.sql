
-- =============================================================================================================================================================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <04/10/2016>
-- Description:	<> EXEC [SPGetAllEmployeeInfoReport]  1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, '1900-01-01', '1900-01-01', '1900-01-01', '1900-01-01', '1900-01-01', '1900-01-01', -1, '1900-01-01', '1900-01-01', -1, -1, -1, '6586', '','', 1, 'superadmin', '2018-06-04'
-- =============================================================================================================================================================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetAllEmployeeInfoReport]	

			@CompanyId						INT = -1,
			@BranchId						INT = -1,
		    @BranchUnitId					INT = -1,		
			@BranchUnitDepartmentId			INT = -1,
			@DepartmentSectionId			INT = -1,
			@DepartmentLineId				INT = -1,
			@EmployeeTypeId					INT = -1,
			@EmployeeGradeId				INT = -1,
			@EmployeeDesignationId			INT = -1,
			@BloodGroupId					INT = -1,
			@GenderId						INT = -1,	
			@ReligionId						INT = -1,		
			@MaritalStateId					INT = -1,		
				
			
			@JoiningDateBegin				DATETIME = NULL,
			@JoiningDateEnd					DATETIME = NULL,
			@ConfirmationDateBegin			DATETIME = NULL,
			@ConfirmationDateEnd			DATETIME = NULL,
			@QuitDateBegin					DATETIME = NULL,
			@QuitDateEnd					DATETIME = NULL,	
			@BirthDayMonth				    INT = -1,		
			@MariageAnniversaryDateBegin	DATETIME = NULL,
			@MariageAnniversaryDateEnd		DATETIME = NULL,

			@PermanentCountryId				INT = -1,
			@PermanentDistrictId			INT = -1,		
			@EducationLevelId				INT = -1,
			
			@EmployeeCardId					NVARCHAR(100) = '',
			@EmployeeName					NVARCHAR(100) = '',
			@MobileNo						NVARCHAR(100) = '',

			@ActiveStatus					INT = -1,
			
			@UserName						NVARCHAR(100) = '',
	        @FromDate					    DATETIME = NULL
			
AS
BEGIN
	
			SET NOCOUNT ON;
	
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

			IF(CAST(@ConfirmationDateBegin AS DATE) = '1900-01-01')
				SET @ConfirmationDateBegin = NULL;

			IF(CAST(@ConfirmationDateEnd AS DATE) = '1900-01-01')
				SET @ConfirmationDateEnd =NULL;

			IF(CAST(@QuitDateBegin AS DATE) = '1900-01-01')
				SET @QuitDateBegin = NULL;

			IF(CAST(@QuitDateEnd AS DATE) = '1900-01-01')
				SET @QuitDateEnd =NULL;

			IF(CAST(@MariageAnniversaryDateBegin AS DATE) = '1900-01-01')
				SET @MariageAnniversaryDateBegin = NULL;

			IF(CAST(@MariageAnniversaryDateEnd AS DATE) = '1900-01-01')
				SET @MariageAnniversaryDateEnd =NULL;

			IF(CAST(@FromDate AS DATE) = '1900-01-01')
				SET @FromDate =NULL;

			
						
	  SELECT 
			 @CompanyName					AS CompanyName
			,@CompanyAddress				AS CompanyAddress
			,@BranchName					AS BranchName
			,@UnitName						AS UnitName
			,@DepartmentName				AS DepartmentName
			,@SectionName					AS SectionName
			,@LineName						AS LineName
			,@EmployeeTypeName              AS EmployeeTypeName		
				
	        ,Employee.EmployeeCardId		AS EmployeeCardId
			,EmployeeCompanyInfo.PunchCardNo AS PunchCardNo
			,Employee.Name					AS EmployeeName	
			
			,Company.Name					AS Company
			,Branch.Name					AS Branch
			,Unit.Name						AS Unit
			,Department.Name				AS Department
			,Section.Name					AS Section
			,Line.Name						AS Line
			,EmployeeType.Title				AS EmployeeType
			,EmployeeGrade.Name				AS EmployeeGrade
			,EmployeeDesignation.Title		AS EmployeeDesignation
			,Employee.JoiningDate AS JoiningDate  
			,Employee.ConfirmationDate AS ConfirmationDate  
			,Employee.QuitDate AS QuitDate  
			,qt.Type AS QuitType
			,employeeSalaryInfo.BasicSalary AS BasicSalary	
			,employeeSalaryInfo.GrossSalary AS GrossSalary						
			,ActiveStatus=
				CASE
					WHEN Employee.Status = 1 THEN 'Active'
					WHEN Employee.Status = 2 THEN 'Inactive'
				END					
			,Employee.MothersName			AS MothersName
			,Employee.FathersName			AS FathersName
			,Gender.Title					AS GenderName	
			,Employee.DateOfBirth           AS BirthDate  	
			,PresentAddress.MobilePhone     AS MobilePhone
			,BloodGroup.GroupName			AS BloodGroup		
			,Religion.Name					AS ReligionName
			,MaritalState.Title				AS MaritalState			 
			,Employee.MariageAnniversary    AS MarriageAnniversaryDate   

			,Country.CountryName			AS CountryName
			,DIST.Name						AS DistrictName
			,Employee.NationalIdNo			AS NationalIdNo
			,Employee.BirthRegistrationNo	AS BirthRegistrationNo
			,Employee.TaxIdentificationNo	AS TaxIdentificationNo
			,EducationLevel.Title			AS EducationLevel

			,(SELECT TOP (1) FromDate
				FROM EmployeeSalary
				WHERE (EmployeeId = employee.EmployeeId) AND CAST(FromDate AS DATE) <> CAST(employee.JoiningDate AS DATE) AND (IsActive = 1)
				ORDER BY FromDate DESC) AS LastIncrementDate

			,SkillSet.Title AS SkillType
			
			,ISNULL((SELECT TOP (1) GrossSalary		-- (Last Gross - Second Last Gross)
				FROM EmployeeSalary
				WHERE (EmployeeId = employee.EmployeeId) AND CAST(FromDate AS DATE) <> CAST(employee.JoiningDate AS DATE) AND (IsActive = 1)
				ORDER BY FromDate DESC), 0) - ISNULL((SELECT TOP(1) GrossSalary FROM 
												(SELECT TOP(2) GrossSalary,FromDate FROM EmployeeSalary 
												WHERE IsActive = 1 AND EmployeeId = employee.EmployeeId
												ORDER BY FromDate DESC) AS SalaryT 
												ORDER BY SalaryT.FromDate), 0)      AS LastIncrementAmount	
	
			,PresentAddress.MailingAddress +', '+ policeStation.Name +', '+ district.Name PresentAddress
			,PermanentAddress.MailingAddress +', '+ PSTE.Name + ', '+ DIST.Name PermanentAddress

			FROM Employee LEFT JOIN
			EmployeePresentAddress presentAddress ON Employee.EmployeeId = presentAddress.EmployeeId AND presentAddress.IsActive = 1 AND presentAddress.Status = 1  LEFT JOIN
			EmployeePermanentAddress permanentAddress ON Employee.EmployeeId = permanentAddress.EmployeeId AND permanentAddress.IsActive = 1 AND permanentAddress.Status = 1 LEFT JOIN
			District district ON presentAddress.DistrictId = district.Id AND district.IsActive = 1 LEFT JOIN
			PoliceStation policeStation ON presentAddress.PoliceStationId = policeStation.Id AND policeStation.IsActive = 1 LEFT JOIN 
			District DIST ON permanentAddress.DistrictId = DIST.Id AND DIST.IsActive = 1  LEFT JOIN
			PoliceStation PSTE ON permanentAddress.PoliceStationId = PSTE.Id AND PSTE.IsActive = 1  LEFT JOIN		
			
			(SELECT EmployeeId, PunchCardNo, FromDate,JobTypeId, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
			FROM EmployeeCompanyInfo AS employeeCompanyInfo
			WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL))
			AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
			ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1  LEFT JOIN   
			
			SkillSet ON SkillSet.Id = employeeCompanyInfo.JobTypeId AND SkillSet.IsActive = 1 LEFT JOIN
                                            
			EmployeeDesignation ON EmployeeCompanyInfo.DesignationId = EmployeeDesignation.Id AND EmployeeDesignation.IsActive = 1 LEFT JOIN
			EmployeeType ON EmployeeDesignation.EmployeeTypeId = EmployeeType.Id AND EmployeeType.IsActive = 1 LEFT JOIN
			EmployeeGrade ON EmployeeDesignation.GradeId = EmployeeGrade.Id AND EmployeeGrade.IsActive = 1  
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
			LEFT JOIN Line line ON departmentLine.LineId = line.LineId LEFT JOIN
			BloodGroup ON BloodGroup.Id = Employee.BloodGroupId AND BloodGroup.IsActive = 1 LEFT JOIN
			Gender ON Gender.GenderId = Employee.GenderId AND Gender.IsActive = 1 LEFT JOIN
			Religion ON Religion.ReligionId = Employee.ReligionId AND Religion.IsActive = 1 LEFT JOIN
			MaritalState ON MaritalState.MaritalStateId = Employee.MaritalStateId AND MaritalState.IsActive = 1 LEFT JOIN
			Country ON Country.Id = permanentAddress.CountryId AND Country.IsActive = 1 LEFT JOIN

			(SELECT EmployeeEducation.EducationLevelId, EmployeeEducation.EmployeeId, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY EmployeeEducation.EducationLevelId DESC) AS rowNum					
			FROM EmployeeEducation
			WHERE  EmployeeEducation.IsActive = 1) HighestEducationLevel
			ON Employee.EmployeeId = HighestEducationLevel.EmployeeId AND HighestEducationLevel.rowNum = 1
			LEFT OUTER JOIN EducationLevel ON EducationLevel.Id = HighestEducationLevel.EducationLevelId LEFT JOIN

			(SELECT EmployeeId,BasicSalary,GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						
			FROM EmployeeSalary AS employeeSalary
			WHERE ((CAST(employeeSalary.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL))
			AND employeeSalary.IsActive = 1) employeeSalaryInfo 
			ON employee.EmployeeId = employeeSalaryInfo.EmployeeId AND employeeSalaryInfo.rowNumSal = 1

			LEFT JOIN QuitType qt
			on employee.QuitTypeId = qt.QuitTypeId		

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
			AND (Employee.BloodGroupId = @BloodGroupId OR @BloodGroupId = -1)
			AND (Employee.GenderId = @GenderId OR @GenderId =-1)
			AND (Employee.ReligionId = @ReligionId OR @ReligionId = -1)
			AND (Employee.MaritalStateId = @MaritalStateId OR @MaritalStateId = -1)
			AND (Month(Employee.DateOfBirth) = @BirthDayMonth OR @BirthDayMonth = -1)
			AND (CAST(Employee.JoiningDate AS date) >= CAST(@JoiningDateBegin AS date) OR @JoiningDateBegin IS NULL)
			AND (CAST(Employee.JoiningDate AS date) <= CAST(@JoiningDateEnd AS date) OR @JoiningDateEnd IS NULL)
			AND (CAST(Employee.ConfirmationDate AS date) >= CAST(@ConfirmationDateBegin AS date) OR @ConfirmationDateBegin IS NULL)
			AND (CAST(Employee.ConfirmationDate AS date) <= CAST(@ConfirmationDateEnd AS date) OR @ConfirmationDateEnd IS NULL)
			AND (CAST(Employee.MariageAnniversary AS date) >= CAST(@MariageAnniversaryDateBegin AS date) OR @MariageAnniversaryDateBegin IS NULL)
			AND (CAST(Employee.MariageAnniversary AS date) <= CAST(@MariageAnniversaryDateEnd AS date) OR @MariageAnniversaryDateEnd IS NULL)
			AND (CAST(Employee.QuitDate AS date) >= CAST(@QuitDateBegin AS date) OR @QuitDateBegin IS NULL)
			AND (CAST(Employee.QuitDate AS date) <= CAST(@QuitDateEnd AS date) OR @QuitDateEnd IS NULL)
			AND ((permanentAddress.CountryId = @PermanentCountryId) OR (@PermanentCountryId = -1))
			AND ((permanentAddress.DistrictId = @PermanentDistrictId) OR (@PermanentDistrictId = -1))
			AND ((HighestEducationLevel.EducationLevelId = @EducationLevelId) OR (@EducationLevelId  =- 1))
			AND ((employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId =''))
			AND ((employee.Name LIKE '%' + @EmployeeName + '%') OR (@EmployeeName =''))
			AND ((presentAddress.MobilePhone LIKE '%' + @MobileNo + '%') OR (@MobileNo =''))
			AND ((employee.[Status] = @ActiveStatus) OR (@ActiveStatus = -1))
			AND (employee.IsActive = 1)
			AND (presentAddress.IsActive = 1)
			AND (presentAddress.[Status] = 1)
			AND (employeeCompanyInfo.rowNum = 1)
			AND (employeeSalaryInfo.rowNumSal = 1))			 				   		
			ORDER BY employee.EmployeeCardId
			
END






