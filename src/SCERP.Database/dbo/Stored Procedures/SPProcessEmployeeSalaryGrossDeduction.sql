
-- ====================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-04>
-- Description:	<> EXEC [SPProcessEmployeeSalaryGrossDeduction] 1, 1, 1, -1, -1, -1, -1,'0715', 2017, 12, '2017-11-26', '2017-12-25', 'superadmin'   (FULL MONTH PROCESS)
-- ====================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeSalaryGrossDeduction]
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
	   @UserName  NVARCHAR(100)
AS
BEGIN
	
		  SET XACT_ABORT ON;
		  SET NOCOUNT ON;

		  DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
		  DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
		  DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
		  DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);
		  DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);

		  BEGIN TRAN

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

			IF (@DepartmentSectionId  = -1)
			BEGIN
				SET @DepartmentSectionId = NULL
			END
			

			IF (@DepartmentLineId  = -1)
			BEGIN
				SET @DepartmentLineId = NULL
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
					
			IF (@EmployeeCardId  = '')
			BEGIN
				SET @EmployeeCardId = NULL
			END

			DECLARE @UserID UNIQUEIDENTIFIER;
			SELECT @UserID = EmployeeID FROM [User] WHERE UserName = @UserName;			

			DELETE FROM [dbo].[EmployeeProcessedSalaryGrossDeduction]
			WHERE           
			(
			(CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds))
			AND (BranchId IN (SELECT BranchIDs FROM @ListOfBranchIds))
			AND (BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds))
			AND (DepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds))
			AND (SectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
			AND (LineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
			AND (EmployeeTypeId IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds))
			AND (EmployeeCardId = @EmployeeCardId OR @EmployeeCardId IS NULL)		
			AND ([Year] = @Year)
			AND ([Month] = @Month)
			AND ([FromDate] = @FromDate)
			AND ([ToDate] = @ToDate)
			AND (IsActive=1)
			)



		  INSERT INTO [dbo].[EmployeeProcessedSalaryGrossDeduction]
           ([EmployeeId]
           ,[EmployeeCardId]
           ,[Name]
           ,[NameInBengali]
           ,[MobileNo]
           ,[Year]
           ,[Month]
           ,[MonthName]
		   ,[FromDate]
		   ,[ToDate]
           ,[CompanyId]
           ,[CompanyName]
           ,[CompanyNameInBengali]
           ,[CompanyAddress]
           ,[CompanyAddressInBengali]
           ,[BranchId]
           ,[Branch]
           ,[BranchInBengali]
           ,[BranchUnitId]
           ,[Unit]
           ,[UnitInBengali]
           ,[DepartmentId]
           ,[Department]
           ,[DepartmentInBengali]
           ,[SectionId]
           ,[Section]
           ,[SectionInBengali]
           ,[LineId]
           ,[Line]
           ,[LineInBengali]
           ,[EmployeeTypeId]
           ,[EmployeeType]
           ,[EmployeeTypeInBengali]
           ,[GradeId]
           ,[Grade]
           ,[GradeInBengali]
           ,[DesignationId]
           ,[Designation]
           ,[DesignationInBengali]
           ,[EmployeeCategoryId]
           ,[JoiningDate]
           ,[QuitDate]
           ,[TotalDays]
           ,[WorkingDays]
           ,[PresentDays]
           ,[LateDays]
           ,[OSDDays]
           ,[AbsentDays]
           ,[LeaveDays]
           ,[LWPDays]
           ,[HolidayDays]
           ,[WeekendDays]
           ,[Paydays]
           ,[CasualLeave]
           ,[SickLeave]
           ,[MaternityLeave]
           ,[EarnLeave]
           ,[GrossSalary]
           ,[BasicSalary]
           ,[HouseRent]
           ,[MedicalAllowance]
           ,[Conveyance]
           ,[FoodAllowance]
           ,[EntertainmentAllowance]
           ,[LWPFee]
           ,[AbsentFee]
		   ,[PenaltyFee]
           ,[Advance]
           ,[Stamp]
           ,[TotalDeduction]
           ,[AttendanceBonus]
           ,[ShiftingBonus]
           ,[TotalBonus]
           ,[TotalPaid]
           ,[Rate]
		   ,[OTRate]
           ,[OTHours]
           ,[TotalOTAmount]
           ,[NetAmount]
		   ,[TotalExtraOTHours]
		   ,[TotalExtraOTAmount]
		   ,[TotalWeekendOTHours]
		   ,[TotalWeekendOTAmount]
		   ,[TotalHolidayOTHours]
		   ,[TotalHolidayOTAmount]
		   ,[AdvancedIncomeTax]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[EditedDate]
           ,[EditedBy]
           ,[IsActive])

