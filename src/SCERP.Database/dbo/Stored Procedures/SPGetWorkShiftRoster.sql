
-- ===========================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2018-01-03>
-- Description:	<> EXEC [SPGetWorkShiftRoster] 'Dyeing', 'Group-1','700031','','2017-12-31'
-- ===========================================================================================================


CREATE PROCEDURE [dbo].[SPGetWorkShiftRoster]
						

									  @UnitName				NVARCHAR(50)
									 ,@GroupName			NVARCHAR(50)		
									 ,@EmployeeCardId		NVARCHAR(50)					
									 ,@EmployeeName			NVARCHAR(50)	
									 ,@FromDate				DATETIME
AS

BEGIN
	
	SET NOCOUNT ON;
			 						
							DECLARE @Date	DATETIME
							
							IF(@FromDate = '1900-01-01')
								SET @FromDate = CURRENT_TIMESTAMP
									
														
							SELECT 
								   WorkGroupRoster.UnitName
								  ,WorkGroupRoster.GroupName													 
								  ,ShiftName =  
									  CASE WorkShiftRoster.ShiftName  
										 WHEN 'Day' THEN 'Morning'  
										 WHEN 'Night' THEN 'Evening'  
										 WHEN 'General' THEN 'General'												
									  END
								  ,WorkGroupRoster.EmployeeCardId
								  ,Employee.Name					AS EmployeeName
								  ,EmployeeDesignation.Title		AS Designation
								  ,Section.Name						AS Section

								FROM WorkGroupRoster
							  
								LEFT JOIN  Employee AS  employee ON employee.EmployeeId = WorkGroupRoster.EmployeeId
								LEFT JOIN WorkShiftRoster ON WorkShiftRoster.UnitName = WorkGroupRoster.UnitName AND WorkShiftRoster.GroupName = WorkGroupRoster.GroupName

								LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
								ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
								FROM EmployeeCompanyInfo AS employeeCompanyInfo 
								WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
								ON employee.EmployeeId = employeeCompanyInfo.EmployeeId 

								LEFT JOIN EmployeePresentAddress AS employeePresentAddress  ON employee.EmployeeId = employeePresentAddress.EmployeeId AND employeePresentAddress.Status = 1 AND employeePresentAddress.IsActive = 1
								LEFT JOIN District DIS ON employeePresentAddress.DistrictId = DIS.Id 
								LEFT OUTER JOIN PoliceStation PST ON employeePresentAddress.PoliceStationId = PST.Id 
								LEFT JOIN EmployeePermanentAddress AS EmployeePermanentAddress ON employee.EmployeeId = EmployeePermanentAddress.EmployeeId						
								LEFT JOIN District DIST ON EmployeePermanentAddress.DistrictId = DIST.Id 
								LEFT OUTER JOIN PoliceStation PSTE ON EmployeePermanentAddress.PoliceStationId = PSTE.Id 
								INNER JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
								INNER JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
								INNER JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
								INNER JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
								INNER JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
								INNER JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
								INNER JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
								INNER JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
								LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
								LEFT JOIN Section section on departmentSection.SectionId = section.SectionId

								WHERE (WorkGroupRoster.UnitName = @UnitName OR @UnitName = '')
								AND (WorkGroupRoster.GroupName = @GroupName OR @GroupName = '')
								AND (WorkGroupRoster.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId = '')
								AND (employee.Name LIKE '%'+ @EmployeeName +'%' OR @EmployeeName = '')
								AND employeeCompanyInfo.rowNum = 1

								ORDER BY WorkGroupRoster.EmployeeCardId
END