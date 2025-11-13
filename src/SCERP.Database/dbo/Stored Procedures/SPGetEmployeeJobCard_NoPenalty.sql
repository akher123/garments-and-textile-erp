
-- =================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-11-21>
-- Description:	<> EXEC SPGetEmployeeJobCard_NoPenalty  1, 1, -1, -1, -1, -1, -1,'', 2017, 7, '2017-06-26', '2017-07-25', 'superadmin'			  
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeJobCard_NoPenalty]
	
				@CompanyId INT = -1,
				@BranchId INT = -1,
				@BranchUnitId INT = -1,
				@BranchUnitDepartmentId INT = -1, 
				@DepartmentSectionId INT = -1, 
				@DepartmentLineId INT = -1, 
				@EmployeeTypeId INT = -1,   
				@EmployeeCardId NVARCHAR(100),
				@Year INT,
				@Month INT,
				@FromDate DATETIME,
				@ToDate DATETIME,
				@UserName NVARCHAR(100)
				
AS
BEGIN
	
		SET NOCOUNT ON;

		DECLARE 	@FromDateNew DATETIME = @FromDate ,
					@ToDateNew DATETIME = @ToDate

		BEGIN TRAN

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
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfCompanyIds VALUES (@CompanyId)
		END

		IF(@BranchId = -1)
		BEGIN
			INSERT INTO @ListOfBranchIds
			SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchIds VALUES (@BranchId)
		END

		IF(@BranchUnitId = -1)
		BEGIN
			INSERT INTO @ListOfBranchUnitIds
			SELECT DISTINCT BranchUnitId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchUnitIds VALUES (@BranchUnitId)
		END

		IF(@BranchUnitDepartmentId = -1)
		BEGIN
			INSERT INTO @ListOfBranchUnitDepartmentIds
			SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchUnitDepartmentIds VALUES (@BranchUnitDepartmentId)
		END

		IF(@EmployeeTypeID = -1)
		BEGIN
			INSERT INTO @ListOfEmployeeTypeIds
			SELECT DISTINCT EmployeeTypeId FROM UserPermissionForEmployeeLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfEmployeeTypeIds VALUES (@EmployeeTypeID)
		END

		IF (@DepartmentSectionId  = -1)
		BEGIN
			SET @DepartmentSectionId = NULL
		END

		IF (@DepartmentLineId  = -1)
		BEGIN
			SET @DepartmentLineId = NULL
		END

		SELECT 
		ejc.CompanyName,
		ejc.CompanyAddress,
		ejc.EmployeeName AS Name, 
		ejc.EmployeeCardId,
		ejc.BranchName AS Branch,
		ejc.UnitName AS Unit,
		ejc.DepartmentName AS Department,
		ejc.SectionName AS Section,
		ejc.LineName AS Line,
		ejc.EmployeeType,
		ejc.EmployeeGrade,
		ejc.EmployeeDesignation,
		ejc.MobileNo, 
		ejc.JoiningDate,
		ejc.QuitDate,
		ejc.GrossSalary,
		ejc.PresentDays,
		ejc.LateDays,
		ejc.OSDDays,
		ejc.AbsentDays,
		ejc.LeaveDays,
		ejc.Holidays,
		ejc.WeekendDays,
		ejc.LWPDays AS LWP,
		ejc.TotalDays,
		ejc.WorkingDays,
		ejc.PayDays,
		ejc.TotalOTHours AS OTHourLast,
		ejc.TotalExtraOTHours AS TotalExtraOTHours,
		ejc.TotalWeekendOTHours AS TotalWeekendOTHours,
		ejc.TotalHolidayOTHours AS TotalHolidayOTHours,
		ejc.EmployeeOTRate AS OTRate,
		eio.TransactionDate AS [Date],
		CONVERT(VARCHAR(10),DATENAME(dw, eio.TransactionDate)) AS [DayName],
		eio.WorkShiftName AS [Shift],
		CASE eio.[Status] 
		WHEN 'Late' THEN 'Present' 
		WHEN 'OSD'  THEN 'Present'
		ELSE [Status] END AS [Status],
		CONVERT(VARCHAR(5),eio.InTime, 108) AS InTime,
		CONVERT(VARCHAR(5),eio.OutTime, 108) AS OutTime,			 
		CASE eio.LateInMinutes 
				WHEN 0 THEN NULL 
				ELSE eio.LateInMinutes 
			 END  
		 AS [Delay],

		CASE WHEN eio.[Status] = 'WEEKEND' 
			 OR eio.[Status] = 'LEAVE' 
			 OR eio.[Status] = 'HOLIDAY' 
			 OR eio.[Status] = 'OSD' 
			 OR eio.[Status] = 'ABSENT' 
			 OR eio.[Status] = '' 
			 OR eio.[Status] = NULL 
			 THEN NULL 
			 ELSE  eio.OTHours 
			 END AS OTHours,

		CASE WHEN eio.[Status] = 'WEEKEND' 
			 OR eio.[Status] = 'LEAVE' 
			 OR eio.[Status] = 'HOLIDAY' 
			 OR eio.[Status] = 'OSD' 
			 OR eio.[Status] = 'ABSENT' 
			 OR eio.[Status] = '' 
			 OR eio.[Status] = NULL 
			 THEN NULL 
			 ELSE  eio.ExtraOTHours 
			 END AS ExtraOTHours,
		
		CASE WHEN eio.[Status] = 'LEAVE' 
			 OR eio.[Status] = 'OSD' 
			 OR eio.[Status] = '' 
			 OR eio.[Status] = NULL 
			 THEN NULL 
			 ELSE  eio.WeekendOTHours 
			 END AS WeekendOTHours,

		CASE WHEN eio.[Status] = 'LEAVE' 
			OR eio.[Status] = 'OSD' 
			OR eio.[Status] = '' 
			OR eio.[Status] = NULL 
			THEN NULL 
			ELSE  eio.HolidayOTHours 
			END AS HolidayOTHours,
	
		eio.Remarks,

		ejc.TotalPenaltyOTHours,
		ejc.TotalPenaltyAttendanceDays,
		ejc.TotalPenaltyLeaveDays,
		ejc.TotalPenaltyFinancialAmount,
		
	   (SELECT TOP (1) FromDate
	    FROM EmployeeSalary
		WHERE (EmployeeId = ejc.EmployeeId) AND CAST(FromDate AS DATE) <> CAST(ejc.JoiningDate AS DATE) AND (IsActive = 1)
		ORDER BY FromDate DESC) AS LastIncrementDate,

		SkillSet.Title AS SkillType	
	
		,DATENAME(MONTH, @ToDate)  AS Month
	    ,DATENAME(YEAR, @ToDate)  AS Year
				
		FROM EmployeeJobCard_NoPenalty ejc
		RIGHT JOIN EmployeeInOut_NoPenalty eio
		ON ejc.EmployeeId = eio.EmployeeId  

		LEFT JOIN (SELECT EmployeeId, JobTypeId,
		ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
		FROM EmployeeCompanyInfo AS employeeCompanyInfo 
		WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @ToDateNew) OR (@ToDateNew IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
		ON ejc.EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1 

		LEFT JOIN SkillSet ON SkillSet.Id = employeeCompanyInfo.JobTypeId AND SkillSet.IsActive = 1

		WHERE ejc.[Year] = @Year
		AND ejc.[Month] = @Month
		AND ejc.FromDate = CAST(@FromDateNew AS DATE)
		AND ejc.ToDate = CAST(@ToDateNew AS DATE)
		AND ejc.CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds)
		AND ejc.BranchId IN (SELECT BranchIDs FROM @ListOfBranchIds)
		AND ejc.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
		AND ejc.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
		AND (ejc.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
		AND (ejc.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
		AND ejc.employeeTypeId IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
		AND ejc.EmployeeCardId = @EmployeeCardId
		AND CAST(eio.TransactionDate AS DATE) >= CAST(@FromDateNew AS DATE)
		AND CAST(eio.TransactionDate AS DATE) <= CAST(@ToDateNew AS DATE)
		AND ejc.IsActive = 1 
		AND eio.IsActive =1	
		ORDER BY eio.TransactionDate ASC 	

		COMMIT TRAN
END





