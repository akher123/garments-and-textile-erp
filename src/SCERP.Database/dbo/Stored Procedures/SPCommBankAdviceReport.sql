-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <16/04/2016>
-- Description:	<> exec [SPCommBankAdviceReport] 2
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommBankAdviceReport]
			
										 					
						  @ExportId	BIGINT			

AS
BEGIN
	
			SET NOCOUNT ON;

					SELECT [CommBankAdvice].[BankAdviceId]
						  ,[CommBankAdvice].[ExportId]
						  ,[CommBankAdvice].[AccHeadId]
						  ,[CommAccHead].   [AccHeadName]
						  ,[CommBankAdvice].[HeadType]
						  ,[CommBankAdvice].[Currency]
						  ,[CommBankAdvice].[Amount]
						  ,[CommBankAdvice].[Rate]
						  ,[CommBankAdvice].[AmountInTaka]
						  ,[CommBankAdvice].[Particulars]
						  ,[CommBankAdvice].[BankRefNo]
						  ,[CommBankAdvice].[CompId]
						  ,[CommBankAdvice].[CreatedDate]
						  ,[CommBankAdvice].[CreatedBy]
						  ,[CommBankAdvice].[EditedDate]
						  ,[CommBankAdvice].[EditedBy]
						  ,[CommBankAdvice].[IsActive]

						  ,[CommExport].[ExportRefId]
						  ,[CommExport].[ExportNo]
						  ,[CommExport].[ExportDate]
						  ,[CommExport].[InvoiceNo]
						  ,[CommExport].[InvoiceDate]
						  ,[CommExport].[InvoiceValue]

						  ,[COMMLcInfo].LcNo

					  FROM [dbo].[CommBankAdvice]
				      JOIN [CommExport] ON [CommExport].[ExportId] = [CommBankAdvice].[ExportId] 
					  JOIN [COMMLcInfo] ON  [COMMLcInfo].[LcId] = [CommExport].[LcId] AND [COMMLcInfo].[IsActive] = 1
					  JOIN [dbo].[CommAccHead] ON [dbo].[CommAccHead].[AccHeadId] = [CommBankAdvice].[AccHeadId] AND [CommAccHead].[IsActive] = 1
					  
					 WHERE [CommExport].[ExportId] = @ExportId AND [CommBankAdvice].[IsActive] = 1
				  ORDER BY [CommAccHead].[DisplayOrder]		  						  											  							
END