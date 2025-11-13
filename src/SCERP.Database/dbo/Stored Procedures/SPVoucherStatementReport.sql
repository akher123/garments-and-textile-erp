
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <04/04/2015>
-- Description:	<> exec SPVoucherStatementReport '','2015-09-01','2015-10-13'
-- =============================================
CREATE PROCEDURE [dbo].[SPVoucherStatementReport]


						 @VoucherType NVARCHAR(100)
						,@FromDate DATETIME
						,@ToDate DATETIME
						 
AS

BEGIN
	
						SET NOCOUNT ON;

						SELECT		 CONVERT(VARCHAR(10), M.VoucherDate, 103) AS VoucherDate
									,M.VoucherRefNo AS VoucherRefNo
									,M.VoucherNo AS   VoucherNo
									,M.VoucherType AS VoucherType 
									,M.Particulars AS Particulars
									,A.AccountCode AS AccountCode
									,A.AccountName AS AccountName
									,D.Debit as Debit
									,D.Credit AS Credit
									,CONVERT(VARCHAR(10),@FromDate, 103) AS PeriodStartDate
									,CONVERT(VARCHAR(10),@ToDate, 103) AS PeriodEndDate

									FROM Acc_VoucherMaster AS M 
									INNER JOIN Acc_VoucherDetail AS D ON M.Id = D.RefId 
									INNER JOIN Acc_GLAccounts AS A ON D.GLID = A.Id 

									WHERE(M.VoucherType = @VoucherType OR @VoucherType = '' ) 
									AND CONVERT(datetime, M.VoucherDate, 103) BETWEEN CONVERT(datetime, @FromDate, 103) AND CONVERT(datetime, @ToDate, 103)
									ORDER BY M.VoucherNo, D.Debit DESC, M.VoucherDate
																																 													
END








