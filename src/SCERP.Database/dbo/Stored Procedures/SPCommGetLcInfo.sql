-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> exec SPCommGetLcInfo '001', 0, 0, '01/01/2000', '01/01/2000', '', '',''
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommGetLcInfo]
			

						   @CompanyId		NVARCHAR(3)	
						  ,@BuyerId			BIGINT						 
						  ,@LcType		    INT
						  ,@FromDate		DATETIME
						  ,@ToDate			DATETIME
						  ,@LcBank			NVARCHAR(100)	
						  ,@ReceiveBank		NVARCHAR(100)
						  ,@LCNO		    NVARCHAR(100)		   
AS
BEGIN
	
			SET NOCOUNT ON;

						   SELECT 
						   [LcId]
						  ,[LcNo]
						  ,[LcDate]
						  ,[COMMLcInfo].[BuyerId]
						  ,[COMMLcInfo].ReceivingBankId	AS ReceivingBankId		  
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
						  ,[LcType]
						  ,([Beneficary] + '$' + [OM_Buyer].[BuyerName]) AS Beneficary
						  ,[PartialShipment]
						  ,SalesContactNo
						  ,[Description],
						  AppliedDate,
						   IncentiveClaimValue,
						    NewMarketCliam,
							 BkmeaCertificat, 
							 FirstAuditStatus,
							  CertificateOvservation,
							   FinalClaimAmount,
							    ReceiveAmount, 
								ReceiveDate, 
                         CashIncentiveRemarks,
						      RStatus,
							  CommissionsAmount,
							  CommissionPrc,
							  FreightAmount
						  ,[CreatedDate]
						  ,[CreatedBy]
						  ,[EditedDate]
						  ,[EditedBy]
						  ,[IsActive]
					  FROM [dbo].[COMMLcInfo]
					  LEFT JOIN OM_Buyer ON OM_Buyer.BuyerId = COMMLcInfo.BuyerId AND OM_Buyer.CompId = @CompanyId

					  WHERE [IsActive] = 1
					  AND ([COMMLcInfo].BuyerId = @BuyerId OR @BuyerId = 0)
					  AND ([COMMLcInfo].LcType = @LcType OR @LcType = 0)
					  AND ((CAST([COMMLcInfo].LcDate AS Date) >= @FromDate) OR (@FromDate ='01/01/2000'))
					  AND ((CAST([COMMLcInfo].LcDate AS Date) <= @ToDate) OR (@ToDate = '01/01/2000'))
					  AND ([COMMLcInfo].LcIssuingBank LIKE '%'+ @LcBank +'%' OR @LcBank = '') 
				      AND ([COMMLcInfo].ReceivingBank LIKE '%'+ @ReceiveBank +'%' OR @ReceiveBank = '')	
					  AND ([COMMLcInfo].LcNo LIKE '%'+ @LCNO +'%' OR @LCNO = '')	
					  
					  ORDER BY  [LcDate] DESC, [COMMLcInfo].[BuyerId]	 	
					  					  														  						  											  							
END