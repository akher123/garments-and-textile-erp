
-- =============================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-11-18>
-- Description:	<> EXEC [HRMSPUpdateEmployeeLeave] -1, -1, -1, -1, -1, -1, '2015-10-01','2015-10-31', -1, '0985','superadmin'
-- =============================================================================================================================

CREATE PROCEDURE [dbo].[HRMSPUpdateEmployeeLeave]
				 @EmployeeLeaveId INT,
				 @EmployeeId UNIQUEIDENTIFIER,
				 @EmployeeCardId NVARCHAR(100),
				 @LeaveTypeId INT,
				 @SubmitDate DATETIME,
				 @AppliedFromDate DATETIME,
				 @AppliedToDate DATETIME,
				 @AppliedTotalDays INT,
				 @LeavePurpose NVARCHAR(MAX),
				 @EmergencyPhoneNo NVARCHAR(100) = '',
				 @AddressDuringLeave NVARCHAR(MAX) = '',
				 @RecommendedFromDate DATETIME= '1900-01-01',
				 @RecommendedToDate DATETIME= '1900-01-01',
				 @RecommendedTotalDays INT = -1,
				 @RecommendationStatus INT = -1,
				 @RecommendationStatusDate DATETIME = '1900-01-01',
				 @RecommendationPerson UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000',
				 @RecommendationComment NVARCHAR(MAX)='',
				 @ApprovedFromDate DATETIME = '1900-01-01',
				 @ApprovedToDate DATETIME = '1900-01-01',
				 @ApprovedTotalDays INT = -1,
				 @ApprovalStatus INT = -1,
				 @ApprovalStatusDate DATETIME = '1900-01-01',
				 @ApprovalPerson UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000',
				 @ApprovalComment NVARCHAR(MAX) = '',
				 @ConsumedFromDate DATETIME = '1900-01-01',
				 @ConsumedToDate DATETIME = '1900-01-01',
				 @ConsumedTotalDays INT = -1,
				 @JoinedBeforeDays INT = 0,
				 @ResumeDate DATETIME = '1900-01-01',
				 @CreatedDate DATETIME,
				 @CreatedBy UNIQUEIDENTIFIER,
				 @EditedDate DATETIME,
				 @EditedBy UNIQUEIDENTIFIER,
				 @IsActive BIT				 
