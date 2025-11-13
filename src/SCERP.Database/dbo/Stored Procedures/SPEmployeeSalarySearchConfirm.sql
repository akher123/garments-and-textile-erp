-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <18/04/2015>
-- Description:	<> exec SPEmployeeSalarySearchConfirm  '06/01/2015', '06/30/2015'
-- =============================================
CREATE PROCEDURE [dbo].[SPEmployeeSalarySearchConfirm]

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

						 ,CONVERT(VARCHAR(10),EmployeeSalary_Processed.FromDate, 103) AS FromDate
						 ,CONVERT(VARCHAR(10),EmployeeSalary_Processed.ToDate, 103) AS ToDate							 					
						 ,EmployeeSalary_Processed.LWP
						 ,EmployeeSalary_Processed.Absent
						 ,EmployeeSalary_Processed.Advance
						 ,EmployeeSalary_Processed.Stamp
						 ,EmployeeSalary_Processed.TotalDeduction
						 ,EmployeeSalary_Processed.AttendanceBonus
						 ,EmployeeSalary_Processed.ShiftingAllowance
						 ,EmployeeSalary_Processed.PayableAmount
						 ,EmployeeSalary_Processed.OTHours
						 ,EmployeeSalary_Processed.OTRate
						 ,EmployeeSalary_Processed.OTAmount

						 ,Month( EmployeeSalary_Processed.ToDate) AS Month
						 ,Year(EmployeeSalary_Processed.ToDate) AS Year
						 ,EmployeeSalary_Processed.GrossSalary
						 ,EmployeeSalary_Processed.BasicSalary
						 ,EmployeeSalary_Processed.HouseRent
						 ,EmployeeSalary_Processed.MedicalAllowance
						 ,EmployeeSalary_Processed.Conveyance
						 ,EmployeeSalary_Processed.FoodAllowance
						 ,EmployeeSalary_Processed.EntertainmentAllowance
						 ,EmployeeSalary_Processed.Tax
						 ,EmployeeSalary_Processed.ProvidentFund
						 ,EmployeeSalary_Processed.NetSalaryPaid
						 ,EmployeeSalary_Processed.Comments
											
						 ,BranchUnitDepartment.BranchUnitDepartmentId AS DepartmentId
						 ,DepartmentSection.DepartmentSectionId AS SectionId
						 ,DepartmentLine.DepartmentLineId AS LineId
					     ,CONVERT(VARCHAR(10),EmployeeSalary_Processed.FromDate, 103) AS FromDate
						 ,CONVERT(VARCHAR(10),EmployeeSalary_Processed.ToDate, 103) AS ToDate
						 ,EmployeeSalary_Processed.IsActive AS IsActive
						 				
	FROM				EmployeeSalary_Processed	LEFT OUTER JOIN 																										
						Employee ON EmployeeSalary_Processed.EmployeeId = Employee.EmployeeId AND Employee.IsActive = 1													 					

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
					
					    WHERE EmployeeSalary_Processed.IsActive = 1 
						AND (employeePresentAddress.[Status] = 1 AND employeePresentAddress.IsActive = 1) 
						AND employeeCompanyInfo.rowNum = 1
						AND (CAST(EmployeeSalary_Processed.FromDate AS date) >= CAST(@FromDate AS date) OR @FromDate IS NULL)
						AND (CAST(EmployeeSalary_Processed.ToDate AS date) <= CAST(@ToDate AS date) OR @ToDate IS NULL)

					    ORDER BY Employee.EmployeeCardId
END




