
-- =======================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-11-21>
-- Description:	<> EXEC SPGetEmployeeJobCardModelKnittingDyeing   1,  1, 1, -1, -1, -1, -1, '0835', 2018, 02, '2018-01-26', '2018-02-25', 'superadmin'
-- ======================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeJobCardModelKnittingDyeing]	
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

		DECLARE 	@FromDateNew DATETIME = @FromDate 
				   ,@ToDateNew DATETIME = @ToDate
				   ,@TotalPenaltyDays DECIMAL = 0


		BEGIN TRAN

		SELECT @TotalPenaltyDays = SUM(Penalty) FROM HrmPenalty 
		WHERE EmployeeCardId = @EmployeeCardId 
		AND CAST(PenaltyDate AS DATE) BETWEEN @FromDate AND @ToDate
		AND PenaltyTypeId = 2 AND IsActive = 1


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
		(ejc.PresentDays - CAST(ISNULL(@TotalPenaltyDays, 0)AS INT)) AS PresentDays,
		ejc.LateDays,
		ejc.OSDDays,
	   (ejc.AbsentDays + CAST(ISNULL( @TotalPenaltyDays, 0) AS INT)) AS AbsentDays,
	    ejc.LeaveDays  AS LeaveDays,
		ejc.Holidays,
		ejc.WeekendDays,
		ejc.LWPDays AS LWP,
		ejc.TotalDays,
		ejc.WorkingDays,
		ejc.PayDays,
		ejc.TotalOTHours AS OTHourLast,
		eio.TransactionDate AS [Date],
		CONVERT(VARCHAR(10),DATENAME(dw, eio.TransactionDate)) AS [DayName],
		eio.WorkShiftName AS [Shift],

		CASE eio.[Status] WHEN 'Late' THEN 'Present' ELSE [Status] END AS [Status],
		CASE eio.[Status] WHEN 'WEEKEND' THEN '' ELSE  CONVERT(VARCHAR(5),eio.InTime, 108) END AS InTime,
		CASE eio.[Status] WHEN 'WEEKEND' THEN '' ELSE  CONVERT(VARCHAR(5),eio.OutTime, 108) END AS OutTime,
		CASE eio.[Status] 
			 WHEN 'WEEKEND' THEN NULL 
			 ELSE  
			 CASE eio.LateInMinutes 
				WHEN 0 THEN NULL 
				ELSE eio.LateInMinutes 
			 END  
		END AS [Delay],
			 
		CASE WHEN eio.[Status] = 'WEEKEND' 
			 OR eio.[Status] = 'LEAVE' 
			 OR eio.[Status] = 'HOLIDAY' 
			 OR eio.[Status] = 'OSD' 
			 OR eio.[Status] = 'ABSENT' 
			 OR  eio.[Status] = '' 
			 OR eio.[Status] = NULL 
			 THEN NULL 
			 ELSE  eio.OTHours 
			 END AS OTHours

		,eio.Remarks		
		,DATENAME(MONTH, @ToDateNew)  AS Month
		,DATENAME(YEAR, @ToDateNew)  AS Year

		FROM EmployeeJobCardModelKnittingDyeing ejc
		RIGHT JOIN EmployeeInOutModelKnittingDyeing eio
		ON ejc.EmployeeId = eio.EmployeeId  
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
		AND ejc.EmployeeCardId = @employeeCardId
		AND CAST(eio.TransactionDate AS DATE) >= CAST(@FromDateNew AS DATE)
		AND CAST(eio.TransactionDate AS DATE) <= CAST(@ToDateNew AS DATE)
		AND ejc.IsActive = 1 
		AND eio.IsActive =1	
		ORDER BY eio.TransactionDate ASC 	

		COMMIT TRAN
END





