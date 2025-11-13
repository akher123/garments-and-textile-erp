
	CREATE view [dbo].[VwPurchaseOrderDetail]
	as
	select 
	
		 CPD.PurchaseOrderDetailId,
		 CPD.PurchaseOrderId,
		 CPD.PurchaseOrderRefId,
		 CPD.CompId,
		 I.ItemId,
		 I.ItemName,
		 CPD.ItemCode,
		 CPD.xRate,
		 PO.SupplierId,
		 SPL.CompanyName as SupplierName,
		 PC.ColorName,
		 PS.SizeName,
		 CPD.SizeRefId,
		 CPD.GSizeRefId,
		 CPD.ColorRefId,
		 CPD.GColorRefId,
		 CPD.Quantity ,
		 MU.UnitName
		 from  CommPurchaseOrderDetail as CPD 
		 inner join CommPurchaseOrder as PO on CPD.PurchaseOrderId=PO.PurchaseOrderId and CPD.CompId=PO.CompId
		 inner join Mrc_SupplierCompany as SPL on PO.SupplierId=SPL.SupplierCompanyId
         left join OM_Color as PC on CPD.ColorRefId=PC.ColorRefId and CPD.CompId=PC.CompId
         left join OM_Size as PS on CPD.SizeRefId=PS.SizeRefId and CPD.CompId=PS.CompId
         inner join Inventory_Item as I on CPD.ItemCode=I.ItemCode 

         left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
	

