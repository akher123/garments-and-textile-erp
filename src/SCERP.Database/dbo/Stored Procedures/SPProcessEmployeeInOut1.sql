
-- ==========================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-12>						*** Running Time : 12:18:13 ***

-- Single Employee : <>   EXEC [SPProcessEmployeeInOut1] 1, NULL, NULL, NULL, NULL, NULL, '2016-09-26','2016-10-25', 0, 0, NULL, '1099','superadmin', '03'

-- Full Company:	 <>   EXEC [SPProcessEmployeeInOut1] 1, 1, 1, NULL, NULL, NULL, '2017-01-07','2017-01-07', 0, 0, NULL, '','superadmin', '05'

-- 33 employee :     <>   EXEC [SPProcessEmployeeInOut1] 1, 1, 2, NULL, NULL, NULL, '2016-09-26','2016-10-25', 0, 0, NULL, '','superadmin' , '11'

-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeInOut1]
	   @CompanyId INT = NULL,
	   @BranchId INT = NULL,
	   @BranchUnitId INT = NULL,
	   @BranchUnitDepartmentId INT = NULL, 
	   @DepartmentSectionId INT = NULL, 
	   @DepartmentLineId INT = NULL, 
	   @FromDate DATETIME = NULL,
	   @ToDate DATETIME = NULL,
	   @NoOfFromDaysBack TINYINT = 0,
	   @NoOfToDaysBack TINYINT = 0,
	   @EmployeeTypeId INT = NULL,
	   @EmployeeCardId NVARCHAR(100) = '',
	   @UserName NVARCHAR(100),
	   @UT NVARCHAR(2) ='01'
