-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <02/03/2016>
-- Description:	<> exec SPCommExportListReport '01/01/2000', '01/01/2000', ''
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommExportListReport]
			
										 					
						   @FromDate		DATETIME
						  ,@ToDate			DATETIME	
						  ,@SearchKey		NVARCHAR(50)				

AS
BEGIN
	
			SET NOCOUNT ON;

				SELECT 
					   [ExportId]
					  ,[COMMLcInfo].LcNo
					  ,[ExportRefId]
					  ,[ExportNo]
					  ,CONVERT(VARCHAR(10), [ExportDate], 103) AS ExportDate					  	
					  ,[InvoiceNo]
					  ,CONVERT(VARCHAR(10), [InvoiceDate], 103) AS InvoiceDate					  					  
					  ,CAST([InvoiceValue] AS DECIMAL(18,2)) AS [InvoiceValue]
					  ,[BankRefNo]
					  ,CONVERT(VARCHAR(10), [BankRefDate], 103) AS BankRefDate					 					  
					  ,CAST([RealizedValue] AS DECIMAL(18,2)) AS [RealizedValue]
					  ,CONVERT(VARCHAR(10), [RealizedDate], 103) AS RealizedDate					  
					  ,[BillOfLadingNo]
					  ,CONVERT(VARCHAR(10), [BillOfLadingDate], 103) AS BillOfLadingDate					  
					  ,[SBNo]
					  ,CONVERT(VARCHAR(10), [SBNoDate], 103) AS SBNoDate					  
					  ,[CommExport].[LcId]
					  ,[CompId]

					  ,[FcAmount]
					  ,[UdNoLocal]
					  ,[UdNoForeign]					  
					  ,CONVERT(VARCHAR(10), [UdDateLocal], 103) AS [UdDateLocal]
					  ,CONVERT(VARCHAR(10), [UdDateForeign], 103) AS [UdDateForeign]
					  
			FROM [dbo].[CommExport]
	   LEFT JOIN [dbo].[COMMLcInfo] ON COMMLcInfo.LcId = [CommExport].LcId	AND [COMMLcInfo].IsActive = 1
				
	      WHERE ((CAST([CommExport].[ExportDate] AS Date) >= @FromDate) OR (@FromDate ='01/01/2000'))
		    AND ((CAST([CommExport].[ExportDate] AS Date) <= @ToDate) OR (@ToDate = '01/01/2000'))

			AND (	   [COMMLcInfo].LcNo			LIKE '%'+ @SearchKey +'%' 
				    OR [CommExport].ExportNo		LIKE '%'+ @SearchKey +'%' 
				    OR [CommExport].InvoiceNo		LIKE '%'+ @SearchKey +'%'				
				    OR [CommExport].BankRefNo		LIKE '%'+ @SearchKey +'%'				 
				    OR [CommExport].BillOfLadingNo  LIKE '%'+ @SearchKey +'%'
				    OR [CommExport].SBNo			LIKE '%'+ @SearchKey +'%'

			   OR CAST([CommExport].InvoiceValue AS NVARCHAR(50))  LIKE '%'+ @SearchKey +'%'
			   OR CAST([CommExport].RealizedValue AS NVARCHAR(50)) LIKE '%'+ @SearchKey +'%'

			   OR @SearchKey = '')

		   ORDER BY    COMMLcInfo.LcNo	  														  						  											  							
END


