


CREATE FUNCTION [dbo].[fnGetOverTimeRate] (  

	@FromDate DateTime,
	@ToDate DateTime
)

RETURNS DECIMAL(18,2)

AS BEGIN
   
   DECLARE @Days INT,	
		   @Rate DECIMAL(18,2) = 0

		   SELECT TOP(1) @Rate = OvertimeSettings.OvertimeRate FROM OvertimeSettings	
		   WHERE OvertimeSettings.FromDate <= @ToDate 		 
		   AND OvertimeSettings.IsActive = 1
		   ORDER BY OvertimeSettings.FromDate DESC
		  	
	IF @Days IS NULL
		SET @Days = 0		
				   		
    RETURN @Rate
END





