-- ===============================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <10/09/2016>
-- Description:	<> EXEC SPDailyAttendanceCombined  0, 0, 0, 0, 0, 0, 1, '', '2017-10-24', '2017-09-26'
-- ===============================================================================================================

CREATE PROCEDURE [dbo].[SPDailyAttendanceCombined]
	
									
									 @CompanyId		            INT = 0
									,@BranchId	      	        INT = 0
									,@BranchUnitId		        INT = 0
									,@BranchUnitDepartmentId    INT = 0
									,@SectionId					INT = 0
									,@LineId					INT = 0
									,@EmployeeTypeId			INT = NULL
									,@employeeCardId			NVARCHAR(100) = NULL
								    ,@EffectiveDate				DATETIME 
									,@FromDate					DATETIME 
															
AS
								 
BEGIN
	
			    SET NOCOUNT ON;
						
						DECLARE @EmployeeType NVARCHAR(50) = NULL

						IF(@EmployeeTypeId = 2)
							SET @EmployeeType = 'Management'
						ELSE 							
							SET @EmployeeType = 'Team Member'
						
						
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
							

						  CREATE TABLE #TempTable  
					      ( 
								UnitName			NVARCHAR(100)
							   ,EmployeeCardId		NVARCHAR(100)
							   ,EmployeeName		NVARCHAR(100)
							   ,Designation			NVARCHAR(100)
							   ,DesignationId		INT
							   ,Department			NVARCHAR(100)
							   ,Section				NVARCHAR(100)
							   ,SectionId			INT
							   ,Line				NVARCHAR(100)
							   ,Type				NVARCHAR(100)
							   ,Date				DATETIME
							   ,MobilePhone			NVARCHAR(100)
							   ,Remarks				NVARCHAR(100)
							   ,Gender				NVARCHAR(100)
							   ,ServiceStatus		NVARCHAR(100)
					       ) 	
						
						  --CREATE TABLE ManPowerApprovedSummary 
						  --(
								--UnitName		NVARCHAR(100)
							 --  ,SectionName		NVARCHAR(100)
							 --  ,SectionId		INT
							 --  ,Designation		NVARCHAR(100)
							 --  ,DesignationId	INT
							 --  ,Regular			INT
							 --  ,NewJoin		    INT
							 --  ,Total			INT
							 --  ,Present			INT
							 --  ,Absent			INT
							 --  ,Leave			INT
							 --  ,Approved		INT
							 --  ,Male			INT
							 --  ,Female			INT
							 --  ,AbsentPercent	FLOAT
						  -- )

						TRUNCATE TABLE ManPowerApprovedSummary

						INSERT INTO ManPowerApprovedSummary(UnitName, SectionName, SectionId, Designation, DesignationId)
											
						SELECT	 Unit.Name						
								,Section.Name
								,Section.SectionId
								,EmployeeDesignation.Title
								,EmployeeDesignation.Id		
																																																			 																 																												
						FROM	     Employee employee
									 LEFT JOIN 													
									 (SELECT EmployeeSalary.EmployeeId,EmployeeSalary.BasicSalary,EmployeeSalary.GrossSalary, 
									 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
									 FROM EmployeeSalary AS EmployeeSalary 
									 WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @EffectiveDate) AND EmployeeSalary.IsActive=1)) employeeSalary 
									 ON employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNumSal = 1  

									 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
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
						       
							WHERE	(company.Id = @CompanyId OR @CompanyId = 0)
									AND (branch.Id = @BranchId OR @BranchId = 0)
									AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = 0)
									AND ((BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId = 0))
									AND ((employeeCompanyInfo.DepartmentSectionId = @SectionId) OR (@SectionId = 0))
									AND ((employeeCompanyInfo.DepartmentLineId = @LineId) OR (@LineId = 0))
									AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId =''))
							
									AND EmployeeWorkShift.IsActive = 1								
									AND (employee.IsActive = 1)
									AND (employee.[Status] = 1)							
									AND employeeType.Id NOT IN (1)										
									AND branchUnit.BranchUnitId IN (1,2)					--- 1 for Garments, 2 for Knitting
									AND departmentSection.DepartmentSectionId NOT IN (35)	--- Not for security
									AND EmployeeType.Title LIKE '%' + @EmployeeType + '%'
								    GROUP BY Unit.Name, Section.Name, Section.SectionId, EmployeeDesignation.Title, EmployeeDesignation.Id				
					
				
						INSERT INTO  #TempTable																																																																																				    
						SELECT	      Unit.Name						AS UnitName
									 ,Employee.EmployeeCardId	
									 ,Employee.Name					AS EmployeeName		
									 ,EmployeeDesignation.Title		AS Designation	
									 ,EmployeeDesignation.Id									
									 ,Department.Name				AS Department	
									 ,Section.Name					AS Section	
									 ,Section.SectionId
									 ,Line.Name						AS Line
									 ,EmployeeType.Title			AS Type																	
									 ,@EffectiveDate				AS Date							
									 ,EmployeePresentAddress.MobilePhone
									 ,[dbo].[fnGetAttendanceStatusShort] (Employee.EmployeeId, @EffectiveDate,EmployeeWorkShift.EmployeeWorkShiftId) AS Remarks
									 ,Gender.Title AS Gender
						 			 ,'ServiceStatus' = 
										  CASE   
											 WHEN employee.JoiningDate BETWEEN @FromDate AND @EffectiveDate THEN 'NewJoin'  
											 ELSE 'Regular'  											
										  END 	
																																										 																 																												
						FROM	     Employee employee
									 LEFT JOIN 													
									 (SELECT EmployeeSalary.EmployeeId,EmployeeSalary.BasicSalary,EmployeeSalary.GrossSalary, 
									 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
									 FROM EmployeeSalary AS EmployeeSalary 
									 WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @EffectiveDate) AND EmployeeSalary.IsActive=1)) employeeSalary 
									 ON employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNumSal = 1  

									 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
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
						       
							WHERE	(company.Id = @CompanyId OR @CompanyId = 0)
									AND (branch.Id = @BranchId OR @BranchId = 0)
									AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = 0)
									AND ((BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId = 0))
									AND ((employeeCompanyInfo.DepartmentSectionId = @SectionId) OR (@SectionId = 0))
									AND ((employeeCompanyInfo.DepartmentLineId = @LineId) OR (@LineId = 0))
									AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId =''))							
									AND EmployeeWorkShift.IsActive = 1								
									AND (employee.IsActive = 1)
									AND (employee.[Status] = 1)							
									AND employeeType.Id NOT IN (1)										
									AND branchUnit.BranchUnitId IN (1,2)					--- 1 for Garments, 2 for Knitting
									AND departmentSection.DepartmentSectionId NOT IN (35)	--- Not for security
									AND EmployeeType.Title LIKE '%' + @EmployeeType + '%'
								    		
									ORDER BY Employee.EmployeeCardId	


				----------------------- Now UPDATE Each Coloumn -----------------------------																																																																		
									
									UPDATE ManPowerApprovedSummary 
									SET ManPowerApprovedSummary.Regular = TempTable.Regular	 
									FROM ManPowerApprovedSummary INNER JOIN 
									(	   
										SELECT Sectionid
											  ,DesignationId
											  ,SUM(1) AS Regular 
										FROM #TempTable 
										WHERE ServiceStatus = 'Regular'
										GROUP BY Sectionid, DesignationId
									) AS TempTable ON ManPowerApprovedSummary.SectionId = TempTable.SectionId AND ManPowerApprovedSummary.DesignationId = TempTable.DesignationId


									UPDATE ManPowerApprovedSummary 
									SET ManPowerApprovedSummary.NewJoin = TempTable.NewJoin	 
									FROM ManPowerApprovedSummary INNER JOIN 
									(	   
										SELECT Sectionid
											  ,DesignationId
											  ,SUM(1) AS NewJoin 
										FROM #TempTable 
										WHERE ServiceStatus = 'NewJoin'
										GROUP BY Sectionid, DesignationId
									) AS TempTable ON ManPowerApprovedSummary.SectionId = TempTable.SectionId AND ManPowerApprovedSummary.DesignationId = TempTable.DesignationId


									UPDATE ManPowerApprovedSummary 
									SET ManPowerApprovedSummary.Total = TempTable.Total	 
									FROM ManPowerApprovedSummary INNER JOIN 
									(	   
										SELECT Sectionid
											  ,DesignationId
											  ,SUM(1) AS Total 
										FROM #TempTable 
										GROUP BY Sectionid, DesignationId
									) AS TempTable ON ManPowerApprovedSummary.SectionId = TempTable.SectionId AND ManPowerApprovedSummary.DesignationId = TempTable.DesignationId


									UPDATE ManPowerApprovedSummary 
									SET ManPowerApprovedSummary.Present = TempTable.Present	 
									FROM ManPowerApprovedSummary INNER JOIN 
									(	   
										SELECT Sectionid
											  ,DesignationId
											  ,SUM(1) AS Present 
										FROM #TempTable 
										WHERE Remarks = 'Present' OR Remarks = 'Late' OR Remarks = 'OSD' OR Remarks = 'Weekend'
										GROUP BY Sectionid, DesignationId
									) AS TempTable ON ManPowerApprovedSummary.SectionId = TempTable.SectionId AND ManPowerApprovedSummary.DesignationId = TempTable.DesignationId


									UPDATE ManPowerApprovedSummary 
									SET ManPowerApprovedSummary.Absent = TempTable.Absent	 
									FROM ManPowerApprovedSummary INNER JOIN 
									(	   
										SELECT Sectionid
											  ,DesignationId
											  ,SUM(1) AS Absent 
										FROM #TempTable 
										WHERE Remarks = 'Absent'
										GROUP BY Sectionid, DesignationId
									) AS TempTable ON ManPowerApprovedSummary.SectionId = TempTable.SectionId AND ManPowerApprovedSummary.DesignationId = TempTable.DesignationId


									UPDATE ManPowerApprovedSummary 
									SET ManPowerApprovedSummary.Leave = TempTable.Leave	 
									FROM ManPowerApprovedSummary INNER JOIN 
									(	   
										SELECT Sectionid
											  ,DesignationId
											  ,SUM(1) AS Leave 
										FROM #TempTable 
										WHERE Remarks = 'Leave'
										GROUP BY Sectionid, DesignationId
									) AS TempTable ON ManPowerApprovedSummary.SectionId = TempTable.SectionId AND ManPowerApprovedSummary.DesignationId = TempTable.DesignationId


									-- Approved Employee will come from ManPowerApproved Table --

									UPDATE ManPowerApprovedSummary 
									SET ManPowerApprovedSummary.Approved = TempTable.Total	 
									FROM ManPowerApprovedSummary INNER JOIN 
									(	   
										SELECT Sectionid
											  ,DesignationId
											  ,SUM(1) AS Total 
										FROM #TempTable 
										GROUP BY Sectionid, DesignationId
									) AS TempTable ON ManPowerApprovedSummary.SectionId = TempTable.SectionId AND ManPowerApprovedSummary.DesignationId = TempTable.DesignationId


									UPDATE ManPowerApprovedSummary 
									SET ManPowerApprovedSummary.Male = TempTable.Male	 
									FROM ManPowerApprovedSummary INNER JOIN 
									(	   
										SELECT Sectionid
											  ,DesignationId
											  ,SUM(1) AS Male 
										FROM #TempTable 
										WHERE Gender = 'Male'
										GROUP BY Sectionid, DesignationId
									) AS TempTable ON ManPowerApprovedSummary.SectionId = TempTable.SectionId AND ManPowerApprovedSummary.DesignationId = TempTable.DesignationId


									UPDATE ManPowerApprovedSummary 
									SET ManPowerApprovedSummary.Female = TempTable.Female	 
									FROM ManPowerApprovedSummary INNER JOIN 
									(	   
										SELECT Sectionid
											  ,DesignationId
											  ,SUM(1) AS Female 
										FROM #TempTable 
										WHERE Gender = 'Female'
										GROUP BY Sectionid, DesignationId
									) AS TempTable ON ManPowerApprovedSummary.SectionId = TempTable.SectionId AND ManPowerApprovedSummary.DesignationId = TempTable.DesignationId
									
										
																			
									SELECT * FROM ManPowerApprovedSummary
													 									 													  					  														  						  											  							
END