
-- ==============================================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <31/03/2015>
-- Description:	<> exec SPShortLeaveDetail 1,1,1,1,NULL, NULL,'5146fd70-8cee-4022-a606-7cfafeb7874c', '05/19/2015','05/20/2015', null, 'superadmin'
-- ==============================================================================================================================================

CREATE PROCEDURE [dbo].[SPShortLeaveDetail]
						  @CompanyID INT = NULL,
						  @BranchID INT = NULL,
						  @BranchUnitID INT = NULL,
						  @BranchUnitDepartmentId INT = NULL,
						  @DepartmentSectionId INT = NULL,
						  @DepartmentLineId INT = NULL,
						  @EmployeeId UNIQUEIDENTIFIER,
						  @FromDate DATETIME,
						  @ToDate DATETIME,
						  @Reasontype TINYINT = NULL,
						  @UserName NVARCHAR(100)

AS
BEGIN
	
	SET NOCOUNT ON;

		DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
		DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
		DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
		DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);
		DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);
		
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


		INSERT INTO @ListOfEmployeeTypeIds
		SELECT DISTINCT EmployeeTypeId FROM UserPermissionForEmployeeLevel
		WHERE UserName = @UserName;

		Declare @StartDate Datetime = Current_timestamp

		EXEC SPGenerateTempDate @FromDate, @ToDate, @EmployeeId;
	  
		SELECT			  TempDate.Id
						 ,TempDate.EmployeeId
					     ,Employee.EmployeeCardId
						 ,Employee.Name 
						 ,EmployeeDesignation.Title AS Designation	
						 ,EmployeeGrade.Name AS Grade														 
						 ,EmployeeType.Title AS EmployeeType
						 ,Company.Name AS CompanyName
						 ,Company.FullAddress AS CompanyAddress
						 ,Branch.Name AS Branch
						 ,Unit.Name AS Unit	
						 ,Department.Name AS Department
						 ,Section.Name AS Section	
						 ,Line.Name	AS Line	
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) AS JoiningDate									
						 ,CONVERT(VARCHAR(10), TempDate.MonthDate, 103) AS Date
						 ,TempDate.MonthDay				
						 ,CONVERT(INT, EmployeeShortLeave.ReasonType) ReasonType
						 ,[ReasonName]=
							CASE
								WHEN EmployeeShortLeave.ReasonType = 1 THEN 'Personal'
								WHEN EmployeeShortLeave.ReasonType = 2 THEN 'Official'
								ELSE ''
							END
						 ,EmployeeShortLeave.ReasonDescription
						 ,EmployeeShortLeave.FromTime
						 ,EmployeeShortLeave.ToTime
						 ,EmployeeShortLeave.TotalHours
						 ,BranchUnitDepartment.BranchUnitDepartmentId AS DepartmentId
						 ,DepartmentSection.DepartmentSectionId AS SectionId
						 ,DepartmentLine.DepartmentLineId AS LineId						
				 		FROM  TempDate	LEFT OUTER JOIN 	 		
						EmployeeShortLeave ON TempDate.MonthDate = EmployeeShortLeave.[Date] AND EmployeeShortLeave.[EmployeeId] = @EmployeeId AND (EmployeeShortLeave.[ReasonType] = @Reasontype OR @Reasontype IS NULL) LEFT OUTER JOIN 																										
						Employee ON TempDate.EmployeeId = Employee.EmployeeId AND Employee.IsActive = 1 																		 					                         
						LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE (CAST(employeeCompanyInfo.FromDate AS Date) <= @StartDate) 
							  AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
							  ON employee.EmployeeId = employeeCompanyInfo.EmployeeId
						INNER JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
						INNER JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
						INNER JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
						INNER JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
						INNER JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
						INNER JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
						INNER JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
						INNER JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
						INNER JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
						INNER JOIN Company  AS company ON branch.CompanyId=company.Id															
						LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
						LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
						LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
						LEFT JOIN Line line on departmentLine.LineId = line.LineId	
						WHERE 
						company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds)
						AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
						AND branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
						AND branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
						AND ((departmentSection.DepartmentSectionId = @DepartmentSectionId) OR (@DepartmentSectionId IS NULL))
						AND ((departmentLine.DepartmentLineId = @DepartmentLineId) OR (@DepartmentLineId IS NULL))
						AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
						AND ((employee.EmployeeId = @EmployeeID) OR (@EmployeeID IS NULL))
						AND (employee.IsActive=1)
						AND (employee.[Status] = 1)
						AND employeeCompanyInfo.rowNum = 1 																  
					    ORDER BY TempDate.MonthDate
END



