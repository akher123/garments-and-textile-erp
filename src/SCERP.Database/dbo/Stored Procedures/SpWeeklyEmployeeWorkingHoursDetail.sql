-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <10/09/2016>
-- Description:	<> EXEC SpWeeklyEmployeeWorkingHoursDetail  1, 1, NULL, NULL, NULL, NULL, NULL, '', '2017-04-27', '2017-06-02'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SpWeeklyEmployeeWorkingHoursDetail]
	
									
									 @CompanyId		            INT = NULL
									,@BranchId	      	        INT = NULL
									,@BranchUnitId		        INT = NULL
									,@BranchUnitDepartmentId    INT = NULL
									,@SectionId					INT = NULL
									,@LineId					INT = NULL
									,@EmployeeTypeId			INT = NULL
									,@employeeCardId			NVARCHAR(100) = NULL
								    ,@fromDate					DATETIME 
									,@toDate					DATETIME 
															
AS
								 
BEGIN
	
			    SET NOCOUNT ON;
					
						 
				    SELECT	 Department.Name AS DepartmentName
							,Section.Name AS Section
							,HOurs.EmployeeCardId 
							,Employee.Name 
							,HOurs.WorkingHours
							,(SELECT SUM(WeekendOTHours) FROM EmployeeInOut WHERE CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate AND EmployeeInOut.EmployeeId = HOurs.EmployeeId AND IsActive = 1 AND (Status = 'weekend') ) AS WeekendOTHours
							,ISNULL((SELECT SUM(HolidayOTHours) FROM EmployeeInOut WHERE CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate AND EmployeeInOut.EmployeeId = HOurs.EmployeeId AND IsActive = 1 AND (Status = 'holiday') ),0) AS HolidayOTHours

							FROM 

							(SELECT EmployeeId
								  ,EmployeeCardId
								  ,(SUM(8) + SUM([OTHours]) + SUM([ExtraOTHours])) AS WorkingHours
	    
							  FROM [dbo].[EmployeeInOut]
							  WHERE CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate 
							  AND IsActive = 1 					
							  AND (Status = 'Present' OR Status = 'Late') 
							  AND EmployeeType LIKE '%Team Member%'
							  GROUP BY EmployeeCardId, EmployeeId
							  ) AS HOurs
							  LEFT JOIN 

							(SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,IsEligibleForOvertime,
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
							FROM EmployeeCompanyInfo AS employeeCompanyInfo 
							WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
							ON HOurs.EmployeeId = employeeCompanyInfo.EmployeeId	AND employeeCompanyInfo.rowNum = 1 
							LEFT JOIN Employee ON Employee.EmployeeId = HOurs.EmployeeId
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
																	    										 																 																 																																										       
							WHERE	(company.Id = @CompanyId OR @CompanyId IS NULL)
									AND (branch.Id = @BranchId OR @BranchId IS NULL)
									AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId IS NULL)
									AND ((BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId IS NULL))
									AND ((employeeCompanyInfo.DepartmentSectionId = @SectionId) OR (@SectionId IS NULL))
									AND ((employeeCompanyInfo.DepartmentLineId = @LineId) OR (@LineId IS NULL))
									AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId =''))
									AND ((EmployeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId IS NULL))									
									AND (employee.IsActive = 1)
									AND (employee.[Status] = 1)																						
												
									ORDER BY Employee.EmployeeCardId	
					 									 													  					  														  						  											  							
END