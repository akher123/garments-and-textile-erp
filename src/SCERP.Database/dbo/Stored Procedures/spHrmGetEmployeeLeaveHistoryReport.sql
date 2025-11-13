
-- ==========================================================================================================================================================================
-- Author:		Golam Rabbi
-- Create date: 2015.12.15
-- ==========================================================================================================================================================================

-- EXEC spHrmGetEmployeeLeaveHistoryReport -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,'1900-01-01','1900-01-01','1900-01-01','1900-01-01', '0835','',-1, -1, 2019, '2019-09-04', 'superadmin'

-- ==========================================================================================================================================================================

CREATE PROCEDURE [dbo].[spHrmGetEmployeeLeaveHistoryReport]
				@CompanyId						INT = -1,
				@BranchId						INT = -1,
				@BranchUnitId					INT = -1,		
				@BranchUnitDepartmentId			INT = -1,
				@DepartmentSectionId			INT = -1,
				@DepartmentLineId				INT = -1,
				@EmployeeTypeId					INT = -1,
				@EmployeeGradeId				INT = -1,
				@EmployeeDesignationId			INT = -1,
				@GenderId						INT = -1,	
				@JoiningDateBegin				DATETIME = '1900-01-01',
				@JoiningDateEnd					DATETIME = '1900-01-01',
				@QuitDateBegin					DATETIME = '1900-01-01',
				@QuitDateEnd					DATETIME = '1900-01-01',	
				@EmployeeCardId					NVARCHAR(100) = '',
				@EmployeeName					NVARCHAR(100) = '',
				@LeaveTypeId					INT = -1,
				@ActiveStatus					INT = -1,		
				@Year							INT = -1,
				@UpToDate					    DATETIME = '1900-01-01',
				@UserName						NVARCHAR(100) = ''	
				
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRAN

			DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
			DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
			DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
			DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);
			DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);

			DECLARE @CompanyName NVARCHAR(100) = ''; 
			DECLARE @CompanyAddress NVARCHAR(100) = ''; 
			DECLARE @BranchName NVARCHAR(100) = ''; 
			DECLARE @UnitName NVARCHAR(100) = ''; 
			DECLARE @DepartmentName NVARCHAR(100) = ''; 
			DECLARE @SectionName NVARCHAR(100) = '';
			DECLARE @LineName NVARCHAR(100) = '';
			DECLARE @EmployeeTypeName NVARCHAR(100) = '';

			-- Insert statements for procedure here
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
			   INSERT INTO @ListOfCompanyIds VALUES (@CompanyID)
			   
			   SELECT @CompanyName= comp.Name, 
					  @CompanyAddress = comp.FullAddress
			   FROM Company comp 
			   WHERE comp.Id = @companyID
			END

			IF(@BranchID = -1)
			BEGIN
			   INSERT INTO @ListOfBranchIds
			   SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
			   WHERE UserName = @UserName;

			   SET @BranchName = '';
			END  
			ELSE
			BEGIN
			   INSERT INTO @ListOfBranchIds VALUES (@BranchID)
			   
			   SELECT @BranchName= brnch.Name
			   FROM Branch brnch 
			   WHERE brnch.Id = @branchId
			END

			IF(@BranchUnitId=-1)
			BEGIN
			   INSERT INTO @ListOfBranchUnitIds
			   SELECT DISTINCT BranchUnitId FROM UserPermissionForDepartmentLevel
			   WHERE UserName = @UserName;

			   SET @UnitName = '';
			END  
			ELSE
			BEGIN
			   INSERT INTO @ListOfBranchUnitIds VALUES (@BranchUnitId)
			   
			   SELECT TOP(1) @UnitName= unit.Name
			   FROM Unit unit 
			   INNER JOIN BranchUnit bunit ON unit.UnitId = bunit.UnitId
			   WHERE bunit.BranchUnitId = @branchUnitId
			END

			IF(@BranchUnitDepartmentId =-1)
			BEGIN
			   INSERT INTO @ListOfBranchUnitDepartmentIds
			   SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
			   WHERE UserName = @UserName;

			   SET @DepartmentName = '';
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

			IF(@DepartmentSectionId  = -1)
			BEGIN
				SET @SectionName = ''			  
			END
			ELSE
			BEGIN
					Select @SectionName = sec.Name FROM Section sec
									INNER JOIN DepartmentSection dsec
									ON sec.SectionId = dsec.SectionId
									WHERE dsec.DepartmentSectionId = @DepartmentSectionId
			END
				
			IF(@DepartmentLineId = -1)
			BEGIN
				SET @LineName = ''				  
			END
			ELSE
			BEGIN
					Select @LineName = line.Name FROM Line line
									INNER JOIN DepartmentLine dline
									ON line.LineId = dline.LineId
									WHERE dline.DepartmentLineId = @DepartmentLineId
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

			IF(CAST(@JoiningDateBegin AS DATE) = '1900-01-01')
				SET @JoiningDateBegin = NULL;

			IF(CAST(@JoiningDateEnd AS DATE) = '1900-01-01')
				SET @JoiningDateEnd =NULL;

			IF(CAST(@QuitDateBegin AS DATE) = '1900-01-01')
				SET @QuitDateBegin = NULL;

			IF(CAST(@QuitDateEnd AS DATE) = '1900-01-01')
				SET @QuitDateEnd =NULL;

			IF(CAST(@UpToDate AS DATE) = '1900-01-01')
				SET @UpToDate =NULL;	
			
			IF(@Year<> - 1)
			BEGIN
				DECLARE  @EmployeeLeaveInfo  TABLE(
											 RowID	INT    IDENTITY ( 1 , 1 ),
											 EmployeeId UNIQUEIDENTIFIER,
											 EmployeeCardId NVARCHAR(100),
											 BranchUnitId INT,
											 EmployeeTypeId INT);

				INSERT INTO @EmployeeLeaveInfo
				SELECT DISTINCT employee.EmployeeId, employee.EmployeeCardId, branchUnit.BranchUnitId, employeeType.Id  
				FROM
				Employee employee 
				LEFT JOIN
				(SELECT EmployeeId, PunchCardNo, FromDate,DesignationId,BranchUnitDepartmentId,DepartmentSectionId,DepartmentLineId, 
				 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
				FROM EmployeeCompanyInfo AS employeeCompanyInfo
				WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @UpToDate) OR (@UpToDate IS NULL))
				AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
				ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1  
	
				LEFT JOIN EmployeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id AND employeeDesignation.IsActive = 1 
				LEFT JOIN EmployeeType ON employeeDesignation.EmployeeTypeId = employeeType.Id AND employeeType.IsActive = 1 
				LEFT JOIN EmployeeGrade ON employeeDesignation.GradeId = employeeGrade.Id AND employeeGrade.IsActive = 1  
				LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId 
				LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
				LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
				LEFT JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
				LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
				LEFT JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
				LEFT JOIN Company  AS company ON branch.CompanyId = company.Id
				LEFT JOIN DepartmentSection departmentSection ON employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId 
				LEFT JOIN Section section ON departmentSection.SectionId = section.SectionId
				LEFT JOIN DepartmentLine departmentLine ON employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
				LEFT JOIN Line line ON departmentLine.LineId = line.LineId 
				LEFT JOIN Gender ON Gender.GenderId = Employee.GenderId AND Gender.IsActive = 1 

				WHERE
				(company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds)
				AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
				AND branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
				AND branchUnitDepartment.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
				AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId = -1)
				AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId = -1)			 							
				AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
				AND ((employeeGrade.Id = @EmployeeGradeID) OR (@EmployeeGradeID = -1))
				AND ((employeeDesignation.Id = @EmployeeDesignationID) OR (@EmployeeDesignationID = -1))
				AND (Employee.GenderId = @GenderId OR @GenderId =-1)
				AND ((CAST(Employee.JoiningDate AS date) >= CAST(@JoiningDateBegin AS date)) OR (@JoiningDateBegin IS NULL))
				AND ((CAST(Employee.JoiningDate AS date) <= CAST(@JoiningDateEnd AS date)) OR (@JoiningDateEnd IS NULL))
				AND ((CAST(Employee.QuitDate AS date) >= CAST(@QuitDateBegin AS date)) OR (@QuitDateBegin IS NULL))
				AND ((CAST(Employee.QuitDate AS date) <= CAST(@QuitDateEnd AS date)) OR (@QuitDateEnd IS NULL))
				AND ((employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId =''))
				AND ((employee.Name LIKE '%' + @EmployeeName + '%') OR (@EmployeeName =''))
				AND ((employee.[Status] = @ActiveStatus) OR (@ActiveStatus = -1))
				AND (employee.IsActive = 1)
				AND (employeeCompanyInfo.rowNum = 1))

		
				DECLARE  @imax INT, @i INT;	
				SELECT @imax = COUNT(EmployeeId) FROM @EmployeeLeaveInfo

				SET @i = 1
		
				WHILE (@i <= @imax)
				BEGIN	
					DECLARE @EmployeeIdTemp UNIQUEIDENTIFIER,
						@EmployeeCardIdTemp NVARCHAR(100),
						@BranchUnitIdTemp INT,
						@EmployeeTypeIdTemp INT;

					SELECT @EmployeeIdTemp = EmployeeId,
					   @EmployeeCardIdTemp = EmployeeCardId,
					   @BranchUnitIdTemp =  BranchUnitId,
					   @EmployeeTypeIdTemp = EmployeeTypeId
					   FROM @EmployeeLeaveInfo
					   WHERE RowID = @i;

					DECLARE @EmployeeLeaveHistoryId INT = NULL;
					SELECT @EmployeeLeaveHistoryId = elh.EmployeeLeaveHistoryId
				      FROM EmployeeLeaveHistory elh
				      WHERE elh.EmployeeId = @EmployeeIdTemp
				      AND elh.[Year] = @Year
				      AND elh.IsActive = 1


			  
					IF(@EmployeeLeaveHistoryId IS NULL)
					BEGIN
						INSERT INTO EmployeeLeaveHistory 
						(EmployeeId,
						 EmployeeCardId,
						 [Year],
						 LeaveTypeId,
						 NoOfAllowableLeaveDays,
						 NoOfConsumedLeaveDays,
						 NoOfRemainingLeaveDays,
						 CreatedDate,
						 IsActive)
					SELECT @EmployeeIdTemp,
						   @EmployeeCardIdTemp,
						   @Year,
						   LeaveTypeId,
						   NoOfDays,
						   0,
						   0,
						   CURRENT_TIMESTAMP,
						   1
						   FROM LeaveSetting
						   WHERE EmployeeTypeId = @EmployeeTypeIdTemp
						   AND BranchUnitId = @BranchUnitIdTemp
						   AND IsActive = 1	
					END
					ELSE
					BEGIN
						DECLARE  @EmployeeLeaveSetting TABLE(
											 RowID	INT    IDENTITY ( 1 , 1 ),
											 LeaveTypeId INT
											 );	

				   
						DECLARE @EmployeeLeaveSettingId INT;

						INSERT INTO @EmployeeLeaveSetting
						(
							LeaveTypeId
						)					
						SELECT LeaveTypeId
						FROM LeaveSetting
						WHERE EmployeeTypeId = @EmployeeTypeIdTemp
						AND BranchUnitId = @BranchUnitIdTemp
						AND IsActive = 1	
					
						DECLARE  @jmax INT, @j INT;	
						SELECT @jmax = COUNT(RowID) FROM @EmployeeLeaveSetting
						SET @j = 1
		
						WHILE (@j <= @jmax)
						BEGIN
							DECLARE @EmployeeLeaveTypeIdTemp INT;

							SELECT @EmployeeLeaveTypeIdTemp = LeaveTypeId
								FROM @EmployeeLeaveSetting
								WHERE RowID = @j;

							DECLARE @EmployeeLeaveHistoryTempId INT = NULL;
							SELECT @EmployeeLeaveHistoryTempId = elh.EmployeeLeaveHistoryId
								  FROM EmployeeLeaveHistory elh
								  WHERE elh.EmployeeId = @EmployeeIdTemp
								  AND elh.[Year] = @Year
								  AND elh.LeaveTypeId = @EmployeeLeaveTypeIdTemp
								  AND elh.IsActive = 1

							IF(@EmployeeLeaveHistoryTempId IS NULL)
							BEGIN
								INSERT INTO EmployeeLeaveHistory 
								(EmployeeId,
								 EmployeeCardId,
								 [Year],
								 LeaveTypeId,
								 NoOfAllowableLeaveDays,
								 NoOfConsumedLeaveDays,
								 NoOfRemainingLeaveDays,
								 CreatedDate,
								 IsActive)
								SELECT @EmployeeIdTemp,
								   @EmployeeCardIdTemp,
								   @Year,
								   LeaveTypeId,
								   NoOfDays,
								   0,
								   0,
								   CURRENT_TIMESTAMP,
								   1
								   FROM LeaveSetting
								   WHERE EmployeeTypeId = @EmployeeTypeId
								   AND BranchUnitId = @BranchUnitId
								   AND LeaveTypeId = @EmployeeLeaveTypeIdTemp
								   AND IsActive = 1					
							END
							SET @j = @j + 1;
						END

						DELETE FROM @EmployeeLeaveSetting;
					END
			   
				   UPDATE EmployeeLeaveHistory
				   SET NoOfConsumedLeaveDays = 
						(SELECT ISNULL(COUNT(ConsumedDate),0)
							FROM EmployeeLeaveDetail
							WHERE EmployeeLeaveHistory.EmployeeId = EmployeeLeaveDetail.EmployeeId
							AND EmployeeLeaveHistory.LeaveTypeId = EmployeeLeaveDetail.LeaveTypeId
							AND EmployeeLeaveDetail.IsActive = 1
							AND EmployeeLeaveHistory.IsActive = 1
							AND Year(EmployeeLeaveDetail.ConsumedDate) = @Year
							GROUP BY EmployeeLeaveDetail.LeaveTypeId
						)
					WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeIdTemp
					AND EmployeeLeaveHistory.[Year] = @Year
					AND EmployeeLeaveHistory.IsActive = 1

				   UPDATE EmployeeLeaveHistory
				   SET NoOfRemainingLeaveDays = (ISNULL(NoOfAllowableLeaveDays,0) - ISNULL(NoOfConsumedLeaveDays,0))
				   WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeIdTemp
				   AND EmployeeLeaveHistory.[Year] = @Year	
				   AND EmployeeLeaveHistory.IsActive = 1
 
			-- Newly Added for earn leave --
				   	DECLARE   @EarnLeave					INT
					DECLARE   @ConsumedEarnLeave			INT
				

				   	SELECT @EarnLeave = CAST(COUNT(1)/18 AS INT) FROM EmployeeInOut 
					WHERE EmployeeId = @EmployeeIdTemp 
					AND (Status = 'Present' OR Status = 'Late') 
					AND CAST(TransactionDate AS DATE) BETWEEN '2017-01-01' AND CAST(@UpToDate AS DATE)
					AND EmployeeTypeId IN (2,3,4,5)
					AND JoiningDate <= DATEADD(Year, -1, CAST(GETDATE() AS DATE))
					AND IsActive = 1

					--SELECT @ConsumedEarnLeave = COUNT(1) FROM EmployeeLeaveDetail
					--WHERE EmployeeCardId = @EmployeeCardId 					
					--AND CAST(ConsumedDate AS DATE) <= @UpToDate	
					--AND LeaveTypeId = 5		
					--AND IsActive = 1
					
				
					SELECT @ConsumedEarnLeave = ISNULL(CAST(SUM(Days) AS INT), 0) FROM [EarnLeavegivenByYear] 
					WHERE [EarnLeavegivenByYear].EmployeeCardId = @EmployeeCardId
					GROUP BY EmployeeId


					SELECT @ConsumedEarnLeave = @ConsumedEarnLeave + ISNULL(CAST(SUM(ApprovedTotalDays) AS INT), 0) 
					FROM EmployeeLeave 
					WHERE EmployeeCardId = @EmployeeCardId
					AND LeaveTypeId = 5 AND ApprovalStatus = 1 
					AND IsActive = 1 AND CAST(ApprovedFromDate AS DATE) >= '2017-01-01' AND CAST(ApprovedToDate AS DATE) <= GETDATE() 

					
				   	UPDATE EmployeeLeaveHistory   
					SET NoOfAllowableLeaveDays = @EarnLeave
					WHERE EmployeeId = @EmployeeIdTemp
					AND LeaveTypeId = 5
					AND Year = @Year
					AND IsActive = 1									

					-- Set consumed earn leave
					UPDATE EmployeeLeaveHistory   
					SET NoOfConsumedLeaveDays = @ConsumedEarnLeave
					WHERE EmployeeId = @EmployeeIdTemp
					AND LeaveTypeId = 5
					AND Year = @Year
					AND IsActive = 1

					-- Set remaining earn leave
					UPDATE EmployeeLeaveHistory   
					SET [NoOfRemainingLeaveDays] = (@EarnLeave- @ConsumedEarnLeave)
					WHERE EmployeeId = @EmployeeIdTemp
					AND LeaveTypeId = 5
					AND Year = @Year
					AND IsActive = 1
		
				   SET @i = @i + 1 

				END

				DELETE FROM @EmployeeLeaveInfo;

			END

				  DELETE FROM [EmployeeLeaveHistory]
				  WHERE LeaveTypeId = 4 AND EmployeeId IN
				  (
						SELECT EmployeeId FROM Employee WHERE GenderId = 1
				  )

			SELECT  
					 @CompanyName					AS CompanyName
					,@CompanyAddress				AS CompanyAddress
					,@BranchName					AS BranchName
					,@UnitName						AS UnitName
					,@DepartmentName				AS DepartmentName
					,@SectionName					AS SectionName
					,@LineName						AS LineName
					,@EmployeeTypeName              AS EmployeeTypeName	
					
					,Department.Name				AS Department
					,Section.Name					AS Section
					,Line.Name						AS Line
					,EmployeeType.Title				AS EmployeeType
					,EmployeeDesignation.Title		AS EmployeeDesignation						
					,elh.EmployeeCardId
					,Employee.Name AS EmployeeName
					,elh.Year AS [Year]
					,lt.Title AS LeaveTitle
					,ISNULL(elh.NoOfAllowableLeaveDays,0) AS TotalAllowedLeave
					,ISNULL(elh.NoOfConsumedLeaveDays,0) AS TotalConsumedLeave
					,ISNULL(elh.NoOfRemainingLeaveDays,0) AS ToalAvailableLeave
					FROM EmployeeLeaveHistory elh
					INNER JOIN LeaveType lt ON elh.LeaveTypeID = lt.Id
					INNER JOIN Employee employee ON elh.EmployeeId = employee.EmployeeId
					LEFT JOIN
					(SELECT EmployeeId, PunchCardNo, FromDate,DesignationId,BranchUnitDepartmentId,DepartmentSectionId,DepartmentLineId, 
					 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
					FROM EmployeeCompanyInfo AS employeeCompanyInfo
					WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @UpToDate) OR (@UpToDate IS NULL))
					AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
					ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1  
	
					LEFT JOIN EmployeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id AND employeeDesignation.IsActive = 1 
					LEFT JOIN EmployeeType ON employeeDesignation.EmployeeTypeId = employeeType.Id AND employeeType.IsActive = 1 
					LEFT JOIN EmployeeGrade ON employeeDesignation.GradeId = employeeGrade.Id AND employeeGrade.IsActive = 1  
					LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId 
					LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
					LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
					LEFT JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
					LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
					LEFT JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
					LEFT JOIN Company  AS company ON branch.CompanyId = company.Id
					LEFT JOIN DepartmentSection departmentSection ON employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId 
					LEFT JOIN Section section ON departmentSection.SectionId = section.SectionId
					LEFT JOIN DepartmentLine departmentLine ON employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
					LEFT JOIN Line line ON departmentLine.LineId = line.LineId 

					WHERE
					(company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds)
					AND branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds)
					AND branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
					AND branchUnitDepartment.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
					AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId = -1)
					AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId = -1)			 							
					AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
					AND ((employeeGrade.Id = @EmployeeGradeID) OR (@EmployeeGradeID = -1))
					AND ((employeeDesignation.Id = @EmployeeDesignationID) OR (@EmployeeDesignationID = -1))
					AND (Employee.GenderId = @GenderId OR @GenderId =-1)
					AND ((CAST(Employee.JoiningDate AS date) >= CAST(@JoiningDateBegin AS date)) OR (@JoiningDateBegin IS NULL))
					AND ((CAST(Employee.JoiningDate AS date) <= CAST(@JoiningDateEnd AS date)) OR (@JoiningDateEnd IS NULL))
					AND ((CAST(Employee.QuitDate AS date) >= CAST(@QuitDateBegin AS date)) OR (@QuitDateBegin IS NULL))
					AND ((CAST(Employee.QuitDate AS date) <= CAST(@QuitDateEnd AS date)) OR (@QuitDateEnd IS NULL))
					AND ((employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId =''))
					AND ((employee.Name LIKE '%' + @EmployeeName + '%') OR (@EmployeeName =''))
					AND ((employee.[Status] = @ActiveStatus) OR (@ActiveStatus = -1))
					AND (employee.IsActive = 1)
					AND (employeeCompanyInfo.rowNum = 1))
					AND (elh.[Year] = @Year OR @Year = -1)
					AND (elh.LeaveTypeId = @LeaveTypeId OR @LeaveTypeId = -1)
					AND elh.IsActive = 1
					AND lt.IsActive = 1
					ORDER BY LeaveTypeId

	COMMIT TRAN

END
