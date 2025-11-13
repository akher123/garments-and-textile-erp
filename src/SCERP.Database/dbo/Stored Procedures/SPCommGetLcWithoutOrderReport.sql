-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/04/2016>
-- Description:	<> exec SPCommGetLcWithoutOrderReport '001'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommGetLcWithoutOrderReport]
			

						   @CompanyId		NVARCHAR(3)	
			
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
						  ,[LcIssuingBank]
						  ,[LcIssuingBankAddress]
						  ,[ReceivingBank]
						  ,[ReceivingBankAddress]
						  ,[LcType]
						  ,[Beneficary] AS [Beneficary]
						  ,[OM_Buyer].[BuyerName]
						  ,[PartialShipment]
						  ,[Description]
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
					  LEFT JOIN [OM_Buyer] ON [OM_Buyer].[BuyerId] = [COMMLcInfo].[BuyerId] AND [OM_Buyer].[CompId] = @CompanyId
					  WHERE [COMMLcInfo].[IsActive] = 1 AND [COMMLcInfo].[LcId] NOT IN (SELECT LcRefId FROM [OM_BuyerOrder] WHERE [OM_BuyerOrder].[CompId] = @CompanyId AND LcRefId IS NOT NULL )
									  
					  ORDER BY [COMMLcInfo].[BuyerId], [LcDate]	
					  					  					  														  						  											  							
END