-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <20/04/2016>
-- Description:	<> exec SPCommLcWithOrderSummaryReport '001'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommLcWithOrderSummaryReport]
			

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
							  ,[OM_BuyerOrder].[OAmount]   AS OrderAmount		
							  ,[OM_BuyerOrder].[Quantity]  AS OrderQuantity																																											  																										 										
							  ,[Company].[Name] AS CompanyName
											   
						  FROM [dbo].[COMMLcInfo]
			         LEFT JOIN [dbo].[OM_BuyerOrder] ON [dbo].[COMMLcInfo].LcId = [dbo].[OM_BuyerOrder].LcRefId AND [COMMLcInfo].[IsActive] = 1 AND [OM_BuyerOrder].[CompId] = @CompanyId			   
					 LEFT JOIN [OM_Buyer] ON [OM_Buyer].[BuyerId] = [COMMLcInfo].[BuyerId] AND [OM_Buyer].[CompId] = @CompanyId
			         LEFT JOIN [Company] ON [OM_BuyerOrder].CompId = Company.CompanyRefId
			   			   		  					   				   														  					  														  						  											  							
END