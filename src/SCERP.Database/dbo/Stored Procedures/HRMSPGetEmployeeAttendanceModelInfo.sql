-- =========================================================================================================================================================
-- Author:		Golam Rabbi
-- Create date: 2015.11.10
-- Description:	<> EXEC HRMSPGetEmployeeAttendanceModelInfo  -1, -1, -1, -1, -1, -1, -1, '', -1, '2015-12-08', '2015-12-08', '', 1, 0.0, 0.0, 0.0, 'superadmin'
-- =========================================================================================================================================================

CREATE PROCEDURE [dbo].[HRMSPGetEmployeeAttendanceModelInfo]
				@CompanyId INT = -1,
				@BranchId INT = -1,
				@BranchUnitId INT = -1,
				@BranchUnitDepartmentId INT = -1, 
				@DepartmentSectionId INT = -1, 
				@DepartmentLineId INT = -1, 
				@EmployeeTypeId INT = -1,   
				@EmployeeCardId NVARCHAR(100) = '',
				@BranchUnitWorkShiftId INT = -1,
				@FromDate DATETIME = '1900-01-01',	
				@Todate DATETIME = '1900-01-01',
				@AttendanceStatus NVARCHAR(20) = '',
				@TotalContinuousAbsentDays INT = -1,
				@OTHours NUMERIC(18,2) = 0.0,
				@UserName NVARCHAR(100)

