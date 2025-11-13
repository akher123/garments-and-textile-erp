-- =====================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <14/01/2020>
-- Description:	<> EXEC  GetAgingReport '2020-01-14', 1
-- =====================================================

CREATE PROCEDURE [dbo].[GetAgingReport]
												
																		
								    @FromDate			DATE 
								   ,@CompanySectorId	INT
							
																		   
AS

BEGIN
	
			SET NOCOUNT ON;
					  		
							DECLARE @CurrentMonthFromDate  DATE = DATEADD(DAY, -30, @FromDate)
							DECLARE @CurrentMonthToDate	   DATE = @FromDate
							DECLARE @FirstMonthFromDate	   DATE = DATEADD(DAY, -60, @FromDate)
							DECLARE @FirstMonthToDate	   DATE = DATEADD(DAY, -31, @FromDate)
							DECLARE @SecondMonthFromDate   DATE = DATEADD(DAY, -90, @FromDate)
							DECLARE @SecondMonthToDate	   DATE = DATEADD(DAY, -61, @FromDate)
							DECLARE @ThirdMonthFromDate	   DATE = DATEADD(DAY, -90, @FromDate)
						

							TRUNCATE TABLE [Acc_ReportAging]
							
							INSERT INTO [dbo].[Acc_ReportAging]
											 ([PartyName]
											 ,[PartyGLId]
											 ,[PartyGLCode]
											 ,[PartyControlCode]
											 ,IsChanged																				
											 ,[IsActive])											   							           
									   SELECT [AccountName]
									         ,Id
										     ,[AccountCode]
										     ,[ControlCode]
											 ,1
											 ,1
									     FROM [dbo].[Acc_GLAccounts]
									    WHERE ControlCode = '1020460' AND IsActive = 1
										
									  									  
									UPDATE [dbo].[Acc_ReportAging]   
									   SET [CurrentMonth] = 0
										  ,[FirstMonth] = 0
										  ,[SecondMonth] = 0
										  ,[ThirdMonth] = 0
										  ,[TotalAmount] = 0
										  ,[TotalDisbursed] = 0

									  -- Current month data insert --
									  UPDATE [Acc_ReportAging]
									  SET [CurrentMonth] = ISNULL(Debit, 0)
									  FROM [Acc_ReportAging]
									  LEFT JOIN 								 
									  (SELECT [Acc_VoucherDetail].[GLID], SUM(Debit) AS Debit										
									  FROM [dbo].[Acc_VoucherMaster]
									  JOIN [Acc_VoucherDetail] ON [Acc_VoucherDetail].[RefId] = [Acc_VoucherMaster].[Id]
									  WHERE [Acc_VoucherMaster].VoucherDate BETWEEN @CurrentMonthFromDate AND @CurrentMonthToDate
									  --AND [Acc_VoucherMaster].VoucherType IN('BR', 'CR')
									  GROUP BY [Acc_VoucherDetail].[GLID]) TempTable ON TempTable.[GLID] = [Acc_ReportAging].[PartyGLId]

									  -- First month data insert --
									  UPDATE [Acc_ReportAging]
									  SET FirstMonth = ISNULL(Debit, 0)
									  FROM [Acc_ReportAging]
									  LEFT JOIN 								 
									  (SELECT [Acc_VoucherDetail].[GLID], SUM(Debit) AS Debit										
									  FROM [dbo].[Acc_VoucherMaster]
									  JOIN [Acc_VoucherDetail] ON [Acc_VoucherDetail].[RefId] = [Acc_VoucherMaster].[Id]
									  WHERE [Acc_VoucherMaster].VoucherDate BETWEEN @FirstMonthFromDate AND @FirstMonthToDate
									  GROUP BY [Acc_VoucherDetail].[GLID]) TempTable ON TempTable.[GLID] = [Acc_ReportAging].[PartyGLId]

									   -- Second month data insert --
									  UPDATE [Acc_ReportAging]
									  SET SecondMonth = ISNULL(Debit, 0)
									  FROM [Acc_ReportAging]
									  LEFT JOIN 								 
									  (SELECT [Acc_VoucherDetail].[GLID], SUM(Debit) AS Debit										
									  FROM [dbo].[Acc_VoucherMaster]
									  JOIN [Acc_VoucherDetail] ON [Acc_VoucherDetail].[RefId] = [Acc_VoucherMaster].[Id]
									  WHERE [Acc_VoucherMaster].VoucherDate BETWEEN @SecondMonthFromDate AND @SecondMonthToDate
									  GROUP BY [Acc_VoucherDetail].[GLID]) TempTable ON TempTable.[GLID] = [Acc_ReportAging].[PartyGLId]

									    -- Third month data insert --
									  UPDATE [Acc_ReportAging]
									  SET ThirdMonth = ISNULL(Debit, 0)
									  FROM [Acc_ReportAging]
									  LEFT JOIN 								 
									  (SELECT [Acc_VoucherDetail].[GLID], SUM(Debit) AS Debit										
									  FROM [dbo].[Acc_VoucherMaster]
									  JOIN [Acc_VoucherDetail] ON [Acc_VoucherDetail].[RefId] = [Acc_VoucherMaster].[Id]
									  WHERE [Acc_VoucherMaster].VoucherDate < @ThirdMonthFromDate
									  GROUP BY [Acc_VoucherDetail].[GLID]) TempTable ON TempTable.[GLID] = [Acc_ReportAging].[PartyGLId]

									  -- Account Received --
									  UPDATE [Acc_ReportAging]
									  SET TotalDisbursed = ISNULL(Credit, 0)
									  FROM [Acc_ReportAging]
									  LEFT JOIN 								 
									  (SELECT [Acc_VoucherDetail].[GLID], SUM(Credit) AS Credit										
									  FROM [dbo].[Acc_VoucherMaster]
									  JOIN [Acc_VoucherDetail] ON [Acc_VoucherDetail].[RefId] = [Acc_VoucherMaster].[Id]
									  WHERE [Acc_VoucherMaster].VoucherDate <= @CurrentMonthToDate
									  GROUP BY [Acc_VoucherDetail].[GLID]) TempTable ON TempTable.[GLID] = [Acc_ReportAging].[PartyGLId]

									  --UPDATE [dbo].[Acc_ReportAging]   
									  -- SET [CurrentMonth] = 500
										 -- ,[FirstMonth] = 1000
										 -- ,[SecondMonth] = 2000
										 -- ,[ThirdMonth] = 1000
										 -- ,[TotalAmount] = 4500
										 -- ,[TotalDisbursed] = 5000
										 -- ,IsChanged = 1

									   -----------------

									   UPDATE Acc_ReportAging
									   SET ThirdMonth = 0
									      ,TotalDisbursed = TotalDisbursed - ThirdMonth
										  ,IsChanged = 0
									   WHERE ThirdMonth <= TotalDisbursed					

									   UPDATE [Acc_ReportAging]
									   SET [TotalDisbursed] = 0
									      ,ThirdMonth = ThirdMonth - [TotalDisbursed]
									   WHERE [TotalDisbursed] < ThirdMonth AND IsChanged = 1

									   ------------------

									   UPDATE [Acc_ReportAging]
									   SET IsChanged = 1

									   UPDATE [Acc_ReportAging]
									   SET [SecondMonth] = 0
									      ,[TotalDisbursed] = [TotalDisbursed] - [SecondMonth]
										  ,IsChanged = 0
									   WHERE [SecondMonth] <= [TotalDisbursed]

									   UPDATE [Acc_ReportAging]
									   SET [TotalDisbursed] = 0
									      ,[SecondMonth] = [SecondMonth] - [TotalDisbursed]
									   WHERE [TotalDisbursed] < [SecondMonth] AND IsChanged = 1

									   -------------------
									   UPDATE [Acc_ReportAging]
									   SET IsChanged = 1

									   UPDATE [Acc_ReportAging]
									   SET [FirstMonth] = 0
									      ,[TotalDisbursed] = [TotalDisbursed] - [FirstMonth]
										  ,IsChanged = 0
									   WHERE [FirstMonth] <= [TotalDisbursed]

									   UPDATE [Acc_ReportAging]
									   SET [TotalDisbursed] = 0
									      ,[FirstMonth] = [FirstMonth] - [TotalDisbursed]
									   WHERE [TotalDisbursed] < [FirstMonth] AND IsChanged = 1

									   -------

									   UPDATE [Acc_ReportAging]
									   SET IsChanged = 1

									   UPDATE [Acc_ReportAging]
									   SET CurrentMonth = 0
									      ,[TotalDisbursed] = [TotalDisbursed] - CurrentMonth
										  ,IsChanged = 0
									   WHERE CurrentMonth <= [TotalDisbursed]

									   UPDATE [Acc_ReportAging]
									   SET [TotalDisbursed] = 0
									      ,CurrentMonth = CurrentMonth - [TotalDisbursed]
									   WHERE [TotalDisbursed] < CurrentMonth AND IsChanged = 1

									   -------------------

										SELECT [SerialId]
											  ,[PartyName]
											  ,[PartyGLId]
											  ,[PartyGLCode]
											  ,[PartyControlCode]
											  ,[CurrentMonth]
											  ,[FirstMonth]
											  ,[SecondMonth]
											  ,[ThirdMonth]
											  ,[TotalAmount]
											  ,[TotalDisbursed]
											  ,IsChanged
											  ,[CreatedDate]
											  ,[CreatedBy]
											  ,[EditedDate]
											  ,[EditedBy]
											  ,[IsActive]
										  FROM [dbo].[Acc_ReportAging]
										  ORDER BY [PartyName]
										  							  					  														  						  											  							
END