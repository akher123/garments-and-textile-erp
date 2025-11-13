-- =============================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <06/12/2018>
-- Description:	<> EXEC SPCommGetCashLcReport  '', '', '01/01/2000', '01/01/2020', '', '', ''
-- =============================================================================================

CREATE PROCEDURE [dbo].[SPCommGetCashLcReport]
			
							
						   @SupplierName			NVARCHAR(100)
						  ,@CashLcNo				NVARCHAR(100)								 
						  ,@LcFromDate				DATETIME
						  ,@LcToDate				DATETIME
						  ,@ShipmentMode			NVARCHAR(100)
						  ,@PortOfDelivery			NVARCHAR(100)
						  ,@ItemName				NVARCHAR(100)
						

AS

BEGIN
	
			SET NOCOUNT ON;

					    SELECT [CashLcId]
							  ,[CashLcNo]
							  ,[LcDate]
							  ,[Item]
							  ,[QuantitySetMultiple]
							  ,[Quantity]
							  ,[SupplierName]
							  ,[PortOfDelivery]
							  ,[DateOfBLMultiple]
							  ,[DateOfBL]
							  ,[CountryOfOrigin]
							  ,[PaymentTerms]
							  ,[BillOfImportCode]
							  ,[WayOfEntry]
							  ,[BillOfEntry]
							  ,[BillOfImport]
							  ,[DateOfBillMultiple]
							  ,[DateOfBill]
							  ,[LcValue]
							  ,[BankRef]
							  ,[PaymentDate]
							  ,[CreatedDate]
							  ,[EditedDate]
							  ,[CreatedBy]
							  ,[EditedBy]
							  ,[Remarks]
						  FROM [dbo].[CommCashLc]	
						  WHERE (SupplierName LIKE '%' + @SupplierName + '%' OR @SupplierName = '')	
						  AND (CashLcNo LIKE '%' + @CashLcNo + '%' OR @CashLcNo = '')
						  AND ((CAST(LcDate AS Date) >= @LcFromDate) OR (@LcFromDate ='01/01/2000'))	
						  AND ((CAST(LcDate AS Date) <= @LcToDate) OR (@LcToDate ='01/01/2020'))
						  AND (WayOfEntry LIKE '%' + @ShipmentMode + '%' OR @ShipmentMode ='')	
						  AND (PortOfDelivery LIKE '%' + @PortOfDelivery + '%' OR @PortOfDelivery = '')
						  AND (Item LIKE '%' + @ItemName + '%' OR @ItemName = '')
						  		  						  											  							
END