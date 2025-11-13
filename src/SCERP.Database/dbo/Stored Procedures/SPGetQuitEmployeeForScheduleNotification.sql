
-- ==========================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2017-11-18>
-- Description:	<> EXEC [SPGetQuitEmployeeForScheduleNotification]
-- ==========================================================================================================================

CREATE PROCEDURE [dbo].[SPGetQuitEmployeeForScheduleNotification]
				
AS

BEGIN
	
	SET NOCOUNT ON;
						  				
						  DECLARE @CheckDate DATETIME = CURRENT_TIMESTAMP 
						  DECLARE @QuitDate  DATETIME

						  SET @QuitDate = DATEADD(DAY, -10, @CheckDate)

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
						 ,employee.QuitDate
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

						 WHERE CAST(employee.QuitDate AS DATE) = CAST(@QuitDate AS DATE)
						 AND Employee.QuitTypeId = 2

						 ORDER BY CAST(Employee.EmployeeCardId AS INT) 
													
END