AS
BEGIN
	
				SET NOCOUNT ON;
				
				DECLARE @CompanyName NVARCHAR(100) = ''; 
				DECLARE @BranchName NVARCHAR(100) = ''; 
				DECLARE @UnitName NVARCHAR(100) = ''; 
				DECLARE @DepartmentName NVARCHAR(100) = ''; 
				DECLARE @SectionName NVARCHAR(100) = '';
				DECLARE @LineName NVARCHAR(100) = '';
				DECLARE @EmployeeTypeName NVARCHAR(100) = '';
				DECLARE @CompanyAddress NVARCHAR(100) = ''; 


				DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
				DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
				DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
				DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);
				DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);

				IF(@CompanyId = -1)
				BEGIN
					INSERT INTO @ListOfCompanyIds
					SELECT DISTINCT CompanyId FROM UserPermissionForDepartmentLevel
					WHERE UserName = @UserName;
					
					SET @CompanyName = '';
					SET @CompanyAddress = '';

				END  
				ELSE
				BEGIN
					INSERT INTO @ListOfCompanyIds VALUES (@CompanyId)
					
					SELECT @CompanyName = company.Name, 
						   @CompanyAddress = company.FullAddress 
					       FROM Company  AS company 
					       WHERE company.Id = @CompanyId 
				END

				IF(@BranchId = -1)
				BEGIN
					INSERT INTO @ListOfBranchIds
					SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
					WHERE UserName = @UserName;

					SET @BranchName = '';
				END  
				ELSE
				BEGIN
					INSERT INTO @ListOfBranchIds VALUES (@BranchId)
					
					SELECT @BranchName = branch.Name FROM 
									 Branch  AS branch 
									 WHERE branch.Id = @BranchId 
				END

				IF(@BranchUnitId = -1)
				BEGIN
					INSERT INTO @ListOfBranchUnitIds
					SELECT DISTINCT BranchUnitId FROM UserPermissionForDepartmentLevel
					WHERE UserName = @UserName;
					
					SELECT @UnitName = '';
				END  
				ELSE
				BEGIN
					INSERT INTO @ListOfBranchUnitIds VALUES (@BranchUnitId)

					SELECT @UnitName = unit.Name FROM 
								   Unit  AS unit 
								   LEFT JOIN BranchUnit  AS branchUnit ON unit.UnitId = branchUnit.UnitId
								   WHERE branchUnit.BranchUnitId = @BranchUnitId 
				END

				IF(@BranchUnitDepartmentId = -1)
				BEGIN
					INSERT INTO @ListOfBranchUnitDepartmentIds
					SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
					WHERE UserName = @UserName;

					SELECT @DepartmentName = '';
				END  
				ELSE
				BEGIN
					INSERT INTO @ListOfBranchUnitDepartmentIds VALUES (@BranchUnitDepartmentId)

					SELECT @DepartmentName = department.Name FROM 
										 BranchUnitDepartment  AS branchUnitDepartment 
										 LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
										 LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
										 WHERE branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId 
				END

				
				IF (@DepartmentSectionId  = -1)
				BEGIN
					SET @DepartmentSectionId = NULL
					SET @SectionName = ''
				END
				ELSE
				BEGIN
					Select @SectionName = sec.Name FROM Section sec
										INNER JOIN DepartmentSection dsec
										ON sec.SectionId = dsec.SectionId
										WHERE dsec.DepartmentSectionId = @DepartmentSectionId
				END

				IF (@DepartmentLineId  = -1)
				BEGIN
					SET @DepartmentLineId = NULL
					SET @LineName = ''
				END
				ELSE
				BEGIN
					SELECT @LineName = line.Name FROM Line line
									  INNER JOIN DepartmentLine dline
									  ON line.LineId = dline.LineId
									  WHERE dline.DepartmentLineId = @DepartmentLineId
				END

				IF(@EmployeeTypeID = -1)
				BEGIN
					INSERT INTO @ListOfEmployeeTypeIds
					SELECT DISTINCT EmployeeTypeId FROM UserPermissionForEmployeeLevel
					WHERE UserName = @UserName;

					SET @EmployeeTypeName = '';
				END  
				ELSE
				BEGIN
					INSERT INTO @ListOfEmployeeTypeIds VALUES (@EmployeeTypeID)

					SELECT @EmployeeTypeName = et.Title FROM EmployeeType et
											  WHERE et.Id = @EmployeeTypeId
				END

				DECLARE @EmployeeId UNIQUEIDENTIFIER;
				IF(@EmployeeCardId='')
				BEGIN
					SET @EmployeeId = NULL
				END
				ELSE
				BEGIN
					SELECT @EmployeeId = EmployeeId FROM Employee WHERE EmployeeCardId = @EmployeeCardId
				END

				IF (@BranchUnitWorkShiftId  = -1)
				BEGIN
					SET @BranchUnitWorkShiftId = NULL
				END


				IF (@FromDate  = '1900-01-01')
				BEGIN
					SET @FromDate = NULL
				END

				IF (@Todate  = '1900-01-01')
				BEGIN
					SET @Todate = NULL
				END


				IF(@AttendanceStatus = '')
				BEGIN
				  SET @AttendanceStatus = NULL
				END

				DECLARE @ExtendedAttendanceStatus NVARCHAR(100) = '';

				SET @ExtendedAttendanceStatus = CASE @AttendanceStatus
													WHEN 'Present' THEN 'Late' 
													WHEN 'Late' THEN 'Late'  
													WHEN 'OSD' THEN 'OSD' 
													WHEN 'Absent' THEN 'Absent' 
													WHEN 'Leave' THEN 'Leave' 
													ELSE NULL
											    END 

				IF (@TotalContinuousAbsentDays  = -1)
				BEGIN
					SET @TotalContinuousAbsentDays = NULL
				END

				IF(@OTHours = -1.00)
					SET @OTHours = NULL;

				 SET FMTONLY OFF

				 SELECT 

				 @CompanyName AS CompanyName
				,@CompanyAddress AS CompanyAddress
				,@BranchName AS BranchName
				,@UnitName AS UnitName
				,@DepartmentName AS DepartmentName
				,@SectionName AS SectionName
				,@LineName AS LineName
				,@EmployeeTypeName AS EmployeeTypeName
				,@FromDate AS FromDate
				,@Todate AS ToDate
				,eio.DepartmentName AS Department
				,eio.TransactionDate AS [Date]
				,eio.EmployeeId
				,eio.EmployeeCardId
				,eio.EmployeeName
				,eio.MobileNo
				,eio.JoiningDate
				,eio.SectionName AS Section
				,eio.LineName AS Line
				,eio.EmployeeType 
				,eio.EmployeeDesignation
				,eio.WorkShiftName 
				,CONVERT(VARCHAR(10), eio.InTime, 108)  AS InTime
				,CONVERT(VARCHAR(10), eio.OutTime, 108)  AS OutTime		
				,CASE eio.[Status] WHEN 'Late' THEN 'Present' ELSE eio.[Status] END AS [Status]
				,CONVERT(VARCHAR(10), eio.LateInMinutes,108) AS LateTime
				,CONVERT(VARCHAR(10), eio.LastDayOutTime, 108)  AS LastDayOutTime
				,eio.TotalContinuousAbsentDays
				,eio.OTHours
				,eio.LastDayOTHours
				,eio.Remarks
				,'' AS SignatureOfEmployee
				
				FROM EmployeeInOutModel eio
				WHERE 
				eio.IsActive = 1
				AND eio.CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds)
				AND eio.BranchId IN (SELECT BranchIDs FROM @ListOfBranchIds)
				AND eio.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
				AND eio.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
				AND (eio.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
				AND (eio.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
				AND eio.EmployeeTypeId IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
				AND ((eio.EmployeeId = @EmployeeId) OR (@EmployeeId IS NULL))
				AND ((eio.[Status] = @AttendanceStatus) OR (eio.[Status]= @ExtendedAttendanceStatus) OR (@AttendanceStatus IS NULL))	
				AND ((CAST(eio.TransactionDate AS DATE) >= CAST(@FromDate AS DATE)) OR (@FromDate IS NULL))
				AND ((CAST(eio.TransactionDate AS DATE) <= CAST(@ToDate AS DATE)) OR (@ToDate IS NULL))
				AND ((eio.TotalContinuousAbsentDays = @TotalContinuousAbsentDays) OR (@TotalContinuousAbsentDays IS NULL))  	
				AND (OTHours > @OTHours OR @OTHours IS NULL)	
				ORDER BY DepartmentName, EmployeeCardId, TransactionDate ASC			
				
				SET NOCOUNT OFF;							
																																																																																																																										 																																												 																												
END



