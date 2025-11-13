-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/04/2016>
-- Description:	<> exec SPCommGetOrderWithoutLcReport '001'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommGetOrderWithoutLcReport]
			

						   @CompanyId		NVARCHAR(3)	
			
AS
BEGIN
	
			SET NOCOUNT ON;

						SELECT [OM_BuyerOrder].LcRefId		AS [LcRefId]
							  ,[OrderDate]					AS [OrderDate]
							  ,[RefNo]						AS [OrderRefNo]
							  ,[OM_Buyer].[BuyerName]		AS [BuyerName]
							  ,[Quantity]					AS [OrderQuantity]												  
							  ,[OM_Merchandiser].[EmpName]  AS [Merchandiser]
							  ,[Fab]						AS [Fabrication]
							  ,[OM_Season].[SeasonName]		AS [SeasonName]
							  ,(SELECT SUM(Quantity) FROM OM_BuyOrdStyle WHERE OM_BuyOrdStyle.OrderNo = [OM_BuyerOrder].[OrderNo] AND OM_BuyOrdStyle.CompId = @CompanyId ) 
															AS [StyleQuantity]

						  FROM [dbo].[OM_BuyerOrder]
						  JOIN OM_Buyer ON OM_Buyer.BuyerRefId = OM_BuyerOrder.BuyerRefId AND OM_Buyer.CompId = @CompanyId
						  JOIN OM_Merchandiser ON OM_Merchandiser.EmpId = OM_BuyerOrder.MerchandiserId AND OM_Merchandiser.CompId = @CompanyId
						  JOIN OM_Season ON OM_Season.SeasonRefId = OM_BuyerOrder.SeasonRefId AND OM_Season.CompId = @CompanyId

						  WHERE [OM_BuyerOrder].CompId = @CompanyId AND [OM_BuyerOrder].LcRefId IS NULL 
						  ORDER BY [OM_BuyerOrder].OrderDate
														  									  					  					  														  						  											  							
END