-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <27/03/2016>
-- Description:	<> exec SPCommImportExportPerformanceReport '001'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommImportExportPerformanceReport]
			

						@CompanyId		NVARCHAR(3)	

AS
BEGIN
	
			SET NOCOUNT ON;
				

					SELECT [CommImportExportPerformance].[SerialId]
						  ,[CommImportExportPerformance].[LcId]
						  ,[CommImportExportPerformance].[LcNo]						  
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[LcDate], 3) AS [LcDate]
						  ,[CommImportExportPerformance].[BuyerId]
						  ,[OM_Buyer].[BuyerName]
						  ,[CommImportExportPerformance].[LcAmount]
						  ,CAST([CommImportExportPerformance].[LcQuantity] AS INT) AS [LcQuantity]								  
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[MatureDate], 3) AS [MatureDate]						  
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[ExpiryDate], 3) AS [ExpiryDate]
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[ExtensionDate], 3) AS [ExtensionDate]						  
						  ,[CommImportExportPerformance].[LcIssuingBank]
						  ,[CommImportExportPerformance].[LcIssuingBankAddress]
						  ,[CommImportExportPerformance].[ReceivingBank]
						  ,[CommImportExportPerformance].[ReceivingBankAddress]
						  ,[CommImportExportPerformance].[LcType]
						  ,[CommImportExportPerformance].[Beneficary]
						  ,[CommImportExportPerformance].[PartialShipment]
						  ,[CommImportExportPerformance].[Description]

						  ,[CommImportExportPerformance].[ExportNo]						  
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[ExportDate], 3) AS [ExportDate]
						  ,[CommImportExportPerformance].[InvoiceNo]
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[InvoiceDate], 3) AS [InvoiceDate]						  					
						  ,CAST([CommImportExportPerformance].[InvoiceValue] AS DECIMAL(18,2)) AS [InvoiceValue]
						  ,[CommImportExportPerformance].[BankRefNo]						  
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[BankRefDate], 3) AS [BankRefDate]						  						  
						  ,CAST([CommImportExportPerformance].[RealizedValue] AS DECIMAL(18,2)) AS [RealizedValue]						  					  
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[RealizedDate], 3) AS [RealizedDate]
						  ,[CommImportExportPerformance].[BillOfLadingNo]						  
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[BillOfLadingDate], 3) AS [BillOfLadingDate]
						  ,[CommImportExportPerformance].[SBNo]						  
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[SBNoDate], 3) AS [SBNoDate]

						  ,[CommImportExportPerformance].[BbLcNo]						  
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[BbLcDate], 3) AS [BbLcDate]
						  ,[CommImportExportPerformance].[SupplierCompanyRefId]
						  ,[Mrc_SupplierCompany].CompanyName AS SupplierCompany
						  ,[CommImportExportPerformance].[BbLcAmount]
						  ,[CommImportExportPerformance].[BbLcQuantity]						  
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[BbLcMatureDate], 3) AS [BBLCMatureDate]
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[ExpiryDate], 3) AS [BBLCExpiryDate]					   
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[ExtensionDate], 3) AS [BBLCExtensionDate]
						  ,[CommImportExportPerformance].[BbLcIssuingBank]
						  ,[CommImportExportPerformance].[BbLcIssuingBankAddress]
						  ,[CommImportExportPerformance].[ReceivingBank] AS [BBLCReceivingBank]
						  ,[CommImportExportPerformance].[ReceivingBankAddress] AS [BBLCReceivingBankAddress]
						  ,[CommImportExportPerformance].[BbLcType]
						  ,[CommImportExportPerformance].[Beneficiary]
						  ,[CommImportExportPerformance].[PartialShipment] AS [BBLCPartialShipment]
						  ,[CommImportExportPerformance].[Description] AS [BBLCDescription]
						  ,[CommImportExportPerformance].[IfdbcNo]						 
						  ,CONVERT(VARCHAR(10), [CommImportExportPerformance].[IfdbcDate], 3) AS [IfdbcDate]
						  ,[CommImportExportPerformance].[IfdbcValue]
						  ,[CommImportExportPerformance].[PcsSanctionAmount]
    						
					  FROM [dbo].[CommImportExportPerformance]
		
				
				LEFT JOIN OM_Buyer ON OM_Buyer.BuyerId = [CommImportExportPerformance].BuyerId AND OM_Buyer.CompId = @CompanyId
				LEFT JOIN Mrc_SupplierCompany ON Mrc_SupplierCompany.SupplierCompanyId = [CommImportExportPerformance].SupplierCompanyRefId				
							
				ORDER BY  [CommImportExportPerformance].[LcNo]				  														  						  											  							
END