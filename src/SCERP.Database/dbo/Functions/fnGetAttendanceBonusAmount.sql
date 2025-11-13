
-- SELECT [dbo].[fnGetAttendanceBonusAmount] ('48EEB02B-AE42-4B5B-B10E-8CA29CAF01B0', '2019-08-01', '2019-08-31')


CREATE FUNCTION [dbo].[fnGetAttendanceBonusAmount] (  

	@EmployeeId uniqueidentifier,
	@FromDate Datetime,
	@ToDate Datetime
)

RETURNS DECIMAL(18,2)

AS BEGIN		
		DECLARE	@Amount DECIMAL(18,2) = 0.00,
				@Result DECIMAL(18,2) = 0.00,
				@Count INT = 0;

		DECLARE @JoiningDate DATETIME;
		SELECT @JoiningDate = JoiningDate FROM Employee WHERE EmployeeId = @EmployeeId AND IsActive = 1
	
		IF(CAST(@JoiningDate AS DATE) > CAST(@FromDate AS DATE))
			   RETURN @Result
	 
		DECLARE @QuitDate DATETIME;
		SELECT @QuitDate = QuitDate FROM Employee WHERE EmployeeId = @EmployeeId AND IsActive = 1 AND [Status] = 2
	
		IF(@QuitDate IS NOT NULL AND (CAST(@QuitDate AS DATE) < CAST(@ToDate AS DATE)))
			   RETURN @Result	

		SELECT @Amount = ISNULL(AttendanceBonus.Amount,0.0) FROM
		(SELECT  DesignationId, Amount, ROW_NUMBER() OVER (PARTITION BY DesignationId ORDER BY FromDate DESC) AS attendancerowNum						
		FROM AttendanceBonus AS AttendanceBonus 
		WHERE ((CAST(AttendanceBonus.FromDate AS Date) <= @FromDate)) AND AttendanceBonus.IsActive = 1) 
		AttendanceBonus JOIN
		(SELECT EmployeeId, DesignationId, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS companyrowNum						
		FROM EmployeeCompanyInfo AS employeeCompanyInfo 
		WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate)) AND employeeCompanyInfo.IsActive = 1) 
		employeeCompanyInfo 
		ON AttendanceBonus.DesignationId = employeeCompanyInfo.DesignationId AND employeeCompanyInfo.companyrowNum = 1 AND AttendanceBonus.attendancerowNum = 1
		AND employeeCompanyInfo.EmployeeId = @EmployeeId


		IF(@Amount = 0.00)
			RETURN @Result

		SET @Count = dbo.fnGetAbsentDays(@EmployeeId, @FromDate, @ToDate)  -- This is hard code value
		IF(@Count > 0)
			RETURN @Result
		
		SELECT @Count = COUNT(*) FROM HrmPenalty
		WHERE EmployeeId = @EmployeeId AND PenaltyTypeId = 2 AND CAST(PenaltyDate AS DATE) BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE) AND IsActive = 1
		IF(@Count > 0)
			RETURN @Result	
									  
		SET @Count = dbo.fnGetTotalLateDays(@EmployeeId, @FromDate, @ToDate)

		IF(@Count > 1)
			RETURN @Result
		
		/*This rule is not applicable from January-2016*/
		--SELECT @Count = COUNT(EmployeeLeaveDetail.Id) FROM EmployeeLeaveDetail
		--WHERE CAST(EmployeeLeaveDetail.SubmitDate AS date) > CAST(EmployeeLeaveDetail.ConsumedDate AS date)
		--AND CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) >= CAST(@FromDate AS DATE)
		--AND CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) <= CAST(@ToDate AS DATE) 
		--AND  EmployeeLeaveDetail.IsActive = 1 
		--AND  EmployeeLeaveDetail.EmployeeId = @EmployeeId

		--IF(@Count > 0)
		--	RETURN @Result


		SELECT @Count = COUNT(eio.EmployeeId) FROM EmployeeInOut eio
					WHERE  eio.EmployeeId = @EmployeeId
					AND (CAST(eio.TransactionDate AS DATE)) >= (CAST(@FromDate AS DATE))
					AND (CAST(eio.TransactionDate AS DATE)) <= (CAST(@ToDate AS DATE))
					AND eio.IsActive = 1
					AND REPLACE(eio.[Status], ' ', '') = 'LEAVE'
					AND REPLACE(eio.Remarks, ' ', '') NOT LIKE '%alt%'
			
		IF(@Count > 0)
			RETURN @Result

		SET @Result = @Amount
		RETURN @Result

END