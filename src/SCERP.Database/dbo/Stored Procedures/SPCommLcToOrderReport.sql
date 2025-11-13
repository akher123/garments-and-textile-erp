-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <10/04/2016>
-- Description:	<> exec SPCommLcToOrderReport '001'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommLcToOrderReport]
								
						   																	
						   @CompanyId		NVARCHAR(3)	
AS
BEGIN
	
			SET NOCOUNT ON;

					
					SELECT [COMMLcInfo].[LcId]
						  ,[COMMLcInfo].[LcNo]
						  ,[OM_Buyer].[BuyerName]
						  ,[COMMLcInfo].[LcDate]  
						  ,[COMMLcInfo].[LcQuantity]	  
						  ,[COMMLcInfo].[LcAmount]      
						  ,[COMMLcInfo].[MatureDate]
						  ,[COMMLcInfo].[ExpiryDate]
						  ,[COMMLcInfo].[ExtensionDate]
						  ,[COMMLcInfo].[LcIssuingBank]
						  ,[COMMLcInfo].[LcIssuingBankAddress]
						  ,[COMMLcInfo].[ReceivingBank]
						  ,[COMMLcInfo].[ReceivingBankAddress]
						  ,[COMMLcInfo].[LcType]
						  ,[COMMLcInfo].[Beneficary]
						  ,[COMMLcInfo].[Description]
   
						  ,[OM_BuyerOrder].[BuyerOrderId]
						  ,[OM_BuyerOrder].[CompId]
						  ,[OM_BuyerOrder].[OrderNo]
						  ,[OM_BuyerOrder].[RefNo]
						  ,[OM_BuyerOrder].[Quantity]
						  ,[OM_BuyerOrder].[OrderDate]
						  ,[OM_BuyerOrder].[OAmount]
						  ,(
								SELECT TOP(1)[EFD]       	                
								FROM [dbo].[OM_BuyOrdStyle]
								WHERE [OM_BuyOrdStyle].CompId = @CompanyId AND OrderNo = [OM_BuyerOrder].[OrderNo]
								ORDER BY [OM_BuyOrdStyle].[EFD] DESC
						  )AS ShipmentDate
						 
					FROM [dbo].[COMMLcInfo]
					LEFT JOIN OM_BuyerOrder ON OM_BuyerOrder.LcRefId = COMMLcInfo.LcId 
					LEFT JOIN OM_Buyer ON OM_Buyer.BuyerId = [COMMLcInfo].[BuyerId] 
					  
					WHERE 	[COMMLcInfo].IsActive = 1
					AND		OM_BuyerOrder.CompId = @CompanyId
					AND     OM_Buyer.CompId = @CompanyId	
					 					  														  						  											  							
END