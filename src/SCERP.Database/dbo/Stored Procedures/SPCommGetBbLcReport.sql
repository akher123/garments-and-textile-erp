-- =================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> exec SPCommGetBbLcReport '001', 0, 0, '01/01/2000', '01/01/2000'
-- =================================================================================

CREATE PROCEDURE [dbo].[SPCommGetBbLcReport]
			

						   @CompanyId				NVARCHAR(3)
						  ,@SupplierId				INT								 
						  ,@LcType					INT
						  ,@LcRefId					INT
						  ,@FromDate				DATETIME
						  ,@ToDate					DATETIME	
						  ,@MaturityDateFrom		DATETIME	
						  ,@MaturityDateTo			DATETIME	
						  ,@ExpiryDateFrom			DATETIME	
						  ,@ExpiryDateTo			DATETIME
						  ,@BbLcNo					NVARCHAR(30)
						  ,@IssuingBankId			INT

AS

BEGIN
	
			SET NOCOUNT ON;

					   SELECT [BbLcId]
					  ,[LcRefId]
					  ,[BbLcNo]
					  ,[BbLcDate],
					    [SupplierCompanyRefId]
					  ,[BbLcAmount]
					  ,[BbLcQuantity]
					  ,[CommBbLcInfo].[MatureDate]
					  ,[CommBbLcInfo].[ExpiryDate]
					  ,[CommBbLcInfo].[ExtensionDate]
					  ,CommBank.BankName AS [BbLcIssuingBank]
					  ,[BbLcIssuingBankAddress]
					  ,[CommBbLcInfo].[ReceivingBank]
					  ,[CommBbLcInfo].BeNo AS [ReceivingBankAddress]
					  ,[BbLcType]
					  ,([Beneficary] + '$' + COMMLcInfo.LcNo) AS [Beneficiary]
					  ,[CommBbLcInfo].[PartialShipment]
					  ,[CommBbLcInfo].[Description]
					  ,COMMLcInfo.UdEoNo AS [IfdbcNo]
					  ,[CommBbLcInfo].BeDate AS [IfdbcDate]
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
					  LEFT JOIN CommBank ON CommBank.BankId = CommBbLcInfo.IssuingBankId

					  WHERE CommBbLcInfo.IsActive = 1
					  AND (Mrc_SupplierCompany.SupplierCompanyId = @SupplierId OR @SupplierId = 0)
					  AND (CommBbLcInfo.BbLcType = @LcType OR @LcType = 0)
					  AND (CommBbLcInfo.LcRefId = @LcRefId OR @LcRefId = 0)
					  AND (CommBbLcInfo.IssuingBankId = @IssuingBankId OR @IssuingBankId = 0)
					  AND ((CAST(CommBbLcInfo.BbLcDate AS Date) >= @FromDate) OR (@FromDate ='01/01/1900'))
					  AND ((CAST(CommBbLcInfo.BbLcDate AS Date) <= @ToDate) OR (@ToDate = '01/01/2050'))	
					  				  
					  AND ((CAST(CommBbLcInfo.MatureDate AS Date) >= @MaturityDateFrom) OR (@MaturityDateFrom ='01/01/2001'))	
					  AND ((CAST(CommBbLcInfo.MatureDate AS Date) <= @MaturityDateTo) OR (@MaturityDateTo ='01/01/2001'))

					  AND ((CAST(CommBbLcInfo.ExpiryDate AS Date) >= @ExpiryDateFrom) OR (@ExpiryDateFrom ='01/01/2001'))	
					  AND ((CAST(CommBbLcInfo.ExpiryDate AS Date) <= @ExpiryDateTo) OR (@ExpiryDateTo ='01/01/2001'))

					  AND CommBbLcInfo.BbLcNo LIKE '%' + @BbLcNo + '%' 

					  ORDER BY CommBbLcInfo.BbLcNo	  														  						  											  							
END