AS
BEGIN
	
		
		SET XACT_ABORT ON;
		SET NOCOUNT ON;
		 	
		BEGIN TRAN

		
		IF(@FromDate IS NULL)
		BEGIN
			SET @FromDate = CAST(CURRENT_TIMESTAMP AS DATE)
		END
		ELSE
		BEGIN
			SET @FromDate = CAST(@FromDate AS DATE)
		END
		
		IF(@ToDate IS NULL)
		BEGIN
			SET @ToDate = CAST(CURRENT_TIMESTAMP AS DATE)
		END
		ELSE
		BEGIN
			SET @ToDate = CAST(@ToDate AS DATE)
		END

		SET @FromDate = DATEADD(DAY, -@NoOfFromDaysBack, @FromDate);

		SET @ToDate = DATEADD(DAY, -@NoOfToDaysBack, @ToDate)


		IF (@EmployeeCardId  = '')
		BEGIN
			SET @EmployeeCardId = NULL
		END

		
		DECLARE @UserID UNIQUEIDENTIFIER;
		SELECT @UserID = EmployeeID FROM [User] WHERE UserName = @UserName;

		DECLARE	@Days INT;

		DECLARE	@msg NVARCHAR(30) ='FAILED';

		if (@UT ='01')
		BEGIN
		
		Truncate Table tmpEmployee ; 
		INSERT INTO [dbo].[tmpEmployee]
					([CompanyId]
					,[CompanyName]
					,[CompanyAddress]
					,[BranchId]
					,[BranchName]
					,[BranchUnitId]
					,[UnitName]
					,[BranchUnitDepartmentId]
					,[DepartmentName]
					,[DepartmentSectionId]
					,[SectionName]
					,[DepartmentLineId]
					,[LineName]
					,[EmployeeId]
					,[EmployeeCardId]
					,[EmployeeName]
					,[EmployeeTypeId]
					,[EmployeeType]
					,[EmployeeGradeId]
					,[EmployeeGrade]
					,[EmployeeDesignationId]
					,[EmployeeDesignation]
				    ,[JoiningDate]
					,[QuitDate]
					,[MobileNo]
					,[EmployeeWorkShiftId]
					,[BranchUnitWorkShiftId]
					,[WorkShiftName]
					,[ShiftDate]
					)
		SELECT		     
		 company.Id
		,company.Name
		,company.FullAddress
		,branch.Id
		,branch.Name
		,branchUnit.BranchUnitId
		,unit.Name
		,branchUnitDepartment.BranchUnitDepartmentId
		,department.Name
		,departmentSection.DepartmentSectionId
		,section.Name
		,departmentLine.DepartmentLineId
		,line.Name
		,employee.EmployeeId
		,employee.EmployeeCardId
		,employee.Name 
		,employeeType.Id
		,employeeType.Title
		,employeeGrade.Id
		,employeeGrade.Name
		,employeeDesignation.Id
		,employeeDesignation.Title
		,employee.JoiningDate
		,employee.QuitDate
		,presentAddress.MobilePhone
		,ews.EmployeeWorkShiftId
		,buws.BranchUnitWorkShiftId
		,ws.Name
		,ews.ShiftDate			
		FROM					
		Employee AS  employee
						
		LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,IsEligibleForOvertime,
		ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
		FROM EmployeeCompanyInfo AS employeeCompanyInfo 
		WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
		ON employee.EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1 

		LEFT JOIN EmployeePresentAddress presentAddress ON Employee.EmployeeId = presentAddress.EmployeeId AND presentAddress.IsActive = 1 									
		LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id
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
		LEFT JOIN EmployeeWorkShift ews ON ews.EmployeeId = employee.EmployeeId
		INNER JOIN BranchUnitWorkShift buws on ews.BranchUnitWorkShiftId = buws.BranchUnitWorkShiftId
		INNER JOIN WorkShift ws ON buws.WorkShiftId = ws.WorkShiftId

		WHERE (employee.IsActive = 1
		AND ((employee.[Status] = 1) OR 
		    ((employee.[Status] = 2) AND (employee.QuitDate >= @FromDate) AND (employee.QuitDate <= (DATEADD(DAY, 30, @ToDate)))))
		AND (company.Id = @CompanyId OR @CompanyId IS NULL)
		AND (branch.Id = @BranchId OR @BranchId IS NULL)
		AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId IS NULL)
		AND (branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId IS NULL)
		AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
		AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
		AND (employeeType.Id = @EmployeeTypeId OR @EmployeeTypeId IS NULL)
		AND (employee.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId IS NULL)		
		AND (CAST(employee.JoiningDate AS DATE) <= CAST(@ToDate AS DATE))	
		AND ews.ShiftDate BETWEEN @FromDate AND @ToDate
		AND employeeType.Id <> 1
		AND ews.[Status] = 1
		AND ews.IsActive = 1
		AND buws.IsActive = 1
		AND buws.[Status] = 1
		)
		
		
		ORDER BY ShiftDate ASC	;

		--SET @msg ='PROCESS 01 IS COMPLETED.';
		END

		if (@UT ='02') 
		BEGIN


		
		
		UPDATE tmpEmployee  SET [ShiftInTime] = isnull(( SELECT  ws.StartTime																											
					FROM WorkShift AS ws							
					INNER JOIN BranchUnitWorkShift AS buws ON buws.WorkShiftId = ws.WorkShiftId
					INNER JOIN EmployeeWorkShift AS ews ON ews.BranchUnitWorkShiftId = buws.BranchUnitWorkShiftId				
					INNER JOIN Employee AS emp ON emp.EmployeeId = ews.EmployeeId
					WHERE ((CAST(ews.ShiftDate AS DATE) =  CAST(tmpEmployee.ShiftDate AS DATE)) 
						AND (ews.EmployeeId = tmpEmployee.EmployeeId)
						AND ews.EmployeeWorkShiftId = tmpEmployee.EmployeeWorkShiftId
						AND ws.IsActive = 1	
						AND buws.IsActive = 1
						AND buws.[Status] = 1
						AND ews.IsActive = 1
						AND ews.[Status] = 1
						AND emp.IsActive = 1) 
						), [OutTime]) ;
		
			UPDATE tmpEmployee  SET [ShiftOutTime] = isnull(( SELECT  ws.EndTime																											
				FROM WorkShift AS ws							
				INNER JOIN BranchUnitWorkShift AS buws ON buws.WorkShiftId = ws.WorkShiftId
				INNER JOIN EmployeeWorkShift AS ews ON ews.BranchUnitWorkShiftId = buws.BranchUnitWorkShiftId				
				INNER JOIN Employee AS emp ON emp.EmployeeId = ews.EmployeeId
				WHERE ((CAST(ews.ShiftDate AS DATE) =  CAST(tmpEmployee.ShiftDate AS DATE)) 
					AND (ews.EmployeeId = tmpEmployee.EmployeeId)
					AND ews.EmployeeWorkShiftId = tmpEmployee.EmployeeWorkShiftId
					AND ws.IsActive = 1	
					AND buws.IsActive = 1
					AND buws.[Status] = 1
					AND ews.IsActive = 1
					AND ews.[Status] = 1
					AND emp.IsActive = 1) 
					), [OutTime]) ;

		--UPDATE tmpEmployee  SET [ShiftInTime] = [InTime] where [InTime] > [ShiftInTime] and [InTime] < [OutTime] ;

		Update tmpEmployee set [InTime] = dbo.fnGetEmployeeInTime(EmployeeId, ShiftDate, EmployeeWorkShiftId, JoiningDate, QuitDate) ;
		--Update tmpEmployee set [InTime] = ( SELECT TOP 1 CAST(EmployeeDailyAttendance.TransactionDateTime AS time) FROM EmployeeDailyAttendance	
		--	WHERE EmployeeDailyAttendance.EmployeeId = tmpEmployee.EmployeeId AND EmployeeDailyAttendance.IsActive = 1
		--	AND CAST(EmployeeDailyAttendance.TransactionDateTime AS DATE) = tmpEmployee.ShiftDate
		--	AND	CAST(EmployeeDailyAttendance.TransactionDateTime AS time) >= tmpEmployee.ShiftInTime
		--	ORDER BY EmployeeDailyAttendance.TransactionDateTime )


		--SET @msg ='PROCESS 02 IS COMPLETED.';
		END

		if (@UT ='03') 

		BEGIN


		UPDATE tmpEmployee SET [OutTime] = dbo.fnGetEmployeeOutTime(EmployeeId, ShiftDate, EmployeeWorkShiftId, JoiningDate, QuitDate) ;
		
		--UPDATE tmpEmployee SET [OutTime] =( SELECT TOP(1) CAST( EmployeeDailyAttendance.TransactionDateTime AS time) FROM EmployeeDailyAttendance
		--		WHERE EmployeeDailyAttendance.EmployeeId = tmpEmployee.EmployeeId AND EmployeeDailyAttendance.IsActive = 1
		--		AND CAST(EmployeeDailyAttendance.TransactionDateTime AS DATE) = CAST(DATEADD(DAY, 1, tmpEmployee.ShiftDate) AS DATE)
		--		AND	CAST( EmployeeDailyAttendance.TransactionDateTime AS time) <= '06:00:00'
		--		ORDER BY EmployeeDailyAttendance.TransactionDateTime DESC ) ;

		--UPDATE tmpEmployee SET [OutTime] = ( SELECT TOP(1) CAST( EmployeeDailyAttendance.TransactionDateTime AS time) FROM EmployeeDailyAttendance
		--			WHERE EmployeeDailyAttendance.EmployeeId = tmpEmployee.EmployeeId AND EmployeeDailyAttendance.IsActive = 1
		--			AND CAST(EmployeeDailyAttendance.TransactionDateTime AS DATE) = tmpEmployee.ShiftDate
		--			AND	CAST( EmployeeDailyAttendance.TransactionDateTime AS time) >= tmpEmployee.ShiftInTime 
		--			--AND	CAST( EmployeeDailyAttendance.TransactionDateTime AS time) <= tmpEmployee.ShiftOutTime
		--			ORDER BY EmployeeDailyAttendance.TransactionDateTime DESC ) where [OutTime] is null;

		--UPDATE tmpEmployee SET [OutTime] = NULL where [InTime] = [OutTime];

		----UPDATE tmpEmployee SET [LastDayOutTime] = dbo.fnGetEmployeeLastDayOutTime(EmployeeId, ShiftDate) ;
		UPDATE tmpEmployee set [LastDayOutTime] = ( SELECT TOP (1) [LastDayOutTime] FROM EmployeeInOut WHERE  TransactionDate = DATEADD(DAY, - 1, tmpEmployee.ShiftDate) AND (EmployeeId = tmpEmployee.EmployeeId) ) ;
		
		SET @msg ='PROCESS 03 IS COMPLETED.';
		END
		
		if (@UT ='04') 
				BEGIN
		--UPDATE tmpEmployee SET  [LateInMinutes] = dbo.fnGetEmployeeLateTime(EmployeeId, ShiftDate, EmployeeWorkShiftId, JoiningDate, QuitDate) ;

		UPDATE tmpEmployee SET [LateInMinutes]=0 ;
		UPDATE tmpEmployee SET [LateInMinutes] = isnull(( 		SELECT 
			CASE
				WHEN  DATEDIFF(minute,  DATEADD(minute, ws.InBufferTime , ws.StartTime), tmpEmployee.InTime) <= 0 
				THEN 0
				ELSE  DATEDIFF(minute,  DATEADD(minute, ws.InBufferTime , ws.StartTime), tmpEmployee.InTime)
			END		
			FROM WorkShift AS ws							
			INNER JOIN BranchUnitWorkShift AS buws ON buws.WorkShiftId = ws.WorkShiftId
			INNER JOIN EmployeeWorkShift AS ews ON ews.BranchUnitWorkShiftId = buws.BranchUnitWorkShiftId			
			INNER JOIN Employee AS emp ON emp.EmployeeId = ews.EmployeeId
			WHERE ((CAST(ews.ShiftDate AS DATE) =  CAST(tmpEmployee.ShiftDate AS DATE)) 
				AND (ews.EmployeeId = tmpEmployee.EmployeeId)
				AND ews.EmployeeWorkShiftId =tmpEmployee.EmployeeWorkShiftId
				AND ws.IsActive = 1	
				AND buws.IsActive = 1
				AND buws.[Status] = 1
				AND ews.IsActive = 1
				AND ews.[Status] = 1
				AND emp.IsActive = 1)  ),0) ;

			UPDATE tmpEmployee SET [LateInMinutes]=0 where JoiningDate > ShiftDate ;
			UPDATE tmpEmployee SET [LateInMinutes]=0 where  (QuitDate IS NOT NULL AND (CAST(QuitDate AS DATE) < ShiftDate)) ;
			UPDATE tmpEmployee SET [LateInMinutes]=0 where (ShiftDate > CAST(CURRENT_TIMESTAMP AS DATE)) ;
			

		--SET @msg ='PROCESS 05 IS COMPLETED.';
		END


		BEGIN
		UPDATE tmpEmployee SET  [Status] = dbo.fnGetEmployeeAttendanceStatusN(EmployeeId, ShiftDate, EmployeeWorkShiftId, BranchUnitId, JoiningDate, QuitDate)	;
		SET @msg ='PROCESS 04 IS COMPLETED.';
		END	

		if (@UT ='05') 


			

		--if (@UT ='06') 
		BEGIN
		UPDATE tmpEmployee SET  [TotalContinuousAbsentDays] =0;
		UPDATE tmpEmployee SET  [TotalContinuousAbsentDays] = dbo.fnGetTotalContinuousAbsentDays(EmployeeId, ShiftDate, 0,EmployeeWorkShiftId) where Status ='ABSENT'	;
		
		--UPDATE tmpEmployee SET  [TotalContinuousAbsentDays] =0;
		--UPDATE tmpEmployee SET [LPDate]= isnull(( Select top 1 TransactionDate From EmployeeInOut where EmployeeId = tmpEmployee.EmployeeId and TransactionDate <= tmpEmployee.ShiftDate and  Status = 'PRESENT' order by TransactionDate Desc ), JoiningDate) where Status ='ABSENT' ;
		--UPDATE tmpEmployee SET  [TotalContinuousAbsentDays] = DATEDIFF(DAY, LPDate, ShiftDate)  where Status ='ABSENT' ;
		
		--UPDATE tmpEmployee SET  [TotalContinuousAbsentDays] = [TotalContinuousAbsentDays] - (Select Count(*) From EmployeeInOut where EmployeeId = tmpEmployee.EmployeeId and TransactionDate <= tmpEmployee.ShiftDate  and TransactionDate > tmpEmployee.LPDate  and  Status <> 'ABSENT')  where Status ='ABSENT' ;
		--UPDATE tmpEmployee SET  [TotalContinuousAbsentDays] =0 where [TotalContinuousAbsentDays] < 0 ;

		--UPDATE tmpEmployee SET  [TotalContinuousAbsentDays] = 10 where [TotalContinuousAbsentDays] > 10 ;
		
		SET @msg ='PROCESS 06 IS COMPLETED.';
		END

		--if (@UT ='07') 
		BEGIN
		--UPDATE tmpEmployee  SET  [OTHours] = dbo.fnGetOTHoursN(EmployeeId, ShiftDate, EmployeeWorkShiftId, BranchUnitId, JoiningDate, QuitDate)	;
		UPDATE tmpEmployee  SET  [OTHours] = 0;

		--UPDATE tmpEmployee  SET [ShiftOutTime] = isnull(( SELECT  ws.EndTime																											
		--			FROM WorkShift AS ws							
		--			INNER JOIN BranchUnitWorkShift AS buws ON buws.WorkShiftId = ws.WorkShiftId
		--			INNER JOIN EmployeeWorkShift AS ews ON ews.BranchUnitWorkShiftId = buws.BranchUnitWorkShiftId				
		--			INNER JOIN Employee AS emp ON emp.EmployeeId = ews.EmployeeId
		--			WHERE ((CAST(ews.ShiftDate AS DATE) =  CAST(tmpEmployee.ShiftDate AS DATE)) 
		--				AND (ews.EmployeeId = tmpEmployee.EmployeeId)
		--				AND ews.EmployeeWorkShiftId = tmpEmployee.EmployeeWorkShiftId
		--				AND ws.IsActive = 1	
		--				AND buws.IsActive = 1
		--				AND buws.[Status] = 1
		--				AND ews.IsActive = 1
		--				AND ews.[Status] = 1
		--				AND emp.IsActive = 1) 
		--				), [OutTime]) ;

		UPDATE tmpEmployee Set [OTMinutes] = DATEDIFF(MINUTE, ShiftOutTime, OutTime);
		UPDATE tmpEmployee Set [OTMinutes] = [OTMinutes]+1440  WHERE  ({ fn HOUR(OutTime) } <= 3);
		UPDATE tmpEmployee Set [OTMinutes] = 0 where ( [OTMinutes] is null ) or [OTMinutes] < 0;
		UPDATE tmpEmployee Set [OTMinutes] = 120 where [OTMinutes] > 120;
		UPDATE tmpEmployee  SET  [OTHours] = floor([OTMinutes]/60) ;
		UPDATE tmpEmployee Set [OTMinutes] = [OTMinutes] - ([OTHours]*60) ;
		UPDATE tmpEmployee  SET  [OTHours]=[OTHours]+0.5 where [OTMinutes] >= 25 and [OTMinutes] <50 ;
		UPDATE tmpEmployee  SET  [OTHours]=[OTHours]+1 where [OTMinutes] >= 50  ;

		UPDATE tmpEmployee  SET  [EOTHours] = isnull((SELECT  TOP (1) OvertimeHour FROM OvertimeEligibleEmployee WHERE OvertimeDate = tmpEmployee.ShiftDate AND (IsActive = 1) AND EmployeeId = tmpEmployee.EmployeeId ),0) ;
		UPDATE tmpEmployee  SET  [OTHours] = [EOTHours] where [OTHours] > [EOTHours] ;
		UPDATE tmpEmployee  SET  [IsEligibleForOvertime] =  isnull((SELECT top 1 IsEligibleForOvertime FROM EmployeeCompanyInfo where EmployeeId= tmpEmployee.EmployeeId AND (CAST(FromDate AS Date) <= tmpEmployee.ShiftDate) AND (employeeCompanyInfo.IsActive=1) ORDER BY FromDate DESC),0) ;
		UPDATE tmpEmployee  SET  [OTHours] = 0 where [IsEligibleForOvertime]= 0 ;
		UPDATE tmpEmployee  SET  [OTHours] = 0 where [OTHours]= .5  -- Newly Added


		UPDATE tmpEmployee Set [HD] ='N', [WD]='N' ;
		UPDATE tmpEmployee Set [WD]='Y' where DATENAME(weekday, ShiftDate) = 'Friday' ; --- There are Some Exception day
		UPDATE tmpEmployee Set [HD]= isnull(( select top 1 'Y' From HolidaysSetup where tmpEmployee.ShiftDate  BETWEEN startdate AND enddate),'N') ; --- There are Some Exception day
		UPDATE tmpEmployee  SET  [OTHours] = 0 where [HD]='Y' or [WD] ='Y';

		UPDATE tmpEmployee  SET  [OTHours] = 0 where JoiningDate > ShiftDate ;
		UPDATE tmpEmployee  SET  [OTHours] = 0 where ( QuitDate is not null) AND QuitDate < ShiftDate ;
		UPDATE tmpEmployee  SET  [OTHours] = 0 where ShiftDate >  CAST(CURRENT_TIMESTAMP AS DATE) ;
			
						
		--UPDATE tmpEmployee SET  [LastDayOTHours] = dbo.fnGetLastDayOTHours(EmployeeId, ShiftDate)	;
		UPDATE tmpEmployee set LastDayOTHours = isnull(( SELECT     TOP (1) OTHours FROM EmployeeInOut WHERE  TransactionDate = DATEADD(DAY, - 1, tmpEmployee.ShiftDate) AND (EmployeeId = tmpEmployee.EmployeeId) ),0) ;
		SET @msg ='PROCESS 07 IS COMPLETED.';
		END

		--if (@UT ='08') 
		BEGIN
		--UPDATE tmpEmployee SET  [ExtraOTHours] = dbo.fnGetExtraOTHours(EmployeeId, ShiftDate, EmployeeWorkShiftId, BranchUnitId, JoiningDate, QuitDate) ;	
		
		UPDATE tmpEmployee  SET  [ExtraOTHours] = 0;
		UPDATE tmpEmployee Set [OTMinutes] = DATEDIFF(MINUTE, ShiftOutTime, OutTime);
		UPDATE tmpEmployee Set [OTMinutes] = [OTMinutes]+1440  WHERE  ({ fn HOUR(OutTime) } <= 3);
		UPDATE tmpEmployee Set [OTMinutes] = 0 where ( [OTMinutes] is null ) or [OTMinutes] < 0;
		--UPDATE tmpEmployee Set [OTMinutes] = 120 where [OTMinutes] > 120;
		UPDATE tmpEmployee  SET  [ExtraOTHours] = floor([OTMinutes]/60) ;
		UPDATE tmpEmployee Set [OTMinutes] = [OTMinutes] - ([ExtraOTHours]*60) ;
		UPDATE tmpEmployee  SET  [ExtraOTHours]=[ExtraOTHours]+0.5 where [OTMinutes] >= 25 and [OTMinutes] <50 ;
		UPDATE tmpEmployee  SET  [ExtraOTHours]=[ExtraOTHours]+1 where [OTMinutes] >= 50  ;
		UPDATE tmpEmployee  SET  [ExtraOTHours]=[ExtraOTHours]-[OTHours] ;
		UPDATE tmpEmployee  SET  [ExtraOTHours]=0 where [ExtraOTHours] < 0 ;
		UPDATE tmpEmployee  SET  [ExtraOTHours]=0 where [OTHours] =0 ;


		----UPDATE tmpEmployee SET  [LastDayExtraOTHours] = dbo.fnGetLastDayExtraOTHours(EmployeeId, ShiftDate)	;
		UPDATE tmpEmployee set [LastDayExtraOTHours] = isnull(( SELECT     TOP (1) ExtraOTHours FROM EmployeeInOut WHERE  TransactionDate = DATEADD(DAY, - 1, tmpEmployee.ShiftDate) AND (EmployeeId = tmpEmployee.EmployeeId) ),0) ;
		SET @msg ='PROCESS 08 IS COMPLETED.';
		END

		--if (@UT ='09') 
		BEGIN
		--UPDATE tmpEmployee SET  [WeekendOTHours] = dbo.fnGetWeekendOTHours(EmployeeId, ShiftDate, EmployeeWorkShiftId, BranchUnitId, JoiningDate, QuitDate)	;
		--UPDATE tmpEmployee  SET [ShiftInTime] = isnull(( SELECT  ws.StartTime																											
		--			FROM WorkShift AS ws							
		--			INNER JOIN BranchUnitWorkShift AS buws ON buws.WorkShiftId = ws.WorkShiftId
		--			INNER JOIN EmployeeWorkShift AS ews ON ews.BranchUnitWorkShiftId = buws.BranchUnitWorkShiftId				
		--			INNER JOIN Employee AS emp ON emp.EmployeeId = ews.EmployeeId
		--			WHERE ((CAST(ews.ShiftDate AS DATE) =  CAST(tmpEmployee.ShiftDate AS DATE)) 
		--				AND (ews.EmployeeId = tmpEmployee.EmployeeId)
		--				AND ews.EmployeeWorkShiftId = tmpEmployee.EmployeeWorkShiftId
		--				AND ws.IsActive = 1	
		--				AND buws.IsActive = 1
		--				AND buws.[Status] = 1
		--				AND ews.IsActive = 1
		--				AND ews.[Status] = 1
		--				AND emp.IsActive = 1) 
		--				), [OutTime]) ;
		
		
		--UPDATE tmpEmployee  SET [ShiftInTime] = [InTime] where [InTime] > [ShiftInTime] and [InTime] < [OutTime] ;

		--Update tmpEmployee set [InTime] = ( SELECT TOP 1 CAST(EmployeeDailyAttendance.TransactionDateTime AS time) FROM EmployeeDailyAttendance	
		--	WHERE EmployeeDailyAttendance.EmployeeId = tmpEmployee.EmployeeId AND EmployeeDailyAttendance.IsActive = 1
		--	AND CAST(EmployeeDailyAttendance.TransactionDateTime AS DATE) = tmpEmployee.ShiftDate
		--	AND	CAST(EmployeeDailyAttendance.TransactionDateTime AS time) >= tmpEmployee.ShiftInTime
		--	ORDER BY EmployeeDailyAttendance.TransactionDateTime )


		UPDATE tmpEmployee SET  [WeekendOTHours] =0;

		UPDATE tmpEmployee Set [OTMinutes] = DATEDIFF(MINUTE, ShiftInTime, OutTime) where [WD]='Y';

		UPDATE tmpEmployee  SET  [WeekendOTHours] = floor([OTMinutes]/60)  where [WD]='Y' ;
		UPDATE tmpEmployee Set [OTMinutes] = [OTMinutes] - ([WeekendOTHours]*60)  where [WD]='Y' ;
		UPDATE tmpEmployee  SET  [WeekendOTHours]=[WeekendOTHours]+0.5 where [OTMinutes] >= 25 and [OTMinutes] <50  and [WD]='Y';
		UPDATE tmpEmployee  SET  [WeekendOTHours]=[WeekendOTHours]+1 where [OTMinutes] >= 50   and  [WD]='Y';
		 
		UPDATE tmpEmployee  SET  [WeekendOTHours]=[WeekendOTHours]-1  where ( [ShiftInTime] <= CAST('14:00:00' AS TIME(7))) AND ( [OutTime] >= CAST('13:50:00' AS TIME(7))) AND  [WD]='Y' ; -- Lunch Break
		 
		UPDATE tmpEmployee  SET  [WeekendOTHours]=0 where [WeekendOTHours] < 0  and [WD]='Y';
		UPDATE tmpEmployee  SET  [WeekendOTHours]=0 where [IsEligibleForOvertime] = 0  and [WD]='Y';
		
		
		SET @msg ='PROCESS 09 IS COMPLETED.';
		END

		--if (@UT ='10') 
		BEGIN
		--UPDATE tmpEmployee SET  [HolidayOTHours] = dbo.fnGetHolidayOTHours(EmployeeId, ShiftDate, EmployeeWorkShiftId, BranchUnitId, JoiningDate, QuitDate) ;	
		
		UPDATE tmpEmployee SET  [HolidayOTHours] =0;

		UPDATE tmpEmployee Set [OTMinutes] = DATEDIFF(MINUTE, ShiftInTime, OutTime) where [HD]='Y';

		UPDATE tmpEmployee  SET  [HolidayOTHours] = floor([OTMinutes]/60)  where [HD]='Y' ;
		UPDATE tmpEmployee Set [OTMinutes] = [OTMinutes] - ([HolidayOTHours]*60)  where [HD]='Y' ;
		UPDATE tmpEmployee  SET  [HolidayOTHours]=[HolidayOTHours]+0.5 where [OTMinutes] >= 25 and [OTMinutes] <50  and [HD]='Y';
		UPDATE tmpEmployee  SET  [HolidayOTHours]=[HolidayOTHours]+1 where [OTMinutes] >= 50   and  [HD]='Y';
		 
		UPDATE tmpEmployee  SET  [HolidayOTHours]=[HolidayOTHours]-1  where ( [ShiftInTime] <= CAST('14:00:00' AS TIME(7))) AND ( [OutTime] >= CAST('13:50:00' AS TIME(7))) AND  [HD]='Y' ; -- Lunch Break
		 
		UPDATE tmpEmployee  SET  [HolidayOTHours]=0 where [HolidayOTHours] < 0  and [HD]='Y';
		UPDATE tmpEmployee  SET  [HolidayOTHours]=0 where [IsEligibleForOvertime] = 0  and [HD]='Y';
		
		
		
		SET @msg ='PROCESS 10 IS COMPLETED.';
		END

		--if (@UT ='11') 
		BEGIN
		UPDATE tmpEmployee SET  [Remarks] = dbo.fnGetDayRemarksOfEmployee(EmployeeId, ShiftDate, EmployeeWorkShiftId, BranchUnitId, JoiningDate, QuitDate)	;

		
		DELETE FROM [dbo].[EmployeeInOut]
		WHERE           
		(
		(CompanyId = @CompanyId OR @CompanyId IS NULL)
		AND (BranchId = @BranchId OR @BranchId IS NULL)
		AND (BranchUnitId = @BranchUnitId OR @BranchUnitId IS NULL)
		AND (BranchUnitDepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId IS NULL)
		AND (DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
		AND (DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
		AND (EmployeeTypeId = @EmployeeTypeId OR @EmployeeTypeId IS NULL)
		AND (EmployeeCardId = @EmployeeCardId OR @EmployeeCardId IS NULL)		
		AND (TransactionDate >= CAST(@FromDate AS DATE))
		AND (TransactionDate <= CAST(@ToDate AS DATE))
		)

		SET FMTONLY OFF;

		--DECLARE  @imax INT, @i INT;	
		--SELECT @imax = COUNT(EmployeeId) FROM tmpEmployee

		--SET @i = 1
		
		--WHILE (@i <= @imax)
		--BEGIN	
		

		--	IF(@EmployeeWorkShiftIdTemp IS NOT NULL)
		--	BEGIN

				INSERT INTO [dbo].[EmployeeInOut]
					([CompanyId]
					,[CompanyName]
					,[CompanyAddress]
					,[BranchId]
					,[BranchName]
					,[BranchUnitId]
					,[UnitName]
					,[BranchUnitDepartmentId]
					,[DepartmentName]
					,[DepartmentSectionId]
					,[SectionName]
					,[DepartmentLineId]
					,[LineName]
					,[EmployeeId]
					,[EmployeeCardId]
					,[EmployeeName]
					,[EmployeeTypeId]
					,[EmployeeType]
					,[EmployeeGradeId]
					,[EmployeeGrade]
					,[EmployeeDesignationId]
					,[EmployeeDesignation]
					,[JoiningDate]
					,[QuitDate]
					,[MobileNo]
					,[BranchUnitWorkShiftId]
					,[EmployeeWorkShiftId]
					,[WorkShiftName]
					,[TransactionDate]
					,[InTime]
					,[LateInMinutes]
					,[OutTime]
					,[LastDayOutTime]
					,[Status]
					,[TotalContinuousAbsentDays]
					,[OTHours]
					,[LastDayOTHours]
					,[ExtraOTHours]
					,[LastDayExtraOTHours]
					,[WeekendOTHours]
					,[HolidayOTHours]
					,[Remarks]
					,[IsActive]
					)
				SELECT	
					  CompanyId, CompanyName, CompanyAddress, 
					  BranchId, BranchName, BranchUnitId, UnitName, 
					  BranchUnitDepartmentId, DepartmentName, 
                      DepartmentSectionId, SectionName, DepartmentLineId, LineName, 
					  EmployeeId, EmployeeCardId, EmployeeName, EmployeeTypeId, EmployeeType, 
                      EmployeeGradeId, EmployeeGrade, EmployeeDesignationId, EmployeeDesignation, 
					  JoiningDate, QuitDate, MobileNo, 
					  BranchUnitWorkShiftId, EmployeeWorkShiftId,  
                      WorkShiftName, ShiftDate 
					  ,[InTime]
					  ,[LateInMinutes]
					  ,[OutTime]
					  ,[LastDayOutTime]
					  ,[Status]
					  ,[TotalContinuousAbsentDays]
					  ,[OTHours]
					  ,[LastDayOTHours]
					  ,[ExtraOTHours]
					  ,[LastDayExtraOTHours]
					  ,[WeekendOTHours]
					  ,[HolidayOTHours]
					  ,[Remarks]
					  ,1
				FROM tmpEmployee;

				SET @msg ='PROCESS 11 IS COMPLETED.';
				END
				
		--		WHERE RowID = @i



	



		--		--UPDATE [EmployeeInOut]
		--		--SET  CreatedDate = CURRENT_TIMESTAMP,
		--		--     CreatedBy = @UserID
		--		--WHERE [Id] = @EmployeeInOutId

		--	END

		--	SET @i = @i + 1	
		--END

		COMMIT TRAN

		SET @msg ='PROCESS 05 IS COMPLETED.';
		--DECLARE @Result INT = 1;

		--IF (@@ERROR <> 0)
		--	SET @Result = 0;
		
		--SELECT @Result;
		SELECT @msg;
END






