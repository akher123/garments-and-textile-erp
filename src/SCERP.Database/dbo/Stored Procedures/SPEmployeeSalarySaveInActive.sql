
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Modified date: <23/05/2015>
-- Description:	<> exec SPEmployeeSalarySaveInActive '05/01/2015','05/31/2015', 'ae1e9c92-418e-424b-be18-509a58c00f3d'
-- ==========================================================================================================


CREATE PROCEDURE [dbo].[SPEmployeeSalarySaveInActive]
	   
				 @FromDate DATETIME
				,@ToDate DATETIME
				,@EmployeeId uniqueidentifier
	     
AS
BEGIN	

		DECLARE	 @Days INT
				,@Count INT
				,@StartDate DATETIME			
				,@Gross DECIMAL(18,2)= 0.00
				,@GrossCalculate DECIMAL(18,2)= 0.00
				,@Basic  DECIMAL(18,2)= 0.00
				,@House DECIMAL(18,2)= 0.00
				,@Medical DECIMAL(18,2)= 0.00 
				,@Transport DECIMAL(18,2)= 0.00
				,@Food DECIMAL(18,2)= 0.00
				,@Tax DECIMAL(18,2)= 0.00
				,@ProvidentFund DECIMAL(18,2)= 0.00
				,@NetSalaryPaid DECIMAL(18,2)= 0.00
				,@Stamp DECIMAL(18,2) = 10.00
				,@OtherDeduction DECIMAL(18,2) = 0.00
				,@TotalOTHours FLOAT = 0.00
				,@OTRate DECIMAL(18,2) = 0.00
				,@OTAmount DECIMAL(18,2) = 0.00
				,@AttendanceBonus DECIMAL(18,2) = 0.00
				,@ShiftingAllowance DECIMAL(18,2) = 0.00
				,@PayableAmount DECIMAL(18,2) = 0.00
				,@TotalDeduction DECIMAL(18,2) = 0.00
				,@SalaryDeduction DECIMAL(18,2) = 0.00
				,@LWP DECIMAL(18,2) = 0.00
				,@Absent DECIMAL(18,2) = 0.00
				,@Advance DECIMAL(18,2) = 0.00
				,@Comments NVARCHAR(MAX)
				,@Rate DECIMAL(18,2) = 0.00


	SELECT @ToDate = Employee.QuitDate FROM Employee WHERE Employee.EmployeeId = @EmployeeId AND Employee.IsActive = 1

	SET @Days = DATEDIFF(DAY, @FromDate, @ToDate) + 1
	SET @StartDate = @FromDate
	WHILE @Days > 0
	BEGIN	
		
		SELECT TOP(1) @Basic = ES.BasicSalary							
					  FROM EmployeeSalary AS ES WHERE ES.EmployeeId = @EmployeeId 
					  AND ES.IsActive = 1
					  AND ES.FromDate <= @FromDate 
					  ORDER BY ES.FromDate DESC

		SELECT TOP(1) @Rate = OvertimeSettings.OvertimeRate							
					  FROM OvertimeSettings  WHERE OvertimeSettings.IsActive = 1
					  AND FromDate <= @FromDate 
					  ORDER BY FromDate DESC

		SET @OTRate = (@Basic * @Rate)/208

		SELECT TOP(1) @GrossCalculate = @GrossCalculate + (ES.GrossSalary/Day(EOMONTH(@StartDate)))	
					 ,@OTAmount = @OTAmount +  @OTRate * dbo.fnGetTotalOTHours(ES.EmployeeId, @StartDate, @StartDate)						 				
					  FROM EmployeeSalary AS ES WHERE ES.EmployeeId = @EmployeeId		
					  AND ES.FromDate <= @StartDate 
					  ORDER BY ES.FromDate DESC
  	
		SET @StartDate = DATEADD (day, 1, @StartDate)
		SET @Days = @Days - 1;	
	END	
	

	SET @TotalOTHours = dbo.fnGetTotalOTHours(@EmployeeId, @FromDate, @ToDate)

	SELECT  @Advance = dbo.fnGetAdvanceAmount(@EmployeeId, @FromDate, @ToDate)

	SELECT TOP(1) @Gross = ES.GrossSalary,
				  @Basic = ES.BasicSalary,
				  @House = ES.HouseRent,
				  @Medical = ES.MedicalAllowance,
				  @Food = ES.FoodAllowance,
				  @Transport = ES.Conveyance				
				  FROM EmployeeSalary AS ES WHERE ES.EmployeeId = @EmployeeId		
				  AND ES.FromDate <= @FromDate 
				  ORDER BY ES.FromDate DESC

	SELECT TOP(1) @Stamp = StampAmount.Amount FROM StampAmount 
				 WHERE StampAmount.IsActive = 1 AND CAST(StampAmount.FromDate AS date) <= @fromDate
				 ORDER BY StampAmount.FromDate DESC 

	SET @AttendanceBonus = dbo.fnGetAttendanceBonusAmount(@EmployeeId, @FromDate, @ToDate)
	SET @SalaryDeduction = @Gross - CONVERT(decimal(5,0), ROUND(@GrossCalculate,2))
	SET @TotalDeduction = @SalaryDeduction + @Stamp +  + @LWP + @Absent + @Advance
	SET @PayableAmount = @Gross - @TotalDeduction + @AttendanceBonus + @ShiftingAllowance
	SET @NetSalaryPaid = @PayableAmount + @OTAmount	


	IF EXISTS (SELECT * FROM EmployeeSalary_Processed_Temp AS ESP WHERE ESP.EmployeeId = @EmployeeId AND (MONTH(ESP.ToDate) = MONTH(@ToDate) AND YEAR(ESP.ToDate) = YEAR(@ToDate)))
	BEGIN
		UPDATE EmployeeSalary_Processed_Temp 
		SET IsActive = 0
		WHERE EmployeeId = @EmployeeId AND MONTH(ToDate) = MONTH(@ToDate) AND YEAR(ToDate) = YEAR(@ToDate)
	END

	
	INSERT INTO [dbo].[EmployeeSalary_Processed_Temp]
           ([EmployeeId]
           ,[FromDate]
           ,[ToDate]
           ,[GrossSalary]
           ,[BasicSalary]
           ,[HouseRent]
           ,[MedicalAllowance]
           ,[Conveyance]
           ,[FoodAllowance]
           ,[LWP]
           ,[Absent]
           ,[Advance]
           ,[Stamp]
           ,[TotalDeduction]
           ,[AttendanceBonus]
           ,[ShiftingAllowance]
           ,[PayableAmount]
           ,[OTHours]
           ,[OTRate]
           ,[OTAmount]
           ,[NetSalaryPaid]
           ,[Comments]
           ,[Tax]
           ,[ProvidentFund]
           ,[CreatedDate]    
           ,[IsActive])

		    VALUES

           (@EmployeeId
		   ,@FromDate
		   ,@ToDate
		   ,@Gross
		   ,@Basic
		   ,@House
		   ,@Medical
		   ,@Transport
		   ,@Food
		   ,@LWP
		   ,@Absent
		   ,@Advance
		   ,@Stamp
		   ,@TotalDeduction
		   ,@AttendanceBonus
		   ,@ShiftingAllowance
		   ,@PayableAmount
		   ,@TotalOTHours
		   ,@OTRate
		   ,@OTAmount
		   ,@NetSalaryPaid
		   ,@Comments
		   ,@Tax
		   ,@ProvidentFund
		   ,GETDATE()
		   ,1
           )
END





