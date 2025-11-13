
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <08/07/2015>
-- Description:	<> exec SPEmployeeIndividualSalaryHistoryReport  NULL, NULL, '999999'
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPEmployeeIndividualSalaryHistoryReport]
	   

					 @FromDate			DATETIME = NULL
					,@ToDate			DATETIME = NULL		
					,@EmployeeCardId	NVARCHAR(100) = NULL	
					
AS
BEGIN
	
		SET NOCOUNT ON;
					
			DECLARE @CurrentDate Datetime = CURRENT_TIMESTAMP
					  
			SELECT		  CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY Employee.EmployeeCardId)) AS Row
						 ,Employee.EmployeeId
						 ,Employee.EmployeeCardId
						 ,Company.Name AS CompanyName
						 ,Company.FullAddress AS CompanyAddress
						 ,Employee.Name AS Name
						 ,EmployeeDesignation.Title AS Designation
						 ,EmployeeGrade.Name AS Grade
						 ,CONVERT(VARCHAR(10), Employee.JoiningDate, 103) JoiningDate
						 ,Branch.Name AS Branch
						 ,Unit.Name AS Unit
						 ,Department.Name AS Department
						 ,Section.Name AS Section
						 ,Line.Name AS Line

						 ,EmployeeSalary_Processed.GrossSalary
						 ,EmployeeSalary_Processed.BasicSalary
						 ,EmployeeSalary_Processed.HouseRent
						 ,EmployeeSalary_Processed.MedicalAllowance
						 ,EmployeeSalary_Processed.Conveyance
						 ,EmployeeSalary_Processed.FoodAllowance
						 ,EmployeeSalary_Processed.EntertainmentAllowance
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
						 ,EmployeeSalary_Processed.NetSalaryPaid
						 ,EmployeeSalary_Processed.Comments
						 ,EmployeeSalary_Processed.Tax
						 ,EmployeeSalary_Processed.ProvidentFund																																																																										
											 									
						 ,CONVERT(NVARCHAR(20), MONTH(EmployeeSalary_Processed.ToDate)) AS Month
						 ,CONVERT(NVARCHAR(20), YEAR(EmployeeSalary_Processed.ToDate)) AS Year			
						 ,branchUnitDepartment.BranchUnitDepartmentId AS DepartmentId
						 ,departmentSection.DepartmentSectionId AS SectionId
						 ,departmentLine.DepartmentLineId AS LineId
						 ,CONVERT(NVARCHAR(20), datename(month, EmployeeSalary_Processed.ToDate)) AS MonthName
						 ,CONVERT(VARCHAR(10), @FromDate, 103) AS FromDate
						 ,CONVERT(VARCHAR(10), @ToDate, 103) AS ToDate
						
FROM					Employee AS  employee

						LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @CurrentDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1 
							
						JOIN EmployeeSalary_Processed ON EmployeeSalary_Processed.EmployeeId = employee.EmployeeId	
										
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
						AND EmployeeSalary_Processed.IsActive = 1			
						AND employee.EmployeeCardId = @employeeCardId
						AND (EmployeeSalary_Processed.FromDate >= @FromDate OR @FromDate IS NULL)
						AND (EmployeeSalary_Processed.ToDate <= @ToDate OR @ToDate IS NULL ) 	
																																		
						ORDER BY EmployeeCardId ASC	
																	
END




