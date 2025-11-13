

CREATE view [dbo].[VProductionDetail]
as 
select p.ProductionId,
PD.ProductionDetailId,
 P.CompId,P.PType,
 P.ProrgramRefId,
 P.ProductionRefId,
 P.ProcessorRefId,
 P.ProcessRefId ,
 PD.ItemCode,
 I.ItemName,
 PD.ColorRefId,
 C.ColorName,
 PD.SizeRefId,
 SZ.SizeName,
 PD.MeasurementUinitId,
 PD.Qty,
 MU.UnitName
from PROD_Production as P
inner join PROD_ProductionDetaill as PD on PD.ProductionRefId=P.ProductionRefId and PD.ProductionId=P.ProductionId and PD.CompId=P.CompId
left join OM_Color as C on PD.ColorRefId=C.ColorRefId and PD.CompId=C.CompId
left join OM_Size as SZ on PD.SizeRefId=SZ.SizeRefId and PD.CompId=SZ.CompId
left join Inventory_Item as I on PD.ItemCode=I.ItemCode
left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId


