-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <07/09/2016>
-- Description:	<> EXEC SPSalesContact 193
-- ===================================================================================

CREATE PROCEDURE [dbo].[SPSalesContact]
			
 @LcId INT
	                            
								 
								 		
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
	 
	   Lc.[LcId]
      ,Lc.[LcNo]
      ,Lc.[LcDate]
      ,Lc.[BuyerId]
      ,Lc.[LcAmount]
      ,Lc.[LcQuantity]
      ,Lc.[MatureDate]
      ,Lc.[ExpiryDate]
      ,Lc.[ExtensionDate]
      ,Lc.[ShipmentDate]
      ,Lc.[LcIssuingBank]
      ,Lc.[LcIssuingBankAddress]
      ,Lc.[ReceivingBank]
      ,Lc.[ReceivingBankAddress]
      ,Lc.[ReceivingBankId]
      ,Lc.[SalesContactNo]
      ,Lc.[LcType]
      ,Lc.[Beneficary]
      ,Lc.[PartialShipment]
      ,Lc.[Description]
      ,Lc.[AppliedDate]
      ,Lc.[IncentiveClaimValue]
      ,Lc.[NewMarketCliam]
      ,Lc.[BkmeaCertificat]
      ,Lc.[FirstAuditStatus]
      ,Lc.[CertificateOvservation]
      ,Lc.[FinalClaimAmount]
      ,Lc.[ReceiveAmount]
      ,Lc.[ReceiveDate]
      ,Lc.[CashIncentiveRemarks]
      ,Lc.[RStatus]
      ,Lc.[CommissionPrc]
      ,Lc.[CommissionsAmount]
      ,Lc.[FreightAmount]
	  ,Sc.[SalseContactId]
      ,Sc.[LcId]
      ,Sc.[LcNo]
      ,Sc.[LcDate]
      ,Sc.[BuyerId]
      ,Sc.[Amount]
      ,Sc.[Quantity]
      ,Sc.[MatureDate]
      ,Sc.[ExpiryDate]
      ,Sc.[ExtensionDate]
      ,Sc.[ShipmentDate]
      ,Sc.[LcIssuingBank]
      ,Sc.[LcIssuingBankAddress]
      ,Sc.[ReceivingBankAddress]
      ,Sc.[ReceivingBankId]
      ,Sc.[LcType]
      ,Sc.[Description]
      ,(select sum(BbLcAmount) from CommBbLcInfo where LcRefId=@LcId) As BBLCValue
	  ,(select sum(Amount) from CommPackingCredit where LcId=@LcId) As PCAmount
	   from COMMLcInfo as Lc inner join CommSalseContact as Sc on  Lc.[LcId]=Sc.[LcId]
	   where Lc.[LcId]=@LcId
							 
						    
					  					  														  						  											  							
END