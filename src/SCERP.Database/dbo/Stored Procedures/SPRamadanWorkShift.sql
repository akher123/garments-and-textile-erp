
-- ========================================================
-- Author:		<Yasir Arafat>
-- Create date: <2017-12-18>
-- Description:	<> EXEC [SPRamadanWorkShift]  '2020-05-13'
-- ========================================================

CREATE PROCEDURE [dbo].[SPRamadanWorkShift]
						
							  
								@Date		DATETIME
							
															
AS

BEGIN
	
	SET NOCOUNT ON;


						  DECLARE @CheckDate DATETIME 
						  SET @CheckDate = @Date

					      DELETE FROM [EmployeeWorkShift] WHERE CAST(ShiftDate AS DATE) = @CheckDate


						  -- Garments Building --

						  INSERT INTO [dbo].[EmployeeWorkShift]
						  (
								[EmployeeId]
							   ,[BranchUnitWorkShiftId]
							   ,[ShiftDate]
							   ,[StartDate]
							   ,[EndDate]
							   ,[Remarks]
							   ,[Status]
							   ,[CreatedDate]
							   ,[IsActive]
						   )
						   SELECT [Employee].[EmployeeId]
							  ,25
							  ,@CheckDate
							  ,@CheckDate
							  ,@CheckDate
							  ,'Auto Shift Assign'  		
							  ,1
							  ,GETDATE()			
							  ,1
																																																				  					 
							FROM [dbo].[Employee]
							LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
							FROM EmployeeCompanyInfo AS employeeCompanyInfo 
							WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @CheckDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
							ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

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
				
  							WHERE ((employee.IsActive = 1)		
							AND department.Id IN(1, 2, 3, 5, 6, 8, 11, 16) AND Section.SectionId <> 5
							AND ((CAST(employee.JoiningDate AS DATE) <= CAST(@CheckDate AS DATE))
							AND ((CAST(employee.QuitDate AS DATE) >= CAST(@CheckDate AS DATE)) OR employee.QuitDate IS NULL )))


							-- Management Building --

							INSERT INTO [dbo].[EmployeeWorkShift]
							(
								 [EmployeeId]
								,[BranchUnitWorkShiftId]
								,[ShiftDate]
								,[StartDate]
								,[EndDate]
								,[Remarks]
								,[Status]
								,[CreatedDate]
								,[IsActive]
							)
							SELECT [Employee].[EmployeeId]
								,25
								,@CheckDate
								,@CheckDate
								,@CheckDate
								,'Auto Shift Assign'  		
								,1
								,GETDATE()			
								,1
																																																				  					 
							FROM [dbo].[Employee]
							LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
							FROM EmployeeCompanyInfo AS employeeCompanyInfo 
							WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @CheckDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
							ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

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
				
  							WHERE ((employee.IsActive = 1)		
							AND department.Id IN(7, 9, 12, 13, 18, 19,15)
							AND ((CAST(employee.JoiningDate AS DATE) <= CAST(@CheckDate AS DATE))
							AND ((CAST(employee.QuitDate AS DATE) >= CAST(@CheckDate AS DATE)) OR employee.QuitDate IS NULL )))


							--- Sewing Section from 8:30 to 5:00 ---

							INSERT INTO [dbo].[EmployeeWorkShift]
							(
								 [EmployeeId]
								,[BranchUnitWorkShiftId]
								,[ShiftDate]
								,[StartDate]
								,[EndDate]
								,[Remarks]
								,[Status]
								,[CreatedDate]
								,[IsActive]
							)
							SELECT [Employee].[EmployeeId]
								,57
								,@CheckDate
								,@CheckDate
								,@CheckDate
								,'Auto Shift Assign'  		
								,1
								,GETDATE()			
								,1
																																																				  					 
							FROM [dbo].[Employee]
							LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
							FROM EmployeeCompanyInfo AS employeeCompanyInfo 
							WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @CheckDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
							ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

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
				
  							WHERE ((employee.IsActive = 1)		
							AND Section.SectionId = 5
							AND ((CAST(employee.JoiningDate AS DATE) <= CAST(@CheckDate AS DATE))
							AND ((CAST(employee.QuitDate AS DATE) >= CAST(@CheckDate AS DATE)) OR employee.QuitDate IS NULL )))


								-- Knitting Shift --

							   INSERT INTO [dbo].[EmployeeWorkShift]
							   (
									[EmployeeId]
								   ,[BranchUnitWorkShiftId]
								   ,[ShiftDate]
								   ,[StartDate]
								   ,[EndDate]
								   ,[Remarks]
								   ,[Status]
								   ,[CreatedDate]
								   ,[IsActive]
							   )
							   SELECT [Employee].[EmployeeId]
								  ,25
								  ,@CheckDate
								  ,@CheckDate
								  ,@CheckDate
								  ,'Auto Shift Assign'  		
								  ,1
								  ,GETDATE()			
								  ,1
																																																				  					 
								FROM [dbo].[Employee]
								LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
								ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
								FROM EmployeeCompanyInfo AS employeeCompanyInfo 
								WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @CheckDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
								ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

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
				
  								WHERE ((employee.IsActive = 1)		
								AND department.Id = 14

								AND ((CAST(employee.JoiningDate AS DATE) <= CAST(@CheckDate AS DATE))
								AND ((CAST(employee.QuitDate AS DATE) >= CAST(@CheckDate AS DATE)) OR employee.QuitDate IS NULL )))
																					
END