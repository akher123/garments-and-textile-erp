
-- ============================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-11-22>
-- Description:	<> EXEC [SPGetEmployeeSalaryProcessedInfo] 0, 10, 1, 1, 1, -1, -1, -1,-1,'',2015,10, '2015-10-01','2015-10-31','superadmin'
-- ============================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeSalaryProcessedInfo]

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
								    ,[EmployeeCardId] NVARCHAR(100)
									,[Name] NVARCHAR(100)	
									,[MobileNo] NVARCHAR(100)						
									,[Branch] NVARCHAR(100) NULL
									,[Unit] NVARCHAR(100) NULL
									,[Department] NVARCHAR(100) NULL
									,[Section] NVARCHAR(100) NULL
									,[Line] NVARCHAR(100) NULL
									,[EmployeeType] NVARCHAR(100) NULL
									,[Grade] NVARCHAR(100) NULL
									,[Designation] NVARCHAR(100) NULL
									,[JoiningDate] DATETIME NULL
									,[QuitDate] DATETIME NULL
									,[TotalDays] INT NULL
									,[WorkingDays] INT NULL
									,[Paydays] INT NULL
									,[WeekendDays] INT NULL
									,[HolidayDays] INT NULL
									,[PresentDays] INT NULL
									,[AbsentDays] INT NULL
									,[LateDays] INT NULL
									,[LeaveDays] INT NULL
									,[LWPDays] INT NULL
									,[CasualLeave] INT NULL
									,[SickLeave] INT NULL
									,[MaternityLeave] INT NULL
									,[EarnLeave] INT NULL
									,[BasicSalary] NUMERIC(18,2)
									,[HouseRent] NUMERIC(18,2)
									,[MedicalAllowance] NUMERIC(18,2)
									,[Conveyance] NUMERIC(18,2)
									,[FoodAllowance] NUMERIC(18,2) NULL
									,[EntertainmentAllowance] NUMERIC(18,2) NULL
									,[GrossSalary] NUMERIC(18,2) 
									,[LWPFee] NUMERIC(18,2) NULL
									,[AbsentFee] NUMERIC(18,2) NULL
									,[Advance] NUMERIC(18,2) NULL
									,[Stamp] NUMERIC(18,2) 
									,[TotalDeduction] NUMERIC(18,2) 
									,[AttendanceBonus] NUMERIC(18,2) NULL
									,[ShiftingBonus] NUMERIC(18,2) NULL
									,[TotalBonus] NUMERIC(18,2) NULL
									,[TotalPaid] NUMERIC(18,2) 
									,[OTHours] NUMERIC(18,2) NULL
									,[OTRate] NUMERIC(18,2) NULL
									,[TotalOTAmount] NUMERIC(18,2) NULL
									,[NetAmount] NUMERIC(18,2) 
									,[Month] INT
									,[Year] INT
									,[FromDate] DATETIME NULL
									,[ToDate] DATETIME NULL
								);

		 INSERT INTO @ReturnTable
		 SELECT DISTINCT
						 ROW_NUMBER() OVER (ORDER BY EmployeeCardId ASC) AS RowNumber	
						,0			
						,[EmployeeCardId]
						,[Name]		
						,[MobileNo]				
						,[Branch]
						,[Unit]
						,[Department]
						,[Section]
						,[Line]
						,[EmployeeType]
						,[Grade]
						,[Designation]
						,eps.JoiningDate
						,eps.QuitDate
						,[TotalDays]
						,[WorkingDays]
						,[Paydays]
						,[WeekendDays]
						,[HolidayDays]
						,[PresentDays]
						,[AbsentDays]
						,[LateDays]
						,[LeaveDays]
						,[LWPDays]
						,[CasualLeave]
						,[SickLeave]
						,[MaternityLeave]
						,[EarnLeave]
						,[BasicSalary]
						,[HouseRent]
						,[MedicalAllowance]
						,[Conveyance]
						,[FoodAllowance]
						,[EntertainmentAllowance]
						,[GrossSalary]
						,[LWPFee]
						,[AbsentFee]
						,[Advance]
						,[Stamp]
						,[TotalDeduction]
						,[AttendanceBonus]
						,[ShiftingBonus]
						,[TotalBonus]
						,[TotalPaid]
						,[OTHours]
						,[OTRate]
						,[TotalOTAmount]
						,[NetAmount]
						,[Month]
						,[Year]
						,[FromDate]
						,[ToDate]
						
			FROM		[dbo].[EmployeeProcessedSalary] eps
			WHERE 
						eps.[Year] = @Year
						AND eps.[Month] = @Month
						AND CAST(eps.FromDate AS DATE) = CAST(@FromDate AS DATE)
						AND CAST(eps.ToDate AS DATE) = CAST(@ToDate AS DATE)
						AND (eps.CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds))
						AND (eps.BranchId  IN (SELECT BranchIDs FROM @ListOfBranchIds))
						AND (eps.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds))
						AND (eps.DepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds))
						AND (eps.SectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
						AND (eps.LineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
						AND (eps.EmployeeTypeId IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds))
						AND (eps.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId IS NULL)	
						ORDER BY EmployeeCardId ASC	

				DECLARE @TotalRecords INT = NULL;

				SELECT @TotalRecords = COUNT(*) FROM @ReturnTable

				UPDATE @ReturnTable
				SET TotalRows = @TotalRecords;			

				SELECT * FROM @ReturnTable
				WHERE RowID BETWEEN (@StartRowIndex * @MaxRows) + 1 AND ((@StartRowIndex+1) * @MaxRows)

		COMMIT TRAN


END






