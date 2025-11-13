
-- =============================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-11-18>
-- ============================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessBulkEmployeeInOutModel]
	   @FromDate DATETIME,
	   @ToDate DATETIME,
	   @EmployeeList AS dbo.EmployeeList READONLY,
	   @UserName NVARCHAR(100)
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

		
		DECLARE @UserID UNIQUEIDENTIFIER;
		SELECT @UserID = EmployeeID FROM [User] WHERE UserName = @UserName;


		DELETE FROM [dbo].[EmployeeInOutModel]
		WHERE           
		(
		(EmployeeId IN (SELECT EmployeeId FROM @EmployeeList))		
		AND (TransactionDate >= CAST(@FromDate AS DATE))
		AND (TransactionDate <= CAST(@ToDate AS DATE))
		)

		
		DECLARE  @EmployeeInfo  TABLE(
									 RowID	INT    IDENTITY ( 1 , 1 )
									,[CompanyId] INT
									,[CompanyName] NVARCHAR(100)
									,[CompanyAddress] NVARCHAR(MAX)
									,[BranchId] INT
									,[BranchName] NVARCHAR(100)
									,[BranchUnitId] INT
									,[UnitName] NVARCHAR(100)
									,[BranchUnitDepartmentId] INT
									,[DepartmentName] NVARCHAR(100)
									,[DepartmentSectionId] INT
									,[SectionName] NVARCHAR(100)
									,[DepartmentLineId] INT
									,[LineName] NVARCHAR(100)
									,[EmployeeId] UNIQUEIDENTIFIER
									,[EmployeeCardId] NVARCHAR(100)
									,[EmployeeName] NVARCHAR(100)
									,[EmployeeTypeId] INT
									,[EmployeeType] NVARCHAR(100)
									,[EmployeeGradeId] INT
									,[EmployeeGrade] NVARCHAR(100)
									,[EmployeeDesignationId] INT
									,[EmployeeDesignation] NVARCHAR(100)
									,[JoiningDate] DATETIME
									,[QuitDate] DATETIME NULL
									,[MobileNo] NVARCHAR(100)
									,[EmployeeWorkShiftId] INT
									,[BranchUnitWorkShiftId] INT
									,[WorkShiftName] NVARCHAR(100)
									,[ShiftDate] DATETIME
									);

		
		INSERT INTO @EmployeeInfo
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
		AND (CAST(employee.JoiningDate AS DATE) <= CAST(@ToDate AS DATE))	
		AND ews.ShiftDate BETWEEN @FromDate AND @ToDate
		AND employeeType.Id <> 1
		AND ews.[Status] = 1
		AND ews.IsActive = 1
		AND buws.IsActive = 1
		AND buws.[Status] = 1
		AND employee.EmployeeId IN (SELECT EmployeeId FROM @EmployeeList))		
		ORDER BY ShiftDate ASC	


		DECLARE  @imax INT, @i INT;	
		SELECT @imax = COUNT(EmployeeId) FROM @EmployeeInfo

		SET @i = 1
		
		WHILE (@i <= @imax)
		BEGIN	
			  DECLARE @CompanyIdTemp INT
					,@CompanyNameTemp NVARCHAR(100)
					,@CompanyAddresTemp NVARCHAR(MAX)
					,@BranchIdTemp INT
					,@BranchNameTemp NVARCHAR(100)
					,@BranchUnitIdTemp INT
					,@UnitNameTemp NVARCHAR(100)
					,@BranchUnitDepartmentIdTemp INT
					,@DepartmentNameTemp NVARCHAR(100)
					,@DepartmentSectionIdTemp INT
					,@SectionNameTemp NVARCHAR(100)
					,@DepartmentLineIdTemp INT
					,@LineNameTemp NVARCHAR(100)
					,@EmployeeIdTemp UNIQUEIDENTIFIER
					,@EmployeeCardIdTemp NVARCHAR(100)
					,@EmployeeNameTemp NVARCHAR(100)
					,@EmployeeTypeIdTemp INT
					,@EmployeeTypeTemp NVARCHAR(100)
					,@EmployeeGradeIdTemp INT
					,@EmployeeGradeTemp NVARCHAR(100)
					,@EmployeeDesignationIdTemp INT
					,@EmployeeDesignationTemp NVARCHAR(100)
					,@JoiningDateTemp DATETIME
					,@QuitDateTemp DATETIME 
					,@MobileNoTemp NVARCHAR(100)
					,@BranchUnitWorkShiftIdTemp INT
					,@EmployeeWorkShiftIdTemp INT
					,@WorkShiftNameTemp NVARCHAR(100)
					,@ShiftDateTemp DATETIME
						
			SELECT   @CompanyIdTemp = CompanyId
					,@CompanyNameTemp = CompanyName
					,@CompanyAddresTemp = CompanyAddress
					,@BranchIdTemp = BranchId
					,@BranchNameTemp = BranchName
					,@BranchUnitIdTemp = BranchUnitId
					,@UnitNameTemp = UnitName
					,@BranchUnitDepartmentIdTemp = BranchUnitDepartmentId
					,@DepartmentNameTemp = DepartmentName
					,@DepartmentSectionIdTemp = DepartmentSectionId
					,@SectionNameTemp = SectionName
					,@DepartmentLineIdTemp = DepartmentLineId
					,@LineNameTemp = LineName
					,@EmployeeIdTemp = EmployeeId
					,@EmployeeCardIdTemp = EmployeeCardId
					,@EmployeeNameTemp = EmployeeName
					,@EmployeeTypeIdTemp = EmployeeTypeId
					,@EmployeeTypeTemp = EmployeeType
					,@EmployeeGradeIdTemp = EmployeeGradeId
					,@EmployeeGradeTemp = EmployeeGrade
					,@EmployeeDesignationIdTemp = EmployeeDesignationId
					,@EmployeeDesignationTemp = EmployeeDesignation
					,@JoiningDateTemp = JoiningDate
					,@QuitDateTemp = QuitDate
					,@MobileNoTemp = MobileNo
					,@BranchUnitWorkShiftIdTemp = BranchUnitWorkShiftId
					,@EmployeeWorkShiftIdTemp = EmployeeWorkShiftId				
					,@WorkShiftNameTemp = WorkShiftName
					,@ShiftDateTemp = ShiftDate
			FROM @EmployeeInfo
			WHERE RowID = @i;

			IF(@EmployeeWorkShiftIdTemp IS NOT NULL)
			BEGIN

				INSERT INTO [dbo].[EmployeeInOutModel]
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
					,[IsActive]
					)
				SELECT	 @CompanyIdTemp 
						,@CompanyNameTemp 
						,@CompanyAddresTemp 
						,@BranchIdTemp 
						,@BranchNameTemp 
						,@BranchUnitIdTemp 
						,@UnitNameTemp 
						,@BranchUnitDepartmentIdTemp
						,@DepartmentNameTemp 
						,@DepartmentSectionIdTemp
						,@SectionNameTemp
						,@DepartmentLineIdTemp 
						,@LineNameTemp
						,@EmployeeIdTemp 
						,@EmployeeCardIdTemp 
						,@EmployeeNameTemp 
						,@EmployeeTypeIdTemp
						,@EmployeeTypeTemp 
						,@EmployeeGradeIdTemp 
						,@EmployeeGradeTemp
						,@EmployeeDesignationIdTemp 
						,@EmployeeDesignationTemp 
						,@JoiningDateTemp 
						,@QuitDateTemp 
						,@MobileNoTemp 
						,@BranchUnitWorkShiftIdTemp 
						,@EmployeeWorkShiftIdTemp 					
						,@WorkShiftNameTemp 
						,@ShiftDateTemp 
					    ,dbo.fnGetEmployeeInTime(@EmployeeIdTemp, @ShiftDateTemp, @EmployeeWorkShiftIdTemp, @JoiningDateTemp, @QuitDateTemp)
						,1
				FROM @EmployeeInfo
				WHERE RowID = @i

				DECLARE @EmployeeInOutId INT
				SELECT @EmployeeInOutId =  [Id] FROM EmployeeInOutModel
				WHERE EmployeeId = @EmployeeIdTemp
				AND TransactionDate = @ShiftDateTemp
				AND EmployeeWorkShiftId = @EmployeeWorkShiftIdTemp 

				UPDATE [EmployeeInOutModel]
				SET  [LateInMinutes] = dbo.fnGetEmployeeLateTime(@EmployeeIdTemp, @ShiftDateTemp, @EmployeeWorkShiftIdTemp, @JoiningDateTemp, @QuitDateTemp)
					,[OutTime] = dbo.fnGetEmployeeOutTimeModel(@EmployeeIdTemp, @ShiftDateTemp, @EmployeeWorkShiftIdTemp, @JoiningDateTemp, @QuitDateTemp)		
					,[LastDayOutTime] = dbo.fnGetEmployeeLastDayOutTimeModel(@EmployeeIdTemp, @ShiftDateTemp)					
				WHERE [Id] = @EmployeeInOutId

				UPDATE [EmployeeInOutModel]
				SET  [Status] = dbo.fnGetEmployeeAttendanceStatus(@EmployeeIdTemp, @ShiftDateTemp, @EmployeeWorkShiftIdTemp,@BranchUnitIdTemp, @JoiningDateTemp, @QuitDateTemp)	
				WHERE [Id] = @EmployeeInOutId

				UPDATE [EmployeeInOutModel]
				SET  [TotalContinuousAbsentDays] = dbo.fnGetTotalContinuousAbsentDays(@EmployeeIdTemp, @ShiftDateTemp, 0, @EmployeeWorkShiftIdTemp)	
				WHERE [Id] = @EmployeeInOutId

				UPDATE [EmployeeInOutModel]
				SET  [OTHours] = dbo.fnGetOTHours(@EmployeeIdTemp, @ShiftDateTemp, @EmployeeWorkShiftIdTemp,@BranchUnitIdTemp, @JoiningDateTemp, @QuitDateTemp)	
				WHERE [Id] = @EmployeeInOutId

				UPDATE [EmployeeInOutModel]
				SET  [LastDayOTHours] = dbo.fnGetLastDayOTHoursModel(@EmployeeIdTemp, @ShiftDateTemp)	
				WHERE [Id] = @EmployeeInOutId

				UPDATE [EmployeeInOutModel]
				SET  [Remarks] = dbo.fnGetDayRemarksOfEmployee(@EmployeeIdTemp, @ShiftDateTemp, @EmployeeWorkShiftIdTemp,@BranchUnitIdTemp, @JoiningDateTemp, @QuitDateTemp)	
				WHERE [Id] = @EmployeeInOutId

				UPDATE [EmployeeInOutModel]
				SET  CreatedDate = CURRENT_TIMESTAMP,
				     CreatedBy = @UserID
				WHERE [Id] = @EmployeeInOutId

			END

			SET @i = @i + 1	
		END
		
		COMMIT TRAN

	
		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;

END






