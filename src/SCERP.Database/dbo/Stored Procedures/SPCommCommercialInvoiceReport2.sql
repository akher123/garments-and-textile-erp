-- ==============================================================================
-- Author:		< Md.Yasir Arafat >
-- Create date: < 11/05/2016 >
-- Description:	<> exec SPCommCommercialInvoiceReport2 '001'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommCommercialInvoiceReport2]
					
					
						@CompanyId	NVARCHAR(3)		
																	
AS
			
BEGIN
						
			SET NOCOUNT ON;
			SET ANSI_WARNINGS OFF


			SELECT   Company.Name							AS [CompanyName]
					,Company.FullAddress					AS [CompanyAddress]
					,Company.Phone							AS [CompanyPhone]
					,Company.Fax							AS [CompanyFax]
					,Company.Email							AS [CompanyEmail]
					,Company.Website						AS [CompanyWebsite]

					,CommExport.InvoiceNo					AS [InvoiceNo]
					,CommExport.InvoiceDate					AS [InvoiceDate]
					,COMMLcInfo.LcNo						AS [ExportLcNo]
					,COMMLcInfo.LcDate						AS [ExportLcDate]
					,COMMLcInfo.ReceivingBank				AS [NegotiatingBank]
					,COMMLcInfo.ReceivingBankAddress		AS [negotiatingBankAddress]

					,OM_Buyer.BuyerName						AS [ConsigneeName]
					,OM_Buyer.Address1						AS [ConsigneeAddress1]
					,OM_Buyer.Address2						AS [ConsigneeAddress2]
					,OM_Buyer.Address3						AS [ConsigneeAddress3]

					,COMMLcInfo.LcIssuingBank				AS [ConsigneeBank]
					,COMMLcInfo.LcIssuingBankAddress		AS [ConsigneeBankAddress]

					,CommExport.ExportNo					AS [ExportNo]
					,CommExport.ExportDate					AS [ExportDate]
					,CommExport.BillOfLadingNo				AS [BillOfLadingNo]
					,CommExport.BillOfLadingDate			AS [BillOfLadingDate]

					,CommExport.PaymentMode					AS [PaymentMode]
					,CommExport.IncoTerm					AS [IncoTerm]
					,CommExport.ShipmentMode				AS [ShipmentMode]
					,CommExport.PortOfLanding				AS [PortOfLanding]
					,CommExport.PortOfDischarge				AS [PortOfDischarge]
					,CommExport.FinalDestination			AS [FinalDestination]

					,[OM_BuyerOrder].[OrderNo]				AS [OrderNo]
					,[OM_BuyerOrder].[OrderDate]			AS [OrderDate]
					,[OM_BuyerOrder].[RefNo]				AS [RefNo]


			FROM CommExport 
			LEFT JOIN COMMLcInfo		ON COMMLcInfo.LcId = CommExport.LcId
			LEFT JOIN OM_BuyerOrder		ON OM_BuyerOrder.LcRefId = COMMLcInfo.LcId AND OM_BuyerOrder.CompId = @CompanyId
			LEFT JOIN Company			ON Company.CompanyRefId = OM_BuyerOrder.CompId
			LEFT JOIN OM_Buyer			ON OM_Buyer.BuyerRefId = OM_BuyerOrder.BuyerRefId AND OM_Buyer.CompId = @CompanyId
			LEFT JOIN CommExportDetail  ON CommExportDetail.ExportId = CommExport.ExportId


			WHERE CommExport.CompId = @CompanyId
								 	 			  					  														  						  											  							
END