
-- ======================================================================================================================
-- Author		:  <Yasir Arafat>
-- Create date  :  <2017-01-05>
-- Description  :  EXEC SPGetAbsentOtPenaltyEmployee  1, 1, 1, -1, -1, -1, '2017-01-05','2017-01-05', '','superadmin'
-- ======================================================================================================================

CREATE PROCEDURE [dbo].[SPGetAbsentOtPenaltyEmployee]
						
						 @CompanyId		            INT
						,@BranchId	      	        INT
						,@BranchUnitId		        INT
						,@BranchUnitDepartmentId    INT = NULL
						,@DepartmentSectionId		INT = NULL
						,@DepartmentLineId			INT = NULL			
						,@FromDate				    DATETIME = NULL
						,@ToDate					DATETIME = NULL
						,@EmployeeCardId			NVARCHAR(100) = NULL
						,@UserName					NVARCHAR(100)
						
AS
BEGIN
	
	SET NOCOUNT ON;
					
				
					DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);
		
									
					IF(@fromDate IS NULL)
					BEGIN
						SET @fromDate = CAST(CURRENT_TIMESTAMP AS DATE)
					END
					ELSE
					BEGIN
						SET @fromDate = CAST(@fromDate AS DATE)
					END


					IF(@ToDate IS NULL)
					BEGIN
						SET @ToDate = CAST(CURRENT_TIMESTAMP AS DATE)
					END
					ELSE
					BEGIN
						SET @ToDate = CAST(@ToDate AS DATE)
					END


					SELECT		  
						  Employee.EmployeeId AS EmployeeId
						 ,Employee.EmployeeCardId AS EmployeeCardId
						 ,Employee.Name AS EmployeeName
						 ,Unit.Name AS Unit
						 ,Department.Name AS Department									
						 ,Section.Name AS Section
						 ,Line.Name AS Line	
						 ,employeeType.Title  AS EmployeeType
						 ,EmployeeGrade.Name AS EmployeeGrade
						 ,EmployeeDesignation.Title AS EmployeeDesignation		
						 ,employee.JoiningDate	
						 ,employeeSalaryInfo.GrossSalary
						 ,employeeSalaryInfo.BasicSalary		
						 ,CONVERT(DECIMAL(18,2), ((employeeSalaryInfo.GrossSalary/30) - (employeeSalaryInfo.BasicSalary/30))) AS Difference					 
						 ,CONVERT(DECIMAL(18,2), (employeeSalaryInfo.BasicSalary/104)) AS OTRate
						 ,CONVERT(DECIMAL(18,2), (((employeeSalaryInfo.GrossSalary/30) - (employeeSalaryInfo.BasicSalary/30))/(employeeSalaryInfo.BasicSalary/104))) AS PenaltyOTHours
						 					
			FROM		 Employee employee
						 LEFT JOIN EmployeeInOut ON EmployeeInOut.EmployeeId = employee.EmployeeId AND CAST(EmployeeInOut.TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate

						 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @fromDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
						 ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

						 LEFT JOIN (SELECT EmployeeId,GrossSalary,BasicSalary,HouseRent,MedicalAllowance,FoodAllowance,Conveyance,EntertainmentAllowance, FromDate,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						
						 FROM EmployeeSalary AS employeeSalary
						 WHERE ((CAST(employeeSalary.FromDate AS Date) <= @fromDate) OR (@fromDate IS NULL))
						 AND employeeSalary.IsActive = 1) employeeSalaryInfo 
						 ON employee.EmployeeId = employeeSalaryInfo.EmployeeId AND employeeSalaryInfo.rowNumSal = 1

						 LEFT JOIN (SELECT EmployeeId,  WorkGroupId, AssignedDate,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY AssignedDate DESC) AS rowNumWG
						 FROM EmployeeWorkGroup AS employeeWorkGroup 
					     WHERE (CAST(employeeWorkGroup.AssignedDate AS Date) <= @fromDate) AND employeeWorkGroup.IsActive=1) employeeWorkGroup 
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

						 WHERE      
								 ((company.Id = @CompanyId) OR (@CompanyId = -1))
								 AND ((branch.Id = @BranchId) OR (@BranchId = -1))
								 AND ((branchUnit.BranchUnitId = @BranchUnitId) OR (@BranchUnitId = -1)) 

								 AND ((branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId = -1))

								 AND ((departmentSection.DepartmentSectionId = @DepartmentSectionId) OR (@DepartmentSectionId = -1))
								 AND ((departmentLine.DepartmentLineId = @DepartmentLineId) OR (@DepartmentLineId = -1))				
								 AND ((Employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId = ''))						
								 AND (employee.IsActive = 1)						
								 AND EmployeeInOut.Status = 'Absent'	
								 AND section.SectionId <> 32			
								 AND employeeType.Id NOT IN(1,2,3)
						 		 						
						 ORDER BY Employee.EmployeeCardId

END






