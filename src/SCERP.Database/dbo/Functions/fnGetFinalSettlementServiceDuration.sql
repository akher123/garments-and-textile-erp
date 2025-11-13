
-- SELECT [dbo].[fnGetFinalSettlementServiceDuration]('B0EA5327-0A08-4863-BA70-9D0F0DD83053','2018-02-04', '2019-02-25')

CREATE FUNCTION [dbo].[fnGetFinalSettlementServiceDuration] (  

						 @EmployeeId UNIQUEIDENTIFIER
						,@JoinDate DATE
					    ,@QuitDate DATE
)

RETURNS NVARCHAR(20)

AS BEGIN
	
		DECLARE @Result NVARCHAR(200) = ''
		DECLARE @Duration INT

		SET @Duration = DATEDIFF(Day, @joinDate, @QuitDate)


		IF(@Duration < 365)
		BEGIN
				SET @Result = '0'
		END

		IF(@Duration >= 365)
		BEGIN
				SET @Result = CAST(((@Duration - @Duration % 365)/365) AS NVARCHAR(5))
				SET @Duration = @Duration%365
		END
		
		IF(@Duration < 30)
		BEGIN
				SET @Result =  @Result + '-0'
		END		

		IF(@Duration >= 30)
		BEGIN
				SET @Result = @Result + '-' + CAST(((@Duration - @Duration % 30)/30) AS NVARCHAR(5))
				SET @Duration = @Duration % 30
		END

		SET @Result = @Result + '-' + CAST(@Duration AS NVARCHAR(5))
		
		RETURN @Result

END