SELECT  ejc.EmployeeId
	   ,ejc.EmployeeCardId
	   ,ejc.EmployeeName
	   ,ejc.EmployeeNameInBengali
	   ,ejc.MobileNo
	   ,ejc.[Year]
	   ,ejc.[Month]
	   ,ejc.[MonthName]
	   ,ejc.FromDate 
	   ,ejc.ToDate 
	   ,ejc.CompanyId
	   ,ejc.CompanyName
	   ,ejc.CompanyNameInBengali
	   ,ejc.CompanyAddress
	   ,ejc.CompanyAddressInBengali
	   ,ejc.BranchId
	   ,ejc.BranchName
	   ,ejc.BranchNameInBengali
	   ,ejc.BranchUnitId
	   ,ejc.UnitName
	   ,ejc.UnitNameInBengali
	   ,ejc.BranchUnitDepartmentId
	   ,ejc.DepartmentName
	   ,ejc.DepartmentNameInBengali
	   ,ejc.DepartmentSectionId
	   ,ejc.SectionName
	   ,ejc.SectionNameInBengali
	   ,ejc.DepartmentLineId
	   ,ejc.LineName
	   ,ejc.LineNameInBengali
	   ,ejc.EmployeeTypeId
	   ,ejc.EmployeeType
	   ,ejc.EmployeeTypeInBengali
	   ,ejc.EmployeeGradeId
	   ,ejc.EmployeeGrade
	   ,ejc.EmployeeGradeInBengali
	   ,ejc.EmployeeDesignationId
	   ,ejc.EmployeeDesignation
	   ,ejc.EmployeeDesignationInBengali
	   ,ejc.EmployeeCategoryId
	   ,ejc.JoiningDate
	   ,ejc.QuitDate
	   ,ejc.TotalDays
	   ,ejc.WorkingDays
	   ,(ejc.PresentDays - ejc.TotalPenaltyAttendanceDays) AS PresentDays
	   ,ejc.LateDays
	   ,ejc.OSDDays
	   ,(ejc.AbsentDays + ejc.TotalPenaltyAttendanceDays) AS AbsentDays
	   ,ejc.LeaveDays
	   ,ejc.LWPDays
	   ,ejc.Holidays
	   ,ejc.WeekendDays
	   ,ejc.PayDays
	   ,ejc.CasualLeave	
	   ,ejc.SickLeave	
	   ,ejc.MaternityLeave	
	   ,ejc.EarnLeave
	   ,ejc.GrossSalary
	   ,ejc.BasicSalary
	   ,ejc.HouseRent
	   ,ejc.MedicalAllowance  
	   ,ejc.Conveyance
	   ,ejc.FoodAllowance
	   ,ejc.EntertainmentAllowance
	   ,ejc.LWPFee
	   ,CONVERT(DECIMAL(18,2),(ISNULL(ejc.AbsentFee,0.0) + ISNULL(ejc.TotalPenaltyAbsentFee,0.0))) AS AbsentFee
	   ,ejc.TotalPenaltyFinancialAmount
	   ,CONVERT(DECIMAL(18,2), dbo.fnGetAdvanceAmount(ejc.EmployeeId, @FromDate, @ToDate)) AS Advance
	   ,CONVERT(DECIMAL(18,2), (SELECT TOP(1) StampAmount.Amount FROM StampAmount 
						       WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @ToDate
						       ORDER BY StampAmount.FromDate DESC ))  AS Stamp
	   ,CONVERT(DECIMAL(18,2), (ISNULL(ejc.LWPFee, 0.0) 
							   + ISNULL(ejc.AbsentFee, 0.0)
							   + ISNULL(ejc.TotalPenaltyAbsentFee, 0.0)
							   + ISNULL(ejc.TotalPenaltyFinancialAmount, 0.0)
							   + dbo.fnGetAdvancedIncomeTax(ejc.EmployeeId, @FromDate, @ToDate)
							   + (dbo.fnGetAdvanceAmount(ejc.EmployeeId, @FromDate, @ToDate)) 
							   + (SELECT TOP(1) StampAmount.Amount FROM StampAmount 
								   WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @ToDate
						           ORDER BY StampAmount.FromDate DESC ))) AS TotalDeduction
	   ,ejc.AttendanceBonus
	   ,ejc.ShiftingBonus
	   ,CONVERT(DECIMAL(18,2), (ejc.AttendanceBonus + ejc.ShiftingBonus)) AS TotalBonus
	   ,CONVERT(DECIMAL(18,2), (ejc.GrossSalary - (ISNULL(ejc.LWPFee,0.0) + ISNULL(ejc.AbsentFee,0.0) + ISNULL(ejc.TotalPenaltyAbsentFee, 0.0) + ISNULL(ejc.TotalPenaltyFinancialAmount, 0.0) 
	                                             + dbo.fnGetAdvanceAmount(ejc.EmployeeId, @FromDate, @ToDate) 
												 +  (SELECT TOP(1) StampAmount.Amount FROM StampAmount 
													 WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @ToDate
													 ORDER BY StampAmount.FromDate DESC)) 
											     + ejc.AttendanceBonus
												 - dbo.[fnGetAdvancedIncomeTax](ejc.EmployeeId, @FromDate, @ToDate) 
												 - dbo.fnGetMaternityDeduction(ejc.EmployeeId, @FromDate, @ToDate) 
												 + ejc.ShiftingBonus))  AS TotalPaid    -- TotalPaid = gross - deduction - maternity + att Bonus + Shift Allow
	   ,ejc.OTRate AS Rate
	   ,CONVERT(DECIMAL(18,2), ejc.EmployeeOTRate) AS OTRate

	   --,CASE WHEN ((ISNULL(ejc.TotalOTHours, 0.0)) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	   --      THEN (CONVERT(DECIMAL(18,2), (ISNULL(ejc.TotalOTHours, 0.0) - ISNULL(ejc.TotalPenaltyOTHours, 0.0))))

			 --ELSE 0.0 END AS OTHours
		,ISNULL(ejc.TotalOTHours, 0.0) AS OTHours


	   --,CASE WHEN ((ISNULL(ejc.TotalOTHours, 0.0)) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	   --      THEN (CONVERT(DECIMAL(18,2), (ISNULL(ejc.TotalOTAmount, 0.0) - ISNULL(ejc.TotalPenaltyOTAmount, 0.0))))

			 --ELSE 0.0 END AS TotalOTAmount
		,ISNULL(ejc.TotalOTAmount, 0.0) AS TotalOTAmount


	   ,CASE ejc.EmployeeCategoryId
	    WHEN 1
			THEN CONVERT(DECIMAL(18,2),ROUND((ejc.GrossSalary - (ISNULL(ejc.LWPFee,0.0) 
			                                     + ISNULL(ejc.AbsentFee,0.0) 
												 + ISNULL(ejc.TotalPenaltyAbsentFee, 0.0)
												 + dbo.[fnGetAdvancedIncomeTax](ejc.EmployeeId, @FromDate, @ToDate) 
												 + dbo.fnGetMaternityDeduction(ejc.EmployeeId, @FromDate, @ToDate) 
												 + ISNULL(ejc.TotalPenaltyFinancialAmount, 0.0)
			                                     + dbo.fnGetAdvanceAmount(ejc.EmployeeId, @FromDate, @ToDate) 
												 +  (SELECT TOP(1) StampAmount.Amount FROM StampAmount 
													 WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @ToDate
													 ORDER BY StampAmount.FromDate DESC)) 
											     + ejc.AttendanceBonus
												 + ejc.ShiftingBonus
												 + ISNULL(ejc.TotalOTAmount, 0.0)
													),0))
		WHEN 2
			THEN CONVERT(DECIMAL(18,2),ROUND((ejc.GrossSalary - (((ejc.GrossSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 )) * (DATEDIFF(DAY, ejc.QuitDate, @ToDate)))
											     + ISNULL(ejc.LWPFee,0.0) 
												 + ISNULL(ejc.AbsentFee,0.0) 
												 + ISNULL(ejc.TotalPenaltyAbsentFee, 0.0)
												 + ISNULL(ejc.TotalPenaltyFinancialAmount, 0.0) 
												 + dbo.fnGetAdvanceAmount(ejc.EmployeeId, @FromDate, @ToDate) 
												 +  (SELECT TOP(1) StampAmount.Amount FROM StampAmount 
													 WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @ToDate
													 ORDER BY StampAmount.FromDate DESC)) 
											     + ejc.AttendanceBonus
												 + ejc.ShiftingBonus
												 + ISNULL(ejc.TotalOTAmount, 0.0)
													),0))


		WHEN 3
			THEN CONVERT(DECIMAL(18,2),ROUND((ejc.GrossSalary - (((ejc.GrossSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 )) * (DATEDIFF(DAY, @FromDate, ejc.JoiningDate))) 
											     + ISNULL(ejc.LWPFee,0.0) 
												 + ISNULL(ejc.AbsentFee,0.0) 
												 + ISNULL(ejc.TotalPenaltyAbsentFee, 0.0)
												 + ISNULL(ejc.TotalPenaltyFinancialAmount, 0.0)
												 + dbo.fnGetAdvanceAmount(ejc.EmployeeId, @FromDate, @ToDate) 
												 +  (SELECT TOP(1) StampAmount.Amount FROM StampAmount 
													 WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @ToDate
													 ORDER BY StampAmount.FromDate DESC)) 
											     + ejc.AttendanceBonus
												 + ejc.ShiftingBonus
												 + ISNULL(ejc.TotalOTAmount, 0.0)
													),0))

		WHEN 4
		THEN CONVERT(DECIMAL(18,2),ROUND((ejc.GrossSalary - (((ejc.GrossSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 )) * (DATEDIFF(DAY, @FromDate, ejc.JoiningDate)))
												+ ((ejc.GrossSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 )) * (DATEDIFF(DAY, ejc.QuitDate, @ToDate))) 
												+ ISNULL(ejc.LWPFee, 0.0) 
												+ ISNULL(ejc.AbsentFee, 0.0) 
												+ ISNULL(ejc.TotalPenaltyAbsentFee, 0.0)
												+ ISNULL(ejc.TotalPenaltyFinancialAmount, 0.0)
												+ dbo.fnGetAdvanceAmount(ejc.EmployeeId, @FromDate, @ToDate) 
												+  (SELECT TOP(1) StampAmount.Amount FROM StampAmount 
													WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @ToDate
													ORDER BY StampAmount.FromDate DESC)) 
											    + ejc.AttendanceBonus
												+ ejc.ShiftingBonus
												+ ISNULL(ejc.TotalOTAmount, 0.0)
													),0))


			END AS NetAmount
		
	   ,CASE WHEN ((ISNULL(ejc.TotalExtraOTHours, 0.0)) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	         THEN (CONVERT(DECIMAL(18,2), (ISNULL(ejc.TotalExtraOTHours, 0.0) - (ISNULL(ejc.TotalPenaltyOTHours, 0.0)))))

			 -- WHEN (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0))) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
			 -- THEN (CONVERT(DECIMAL(18,2), (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0))) - (ISNULL(ejc.TotalPenaltyOTHours, 0.0)))))

			 ELSE 0.0 END AS TotalExtraOTHours

		 ,CASE WHEN ((ISNULL(ejc.TotalExtraOTHours, 0.0)) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	         THEN (CONVERT(DECIMAL(18,2), (ISNULL(ejc.TotalExtraOTAmount, 0.0) - (ISNULL(ejc.TotalPenaltyOTAmount, 0.0)))))

			 -- WHEN (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0))) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
			 -- THEN (CONVERT(DECIMAL(18,2), (((ISNULL(ejc.TotalOTAmount, 0.0)) + (ISNULL(ejc.TotalExtraOTAmount, 0.0))) - (ISNULL(ejc.TotalPenaltyOTAmount, 0.0)))))

			 ELSE 0.0 END AS TotalExtraOTAmount

		,CASE  WHEN (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0))) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	         THEN (CONVERT(DECIMAL(18,2), (ISNULL(ejc.TotalWeekendOTHours, 0.0))))

			 WHEN (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0)) + (ISNULL(ejc.TotalWeekendOTHours, 0.0))) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	         THEN (CONVERT(DECIMAL(18,2), (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0)) + (ISNULL(ejc.TotalWeekendOTHours, 0.0))) - (ISNULL(ejc.TotalPenaltyOTHours, 0.0)))))

			 ELSE 0.0 END AS TotalWeekeendOTHours

		,CASE  WHEN (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0))) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	         THEN (CONVERT(DECIMAL(18,2), (ISNULL(ejc.TotalWeekendOTAmount, 0.0))))

			 WHEN (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0)) + (ISNULL(ejc.TotalWeekendOTHours, 0.0))) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	         THEN (CONVERT(DECIMAL(18,2), (((ISNULL(ejc.TotalOTAmount, 0.0)) + (ISNULL(ejc.TotalExtraOTAmount, 0.0)) + (ISNULL(ejc.TotalWeekendOTAmount, 0.0))) - (ISNULL(ejc.TotalPenaltyOTAmount, 0.0)))))

			 ELSE 0.0 END AS TotalWeekendOTAmount

	    ,CASE  WHEN (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0)) + (ISNULL(ejc.TotalWeekendOTHours, 0.0))) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	         THEN (CONVERT(DECIMAL(18,2), (ISNULL(ejc.TotalHolidayOTHours, 0.0))))

			 WHEN (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0)) + (ISNULL(ejc.TotalWeekendOTHours, 0.0)) + (ISNULL(ejc.TotalHolidayOTHours, 0.0))) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	         THEN (CONVERT(DECIMAL(18,2), (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0)) + (ISNULL(ejc.TotalWeekendOTHours, 0.0)) + (ISNULL(ejc.TotalHolidayOTHours, 0.0))) - (ISNULL(ejc.TotalPenaltyOTHours, 0.0)))))

			 ELSE 0.0 END AS TotalHolidayOTHours

		,CASE  WHEN (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0)) + (ISNULL(ejc.TotalWeekendOTHours, 0.0))) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	         THEN (CONVERT(DECIMAL(18,2), (ISNULL(ejc.TotalHolidayOTAmount, 0.0))))

			 WHEN (((ISNULL(ejc.TotalOTHours, 0.0)) + (ISNULL(ejc.TotalExtraOTHours, 0.0)) + (ISNULL(ejc.TotalWeekendOTHours, 0.0)) + (ISNULL(ejc.TotalHolidayOTHours, 0.0))) >= (ISNULL(ejc.TotalPenaltyOTHours, 0.0))) 
	         THEN (CONVERT(DECIMAL(18,2), (((ISNULL(ejc.TotalOTAmount, 0.0)) + (ISNULL(ejc.TotalExtraOTAmount, 0.0)) + (ISNULL(ejc.TotalWeekendOTAmount, 0.0)) + (ISNULL(ejc.TotalHolidayOTAmount, 0.0))) - (ISNULL(ejc.TotalPenaltyOTAmount, 0.0)))))

			 ELSE 0.0 END AS TotalHolidayOTAmount

		,dbo.[fnGetAdvancedIncomeTax](ejc.EmployeeId, @FromDate, @ToDate) 
		,CURRENT_TIMESTAMP
		,@UserID
		,NULL
		,NULL
		,1	
		
	    FROM EmployeeJobCardGrossDeduction ejc
		WHERE ejc.[Year] = @Year
		AND ejc.[Month] = @Month
		AND ejc.[FromDate] = @FromDate
		AND ejc.[ToDate] = @ToDate 
		AND ejc.IsActive = 1 
		AND (ejc.CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds))
		AND (ejc.BranchId  IN (SELECT BranchIDs FROM @ListOfBranchIds))
		AND (ejc.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds))
		AND (ejc.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds))
		AND (ejc.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
		AND (ejc.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
		AND (ejc.EmployeeTypeId IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds))
		AND (ejc.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId IS NULL)
		AND ejc.EmployeeTypeId <> 1	 -- 1 for Management Committee
		AND ejc.BranchUnitId IN (1,2) --- 1 for Garments, 2 for Knitting
		---AND ejc.DepartmentSectionId <> 35 ---35 for security
	    AND (ejc.DepartmentSectionId <> 35 OR ejc.DepartmentSectionId IS NULL)---35 for security
		AND ejc.EmployeeId NOT IN (SELECT EmployeeId FROM PayrollExcludedEmployeeFromSalaryProcess eesp
									   WHERE eesp.[Year] = @Year
									   AND eesp.[Month] = @Month
									   AND eesp.[FromDate] = @FromDate
									   AND eesp.[ToDate] = @ToDate 
									   AND eesp.IsActive = 1 )

			-- No Stamp amount deduction for Bank Salary
		
		UPDATE EmployeeProcessedSalaryGrossDeduction
		SET  TotalDeduction = TotalDeduction - Stamp
		    ,TotalPaid = TotalPaid + Stamp
			,NetAmount = NetAmount + Stamp
			,Stamp = 0
		WHERE FromDate = @FromDate 
		AND ToDate = @ToDate
		AND EmployeeId IN
		(
			SELECT EmployeeId     
			FROM EmployeeBankInfo
			WHERE FromDate <= @ToDate 
			AND (ToDate IS NULL OR ToDate >= @FromDate)
		) 


		COMMIT TRAN

		IF (@@ERROR <> 0)
			SELECT 0;
		ELSE
			SELECT 1;
END






