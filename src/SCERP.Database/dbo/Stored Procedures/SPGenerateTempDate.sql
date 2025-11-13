-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGenerateTempDate '02/01/2015','02/10/2015','adb7d419-a37f-4e6d-b40a-3a1b0a66749f'
-- =============================================
CREATE PROCEDURE [dbo].[SPGenerateTempDate]
	   
		@StartDate DATETIME,
		@EndDate DATETIME,
		@EmployeeId uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON;

	TRUNCATE TABLE TempDate

	DECLARE	@Days INT,
			@Count INT
	SET @Days = DATEDIFF(DAY, @StartDate, @EndDate)

WHILE @Days >= 0
	BEGIN
		
		INSERT INTO TempDate (MonthDate, MonthDay, EmployeeId) values(@StartDate, DateName(dw, @StartDate), @EmployeeId)
  	    SET @StartDate =  DATEADD (day, 1, @StartDate)
		SET @Days = @Days - 1;	
	END	   
END




