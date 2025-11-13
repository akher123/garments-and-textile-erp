-- =======================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <19/11/2019>
-- Description:	<> EXEC GetAttendanceRawData '2020-01-02'
-- =======================================================

CREATE PROCEDURE [dbo].[GetAttendanceRawData]
			
									 
									  @Date		DATETIME						
						   
AS

BEGIN
	
			SET NOCOUNT ON;

									  DECLARE @DateToday DATE = GETDATE()
									  DECLARE @DatePrevious DATE = DATEADD(DAY, -2, @DateToday) 
					  												  
									  Truncate Table TempTable

									  INSERT INTO TempTable  -- Insert Intime data to temp table
									  SELECT EmployeeCardId, CAST(TransactionDate AS DATETIME) + CAST(InTime AS DATETIME)
									  FROM EmployeeInOut_NoPenalty
									  WHERE TransactionDate = @Date AND InTime IS NOT NULL
									  		

									  INSERT INTO TempTable  -- Insert Outtime data to temp table
									  SELECT EmployeeCardId, CAST(TransactionDate AS DATETIME) + CAST(OutTime AS DATETIME) 
									  FROM EmployeeInOut_NoPenalty
									  WHERE TransactionDate = @Date AND OutTime BETWEEN CAST('12:00:00.000' AS TIME) AND CAST('23:59:59.000' AS TIME) AND  OutTime IS NOT NULL	
									  

									  INSERT INTO TempTable  -- Insert Outtime data from previous date to temp table
									  SELECT EmployeeCardId, CAST(@Date AS DATETIME) + CAST(OutTime AS DATETIME) 
									  FROM EmployeeInOut_NoPenalty
									  WHERE TransactionDate = DATEADD(DAY, -1, @Date) AND OutTime BETWEEN CAST('00:00:00.000' AS TIME) AND CAST('11:00:00.000' AS TIME) AND  OutTime IS NOT NULL	


									  INSERT INTO TempTable  -- Insert Between data
									  SELECT EmployeeInOut_NoPenalty.[EmployeeCardId]
										    ,CAST([EmployeeDailyAttendance].[TransactionDateTime] AS DATETIME) AS TransactionTime
									  FROM [dbo].[EmployeeDailyAttendance]
									  JOIN EmployeeInOut_NoPenalty ON [EmployeeDailyAttendance].[EmployeeId] = EmployeeInOut_NoPenalty.EmployeeId AND CAST([EmployeeDailyAttendance].[TransactionDateTime] AS DATE) = EmployeeInOut_NoPenalty.TransactionDate
									  WHERE CAST([EmployeeDailyAttendance].[TransactionDateTime] AS DATE) = @Date   
									  AND EmployeeInOut_NoPenalty.InTime < CAST([EmployeeDailyAttendance].[TransactionDateTime] AS TIME)
									  AND EmployeeInOut_NoPenalty.OutTime > CAST([EmployeeDailyAttendance].[TransactionDateTime] AS TIME)


									  IF(CAST(@date AS DATE) BETWEEN @DatePrevious AND @DateToday)  -- Insert current date data
									  BEGIN
											  INSERT INTO TempTable
											  SELECT RIGHT('000' + CAST([EmployeeCardId] AS NVARCHAR(10)), 4)
												    ,CAST([TransactionDateTime] AS DATETIME) 											  
											  FROM [dbo].[EmployeeDailyAttendance]
											  WHERE CAST([TransactionDateTime] AS DATE) = @date AND LEN([EmployeeCardId]) <= 4

											  INSERT INTO TempTable
											  SELECT [EmployeeCardId]
												    ,CAST([TransactionDateTime] AS DATETIME) 											  
											  FROM [dbo].[EmployeeDailyAttendance]
											  WHERE CAST([TransactionDateTime] AS DATE) = @date AND LEN([EmployeeCardId]) = 6
									  END
									  


									  UPDATE TempTable   -- For data alignment 
									  SET 	[EmployeeCardId] = [EmployeeCardId] + '  '						
									  WHERE LEN([EmployeeCardId]) = 4																																
											
																																																					 						 
									  SELECT Table2.[EmployeeCardId] + '  ' + CONVERT(varchar, Table2.TransactionDateTime, 120)	AS RawData	
									   
									  FROM TempTable AS Table2
									  LEFT JOIN EmployeeDailyAttendanceAudit ON EmployeeDailyAttendanceAudit.EmployeeCardId = Table2.EmployeeCardId 
									  AND EmployeeDailyAttendanceAudit.[TransactionDateTime]  =  Table2.TransactionDateTime
									  WHERE Table2.TransactionDateTime IS NOT NULL AND EmployeeDailyAttendanceAudit.[TransactionDateTime] IS NULL 
									  												 
									  ORDER BY Table2.TransactionDateTime DESC
													 					  					  														  						  											  							
END