
-- ===============================================================================================================================
-- Author:		<<Golam Rabbi>
-- Create date: <18/03/2015>
-- Description:	<> EXEC SPGetEmployeeAllPayments -1, -1, -1, -1, -1, -1, -1, '',2019, 05, '2019-04-26','2019-05-25',2,'superadmin'
-- ===============================================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeAllPayments]
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
						@EmployeeCategoryId INT,
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
					  
			SELECT		CASE 
						WHEN @EmployeeCategoryId = 2 THEN 'Quit Employee'
						WHEN @EmployeeCategoryId = 3 THEN 'New Joining Employee'
						WHEN @EmployeeCategoryId = 4 THEN 'New Joining And Quit Employee'
						ELSE 'Regular Employee'
						END AS EmployeeCategory  
						,[EmployeeId]
						,[EmployeeCardId]
						,[CompanyId]
						,[CompanyName]
						,[CompanyAddress]
						,[Name]
						,[Designation]
						,[Grade]
						,CONVERT(VARCHAR(10), eps.JoiningDate, 103) AS JoiningDate
						,CONVERT(VARCHAR(10), eps.QuitDate, 103) AS QuitDate
						,[BranchId]
						,[Branch]
						,[BranchUnitId]
						,[Unit]
						,[Department]
						,CONVERT(NVARCHAR(100),@DepartmentName) AS DepartmentName
						,CONVERT(NVARCHAR(100),@SectionName) AS SectionName
						,CONVERT(NVARCHAR(100),@LineName) AS LineName
					    ,CONVERT(NVARCHAR(100),@EmployeeTypeName) AS EmployeeTypeName
						,[NetAmount]
						,ROUND(CONVERT(DECIMAL(18,2), (ISNULL([TotalExtraOTAmount],0) + ISNULL([TotalWeekendOTAmount],0) + ISNULL([TotalHolidayOTAmount],0))),0) AS TotalOthersOT
						,ROUND(CONVERT(DECIMAL(18,2), (ISNULL([NetAmount],0) + ISNULL([TotalExtraOTAmount],0) + ISNULL([TotalWeekendOTAmount],0) + ISNULL([TotalHolidayOTAmount],0))),0) AS [AmountToBePaid]
						,[Month]
						,[Year]
						,[DepartmentId]
						,[SectionId]
						,[LineId]
						,[EmployeeTypeId]
						,[MonthName]
						,CONVERT(VARCHAR(10), [FromDate], 103) AS FromDate
						,CONVERT(VARCHAR(10), [ToDate], 103) AS ToDate
						
FROM					[dbo].[EmployeeProcessedSalary] eps
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
						AND (eps.EmployeeCategoryId = @EmployeeCategoryId)	
						
						--AND eps.EmployeeCardId > 5046
																	
						ORDER BY EmployeeCardId ASC	
END