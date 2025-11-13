
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <08/03/2016>
-- Description:	<> exec SPCommBbLcPurchaseInfo  '001' , '',  ''
-- =============================================

CREATE PROCEDURE [dbo].[SPCommBbLcPurchaseInfo]
				
						@CompId					NVARCHAR(3)
					   ,@BbLcNo					NVARCHAR(50)
					   ,@PurchaseOrderNo		NVARCHAR(15)
																				 
AS

BEGIN
	
	SET NOCOUNT ON;
						   						
					SELECT [BbLcPurchaseId]												
						  ,[CommBbLcPurchase].[BbLcNo]
						  ,CONVERT(VARCHAR(10), CommPurchaseOrder.PurchaseOrderDate, 103) AS PurchaseOrderDate
						  ,CONVERT(VARCHAR(10), CommPurchaseOrder.ExpDate, 103) AS ExpDate		
						  ,	CASE 
								WHEN CommPurchaseOrder.PType = 'Y' THEN 'Yarn' 
								WHEN CommPurchaseOrder.PType = 'A' THEN 'accessories'
							END	AS ProductType							
								
						  ,OM_BuyerOrder.RefNo	 AS OrderNo			
						  ,[CommBbLcPurchase].[PurchaseOrderNo] 		
						  ,Mrc_SupplierCompany.CompanyName AS SupplierName
					
						  ,CONVERT(DECIMAL(18,0),(SELECT SUM([Quantity])   
							FROM [dbo].[CommPurchaseOrderDetail]
							WHERE [CommPurchaseOrderDetail].PurchaseOrderId = [CommBbLcPurchase].PurchaseOrderRefId
							GROUP BY [CommPurchaseOrderDetail].[PurchaseOrderId]													
							)) AS TotalQuantity

						 ,CONVERT(DECIMAL(18,2),(SELECT SUM([xRate])   
						    FROM [dbo].[CommPurchaseOrderDetail]
						    WHERE [CommPurchaseOrderDetail].PurchaseOrderId = [CommBbLcPurchase].PurchaseOrderRefId
						    GROUP BY [CommPurchaseOrderDetail].[PurchaseOrderId]													
						  )) AS TotalAmount

					  FROM [dbo].[CommBbLcPurchase]		
					  LEFT JOIN CommBbLcInfo ON CommBbLcInfo.BbLcId = [CommBbLcPurchase].[BbLcRefId]
					  LEFT JOIN CommPurchaseOrder ON CommPurchaseOrder.PurchaseOrderId = [CommBbLcPurchase].PurchaseOrderRefId
					  LEFT JOIN Mrc_SupplierCompany ON Mrc_SupplierCompany.SupplierCompanyId = CommPurchaseOrder.SupplierId
					  LEFT JOIN OM_BuyerOrder ON OM_BuyerOrder.OrderNo = CommPurchaseOrder.OrderNo

					  WHERE [CommBbLcPurchase].IsActive = 1	 AND [CommBbLcPurchase].CompId = @CompId
					  AND  ([CommBbLcPurchase].BbLcNo = @BbLcNo OR @BbLcNo = '')														
					  AND  ([CommBbLcPurchase].PurchaseOrderNo = @PurchaseOrderNo OR @PurchaseOrderNo = '')											 													
END