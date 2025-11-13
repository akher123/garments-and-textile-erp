
CREATE PROCEDURE [dbo].[SpOTHoureByTransactionDate]


					@OtDate			DATETIME
				   ,@all			BIT
				   ,@garments		BIT
				   ,@knitting		BIT
				   ,@dyeing			BIT

AS

				DECLARE @list TABLE (BranchUnitId INT)

				IF(@all = 1)
				BEGIN
					 INSERT @list(BranchUnitId) values(1),(2),(3)  
				END

				IF(@garments = 1)
				BEGIN
					 INSERT @list(BranchUnitId) values(1)  
				END

				IF(@knitting = 1)
				BEGIN
					 INSERT @list(BranchUnitId) values(2)  
				END

				IF(@dyeing = 1)
				BEGIN
					 INSERT @list(BranchUnitId) values(3)  
				END


				SELECT EmployeeInOut.DepartmentName
					  ,EmployeeInOut.DepartmentLineId
				      ,LineName AS LINE
					  ,CONVERT(VARCHAR(11),TransactionDate, 106)  AS [DATE]					  
					  ,COUNT(*) AS OTP, SUM(OTHours + ExtraOTHours + WeekendOTHours + HolidayOTHours) AS OTH
					  ,CAST(ROUND(SUM((OTHours + ExtraOTHours + WeekendOTHours + HolidayOTHours)*VEmployee.BasicSalary/104),2) AS DECIMAL(18,2)) AS AMOUNT 
				FROM EmployeeInOut INNER JOIN VEmployee ON EmployeeInOut.EmployeeId = VEmployee.EmployeeId 
				
				WHERE (TransactionDate = Convert(DATE, @OtDate)) 
				AND (OTHours + ExtraOTHours + WeekendOTHours + HolidayOTHours > 0) 
				AND EmployeeInOut.BranchUnitId IN(SELECT BranchUnitId FROM @list)
				GROUP BY EmployeeInOut.DepartmentName, EmployeeInOut.DepartmentLineId, LineName, TransactionDate
				ORDER BY EmployeeInOut.DepartmentLineId