
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <04/04/2015>
-- Description:	<> exec SPVoucherSummaryReport '','2015-09-13','2015-09-13'
-- =============================================
CREATE PROCEDURE [dbo].[SPVoucherSummaryReport]


						 @VoucherType NVARCHAR(100)
						,@FromDate DATETIME
						,@ToDate DATETIME
						 
AS

BEGIN
	
						SET NOCOUNT ON;

																							
						SELECT    A.AccountCode AS AccountCode						
								 ,A.AccountName AS AccountName
								 ,Sum(D.Debit) AS Debit
								 ,Sum(D.Credit) AS Credit
								 ,M.VoucherType AS VoucherType
								 ,CONVERT(VARCHAR(10),@FromDate, 103) AS PeriodStartDate
								 ,CONVERT(VARCHAR(10),@ToDate, 103) AS PeriodEndDate

								  FROM Acc_VoucherMaster AS M 
								  INNER JOIN Acc_VoucherDetail AS D ON M.Id = D.RefId 
								  INNER JOIN Acc_GLAccounts AS A ON D.GLID = A.Id 
								  
								  WHERE (M.VoucherType = @VoucherType OR @VoucherType = '') 							
								  AND CONVERT(datetime, M.VoucherDate, 103) BETWEEN CONVERT(datetime, @FromDate, 103) AND CONVERT(datetime, @ToDate, 103)
								  
								  Group by A.AccountCode , A.AccountName, M.VoucherType	
								  ORDER BY Debit DESC, AccountCode, VoucherType
								 		
																																																	 													
END








