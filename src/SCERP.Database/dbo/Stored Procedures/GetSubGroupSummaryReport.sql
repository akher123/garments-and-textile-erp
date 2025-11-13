-- ======================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/08/2019>
-- Description:	<> EXEC GetSubGroupSummaryReport  1, '10202', '2019-07-01', '2020-06-30'
-- ======================================================================================

CREATE PROCEDURE [dbo].[GetSubGroupSummaryReport]
			

										   @CompanySectorId			INT	
										  ,@SubGroupCode			NVARCHAR(20)	 
										  ,@FromDate				DATETIME
										  ,@ToDate					DATETIME
										  
AS

BEGIN
	
			SET NOCOUNT ON;

									DECLARE @CompanyName		NVARCHAR(100)
									DECLARE @CompanyAddress		NVARCHAR(200)
			
								  				
									SELECT @CompanyName = SectorName
										  ,@CompanyAddress = SectorAddress									
									FROM   Acc_CompanySector
								    WHERE  Id = @CompanySectorId
									

									TRUNCATE TABLE Acc_SubGroupReport						 
												
																   				
									INSERT INTO Acc_SubGroupReport
											   (CompanyName
											   ,CompanyAddress
											   ,FromDate
											   ,ToDate
											   ,SubGroupId
											   ,SubGroupCode
											   ,SubGroupName
											   ,ControlId
											   ,ControlCode
											   ,ControlName												   																		
											   ,[OpeningBalance]
											   ,[TotalDebit]
											   ,[TotalCredit]
											   ,[ClosingBalance]
											   )										 											  
									SELECT     @CompanyName
											  ,@CompanyAddress
											  ,@FromDate
											  ,@ToDate
											  ,ControlAccounts.Id
											  ,ControlAccounts.ControlCode
											  ,ControlAccounts.ControlName
										      ,Acc_ControlAccounts.Id
											  ,Acc_ControlAccounts.ControlCode
											  ,Acc_ControlAccounts.ControlName											
											  ,0
											  ,0
											  ,0
											  ,0
										  FROM Acc_ControlAccounts	
										  JOIN Acc_ControlAccounts AS ControlAccounts ON Acc_ControlAccounts.ParentCode = ControlAccounts.ControlCode					
										  WHERE Acc_ControlAccounts.IsActive = 1 
										  AND Acc_ControlAccounts.ParentCode = @SubGroupCode
											
									      
										  UPDATE Acc_SubGroupReport
										  SET OpeningBalance = TableBalance.Balance
										  FROM Acc_SubGroupReport INNER JOIN										  										  
										  (
											  SELECT Acc_GLAccounts.ControlCode
													,SUM(Debit) - SUM(Credit) AS Balance
											  FROM Acc_VoucherMaster
											  JOIN Acc_VoucherDetail ON Acc_VoucherDetail.RefId = Acc_VoucherMaster.Id
											  JOIN Acc_GLAccounts ON Acc_GLAccounts.Id = Acc_VoucherDetail.GLID

											  WHERE SectorId = @CompanySectorId AND Acc_VoucherMaster.VoucherDate < @FromDate
											  AND Acc_GLAccounts.ControlCode IN(SELECT ControlCode FROM Acc_SubGroupReport )
											  GROUP BY Acc_GLAccounts.ControlCode
										  ) TableBalance ON Acc_SubGroupReport.ControlCode = TableBalance.ControlCode


										  UPDATE Acc_SubGroupReport
										  SET TotalDebit = TableBalance.Debit
										  FROM Acc_SubGroupReport INNER JOIN										  										  
										  (
											  SELECT Acc_GLAccounts.ControlCode
													,SUM(Debit) AS Debit
											  FROM Acc_VoucherMaster
											  JOIN Acc_VoucherDetail ON Acc_VoucherDetail.RefId = Acc_VoucherMaster.Id
											  JOIN Acc_GLAccounts ON Acc_GLAccounts.Id = Acc_VoucherDetail.GLID

											  WHERE SectorId = @CompanySectorId AND Acc_VoucherMaster.VoucherDate BETWEEN @FromDate AND @ToDate
											  AND Acc_GLAccounts.ControlCode IN(SELECT ControlCode FROM Acc_SubGroupReport )
											  GROUP BY Acc_GLAccounts.ControlCode
										  ) TableBalance ON Acc_SubGroupReport.ControlCode = TableBalance.ControlCode
								

										  UPDATE Acc_SubGroupReport
										  SET TotalCredit = TableBalance.Credit
										  FROM Acc_SubGroupReport INNER JOIN										  										  
										  (
											  SELECT Acc_GLAccounts.ControlCode
													,SUM(Credit) AS Credit
											  FROM Acc_VoucherMaster
											  JOIN Acc_VoucherDetail ON Acc_VoucherDetail.RefId = Acc_VoucherMaster.Id
											  JOIN Acc_GLAccounts ON Acc_GLAccounts.Id = Acc_VoucherDetail.GLID

											  WHERE SectorId = @CompanySectorId AND Acc_VoucherMaster.VoucherDate BETWEEN @FromDate AND @ToDate
											  AND Acc_GLAccounts.ControlCode IN(SELECT ControlCode FROM Acc_SubGroupReport )
											  GROUP BY Acc_GLAccounts.ControlCode
										  ) TableBalance ON Acc_SubGroupReport.ControlCode = TableBalance.ControlCode

										  	
									      DELETE FROM [Acc_SubGroupReport] 
										  WHERE([OpeningBalance] = 0 AND [TotalDebit] = 0 AND [TotalCredit] = 0)															
										  

										SELECT   [SubGroupReportId]
												,[CompanyName]
												,[CompanyAddress]
												,[SubGroupId]
												,[SubGroupCode]
												,[SubGroupName]
												,[ControlId]
												,[ControlCode]
												,[ControlName]
												,[FromDate]
												,[ToDate]
												,[OpeningBalance]
												,[TotalDebit]
												,[TotalCredit]
												,([OpeningBalance] + [TotalDebit] - [TotalCredit])  AS [ClosingBalance]
											FROM [dbo].[Acc_SubGroupReport]										
											ORDER BY [ControlCode]				  																								  								 										
													 					  					  														  						  											  							
END