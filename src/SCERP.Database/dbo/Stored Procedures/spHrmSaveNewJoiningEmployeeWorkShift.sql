
-- =================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <27-Dec-15 2:09:40 PM>
-- Description:	<> EXEC [spHrmSaveNewJoiningEmployeeWorkShift] '2016-01-01'
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[spHrmSaveNewJoiningEmployeeWorkShift]
                @EmployeeId  UNIQUEIDENTIFIER,
				@JoiningDate DATETIME,
				@BranchUnitId INT

AS
BEGIN
	
		    SET NOCOUNT ON;
			BEGIN TRAN
		
			DECLARE @FromDate DATETIME = @JoiningDate,
			        @ToDate DATETIME = CURRENT_TIMESTAMP;

			WHILE(@ToDate >= @FromDate)
			BEGIN
				 INSERT INTO EmployeeWorkShift
						  (
						  EmployeeId,
						  BranchUnitWorkShiftId,
						  ShiftDate,
						  Remarks,
						  [Status],
						  CreatedDate,
						  IsActive
						 )	

			SELECT		  Employee.EmployeeId
						 ,branchUnitWorkShiftInfo.BranchUnitWorkShiftId
						 ,@FromDate
						 ,'Auto Shift Assign'
						 ,1							
						 ,CURRENT_TIMESTAMP
						 ,1
											 					
			FROM		 Employee employee
						 
						 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
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
						 LEFT JOIN (SELECT TOP 1 buws.BranchUnitWorkShiftId,FromDate,buws.BranchUnitId,WorkShiftId,
									ROW_NUMBER() OVER (PARTITION BY buws.BranchUnitId ORDER BY FromDate DESC) AS buwsRowNum
									FROM BranchUnitWorkShift AS buws
									INNER JOIN BranchUnit ON buws.BranchUnitId = BranchUnit.BranchUnitId
									WHERE (CAST(buws.FromDate AS DATE) <= CAST(@FromDate AS DATE) 
									AND buws.IsActive = 1
									AND buws.[Status] = 1
									AND BranchUnit.BranchUnitId = @BranchUnitId
									)) branchUnitWorkShiftInfo 
						ON branchUnitWorkShiftInfo.BranchUnitId = BranchUnit.BranchUnitId
						AND branchUnitWorkShiftInfo.buwsRowNum = 1
						INNER JOIN   WorkShift ON branchUnitWorkShiftInfo.WorkShiftId = WorkShift.WorkShiftId	 		

						WHERE ((employee.IsActive = 1)
						AND (employee.[Status] = 1) 
						AND ((CAST(employee.JoiningDate AS DATE) <= CAST(@FromDate AS DATE))
						OR (CAST(employee.QuitDate AS DATE) >= CAST(@FromDate AS DATE)))
						AND (WorkShift.IsActive = 1)
						AND WorkShift.Name = 'MORNING'
						AND employee.EmployeeId  NOT IN
								(
								SELECT EmployeeId FROM EmployeeWorkShift
								WHERE CAST(ShiftDate AS DATE) = @FromDate
								AND IsActive = 1
								)
						)

					ORDER BY Employee.EmployeeCardId


				 SET @FromDate = DATEADD(DAY, 1, @FromDate);
			END

			
						 					
			COMMIT TRAN	
			
			
		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;		 	
						 	 
END


