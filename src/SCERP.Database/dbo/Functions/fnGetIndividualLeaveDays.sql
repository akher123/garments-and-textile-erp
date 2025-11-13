
CREATE FUNCTION [dbo].[fnGetIndividualLeaveDays] (  

	@EmployeeId uniqueidentifier,
	@FromDate DateTime,
	@ToDate DateTime,
	@LeaveTypeId INT
)

RETURNS INT
AS BEGIN
   
        DECLARE @JoiningDate DATETIME;
		SELECT @JoiningDate = JoiningDate FROM Employee WHERE EmployeeId = @EmployeeId AND IsActive = 1
	
		IF(CAST(@JoiningDate AS DATE) > CAST(@FromDate AS DATE))
					SELECT @FromDate = CAST(@JoiningDate AS DATE) 
	
		DECLARE @QuitDate DATETIME;
		SELECT @QuitDate = QuitDate FROM Employee WHERE EmployeeId = @EmployeeId AND IsActive = 1 AND [Status] = 2
	
		IF(@QuitDate IS NOT NULL)
		BEGIN
			IF((CAST(@QuitDate AS DATE) >= CAST(@FromDate AS DATE)) AND (CAST(@QuitDate AS DATE) < CAST(@ToDate AS DATE)))
					SELECT @ToDate = CAST(@QuitDate AS DATE) 
		END

		IF(@ToDate > CAST(CURRENT_TIMESTAMP AS DATE))
			SELECT @ToDate = CAST(CURRENT_TIMESTAMP AS DATE)

		DECLARE 
		@result int = 0
		
		--DECLARE @LastDate DATETIME = NULL, @DateDiff INT = 0;

		--/*When leave is taken between @fromDate and @toDate*/
		--DECLARE @NoOfSimpleLeave INT = 0;

		--SELECT @NoOfSimpleLeave = SUM(EmployeeLeave.ConsumedTotalDays) FROM EmployeeLeave 
		--WHERE EmployeeLeave.EmployeeId = @EmployeeId 
		--AND CAST(EmployeeLeave.ConsumedFromDate AS DATE) >= CAST(@FromDate AS DATE)
		--AND CAST(EmployeeLeave.ConsumedToDate AS DATE) <= CAST(@ToDate AS DATE)
		--AND EmployeeLeave.LeaveTypeId = @LeaveTypeId
		--AND EmployeeLeave.ApprovalStatus = 1
		--AND EmployeeLeave.IsActive = 1;
		
		--SET @NoOfSimpleLeave =  ISNULL(@NoOfSimpleLeave,0);

		--IF @NoOfSimpleLeave <= 0 
		--	SET @NoOfSimpleLeave = 0;

		--/*When leave is taken with previous month's date*/
		--DECLARE @NoOfComplexLeave1 INT = 0, @ConsumedTotalDays1 INT = 0, @DateDiff1 INT = 0;

		--SELECT @ConsumedTotalDays1 = EmployeeLeave.ConsumedTotalDays, @DateDiff1 = DATEDIFF(DAY, EmployeeLeave.ConsumedFromDate, @FromDate)
		--FROM EmployeeLeave 
		--WHERE EmployeeLeave.EmployeeId = @EmployeeId 
		--AND CAST(EmployeeLeave.ConsumedFromDate AS DATE) < CAST(@FromDate AS DATE)
		--AND CAST(EmployeeLeave.ConsumedToDate AS DATE) <= CAST(@ToDate AS DATE)
		--AND EmployeeLeave.LeaveTypeId = @LeaveTypeId
		--AND EmployeeLeave.ApprovalStatus = 1
		--AND EmployeeLeave.IsActive = 1
		--ORDER BY EmployeeLeave.SubmitDate DESC

		--SET @NoOfComplexLeave1 =  ISNULL(@ConsumedTotalDays1,0) - ISNULL(@DateDiff1,0);

		--IF @NoOfComplexLeave1 <= 0
		--	SET @NoOfComplexLeave1 = 0;
	
		--/*When leave is taken with next month's date*/
		--DECLARE @NoOfComplexLeave2 INT = 0, @ConsumedTotalDays2 INT = 0, @DateDiff2 INT = 0;

		--SELECT @ConsumedTotalDays2 = EmployeeLeave.ConsumedTotalDays, @DateDiff2 = DATEDIFF(DAY, @ToDate, EmployeeLeave.ConsumedToDate)
		--FROM EmployeeLeave 
		--WHERE EmployeeLeave.EmployeeId = @EmployeeId 
		--AND CAST(EmployeeLeave.ConsumedFromDate AS DATE) >= CAST(@FromDate AS DATE)
		--AND CAST(EmployeeLeave.ConsumedToDate AS DATE) > CAST(@ToDate AS DATE)
		--AND EmployeeLeave.LeaveTypeId = @LeaveTypeId
		--AND EmployeeLeave.ApprovalStatus = 1
		--AND EmployeeLeave.IsActive = 1
		--ORDER BY EmployeeLeave.SubmitDate DESC

		--SET @NoOfComplexLeave2 =  ISNULL(@ConsumedTotalDays2,0) - ISNULL(@DateDiff2,0);

		--IF @NoOfComplexLeave2 <= 0
		--	SET @NoOfComplexLeave2 = 0;


		--/*When leave is taken with previous month's date and next month's date*/
		--DECLARE @NoOfComplexLeave3 INT = 0, @ConsumedTotalDays3 INT = 0, @DateDiff3 INT = 0, @DateDiff4 INT = 0;

		--SELECT @ConsumedTotalDays3 = EmployeeLeave.ConsumedTotalDays,
		--@DateDiff3 = DATEDIFF(DAY, EmployeeLeave.ConsumedFromDate, @FromDate), 
		--@DateDiff4 = DATEDIFF(DAY, @ToDate, EmployeeLeave.ConsumedToDate)
		--FROM EmployeeLeave 
		--WHERE EmployeeLeave.EmployeeId = @EmployeeId 
		--AND CAST(EmployeeLeave.ConsumedFromDate AS DATE) < CAST(@FromDate AS DATE)
		--AND CAST(EmployeeLeave.ConsumedToDate AS DATE) > CAST(@ToDate AS DATE)
		--AND EmployeeLeave.LeaveTypeId = @LeaveTypeId
		--AND EmployeeLeave.ApprovalStatus = 1
		--AND EmployeeLeave.IsActive = 1
		--ORDER BY EmployeeLeave.SubmitDate DESC

		--SET @NoOfComplexLeave3 =  ISNULL(@ConsumedTotalDays3,0) - ISNULL(@DateDiff3,0) - ISNULL(@DateDiff4,0);

		--IF @NoOfComplexLeave3 <= 0
		--	SET @NoOfComplexLeave3 = 0;
	
		--SET @result = @NoOfSimpleLeave + @NoOfComplexLeave1 + @NoOfComplexLeave2 + @NoOfComplexLeave3; 

		SELECT @result = COUNT(EmployeeLeaveDetail.Id) FROM EmployeeLeaveDetail
		WHERE CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) >= CAST(@FromDate AS DATE)
		AND CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) <= CAST(@ToDate AS DATE) 
		AND EmployeeLeaveDetail.EmployeeId = @EmployeeId
		AND EmployeeLeaveDetail.LeaveTypeId = @LeaveTypeId
		AND EmployeeLeaveDetail.IsActive = 1 
		
		RETURN @result;
END



