
-- ============================================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-11-18>
-- Description:	<> EXEC [HRMSPDeleteEmployeeLeave] 1412, '3eb7faae-0a75-4c48-816c-2666b34aeac6', '0393','2015-12-17','5146fd70-8cee-4022-a606-7cfafeb7874c',0
-- ============================================================================================================================================================

CREATE PROCEDURE [dbo].[HRMSPDeleteEmployeeLeave]
				 @EmployeeLeaveId INT,
				 @EmployeeId UNIQUEIDENTIFIER,
				 @EmployeeCardId NVARCHAR(100),
				 @EditedDate DATETIME,
				 @EditedBy UNIQUEIDENTIFIER,
				 @IsActive BIT				 
AS
BEGIN
	
		SET XACT_ABORT ON;
		SET NOCOUNT ON;

		         	 	
		BEGIN TRAN
			UPDATE [dbo].[EmployeeLeave]		
			SET 
			 [EditedDate] = @EditedDate
			,[EditedBy] = @EditedBy
			,[IsActive] = @IsActive
			WHERE Id = @EmployeeLeaveId
			AND  [EmployeeId] = @EmployeeId
			AND [EmployeeCardId] = @EmployeeCardId		

			UPDATE [dbo]. [EmployeeLeaveDetail]		
			SET 
			 [EditedDate] = @EditedDate
			,[EditedBy] = @EditedBy
			,[IsActive] = @IsActive
			WHERE [EmployeeLeaveId] = @EmployeeLeaveId
			AND  [EmployeeId] = @EmployeeId
			AND [EmployeeCardId] = @EmployeeCardId	

			DECLARE  @imax INT, @i INT, @year INT;		
			DECLARE  @EmployeeLeaveDetailInfo  TABLE(
									 RowID	INT    IDENTITY ( 1 , 1 ),
									 LeaveConsumeDate DATETIME)

			INSERT INTO @EmployeeLeaveDetailInfo
			SELECT ConsumedDate FROM [dbo]. [EmployeeLeaveDetail]			
			WHERE [EmployeeLeaveId] = @EmployeeLeaveId
			AND  [EmployeeId] = @EmployeeId
			AND [EmployeeCardId] = @EmployeeCardId	

			SELECT @imax = COUNT(LeaveConsumeDate) FROM @EmployeeLeaveDetailInfo
			SET @i = 1
		
			WHILE (@i <= @imax)
			BEGIN
				SELECT @year = YEAR(LeaveConsumeDate)
				FROM   @EmployeeLeaveDetailInfo
				WHERE  RowID = @i

				UPDATE EmployeeLeaveHistory
				SET NoOfConsumedLeaveDays = 
					(SELECT ISNULL(COUNT(ConsumedDate),0)
					 FROM EmployeeLeaveDetail
					 WHERE EmployeeLeaveHistory.EmployeeId = EmployeeLeaveDetail.EmployeeId
					 AND EmployeeLeaveHistory.LeaveTypeId = EmployeeLeaveDetail.LeaveTypeId
					 AND Year(EmployeeLeaveDetail.ConsumedDate) = @Year
					 AND EmployeeLeaveDetail.IsActive = 1
					 AND EmployeeLeaveHistory.IsActive = 1
					 GROUP BY EmployeeLeaveDetail.LeaveTypeId
					 )
				WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeId
				AND EmployeeLeaveHistory.[Year] = @year
				AND EmployeeLeaveHistory.IsActive = 1

				UPDATE EmployeeLeaveHistory
				SET NoOfRemainingLeaveDays = (ISNULL(NoOfAllowableLeaveDays,0) - ISNULL(NoOfConsumedLeaveDays,0))
				WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeId
				AND EmployeeLeaveHistory.[Year] = @year	
				AND EmployeeLeaveHistory.IsActive = 1

				SET @i = @i + 1 
			END

			DELETE FROM @EmployeeLeaveDetailInfo

		COMMIT TRAN

		IF (@@ERROR <> 0)
			SELECT 0;
		ELSE
			SELECT 1;

END






