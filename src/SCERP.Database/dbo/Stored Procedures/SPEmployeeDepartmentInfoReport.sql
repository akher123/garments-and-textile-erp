
-- =================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <19/08/2015>
-- Description:	<> exec SPEmployeeDepartmentInfoReport -1,-1,-1,'','golam','01/01/2014','01/01/2016'
-- =================================================================================================================

CREATE PROCEDURE [dbo].[SPEmployeeDepartmentInfoReport]

						    @CompanyId					INT = -1,
							@BranchId					INT = -1,
							@UnitId						INT = -1,		
							@DepartmentId				INT = -1,
							@SectionId					INT = -1,
							@LineId						INT = -1,
							@EmployeeTypeId				INT = -1,
							@EmployeeGradeId			INT = -1,
							@EmployeeDesignationId		INT = -1,
							@EmployeeCardId				NVARCHAR(100),
							@EmployeeName				NVARCHAR(100),
						    @FromDate					DATETIME,
						    @ToDate						DATETIME
																		 						 					

AS
BEGIN
	
	SET NOCOUNT ON;
						
			Declare @StartDate Datetime = CURRENT_TIMESTAMP										
					  
			SELECT		  Company.Name							AS CompanyName
					     ,Company.FullAddress					AS CompanyAddress
						 ,Branch.Name							AS Branch
						 ,Unit.Name								AS Unit
						 ,Department.Name						AS Department
						 ,Section.Name							AS Section
						 ,Line.Name								AS Line
						 ,employeeType.Title					AS EmployeeType
						 ,EmployeeGrade.Name					AS Grade
						 ,EmployeeDesignation.Title				AS Designation
						 ,CONVERT(VARCHAR(10), Employee.JoiningDate, 103) AS JoiningDate
						 ,CONVERT(VARCHAR(10), Employee.ConfirmationDate, 103) AS ConfirmDate
						 		
						 ,EmployeeCompanyInfo.PunchCardNo		AS PunchCardNo
						 ,Employee.EmployeeCardId				AS EmployeeCardId
						 ,Employee.Name							AS EmployeeName
						 ,Employee.Status						AS Status
						 ,Employee.QuitDate						AS QuitDate

						 ,Employee.EmployeeId					AS EmployeeId																															 																									 																																									 
						 ,Department.id AS DepartmentId
						 ,CONVERT(VARCHAR(10), @fromDate, 103) AS FromDate	
						 ,CONVERT(VARCHAR(10), @toDate, 103) AS ToDate	
									
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

						WHERE employee.IsActive = 1
						AND employee.[Status] = 1 	

						AND (employee.EmployeeCardId = @employeeCardId OR  @employeeCardId = '')
						AND (employee.Name LIKE '%' + @EmployeeName + '%' OR @EmployeeName IS NULL)	
						AND (Department.Id = @departmentId OR @departmentId = -1)
						AND (Section.SectionId = @sectionId OR @sectionId = -1)
						AND (Line.LineId = @lineId OR @lineId = -1)
							
						ORDER BY EmployeeCardId ASC																		
END






