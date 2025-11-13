
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <08/07/2015>
-- Description:	<> exec SPEmployeeCurrentSalaryReport -1,-1,-1,11,-1,-1,  -1,-1,-1,  '', NULL, NULL
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPEmployeeCurrentSalaryReport]
	   
		
						 @CompanyId			INT = -1
						,@BranchId			INT = -1
						,@UnitId			INT = -1
						,@DepartmentId		INT = -1
						,@SectionId			INT = -1
						,@LineId			INT = -1

						,@EmployeeTypeId	INT = -1
						,@EmployeeGradeId	INT = -1
						,@DesignationId		INT = -1

						,@EmployeeCardId	NVARCHAR(100) = NULL	
						,@FromDate			DATETIME
						,@ToDate			DATETIME			
						
AS
BEGIN
	
		SET NOCOUNT ON;
								  
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
						 ,Line.Name	AS Line	
						 																																																																											
						 ,CONVERT(NVARCHAR(20), CONVERT(FLOAT, EmployeeSalary.BasicSalary)) AS BasicSalary
						 ,CONVERT(NVARCHAR(20), CONVERT(FLOAT, EmployeeSalary.HouseRent)) AS HouseRent
						 ,CONVERT(NVARCHAR(20), CONVERT(FLOAT, EmployeeSalary.MedicalAllowance)) AS  MedicalAllowance  
						 ,CONVERT(NVARCHAR(20), CONVERT(FLOAT, EmployeeSalary.Conveyance)) AS Conveyance
						 ,CONVERT(NVARCHAR(20), CONVERT(FLOAT, EmployeeSalary.FoodAllowance)) AS FoodAllowance
						 ,CONVERT(NVARCHAR(20), CONVERT(FLOAT, EmployeeSalary.EntertainmentAllowance)) AS EntertainmentAllowance
						 ,CONVERT(NVARCHAR(20), CONVERT(FLOAT, EmployeeSalary.GrossSalary)) AS GrossSalary
						 									
						 ,CONVERT(NVARCHAR(20), MONTH(@ToDate)) AS Month
						 ,CONVERT(NVARCHAR(20), YEAR(@ToDate)) AS Year			
						 ,branchUnitDepartment.BranchUnitDepartmentId AS DepartmentId
						 ,departmentSection.DepartmentSectionId AS SectionId
						 ,departmentLine.DepartmentLineId AS LineId
						 ,CONVERT(NVARCHAR(20), datename(month, @ToDate)) AS MonthName
						 ,CONVERT(VARCHAR(10), @FromDate, 103) AS FromDate
						 ,CONVERT(VARCHAR(10), @ToDate, 103) AS ToDate
						
FROM					Employee AS  employee

						LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1 
										
						LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
						LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
						LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
						LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
						LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
						LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
						LEFT JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
						LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
						LEFT JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
						LEFT JOIN Company  AS company ON branch.CompanyId=company.Id
						
						LEFT JOIN (SELECT EmployeeId, FromDate,ToDate, BasicSalary, HouseRent, MedicalAllowance, Conveyance, FoodAllowance, EntertainmentAllowance, GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSalary					
						FROM EmployeeSalary AS EmployeeSalary 
						WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND EmployeeSalary.IsActive=1) EmployeeSalary 
						ON employee.EmployeeId = EmployeeSalary.EmployeeId AND rowNumSalary = 1
		
						LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
						LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
						LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
						LEFT JOIN Line line on departmentLine.LineId = line.LineId

						WHERE employee.IsActive = 1
						AND employee.[Status] = 1 	

						AND (Company.Id = @CompanyId OR  @CompanyId = -1)											
						AND (Branch.Id = @BranchId	 OR  @BranchId = -1) 
						AND (Unit.UnitId = @UnitId   OR  @UnitId = -1)	
						AND(Department.Id = @DepartmentId OR @DepartmentId = -1)			
						AND(Section.SectionId = @SectionId OR @SectionId = -1)
						AND(Line.LineId = @LineId OR @LineId = -1)
						
						AND(EmployeeType.Id = @EmployeeTypeId OR @EmployeeTypeId = -1)
						AND(EmployeeGrade.Id = @EmployeeCardId OR @EmployeeGradeId = -1)
						AND(EmployeeDesignation.Id = @DesignationId OR @DesignationId = -1)

						AND(Employee.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId = '')

						AND(EmployeeSalary.FromDate >= @FromDate OR @FromDate IS NULL)
						AND(EmployeeSalary.ToDate <= @ToDate OR @ToDate IS NULL)
																						
						ORDER BY EmployeeCardId ASC	
																	
END