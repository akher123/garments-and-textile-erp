-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> exec SPCommGetBbLcInfo '001', 0, 0, '01/01/2000', '01/01/2000'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPCommGetBbLcInfo]
			

						   @CompanyId		NVARCHAR(3)
						  ,@SupplierId		INT								 
						  ,@LcType		    INT
						  ,@FromDate		DATETIME
						  ,@ToDate			DATETIME					

AS
BEGIN
	
			SET NOCOUNT ON;

					   SELECT [BbLcId]
					  ,[LcRefId]
					  ,[BbLcNo]
					  ,[BbLcDate]
					  ,[SupplierCompanyRefId]
					  ,[BbLcAmount]
					  ,[BbLcQuantity]
					  ,[MatureDate]
					  ,[ExpiryDate]
					  ,[ExtensionDate]
					  ,[BbLcIssuingBank]
					  ,[BbLcIssuingBankAddress]
					  ,[ReceivingBank]
					  ,[ReceivingBankAddress]
					  ,[BbLcType]
					  ,[Beneficiary]
					  ,[PartialShipment]
					  ,[Description]
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

					  WHERE CommBbLcInfo.IsActive = 1
					  AND (Mrc_SupplierCompany.SupplierCompanyId = @SupplierId OR @SupplierId = 0)
					  AND (CommBbLcInfo.BbLcType = @LcType OR @LcType = 0)
					  AND ((CAST(CommBbLcInfo.BbLcDate AS Date) >= @FromDate) OR (@FromDate ='01/01/2000'))
					  AND ((CAST(CommBbLcInfo.BbLcDate AS Date) <= @ToDate) OR (@ToDate = '01/01/2000'))	
					  ORDER BY [CommBbLcInfo].[BbLcNo]			  														  						  											  							
END