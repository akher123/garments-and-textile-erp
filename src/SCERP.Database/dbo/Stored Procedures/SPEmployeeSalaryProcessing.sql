
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPEmployeeSalaryProcessing '06/01/2015','06/30/2015', 2, 0, 1, NULL, 2
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPEmployeeSalaryProcessing]
	   
				 @StartDate DATETIME
				,@EndDate DATETIME
				,@DepartmentId INT = NULL
				,@LineId INT = NULL
				,@SectionId INT = NULL
				,@EmployeeId uniqueidentifier = NULL	
				,@EmployeeTypeId INT = NULL		
AS
BEGIN
	
		SET NOCOUNT ON;

		Declare @FromDate Datetime = CURRENT_TIMESTAMP

		IF @EmployeeId = '00000000-0000-0000-0000-000000000000'
		BEGIN
			SET @EmployeeId = NULL
		END

		CREATE TABLE #TempEmployee
		(
			EmployeeId uniqueidentifier
		)
	  
		INSERT INTO #TempEmployee(EmployeeId)
	  
		SELECT	Employee.EmployeeId
																						 				 		
        FROM	Employee AS  employee

				LEFT JOIN (SELECT EmployeeId, FromDate, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
				ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
				FROM EmployeeCompanyInfo AS employeeCompanyInfo 
				WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @EndDate) OR (@EndDate IS NULL)) AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
				ON employee.EmployeeId = employeeCompanyInfo.EmployeeId

				LEFT JOIN EmployeePresentAddress AS employeePresentAddress  ON employee.EmployeeId = employeePresentAddress.EmployeeId
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
				INNER JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
				INNER JOIN Company  AS company ON branch.CompanyId=company.Id								
				LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
				LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
				LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
				LEFT JOIN Line line on departmentLine.LineId = line.LineId				
				
				WHERE employee.IsActive = 1 
				AND (Employee.[Status] = 1 OR (Employee.[Status] = 0 AND Employee.QuitDate >= @EndDate)	)
				AND employeeType.Id = @EmployeeTypeId
				AND (employeePresentAddress.[Status] = 1 AND employeePresentAddress.IsActive = 1) 			
				AND employeeCompanyInfo.rowNum = 1 
				AND (employeeCompanyInfo.BranchUnitDepartmentId = @DepartmentId 				
				OR employeeCompanyInfo.DepartmentLineId = @LineId
				OR employeeCompanyInfo.DepartmentSectionId = @SectionId)				
				AND (Employee.EmployeeId = @EmployeeId OR @EmployeeId IS NULL)

				TRUNCATE TABLE EmployeeSalary_Processed_Temp

				DECLARE ProcessSalary CURSOR FOR
				SELECT EmployeeId FROM #TempEmployee

				OPEN ProcessSalary

				FETCH NEXT FROM ProcessSalary INTO @EmployeeId

				WHILE @@FETCH_STATUS = 0
				BEGIN
					BEGIN TRANSACTION SalaryProcess
						EXEC SPEmployeeSalarySave @StartDate, @EndDate, @EmployeeId
						FETCH NEXT FROM ProcessSalary INTO @EmployeeId
					COMMIT TRANSACTION SalaryProcess;
				END

				CLOSE ProcessSalary
				DEALLOCATE ProcessSalary		
				
				SELECT COUNT(EmployeeId) from EmployeeSalary_Processed_Temp			
																	
END





