-- =============================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <07/09/2016>
-- Description:	<> EXEC SPEarnLeaveSheetQuitEmployee '2019-05-26', '2019-06-25', '5255', -1, -1
-- =============================================================================================

CREATE PROCEDURE [dbo].[SPEarnLeaveSheetQuitEmployee]
			
		
						         @fromDate				DATETIME=NULL
						        ,@toDate				DATETIME=NULL
								,@EmployeeCardId NVARCHAR(100) = '',
							     @CompanyID INT = -1,
	                             @BranchID INT = -1,
	                             @BranchUnitID INT = -1,
	                             @BranchUnitDepartmentID INT = -1,
	                             @DepartmentSectionId INT = -1,
	                             @DepartmentLineId INT = -1,
	                             @EmployeeTypeID INT = -1,
	                             @EmployeeGradeID INT = -1,
	                             @EmployeeDesignationID INT = -1,
	                             @GenderId INT = NULL,
	                             @EmployeeName NVARCHAR(100) = NULL,
	                             @EmployeeMobilePhone NVARCHAR(100) = NULL,
	                             @EmployeeStatus INT = NULL,
	                             @UserName NVARCHAR(100)=NULL,
	                             @StartRowIndex INT = NULL,
	                             @MaxRows INT = NULL,
	                             @SortField INT = NULL,
	                             @SortDiriection INT = NULL
	                             --,@RowCount INT OUTPUT
								 
								 				

AS
BEGIN
	
							 SET NOCOUNT ON;

							 DECLARE @EffectiveFrom DATE = '2017-01-01'


							 SELECT 
							 Employee.EmployeeId						AS EmployeeId
							,Employee.EmployeeCardId					AS CardId
						    ,Employee.Name								AS Employeename
						    ,EmployeeDesignation.Title					AS Designation
							,Department.Name		  					AS DepartmentName
							,EmployeeType.Title							AS EmployeeType										
							,CONVERT(NVARCHAR(12),JoiningDate,106)		AS JoiningDate 
							,EmployeeSalary.GrossSalary					AS GrossSalary
							,employeeSalary.GrossSalary/30				AS PerDayGross
							
							--,FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'totaldays')) AS TotalPresent	
							, ( SELECT COUNT(1) FROM EmployeeInOut WHERE EmployeeId = Employee.EmployeeId AND CAST(TransactionDate AS DATE) >= @EffectiveFrom AND (Status = 'Present' OR Status = 'Late') AND CAST(TransactionDate AS DATE) < @toDate AND IsActive = 1 ) as 	TotalPresent						
							--,([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'earnleave')) AS EarnLeaveDays
							,CAST(CAST(( SELECT COUNT(1) FROM EmployeeInOut WHERE EmployeeId = Employee.EmployeeId AND CAST(TransactionDate AS DATE) >= @EffectiveFrom AND (Status = 'Present' OR Status = 'Late') AND CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate AND IsActive = 1 ) as DECIMAL(18,2))/18.00 as DECIMAL(18,2))  AS EarnLeaveDays
							--,FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'previous')) AS PreviousEarnDays
							, CAST(CAST(( SELECT COUNT(1) FROM EmployeeInOut WHERE EmployeeId = Employee.EmployeeId AND CAST(TransactionDate AS DATE) >= @EffectiveFrom AND (Status = 'Present' OR Status = 'Late') AND CAST(TransactionDate AS DATE) < @fromDate AND IsActive = 1 ) as DECIMAL(18,2))/18.00 as DECIMAL(18,2))  AS PreviousEarnDays														
							--,FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'consumed')) AS EarnLeaveConsumed
							, (ISNULL(( SELECT SUM(ApprovedTotalDays) FROM EmployeeLeave WHERE EmployeeId = Employee.EmployeeId AND LeaveTypeId = 5 AND ApprovalStatus = 1 AND IsActive = 1 AND CAST(ApprovedFromDate AS DATE) >= @FromDate AND CAST(ApprovedToDate AS DATE) <= @ToDate ),0) + ISNULL((SELECT SUM(Days) FROM [EarnLeavegivenByYear] WHERE [EarnLeavegivenByYear].EmployeeId = Employee.EmployeeId GROUP BY EmployeeId), 0))AS EarnLeaveConsumed	
							--,(FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'earnleave')) + FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'previous')) - FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'consumed'))) AS PayableEarnLeaveDays
							,0  AS PayableEarnLeaveDays
							--,CONVERT(DECIMAL(18,2),((FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'earnleave')) + FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'previous')) - FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'consumed'))) * employeeSalary.GrossSalary/26)) AS PayableEarnLeaveAmount
							, 0  AS PayableEarnLeaveAmount
							,'10' Stamp
							--,(CONVERT(DECIMAL(18,2),((FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'earnleave')) + FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'previous')) - FLOOR([dbo].[fnGetEarnLeave] (Employee.EmployeeId, @fromDate, @toDate,'consumed'))) * employeeSalary.GrossSalary/26)) - 10) AS NetPayable
							, 0  AS NetPayable

							FROM [dbo].Employee
                            
							LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
							FROM EmployeeCompanyInfo AS employeeCompanyInfo 
							WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
							ON Employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
							LEFT JOIN 													
								 (SELECT EmployeeSalary.EmployeeId,EmployeeSalary.BasicSalary,EmployeeSalary.GrossSalary, 
								 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
								 FROM EmployeeSalary AS EmployeeSalary 
								 WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @toDate) AND EmployeeSalary.IsActive=1)) employeeSalary 
								 ON employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNumSal = 1  
		
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
				
							WHERE  (employee.IsActive = 1)
								AND (employee.[Status] = 2)
								AND CAST(employee.QuitDate AS DATE) between @fromDate AND @toDate
								AND DATEDIFF(MONTH,CAST(employee.JoiningDate AS DATE),@toDate)/12>=1
								AND employeeType.Id NOT IN( 1,2,3)	 -- 1 for Management Committee
								--AND branchUnit.BranchUnitId IN (1,2) --- 1 for Garments, 2 for Knitting
								--AND departmentSection.DepartmentSectionId <> 35 
								
							AND employeeCompanyInfo.rowNum = 1 
					        AND (EmployeeCardId = @EmployeeCardId OR @EmployeeCardId ='')
					        AND (company.Id=@CompanyID OR @CompanyID=-1)
							AND (branch.Id=@BranchID OR @BranchID =-1)
					        AND (branchUnit.BranchUnitId=@BranchUnitID OR @BranchUnitID =-1)
							AND (branchUnitDepartment.BranchUnitDepartmentId=@BranchUnitDepartmentID OR @BranchUnitDepartmentID =-1)
				            AND ((departmentSection.DepartmentSectionId = @DepartmentSectionId) OR (@DepartmentSectionId =-1))
					        AND ((departmentLine.DepartmentLineId = @DepartmentLineId) OR (@DepartmentLineId =-1))
					
				            AND ((employeeDesignation.Id = @EmployeeDesignationID) OR (@EmployeeDesignationID =-1))
					        AND ((employeeGrade.Id = @EmployeeGradeID) OR (@EmployeeGradeID =-1))
					        AND ((employeeType.Id = @EmployeeTypeID) OR (@EmployeeTypeID =-1))
					        AND ((employee.Name LIKE '%' + @EmployeeName + '%') OR (@EmployeeName IS NULL))
					        
					        AND ((employee.[Status] = @EmployeeStatus) OR (@EmployeeStatus IS NULL))

						    ORDER BY Employee.EmployeeCardId
					  					  														  						  											  							
END