
-- ============================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-12>
-- Description:	<> EXEC [SPGetEmployeeJobCardModelProcessedInfo] 0, 10, 1, 1, 1, -1, -1, -1,-1,'',2015,10, '2015-10-01','2015-10-31','superadmin'
-- ============================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeJobCardModelProcessedInfo]

	   @StartRowIndex INT = NULL,
	   @MaxRows INT = NULL,

	   @CompanyId INT = -1,
	   @BranchId INT = -1,
	   @BranchUnitId INT = -1,
	   @BranchUnitDepartmentId INT = -1, 
	   @DepartmentSectionId INT = -1, 
	   @DepartmentLineId INT = -1, 
	   @EmployeeTypeId INT = -1,
	   @EmployeeCardId NVARCHAR(100) = '',
	   @Year INT = -1,
	   @Month INT = -1,
	   @FromDate DateTime,
	   @ToDate DateTime,	   
	   @UserName NVARCHAR(100)

	   
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

		
		IF (@EmployeeCardId  = '')
		BEGIN
			SET @EmployeeCardId = NULL
		END

		DECLARE @ReturnTable TABLE (
								   RowID BIGINT NULL
								  ,TotalRows INT NULL
								  ,[EmployeeId] UNIQUEIDENTIFIER
								  ,[Year] INT
								  ,[Month] INT
								  ,[MonthName] NVARCHAR(100)
								  ,[FromDate] DATETIME  
								  ,[ToDate] DATETIME
								  ,[CompanyName] NVARCHAR(100) 
								  ,[BranchName] NVARCHAR(100)
								  ,[UnitName] NVARCHAR(100) 
								  ,[DepartmentName] NVARCHAR(100) 
								  ,[SectionName] NVARCHAR(100) NULL
								  ,[LineName] NVARCHAR(100) NULL
								  ,[EmployeeCardId] NVARCHAR(100) 
								  ,[EmployeeName] NVARCHAR(100) 
								  ,[MobileNo] NVARCHAR(100) 
								  ,[EmployeeType] NVARCHAR(100) 
								  ,[EmployeeGrade] NVARCHAR(100) 
								  ,[EmployeeDesignation] NVARCHAR(100) 
								  ,[EmployeeCategoryId] INT
								  ,[JoiningDate] DATETIME
								  ,[QuitDate] DATETIME NULL
								  ,[TotalDays] INT
								  ,[WorkingDays] INT
								  ,[PresentDays] INT
								  ,[LateDays] INT
								  ,[OSDDays] INT
								  ,[AbsentDays] INT
								  ,[LeaveDays] INT
								  ,[LWPDays] INT
								  ,[Holidays] INT  
								  ,[WeekendDays] INT
								  ,[PayDays] INT
								  ,[CasualLeave] INT
								  ,[SickLeave] INT
								  ,[MaternityLeave] INT
								  ,[EarnLeave] INT
								  ,[GrossSalary] NUMERIC(18, 2)
								  ,[BasicSalary] NUMERIC(18, 2)
								  ,[HouseRent] NUMERIC(18, 2)
								  ,[MedicalAllowance] NUMERIC(18, 2)
								  ,[Conveyance] NUMERIC(18, 2)
								  ,[FoodAllowance] NUMERIC(18, 2) NULL
								  ,[EntertainmentAllowance] NUMERIC(18, 2) NULL
								  ,[PerDayBasicSalary] NUMERIC(18, 2) NULL
								  ,[LWPFee] NUMERIC(18, 2) NULL
								  ,[AbsentFee] NUMERIC(18, 2) NULL
								  ,[AttendanceBonus] NUMERIC(18, 2) NULL
								  ,[ShiftingBonus] NUMERIC(18, 2) NULL
								  ,[TotalOTHours] NUMERIC(18, 2) NULL
								  ,[OTRate] NUMERIC(18, 2) NULL
								  ,[EmployeeOTRate] NUMERIC(18, 2) NULL
								  ,[TotalOTAmount] NUMERIC(18, 2) NULL
								);

		 INSERT INTO @ReturnTable
		 SELECT DISTINCT
				 ROW_NUMBER() OVER (ORDER BY EmployeeCardId ASC) AS RowNumber	
				,0			
				,[EmployeeId] 
				,[Year]
				,[Month]
				,[MonthName]
				,[FromDate]
				,[ToDate]
				,[CompanyName]
				,[BranchName]
				,[UnitName]
				,[DepartmentName]
				,[SectionName]
				,[LineName]
				,[EmployeeCardId]
				,[EmployeeName]
				,[MobileNo]
				,[EmployeeType]
				,[EmployeeGrade]
				,[EmployeeDesignation]
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
				,[Holidays]
				,[WeekendDays]
				,[PayDays]
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
				,[PerDayBasicSalary]
				,[LWPFee]
				,[AbsentFee]
				,[AttendanceBonus]
				,[ShiftingBonus]
				,[TotalOTHours]
				,[OTRate]
				,[EmployeeOTRate]
				,[TotalOTAmount]
				FROM EmployeeJobCardModel ejc
				WHERE 
				ejc.IsActive = 1
				AND ejc.CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds)
				AND ejc.BranchId IN (SELECT BranchIDs FROM @ListOfBranchIds)
				AND ejc.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
				AND ejc.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
				AND (ejc.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
				AND (ejc.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
				AND ejc.EmployeeTypeId IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
				AND ((ejc.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId IS NULL))
				AND ejc.[Year] = @Year
				AND ejc.[Month] = @Month
				AND CAST(ejc.FromDate AS DATE) = CAST(@FromDate AS DATE)
				AND CAST(ejc.ToDate AS DATE) = CAST(@ToDate AS DATE)	
				ORDER BY DepartmentName, EmployeeCardId ASC

				DECLARE @TotalRecords INT = NULL;

				SELECT @TotalRecords = COUNT(*) FROM @ReturnTable

				UPDATE @ReturnTable
				SET TotalRows = @TotalRecords;			

				SELECT * FROM @ReturnTable
				WHERE RowID BETWEEN (@StartRowIndex * @MaxRows) + 1 AND ((@StartRowIndex+1) * @MaxRows)

		COMMIT TRAN


END






