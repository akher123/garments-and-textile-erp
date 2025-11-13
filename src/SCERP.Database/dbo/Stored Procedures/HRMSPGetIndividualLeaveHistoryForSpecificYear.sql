
-- ===============================
-- Author:		Md. Yasir Arafat
-- Create date: 2015.12.15
-- Modified : 2018-05-05
-- ===============================

--   EXEC HRMSPGetIndividualLeaveHistoryForSpecificYear '43cb0e34-9226-451c-a9b1-25c78da35477', '0835', 2019, 1, 3

CREATE PROCEDURE [dbo].[HRMSPGetIndividualLeaveHistoryForSpecificYear]


								@EmployeeId UNIQUEIDENTIFIER,
								@EmployeeCardId NVARCHAR(100),
								@Year INT,
								@BranchUnitId INT,
								@EmployeeTypeId INT
AS
BEGIN

	SET NOCOUNT ON;

			BEGIN TRAN

								DECLARE   @EmployeeLeaveHistoryId		INT
								DECLARE   @Gender						INT
								DECLARE   @EarnLeave					INT
								DECLARE   @ConsumedEarnLeave			INT

								SELECT @EmployeeLeaveHistoryId = EmployeeLeaveHistoryId
								FROM EmployeeLeaveHistory
								WHERE EmployeeId = @EmployeeId
								AND Year = @Year
								AND IsActive = 1
								
								SELECT @Gender = Employee.GenderId
								FROM Employee 			
								WHERE Employee.EmployeeId = @EmployeeId

								SELECT @EarnLeave = CAST(COUNT(1)/18 AS INT) FROM EmployeeInOut 
								WHERE EmployeeId = @EmployeeId 
								AND (Status = 'Present' OR Status = 'Late') 
								AND CAST(TransactionDate AS DATE) BETWEEN '2017-01-01' AND CAST(GETDATE() AS DATE)
								AND EmployeeTypeId IN (2,3,4,5)
								AND JoiningDate <= DATEADD(Year, -1, CAST(GETDATE() AS DATE))
								AND IsActive = 1


								--SELECT @ConsumedEarnLeave = ISNULL(CAST(SUM(Days) AS INT), 0) FROM EarnLeaveConsumed
								--WHERE EmployeeCardId = @EmployeeCardId 
								--GROUP BY EmployeeCardId
											
								SELECT @ConsumedEarnLeave = ISNULL(CAST(SUM(Days) AS INT), 0) FROM [EarnLeavegivenByYear] 
								WHERE [EarnLeavegivenByYear].EmployeeCardId = @EmployeeCardId
								GROUP BY EmployeeId


								SELECT @ConsumedEarnLeave = @ConsumedEarnLeave + ISNULL(CAST(SUM(ApprovedTotalDays) AS INT), 0) 
								FROM EmployeeLeave 
								WHERE EmployeeCardId = @EmployeeCardId
								AND LeaveTypeId = 5 AND ApprovalStatus = 1 
								AND IsActive = 1 AND CAST(ApprovedFromDate AS DATE) >= '2017-01-01' AND CAST(ApprovedToDate AS DATE) <= GETDATE() 


								IF(@EmployeeLeaveHistoryId IS NULL)
								BEGIN

									INSERT INTO EmployeeLeaveHistory 
												(
													 EmployeeId,
													 EmployeeCardId,
													 [Year],
													 LeaveTypeId,
													 NoOfAllowableLeaveDays,
													 NoOfConsumedLeaveDays,
													 NoOfRemainingLeaveDays,
													 CreatedDate,
													 IsActive
												)
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
									END

									-- Set allowable earn leave
									UPDATE EmployeeLeaveHistory   
									SET NoOfAllowableLeaveDays = @EarnLeave
									WHERE EmployeeId = @EmployeeId
									AND LeaveTypeId = 5
									AND Year = @Year
									AND IsActive = 1									

									-- Set consumed earn leave
									UPDATE EmployeeLeaveHistory   
									SET NoOfConsumedLeaveDays = @ConsumedEarnLeave
									WHERE EmployeeId = @EmployeeId
									AND LeaveTypeId = 5
									AND Year = @Year
									AND IsActive = 1

									-- Set remaining earn leave
									UPDATE EmployeeLeaveHistory   
									SET [NoOfRemainingLeaveDays] = (@EarnLeave- @ConsumedEarnLeave)
									WHERE EmployeeId = @EmployeeId
									AND LeaveTypeId = 5
									AND Year = @Year
									AND IsActive = 1


									-- Maternity visibile for Female and not visible for Male

									IF(@Gender = 1)
									BEGIN
										SELECT  LeaveType.Title,
												ISNULL(EmployeeLeaveHistory.NoOfAllowableLeaveDays,0) AS Allowed,
												ISNULL(EmployeeLeaveHistory.NoOfConsumedLeaveDays,0) AS Total,
												ISNULL(EmployeeLeaveHistory.NoOfRemainingLeaveDays,0) AS Available
												FROM EmployeeLeaveHistory INNER JOIN LeaveType ON EmployeeLeaveHistory.LeaveTypeID = LeaveType.Id																								
												WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeId
												AND EmployeeLeaveHistory.[Year] = @Year
												AND EmployeeLeaveHistory.IsActive = 1
												AND LeaveType.IsActive = 1
												AND LeaveType.Id <> 4
												ORDER BY LeaveTypeId
									END

									ELSE IF(@Gender = 2)
									BEGIN
										SELECT  LeaveType.Title,
												ISNULL(EmployeeLeaveHistory.NoOfAllowableLeaveDays,0) AS Allowed,
												ISNULL(EmployeeLeaveHistory.NoOfConsumedLeaveDays,0) AS Total,
												ISNULL(EmployeeLeaveHistory.NoOfRemainingLeaveDays,0) AS Available
												FROM EmployeeLeaveHistory INNER JOIN LeaveType ON EmployeeLeaveHistory.LeaveTypeID = LeaveType.Id																								
												WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeId
												AND EmployeeLeaveHistory.[Year] = @Year
												AND EmployeeLeaveHistory.IsActive = 1
												AND LeaveType.IsActive = 1	
												ORDER BY LeaveTypeId										
									END

			COMMIT TRAN

END