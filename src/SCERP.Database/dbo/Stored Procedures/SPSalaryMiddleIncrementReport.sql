

-- ==========================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2016-11-22>					
-- Description:	<> EXEC [SPSalaryMiddleIncrementReport] 1, 1, -1, -1, -1, -1, '2017-07-26','2017-08-25', -1, '','superadmin', 5, 0
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPSalaryMiddleIncrementReport]

					    @CompanyId INT = -1
					   ,@BranchId INT = -1
					   ,@BranchUnitId INT = -1
					   ,@BranchUnitDepartmentId INT = -1
					   ,@DepartmentSectionId INT = -1 
					   ,@DepartmentLineId INT = -1
					   ,@FromDate DATETIME = NULL
					   ,@ToDate DATETIME = NULL				
					   ,@EmployeeTypeId INT = -1
					   ,@EmployeeCardId NVARCHAR(100) = ''
					   ,@UserName NVARCHAR(100) = ''
					   ,@IncrementPercent FLOAT
					   ,@OtherIncrement	DECIMAL(18,2) = 0.0
AS
BEGIN
			
						SET XACT_ABORT ON;
						SET NOCOUNT ON;
		 			
						IF(@FromDate IS NULL)
						BEGIN
							SET @FromDate = CAST(CURRENT_TIMESTAMP AS DATE)
						END
						ELSE
						BEGIN
							SET @FromDate = CAST(@FromDate AS DATE)
						END
		
						IF(@ToDate IS NULL)
						BEGIN
						SET @ToDate = CAST(CURRENT_TIMESTAMP AS DATE)
						END
						ELSE
						BEGIN
							SET @ToDate = CAST(@ToDate AS DATE)
						END


						
						Select A.EmployeeCardId, A.EmployeeId, A.Name, CAST(JoiningDate as DATE) as JoiningDate, PreSal as PreviousSalary, PreSal*0.05 as PIncrement, PreSal*1.05 ProposedSalary, CurSal as CurrentSalary, PreSal*1.05-CurSal as ProposedIncrement From (select EmployeeCardId,EmployeeId,Name,JoiningDate, 
						--(SELECT        SUM(BasicSalary) AS Expr1
                         --FROM            EmployeeSalary
                           --WHERE        (EmployeeId = Employee.EmployeeId) AND (FromDate < '2016-05-26') ) as PSal,  (SELECT        SUM(BasicSalary) AS Expr1
                            --FROM            EmployeeSalary
                            -- WHERE        (EmployeeId = Employee.EmployeeId) AND (FromDate <= '2017-05-25') ) as CSal
							(select  BasicSalary from (SELECT EmployeeId, GrossSalary,  BasicSalary,HouseRent, MedicalAllowance, Conveyance,FoodAllowance,EntertainmentAllowance,FromDate,												
							  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
							  FROM EmployeeSalary  
							  WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @ToDate)) AS EmployeeSal where rowNum=1 and EmployeeId=Employee.EmployeeId) AS CurSal,
							  (select  BasicSalary from (SELECT EmployeeId, GrossSalary,  BasicSalary,HouseRent, MedicalAllowance, Conveyance,FoodAllowance,EntertainmentAllowance,FromDate,												
							  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
							  FROM EmployeeSalary  
							  WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @ToDate)) AS EmployeeSal where rowNum=2 and EmployeeId=Employee.EmployeeId) AS PreSal
                            ,IsActive ,Status, QuitDate
                            FROM [dbo].[Employee] where year(JoiningDate) < 2017 and ( month(JoiningDate)*100+day(JoiningDate)>= month(@FromDate)*100+day(@FromDate) and month(JoiningDate)*100+day(JoiningDate) <month(@ToDate)*100+day(@ToDate) ) and IsActive=1 ) as A				
							LEFT JOIN 
							
							
							(SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,IsEligibleForOvertime,
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
							FROM EmployeeCompanyInfo AS employeeCompanyInfo 
							WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
							ON A.EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1

						     

							LEFT JOIN EmployeePresentAddress presentAddress ON A.EmployeeId = presentAddress.EmployeeId AND presentAddress.IsActive = 1 									
							LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id
							LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
							LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
							LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
							LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId = branchUnit.BranchUnitId
							LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId = unitDepartment.UnitDepartmentId
							LEFT JOIN Unit  AS unit ON branchUnit.UnitId = unit.UnitId
							LEFT JOIN Department  AS department ON unitDepartment.DepartmentId = department.Id
							LEFT JOIN Branch  AS branch ON branchUnit.BranchId = branch.Id
							LEFT JOIN Company  AS company ON branch.CompanyId = company.Id		
							LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
							LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
							LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
							LEFT JOIN Line line on departmentLine.LineId = line.LineId

							WHERE (A.IsActive = 1
							AND ((A.[Status] = 1) )
							AND A.PreSal is not null
							AND A.PreSal*1.05-A.CurSal>1
							--AND A.PreSal!=A.CurSal
							    --OR 
							--((A.[Status] = 2) AND (A.QuitDate >= @FromDate) AND (A.QuitDate <= (DATEADD(DAY, 30, @ToDate)))))
								--AND A.PSal!=A.CSal
							--AND (company.Id = @CompanyId OR @CompanyId = -1)
							--AND (branch.Id = @BranchId OR @BranchId = -1)
							--AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = -1)
							--AND (branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId = -1)
							--AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId = -1)
							--AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId = -1)			 							
							--AND (employeeType.Id = @EmployeeTypeId OR @EmployeeTypeId = -1)

							--AND (A.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId ='')		
							--AND (CAST(A.JoiningDate AS DATE) <= CAST(@ToDate AS DATE))			
							AND employeeType.Id <> 1	
							AND employeeType.Title LIKE '%Team Member%')
							--AND section.Name NOT LIKE '%Security%')


END