AS
BEGIN
	
		SET XACT_ABORT ON;
		SET NOCOUNT ON;

		         	 	
		BEGIN TRAN
				
				DECLARE @Year INT,
				@BranchUnitId INT,
				@EmployeeTypeId INT

				SELECT @BranchUnitId =  branchUnit.BranchUnitId,
					   @EmployeeTypeId = employeeType.Id
				FROM Employee LEFT JOIN
				(SELECT EmployeeId, FromDate, BranchUnitDepartmentId,DesignationId,
					ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
					FROM EmployeeCompanyInfo AS employeeCompanyInfo
					WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= CURRENT_TIMESTAMP)
					AND employeeCompanyInfo.IsActive = 1)) employeeCompanyInfo 
				ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1  
				LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId 
				LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
				LEFT JOIN EmployeeDesignation employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id AND employeeDesignation.IsActive = 1 
				LEFT JOIN EmployeeType employeeType ON employeeDesignation.EmployeeTypeId = employeeType.Id AND employeeType.IsActive = 1
				WHERE Employee.EmployeeId = @EmployeeId


				IF(CAST(@RecommendedFromDate AS DATE) = '1900-01-01')
					SET @RecommendedFromDate = NULL;

				IF(CAST(@RecommendedToDate AS DATE) = '1900-01-01')
					SET @RecommendedToDate =NULL;

				IF(@RecommendedTotalDays = -1)
					SET @RecommendedTotalDays = NULL;
		
				IF(@RecommendationStatus = -1)
					SET @RecommendationStatus = NULL;

				IF(CAST(@RecommendationStatusDate AS DATE) = '1900-01-01')
					SET @RecommendationStatusDate =NULL;

				IF(@RecommendationPerson = '00000000-0000-0000-0000-000000000000')
					SET @RecommendationPerson = NULL;


				IF(CAST(@ApprovedFromDate AS DATE) = '1900-01-01')
					SET @ApprovedFromDate = NULL;

				IF(CAST(@ApprovedToDate AS DATE) = '1900-01-01')
					SET @ApprovedToDate =NULL;

				IF(@ApprovedTotalDays = -1)
					SET @ApprovedTotalDays = NULL;
		
				IF(@ApprovalStatus = -1)
					SET @ApprovalStatus = NULL;

				IF(CAST(@ApprovalStatusDate AS DATE) = '1900-01-01')
					SET @ApprovalStatusDate =NULL;
		
				IF(@ApprovalPerson = '00000000-0000-0000-0000-000000000000')
					SET @ApprovalPerson = NULL;
		
				IF(CAST(@ConsumedFromDate AS DATE) = '1900-01-01')
					SET @ConsumedFromDate = NULL;

				IF(CAST(@ConsumedToDate AS DATE) = '1900-01-01')
					SET @ConsumedToDate =NULL;

				IF(@ConsumedTotalDays = -1)
					SET @ConsumedTotalDays = NULL;

				IF(CAST(@ResumeDate AS DATE) = '1900-01-01')
					SET @ResumeDate = NULL;

				 UPDATE [dbo].[EmployeeLeave]		
				 SET 
				 [EmployeeId] = @EmployeeId
				,[EmployeeCardId] = @EmployeeCardId
				,[LeaveTypeId] = @LeaveTypeId
				,[SubmitDate] = @SubmitDate
				,[AppliedFromDate] = @AppliedFromDate
				,[AppliedToDate] = @AppliedToDate
				,[AppliedTotalDays] = @AppliedTotalDays
				,[LeavePurpose] = @LeavePurpose
				,[EmergencyPhoneNo] = @EmergencyPhoneNo
				,[AddressDuringLeave] = @AddressDuringLeave
				,[RecommendedFromDate] = @RecommendedFromDate
				,[RecommendedToDate] = @RecommendedToDate
				,[RecommendedTotalDays] = @RecommendedTotalDays
				,[RecommendationStatus] = @RecommendationStatus
				,[RecommendationStatusDate] = @RecommendationStatusDate
				,[RecommendationPerson] = @RecommendationPerson
				,[RecommendationComment] = @RecommendationComment
				,[ApprovedFromDate] = @ApprovedFromDate
				,[ApprovedToDate] = @ApprovedToDate
				,[ApprovedTotalDays] = @ApprovedTotalDays
				,[ApprovalStatus] = @ApprovalStatus
				,[ApprovalStatusDate] = @ApprovalStatusDate
				,[ApprovalPerson] = @ApprovalPerson
				,[ApprovalComment] = @ApprovalComment
				,[ConsumedFromDate] = @ConsumedFromDate
				,[ConsumedToDate] = @ConsumedToDate
				,[ConsumedTotalDays] = @ConsumedTotalDays
				,[JoinedBeforeDays] = @JoinedBeforeDays
				,[ResumeDate] = @ResumeDate
				,[CreatedDate] = @CreatedDate
				,[CreatedBy] = @CreatedBy
				,[EditedDate] = @EditedDate
				,[EditedBy] = @EditedBy
				,[IsActive] = @IsActive
				WHERE Id = @EmployeeLeaveId


				DELETE FROM  [EmployeeLeaveDetail]
				WHERE EmployeeLeaveId = @EmployeeLeaveId
					  AND [EmployeeId] = @EmployeeId
					  AND [EmployeeCardId] = @EmployeeCardId
					  AND IsActive = 1


				DECLARE @LeaveTypeTitle NVARCHAR(MAX);
				SELECT @LeaveTypeTitle = Title FROM LeaveType WHERE Id = @LeaveTypeId AND IsActive = 1

				DECLARE	@Days INT;
				SET @Days = DATEDIFF(DAY, @ConsumedFromDate, @ConsumedToDate);

				WHILE @Days >= 0
				BEGIN
					INSERT INTO [dbo].[EmployeeLeaveDetail]
					   ([EmployeeLeaveId]
					   ,[EmployeeId]
					   ,[EmployeeCardId]
					   ,[ConsumedDate]
					   ,[SubmitDate]
					   ,[LeaveTypeId]
					   ,[LeaveTypeTitle]
					   ,[CreatedDate]
					   ,[CreatedBy]
					   ,[IsActive])
					VALUES(
						@EmployeeLeaveId
					   ,@EmployeeId
					   ,@EmployeeCardId
					   ,@ConsumedFromDate
					   ,@SubmitDate
					   ,@LeaveTypeId
					   ,@LeaveTypeTitle
					   ,@CreatedDate
					   ,@CreatedBy
					   ,@IsActive)

					SET @Year = YEAR(@ConsumedFromDate)
			
					EXEC dbo.HRMSPSaveAndUpdateIndividualLeaveHistoryForSpecificYear @EmployeeId, @EmployeeCardId, @Year, @BranchUnitId, @EmployeeTypeId;

					SET @ConsumedFromDate =  DATEADD (day , 1 , @ConsumedFromDate)
					SET @Days = @Days - 1
				END

		COMMIT TRAN

		IF (@@ERROR <> 0)
			SELECT 0;
		ELSE
			SELECT 1;

END






