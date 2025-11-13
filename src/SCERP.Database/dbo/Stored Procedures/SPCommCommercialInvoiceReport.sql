-- ==============================================================================
-- Author:		< Md.Yasir Arafat >
-- Create date: < 11/05/2016 >
-- Description:	<> exec SPCommCommercialInvoiceReport '001'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommCommercialInvoiceReport]
					
					
						@CompanyId	NVARCHAR(3)		
																	
AS
			
BEGIN
						
			SET NOCOUNT ON;
			SET ANSI_WARNINGS OFF

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

						  ,[COMMLcInfo].[ReceivingBank]				AS [NegotiatingBank]
						  ,[COMMLcInfo].[ReceivingBankAddress]		AS [negotiatingBankAddress]


						  ,[OM_Buyer].BuyerName						AS [ConsigneeName]
						  ,[OM_Buyer].[Address1]					AS [ConsigneeAddress1]
						  ,[OM_Buyer].[Address2]					AS [ConsigneeAddress2]
						  ,[OM_Buyer].[Address3]					AS [ConsigneeAddress3]
						  ,[COMMLcInfo].LcIssuingBank				AS [ConsigneeBank]
						  ,[COMMLcInfo].LcIssuingBankAddress		AS [ConsigneeBankAddress]


						  ,[CommExport].[ExportNo]					AS [ExportNo]
						  ,[CommExport].[ExportDate]				AS [ExportDate]
						  ,[CommExport].[BillOfLadingNo]			AS [BillOfLadingNo]
						  ,[CommExport].[BillOfLadingDate]			AS [BillOfLadingDate]
									

					      ,'BANGLADESH'								AS [CountryOfOrigin]
						  ,[CommExport].[PaymentMode]				AS [PaymentMode]
						  ,[CommExport].[Incoterm]					AS [Incoterm]
						  ,[CommExport].[ShipmentMode]				AS [ShipmentMode]
						  ,[CommExport].[PortOfLanding]				AS PortOfLanding
						  ,[CommExport].[PortOfDischarge]		    AS [PortOfDischarge]
						  ,[CommExport].[FinalDestination]			AS [FinalDestination]

						  ,[OM_BuyOrdStyle].[OrderStyleRefId]		AS [OrderStyleRefId]
						  ,[OM_Style].[StyleName]					AS [StyleName]
						  ,[OM_BuyOrdStyle].[Rate]					AS [UnitPrice]
						  ,[OM_BuyOrdStyle].[Rmks]					AS [Remarks]
						  ,[BuyOrdShipDetail].[ShippedQuantity]		AS [ShippedQuantity]
						  ,[OM_BuyOrdStyle].[Rate]	* [BuyOrdShipDetail].[ShippedQuantity] AS [TotalAmount]

						  ,CommExportDetail.CartonQuantity			AS [CartonQuantity]
																																			
			    FROM [dbo].[OM_BuyerOrder]
		   LEFT JOIN [dbo].[Company]			ON [dbo].[Company].[CompanyRefId]				 = [dbo].[OM_BuyerOrder].[CompId] 
		
		   LEFT JOIN [dbo].[OM_Buyer]			ON [dbo].[OM_Buyer].[BuyerRefId]				 = [dbo].[OM_BuyerOrder].[BuyerRefId] AND [dbo].[OM_Buyer].[CompId] = @CompanyId
		   LEFT JOIN [dbo].[Country]			ON [dbo].[Country].[Id]							 = [dbo].[OM_Buyer].[CountryId] 
		   LEFT JOIN [dbo].[COMMLcInfo]			ON [dbo].[COMMLcInfo].[LcId]					 = [dbo].[OM_BuyerOrder].[LcRefId] 
		   LEFT JOIN [dbo].[CommExport]			ON [dbo].[CommExport].[LcId]					 = [dbo].[COMMLcInfo].[LcId] AND [dbo].[CommExport].[CompId] =  @CompanyId		 
		  
		   LEFT JOIN [dbo].[OM_BuyOrdStyle]     ON [dbo].[OM_BuyOrdStyle].OrderNo				 = [dbo].[OM_BuyerOrder].OrderNo AND  [dbo].[OM_BuyOrdStyle].CompId = @CompanyId		   
		   LEFT JOIN [dbo].[OM_BuyOrdShip]		ON [dbo].[OM_BuyOrdShip].OrderStyleRefId	     = [dbo].[OM_BuyOrdStyle].OrderStyleRefId AND [OM_BuyOrdShip].CompId = @CompanyId
		   LEFT JOIN [dbo].[OM_PortOfLoading]	ON [dbo].[OM_PortOfLoading].[PortOfLoadingRefId] = [dbo].[OM_BuyOrdShip].[PortOfLoadingRefId] AND [OM_PortOfLoading].CompId = @CompanyId
		   LEFT JOIN [dbo].[OM_Style]			ON [dbo].[OM_Style].StylerefId					 = [dbo].[OM_BuyOrdStyle].[StyleRefId] AND [OM_Style].[CompID] = @CompanyId
		   LEFT JOIN CommExportDetail ON CommExportDetail.ExportId = CommExport.ExportId
		   LEFT JOIN(
		   				   SELECT [OM_BuyOrdShip].[OrderStyleRefId]
					      ,SUM (ISNULL(OM_BuyOrdShipDetail.ShQty, 0)) AS [ShippedQuantity]
				           FROM [dbo].[OM_BuyOrdShip]
						   JOIN [OM_BuyOrdShipDetail] ON [OM_BuyOrdShipDetail].[OrderShipRefId] = [OM_BuyOrdShip].[OrderShipRefId]
				           GROUP BY [OM_BuyOrdShip].[OrderStyleRefId]
				    ) AS [BuyOrdShipDetail] ON [BuyOrdShipDetail].[OrderStyleRefId] = [OM_BuyOrdStyle].[OrderStyleRefId]

		   WHERE	 [dbo].[OM_BuyerOrder].[CompId] = @CompanyId AND [OM_BuyerOrder].RefNo= '32-274249'		 			  					  														  						  											  							
END