-- ================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: 2015.11.10
-- Description:	<> EXEC [SPGetEmployeeAttendanceSummaryByDesignation2]  1, 1, 1, 0, 0, 0, 0, 46, '2018-04-15', 'superadmin'
-- ================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeAttendanceSummaryByDesignation2]
			@companyId INT,
			@branchId INT,
			@branchUnitId INT,
			@branchUnitDepartmentId INT = -1,
			@departmentSectionId INT = -1,
			@departmentLineId INT = -1,
			@employeeTypeId INT = -1,
			@branchUnitWorkShiftId INT,
			@TransactionDate DateTime,	
			@UserName NVARCHAR(100)

AS
BEGIN
	
				SET NOCOUNT ON;
				
				DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
				DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
				DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
				DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);
				DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);

				DECLARE @CompanyName NVARCHAR(100) = ''; 
				DECLARE @CompanyAddress NVARCHAR(100) = ''; 
				DECLARE @BranchName NVARCHAR(100) = ''; 
				DECLARE @UnitName NVARCHAR(100) = ''; 
				DECLARE @DepartmentName NVARCHAR(100) = ''; 
				DECLARE @SectionName NVARCHAR(100) = '';
				DECLARE @LineName NVARCHAR(100) = '';
				DECLARE @EmployeeTypeName NVARCHAR(100) = '';
				DECLARE @WorkShiftName NVARCHAR(100) = '';


				-- Insert statements for procedure here
				INSERT INTO @ListOfCompanyIds VALUES (@companyID)
				SELECT @CompanyName= comp.Name, 
					   @CompanyAddress = comp.FullAddress
			    FROM Company comp 
				WHERE comp.Id = @companyID


				INSERT INTO @ListOfBranchIds VALUES (@branchID)
				SELECT @BranchName= brnch.Name
			    FROM Branch brnch 
				WHERE brnch.Id = @branchId
				   

				INSERT INTO @ListOfBranchUnitIds VALUES (@branchUnitID)			
				SELECT TOP(1) @UnitName= unit.Name
			    FROM Unit unit 
				INNER JOIN BranchUnit bunit ON unit.UnitId = bunit.UnitId
				WHERE bunit.BranchUnitId = @branchUnitId
				   
								

				IF(@branchUnitDepartmentID  = -1)
				BEGIN
				   INSERT INTO @ListOfBranchUnitDepartmentIds
				   SELECT DISTINCT BranchUnitDepartmentId FROM dbo.UserPermissionForDepartmentLevel
				   WHERE UserName = @UserName;

				   SET @DepartmentName = ''
				END  
				ELSE
				BEGIN
				   INSERT INTO @ListOfBranchUnitDepartmentIds VALUES (@branchUnitDepartmentID)

				   SELECT @DepartmentName = department.Name FROM 
											BranchUnitDepartment  AS branchUnitDepartment 
											LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
											LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
											WHERE branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId 
				END

				
				IF(@DepartmentSectionId  = -1)
				BEGIN
					SET @SectionName = ''			  
				END
				ELSE
				BEGIN
					 Select @SectionName = sec.Name FROM Section sec
										INNER JOIN DepartmentSection dsec
										ON sec.SectionId = dsec.SectionId
										WHERE dsec.DepartmentSectionId = @DepartmentSectionId
				END

				
				IF(@DepartmentLineId = -1)
				BEGIN
				   SET @LineName = ''				  
				END
				ELSE
				BEGIN
					 Select @LineName = line.Name FROM Line line
									  INNER JOIN DepartmentLine dline
									  ON line.LineId = dline.LineId
									  WHERE dline.DepartmentLineId = @DepartmentLineId
				END

				
				IF(@employeeTypeId  = -1)
				BEGIN
				   INSERT INTO @ListOfEmployeeTypeIds
				   SELECT DISTINCT EmployeeTypeId FROM dbo.UserPermissionForEmployeeLevel
				   WHERE UserName = @UserName;

				   SET @EmployeeTypeName = '';
				END  
				ELSE
				BEGIN
				   INSERT INTO @ListOfEmployeeTypeIds VALUES (@employeeTypeId)

				   SELECT @EmployeeTypeName = et.Title FROM EmployeeType et
											  WHERE et.Id = @EmployeeTypeId

				END

				SELECT TOP(1) @WorkShiftName = ws.Name FROM WorkShift ws
											   INNER JOIN BranchUnitWorkShift buws ON ws.WorkShiftId = buws.WorkShiftId
											   WHERE buws.BranchUnitWorkShiftId = @branchUnitWorkShiftId

				SET FMTONLY OFF

				DECLARE @TempEmployeeInOut TABLE
				(
						TransactionDate DATETIME,						
						EmployeeTypeId INT,
						EmployeeType NVARCHAR(100),
						EmployeeDesignationId INT,
						EmployeeDesignation NVARCHAR(100),
						TotalEmployee INT, 
						TotalPresent INT, 
						TotalLate INT, 
						TotalAbsent INT, 
						TotalLeave INT,
						TotalOSD INT
				)

						INSERT INTO @TempEmployeeInOut
						SELECT 
							@TransactionDate,						
							emp.EmployeeTypeId,
							emp.EmployeeType,
							emp.DesID,
							emp.Designation,
							COUNT(emp.EmployeeCardID) as [Total Employee], 
							COUNT(CASE WHEN s.[Status] = 'Present' OR s.[Status] = 'Late' THEN s.[Status] END) AS present, 
							COUNT(CASE WHEN s.[Status] = 'Late' THEN s.[Status] END) AS Late ,
							COUNT(CASE WHEN s.[Status] = 'Absent' THEN s.[Status] END) AS [Absent],
							COUNT(CASE WHEN s.[Status] = 'Leave' THEN s.[Status] END) AS leave ,
							COUNT(CASE WHEN s.[Status] = 'OSD' THEN s.[Status] END) AS OSD 
						FROM VEmployee AS emp
						LEFT JOIN 
						(SELECT EmployeeId, BranchUnitWorkShiftId, WorkShiftName, [Status]
						   FROM EmployeeInOut eio 
						   WHERE cast(eio.TransactionDate AS DATE) = @TransactionDate
								 AND (eio.[Status]<>'') 
								 AND (eio.[Status] IS NOT NULL)
								 AND (eio.IsActive = 1)) AS s ON emp.EmployeeId = s.EmployeeId

						WHERE emp.IsActive = 1						
						AND ((emp.[Status] = 1) OR (emp.QuitDate >= @TransactionDate))						
					    AND CAST(emp.JoiningDate AS DATE) <= CAST(@TransactionDate AS DATE)	
						AND emp.CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds)
						AND emp.BranchId IN (SELECT BranchIDs FROM @ListOfBranchIds)
						AND emp.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
						AND emp.DeptID IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
						AND (emp.SecID = @DepartmentSectionId OR @DepartmentSectionId = -1)
						AND (emp.LineID = @DepartmentLineId OR @DepartmentLineId = -1)			 							
						AND emp.EmployeeTypeId IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
						AND s.BranchUnitWorkShiftId = @branchUnitWorkShiftId
						
						GROUP BY
								 emp.EmployeeTypeId,
								 emp.EmployeeType,
								 emp.DesID,
								 emp.Designation,
								 s.BranchUnitWorkShiftId,
								 s.WorkShiftName,
								 s.[Status]							 							 
						ORDER BY EmployeeType


								SELECT 
								TransactionDate,
								@CompanyName AS CompanyName,
								@CompanyAddress AS CompanyAddress,
								@BranchName AS BranchName,
								@UnitName AS UnitName,
								@DepartmentName AS DepartmentName,
								@SectionName AS SectionName,
								@LineName AS LineName,
								@EmployeeTypeName AS EmployeeTypeName,
								@WorkShiftName AS WorkShift,
								EmployeeType,
								EmployeeDesignation,
								SUM(TotalEmployee) AS TotalEmployee, 
								SUM(TotalPresent) AS TotalPresent,
								SUM(TotalLate) AS TotalLate,
								SUM(TotalAbsent) AS TotalAbsent,
								SUM(TotalLeave) AS TotalLeave,
								SUM(TotalOSD) AS TotalOSD,
								(CONVERT(VARCHAR(10),ROUND(CONVERT(DECIMAL(18,2),((SUM(TotalPresent)*100.00)/SUM(TotalEmployee))),0),103) + '%') AS PercentageOfPresent
								FROM @TempEmployeeInOut
								GROUP BY TransactionDate,EmployeeType,EmployeeDesignation
								ORDER BY EmployeeDesignation	
				
				SET NOCOUNT OFF;							
																																																																																																																										 																																												 																												
END