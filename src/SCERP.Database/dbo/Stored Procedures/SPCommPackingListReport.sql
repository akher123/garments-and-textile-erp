-- ==============================================================================
-- Author:		< Md.Yasir Arafat >
-- Create date: < 14/05/2016 >
-- Description:	<> exec SPCommPackingListReport '001'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommPackingListReport]
					
										
							@CompanyId	NVARCHAR(3)	
																		
AS
			
BEGIN
						
			SET NOCOUNT ON;			

					SELECT [OM_BuyerOrder].[BuyerOrderId]			AS [BuyerOrderId]					
						  ,[OM_BuyerOrder].[OrderNo]				AS [OrderNo]
						  ,[OM_BuyerOrder].[OrderDate]				AS [OrderDate]
						  ,[OM_BuyerOrder].[RefNo]					AS [RefNo]

						  ,[Company].[Name]							AS [CompanyName]
						  ,[Company].[FullAddress]					AS [CompanyAddress]
						  ,[Company].[Phone]						AS [CompanyPhone]
						  ,[Company].[Fax]							AS [CompanyFax]
						  ,[Company].[Email]						AS [CompanyEmail]
						  ,[Company].[Website]						AS [CompanyWebsite]
						  ,[CommExport].[InvoiceNo]					AS [InvoiceNo]
						  ,[CommExport].[InvoiceDate]				AS [InvoiceDate]
						  ,[COMMLcInfo].[LcNo]						AS [ExportLcNo]
						  ,[COMMLcInfo].[LcDate]					AS [ExportLcDate]

						  ,[OM_Consignee].[ConsigneeName]			AS [ConsigneeName]
						  ,[OM_Consignee].[Address1]				AS [ConsigneeAddress1]
						  ,[OM_Consignee].[Address2]				AS [ConsigneeAddress2]
						  ,[OM_Consignee].[Address3]				AS [ConsigneeAddress3]

						  ,[CommExport].[ExportNo]					AS [ExportNo]
						  ,[CommExport].[ExportDate]				AS [ExportDate]
						  ,[CommExport].[BillOfLadingNo]			AS [BillOfLadingNo]
						  ,[CommExport].[BillOfLadingDate]			AS [BillOfLadingDate]
									
					      ,'BANGLADESH'								AS [CountryOfOrigin]
						  ,[OM_PaymentTerm].[PayTerm]				AS [PaymentMode]
						  ,'FOB DHAKA'								AS [Incoterm]
						  ,'BY SEA'									AS [ShipmentMode]
						  ,[OM_PortOfLoading].[PortOfLoadingName]   AS [PortOfLoading]
						  ,'HAMBURG, GERMANY'						AS [PortOfDischarge]
						  ,[Country].[CountryName]					AS [DestinationCountry]

						  ,[OM_BuyOrdStyle].[OrderStyleRefId]		AS [OrderStyleRefId]
						  ,[OM_Style].[StyleName]					AS [StyleName]
						  ,[OM_BuyOrdStyle].[Rate]					AS [UnitPrice]
						  ,[OM_BuyOrdStyle].[Rmks]					AS [Remarks]
						  ,[OM_Color].[ColorName]					AS [ColorName]
						  ,[OM_Size].[SizeName]						AS [SizeName]	
								
																																	
			    FROM [dbo].[OM_BuyerOrder]
		   LEFT JOIN [dbo].[Company]			 ON [dbo].[Company].[CompanyRefId]				  = [dbo].[OM_BuyerOrder].[CompId] 
		   LEFT JOIN [dbo].[OM_Buyer]			 ON [dbo].[OM_Buyer].[BuyerRefId]				  = [dbo].[OM_BuyerOrder].[BuyerRefId] AND [dbo].[OM_Buyer].[CompId] = @CompanyId
		   LEFT JOIN [dbo].[Country]			 ON [dbo].[Country].[Id]						  = [dbo].[OM_Buyer].[CountryId] 
		   LEFT JOIN [dbo].[COMMLcInfo]			 ON [dbo].[COMMLcInfo].[LcId]					  = [dbo].[OM_BuyerOrder].[LcRefId] 
		   LEFT JOIN [dbo].[CommExport]			 ON [dbo].[CommExport].[LcId]					  = [dbo].[COMMLcInfo].[LcId] AND [dbo].[CommExport].[CompId] =  @CompanyId
		   LEFT JOIN [dbo].[OM_Consignee]		 ON [dbo].[OM_Consignee].[BuyerRefId]			  = [dbo].[OM_Buyer].[BuyerRefId] AND [OM_Consignee].CompId = @CompanyId
		   LEFT JOIN [dbo].[OM_PaymentTerm]		 ON [dbo].[OM_PaymentTerm].[PayTermRefId]		  = [dbo].[OM_BuyerOrder].[PayTermRefId] AND [OM_PaymentTerm].CompId = @CompanyId		   
		   LEFT JOIN [dbo].[OM_BuyOrdStyle]      ON [dbo].[OM_BuyOrdStyle].OrderNo				  = [dbo].[OM_BuyerOrder].OrderNo AND  [dbo].[OM_BuyOrdStyle].CompId = @CompanyId		   
		   LEFT JOIN [dbo].[OM_BuyOrdShip]		 ON [dbo].[OM_BuyOrdShip].OrderStyleRefId	      = [dbo].[OM_BuyOrdStyle].OrderStyleRefId AND [OM_BuyOrdShip].CompId = @CompanyId
		   LEFT JOIN [dbo].[OM_PortOfLoading]	 ON [dbo].[OM_PortOfLoading].[PortOfLoadingRefId] = [dbo].[OM_BuyOrdShip].[PortOfLoadingRefId] AND [OM_PortOfLoading].CompId = @CompanyId
		   LEFT JOIN [dbo].[OM_Style]			 ON [dbo].[OM_Style].StylerefId					  = [dbo].[OM_BuyOrdStyle].[StyleRefId] AND [OM_Style].[CompID] = @CompanyId
		   LEFT JOIN [dbo].[OM_BuyOrdShipDetail] ON [dbo].[OM_BuyOrdShipDetail].[OrderShipRefId]  = [dbo].[OM_BuyOrdShip].[OrderShipRefId]
		   LEFT JOIN [dbo].[OM_Color]            ON [dbo].[OM_Color].[ColorRefId]				  = [dbo].[OM_BuyOrdShipDetail].[ColorRefId] AND [OM_Color].[CompId] = @CompanyId 		
		   LEFT JOIN [dbo].[OM_Size]			 ON [dbo].[OM_Size].[SizeRefId]					  = [dbo].[OM_BuyOrdShipDetail].[SizeRefId] AND [OM_Size].[CompId] = @CompanyId				   	
						   
		   WHERE	 [dbo].[OM_BuyerOrder].[CompId] = @CompanyId AND [dbo].[OM_BuyerOrder].RefNo= '32-274249'		 			  					  														  						  											  							
END