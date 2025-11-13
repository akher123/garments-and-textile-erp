
-- SELECT [dbo].[fnGetServiceDuration]('E5FDDA9E-88EF-4E03-B69B-00833C4DD588','2016-08-25')

CREATE FUNCTION [dbo].[fnGetServiceDuration] (  
	@EmployeeId uniqueidentifier,
	@Date Datetime
)

RETURNS INT

AS BEGIN
	
	SET @Date = CAST(@Date AS DATE);

	DECLARE @JoiningDate DATETIME;
	SELECT @JoiningDate = JoiningDate FROM Employee WHERE Employee.EmployeeId = @EmployeeId

	SET @JoiningDate = CAST(@JoiningDate AS DATE);

	DECLARE @BonusPercentge  NUMERIC(18,2);

	DECLARE @OneMonthBeforeEffectiveDate DATETIME = NULL,
			@ThreeMonthBeforeEffectiveDate DATETIME = NULL,
			@SixMonthBeforeEffectiveDate DATETIME = NULL,
			@NineMonthBeforeEffectiveDate DATETIME = NULL,
			@OneYearBeforeEffectiveDate DATETIME = NULL

	
	SET @OneMonthBeforeEffectiveDate = DATEADD(MONTH, -1, @Date);
	SET @ThreeMonthBeforeEffectiveDate = DATEADD(MONTH, -3, @Date);
	SET @SixMonthBeforeEffectiveDate = DATEADD(MONTH, -6, @Date);
	SET @NineMonthBeforeEffectiveDate = DATEADD(MONTH, -9, @Date);
	SET @OneYearBeforeEffectiveDate = DATEADD(YEAR, -1, @Date);

	SELECT @BonusPercentge = 
	CASE
	  WHEN @JoiningDate <= @OneYearBeforeEffectiveDate                                                       THEN 50
	  WHEN @JoiningDate <= @NineMonthBeforeEffectiveDate  AND  @JoiningDate > @OneYearBeforeEffectiveDate    THEN 35
	  WHEN @JoiningDate <= @SixMonthBeforeEffectiveDate   AND  @JoiningDate > @NineMonthBeforeEffectiveDate  THEN 25
	  WHEN @JoiningDate <= @ThreeMonthBeforeEffectiveDate AND  @JoiningDate > @SixMonthBeforeEffectiveDate   THEN 15
	  WHEN @JoiningDate <= @OneMonthBeforeEffectiveDate   AND  @JoiningDate > @ThreeMonthBeforeEffectiveDate THEN 0
	  WHEN @JoiningDate <= @Date						  AND  @JoiningDate > @OneMonthBeforeEffectiveDate   THEN 0
	END
	
	RETURN @BonusPercentge

END






