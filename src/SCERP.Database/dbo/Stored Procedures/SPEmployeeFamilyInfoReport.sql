
-- =============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <15/03/2015>
-- Description:	<> exec SPEmployeeFamilyInfoReport -1,-1,-1,'999999','', -1, '2000','2015'
-- =============================================================================

CREATE PROCEDURE [dbo].[SPEmployeeFamilyInfoReport]
								

						    @DepartmentId		INT 
						   ,@SectionId			INT
						   ,@LineId				INT
						   ,@EmployeeCardId		NVARCHAR(100)												 						 					 	
						   ,@EmployeeName		NVARCHAR(100)
						   ,@ChildGenderId		INT
						   ,@DOBFrom			DateTime
						   ,@DOBTo				DateTime

AS
BEGIN
	
	SET NOCOUNT ON;
			
			Declare @StartDate Datetime = CURRENT_TIMESTAMP		
									
								  
			SELECT		  Company.Name							AS CompanyName
					     ,Company.FullAddress					AS CompanyAddress
						 ,Branch.Name							AS BranchName
						 ,Unit.Name								AS Unit
						 ,Department.Name						AS Department
						 ,Section.Name							AS Section
						 ,Line.Name								AS Line
						 ,employeeType.Title					AS EmployeeType
						 ,EmployeeGrade.Name					AS Grade
						 ,EmployeeDesignation.Title				AS Designation									 		
						 ,EmployeeCompanyInfo.PunchCardNo		AS PunchCardNo
						 ,Employee.EmployeeCardId				AS EmployeeCardId
						 ,Employee.Name							AS Name
						 ,Employee.Status						AS Status
						 ,Employee.QuitDate						AS QuitDate
						 ,Employee.EmployeeId					AS EmployeeId																															 																									 																																									 
						 ,Department.id AS DepartmentId														
						 ,EmployeeFamilyInfo.NameOfChild AS ChildName
						 ,Gender.Title AS ChildGender
						 ,CONVERT(VARCHAR(10), EmployeeFamilyInfo.DateOfBirth, 103) AS DateOfBirth	
					
									
FROM					Employee AS  employee
						LEFT JOIN (SELECT EmployeeId, FromDate, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, PunchCardNo,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @StartDate) OR (@StartDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1 	
														
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
						LEFT JOIN EmployeeFamilyInfo ON EmployeeFamilyInfo.EmployeeId = employee.EmployeeId AND EmployeeFamilyInfo.IsActive = 1
						LEFT JOIN Gender ON Gender.GenderId = EmployeeFamilyInfo.GenderId AND Gender.IsActive = 1


						WHERE employee.IsActive = 1 
						AND employee.[Status] = 1 	
						AND (employee.EmployeeCardId = @employeeCardId OR  @employeeCardId = '')
						AND (Department.Id = @departmentId OR @departmentId = -1)
						AND (Section.SectionId = @sectionId OR @sectionId = -1)
						AND (Line.LineId = @lineId OR @lineId = -1)
					
						AND (EmployeeFamilyInfo.GenderId = @ChildGenderId OR @ChildGenderId = -1)
						AND (employee.Name LIKE '%' + @EmployeeName + '%' OR @EmployeeName IS NULL)	
						AND (EmployeeFamilyInfo.DateOfBirth >= @DOBFrom OR @DOBFrom IS NULL)
						AND (EmployeeFamilyInfo.DateOfBirth <= @DOBTo OR @DOBTo IS NULL )
						ORDER BY EmployeeCardId ASC																		
END





