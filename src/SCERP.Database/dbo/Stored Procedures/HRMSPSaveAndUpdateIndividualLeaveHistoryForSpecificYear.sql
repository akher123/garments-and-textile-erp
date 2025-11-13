
-- =============================================
-- Author:		Golam Rabbi
-- Create date: 2015.12.15
-- =============================================

-- EXEC HRMSPSaveAndUpdateIndividualLeaveHistoryForSpecificYear '13ed233a-e722-40d7-850e-c9e003bb2a83', '0760', 2015, 1, 4

CREATE PROCEDURE [dbo].[HRMSPSaveAndUpdateIndividualLeaveHistoryForSpecificYear]
	@EmployeeId UNIQUEIDENTIFIER,
	@EmployeeCardId NVARCHAR(100),
	@Year INT,
	@BranchUnitId INT,
	@EmployeeTypeId INT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRAN

	DECLARE @EmployeeLeaveHistoryId INT;

	SELECT @EmployeeLeaveHistoryId = elh.EmployeeLeaveHistoryId
	FROM EmployeeLeaveHistory elh
	WHERE elh.EmployeeId = @EmployeeId
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
		SELECT @EmployeeId,
			   @EmployeeCardId,
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
					WHERE EmployeeTypeId = @EmployeeTypeId
					AND BranchUnitId = @BranchUnitId
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
							  WHERE elh.EmployeeId = @EmployeeId
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
							SELECT @EmployeeId,
								   @EmployeeCardId,
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
			AND Year(EmployeeLeaveDetail.ConsumedDate) = @Year
			AND EmployeeLeaveDetail.IsActive = 1
			AND EmployeeLeaveHistory.IsActive = 1
			GROUP BY EmployeeLeaveDetail.LeaveTypeId
		)
	WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeId
	AND EmployeeLeaveHistory.[Year] = @Year
	AND EmployeeLeaveHistory.IsActive = 1

	UPDATE EmployeeLeaveHistory
	SET NoOfRemainingLeaveDays = (ISNULL(NoOfAllowableLeaveDays,0) - ISNULL(NoOfConsumedLeaveDays,0))
	WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeId
	AND EmployeeLeaveHistory.[Year] = @Year	
	AND EmployeeLeaveHistory.IsActive = 1


	COMMIT TRAN

END

