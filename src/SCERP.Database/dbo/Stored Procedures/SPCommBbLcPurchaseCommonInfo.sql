
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <14/03/2016>
-- Description:	<> exec SPCommBbLcPurchaseCommonInfo  '001' , '',  ''
-- =============================================

CREATE PROCEDURE [dbo].[SPCommBbLcPurchaseCommonInfo]
				
						@CompId					NVARCHAR(3)
					   ,@BbLcNo					NVARCHAR(50)
					   ,@PurchaseOrderNo		NVARCHAR(15)
																				 
AS

BEGIN
	
	SET NOCOUNT ON;
						   						
				SELECT BbLcPurchaseId
					  ,BbLcRefId
					  ,CommBbLcPurchaseCommon.BbLcNo
					  ,CommBbLcPurchaseCommon.PurchaseOrderRefId
					  ,CommBbLcPurchaseCommon.PurchaseOrderNo
					  ,CommBbLcPurchaseCommon.PurchaseDate
					  ,CommBbLcPurchaseCommon.ItemCode
					  ,CommBbLcPurchaseCommon.ColorRefId
					  ,CommBbLcPurchaseCommon.SizeRefId
					  ,CommBbLcPurchaseCommon.Quantity
					  ,CommBbLcPurchaseCommon.xRate					
					  ,CASE 
							WHEN CommBbLcPurchaseCommon.PurchaseType = 'O' THEN 'Purchase Order' 
							WHEN CommPurchaseOrder.PType = 'D' THEN 'Data Entry'
					   END	AS PurchaseType	

					  ,CommBbLcPurchaseCommon.CompId
					  ,CommBbLcInfo.BbLcNo
					  ,Inventory_Item.ItemName
					  ,OM_Color.ColorName
					  ,OM_Size.SizeName

					  FROM [dbo].[CommBbLcPurchaseCommon]
					  LEFT JOIN CommBbLcInfo ON CommBbLcInfo.BbLcId = CommBbLcPurchaseCommon.BbLcRefId
					  LEFT JOIN CommPurchaseOrder ON CommPurchaseOrder.PurchaseOrderId = CommBbLcPurchaseCommon.PurchaseOrderRefId AND CommPurchaseOrder.CompId = @CompId
					  LEFT JOIN Inventory_Item ON Inventory_Item.ItemCode = CommBbLcPurchaseCommon.ItemCode
					  LEFT JOIN OM_Color ON OM_Color.ColorRefId = CommBbLcPurchaseCommon.ColorRefId
					  LEFT JOIN OM_Size ON OM_Size.SizeId = CommBbLcPurchaseCommon.SizeRefId AND OM_Size.CompId = @CompId

					  WHERE [CommBbLcPurchaseCommon].IsActive = 1 AND [CommBbLcPurchaseCommon].CompId = @CompId
					  AND  ([CommBbLcPurchaseCommon].BbLcNo = @BbLcNo OR @BbLcNo = '')														
					  AND  ([CommBbLcPurchaseCommon].PurchaseOrderNo = @PurchaseOrderNo OR @PurchaseOrderNo = '')											 													
END