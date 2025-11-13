-- ==============================================================================
-- Author:		<Md. Fazlay Rabby>
-- Create date: <12/20/2017>
-- Description:	<> exec SPCommGetBbLcItemTypeReport '001', NULL, 1, '01/01/2016', '12/20/2017'
-- ==============================================================================================

CREATE PROCEDURE [dbo].[SPCommGetBbLcItemTypeReport]
			

						   @CompanyId		NVARCHAR(3)
						  ,@ItemType		INT =NULL								 
						  ,@IssuingBankId   INT =NULL
						  ,@FromDate		DATETIME=NULL
						  ,@ToDate			DATETIME=NULL					

AS
BEGIN
	
			SET NOCOUNT ON;

					   SELECT [BbLcId]
					  ,[LcRefId]
					  ,[COMMLcInfo].[LcNo]
					  ,[BbLcNo]
					  ,[BbLcDate]
					  ,[SupplierCompanyRefId]
					  ,[IssuingBankId]
					  ,[BbLcAmount]
					  ,[BbLcQuantity]
					  ,[Inventory_Group].[GroupName]
					  ,[CommBbLcInfo].[MatureDate]
					  ,[CommBbLcInfo].[ExpiryDate]
					  ,[CommBbLcInfo].[ExtensionDate]
					  ,[BbLcIssuingBank]
					  ,[BbLcIssuingBankAddress]
					  ,[CommBank].[BankName]
					  ,[CommBank].[BankAddress]
					  ,[CommBbLcInfo].[ReceivingBank]
					  ,[CommBbLcInfo].[ReceivingBankAddress]
					  ,[BbLcType]
					  ,([Beneficary] + '$' + COMMLcInfo.LcNo) AS [Beneficiary]
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
					  ,[CommBbLcInfo].[CompId]
					  ,Mrc_SupplierCompany.CompanyName as Supplier		 
				       FROM [dbo].[CommBbLcInfo]

					  LEFT JOIN Mrc_SupplierCompany ON Mrc_SupplierCompany.SupplierCompanyId = CommBbLcInfo.SupplierCompanyRefId AND Mrc_SupplierCompany.IsActive = 1
					  LEFT JOIN COMMLcInfo ON COMMLcInfo.LcId = CommBbLcInfo.LcRefId AND COMMLcInfo.IsActive = 1
					  LEFT JOIN Inventory_Group ON Inventory_Group.GroupId=CommBbLcInfo.ItemType
					  LEFT JOIN CommBank ON CommBank.BankId=CommBBLcInfo.IssuingBankId
					  WHERE CommBbLcInfo.IsActive = 1
					  AND (CommBbLcInfo.ItemType = @ItemType OR @ItemType IS NULL)
					  AND (CommBbLcInfo.IssuingBankId = @IssuingBankId OR @IssuingBankId IS NULL)
					  AND ((CAST(CommBbLcInfo.BbLcDate AS Date) >= CAST(@FromDate AS date)) OR (@FromDate ='01/01/2000'))
					  AND ((CAST(CommBbLcInfo.BbLcDate AS Date) <= CAST(@ToDate AS date)) OR (@ToDate = '01/01/2000'))	
					  
					  ORDER BY [CommBbLcInfo].[BbLcNo]		  														  						  											  							
END


