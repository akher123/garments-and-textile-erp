
-- SELECT [dbo].[fnGetAdvancedIncomeTax] ('3728ef42-768f-4247-8cc3-378bcf324705','2017-01-26','2017-02-25')

CREATE FUNCTION [dbo].[fnGetAdvancedIncomeTax] (  

				@EmployeeId uniqueidentifier,
				@FromDate DateTime,
				@ToDate DateTime
)

RETURNS DECIMAL(18,2)

AS BEGIN
   
   DECLARE	@Tax DECIMAL(18,2)= 0.00			    				   			

			SELECT @Tax = Amount FROM AdvanceIncomeTax  
			WHERE EmployeeId = @EmployeeId		
																			  		  				  		  		  		  		  		  		  				  
			IF @Tax IS NULL
				SET @Tax = 0

    RETURN @Tax
END




