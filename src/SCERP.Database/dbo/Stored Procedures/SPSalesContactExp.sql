-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <07/09/2016>
-- Description:	<> EXEC SPSalesContactExp 157
-- ===================================================================================

CREATE PROCEDURE [dbo].[SPSalesContactExp]
			
 @LcId INT
	                            
								 
								 		
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
	   Expt.[ExportId]
      ,Expt.[ExportRefId]
      ,Expt.[ExportNo]
      ,Expt.[ExportDate]
      ,Expt.[InvoiceNo]
      ,Expt.[InvoiceDate]
	  ,Expt.[InvoiceQuantity]
      ,Expt.[InvoiceValue]
      ,Expt.[BankRefNo]
      ,Expt.[BankRefDate]
      ,Expt.[RealizedValue]
      ,Expt.[RealizedDate]
	  ,Expt.[ShortFallAmount]
	  ,Expt.[ShortFallReason]
      ,Expt.[BillOfLadingNo]
      ,Expt.[BillOfLadingDate]
      ,Expt.[SBNo]
      ,Expt.[SBNoDate]

      ,Expt.[CompId]
      ,Expt.[FcAmount]
      ,Expt.[UdNoLocal]
      ,Expt.[UdNoForeign]
      ,Expt.[UdDateLocal]
      ,Expt.[UdDateForeign]
      ,Expt.[PaymentMode]
      ,Expt.[IncoTerm]
      ,Expt.[ShipmentMode]
      ,Expt.[PortOfLanding]
      ,Expt.[PortOfDischarge]
      ,Expt.[FinalDestination]
      ,Expt.[SalseContactId]

      ,Sc.[LcId]
      ,Sc.[LcNo]
      ,Sc.[LcDate]
      ,Sc.[BuyerId]
      ,Sc.[Amount]
      ,Sc.[Quantity]
      ,Sc.[MatureDate]
      ,Sc.[ExpiryDate]
      ,Sc.[ExtensionDate]
      ,Sc.[ShipmentDate]
      ,Sc.[LcIssuingBank]
      ,Sc.[LcIssuingBankAddress]
      ,Sc.[ReceivingBankAddress]
      ,Sc.[ReceivingBankId]
      ,Sc.[LcType]
      ,Sc.[Description]
     
	   from CommSalseContact as Sc  Left join CommExport  as Expt on  Sc.[SalseContactId]=Expt.[SalseContactId]
	   where Sc.[LcId]=@LcId
							 
						    
					  					  														  						  											  							
END