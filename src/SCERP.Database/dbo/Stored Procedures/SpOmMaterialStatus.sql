-- Author:		<Author,,Md.Akheruzzaman>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpOmMaterialStatus]
	@CompId varchar(3),
	@OrderStyleRefId varchar(7)
AS
BEGIN
	SET NOCOUNT ON;
      select 
			 I.ItemName,
			 CPD.ItemCode,
			 CPD.Quantity  as ReqQty,
			 CPD.Quantity as PiQty,
			 0 as RemainingQty,
			 0 as PiBalanceQty,
			 0 as RecvdQty,
			 0 as StockQty,
			 0 as PracticalQty,
			 SPL.CompanyName as SupplierName,
			 PC.ColorName,
			 PS.SizeName,
			 MU.UnitName
			 from  CommPurchaseOrderDetail as CPD 
			 inner join CommPurchaseOrder as PO on CPD.PurchaseOrderId=PO.PurchaseOrderId and CPD.CompId=PO.CompId
			 inner join Mrc_SupplierCompany as SPL on PO.SupplierId=SPL.SupplierCompanyId
			 left join OM_Color as PC on CPD.ColorRefId=PC.ColorRefId and CPD.CompId=PC.CompId
			 left join OM_Size as PS on CPD.SizeRefId=PS.SizeRefId and CPD.CompId=PS.CompId
			 inner join Inventory_Item as I on CPD.ItemCode=I.ItemCode 
			 left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
			 where PO.OrderStyleRefId=@OrderStyleRefId and PO.CompId=@CompId 
END
