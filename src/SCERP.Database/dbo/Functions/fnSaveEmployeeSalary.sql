
CREATE FUNCTION [dbo].[fnSaveEmployeeSalary] (  

	@EmployeeId uniqueidentifier,
	@FromDate Datetime,
	@ToDate Datetime
)

RETURNS int

AS BEGIN
	
	DECLARE	@Days INT,
			@Count INT,
			@Gross DECIMAL(18,2)= 0.00

	SET @Days = DATEDIFF(DAY, @FromDate, @ToDate)

	WHILE @Days >= 0
		BEGIN	
			SELECT @Gross = @Gross+ ES.GrossSalary FROM EmployeeSalary AS ES WHERE ES.EmployeeId = @EmployeeId
			
  			SET @FromDate =  DATEADD (day, 1, @FromDate)
			SET @Days = @Days - 1;	
		END	   
			--INSERT INTO EmployeeSalary_Processed_Temp(GrossSalary) VALUES(@Gross)
			EXEC  SPGenerateTempDateMultipleEmp '02/01/2015','02/10/2015', NULL, 8, NULL, NULL, NULL

	DECLARE @Status INT
	RETURN @Status

END

