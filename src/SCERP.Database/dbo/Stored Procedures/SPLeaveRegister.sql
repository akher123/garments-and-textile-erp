-- =============================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <22/09/2019>
-- Description:	<> EXEC [SPLeaveRegister] '2019-09-22', '0114'
-- =============================================================

CREATE PROCEDURE [dbo].[SPLeaveRegister]
			
								    						
						     @EffectiveDate					DATETIME 
							,@EmployeeCardId				NVARCHAR(20)
	                  	        	                     	                 	                   								 								 	
AS

BEGIN
	
							 SET NOCOUNT ON;

							 DECLARE @YearStartDate	DATETIME = DATEADD(yy, DATEDIFF(yy, 0, @EffectiveDate), 0)		
							  
					SELECT	 Company.NameInBengali						AS CompanyName
							,Company.FullAddressInBengali				AS CompanyAddress
							,Employee.EmployeeId						AS EmployeeId
							,Employee.EmployeeCardId					AS CardId
						    ,Employee.NameInBengali						AS Employeename
						    ,EmployeeDesignation.TitleInBengali			AS Designation
							,branch.NameInBengali						AS BranchName
							,unit.NameInBengali							AS UnitName															
							,Department.NameInBengali		  			AS DepartmentName
							,Section.NameInBengali						AS SectionName
							,line.NameInBengali							AS LineName
							,EmployeeType.TitleInBengali				AS EmployeeType										
							,CONVERT(NVARCHAR(12), Employee.JoiningDate, 103)	AS JoiningDate 

							,ISNULL((SELECT NoOfAllowableLeaveDays FROM EmployeeLeaveHistory WHERE EmployeeLeaveHistory.EmployeeId = Employee.EmployeeId AND Year = YEAR(@YearStartDate) AND LeaveTypeId = 5), 0) AS YearStartEarnLeave																																	
							,ISNULL((SELECT SUM(ConsumedTotalDays) FROM EmployeeLeave WHERE LeaveTypeId = 5 AND EmployeeId = employee.EmployeeId AND EmployeeLeave.ConsumedFromDate BETWEEN @YearStartDate AND @EffectiveDate AND EmployeeLeave.IsActive = 1 GROUP BY EmployeeId),0) AS EarnLeaveConsumed	
							,ISNULL((SELECT SUM(ConsumedTotalDays) FROM EmployeeLeave WHERE LeaveTypeId = 1 AND EmployeeId = employee.EmployeeId AND EmployeeLeave.ConsumedFromDate BETWEEN @YearStartDate AND @EffectiveDate AND EmployeeLeave.IsActive = 1 GROUP BY EmployeeId),0) AS CasualLeaveConsumed	
							,ISNULL((SELECT SUM(ConsumedTotalDays) FROM EmployeeLeave WHERE LeaveTypeId = 2 AND EmployeeId = employee.EmployeeId AND EmployeeLeave.ConsumedFromDate BETWEEN @YearStartDate AND @EffectiveDate AND EmployeeLeave.IsActive = 1 GROUP BY EmployeeId),0) AS SickLeaveConsumed	

							,CONVERT(NVARCHAR(12), EmployeeLeave.ConsumedFromDate, 103)		AS LeaveStartDate
							,EmployeeLeave.ConsumedTotalDays								AS TotalLeaveDays
							,EarnLeavegivenByYear.Days										AS EarnLeaveGiven
							,CONVERT(NVARCHAR(12), EarnLeavegivenByYear.ExecuteDate, 103)	AS EarnLeaveGivenDate

							,ISNULL((SELECT NoOfRemainingLeaveDays FROM EmployeeLeaveHistory WHERE EmployeeLeaveHistory.EmployeeId = Employee.EmployeeId AND Year = YEAR(@YearStartDate) AND LeaveTypeId = 5), 0) AS EarnLeaveRemain
							,ISNULL((SELECT NoOfRemainingLeaveDays FROM EmployeeLeaveHistory WHERE EmployeeLeaveHistory.EmployeeId = Employee.EmployeeId AND Year = YEAR(@YearStartDate) AND LeaveTypeId = 1), 0) AS CasualLeaveRemain
							,ISNULL((SELECT NoOfRemainingLeaveDays FROM EmployeeLeaveHistory WHERE EmployeeLeaveHistory.EmployeeId = Employee.EmployeeId AND Year = YEAR(@YearStartDate) AND LeaveTypeId = 2), 0) AS SickLeaveRemain																																	

							FROM dbo.Employee
                            
							LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum							
							FROM EmployeeCompanyInfo AS employeeCompanyInfo 
							WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @EffectiveDate) AND (employeeCompanyInfo.IsActive = 1))) employeeCompanyInfo 
							ON Employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
																											
							LEFT JOIN (SELECT EmployeeSalary.EmployeeId, EmployeeSalary.BasicSalary, EmployeeSalary.GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal													 
							FROM EmployeeSalary AS EmployeeSalary 
							WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @EffectiveDate) AND EmployeeSalary.IsActive = 1)) employeeSalary 
							ON employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNumSal = 1  
							
							LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id
							LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
							LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
							LEFT JOIN BranchUnitDepartment AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
							LEFT JOIN BranchUnit AS branchUnit ON branchUnitDepartment.BranchUnitId = branchUnit.BranchUnitId
							LEFT JOIN UnitDepartment AS unitDepartment ON branchUnitDepartment.UnitDepartmentId = unitDepartment.UnitDepartmentId
							LEFT JOIN Unit AS unit ON branchUnit.UnitId = unit.UnitId
							LEFT JOIN Department AS department ON unitDepartment.DepartmentId = department.Id
							LEFT JOIN Branch AS branch ON branchUnit.BranchId = branch.Id
							LEFT JOIN Company AS company ON branch.CompanyId = company.Id
							LEFT JOIN DepartmentSection departmentSection ON employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
							LEFT JOIN Section section ON departmentSection.SectionId = section.SectionId
							LEFT JOIN DepartmentLine departmentLine ON employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
							LEFT JOIN Line line ON departmentLine.LineId = line.LineId

							LEFT JOIN EmployeeLeave ON EmployeeLeave.EmployeeId = Employee.EmployeeId AND EmployeeLeave.ConsumedFromDate >= @YearStartDate
							LEFT JOIN EarnLeavegivenByYear ON EarnLeavegivenByYear.EmployeeId = Employee.EmployeeId AND EarnLeavegivenByYear.ExecuteDate BETWEEN @YearStartDate AND @EffectiveDate

							WHERE (Employee.EmployeeCardId = @EmployeeCardId)																																														       							    																					
							AND employeeCompanyInfo.rowNum = 1 							
							AND EmployeeType.Id IN(4,5) 
							AND Employee.IsActive = 1																																																				  														  						  											  							
END