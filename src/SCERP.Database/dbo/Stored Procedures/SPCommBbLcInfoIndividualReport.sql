-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> exec SPCommBbLcInfoIndividualReport '001', 5
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommBbLcInfoIndividualReport]
			

						   @CompanyId		NVARCHAR(3)
						  ,@BbLcId			INT								 
						 			

AS
BEGIN
	
			SET NOCOUNT ON;

					   SELECT [BbLcId]
					  ,[LcRefId]
					  ,[BbLcNo]
					  ,[BbLcDate]
					  ,Mrc_SupplierCompany.CompanyName
					  ,COMMLcInfo.LcNo
					  ,[BbLcAmount]
					  ,[BbLcQuantity]
					  ,[CommBbLcInfo].[MatureDate]
					  ,[CommBbLcInfo].[ExpiryDate]
					  ,[CommBbLcInfo].[ExtensionDate]
					  ,[BbLcIssuingBank]
					  ,[BbLcIssuingBankAddress]
					  ,[CommBbLcInfo].[ReceivingBank]
					  ,[CommBbLcInfo].[ReceivingBankAddress]
					  ,[BbLcType]
					  ,[Beneficiary]
					  ,[CommBbLcInfo].[PartialShipment]
					  ,[CommBbLcInfo].[Description]	
					  ,[IfdbcNo]
					  ,[IfdbcDate]
					  ,[IfdbcValue]
					  ,[PcsSanctionAmount]
					  ,[CommBbLcInfo].[CreatedDate]
					  ,[CommBbLcInfo].[CreatedBy]
					  ,[CommBbLcInfo].[EditedDate]
					  ,[CommBbLcInfo].[EditedBy]
					  ,[CommBbLcInfo].[IsActive]				 
				       FROM [dbo].[CommBbLcInfo]

					  LEFT JOIN Mrc_SupplierCompany ON Mrc_SupplierCompany.SupplierCompanyId = CommBbLcInfo.SupplierCompanyRefId AND Mrc_SupplierCompany.IsActive = 1
					  LEFT JOIN COMMLcInfo ON COMMLcInfo.LcId = CommBbLcInfo.LcRefId AND COMMLcInfo.IsActive = 1

					  WHERE CommBbLcInfo.IsActive = 1
					  AND   CommBbLcInfo.BbLcId = @BbLcId
					  					  														  						  											  							
END