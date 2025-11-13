
-- ==================================================================================================================================================================
-- Author:		Golam Rabbi
-- Create date: <01/03/2015>
-- Description:	<> EXEC [SPGetAllEmployeeJobCard] -1, -1, -1, -1, -1, -1, -1, '', 2016, 5, '2016-04-26','2016-05-25', '', -1, -1, -1.00, -1.00, -1.00, 'superadmin'
-- ==================================================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetAllEmployeeJobCard]
				@CompanyId INT = -1,
				@BranchId INT = -1,
				@BranchUnitId INT = -1,
				@BranchUnitDepartmentId INT = -1,
				@DepartmentSectionId INT = -1,
				@DepartmentLineId INT = -1,
				@EmployeeTypeId INT = -1,
				@EmployeeCardId NVARCHAR(100) = '',
				@Year INT,
				@Month INT,
				@FromDate DATETIME,
				@ToDate DATETIME,				
				@AttendanceStatus NVARCHAR(20) = '',
				@EmployeeActiveStatusId INT = -1,
				@EmployeeCategoryId INT = -1,
				@OTHours NUMERIC(18,2) = -1.00,
				@ExtraOTHours NUMERIC(18,2) = -1.00 ,
				@WeekendOTHours NUMERIC(18,2) = -1.00,
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
			DECLARE @BranchName NVARCHAR(100) = ''; 
			DECLARE @UnitName NVARCHAR(100) = ''; 
			DECLARE @DepartmentName NVARCHAR(100) = ''; 
			DECLARE @SectionName NVARCHAR(100) = '';
			DECLARE @LineName NVARCHAR(100) = '';
			DECLARE @EmployeeTypeName NVARCHAR(100) = '';
			DECLARE @CompanyAddress NVARCHAR(100) = ''; 

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

				SET @BranchName = ''
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
				 Select @LineName = line.Name FROM Line line
									  INNER JOIN DepartmentLine dline
									  ON line.LineId = dline.LineId
									  WHERE dline.DepartmentLineId = @DepartmentLineId
			END

		
			IF (@EmployeeCardId  = '')
			BEGIN
				SET @EmployeeCardId = NULL
			END


			DECLARE @TotalPresentDays INT = NULL,
					@TotalOSDDays INT = NULL,
					@TotalLateDays INT = NULL,
					@TotalAbsentDays INT = NULL,
					@TotalLeaveDays INT = NULL
					
			IF(@AttendanceStatus = 'Present')
			  SET @TotalPresentDays = 0;

			ELSE IF(@AttendanceStatus = 'OSD')
			  SET @TotalOSDDays = 0;

			ELSE IF(@AttendanceStatus = 'Late')
			  SET @TotalLateDays = 0;
					
			ELSE IF(@AttendanceStatus = 'Absent')
			  SET @TotalAbsentDays = 0;
				
			ELSE IF(@AttendanceStatus = 'Leave')
			  SET @TotalLeaveDays = 0;
			
			IF(@OTHours = -1.00)
			  SET @OTHours = NULL;

			IF(@ExtraOTHours= -1.00)
			  SET @ExtraOTHours = NULL;

			IF(@WeekendOTHours= -1.00)
			  SET @WeekendOTHours = NULL;


			SELECT
			 @CompanyName AS CompanyName
			,@BranchName AS BranchName
			,@UnitName AS UnitName
			,@DepartmentName AS DepartmentName
			,@SectionName AS SectionName
			,@LineName AS LineName
			,@EmployeeTypeName AS EmployeeTypeName
			,@CompanyAddress AS CompanyAddress,
			ejc.EmployeeId AS EmployeeId, 
			ejc.EmployeeCardId AS EmployeeCardId,
			ejc.EmployeeName AS EmployeeName,
			ejc.DepartmentName AS Department,
			ejc.SectionName AS Section,
			ejc.LineName AS Line,
			ejc.EmployeeType AS EmployeeType,
			ejc.EmployeeDesignation AS Designation,
			ejc.JoiningDate AS JoiningDate,
			ejc.QuitDate AS QuitDate,
			(DATEDIFF(DAY, @FromDate, @ToDate) + 1) AS TotalDays,
			ejc.PresentDays  AS PresentDays,
			ejc.OSDDays  AS OSDDays,
			ejc.LateDays  AS LateDays,
			ejc.AbsentDays AS AbsentDays,
			ejc.WeekendDays AS WeekendDays,
			ejc.Holidays AS Holidays,
			ejc.LeaveDays AS LeaveDays,
		    ejc.LWPDays AS LWPDays,
			ejc.WorkingDays AS WorkingDays,
			ejc.PayDays AS PayDays,
			ejc.TotalOTHours AS TotalOTHours,
			ejc.TotalExtraOTHours AS TotalExtraOTHours,
			ejc.TotalWeekendOTHours AS TotalWeekendOTHours,
			ejc.TotalHolidayOTHours AS TotalHolidayOTHours,
			ejc.TotalPenaltyOTHours AS TotalPenaltyOTHours,
			ejc.TotalPenaltyAttendanceDays AS TotalPenaltyAttendanceDays,
			ejc.TotalPenaltyLeaveDays AS TotalPenaltyLeaveDays,
			ejc.TotalPenaltyFinancialAmount AS TotalPenaltyFinancialAmount,
			@FromDate AS FromDate,
			@ToDate AS ToDate
				 				 		
			FROM EmployeeJobCard ejc
			WHERE ejc.[Year] = @Year
			AND ejc.[Month] = @Month
			AND CAST(ejc.FromDate AS DATE) = CAST(@FromDate AS DATE)
			AND CAST(ejc.ToDate AS DATE) = CAST(@ToDate AS DATE)
			AND ejc.IsActive = 1 
			AND (ejc.CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds))
			AND (ejc.BranchId  IN (SELECT BranchIDs FROM @ListOfBranchIds))
			AND (ejc.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds))
			AND (ejc.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds))
			AND (ejc.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
			AND (ejc.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
			AND (ejc.EmployeeTypeId IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds))
			AND (ejc.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId IS NULL)	
			AND (ejc.PresentDays > @TotalPresentDays OR @TotalPresentDays IS NULL) 	
			AND (ejc.OSDDays > @TotalOSDDays OR @TotalOSDDays IS NULL)
			AND (ejc.LateDays > @TotalLateDays OR @TotalLateDays IS NULL)
			AND (ejc.AbsentDays > @TotalAbsentDays OR @TotalAbsentDays IS NULL)
			AND (ejc.LeaveDays > @TotalLeaveDays OR @TotalLeaveDays IS NULL)	
			AND (ejc.EmployeeActiveStatusId = @EmployeeActiveStatusId OR @EmployeeActiveStatusId = -1)	
			AND (ejc.EmployeeCategoryId = @EmployeeCategoryId OR @EmployeeCategoryId = -1)	
			AND (ejc.TotalOTHours > @OTHours OR @OTHours IS NULL)
			AND (ejc.TotalExtraOTHours > @ExtraOTHours OR @ExtraOTHours IS NULL)		
			AND (ejc.TotalWeekendOTHours > @WeekendOTHours OR @WeekendOTHours IS NULL)		
			ORDER BY ejc.EmployeeCardId ASC 				
END





