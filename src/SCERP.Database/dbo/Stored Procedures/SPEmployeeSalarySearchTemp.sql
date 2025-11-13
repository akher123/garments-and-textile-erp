-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <18/04/2015>
-- Description:	<> exec SPEmployeeSalarySearchTemp  '05/01/2015', '05/31/2015'
-- =============================================
CREATE PROCEDURE [dbo].[SPEmployeeSalarySearchTemp]

			@FromDate Datetime,
			@ToDate Datetime 

AS
BEGIN
	
	SET NOCOUNT ON;

			Declare @StartDate Datetime = CURRENT_TIMESTAMP

			SELECT		  Employee.EmployeeId
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

						 ,CONVERT(VARCHAR(10),EmployeeSalary_Processed_Temp.FromDate, 103) AS FromDate
						 ,CONVERT(VARCHAR(10),EmployeeSalary_Processed_Temp.ToDate, 103) AS ToDate							 					
						 ,EmployeeSalary_Processed_Temp.LWP
						 ,EmployeeSalary_Processed_Temp.Absent
						 ,EmployeeSalary_Processed_Temp.Advance
						 ,EmployeeSalary_Processed_Temp.Stamp
						 ,EmployeeSalary_Processed_Temp.TotalDeduction
						 ,EmployeeSalary_Processed_Temp.AttendanceBonus
						 ,EmployeeSalary_Processed_Temp.ShiftingAllowance
						 ,EmployeeSalary_Processed_Temp.PayableAmount
						 ,EmployeeSalary_Processed_Temp.OTHours
						 ,EmployeeSalary_Processed_Temp.OTRate
						 ,EmployeeSalary_Processed_Temp.OTAmount

						 ,Month( EmployeeSalary_Processed_Temp.ToDate) AS Month
						 ,Year(EmployeeSalary_Processed_Temp.ToDate) AS Year
						 ,EmployeeSalary_Processed_Temp.GrossSalary
						 ,EmployeeSalary_Processed_Temp.BasicSalary
						 ,EmployeeSalary_Processed_Temp.HouseRent
						 ,EmployeeSalary_Processed_Temp.MedicalAllowance
						 ,EmployeeSalary_Processed_Temp.Conveyance
						 ,EmployeeSalary_Processed_Temp.FoodAllowance
						 ,EmployeeSalary_Processed_Temp.EntertainmentAllowance
						 ,EmployeeSalary_Processed_Temp.Tax
						 ,EmployeeSalary_Processed_Temp.ProvidentFund
						 ,EmployeeSalary_Processed_Temp.NetSalaryPaid
						 ,EmployeeSalary_Processed_Temp.Comments
					     						
						 ,BranchUnitDepartment.BranchUnitDepartmentId AS DepartmentId
						 ,DepartmentSection.DepartmentSectionId AS SectionId
						 ,DepartmentLine.DepartmentLineId AS LineId
					     ,CONVERT(VARCHAR(10),EmployeeSalary_Processed_Temp.FromDate, 103) AS FromDate
						 ,CONVERT(VARCHAR(10),EmployeeSalary_Processed_Temp.ToDate, 103) AS ToDate
						 ,EmployeeSalary_Processed_Temp.IsActive AS IsActive
						 				
	FROM				EmployeeSalary_Processed_Temp	LEFT OUTER JOIN 																										
						Employee ON EmployeeSalary_Processed_Temp.EmployeeId = Employee.EmployeeId AND Employee.IsActive = 1 													 					

                        LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @ToDate) OR (@ToDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId

						LEFT JOIN EmployeePresentAddress AS employeePresentAddress  ON employee.EmployeeId = employeePresentAddress.EmployeeId
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
					
					    WHERE EmployeeSalary_Processed_Temp.IsActive = 1 
						AND (employeePresentAddress.[Status] = 1 AND employeePresentAddress.IsActive = 1) 
						AND employeeCompanyInfo.rowNum = 1

					    ORDER BY Employee.EmployeeCardId
END





