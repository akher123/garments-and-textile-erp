
-- =================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2018-10-10>
-- Description:	<> EXEC [dbo].[SPEmployeeAttendanceIndividual] '43CB0E34-9226-451C-A9B1-25C78DA35477', '2018-10-12'
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[SPEmployeeAttendanceIndividual]
									
																		
									 @EmployeeId UNIQUEIDENTIFIER
									,@UpToDate DATETIME = '1900-01-01'
									
AS

BEGIN
									DECLARE @EmployeeCardId VARCHAR(50)
									
									SELECT TOP(1) @EmployeeCardId = EmployeeCardId FROM Employee WHERE EmployeeId = @EmployeeId

									CREATE TABLE #EmployeeInOut
									( 
										 EmployeeCardId			NVARCHAR(50)
										,TransactionDate		DATETIME
										,Status					NVARCHAR(50)	  
									)

									INSERT INTO #EmployeeInOut
									SELECT EmployeeCardId  
										  ,TransactionDate
										  ,Status
									FROM EmployeeInout
									WHERE EmployeeCardId = @EmployeeCardId


									SELECT Year 
										  ,EmployeeCardId
										  ,(SELECT COUNT(Status) FROM #EmployeeInOut WHERE #EmployeeInOut.EmployeeCardId = Table1.EmployeeCardId AND YEAR(#EmployeeInOut.TransactionDate) = Table1.Year AND #EmployeeInOut.Status IN('Present', 'Late','OSD' )) AS Present
										  ,(SELECT COUNT(Status) FROM #EmployeeInOut WHERE #EmployeeInOut.EmployeeCardId = Table1.EmployeeCardId AND YEAR(#EmployeeInOut.TransactionDate) = Table1.Year AND #EmployeeInOut.Status IN('Late'))					AS Late
										  ,(SELECT COUNT(Status) FROM #EmployeeInOut WHERE #EmployeeInOut.EmployeeCardId = Table1.EmployeeCardId AND YEAR(#EmployeeInOut.TransactionDate) = Table1.Year AND #EmployeeInOut.Status IN('Absent'))					AS Absent
										  ,(SELECT COUNT(Status) FROM #EmployeeInOut WHERE #EmployeeInOut.EmployeeCardId = Table1.EmployeeCardId AND YEAR(#EmployeeInOut.TransactionDate) = Table1.Year AND #EmployeeInOut.Status IN('Leave'))					AS Leave
									FROM 

								   (SELECT YEAR(TransactionDate) AS Year
										 ,EmployeeCardId	 
										  FROM #EmployeeInOut
										  WHERE EmployeeCardId = @EmployeeCardId
										  GROUP BY YEAR(TransactionDate), EmployeeCardId
								   ) AS Table1 
								     ORDER BY Year DESC

END