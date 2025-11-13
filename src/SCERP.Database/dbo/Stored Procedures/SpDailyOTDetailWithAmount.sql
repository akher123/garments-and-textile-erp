-- =============================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <10/09/2018>
-- Description:	<> EXEC SpDailyOTDetailWithAmount  1, 1, 1, 6, 6, NULL, NULL, '', '2018-09-05'
-- =============================================================================================

CREATE PROCEDURE [dbo].[SpDailyOTDetailWithAmount]
									
									
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
							
						  TRUNCATE TABLE EmployeeInOutTemp
						  
						  INSERT INTO [dbo].[EmployeeInOutTemp]
										   ([CompanyId]
										   ,[CompanyName]
										   ,[CompanyAddress]
										   ,[BranchId]
										   ,[BranchName]
										   ,[BranchUnitId]
										   ,[UnitName]
										   ,[BranchUnitDepartmentId]
										   ,[DepartmentName]
										   ,[DepartmentSectionId]
										   ,[SectionName]
										   ,[DepartmentLineId]
										   ,[LineName]
										   ,[EmployeeId]
										   ,[EmployeeCardId]
										   ,[EmployeeName]
										   ,[EmployeeTypeId]
										   ,[EmployeeType]
										   ,[EmployeeGradeId]
										   ,[EmployeeGrade]
										   ,[EmployeeDesignationId]
										   ,[EmployeeDesignation]
										   ,[JoiningDate]
										   ,[QuitDate]
										   ,[BranchUnitWorkShiftId]
										   ,[WorkShiftName]
										   ,[EmployeeWorkShiftId]
										   ,[MobileNo]
										   ,[TransactionDate]
										   ,[InTime]
										   ,[OutTime]
										   ,[LastDayOutTime]
										   ,[Status]
										   ,[LateInMinutes]
										   ,[TotalContinuousAbsentDays]
										   ,[OTHours]
										   ,[LastDayOTHours]
										   ,[ExtraOTHours]
										   ,[LastDayExtraOTHours]
										   ,[WeekendOTHours]
										   ,[HolidayOTHours]
										   ,[Remarks]
										   ,[CreatedDate]
										   ,[CreatedBy]
										   ,[IsActive])
						  			SELECT 
										   [CompanyId]
										  ,[CompanyName]
										  ,[CompanyAddress]
										  ,[BranchId]
										  ,[BranchName]
										  ,[BranchUnitId]
										  ,[UnitName]
										  ,[BranchUnitDepartmentId]
										  ,[DepartmentName]
										  ,[DepartmentSectionId]
										  ,[SectionName]
										  ,[DepartmentLineId]
										  ,[LineName]
										  ,[EmployeeId]
										  ,[EmployeeCardId]
										  ,[EmployeeName]
										  ,[EmployeeTypeId]
										  ,[EmployeeType]
										  ,[EmployeeGradeId]
										  ,[EmployeeGrade]
										  ,[EmployeeDesignationId]
										  ,[EmployeeDesignation]
										  ,[JoiningDate]
										  ,[QuitDate]
										  ,[BranchUnitWorkShiftId]
										  ,[WorkShiftName]
										  ,[EmployeeWorkShiftId]
										  ,[MobileNo]
										  ,[TransactionDate]
										  ,[InTime]
										  ,[OutTime]
										  ,[LastDayOutTime]
										  ,[Status]
										  ,[LateInMinutes]
										  ,[TotalContinuousAbsentDays]
										  ,[OTHours]
										  ,[LastDayOTHours]
										  ,[ExtraOTHours]
										  ,[LastDayExtraOTHours]
										  ,[WeekendOTHours]
										  ,[HolidayOTHours]
										  ,[Remarks]
										  ,[CreatedDate]
										  ,[CreatedBy]
										  ,[IsActive]
									  FROM [dbo].[EmployeeInOut]
									  WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND BranchUnitId IN(1,2) 					   								 	
				
						  CREATE TABLE #TableTemp 
						  ( 			
								 Date						DATE					
								,Section					NVARCHAR(100)
								,SectionId					INT
								,Line						NVARCHAR(100)
								,LineId						INT

								,RegularOTEmployee			INT
								,RegularOT					DECIMAL(10,2)
								,RegularOTAmount			DECIMAL(10,2)

								,ExtraOTEmployee			INT
								,ExtraOT					DECIMAL(10,2)
								,ExtraOTAmount				DECIMAL(10,2)

								,OthersOTEmployee			INT
								,OthersOT					DECIMAL(10,2)
								,OthersOTAmount				DECIMAL(10,2)							
						  ) 	 		 
					 						 						 
						 INSERT INTO #TableTemp
						 (
								 Date
								,Section			
								,SectionId		
								,Line			
								,LineId		
									
								,RegularOTEmployee			
								,RegularOT					
								,RegularOTAmount			

								,ExtraOTEmployee			
								,ExtraOT					
								,ExtraOTAmount			

								,OthersOTEmployee			
								,OthersOT					
								,OthersOTAmount				
						  )	
						
					SELECT		  TransactionDate													
								 ,Section.Name									AS Section					
								 ,departmentSection.DepartmentSectionId			AS SectionId
								 ,line.Name										AS Line
								 ,line.LineId									AS LineId	
								 ,0	
								 ,0
								 ,0
								 ,0
								 ,0
								 ,0
								 ,0
								 ,0
								 ,0
						 						 											 					 																											 					
					FROM		 EmployeeInOutTemp					 
								 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
								 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
								 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
								 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @EffectiveDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
								 ON EmployeeInOutTemp.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
					
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
																				  						       								 						  
						WHERE	 CAST(TransactionDate AS DATE) = @EffectiveDate
								 AND (OTHours > 0 OR ExtraOTHours > 0 OR WeekendOTHours > 0 OR HolidayOTHours > 0)
								 AND branchUnit.BranchUnitId IN(1,2)
					

						GROUP BY TransactionDate
								,Department.Name
								,Department.Id
								,Section.Name
								,departmentSection.DepartmentSectionId
								,line.LineId	
								,line.Name
						

						-- Update Line
								UPDATE #TableTemp
								SET RegularOTEmployee = (SELECT ISNULL(COUNT(1), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND OTHours > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentLineId = #TableTemp.LineId)
						
								UPDATE #TableTemp
								SET RegularOT = (SELECT ISNULL(SUM(OTHours), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND OTHours > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentLineId = #TableTemp.LineId)

								UPDATE #TableTemp
								SET RegularOTAmount  =  (SELECT ISNULL(SUM(OTHours * BasicSalary/104), 0) FROM EmployeeInOutTemp LEFT JOIN
														(SELECT EmployeeId, BasicSalary, GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						
														FROM EmployeeSalary AS employeeSalary
														WHERE ((CAST(employeeSalary.FromDate AS Date) <= @EffectiveDate) OR (@EffectiveDate IS NULL))
														AND employeeSalary.IsActive = 1) employeeSalaryInfo 
														ON EmployeeInOutTemp.EmployeeId = employeeSalaryInfo.EmployeeId AND employeeSalaryInfo.rowNumSal = 1
														WHERE CAST(TransactionDate AS DATE) = @EffectiveDate 
														AND OTHours > 0 
														AND BranchUnitId IN(1,2) 
														AND EmployeeInOutTemp.DepartmentLineId = #TableTemp.LineId)
							
								UPDATE #TableTemp
								SET ExtraOTEmployee = (SELECT ISNULL(COUNT(1), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND ExtraOTHours > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentLineId = #TableTemp.LineId)

							    UPDATE #TableTemp
								SET ExtraOT = (SELECT ISNULL(SUM(ExtraOTHours), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND ExtraOTHours > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentLineId = #TableTemp.LineId)
									
								UPDATE #TableTemp
								SET ExtraOTAmount  =  (SELECT ISNULL(SUM(ExtraOTHours * BasicSalary/104), 0) FROM EmployeeInOutTemp LEFT JOIN
														(SELECT EmployeeId, BasicSalary, GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						
														FROM EmployeeSalary AS employeeSalary
														WHERE ((CAST(employeeSalary.FromDate AS Date) <= @EffectiveDate) OR (@EffectiveDate IS NULL))
														AND employeeSalary.IsActive = 1) employeeSalaryInfo 
														ON EmployeeInOutTemp.EmployeeId = employeeSalaryInfo.EmployeeId AND employeeSalaryInfo.rowNumSal = 1
														WHERE CAST(TransactionDate AS DATE) = @EffectiveDate 
														AND ExtraOTHours > 0 
														AND BranchUnitId IN(1,2) 
														AND EmployeeInOutTemp.DepartmentLineId = #TableTemp.LineId)

								UPDATE #TableTemp
								SET OthersOTEmployee = (SELECT ISNULL(COUNT(1), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND (WeekendOTHours + HolidayOTHours) > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentLineId = #TableTemp.LineId)

								UPDATE #TableTemp
								SET OthersOT = (SELECT ISNULL(SUM(WeekendOTHours + HolidayOTHours), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND (WeekendOTHours + HolidayOTHours) > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentLineId = #TableTemp.LineId)	
								
								UPDATE #TableTemp
								SET OthersOTAmount  =   (SELECT ISNULL(SUM((WeekendOTHours + HolidayOTHours) * BasicSalary/104), 0) FROM EmployeeInOutTemp LEFT JOIN
														(SELECT EmployeeId, BasicSalary, GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						
														FROM EmployeeSalary AS employeeSalary
														WHERE ((CAST(employeeSalary.FromDate AS Date) <= @EffectiveDate) OR (@EffectiveDate IS NULL))
														AND employeeSalary.IsActive = 1) employeeSalaryInfo 
														ON EmployeeInOutTemp.EmployeeId = employeeSalaryInfo.EmployeeId AND employeeSalaryInfo.rowNumSal = 1
														WHERE CAST(TransactionDate AS DATE) = @EffectiveDate 
														AND (WeekendOTHours + HolidayOTHours) > 0 
														AND BranchUnitId IN(1,2) 
														AND EmployeeInOutTemp.DepartmentLineId = #TableTemp.LineId)


						-- Section Update
								UPDATE #TableTemp
								SET RegularOTEmployee = (SELECT ISNULL(COUNT(1), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND OTHours > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentSectionId = #TableTemp.SectionId)
								WHERE #TableTemp.LineId IS NULL
								
								UPDATE #TableTemp
								SET RegularOT = (SELECT ISNULL(SUM(OTHours), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND OTHours > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentSectionId = #TableTemp.SectionId)
								WHERE #TableTemp.LineId IS NULL

								UPDATE #TableTemp
								SET RegularOTAmount = (SELECT SUM(OTHours * BasicSalary/104) FROM EmployeeInOutTemp LEFT JOIN
													(SELECT EmployeeId, BasicSalary, GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						
													FROM EmployeeSalary AS employeeSalary
													WHERE ((CAST(employeeSalary.FromDate AS Date) <= @EffectiveDate) OR (@EffectiveDate IS NULL))
													AND employeeSalary.IsActive = 1) employeeSalaryInfo 
													ON EmployeeInOutTemp.EmployeeId = employeeSalaryInfo.EmployeeId AND employeeSalaryInfo.rowNumSal = 1
													WHERE CAST(TransactionDate AS DATE) = @EffectiveDate 
													AND OTHours > 0 
													AND BranchUnitId IN(1,2) 
													AND EmployeeInOutTemp.DepartmentSectionId = #TableTemp.SectionId)
													WHERE #TableTemp.LineId IS NULL
								
								UPDATE #TableTemp
								SET ExtraOTEmployee = (SELECT ISNULL(COUNT(1), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND ExtraOTHours > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentSectionId = #TableTemp.SectionId)
								WHERE #TableTemp.LineId IS NULL

							    UPDATE #TableTemp
								SET ExtraOT = (SELECT ISNULL(SUM(ExtraOTHours), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND ExtraOTHours > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentSectionId = #TableTemp.SectionId)
								WHERE #TableTemp.LineId IS NULL
								
								UPDATE #TableTemp
								SET ExtraOTAmount  =  (SELECT ISNULL(SUM(ExtraOTHours * BasicSalary/104), 0) FROM EmployeeInOutTemp LEFT JOIN
														(SELECT EmployeeId, BasicSalary, GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						
														FROM EmployeeSalary AS employeeSalary
														WHERE ((CAST(employeeSalary.FromDate AS Date) <= @EffectiveDate) OR (@EffectiveDate IS NULL))
														AND employeeSalary.IsActive = 1) employeeSalaryInfo 
														ON EmployeeInOutTemp.EmployeeId = employeeSalaryInfo.EmployeeId AND employeeSalaryInfo.rowNumSal = 1
														WHERE CAST(TransactionDate AS DATE) = @EffectiveDate 
														AND ExtraOTHours > 0 
														AND BranchUnitId IN(1,2) 
														AND EmployeeInOutTemp.DepartmentSectionId = #TableTemp.SectionId)
														WHERE #TableTemp.LineId IS NULL
								
								
								UPDATE #TableTemp
								SET OthersOTEmployee = (SELECT ISNULL(COUNT(1), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND (WeekendOTHours + HolidayOTHours) > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentSectionId = #TableTemp.SectionId)
								WHERE #TableTemp.LineId IS NULL

								UPDATE #TableTemp
								SET OthersOT = (SELECT ISNULL(SUM(WeekendOTHours + HolidayOTHours), 0) FROM EmployeeInOutTemp WHERE CAST(TransactionDate AS DATE) = @EffectiveDate AND (WeekendOTHours + HolidayOTHours) > 0 AND BranchUnitId IN(1,2) AND EmployeeInOutTemp.DepartmentSectionId = #TableTemp.SectionId)	
								WHERE #TableTemp.LineId IS NULL	
								
								UPDATE #TableTemp
								SET OthersOTAmount  =   (SELECT ISNULL(SUM((WeekendOTHours + HolidayOTHours) * BasicSalary/104), 0) FROM EmployeeInOutTemp LEFT JOIN
														(SELECT EmployeeId, BasicSalary, GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						
														FROM EmployeeSalary AS employeeSalary
														WHERE ((CAST(employeeSalary.FromDate AS Date) <= @EffectiveDate) OR (@EffectiveDate IS NULL))
														AND employeeSalary.IsActive = 1) employeeSalaryInfo 
														ON EmployeeInOutTemp.EmployeeId = employeeSalaryInfo.EmployeeId AND employeeSalaryInfo.rowNumSal = 1
														WHERE CAST(TransactionDate AS DATE) = @EffectiveDate 
														AND (WeekendOTHours + HolidayOTHours) > 0 
														AND BranchUnitId IN(1,2) 
														AND EmployeeInOutTemp.DepartmentSectionId = #TableTemp.SectionId)
														WHERE #TableTemp.LineId IS NULL

												
								SELECT 	 CONVERT(VARCHAR(10),Date, 103) AS Date
										,Section			
										,SectionId		
										,Line			
										,LineId											
										,RegularOTEmployee			
										,RegularOT					
										,RegularOTAmount			
										,ExtraOTEmployee			
										,ExtraOT					
										,ExtraOTAmount			
										,OthersOTEmployee			
										,OthersOT					
										,OthersOTAmount
										,(RegularOT + ExtraOT + OthersOT) AS TotalOT
										,(RegularOTAmount + ExtraOTAmount + OthersOTAmount) AS TotalOTAmount
								   FROM #TableTemp
								ORDER BY Line, Section
																																																																	 									 													  					  														  						  											  							
END