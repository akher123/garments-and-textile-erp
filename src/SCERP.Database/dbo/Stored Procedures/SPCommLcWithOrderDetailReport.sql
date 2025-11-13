-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <20/04/2016>
-- Description:	<> exec SPCommLcWithOrderDetailReport '001'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommLcWithOrderDetailReport]
			

						   @CompanyId		NVARCHAR(3)			
										
AS
BEGIN
	
			SET NOCOUNT ON;

												
						SELECT [COMMLcInfo].[LcId]
							  ,[OM_Buyer].[BuyerName]
							  ,[COMMLcInfo].[LcNo]
							  ,[COMMLcInfo].[LcDate]							  													
							  ,[COMMLcInfo].[LcAmount]
							  ,[COMMLcInfo].[LcQuantity]
							  ,[COMMLcInfo].[MatureDate]
							  ,[COMMLcInfo].[ExpiryDate]
							  ,[COMMLcInfo].[ExtensionDate]

							  ,[OM_Merchandiser].[EmpName]		AS MerchandiserName
							  ,[OM_BuyerOrder].[OAmount]		AS OrderAmount		
							  ,[OM_BuyerOrder].[Quantity]		AS OrderQuantity
							  ,[OM_BuyerOrder].[Fab]			AS Fabrication																																																
							  ,[OM_BuyerOrder].[BuyerOrderId]
							  ,[OM_BuyerOrder].[CompId]							  
							  ,[OM_BuyerOrder].[OrderNo]
							  ,[OM_BuyerOrder].[OrderDate]
							  ,[OM_BuyerOrder].[RefNo]
							  ,[OM_BuyerOrder].[RefDate]
							  ,[OM_BuyerOrder].[SampleOrdNo]
							  ,[OM_BuyerOrder].[BuyerRefId]
							  ,[OM_BuyerOrder].[DGRefNo]																	 
							  ,[OM_BuyerOrder].[Closed]
							  ,[OM_BuyerOrder].[CloseDate]											 
							  ,[Company].[Name] AS CompanyName
							  ,(SELECT COUNT(*) FROM OM_BuyOrdStyle WHERE OrderNo = [OM_BuyerOrder].[OrderNo]) AS [Style]
					
							   
						  FROM [dbo].[COMMLcInfo]
			         LEFT JOIN [dbo].[OM_BuyerOrder] ON [dbo].[COMMLcInfo].LcId = [dbo].[OM_BuyerOrder].[LcRefId] AND [COMMLcInfo].IsActive = 1 AND [OM_BuyerOrder].[CompId] = @CompanyId			   
					 LEFT JOIN [OM_Buyer] ON [OM_Buyer].[BuyerId] = [COMMLcInfo].[BuyerId] AND [OM_Buyer].[CompId] = @CompanyId
					 LEFT JOIN [OM_Merchandiser] ON [OM_Merchandiser].[EmpId] = [OM_BuyerOrder].[MerchandiserId] AND [OM_Merchandiser].[CompId] = @CompanyId
			         LEFT JOIN [Company] ON [OM_BuyerOrder].CompId = [Company].[CompanyRefId]
			   			   		  					   				   														  					  														  						  											  							
END