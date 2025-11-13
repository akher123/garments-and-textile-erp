
-- =======================================================================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2016.05.25>
-- Description:	<> EXEC spHrmGetManpowerSkillReport 1, 1, 1, 6, -1, -1, -1, -1, 1, '1900-01-01', '1900-01-01', '1900-01-01', '1900-01-01', '1900-01-01', '1900-01-01','superadmin'
-- =======================================================================================================================================================================================

CREATE PROCEDURE [dbo].[spHrmGetManpowerSkillReport]	
	
				@CompanyId						INT = -1,
				@BranchId						INT = -1,
				@BranchUnitId					INT = -1,		
				@BranchUnitDepartmentId			INT = -1,
				@DepartmentSectionId			INT = -1,
				@DepartmentLineId				INT = -1,
				@EmployeeTypeId					INT = -1,				
				@EmployeeDesignationId			INT = -1,
				@GenderId						INT = -1,	
			
				@JoiningDateBegin				DATETIME = NULL,
				@JoiningDateEnd					DATETIME = NULL,
				@ConfirmationDateBegin			DATETIME = NULL,
				@ConfirmationDateEnd			DATETIME = NULL,
				@QuitDateBegin					DATETIME = NULL,
				@QuitDateEnd					DATETIME = NULL,
				
				@UserName						NVARCHAR(100) = ''		
				 
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

			DECLARE @FromDate DATETIME = CURRENT_TIMESTAMP

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
			

			 SELECT 
			 @CompanyName AS CompanyName
			,@CompanyAddress AS CompanyAddress
			,@BranchName AS BranchName
			,@UnitName AS UnitName
			,@DepartmentName AS DepartmentName
			,@SectionName AS SectionName
			,@LineName AS LineName
			,@EmployeeTypeName AS EmployeeTypeName
			,DeptID 
			,ISNULL(SecID,0) AS SecID
			,ISNULL(LineID,0) AS LineID
			,Department
			,Section  
			,Line
			,EmployeeType
						
			,ISNULL(SkillSet.Title, VEmployee.Designation) AS Designation	
				
			,COUNT(VEmployee.EmployeeID) AS TotalEmployee

			FROM VEmployee	LEFT JOIN

			(SELECT EmployeeId, JobTypeId, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
			FROM EmployeeCompanyInfo AS employeeCompanyInfo
			WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL))
			AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
			ON VEmployee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

			LEFT JOIN SkillSet ON SkillSet.Id = employeeCompanyInfo.JobTypeId AND SkillSet.IsActive = 1	

			WHERE 
			(CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds)
			AND BranchId IN (SELECT BranchIDs FROM @ListOfBranchIds)
			AND BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
			AND DeptID IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
			AND (SecID = @DepartmentSectionId OR @DepartmentSectionId = -1)
			AND (LineID = @DepartmentLineId OR @DepartmentLineId = -1)			 							
			AND EmployeeTypeId IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)		
			AND ((DesID = @EmployeeDesignationID) OR (@EmployeeDesignationID = -1))		
			AND (GenderId = @GenderId OR @GenderId =-1)
			AND (CAST(JoiningDate AS DATE) >= CAST(@JoiningDateBegin AS DATE) OR @JoiningDateBegin IS NULL)
			AND (CAST(JoiningDate AS DATE) <= CAST(@JoiningDateEnd AS DATE) OR @JoiningDateEnd IS NULL)
			AND (CAST(ConfirmationDate AS DATE) >= CAST(@ConfirmationDateBegin AS DATE) OR @ConfirmationDateBegin IS NULL)
			AND (CAST(ConfirmationDate AS DATE) <= CAST(@ConfirmationDateEnd AS DATE) OR @ConfirmationDateEnd IS NULL)
			AND (CAST(QuitDate AS DATE) >= CAST(@QuitDateBegin AS date) OR @QuitDateBegin IS NULL)
			AND (CAST(QuitDate AS DATE) <= CAST(@QuitDateEnd AS date) OR @QuitDateEnd IS NULL)
			AND (VEmployee.IsActive = 1) 
			AND ([Status] = 1))
			GROUP BY DeptID, SecID, LineID, Department, Section, Line, EmployeeType, ISNULL(SkillSet.Title, VEmployee.Designation)
			ORDER BY DeptID, SecID, LineID
																					 													
END




