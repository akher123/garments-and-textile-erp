-- ===================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <10/09/2016>
-- Description:	<> EXEC SpDailyAttendanceReport   1, 1, 1, NULL, NULL, NULL, NULL, '', '2020-09-20'
-- ===================================================================================================

CREATE PROCEDURE [dbo].[SpDailyAttendanceReport]
	
									
										 @CompanyId		            INT = NULL
										,@BranchId	      	        INT = NULL
										,@BranchUnitId		        INT = NULL
										,@BranchUnitDepartmentId    INT = NULL
										,@SectionId					INT = NULL
										,@LineId					INT = NULL
										,@EmployeeTypeId			INT = NULL
										,@employeeCardId			NVARCHAR(100) = NULL
										,@EffectiveDate				DATETIME 
															
AS
								 
BEGIN
	
			    SET NOCOUNT ON;
					
								TRUNCATE TABLE 	EmployeeDailyAttendance2

								INSERT INTO [dbo].[EmployeeDailyAttendance2]
									   ([EmployeeId]
									   ,[EmployeeCardId]
									   ,[TransactionDateTime]
									   ,[FunctionKey]
									   ,[IsFromMachine]
									   ,[Remarks]
									   ,[CreatedDate]
									   ,[CreatedBy]
									   ,[EditedDate]
									   ,[EditedBy]
									   ,[IsActive])
								  SELECT 
									   [EmployeeId]
									  ,[EmployeeCardId]
									  ,[TransactionDateTime]
									  ,[FunctionKey]
									  ,[IsFromMachine]
									  ,[Remarks]
									  ,[CreatedDate]
									  ,[CreatedBy]
									  ,[EditedDate]
									  ,[EditedBy]
									  ,[IsActive]
								  FROM [dbo].[EmployeeDailyAttendance]
								  WHERE CAST([TransactionDateTime] AS DATE) = CAST(@EffectiveDate AS DATE) AND IsActive = 1
								  
								  INSERT INTO [dbo].[EmployeeDailyAttendance2]
									   ([EmployeeId]
									   ,[EmployeeCardId]
									   ,[TransactionDateTime]
									   ,[FunctionKey]
									   ,[IsFromMachine]
									   ,[Remarks]
									   ,[CreatedDate]
									   ,[CreatedBy]
									   ,[EditedDate]
									   ,[EditedBy]
									   ,[IsActive])
								  SELECT 
									   [EmployeeId]
									  ,[EmployeeCardId]
									  ,[TransactionDateTime]
									  ,[FunctionKey]
									  ,[IsFromMachine]
									  ,[Remarks]
									  ,[CreatedDate]
									  ,[CreatedBy]
									  ,[EditedDate]
									  ,[EditedBy]
									  ,[IsActive]
								  FROM [dbo].[EmployeeDailyAttendance]
								  WHERE CAST([TransactionDateTime] AS DATE) = CAST(DATEADD(DAY, -1, @EffectiveDate) AS DATE) 
								  AND LEN(EmployeeCardId) > 4
								  AND CAST([TransactionDateTime] AS TIME) > '13:00:00' AND IsActive = 1
								  

								  CREATE TABLE #TableTemp 
								  ( 
										Department		NVARCHAR(100)
									   ,DepartmentId	INT
									   ,Section			NVARCHAR(100)
									   ,SectionId		INT
									   ,Line			NVARCHAR(100)
									   ,LineId			INT
									   ,Remarks			NVARCHAR(20)
								  ) 

								  INSERT INTO #TableTemp
								  (
										Department		
									   ,DepartmentId	
									   ,Section			
									   ,SectionId		
									   ,Line			
									   ,LineId			
									   ,Remarks			
								  )

							SELECT	     Department.Name							AS Department	
										,branchUnitDepartment.UnitDepartmentId		AS DepartmentId
										,Section.Name								AS Section
										,departmentSection.DepartmentSectionId      AS SectionId
										,Line.Name									AS Line
										,departmentLine.DepartmentLineId		    AS LineId																																																		
										,[dbo].[fnGetAttendanceStatusShort] (Employee.EmployeeId, @EffectiveDate, EmployeeWorkShift.EmployeeWorkShiftId) AS Remarks												
																	 																 																 																												
							FROM	    Employee employee
										LEFT JOIN 													
										(SELECT EmployeeSalary.EmployeeId,EmployeeSalary.BasicSalary,EmployeeSalary.GrossSalary, 
										ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
										FROM EmployeeSalary AS EmployeeSalary 
										WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @EffectiveDate) AND EmployeeSalary.IsActive=1)) employeeSalary 
										ON employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNumSal = 1  

										LEFT JOIN (SELECT EmployeeId, FromDate, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
										ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
										FROM EmployeeCompanyInfo AS employeeCompanyInfo 
										WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @EffectiveDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
										ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
						 			
										LEFT JOIN EmployeePresentAddress ON EmployeePresentAddress.EmployeeId = Employee.EmployeeId 
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
										LEFT JOIN EmployeeWorkShift ON EmployeeWorkShift.EmployeeId = employee.EmployeeId AND CAST(EmployeeWorkShift.ShiftDate AS DATE) = CAST(@EffectiveDate AS DATE)
																			
								WHERE	(company.Id = @CompanyId OR @CompanyId IS NULL)
										AND (branch.Id = @BranchId OR @BranchId IS NULL)
										AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId IS NULL)
										AND ((BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId IS NULL))
										AND ((employeeCompanyInfo.DepartmentSectionId = @SectionId) OR (@SectionId IS NULL))
										AND ((employeeCompanyInfo.DepartmentLineId = @LineId) OR (@LineId IS NULL))
										AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId =''))
										AND ((EmployeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId IS NULL))	
										AND EmployeeWorkShift.IsActive = 1								
										AND (employee.IsActive = 1)
										AND (employee.QuitDate IS NULL OR employee.QuitDate >= @EffectiveDate)
										AND employee.JoiningDate <= @EffectiveDate							
										AND employeeType.Id <> 1								
										AND branchUnit.BranchUnitId IN (1,2,3)					
										AND departmentSection.DepartmentSectionId NOT IN (35)


								  TRUNCATE TABLE DailyPresentReport	

								  INSERT INTO [dbo].[DailyPresentReport]										
								  (  
									   [DepartmentName]
									  ,[DepartmentId]
									  ,[SectionName]
									  ,[SectionId]
									  ,[LineName]
									  ,[LineId]
									  ,[TotalEmployee]									
									  ,IsActive
								  )
								 SELECT  Department
								  		,DepartmentId
										,Section
										,SectionId
										,Line
										,LineId
										,COUNT(Remarks)										 
										,1															
									FROM	#TableTemp
									GROUP BY Department, DepartmentId, Section, SectionId, Line, LineId									    
								
								
									UPDATE DailyPresentReport  -- update budget manpower by department
									SET DailyPresentReport.BudgetManPower = ManPowerApprovedByDepartment.ApprovedManPower
									FROM DailyPresentReport 
									INNER JOIN ManPowerApprovedByDepartment ON DailyPresentReport.DepartmentId = ManPowerApprovedByDepartment.DepartmentId
							
									UPDATE DailyPresentReport	-- update budget manpower by section
									SET DailyPresentReport.BudgetManPower = ManPowerApprovedBySection.ApprovedManPower
									FROM DailyPresentReport 
									INNER JOIN ManPowerApprovedBySection ON DailyPresentReport.SectionId = ManPowerApprovedBySection.SectionId	
								  
								   
								    UPDATE DailyPresentReport  -- Present status update by department
									SET DailyPresentReport.Present = PresentTable.TotalPresent
									FROM DailyPresentReport 
									INNER JOIN 
								    (
										SELECT COUNT(1) AS TotalPresent
											  ,SectionId
											  											
										FROM #TableTemp 
										WHERE Remarks IN('Present', 'Late') AND LineId IS NULL
										GROUP BY SectionId
								    ) AS PresentTable ON PresentTable.SectionId = DailyPresentReport.SectionId 


									UPDATE DailyPresentReport  -- late status update by department
									SET DailyPresentReport.Late = PresentTable.TotalLate
									FROM DailyPresentReport 
									INNER JOIN 
								    (
										SELECT COUNT(1) AS TotalLate
											  ,SectionId
											  											
										FROM #TableTemp 
										WHERE Remarks = 'Late' AND LineId IS NULL
										GROUP BY SectionId
								    ) AS PresentTable ON PresentTable.SectionId = DailyPresentReport.SectionId


									UPDATE DailyPresentReport  -- absent status update by department
									SET DailyPresentReport.Absent = PresentTable.TotalAbsent
									FROM DailyPresentReport 
									INNER JOIN 
								    (
										SELECT COUNT(1) AS TotalAbsent
											  ,SectionId
											  											
										FROM #TableTemp 
										WHERE Remarks = 'Absent' AND LineId IS NULL
										GROUP BY SectionId
								    ) AS PresentTable ON PresentTable.SectionId = DailyPresentReport.SectionId


									UPDATE DailyPresentReport  -- leave status update by department
									SET DailyPresentReport.Leave = PresentTable.TotalLeave
									FROM DailyPresentReport 
									INNER JOIN 
								    (
										SELECT COUNT(1) AS TotalLeave
											  ,SectionId
											  											
										FROM #TableTemp 
										WHERE Remarks = 'Leave' AND LineId IS NULL
										GROUP BY SectionId
								    ) AS PresentTable ON PresentTable.SectionId = DailyPresentReport.SectionId


									UPDATE DailyPresentReport									 
										SET	 BudgetManPower = 0											
											,Present = 0
											,Late = 0
											,Absent = 0
											,Leave = 0
									WHERE LineId IS NOT NULL


									UPDATE DailyPresentReport  -- Present status update by line
									SET DailyPresentReport.Present = PresentTable.TotalPresent
									FROM DailyPresentReport 
									INNER JOIN 
								    (
										SELECT COUNT(1) AS TotalPresent
											  ,LineId
											  											
										FROM #TableTemp 
										WHERE Remarks IN('Present', 'Late')
										GROUP BY LineId
								    ) AS PresentTable ON PresentTable.LineId = DailyPresentReport.LineId AND PresentTable.LineId IS NOT NULL


									UPDATE DailyPresentReport  -- Late status update by line
									SET DailyPresentReport.Late = PresentTable.TotalLate
									FROM DailyPresentReport 
									INNER JOIN 
								    (
										SELECT COUNT(1) AS TotalLate
											  ,LineId
											  											
										FROM #TableTemp 
										WHERE Remarks = 'Late'
										GROUP BY LineId
								    ) AS PresentTable ON PresentTable.LineId = DailyPresentReport.LineId AND PresentTable.LineId IS NOT NULL


									UPDATE DailyPresentReport  -- absent status update by line
									SET DailyPresentReport.Absent = PresentTable.TotalAbsent
									FROM DailyPresentReport 
									INNER JOIN 
								    (
										SELECT COUNT(1) AS TotalAbsent
											  ,LineId
											  											
										FROM #TableTemp 
										WHERE Remarks = 'Absent'
										GROUP BY LineId
								    ) AS PresentTable ON PresentTable.LineId = DailyPresentReport.LineId AND PresentTable.LineId IS NOT NULL


									UPDATE DailyPresentReport  -- leave status update by line
									SET DailyPresentReport.Leave = PresentTable.TotalLeave
									FROM DailyPresentReport 
									INNER JOIN 
								    (
										SELECT COUNT(1) AS TotalLeave
											  ,LineId
											  											
										FROM #TableTemp 
										WHERE Remarks = 'Leave'
										GROUP BY LineId
								    ) AS PresentTable ON PresentTable.LineId = DailyPresentReport.LineId AND PresentTable.LineId IS NOT NULL
							

									UPDATE DailyPresentReport									 											 										
									SET	 Present = ISNULL(Present, 0)
										,Late = ISNULL(Late, 0)
										,Absent = ISNULL(Absent, 0)
										,Leave = ISNULL(Leave, 0)


									UPDATE DailyPresentReport
									SET [Percent] = CAST(CAST(Present AS FLOAT) / CAST(TotalEmployee AS FLOAT) * 100 AS NUMERIC(18,2))


									IF EXISTS( SELECT 1 FROM DailyPresentReport WHERE DepartmentId = 6)
									BEGIN

											  INSERT INTO [dbo].[DailyPresentReport]
															   ([DepartmentName]
															   ,[DepartmentId]
															   ,[SectionName]
															   ,[SectionId]
															   ,[LineName]
															   ,[LineId]
															   ,[BudgetManPower]
															   ,[TotalEmployee]
															   ,[Present]
															   ,[Late]
															   ,[Absent]
															   ,[Leave]
															   ,[Balance]
															   ,[Percent]													
															   ,[IsActive])
															VALUES													   
																( 'Production'
																,6
																,'Sewing'
																,6
																,'SEWING TOTAL'
																,25
																,100
																,90
																,80
																,10
																,0
																,0
																,-10
																,0											
																,1)
													   

											UPDATE DailyPresentReport
											SET BudgetManPower = (SELECT ApprovedManPower FROM ManPowerApprovedBySection WHERE SectionId = 6)
											   ,TotalEmployee = (SELECT COUNT(1) FROM #TableTemp WHERE SectionId = 6)
											   ,Present = (SELECT COUNT(1) FROM #TableTemp WHERE Remarks IN('Present', 'Late') AND SectionId = 6)
											   ,Late = (SELECT COUNT(1) FROM #TableTemp WHERE Remarks = 'Late' AND SectionId = 6)
											   ,[Absent] = (SELECT COUNT(1) FROM #TableTemp WHERE Remarks = 'Absent' AND SectionId = 6)
											   ,Leave = (SELECT COUNT(1) FROM #TableTemp WHERE Remarks = 'Leave' AND SectionId = 6)									 
											WHERE LineId  = 25

											UPDATE DailyPresentReport
												SET [Balance] = BudgetManPower - TotalEmployee
												   ,[Percent] = CAST(CAST((Present) AS FLOAT)/TotalEmployee * 100 AS NUMERIC(18,2))
											WHERE LineId  = 25
									  END


									  --******************
									  UPDATE DailyPresentReport SET LineId = LineId + 1 where (DepartmentId = 6) AND (SectionId = 6) AND LineId >= 8

									  INSERT INTO DailyPresentReport
									  (DepartmentName, DepartmentId, SectionName, SectionId, LineName, LineId, BudgetManPower, TotalEmployee, Present, Late, Absent, Leave, [Percent], IsActive)
									  SELECT DepartmentName, DepartmentId, SectionName, SectionId, '1 to 6 Total' AS LineName, 8 AS LineId, SUM(BudgetManPower) AS BudgetManPower, SUM(TotalEmployee) AS TotalEmployee, SUM(Present) AS Present, SUM(Late) 
											 AS Late, SUM(Absent) AS Absent, SUM(Leave) AS Leave, 0 AS [Percent], 1 AS IsActive
									  FROM DailyPresentReport AS DailyPresentReport_1
									  WHERE (DepartmentId = 6) AND (SectionId = 6)  AND LineId BETWEEN 2 AND 7
									  GROUP BY DepartmentName, DepartmentId, SectionName, SectionId


									  UPDATE DailyPresentReport SET LineId = LineId + 1 where (DepartmentId = 6) AND (SectionId = 6) AND LineId > 16


									  INSERT INTO DailyPresentReport
									  (DepartmentName, DepartmentId, SectionName, SectionId, LineName, LineId, BudgetManPower, TotalEmployee, Present, Late, Absent, Leave, [Percent], IsActive)
									  SELECT DepartmentName, DepartmentId, SectionName, SectionId, '7 TO 14 TOTAL' AS LineName, 17 AS LineId, SUM(BudgetManPower) AS BudgetManPower, SUM(TotalEmployee) AS TotalEmployee, SUM(Present) AS Present, SUM(Late) 
									  AS Late, SUM(Absent) AS Absent, SUM(Leave) AS Leave, 0 AS [Percent], 1 AS IsActive
									  FROM DailyPresentReport AS DailyPresentReport_1
									  WHERE (DepartmentId = 6) AND (SectionId = 6) AND LineId BETWEEN 9 AND 16
									  GROUP BY DepartmentName, DepartmentId, SectionName, SectionId


									  UPDATE DailyPresentReport SET LineId = LineId + 1 WHERE (DepartmentId = 6) AND (SectionId = 6) AND LineId > 21


									  INSERT INTO DailyPresentReport
								      (DepartmentName, DepartmentId, SectionName, SectionId, LineName, LineId, BudgetManPower, TotalEmployee, Present, Late, Absent, Leave, [Percent], IsActive)
									  SELECT DepartmentName, DepartmentId, SectionName, SectionId, '15 TO 18 TOTAL' AS LineName, 22 AS LineId, SUM(BudgetManPower) AS BudgetManPower, SUM(TotalEmployee) AS TotalEmployee, SUM(Present) AS Present, SUM(Late) 
									  AS Late, SUM(Absent) AS Absent, SUM(Leave) AS Leave, 0 AS [Percent], 1 AS IsActive
									  FROM  DailyPresentReport AS DailyPresentReport_1
									  WHERE (DepartmentId = 6) AND (SectionId = 6) AND LineId BETWEEN  18 AND 21
									  GROUP BY DepartmentName, DepartmentId, SectionName, SectionId									 


									  INSERT INTO DailyPresentReport
								      (DepartmentName, DepartmentId, SectionName, SectionId, LineName, LineId, BudgetManPower, TotalEmployee, Present, Late, Absent, Leave, [Percent], IsActive)
									  SELECT DepartmentName, DepartmentId, SectionName, SectionId, '19 TO 21 TOTAL' AS LineName, 27 AS LineId, SUM(BudgetManPower) AS BudgetManPower, SUM(TotalEmployee) AS TotalEmployee, SUM(Present) AS Present, SUM(Late) 
									  AS Late, SUM(Absent) AS Absent, SUM(Leave) AS Leave, 0 AS [Percent], 1 AS IsActive
									  FROM  DailyPresentReport AS DailyPresentReport_1
									  WHERE (DepartmentId = 6) AND (SectionId = 6) AND LineId BETWEEN  23 AND 26
									  GROUP BY DepartmentName, DepartmentId, SectionName, SectionId


									  --INSERT INTO DailyPresentReport
								   --   (DepartmentName, DepartmentId, SectionName, SectionId, LineName, LineId, BudgetManPower, TotalEmployee, Present, Late, Absent, Leave, [Percent], IsActive)
									  --SELECT DepartmentName, DepartmentId, SectionName, SectionId, '19 TO 21 TOTAL' AS LineName, 27 AS LineId, SUM(BudgetManPower) AS BudgetManPower, SUM(TotalEmployee) AS TotalEmployee, SUM(Present) AS Present, SUM(Late) 
									  --AS Late, SUM(Absent) AS Absent, SUM(Leave) AS Leave, 0 AS [Percent], 1 AS IsActive
									  --FROM  DailyPresentReport AS DailyPresentReport_1
									  --WHERE (DepartmentId = 6) AND (SectionId = 6) AND LineId BETWEEN  23 AND 26
									  --GROUP BY DepartmentName, DepartmentId, SectionName, SectionId


									  UPDATE DailyPresentReport
												SET 
												    [Percent] = CAST(CAST((Present) AS FLOAT)/TotalEmployee * 100 AS NUMERIC(18,2))
											WHERE LineId  BETWEEN 2 AND 28


									  --******************

									  SELECT [DailyPresentReportId]
											,[DepartmentName]
											,[DepartmentId]
											,[SectionName]
											,[SectionId]
											,[LineName]
											,[LineId]
											,[BudgetManPower]
											,[TotalEmployee]
											,[Present]
											,[Late]
											,[Absent]
											,[Leave]
											,[Balance]
											,[Percent]									
										FROM [dbo].[DailyPresentReport]
										ORDER BY [SectionId]
					 									 													  					  														  						  											  							
END