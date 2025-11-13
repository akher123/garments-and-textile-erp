
-- =================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-11-18>
-- Description:	<> EXEC [dbo].[spHrmCheckEmployeeLeaveExistence] 'b37b3adc-1e4d-4b05-ab63-37c508c41813', '2016-04-12', '2016-04-13'
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[spHrmCheckEmployeeLeaveExistence]
				 @EmployeeId UNIQUEIDENTIFIER,
				 @ConsumedFromDate DATETIME = '1900-01-01',
				 @ConsumedToDate DATETIME = '1900-01-01'
AS
BEGIN
	
		SET XACT_ABORT ON;
		SET NOCOUNT ON;
		 	

		DECLARE @CountLeave INT = 0;
		DECLARE	@Days INT;
		SET @Days = DATEDIFF(DAY, @ConsumedFromDate, @ConsumedToDate);

		WHILE @Days >= 0
		BEGIN
					
			SELECT @CountLeave = ISNULL(COUNT([EmployeeLeaveId]),0)
			FROM [dbo].[EmployeeLeaveDetail] eld
			WHERE eld.EmployeeId = @EmployeeId
			AND CAST(eld.ConsumedDate AS DATE) = CAST(@ConsumedFromDate AS DATE)
			AND eld.IsActive = 1

			IF(@CountLeave > 0)
			BREAK;

			SET @ConsumedFromDate =  DATEADD (day , 1 , @ConsumedFromDate)
		    SET @Days = @Days - 1
		END

		IF (@CountLeave > 0)
			SELECT 1;
		ELSE
			SELECT 0;

END






