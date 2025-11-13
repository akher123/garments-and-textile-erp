-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> exec [SPCommLcInfoIndividualReport] '001', 1
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommLcInfoIndividualReport]
			

						   @CompanyId		NVARCHAR(3)	
						  ,@LcId			INT		

AS
BEGIN
	
			SET NOCOUNT ON;

					SELECT [LcId]
						  ,[LcNo]
						  ,[LcDate]
						  ,[COMMLcInfo].[BuyerId]						  
						  ,[LcAmount]
						  ,[LcQuantity]
						  ,[MatureDate]
						  ,[ExpiryDate]
						  ,[ExtensionDate]
						  ,[ShipmentDate]
						  ,[LcIssuingBank]
						  ,[LcIssuingBankAddress]
						  ,[ReceivingBank]
						  ,[ReceivingBankAddress]
						  , ReceivingBankId
						  ,[LcType]
						  ,([Beneficary] + '$' + [OM_Buyer].[BuyerName]) AS Beneficary
						  ,[PartialShipment]
						  ,SalesContactNo
						  ,[Description]
						  ,AppliedDate 
						  ,IncentiveClaimValue 
						  ,NewMarketCliam 
						  ,BTMACertificate
						  ,BkmeaCertificat 
						  ,FirstAuditStatus 
						  ,CertificateOvservation 
						  ,FinalClaimAmount 
						  ,ReceiveAmount 
						  ,ReceiveDate 
						  ,CashIncentiveRemarks 
						  ,[CreatedDate]
						  ,[CreatedBy]
						  ,[EditedDate]
						  ,[EditedBy]
						  ,[IsActive],
						     RStatus,
							  CommissionsAmount,
							  CommissionPrc,
							  FreightAmount

					  FROM [dbo].[COMMLcInfo]
					  LEFT JOIN OM_Buyer ON OM_Buyer.BuyerId = COMMLcInfo.BuyerId AND OM_Buyer.CompId = @CompanyId

					  WHERE [IsActive] = 1
					  AND  ([COMMLcInfo].LcId = @LcId)						  					  														  						  											  							
END