CREATE PROCEDURE [dbo].[spGetYarnBookingByPiRefId]
@CompId char(3),
@PiRfId varchar(7)
AS 
SELECT 
		 PO.OrderStyleRefId,
		 (	 select StyleName from VOM_BuyOrdStyle  where OrderStyleRefId= PO.OrderStyleRefId and CompId= CPD.CompId) AS StyleName,
		 CPD.CompId,
		 I.ItemId,
		 I.ItemName,
		 CPD.ItemCode,
		 CPD.xRate,
		 PC.ColorName,
		 PS.SizeName,
		 CPD.SizeRefId,
		-- CPD.GSizeRefId,
		 CPD.ColorRefId,
		-- CPD.GColorRefId,
		 SUM( CPD.Quantity) as Quantity ,
		 ISNULL((select SUM(ISNULL(D.ReceivedQty,0)-ISNULL(D.RejectedQty,0)) from Inventory_MaterialReceiveAgainstPo AS R
         INNER JOIN Inventory_MaterialReceiveAgainstPoDetail AS D ON R.MaterialReceiveAgstPoId=D.MaterialReceiveAgstPoId
          where R.StoreId=1 and  R.SupplierId=PO.SupplierId and R.PoNo=PO.PurchaseOrderNo and D.OrderStyleRefId=PO.OrderStyleRefId and R.CompId=@CompId and D.ItemId=I.ItemId and D.SizeRefId=CPD.SizeRefId and D.FColorRefId=CPD.ColorRefId ),0.0) AS TotalRcvQty,
		 MU.UnitName
         FROM  CommPurchaseOrderDetail as CPD 
		 inner join CommPurchaseOrder as PO on CPD.PurchaseOrderId=PO.PurchaseOrderId and CPD.CompId=PO.CompId
         left join OM_Color as PC on CPD.ColorRefId=PC.ColorRefId and CPD.CompId=PC.CompId
         left join OM_Size as PS on CPD.SizeRefId=PS.SizeRefId and CPD.CompId=PS.CompId
         inner join Inventory_Item as I on CPD.ItemCode=I.ItemCode 
         left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
		 WHERE PO.PType='Y' and PO.PurchaseOrderNo=@PiRfId AND PO.CompId=@CompId 
		 GROUP BY 
		 PO.OrderStyleRefId,
		 PO.PurchaseOrderNo,
		 CPD.CompId,
		 I.ItemId,
		 I.ItemName,
		 CPD.ItemCode,
		 CPD.xRate,
		 PO.SupplierId,
		 PC.ColorName,
		 PS.SizeName,
		 CPD.SizeRefId,
		-- CPD.GSizeRefId,
		 CPD.ColorRefId,
		-- CPD.GColorRefId,
		 MU.UnitName

		 having 	CAST(SUM(CPD.Quantity) as decimal(19,2)) - CAST(ISNULL((select SUM(ISNULL(D.ReceivedQty,0)-ISNULL(D.RejectedQty,0)) from Inventory_MaterialReceiveAgainstPo AS R
         INNER JOIN Inventory_MaterialReceiveAgainstPoDetail AS D ON R.MaterialReceiveAgstPoId=D.MaterialReceiveAgstPoId
         where R.StoreId=1 and  R.SupplierId=PO.SupplierId and R.PoNo=PO.PurchaseOrderNo and D.OrderStyleRefId=PO.OrderStyleRefId and R.CompId=@CompId and D.ItemId=I.ItemId and D.SizeRefId=CPD.SizeRefId and D.FColorRefId=CPD.ColorRefId ),0.0) as decimal(19,2))!=0.0


	

		 
		 


