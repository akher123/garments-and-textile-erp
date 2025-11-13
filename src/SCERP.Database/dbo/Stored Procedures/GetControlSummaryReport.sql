-- ======================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/08/2019>
-- Description:	<> EXEC GetControlSummaryReport  1, '2030204', '2018-07-01', '2019-06-30'
-- ======================================================================================

CREATE PROCEDURE [dbo].[GetControlSummaryReport]
			

										   @CompanySectorId			INT	
										  ,@ControlCode				NVARCHAR(20)	 
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
									
									TRUNCATE TABLE Acc_ControlReport							 
																   				
									INSERT INTO Acc_ControlReport
											   (CompanyName
											   ,CompanyAddress
											   ,FromDate
											   ,ToDate
											   ,ControlId
											   ,ControlCode
											   ,ControlName										
											   ,GLAccountId
											   ,GLAccountCode
											   ,GLAccountName
											   ,[OpeningBalance]
											   ,[TotalDebit]
											   ,[TotalCredit]
											   ,[ClosingBalance]
											   )										 											  
									SELECT     @CompanyName
											  ,@CompanyAddress
											  ,@FromDate
											  ,@ToDate
										      ,Acc_ControlAccounts.Id
											  ,Acc_ControlAccounts.ControlCode
											  ,Acc_ControlAccounts.ControlName
											  ,Acc_GLAccounts.Id
											  ,Acc_GLAccounts.AccountCode
											  ,Acc_GLAccounts.AccountName
											  ,0
											  ,0
											  ,0
											  ,0
										  FROM Acc_GLAccounts
										  JOIN Acc_ControlAccounts ON Acc_GLAccounts.ControlCode = Acc_ControlAccounts.ControlCode
										  WHERE Acc_GLAccounts.IsActive = 1 
										  AND Acc_GLAccounts.ControlCode = @ControlCode
											
									      
										  UPDATE Acc_ControlReport
										  SET OpeningBalance = TableBalance.Balance
										  FROM Acc_ControlReport INNER JOIN										  										  
										  (
											  SELECT Acc_VoucherDetail.GLID
													,SUM(Debit) - SUM(Credit) AS Balance
											  FROM Acc_VoucherMaster
											  JOIN Acc_VoucherDetail ON Acc_VoucherDetail.RefId = Acc_VoucherMaster.Id
											  WHERE SectorId = @CompanySectorId AND Acc_VoucherMaster.VoucherDate < @FromDate
											  AND Acc_VoucherDetail.GLID IN(SELECT GLAccountId FROM Acc_ControlReport )
											  GROUP BY Acc_VoucherDetail.GLID
										  ) TableBalance ON Acc_ControlReport.GLAccountId = TableBalance.GLID


										  UPDATE Acc_ControlReport
										  SET TotalDebit = TableBalance.Debit
										  FROM Acc_ControlReport INNER JOIN										  										  
										  (
											  SELECT Acc_VoucherDetail.GLID
													,SUM(Debit) AS Debit
											  FROM Acc_VoucherMaster
											  JOIN Acc_VoucherDetail ON Acc_VoucherDetail.RefId = Acc_VoucherMaster.Id
											  WHERE SectorId = @CompanySectorId AND Acc_VoucherMaster.VoucherDate BETWEEN @FromDate AND @ToDate
											  AND Acc_VoucherDetail.GLID IN(SELECT GLAccountId FROM Acc_ControlReport )
											  GROUP BY Acc_VoucherDetail.GLID
										  ) TableBalance ON Acc_ControlReport.GLAccountId = TableBalance.GLID


										  UPDATE Acc_ControlReport
										  SET TotalCredit = TableBalance.Credit
										  FROM Acc_ControlReport INNER JOIN										  										  
										  (
											  SELECT Acc_VoucherDetail.GLID
													,SUM(Credit) AS Credit
											  FROM Acc_VoucherMaster
											  JOIN Acc_VoucherDetail ON Acc_VoucherDetail.RefId = Acc_VoucherMaster.Id
											  WHERE SectorId = @CompanySectorId AND Acc_VoucherMaster.VoucherDate BETWEEN @FromDate AND @ToDate
											  AND Acc_VoucherDetail.GLID IN(SELECT GLAccountId FROM Acc_ControlReport )
											  GROUP BY Acc_VoucherDetail.GLID
										  ) TableBalance ON Acc_ControlReport.GLAccountId = TableBalance.GLID


									    DELETE FROM [Acc_ControlReport] 
										WHERE([OpeningBalance] = 0 AND [TotalDebit] = 0 AND [TotalCredit] = 0)
									

										SELECT [ControlReportId]
											  ,[CompanyName]
											  ,[CompanyAddress]
											  ,[ControlId]
											  ,[ControlCode]
											  ,[ControlName]
											  ,[FromDate]
											  ,[ToDate]
											  ,[GLAccountId]
											  ,[GLAccountCode]
											  ,[GLAccountName]
											  ,[OpeningBalance]
											  ,[TotalDebit]
											  ,[TotalCredit]
											  ,([OpeningBalance] + [TotalDebit] - [TotalCredit])  AS [ClosingBalance]
										  FROM [dbo].[Acc_ControlReport]				
										  ORDER BY [GLAccountName]						  																								  								 										
													 					  					  														  						  											  							
END