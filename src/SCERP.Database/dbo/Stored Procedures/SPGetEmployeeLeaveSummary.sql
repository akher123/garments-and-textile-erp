
-- ===========================================================================================================
-- Author:		<Md. Yasir Arafat>
-- Create date: <2018-05-09>
-- Description:	<>	EXEC [SPGetEmployeeLeaveSummary] 1, 1, 1, 6, 0, 0, '', '2017-01-01', '2017-12-31'
-- ===========================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeLeaveSummary]
										
											
									 @CompanyId		            INT = NULL
									,@BranchId	      	        INT = NULL
									,@BranchUnitId		        INT = NULL
									,@BranchUnitDepartmentId    INT = NULL
									,@SectionId					INT = NULL	
									,@EmployeeTypeId			INT = NULL
									,@EmployeeCardId			NVARCHAR(100) = NULL	
									,@FromDate					DATETIME
									,@ToDate					DATETIME 

AS

BEGIN
	
	SET NOCOUNT ON

							CREATE TABLE #EmployeeLeaveDetail
							(	
								DepartmentName			NVARCHAR(100)
							   ,SectionName				NVARCHAR(100)
							   ,EmployeeTypeName		NVARCHAR(100)
							   ,EmployeeTypeId			INT
							   ,EmployeeType			NVARCHAR(100)
							   ,Month					NVARCHAR(100)
							   ,MonthId					INT
							   ,CL						INT
							   ,SL						INT
							   ,EL						INT
							   ,ML						INT	
							   ,CLPerson				INT
							   ,SLPerson				INT
							   ,ELPerson				INT
							   ,MLPerson				INT						  
							)

							CREATE TABLE #EmployeeLeaveDetailTemp
							(	
								DepartmentName			NVARCHAR(100)
							   ,SectionName				NVARCHAR(100)
							   ,EmployeeTypeName		NVARCHAR(100)
							   ,EmployeeTypeId			INT
							   ,EmployeeType			NVARCHAR(100)
							   ,Month					NVARCHAR(100)
							   ,MonthId					INT
							   ,LeaveTypeId				INT 
							   ,TotalLeave				INT
							)

							CREATE TABLE #EmployeeLeaveDetailPerson
							(	
								DepartmentName			NVARCHAR(100)
							   ,SectionName				NVARCHAR(100)
							   ,EmployeeTypeName		NVARCHAR(100)
							   ,EmployeeTypeId			INT
							   ,EmployeeType			NVARCHAR(100)
							   ,Month					NVARCHAR(100)
							   ,MonthId					INT
							   ,LeaveTypeId				INT 
							   ,TotalPerson				INT
							)



							-- INSERT INTO #EmployeeLeaveDetail TABLE

							 INSERT INTO #EmployeeLeaveDetail														 
							 SELECT  Department.Name						AS DepartmentName
									,Section.Name							AS SectionName
									,EmployeeType.Title						AS EmployeeTypeName			
									,EmployeeType.Id						AS EmployeeTypeId	
									,EmployeeType =  
												  CASE   
													 WHEN EmployeeType.Id IN (1,2,3) THEN 'Staff' 								
													 ELSE 'Worker' 
												  END	
									,CONVERT(varchar(15), DATENAME(MONTH, [ConsumedDate])) AS Month													
									,MONTH(EmployeeLeaveDetail.ConsumedDate) AS MonthId
									,0
									,0
									,0
									,0
									,0
									,0
									,0
									,0
																													 												 												 
						    FROM EmployeeLeaveDetail
								 JOIN Employee ON EmployeeLeaveDetail.EmployeeId = Employee.EmployeeId										 								 
								 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
								 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
								 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
								 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @ToDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
								 ON Employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

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
								  
						    WHERE (company.Id = @CompanyId OR @CompanyId = 0)
								 AND (Branch.Id = @BranchId OR @BranchId = 0)
								 AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = 0)
								 AND ((branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId = 0))
								 AND ((departmentSection.DepartmentSectionId = @SectionId) OR (@SectionId = 0))												
								 AND ((employeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId = 0))
								 AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId =''))
								 AND CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) BETWEEN @FromDate AND @ToDate											

							GROUP BY Department.Name
									,Section.Name
									,EmployeeType.Title
									,EmployeeType.Id
									,CONVERT(varchar(15), DATENAME(MONTH, [ConsumedDate]))				
									,MONTH(EmployeeLeaveDetail.ConsumedDate)




						 -- INSERT INTO #EmployeeLeaveDetailTemp TABLE

						 INSERT INTO #EmployeeLeaveDetailTemp														 
					     SELECT  Department.Name						AS DepartmentName
							    ,Section.Name							AS SectionName
								,EmployeeType.Title						AS EmployeeTypeName			
								,EmployeeType.Id						AS EmployeeTypeId	
								,EmployeeType =  
											  CASE   
												 WHEN EmployeeType.Id IN (1,2,3) THEN 'Staff' 								
												 ELSE 'Worker' 
											  END	
								,CONVERT(varchar(15), DATENAME(MONTH, [ConsumedDate])) AS Month													
								,MONTH(EmployeeLeaveDetail.ConsumedDate) AS MonthId
								,EmployeeLeaveDetail.LeaveTypeId
								,SUM(1) AS TotalLeave
																													 												 												 
						    FROM EmployeeLeaveDetail
								 JOIN Employee ON EmployeeLeaveDetail.EmployeeId = Employee.EmployeeId										 								 
								 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
								 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
								 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
								 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @ToDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
								 ON Employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

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
								  
						      WHERE (company.Id = @CompanyId OR @CompanyId = 0)
								 AND (Branch.Id = @BranchId OR @BranchId = 0)
								 AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = 0)
								 AND ((branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId = 0))
								 AND ((departmentSection.DepartmentSectionId = @SectionId) OR (@SectionId = 0))												
								 AND ((employeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId = 0))
								 AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId =''))
								 AND CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) BETWEEN @FromDate AND @ToDate											

						GROUP BY Department.Name
								,Section.Name
								,EmployeeType.Title
								,EmployeeType.Id
								,CONVERT(varchar(15), DATENAME(MONTH, [ConsumedDate]))				
								,MONTH(EmployeeLeaveDetail.ConsumedDate)
								,EmployeeLeaveDetail.LeaveTypeId

								


						 -- INSERT INTO #EmployeeLeaveDetailPerson TABLE

						 INSERT INTO #EmployeeLeaveDetailPerson														 
					     SELECT  Department.Name						AS DepartmentName
							    ,Section.Name							AS SectionName
								,EmployeeType.Title						AS EmployeeTypeName			
								,EmployeeType.Id						AS EmployeeTypeId	
								,EmployeeType =  
											  CASE   
												 WHEN EmployeeType.Id IN (1,2,3) THEN 'Staff' 								
												 ELSE 'Worker' 
											  END	
								,CONVERT(varchar(15), DATENAME(MONTH, [ConsumedDate])) AS Month													
								,MONTH(EmployeeLeaveDetail.ConsumedDate) AS MonthId
								,EmployeeLeaveDetail.LeaveTypeId
								,SUM(1) AS TotalPerson
																													 												 												 
						    FROM EmployeeLeaveDetail
								 JOIN Employee ON EmployeeLeaveDetail.EmployeeId = Employee.EmployeeId										 								 
								 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
								 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
								 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
								 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @ToDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
								 ON Employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

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
								  
						      WHERE (company.Id = @CompanyId OR @CompanyId = 0)
								 AND (Branch.Id = @BranchId OR @BranchId = 0)
								 AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = 0)
								 AND ((branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId = 0))
								 AND ((departmentSection.DepartmentSectionId = @SectionId) OR (@SectionId = 0))												
								 AND ((employeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId = 0))
								 AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId =''))
								 AND CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) BETWEEN @FromDate AND @ToDate											

						GROUP BY Department.Name
								,Section.Name
								,EmployeeType.Title
								,EmployeeType.Id
								,CONVERT(varchar(15), DATENAME(MONTH, [ConsumedDate]))				
								,MONTH(EmployeeLeaveDetail.ConsumedDate)
								,EmployeeLeaveDetail.LeaveTypeId
								,EmployeeLeaveDetail.EmployeeCardId


						-- UPDATE No of Days --

					      UPDATE #EmployeeLeaveDetail
							SET CL = (SELECT TotalLeave FROM #EmployeeLeaveDetailTemp 
						  WHERE #EmployeeLeaveDetail.DepartmentName = #EmployeeLeaveDetailTemp.DepartmentName							 
							AND #EmployeeLeaveDetail.SectionName = #EmployeeLeaveDetailTemp.SectionName
							AND #EmployeeLeaveDetail.EmployeeType = #EmployeeLeaveDetailTemp.EmployeeType
							AND #EmployeeLeaveDetail.EmployeeTypeId = #EmployeeLeaveDetailTemp.EmployeeTypeId
							AND #EmployeeLeaveDetail.MonthId = #EmployeeLeaveDetailTemp.MonthId
							AND #EmployeeLeaveDetailTemp.LeaveTypeId = 1
							)

						  UPDATE #EmployeeLeaveDetail
							SET SL = (SELECT TotalLeave FROM #EmployeeLeaveDetailTemp 
						  WHERE #EmployeeLeaveDetail.DepartmentName = #EmployeeLeaveDetailTemp.DepartmentName							 
							AND #EmployeeLeaveDetail.SectionName = #EmployeeLeaveDetailTemp.SectionName
							AND #EmployeeLeaveDetail.EmployeeType = #EmployeeLeaveDetailTemp.EmployeeType
							AND #EmployeeLeaveDetail.EmployeeTypeId = #EmployeeLeaveDetailTemp.EmployeeTypeId
							AND #EmployeeLeaveDetail.MonthId = #EmployeeLeaveDetailTemp.MonthId
							AND #EmployeeLeaveDetailTemp.LeaveTypeId = 2
							)

						  UPDATE #EmployeeLeaveDetail
							SET EL = (SELECT TotalLeave FROM #EmployeeLeaveDetailTemp 
						  WHERE #EmployeeLeaveDetail.DepartmentName = #EmployeeLeaveDetailTemp.DepartmentName							 
							AND #EmployeeLeaveDetail.SectionName = #EmployeeLeaveDetailTemp.SectionName
							AND #EmployeeLeaveDetail.EmployeeType = #EmployeeLeaveDetailTemp.EmployeeType
							AND #EmployeeLeaveDetail.EmployeeTypeId = #EmployeeLeaveDetailTemp.EmployeeTypeId
							AND #EmployeeLeaveDetail.MonthId = #EmployeeLeaveDetailTemp.MonthId
							AND #EmployeeLeaveDetailTemp.LeaveTypeId = 5
							)

						  UPDATE #EmployeeLeaveDetail
							SET ML = (SELECT TotalLeave FROM #EmployeeLeaveDetailTemp 
						  WHERE #EmployeeLeaveDetail.DepartmentName = #EmployeeLeaveDetailTemp.DepartmentName							 
							AND #EmployeeLeaveDetail.SectionName = #EmployeeLeaveDetailTemp.SectionName
							AND #EmployeeLeaveDetail.EmployeeType = #EmployeeLeaveDetailTemp.EmployeeType
							AND #EmployeeLeaveDetail.EmployeeTypeId = #EmployeeLeaveDetailTemp.EmployeeTypeId
							AND #EmployeeLeaveDetail.MonthId = #EmployeeLeaveDetailTemp.MonthId
							AND #EmployeeLeaveDetailTemp.LeaveTypeId = 4
							)
							


						-- UPDATE No of Person --

						  UPDATE #EmployeeLeaveDetail
							SET CLPerson = (SELECT COUNT(1) FROM #EmployeeLeaveDetailPerson 
						  WHERE #EmployeeLeaveDetail.DepartmentName = #EmployeeLeaveDetailPerson.DepartmentName							 
							AND #EmployeeLeaveDetail.SectionName = #EmployeeLeaveDetailPerson.SectionName
							AND #EmployeeLeaveDetail.EmployeeType = #EmployeeLeaveDetailPerson.EmployeeType
							AND #EmployeeLeaveDetail.EmployeeTypeId = #EmployeeLeaveDetailPerson.EmployeeTypeId
							AND #EmployeeLeaveDetail.MonthId = #EmployeeLeaveDetailPerson.MonthId
							AND #EmployeeLeaveDetailPerson.LeaveTypeId = 1
							GROUP BY #EmployeeLeaveDetailPerson.DepartmentName
								,#EmployeeLeaveDetailPerson.SectionName
								,#EmployeeLeaveDetailPerson.EmployeeTypeName
								,#EmployeeLeaveDetailPerson.EmployeeTypeId
								,Month			
								,MonthId
								,#EmployeeLeaveDetailPerson.LeaveTypeId								
							)

						  UPDATE #EmployeeLeaveDetail
							SET SLPerson = (SELECT COUNT(1) FROM #EmployeeLeaveDetailPerson 
						  WHERE #EmployeeLeaveDetail.DepartmentName = #EmployeeLeaveDetailPerson.DepartmentName							 
							AND #EmployeeLeaveDetail.SectionName = #EmployeeLeaveDetailPerson.SectionName
							AND #EmployeeLeaveDetail.EmployeeType = #EmployeeLeaveDetailPerson.EmployeeType
							AND #EmployeeLeaveDetail.EmployeeTypeId = #EmployeeLeaveDetailPerson.EmployeeTypeId
							AND #EmployeeLeaveDetail.MonthId = #EmployeeLeaveDetailPerson.MonthId
							AND #EmployeeLeaveDetailPerson.LeaveTypeId = 2
							GROUP BY #EmployeeLeaveDetailPerson.DepartmentName
								,#EmployeeLeaveDetailPerson.SectionName
								,#EmployeeLeaveDetailPerson.EmployeeTypeName
								,#EmployeeLeaveDetailPerson.EmployeeTypeId
								,Month			
								,MonthId
								,#EmployeeLeaveDetailPerson.LeaveTypeId								
							)

						  UPDATE #EmployeeLeaveDetail
							SET ELPerson = (SELECT COUNT(1) FROM #EmployeeLeaveDetailPerson 
						  WHERE #EmployeeLeaveDetail.DepartmentName = #EmployeeLeaveDetailPerson.DepartmentName							 
							AND #EmployeeLeaveDetail.SectionName = #EmployeeLeaveDetailPerson.SectionName
							AND #EmployeeLeaveDetail.EmployeeType = #EmployeeLeaveDetailPerson.EmployeeType
							AND #EmployeeLeaveDetail.EmployeeTypeId = #EmployeeLeaveDetailPerson.EmployeeTypeId
							AND #EmployeeLeaveDetail.MonthId = #EmployeeLeaveDetailPerson.MonthId
							AND #EmployeeLeaveDetailPerson.LeaveTypeId = 5
							GROUP BY #EmployeeLeaveDetailPerson.DepartmentName
								,#EmployeeLeaveDetailPerson.SectionName
								,#EmployeeLeaveDetailPerson.EmployeeTypeName
								,#EmployeeLeaveDetailPerson.EmployeeTypeId
								,Month			
								,MonthId
								,#EmployeeLeaveDetailPerson.LeaveTypeId								
							)

						  UPDATE #EmployeeLeaveDetail
							SET MLPerson = (SELECT COUNT(1) FROM #EmployeeLeaveDetailPerson 
						  WHERE #EmployeeLeaveDetail.DepartmentName = #EmployeeLeaveDetailPerson.DepartmentName							 
							AND #EmployeeLeaveDetail.SectionName = #EmployeeLeaveDetailPerson.SectionName
							AND #EmployeeLeaveDetail.EmployeeType = #EmployeeLeaveDetailPerson.EmployeeType
							AND #EmployeeLeaveDetail.EmployeeTypeId = #EmployeeLeaveDetailPerson.EmployeeTypeId
							AND #EmployeeLeaveDetail.MonthId = #EmployeeLeaveDetailPerson.MonthId
							AND #EmployeeLeaveDetailPerson.LeaveTypeId = 4
							GROUP BY #EmployeeLeaveDetailPerson.DepartmentName
								,#EmployeeLeaveDetailPerson.SectionName
								,#EmployeeLeaveDetailPerson.EmployeeTypeName
								,#EmployeeLeaveDetailPerson.EmployeeTypeId
								,Month			
								,MonthId
								,#EmployeeLeaveDetailPerson.LeaveTypeId								
							)
																					
										 
					  SELECT DepartmentName			
							,SectionName				
							,EmployeeTypeName		
							,EmployeeTypeId			
							,EmployeeType			
							,Month					
							,MonthId					
							,ISNULL(CL, 0) AS CL					
							,ISNULL(SL, 0) AS SL					
							,ISNULL(EL, 0) AS EL					
							,ISNULL(ML, 0) AS ML

							,ISNULL(CLPerson, 0) AS CLPerson
							,ISNULL(SLPerson, 0) AS SLPerson
							,ISNULL(ELPerson, 0) AS ELPerson
							,ISNULL(MLPerson, 0) AS MLPerson	
							
						FROM #EmployeeLeaveDetail	
						ORDER BY MonthId, EmployeeTypeId, DepartmentName, SectionName	
																																 																																						 								  												  								
END