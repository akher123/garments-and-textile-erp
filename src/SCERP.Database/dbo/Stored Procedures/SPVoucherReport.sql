
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <04/04/2015>
-- Description:	<> exec SPVoucherReport 10674
-- =============================================
CREATE PROCEDURE [dbo].[SPVoucherReport]


								 @Id BIGINT
						 
AS

BEGIN
	
	SET NOCOUNT ON;

																					
			SELECT				 Acc_VoucherMaster.Id
								,Acc_VoucherMaster.VoucherType
								,Acc_VoucherMaster.VoucherNo
								,Acc_VoucherMaster.VoucherRefNo
								,CONVERT(VARCHAR(10), Acc_VoucherMaster.VoucherDate, 103) AS VoucherDate
								,Acc_VoucherMaster.CheckNo
								,CONVERT(VARCHAR(10), Acc_VoucherMaster.CheckDate, 103) AS CheckDate
								,Acc_VoucherMaster.Particulars
								,Acc_VoucherDetail.GLID
								,Acc_VoucherDetail.Particulars AS DetailParticulars
								,CAST(Acc_VoucherDetail.Debit AS DECIMAL(18,5)) AS Debit
								,CAST(Acc_VoucherDetail.Credit AS DECIMAL(18,5)) AS Credit	
								,Acc_VoucherMaster.TotalAmountInWord
								,Acc_GLAccounts.ControlCode
								,Acc_GLAccounts.AccountCode
								,Acc_GLAccounts.AccountName
								,Acc_CompanySector.SectorCode
								,Acc_CompanySector.SectorName
								,Acc_CostCentre.CostCentreCode
								,Acc_CostCentre.CostCentreName
								,'2015' AS  PeriodName
								,'2015-01-01' AS PeriodStartDate
								,'2015-12-31' AS PeriodEndDate
								,'' AS Address																

			FROM				Acc_VoucherMaster INNER JOIN
								Acc_VoucherDetail ON Acc_VoucherMaster.Id = Acc_VoucherDetail.RefId INNER JOIN
								Acc_CompanySector ON Acc_VoucherMaster.SectorId = Acc_CompanySector.Id INNER JOIN
								Acc_CostCentre ON Acc_CompanySector.Id = Acc_CostCentre.SectorId INNER JOIN								
								Acc_GLAccounts ON Acc_VoucherDetail.GLID = Acc_GLAccounts.Id
						
								WHERE 	Acc_VoucherMaster.Id = @Id
								ORDER BY Acc_VoucherDetail.Debit DESC
																	 													
END








