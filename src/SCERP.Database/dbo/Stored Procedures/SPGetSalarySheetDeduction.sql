

-- =========================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <18/03/2015>
-- Description:	<> EXEC [SPGetSalarySheetGrossDeduction] -1, -1, -1, -1, -1, -1, -1, '', 2016,1,'2015-12-26','2016-01-25',-1,'superadmin'
-- ========================================================================================================================


CREATE PROCEDURE [dbo].[SPGetSalarySheetDeduction]

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
						@EmployeeCategoryId INT = -1,
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
			
			IF (@EmployeeCategoryId  = -1)
			BEGIN
				SET @EmployeeCategoryId = NULL
			END

			DECLARE @NewLineChar AS CHAR(2) = CHAR(13) + CHAR(10)
				  
			SELECT		 
						CASE 
						WHEN @EmployeeCategoryId = 1 THEN 'Regular Employee'
						WHEN @EmployeeCategoryId = 2 THEN 'Quit Employee'
						WHEN @EmployeeCategoryId = 3 THEN 'New Joining Employee'
						WHEN @EmployeeCategoryId = 4 THEN 'New Joining And Quit Employee'
						ELSE 'All Employees'
						END AS EmployeeCategory
						,CONVERT(NVARCHAR(100),@CompanyName) AS CompanyName
						,CONVERT(NVARCHAR(100),@CompanyAddress) AS CompanyAddress
						,CONVERT(NVARCHAR(100),@BranchName) AS BranchName
						,CONVERT(NVARCHAR(100),@UnitName) AS UnitName
						,CONVERT(NVARCHAR(100),@DepartmentName) AS DepartmentName
						,CONVERT(NVARCHAR(100),@SectionName) AS SectionName
						,CONVERT(NVARCHAR(100),@LineName) AS LineName
					    ,CONVERT(NVARCHAR(100),@EmployeeTypeName) AS EmployeeTypeName 						
						,[CompanyId]
						,[CompanyName] AS Company
						,[BranchId]
						,[Branch]
						,[BranchUnitId]
						,[Unit]
						,[DepartmentId]
						,[Department]
						,[SectionId]
						,[Section]
						,[LineId]
						,[Line]
						,[EmployeeTypeId]
						,[EmployeeType]
						,[EmployeeId]
						,[EmployeeCardId]
						,[Name]
						,[EmployeeCardId] + @NewLineChar + [Name] AS CardIdAndName
						,[Designation]
						,[Grade]
						,[Grade] + @NewLineChar + [Designation] AS GradeAndDesignation
						,CONVERT(VARCHAR(10), eps.JoiningDate, 103) AS JoiningDate
						,CONVERT(VARCHAR(10), eps.QuitDate, 103) AS QuitDate
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
						,[TotalExtraOTAmount]
						,[TotalWeekendOTAmount]
						,[Month]
						,[Year]						
						,[MonthName]
						,CONVERT(VARCHAR(10), [FromDate], 103) AS FromDate
						,CONVERT(VARCHAR(10), [ToDate], 103) AS ToDate
						,AdvancedIncomeTax
						
FROM					[dbo].[EmployeeProcessedSalaryGrossDeduction] eps
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
						AND (eps.EmployeeCategoryId = @EmployeeCategoryId OR @EmployeeCategoryId IS NULL)						
																	
						ORDER BY EmployeeCardId ASC	
END






