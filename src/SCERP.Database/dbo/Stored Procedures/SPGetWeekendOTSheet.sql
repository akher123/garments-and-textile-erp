
-- ============================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <30-Sep-15 2:09:40 PM>
-- Description:	<> EXEC [SPGetWeekendOTSheet] -1, -1, -1, -1, -1, -1, -1,'', 2015, 10,'2015-10-01','2015-10-31', 1, 'superadmin'
-- =============================================================================================================================


CREATE PROCEDURE [dbo].[SPGetWeekendOTSheet]
						
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

			SELECT 
				   @CompanyName AS CompanyName
				  ,@BranchName AS BranchName
				  ,@UnitName AS UnitName
				  ,@DepartmentName AS DepartmentName
				  ,@SectionName AS SectionName
				  ,@LineName AS LineName
				  ,@EmployeeTypeName AS EmployeeTypeName
				  ,@CompanyAddress AS CompanyAddress
				  ,eps.[EmployeeId]
				  ,eps.[EmployeeCardId]
				  ,eps.[Name] 
				  ,eps.[CompanyId]
				  ,eps.[CompanyName] AS Company
				  ,eps.[BranchId]
				  ,eps.[Branch]
				  ,eps.[BranchUnitId]
				  ,eps.[Unit] 
				  ,eps.[DepartmentId] 
				  ,eps.[Department] 
				  ,eps.[SectionId] 
				  ,eps.[Section] 
				  ,eps.[LineId] 
				  ,eps.[Line]
				  ,eps.[EmployeeTypeId]
				  ,eps.[EmployeeType]
				  ,eps.[Designation] 
				  ,eps.[Grade] AS Grade
				  ,CONVERT(VARCHAR(10), eps.JoiningDate, 103) AS JoiningDate
				  ,eps.[BasicSalary]
				  ,eps.[GrossSalary]
				  ,eps.[TotalWeekendOTHours] 
				  ,eps.[OTRate] 
				  ,eps.[TotalWeekendOTAmount] 
				  ,eps.[MonthName] AS   [MonthYear]
				  ,CONVERT(VARCHAR(10), [FromDate], 103) AS FromDate
				  ,CONVERT(VARCHAR(10), [ToDate], 103) AS ToDate
			
				FROM [dbo].[EmployeeProcessedSalary] eps

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
						AND  eps.TotalWeekendOTAmount > 0.0	
						
						--AND eps.EmployeeCardId > 5046

						ORDER BY EmployeeCardId ASC	
END






