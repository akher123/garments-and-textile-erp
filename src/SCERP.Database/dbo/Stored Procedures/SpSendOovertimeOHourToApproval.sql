			CREATE procedure [dbo].[SpSendOovertimeOHourToApproval]
					 @OtDate datetime,
					 @PrepairedBy uniqueidentifier=null,
					 @Message bit out
			AS
			declare @countSign int ;
			set @countSign=(select Count(*) from LineOvertimeHour where TransactionDate=Convert(date,@OtDate) and (FirstSign='Y' or  SecondSign='Y'))
	if (@countSign>0)
	BEGIN
	set @Message=0
	END
	else
	BEGIN
	delete LineOvertimeHour   where (TransactionDate = Convert(date,@OtDate))
	set @Message=1
	
	--INSERT INTO LineOvertimeHour
	--	 (CompanyId, TransactionDate, DepartmentLineId, OvertimePerson, OvertimeHour,TTLOtAmount, FirstSign, SecondSign,PrepairedBy,Line)
	--		SELECT     CompanyId, TransactionDate, DepartmentLineId, COUNT(*) AS OvertimePerson, SUM(OTHours + ExtraOTHours + WeekendOTHours + HolidayOTHours) AS OvertimeHour,0, 'N' AS FirstSign, 
	--							  'N' AS SecondSign,@PrepairedBy,LineName
	--		FROM         EmployeeInOut
	--		WHERE     (TransactionDate = Convert(date,@OtDate)) AND (OTHours + ExtraOTHours + WeekendOTHours + HolidayOTHours > 0) AND (DepartmentLineId IS NOT NULL)
	--		GROUP BY DepartmentLineId, CompanyId, TransactionDate,LineName
			
	--	END	

		INSERT INTO LineOvertimeHour
		 (CompanyId, TransactionDate, DepartmentLineId, OvertimePerson, OvertimeHour,TTLOtAmount, FirstSign, SecondSign,PrepairedBy,Line)
			SELECT     I.CompanyId, I.TransactionDate, I.DepartmentLineId, COUNT(*) AS OvertimePerson, 
                      SUM(I.OTHours + I.ExtraOTHours + I.WeekendOTHours + I.HolidayOTHours) AS OvertimeHour, 
                      SUM((I.OTHours + I.ExtraOTHours + I.WeekendOTHours + I.HolidayOTHours) * E.BasicSalary / 104) AS TTLOtAmount, 'N' AS FirstSign, 'N' AS SecondSign, 
                      @PrepairedBy,I.LineName
FROM         EmployeeInOut AS I INNER JOIN
                      VEmployee AS E ON I.EmployeeId = E.EmployeeId
WHERE     (I.TransactionDate = CONVERT(date, @OtDate)) AND (I.OTHours + I.ExtraOTHours + I.WeekendOTHours + I.HolidayOTHours > 0) AND 
                      (I.DepartmentLineId IS NOT NULL)
GROUP BY I.DepartmentLineId, I.LineName, I.CompanyId, I.TransactionDate
	
			
	END	
	
		

		