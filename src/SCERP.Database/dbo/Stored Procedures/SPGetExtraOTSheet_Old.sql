
-- ===========================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <30-Sep-15 2:09:40 PM>
-- Description:	<> EXEC [SPGetExtraOTSheet] 1, 1, 1, NULL, NULL, NULL, NULL, NULL,'2015-09-01','2015-09-30'
-- ===========================================================================================================


CREATE PROCEDURE [dbo].[SPGetExtraOTSheet_Old]
						
						@CompanyId INT,
						@BranchId INT,
						@BranchUnitId INT,
						@BranchUnitDepartmentId INT = NULL,
						@DepartmentSectionId INT = NULL,
						@DepartmentLineId INT = NULL,
						@EmployeeTypeId INT = NULL,
						@EmployeeCardId NVARCHAR(100) = NULL,
						@StartDate DATETIME,
						@EndDate DATETIME

AS
BEGIN
	
	SET NOCOUNT ON;

			DECLARE @OTHourRate NUMERIC (18,2);

			SELECT		  Employee.EmployeeId
						 ,Employee.EmployeeCardId
						 ,Employee.Name 
						 ,Company.Name AS CompanyName
						 ,Company.FullAddress AS CompanyAddress														
						 ,Branch.Name AS Branch
						 ,Unit.Name AS Unit		
						 ,Department.Name AS Department			
						 ,Section.Name AS Section
						 ,Line.Name AS Line		
						 ,employeeType.Title AS EmployeeType
						 ,EmployeeDesignation.Title AS Designation						
						 ,EmployeeGrade.Name AS Grade						 			
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) JoiningDate						 
						 ,ROUND(EmployeeSalary.BasicSalary,0) AS BasicSalary						 																																							
						 ,ROUND(EmployeeSalary.GrossSalary,0) AS GrossSalary
						 ,CONVERT(VARCHAR(10), @StartDate, 103) AS FromDate
						 ,CONVERT(VARCHAR(10), @EndDate, 103) AS ToDate
						 ,CONVERT(NVARCHAR(20),dbo.fnGetTotalExtraOTHours(Employee.EmployeeId, @StartDate, @EndDate)) AS TotalExtraOTHour
						 ,CASE employeeCompanyInfo.IsEligibleForOvertime WHEN 1 THEN  CONVERT(NVARCHAR(20), CAST(((EmployeeSalary.BasicSalary/208.00) * dbo.fnGetOverTimeRate(@StartDate, @EndDate)) AS DECIMAL(18,2))) ELSE '0.00' END AS OTHourRate
						 ,CONVERT(NVARCHAR(20), (ROUND((((EmployeeSalary.BasicSalary/208.00) * (dbo.fnGetOverTimeRate(@StartDate, @EndDate))) * (dbo.fnGetTotalExtraOTHours(Employee.EmployeeId, @StartDate, @EndDate))),0))) AS OTAmount   						 																												
						 ,DATENAME(m,@StartDate)+'-'+CAST(DATEPART(yyyy,@StartDate) AS VARCHAR) as MonthYear

FROM					 Employee 
						 
						 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, IsEligibleForOvertime,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @EndDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
						 ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
						 
						 LEFT OUTER JOIN 													
						 (SELECT EmployeeSalary.EmployeeId,EmployeeSalary.BasicSalary,EmployeeSalary.GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
						 FROM EmployeeSalary AS EmployeeSalary 
						 WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @EndDate) AND EmployeeSalary.IsActive=1)) EmployeeSalary 
						 ON employee.EmployeeId = EmployeeSalary.EmployeeId AND EmployeeSalary.rowNumSal = 1  

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

						 WHERE      
								(company.Id = @CompanyId)
								AND (branch.Id = @BranchId)
								AND (branchUnit.BranchUnitId = @BranchUnitId)
								AND ((BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId IS NULL))
								AND ((employeeCompanyInfo.DepartmentSectionId = @DepartmentSectionId) OR (@DepartmentSectionId IS NULL))
								AND ((employeeCompanyInfo.DepartmentLineId = @DepartmentLineId) OR (@DepartmentLineId IS NULL))
								AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId IS NULL))
								AND ((EmployeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId IS NULL))
							    AND (employee.IsActive = 1)
								AND (employee.[Status] = 1)	
								AND ((dbo.fnGetTotalExtraOTHours(Employee.EmployeeId, @StartDate, @EndDate)) > 0.0)

						 ORDER BY Employee.EmployeeCardId

END






