-- ===============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <27/11/2019>
-- Description:	<> EXEC GetSubGroupTrialBalance  1, 38, '2019-07-01', '2019-10-31'
-- ===============================================================================

CREATE PROCEDURE [dbo].[GetSubGroupTrialBalance]
			

										   @CompanySectorId			INT	
										  ,@FiscalPeriodId			INT							 
										  ,@FromDate				DATETIME
										  ,@ToDate					DATETIME
						   
AS

BEGIN
	
			SET NOCOUNT ON;

									DECLARE @CompanyName				NVARCHAR(100)
									DECLARE @CompanyAddress				NVARCHAR(200)
									DECLARE @FiscalStartDate			DATE
									DECLARE @PreviousIncomeExpense		DECIMAL(18,2)
									
									 
									SELECT @FiscalStartDate = [PeriodStartDate]      
									FROM [dbo].[Acc_FinancialPeriod]
									WHERE Id = @FiscalPeriodId
											
															  				
									SELECT @CompanyName = SectorName
										  ,@CompanyAddress = SectorAddress									
									FROM   Acc_CompanySector
								    WHERE  Id = @CompanySectorId
									

									TRUNCATE TABLE Acc_ReportControlTrialBalance							 
											
																   				
									INSERT INTO [dbo].[Acc_ReportControlTrialBalance]
												   ([GroupCode]
												   ,[GroupName]
												   ,[SubGroupCode]
												   ,[SubGroupName]
												   ,[SubControlCode]
												   ,[SubControlName]
												   ,[ControlCode]
												   ,[ControlName]
												   ,[GLId]
												   ,[GLCode]
												   ,[GLName]
												   ,[OpeningBalance]
												   ,[Debit]
												   ,[Credit]
												   ,[ClosingBalance]
												   ,[IsActive])
												SELECT 
													Acc_ControlAccounts4.ControlCode
												   ,Acc_ControlAccounts4.ControlName
												   ,Acc_ControlAccounts3.ControlCode
												   ,Acc_ControlAccounts3.ControlName
												   ,Acc_ControlAccounts2.ControlCode
												   ,Acc_ControlAccounts2.ControlName
												   ,Acc_ControlAccounts.ControlCode
												   ,Acc_ControlAccounts.ControlName
												   ,Acc_GLAccounts.Id
												   ,Acc_GLAccounts.AccountCode
												   ,Acc_GLAccounts.AccountName
												   ,0
												   ,0
												   ,0
												   ,0
												   ,1
										  FROM Acc_ControlAccounts
										  JOIN Acc_GLAccounts ON Acc_GLAccounts.ControlCode = Acc_ControlAccounts.ControlCode
										  JOIN Acc_ControlAccounts AS Acc_ControlAccounts2 ON Acc_ControlAccounts.ParentCode = Acc_ControlAccounts2.ControlCode
										  JOIN Acc_ControlAccounts AS Acc_ControlAccounts3 ON Acc_ControlAccounts2.ParentCode = Acc_ControlAccounts3.ControlCode
										  JOIN Acc_ControlAccounts AS Acc_ControlAccounts4 ON Acc_ControlAccounts3.ParentCode = Acc_ControlAccounts4.ControlCode
										  WHERE Acc_ControlAccounts.ControlLevel = 4 AND Acc_ControlAccounts.IsActive = 1
									      

										  UPDATE Acc_ReportControlTrialBalance
										  SET OpeningBalance = TableBalance.Balance
										  FROM Acc_ReportControlTrialBalance INNER JOIN										  										  
										  (
											  SELECT Acc_VoucherDetail.GLID
													,SUM(Debit) - SUM(Credit) AS Balance
											  FROM Acc_VoucherMaster
											  JOIN Acc_VoucherDetail ON Acc_VoucherDetail.RefId = Acc_VoucherMaster.Id
											  WHERE SectorId = @CompanySectorId AND Acc_VoucherMaster.VoucherDate < @FromDate
											  AND Acc_VoucherDetail.GLID IN(SELECT GLId FROM Acc_ReportControlTrialBalance )
											  GROUP BY Acc_VoucherDetail.GLID
										  ) TableBalance ON Acc_ReportControlTrialBalance.GLId = TableBalance.GLID
										

										  UPDATE Acc_ReportControlTrialBalance
										  SET OpeningBalance =  ISNULL((SELECT SUM(Debit) - SUM(Credit) AS Balance																		
																FROM Acc_VoucherMaster
																JOIN Acc_VoucherDetail ON Acc_VoucherDetail.RefId = Acc_VoucherMaster.Id
																WHERE SectorId = 1 AND Acc_VoucherMaster.VoucherDate >= @FiscalStartDate AND Acc_VoucherMaster.VoucherDate < @FromDate
																AND GLID = Acc_ReportControlTrialBalance.GLId),0)
										  WHERE GroupCode IN(3,4)


										  UPDATE Acc_ReportControlTrialBalance
										  SET Debit = TableBalance.Debit
										  FROM Acc_ReportControlTrialBalance INNER JOIN										  										  
										  (
											  SELECT Acc_VoucherDetail.GLID
													,SUM(Debit) AS Debit
											  FROM Acc_VoucherMaster
											  JOIN Acc_VoucherDetail ON Acc_VoucherDetail.RefId = Acc_VoucherMaster.Id
											  WHERE SectorId = @CompanySectorId AND Acc_VoucherMaster.VoucherDate BETWEEN @FromDate AND @ToDate
											  AND Acc_VoucherDetail.GLID IN(SELECT GLId FROM Acc_ReportControlTrialBalance )
											  GROUP BY Acc_VoucherDetail.GLID
										  ) TableBalance ON Acc_ReportControlTrialBalance.GLId = TableBalance.GLID


										  UPDATE Acc_ReportControlTrialBalance
										  SET Credit = TableBalance.Credit
										  FROM Acc_ReportControlTrialBalance INNER JOIN										  										  
										  (
											  SELECT Acc_VoucherDetail.GLID
													,SUM(Credit) AS Credit
											  FROM Acc_VoucherMaster
											  JOIN Acc_VoucherDetail ON Acc_VoucherDetail.RefId = Acc_VoucherMaster.Id
											  WHERE SectorId = @CompanySectorId AND Acc_VoucherMaster.VoucherDate BETWEEN @FromDate AND @ToDate
											  AND Acc_VoucherDetail.GLID IN(SELECT GLId FROM Acc_ReportControlTrialBalance )
											  GROUP BY Acc_VoucherDetail.GLID
										  ) TableBalance ON Acc_ReportControlTrialBalance.GLId = TableBalance.GLID
										

										 -- Previous Income and Expense sum add to General Reserve (GLID = 6)
										  SELECT @PreviousIncomeExpense = (SUM(Debit) - SUM(Credit))																							
										  FROM Acc_ControlAccounts
										  JOIN Acc_GLAccounts ON Acc_GLAccounts.ControlCode = Acc_ControlAccounts.ControlCode
										  JOIN Acc_VoucherDetail ON Acc_VoucherDetail.GLID = Acc_GLAccounts.Id
										  JOIN Acc_VoucherMaster ON Acc_VoucherMaster.Id = Acc_VoucherDetail.RefId
										  JOIN Acc_ControlAccounts AS Acc_ControlAccounts2 ON Acc_ControlAccounts.ParentCode = Acc_ControlAccounts2.ControlCode
										  JOIN Acc_ControlAccounts AS Acc_ControlAccounts3 ON Acc_ControlAccounts2.ParentCode = Acc_ControlAccounts3.ControlCode
										  JOIN Acc_ControlAccounts AS Acc_ControlAccounts4 ON Acc_ControlAccounts3.ParentCode = Acc_ControlAccounts4.ControlCode
										  WHERE Acc_ControlAccounts.ControlLevel = 4 AND Acc_ControlAccounts.IsActive = 1
										  AND Acc_ControlAccounts4.ControlCode IN(3,4) AND Acc_VoucherMaster.VoucherDate < @FiscalStartDate
							

										  UPDATE [Acc_ReportControlTrialBalance]
										  SET OpeningBalance = OpeningBalance + @PreviousIncomeExpense
										  WHERE [Acc_ReportControlTrialBalance].GLId = 6


									      DELETE FROM [Acc_ReportControlTrialBalance] 
										  WHERE([OpeningBalance] = 0 AND Debit = 0 AND Credit = 0)									
										  

										  UPDATE [Acc_ReportControlTrialBalance]
										  SET [ClosingBalance] = ([OpeningBalance] + [Debit] - [Credit])
																										  										  
										 									 										  	  																																					  								 																				 
										  SELECT   @CompanyName      AS CompanyName
												  ,@CompanyAddress   AS CompanyAddress
												  ,CONVERT(VARCHAR, @FromDate, 103)  AS FromDate
												  ,CONVERT(VARCHAR, @ToDate, 103)    AS ToDate										
												  ,[GroupCode]
												  ,[GroupName]
												  ,[SubGroupCode]
												  ,[SubGroupName]
												  ,[SubControlCode]
												  ,[SubControlName]																						
												  ,SUM([OpeningBalance]) AS [OpeningBalance]
												  ,SUM([Debit]) AS [Debit]
												  ,SUM([Credit]) AS [Credit]
												  ,SUM([ClosingBalance]) AS [ClosingBalance]	 								
										     FROM  [dbo].[Acc_ReportControlTrialBalance]
									     GROUP BY  [GroupCode]
												  ,[GroupName]
												  ,[SubGroupCode]
												  ,[SubGroupName]
												  ,[SubControlCode]
												  ,[SubControlName]													
										 ORDER  BY [GroupName],[SubGroupName],[SubControlName]
											  	 					  					  														  						  											  							
END