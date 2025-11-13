
-- =======================================================================================================================================
-- Author:		<S. M. Golam Rabbi>
-- Create date: <2015-10-18>						*** Running Time : 28:17
-- Description:	<> EXEC dbo.[SPProcessEmployeeJobCard]  1, 1, -1, -1, -1, -1, -1, '', 2020, 07, '2020-06-26','2020-07-25','superadmin'  (FULL MONTH PROCESS)
-- =======================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeJobCard]	   				
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

			DECLARE @UserID UNIQUEIDENTIFIER;
			SELECT @UserID = EmployeeID FROM [User] WHERE UserName = @UserName;

			DECLARE	@Days INT;

			DELETE FROM [dbo].[EmployeeJobCard]
			WHERE           
			(
			(CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds))
			AND (BranchId IN (SELECT BranchIDs FROM @ListOfBranchIds))
			AND (BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds))
			AND (BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds))
			AND (DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
			AND (DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
			AND (EmployeeTypeId IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds))
			AND (EmployeeCardId = @EmployeeCardId OR @EmployeeCardId IS NULL)		
			AND ([Year] = @Year)
			AND ([Month] = @Month)
			AND (CAST([FromDate] AS DATE) = @FromDate)
			AND (CAST([ToDate] AS DATE) = @ToDate)
			AND (IsActive=1)
			)		
		
			SET FMTONLY OFF;

		    BEGIN
					INSERT INTO [dbo].[EmployeeJobCard]
				   ([EmployeeId]
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
				   ,[BranchName]
				   ,[BranchNameInBengali]
				   ,[BranchUnitId]
				   ,[UnitName]
				   ,[UnitNameInBengali]
				   ,[BranchUnitDepartmentId]
				   ,[DepartmentName]
				   ,[DepartmentNameInBengali]
				   ,[DepartmentSectionId]
				   ,[SectionName]
				   ,[SectionNameInBengali]
				   ,[DepartmentLineId]
				   ,[LineName]
				   ,[LineNameInBengali]
				   ,[EmployeeCardId]
				   ,[EmployeeName]
				   ,[EmployeeNameInBengali]
				   ,[MobileNo]
				   ,[EmployeeTypeId]
				   ,[EmployeeType]
				   ,[EmployeeTypeInBengali]
				   ,[EmployeeGradeId]
				   ,[EmployeeGrade]
				   ,[EmployeeGradeInBengali]
				   ,[EmployeeDesignationId]
				   ,[EmployeeDesignation]
				   ,[EmployeeDesignationInBengali]
				   ,[EmployeeActiveStatusId]
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
				   ,[TotalExtraOTHours]
				   ,[TotalWeekendOTHours]
				   ,[TotalHolidayOTHours]
				   ,[OTRate]
				   ,[EmployeeOTRate]
				   ,[TotalOTAmount]
				   ,[TotalExtraOTAmount]
				   ,[TotalWeekendOTAmount]
				   ,[TotalHolidayOTAmount]
				   ,[TotalPenaltyOTHours]
				   ,[TotalPenaltyOTAmount]
				   ,[TotalPenaltyAttendanceDays]
				   ,[TotalPenaltyAbsentFee]
				   ,[TotalPenaltyLeaveDays]
				   ,[TotalPenaltyFinancialAmount]
				   ,[CreatedDate]
				   ,[CreatedBy]
				   ,[IsActive])
					  
					SELECT
					 employee.EmployeeId		     
					,@Year
					,@Month	
					,(DATENAME(m,@ToDate)+'-'+CAST(DATEPART(yyyy,@ToDate) AS VARCHAR(100)))		
					,@FromDate
					,@ToDate	
					,company.Id
					,company.Name
					,ISNULL(company.NameInBengali,'')
					,ISNULL(company.FullAddress,'')
					,ISNULL(company.FullAddressInBengali,'')
					,branch.Id
					,branch.Name
					,ISNULL(branch.NameInBengali,'')
					,branchUnit.BranchUnitId
					,unit.Name
					,ISNULL(unit.NameInBengali,'')
					,branchUnitDepartment.BranchUnitDepartmentId
					,department.Name
					,ISNULL(department.NameInBengali,'')
					,ISNULL(departmentSection.DepartmentSectionId,0)
					,ISNULL(section.Name,'')
					,ISNULL(section.NameInBengali,'')
					,ISNULL(departmentLine.DepartmentLineId,0)
					,ISNULL(line.Name,'')
					,ISNULL(line.NameInBengali,'')
					,employee.EmployeeCardId
					,employee.Name 
					,employee.NameInBengali
					,ISNULL(presentAddress.MobilePhone,'')
					,employeeType.Id
					,employeeType.Title
					,ISNULL(employeeType.TitleInBengali,'')
					,employeeGrade.Id
					,employeeGrade.Name
					,ISNULL(employeeGrade.NameInBengali,'')
					,employeeDesignation.Id
					,employeeDesignation.Title
					,ISNULL(employeeDesignation.TitleInBengali,'')
					,employee.[Status] 
					,CASE 
						WHEN ((CAST(employee.JoiningDate AS DATE) >= @FromDate AND  CAST(employee.JoiningDate AS DATE) <= @ToDate) 
							 AND ((employee.QuitDate IS NULL) OR ((employee.QuitDate > @ToDate) AND (employee.QuitDate <= (DATEADD(DAY, 30, @ToDate)))))) THEN 3 
						WHEN ((employee.[Status] = 2) AND (CAST(employee.JoiningDate AS DATE) >= @FromDate) AND  (CAST(employee.JoiningDate AS DATE) <= @ToDate) 
							AND (CAST(employee.QuitDate AS DATE) >= @FromDate) AND  (CAST(employee.QuitDate AS DATE) <= @ToDate)) THEN 4 
						WHEN ((employee.[Status] = 2) AND (employee.QuitDate >= @FromDate) AND (employee.QuitDate <= @ToDate) AND (employee.JoiningDate < @FromDate)) THEN 2
						WHEN ((employee.[Status] = 2) AND (employee.QuitDate > @ToDate) AND (employee.QuitDate <= (DATEADD(DAY, 30, @ToDate)))) THEN 1
						ELSE employee.[Status] 
						END AS EmployeeCategory 
					,employee.JoiningDate
					,employee.QuitDate
					,DATEDIFF(DAY, @FromDate, @ToDate) + 1
					,dbo.fnGetWorkingDays(Employee.EmployeeId, @FromDate, @ToDate)	
					,dbo.fnGetPresentDays(Employee.EmployeeId, @FromDate, @ToDate)
					,dbo.fnGetTotalLateDays(Employee.EmployeeId, @FromDate, @ToDate)
					,dbo.fnGetTotalOSDDays(Employee.EmployeeId, @FromDate, @ToDate)
					,dbo.fnGetAbsentDays(Employee.EmployeeId, @FromDate, @ToDate)
					,dbo.fnGetTotalLeave(Employee.EmployeeId, @FromDate, @ToDate)
					,dbo.fnGetIndividualLeaveDays(Employee.EmployeeId, @FromDate, @ToDate, 6) -- LWP Leave
					,dbo.fnGetHolidays(Employee.EmployeeId,@FromDate, @ToDate)
					,dbo.fnGetWeekend(Employee.EmployeeId,@FromDate, @ToDate)
					,dbo.fnGetPayDays(Employee.EmployeeId,@FromDate, @ToDate)
					,dbo.fnGetIndividualLeaveDays(Employee.EmployeeId, @FromDate, @ToDate, 1) -- Casual Leave
					,dbo.fnGetIndividualLeaveDays(Employee.EmployeeId, @FromDate, @ToDate, 2) -- Sick Leave
					,dbo.fnGetIndividualLeaveDays(Employee.EmployeeId, @FromDate, @ToDate, 4) -- Maternity Leave
					,dbo.fnGetIndividualLeaveDays(Employee.EmployeeId, @FromDate, @ToDate, 5) -- Earn Leave												
					,CONVERT(DECIMAL(18,2), ISNULL(employeeSalary.GrossSalary, 0))
					,CONVERT(DECIMAL(18,2), ISNULL(employeeSalary.BasicSalary, 0))
					,CONVERT(DECIMAL(18,2), ISNULL(employeeSalary.HouseRent, 0))
					,CONVERT(DECIMAL(18,2), ISNULL(employeeSalary.MedicalAllowance,0)) 
					,CONVERT(DECIMAL(18,2), ISNULL(employeeSalary.Conveyance, 0)) 
					,CONVERT(DECIMAL(18,2), ISNULL(employeeSalary.FoodAllowance, 0))
					,CONVERT(DECIMAL(18,2), ISNULL(employeeSalary.EntertainmentAllowance, 0)) 
					,CONVERT(DECIMAL(18,2), ISNULL((employeeSalary.BasicSalary /(DATEDIFF(DAY, @FromDate, @ToDate) + 1 )), 0))
					,CONVERT(DECIMAL(18,2), ISNULL(((employeeSalary.BasicSalary /30) * (dbo.fnGetIndividualLeaveDays(Employee.EmployeeId, @FromDate, @ToDate, 6))), 0)) -- LWP
					,CONVERT(DECIMAL(18,2), ISNULL((((employeeSalary.BasicSalary /30) * (dbo.fnGetAbsentDays(Employee.EmployeeId, @FromDate, @ToDate)))), 0)) -- + ((employeeSalary.GrossSalary /30) * 5))) --- For November'2015 (25 Days Salary).Gross salary of 5 days will be added to absent fee. It will be changed from december'2015.
					,CONVERT(DECIMAL(18,2), ISNULL(dbo.fnGetAttendanceBonusAmount(Employee.EmployeeId, @FromDate, @ToDate), 0))
					,CONVERT(DECIMAL(18,2), 0.00)	-- ShiftingBonus...Business logic not defined yet
					,CONVERT(DECIMAL(18,2),ISNULL((dbo.fnGetTotalOTHours(Employee.EmployeeId, @fromDate, @toDate)), 0))
					,CONVERT(DECIMAL(18,2),ISNULL((dbo.fnGetTotalExtraOTHours(Employee.EmployeeId, @fromDate, @toDate)), 0))
					,CONVERT(DECIMAL(18,2),ISNULL((dbo.fnGetTotalWeekendOTHours(Employee.EmployeeId, @fromDate, @toDate)), 0))
					,CONVERT(DECIMAL(18,2),ISNULL((dbo.fnGetTotalHolidayOTHours(Employee.EmployeeId, @fromDate, @toDate)), 0))
					,ISNULL(dbo.fnGetOverTimeRate(@fromDate, @toDate),0)
					,CASE employeeCompanyInfo.IsEligibleForOvertime WHEN 1 THEN 
								CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@fromDate, @toDate))) AS DECIMAL(18,2)) ELSE 0.00 END 

					,CONVERT(DECIMAL(18,2), (CAST((((employeeSalary.BasicSalary/208.00)* (dbo.fnGetOverTimeRate(@fromDate, @toDate))) 
								* (dbo.fnGetTotalOTHours(Employee.EmployeeId, @fromDate, @toDate))) AS DECIMAL(18,2))))
				
					,CONVERT(DECIMAL(18,2), (CAST((((employeeSalary.BasicSalary/208.00)* (dbo.fnGetOverTimeRate(@fromDate, @toDate))) 
								* (dbo.fnGetTotalExtraOTHours(Employee.EmployeeId, @fromDate, @toDate))) AS DECIMAL(18,2))))

					,CONVERT(DECIMAL(18,2), (CAST((((employeeSalary.BasicSalary/208.00)* (dbo.fnGetOverTimeRate(@fromDate, @toDate))) 
								* (dbo.fnGetTotalWeekendOTHours(Employee.EmployeeId, @fromDate, @toDate))) AS DECIMAL(18,2))))

					,CONVERT(DECIMAL(18,2), (CAST((((employeeSalary.BasicSalary/208.00)* (dbo.fnGetOverTimeRate(@fromDate, @toDate))) 
								* (dbo.fnGetTotalHolidayOTHours(Employee.EmployeeId, @fromDate, @toDate))) AS DECIMAL(18,2))))

					,CONVERT(DECIMAL(18,2), dbo.fnGetEmployeeTotalPenaltyOTHours(employee.EmployeeId, employee.JoiningDate, employee.QuitDate, @fromDate, @toDate) + dbo.fnGetAbsentPenalty(employee.EmployeeId,@fromDate, @toDate))

					,CONVERT(DECIMAL(18,2), (CAST((((employeeSalary.BasicSalary/208.00)* (dbo.fnGetOverTimeRate(@fromDate, @toDate))) 
								* (dbo.fnGetEmployeeTotalPenaltyOTHours(employee.EmployeeId, employee.JoiningDate, employee.QuitDate, @fromDate, @toDate) + dbo.fnGetAbsentPenalty(employee.EmployeeId,@fromDate, @toDate) )) AS DECIMAL(18,2))))

					,dbo.fnGetEmployeeTotalPenaltyAttendanceDays(employee.EmployeeId, employee.JoiningDate, employee.QuitDate, @fromDate, @toDate) 

					,CONVERT(DECIMAL(18,2), (((employeeSalary.BasicSalary /30) * (dbo.fnGetEmployeeTotalPenaltyAttendanceDays(employee.EmployeeId, employee.JoiningDate, employee.QuitDate, @fromDate, @toDate))))) 

					,dbo.fnGetEmployeeTotalPenaltyLeaveDays(employee.EmployeeId, employee.JoiningDate, employee.QuitDate, @fromDate, @toDate) 

					,dbo.fnGetEmployeeTotalPenaltyFinancialAmount(employee.EmployeeId, employee.JoiningDate, employee.QuitDate, @fromDate, @toDate) 

					,CURRENT_TIMESTAMP
					,@UserID
					,1
						
				FROM					
				Employee AS  employee
						
				LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,IsEligibleForOvertime,
				ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
				FROM EmployeeCompanyInfo AS employeeCompanyInfo 
				WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
				ON employee.EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1 

				LEFT JOIN (SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,
							employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,
							employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, 
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
							FROM EmployeeSalary AS employeeSalary 
							WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @toDate) AND EmployeeSalary.IsActive=1) employeeSalary 
							ON employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1  

				LEFT JOIN EmployeePresentAddress presentAddress ON Employee.EmployeeId = presentAddress.EmployeeId AND presentAddress.IsActive = 1 
										
				LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
				LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
				LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
				LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
				LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
				LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
				LEFT JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
				LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
				LEFT JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
				LEFT JOIN Company  AS company ON branch.CompanyId=company.Id		
				LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
				LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
				LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
				LEFT JOIN Line line on departmentLine.LineId = line.LineId

				WHERE employee.IsActive = 1
				AND ((employee.[Status] = 1) OR 
					((employee.[Status] = 2) AND (employee.QuitDate >= @FromDate) AND (employee.QuitDate <= @ToDate)) OR
					((employee.[Status] = 2) AND (employee.QuitDate > @ToDate) AND (employee.QuitDate <= DATEADD(DAY, 30, @ToDate)))) 
				AND company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds)
				AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
				AND branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
				AND branchUnitDepartment.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
				AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
				AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
				AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
				AND ((employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId IS NULL))	
				AND CAST(employee.JoiningDate AS DATE) <=  CAST(@ToDate AS DATE)	
				AND employeeType.Id <> 1
				ORDER BY EmployeeCardId ASC		
			END

			COMMIT TRAN

			IF (@@ERROR <> 0)
				SELECT 0;
			ELSE
				SELECT 1;

				
END





