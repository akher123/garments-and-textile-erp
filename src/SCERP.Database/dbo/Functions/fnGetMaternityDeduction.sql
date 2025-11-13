
-- SELECT [dbo].[fnGetMaternityDeduction] ('E7EFF288-8387-440E-886B-20D89DEFE79E','2017-12-26','2018-01-25')

CREATE FUNCTION [dbo].[fnGetMaternityDeduction] (  

				@EmployeeId uniqueidentifier,
				@FromDate DateTime,
				@ToDate DateTime
)

RETURNS DECIMAL(18,2)

AS BEGIN
   
   DECLARE	 @Days INT
			,@TotalDays INT
			,@Count INT
			,@StartDate DATETIME		
		    ,@MaternityDeduction DECIMAL(18,2)= 0.00
				   		
			IF EXISTS(SELECT 1 FROM Employee WHERE EmployeeId = @EmployeeId AND GenderId = 1)
			RETURN @MaternityDeduction	
			
			IF NOT EXISTS(SELECT 1 FROM EmployeeLeave WHERE EmployeeId = @EmployeeId AND LeaveTypeId = 4 AND ((@FromDate BETWEEN ApprovedFromDate AND ApprovedToDate) OR (@ToDate BETWEEN ApprovedFromDate AND ApprovedToDate)))
			RETURN @MaternityDeduction	
			
		    SET @Days = DATEDIFF(DAY, @FromDate, @ToDate) + 1
			SET @TotalDays = @Days
			SET @StartDate = @FromDate

			WHILE @Days > 0
			BEGIN							 					
					IF EXISTS (SELECT  1 FROM EmployeeInOut WHERE EmployeeInOut.EmployeeId = @EmployeeId AND CAST(EmployeeInOut.TransactionDate AS DATE) = CAST(@StartDate AS DATE) AND Status = 'Leave' AND [Remarks] = 'Maternity Leave')													
					BEGIN
						SELECT TOP(1) @MaternityDeduction = @MaternityDeduction + (EmployeeSalary.GrossSalary/@TotalDays)							 						 				
									  FROM EmployeeSalary 
									  WHERE EmployeeSalary.EmployeeId = @EmployeeId		
									  AND EmployeeSalary.FromDate <= @StartDate 
									  ORDER BY EmployeeSalary.FromDate DESC  	
					END

					SET @StartDate = DATEADD (day, 1, @StartDate)
					SET @Days = @Days - 1	
					SET @Count = 0
			END	
		  		  		  		  		  		  		  				  
			IF @MaternityDeduction IS NULL
				SET @MaternityDeduction = 0

			RETURN @MaternityDeduction
END