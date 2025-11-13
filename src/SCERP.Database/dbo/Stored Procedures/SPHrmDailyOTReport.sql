-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <02/11/2016>
-- Description:	<> EXEC SPHrmDailyOTReport '2017-01-01'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPHrmDailyOTReport]
			

						   @fromDate DATETIME = '2016-09-26'				

AS
BEGIN
	
			SET NOCOUNT ON;
				

					SELECT EmployeeInOut.TransactionDate
				   ,Department.Name					AS DepartmentName
				   ,Section.Name					AS SectionName
				   ,Line.Name						AS LineName
				   ,COUNT(EmployeeInOut.EmployeeId) AS NoOfEmployee
				   ,SUM([OTHours])					AS OTHours
				   ,SUM([OTHours] * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@fromDate, @fromDate))) AS DECIMAL(18,2))) AS OTAmount

				   ,SUM([ExtraOTHours])				AS ExtraOTHours
				   ,SUM([ExtraOTHours] * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@fromDate, @fromDate))) AS DECIMAL(18,2))) AS ExtraOTAmount

				   ,SUM([WeekendOTHours])			AS WeekendOTHours
				   ,SUM([WeekendOTHours] * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@fromDate, @fromDate))) AS DECIMAL(18,2))) AS WeekendOTAmount

				   ,SUM([HolidayOTHours])			AS HolidayOTHours
				   ,SUM([HolidayOTHours] * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@fromDate, @fromDate))) AS DECIMAL(18,2))) AS HolidayOTAmount
 	  
					FROM [dbo].[EmployeeInOut]
  
					LEFT JOIN (SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,
					employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,
					employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, 
					ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
					FROM EmployeeSalary AS employeeSalary 
					WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @fromDate) AND EmployeeSalary.IsActive=1) employeeSalary 
					ON [EmployeeInOut].EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1

					LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
					ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
					FROM EmployeeCompanyInfo AS employeeCompanyInfo 
					WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @fromDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
					ON EmployeeInOut.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

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

				    WHERE CAST(EmployeeInOut.TransactionDate AS DATE) BETWEEN @fromDate AND @fromDate
				    AND (OTHours > 0 OR WeekendOTHours > 0 OR HolidayOTHours > 0)    
				    AND EmployeeInOut.BranchUnitId IN(1,2)
				    AND EmployeeInOut.IsActive = 1

				    GROUP BY EmployeeInOut.TransactionDate, Department.Name, Section.Name, Line.Name

                    ORDER BY [TransactionDate] 	  														  						  											  							
END


