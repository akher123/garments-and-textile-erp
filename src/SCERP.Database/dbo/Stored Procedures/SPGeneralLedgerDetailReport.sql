
-- ===========================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <14/09/2015>
-- Description:	<> exec SPGeneralLedgerDetailReport '1020601000', 1, 1, '01/01/2015','11/15/2015'
-- ===========================================================================================


CREATE PROCEDURE [dbo].[SPGeneralLedgerDetailReport]


						 @AccountCode NVARCHAR(10)
						,@SectorId INT
						,@CostCentreId INT						
						,@FromDate DATETIME
						,@ToDate DATETIME
						 
AS

BEGIN
	
						 SET NOCOUNT ON;


						 DECLARE @GLName NVARCHAR(MAX)
							    ,@GLId INT


						 SELECT @GLId = Acc_GLAccounts.Id FROM Acc_GLAccounts WHERE Acc_GLAccounts.AccountCode = @AccountCode

						 SELECT @GLName = Acc_GLAccounts.AccountName FROM Acc_GLAccounts WHERE Acc_GLAccounts.Id = @GLId
										
						 SELECT 
						 Acc_CompanySector.SectorName
						,Acc_CostCentre.CostCentreName
						,Acc_VoucherMaster.VoucherNo	
						,Acc_VoucherMaster.VoucherRefNo
						,@GLName AS GLNameSearch
						,Acc_GLAccounts.AccountName AS GLName			
						,Acc_VoucherDetail.Debit
						,Acc_VoucherDetail.Credit
						,Acc_VoucherDetail.FirstCurValue
						,Acc_VoucherDetail.SecendCurValue
						,Acc_VoucherDetail.ThirdCurValue
						,CONVERT(VARCHAR(10),Acc_VoucherMaster.VoucherDate, 103) AS VoucherDate
						,CONVERT(VARCHAR(10),@FromDate, 103) AS FromDate	
						,CONVERT(VARCHAR(10),@ToDate, 103) AS ToDate
						,0.00 as Balance
						,Acc_VoucherMaster.Particulars
		
						FROM Acc_VoucherMaster
						LEFT JOIN Acc_VoucherDetail ON Acc_VoucherMaster.Id = Acc_VoucherDetail.RefId
						LEFT JOIN Acc_GLAccounts ON Acc_GLAccounts.Id = Acc_VoucherDetail.GLID
						LEFT JOIN Acc_CompanySector ON Acc_CompanySector.Id = Acc_VoucherMaster.SectorId
						LEFT JOIN Acc_CostCentre ON Acc_CostCentre.Id = Acc_VoucherMaster.CostCentreId
						WHERE Acc_VoucherMaster.VoucherNo IN
						(
							SELECT Acc_VoucherMaster.VoucherNo FROM Acc_VoucherMaster
							JOIN Acc_VoucherDetail ON Acc_VoucherMaster.Id = Acc_VoucherDetail.RefId
							WHERE Acc_VoucherDetail.GLID = @GLId
						)
						AND Acc_VoucherDetail.GLID <> @GLId 
						AND Acc_VoucherMaster.SectorId = @SectorId
						--AND Acc_VoucherMaster.CostCentreId = @CostCentreId
						AND Acc_VoucherMaster.VoucherDate BETWEEN @FromDate AND @ToDate

						ORDER BY Acc_VoucherMaster.VoucherDate, Acc_VoucherMaster.VoucherNo
																				 													
END








