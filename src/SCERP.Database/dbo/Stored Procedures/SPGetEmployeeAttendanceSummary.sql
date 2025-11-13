-- =========================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: 2015.11.10
-- Description:	<> EXEC [SPGetEmployeeAttendanceSummary]  1, 1, 1, -1, -1, -1, -1, 1, '2017-05-01', 'superadmin'
-- =========================================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeAttendanceSummary]
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

				
				DECLARE @DepartmentName NVARCHAR(100) = ''; 
				DECLARE @SectionName NVARCHAR(100) = '';
				DECLARE @LineName NVARCHAR(100) = '';
				DECLARE @EmployeeTypeName NVARCHAR(100) = '';
				DECLARE @WorkShiftName NVARCHAR(100) = '';


				-- Insert statements for procedure here
				INSERT INTO @ListOfCompanyIds VALUES (@companyID)

				INSERT INTO @ListOfBranchIds VALUES (@branchID)
				   
				INSERT INTO @ListOfBranchUnitIds VALUES (@branchUnitID)
								

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

				DECLARE @TempEmployeeInOut TABLE(
						TransactionDate DATETIME,
						CompanyId INT,
						CompanyName NVARCHAR(100),
						CompanyAddress NVARCHAR(MAX),
						BranchId INT,
						BranchName NVARCHAR(100),
						BranchUnitId INT,
						UnitName NVARCHAR(100),
						BranchUnitDepartmentId INT,
						DepartmentName NVARCHAR(100), 
						DepartmentSectionId INT,
						SectionName NVARCHAR(100), 
						DepartmentLineId INT,
						LineName NVARCHAR(100),
						EmployeeTypeId INT,
						EmployeeType NVARCHAR(100),
						BranchUnitWorkShiftId INT,
						WorkShiftName NVARCHAR(100),
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
							emp.CompanyId,
							emp.CompanyName,
							emp.CompanyAddress,
							emp.BranchId,
							emp.BranchName,
							emp.BranchUnitId,
							emp.UnitName,
							emp.DeptID,
							emp.Department, 
							emp.SecID,
							emp.Section,
							emp.LineID,
							emp.Line,
							emp.EmployeeTypeId,
							emp.EmployeeType,
							s.BranchUnitWorkShiftId,
							s.WorkShiftName,
							COUNT(emp.EmployeeCardID) AS [Total Employee], 
							COUNT(CASE WHEN s.[Status] = 'Present' OR s.[Status] = 'Late' THEN s.[Status] END) AS present, 
							COUNT(CASE WHEN s.[Status] = 'Late' THEN s.[Status] END) AS Late ,
							COUNT(CASE WHEN s.[Status] = 'Absent' THEN s.[Status] END) AS [Absent],
							COUNT(CASE WHEN s.[Status] = 'Leave' THEN s.[Status] END) AS leave ,
							COUNT(CASE WHEN s.[Status] = 'OSD' THEN s.[Status] END) AS OSD 
						FROM VEmployee AS emp
						LEFT JOIN 
						(SELECT EmployeeId, BranchUnitWorkShiftId, WorkShiftName, [Status]
						   FROM EmployeeInOut eio 
						   WHERE cast(eio.TransactionDate AS DATE) = @TransactionDate) AS s ON emp.EmployeeId = s.EmployeeId
						WHERE 
						emp.IsActive = 1
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
						AND emp.Section <> 'Security'
						GROUP BY emp.CompanyId, 
								 emp.CompanyName, 
								 emp.CompanyAddress,
								 emp.BranchId, 
								 emp.BranchName, 
								 emp.BranchUnitId, 
								 emp.UnitName, 
								 emp.DeptID, 
								 emp.Department, 
								 emp.SecID, 
								 emp.Section, 
								 emp.LineID,
								 emp.Line,
								 emp.EmployeeTypeId,
								 emp.EmployeeType,
								 s.BranchUnitWorkShiftId,
								 s.WorkShiftName,
								 s.[Status]							 							 
						ORDER BY Department

						SELECT 
						TransactionDate,
						CompanyName,
						CompanyAddress,
						BranchName,
						UnitName,
						@DepartmentName AS DepartmentName,
						DepartmentName AS Department, 
						@SectionName AS SectionName,
						SectionName AS Section, 
						@LineName AS LineName,
						LineName AS Line, 
						@EmployeeTypeName AS EmployeeTypeName,
						@WorkShiftName AS WorkShift,
						SUM(TotalEmployee) AS TotalEmployee, 
						SUM(TotalPresent) AS TotalPresent,
						SUM(TotalLate) AS TotalLate,
						SUM(TotalAbsent) AS TotalAbsent,
						SUM(TotalLeave) AS TotalLeave,
						SUM(TotalOSD) AS TotalOSD,
						(CONVERT(VARCHAR(10),ROUND(CONVERT(DECIMAL(18,2),((SUM(TotalPresent)*100.00)/SUM(TotalEmployee))),0),103) + '%') AS PercentageOfPresent
						FROM @TempEmployeeInOut
						GROUP BY TransactionDate,CompanyName,CompanyAddress,BranchName,UnitName,DepartmentName, SectionName, LineName, WorkShiftName
						ORDER BY LineName	
				
				SET NOCOUNT OFF;							
																																																																																																																										 																																												 																												
END



