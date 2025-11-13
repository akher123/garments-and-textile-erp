
-- ===========================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <14/09/2015>
-- Description:	<> exec SPGetReceivePaymentDataReport  1, '10206','10207', '02/09/2015','13/10/2015'
-- ===========================================================================================
 
CREATE PROCEDURE [dbo].[SPGetReceivePaymentDataReport]
						

						 @SectorId			INT		
						,@ControlCode1		NVARCHAR(20)
						,@ControlCode2		NVARCHAR(20)					
						,@FromDate			NVARCHAR(50)
						,@ToDate			NVARCHAR(50)					 
AS

BEGIN
	
						SET NOCOUNT ON;
															
						SELECT 
															
					    Acc_ControlAccounts.ControlName
					   ,Acc_GLAccounts.AccountName
					   ,SUM(Acc_VoucherDetail.Debit) AS Debit
					   ,SUM(Acc_VoucherDetail.Credit) AS Credit
							
						FROM Acc_VoucherMaster
						LEFT JOIN Acc_VoucherDetail ON Acc_VoucherMaster.Id = Acc_VoucherDetail.RefId
						LEFT JOIN Acc_GLAccounts ON Acc_GLAccounts.Id = Acc_VoucherDetail.GLID
						LEFT JOIN Acc_ControlAccounts ON Acc_ControlAccounts.ControlCode = Acc_GLAccounts.ControlCode
						LEFT JOIN Acc_CompanySector ON Acc_CompanySector.Id = Acc_VoucherMaster.SectorId
						LEFT JOIN Acc_CostCentre ON Acc_CostCentre.Id = Acc_VoucherMaster.CostCentreId
						WHERE Acc_VoucherMaster.VoucherNo IN
						(
							SELECT Acc_VoucherMaster.VoucherNo 
							FROM Acc_VoucherMaster
							LEFT JOIN Acc_VoucherDetail ON Acc_VoucherMaster.Id = Acc_VoucherDetail.RefId
							LEFT JOIN Acc_GLAccounts ON Acc_GLAccounts.Id = Acc_VoucherDetail.GLID							
							WHERE Acc_GLAccounts.AccountCode LIKE @ControlCode1+'%' OR Acc_GLAccounts.AccountCode LIKE @ControlCode2+'%'
						)
						AND Acc_GLAccounts.AccountCode NOT LIKE @ControlCode1+'%' AND Acc_GLAccounts.AccountCode NOT LIKE @ControlCode2+'%'				        						
						AND Acc_VoucherMaster.SectorId = @SectorId					
						AND Acc_VoucherMaster.VoucherDate BETWEEN CONVERT(date, @FromDate, 103) AND CONVERT(date, @ToDate, 103)
						GROUP BY Acc_VoucherDetail.GLID, Acc_ControlAccounts.ControlName, Acc_GLAccounts.AccountName
																										 													
END



