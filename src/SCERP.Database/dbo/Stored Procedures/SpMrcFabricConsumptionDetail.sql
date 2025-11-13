
CREATE procedure [dbo].[SpMrcFabricConsumptionDetail] 
 
@CompId varchar(3),
@OrderStyleRefId  varchar(7)
as 
select
ISNULL(I.ItemName,'----') as ItemName,
ISNULL(MU.UnitName,'----') as UnitName,
 GS.SizeName as GSizeName ,
GC.ColorName as GColorName,
PC.ColorName as PColorName,
PS.SizeName  as  PSizeName,
BC.ColorName BaseColorName,
TW.SizeName as  TableWidth,
Comp.ComponentName,
CCD.PAllow,
(isnull(CCD.PPQty,0.0)*12) as PDzCons,
CCD.PPQty,
CCD.TQty,
CCD.GSM,
CCD.ProcessLoss as DyeingWsPrc,
CCD.ApprovedLD as ApprovedLD,
IIF(Comp.CompType=3, CCD.RequiredQty*2, CCD.RequiredQty) as RequiredQty,
ISNULL(CCD.TQty/(1-(CCD.ProcessLoss*0.01)),0) as GreyQty,
ISNULL(CCD.QuantityP,0.00) as QuantityP,
CS.ItemDescription as Remarks
from OM_CompConsumption as CC
inner join OM_CompConsumptionDetail as CCD on  CC.OrderStyleRefId=CCD.OrderStyleRefId and  CC.ConsRefId=CCD.ConsRefId and  CC.ComponentSlNo = CCD.CompomentSlNo
inner join OM_Component as Comp on CC.ComponentRefId=Comp.ComponentRefId and CC.CompId=Comp.CompId
inner join OM_Consumption as CS on CCD.ConsRefId=CS.ConsRefId and CCD.CompID=CS.CompId
inner join Inventory_Item as I on CS.ItemCode=I.ItemCode and CS.CompId=I.CompId
left join OM_Color as GC on CCD.GColorRefId=GC.ColorRefId and CCD.CompID=GC.CompId
left join OM_Color as PC on CCD.PColorRefId=PC.ColorRefId and CCD.CompID=PC.CompId
left join OM_Color as BC on CCD.BaseColorRefId=BC.ColorRefId and CCD.CompID=BC.CompId
left join OM_Size as GS on CCD.GSizeRefId=GS.SizeRefId and CCD.CompID=GS.CompId
left join OM_Size as PS on CCD.PSizeRefId=PS.SizeRefId and CCD.CompID=PS.CompId
left join OM_Size as TW on CCD.TableWidthID=TW.SizeRefId and CCD.CompID=TW.CompId
left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
left join OM_BuyOrdStyleSize as SZ on CC.OrderStyleRefId=SZ.OrderStyleRefId and GS.SizeRefId=SZ.SizeRefId and SZ.CompId=CS.CompId
where CS.OrderStyleRefId=@OrderStyleRefId and CS.CompId=@CompId  and CCD.PPQty>0 
order by SZ.SizeRow






--SELECT   
-- M.ItemCode, 
--replace(I.ItemName,' ','') as ItemName,
--C.ColorName,
--S.SizeName, 
--U.UnitName,
--sum(round(D.QuantityP,3)) as Quantity,
--sum(round((D.PPQty * 12),3)) AS [Cons/DZ],
--sum(round(D.PAllow,2)) as Allowance, 
--sum(round(D.TotalQty,3)) as Total
--FROM  
-- OM_ConsumptionDetail AS D
-- inner JOIN OM_Consumption AS M ON D.CompId = M.CompId AND D.ConsRefId = M.ConsRefId
-- inner join Inventory_Item as I on M.ItemCode=I.ItemCode and D.CompId=I.CompId
-- left join MeasurementUnit as U on I.MeasurementUinitId=U.UnitId
-- left join OM_Color as C on D.PColorRefId=C.ColorRefId and D.CompId=C.CompId
-- left join OM_Size as S on D.PSizeRefId=S.SizeRefId and D.CompId=S.CompId
--WHERE (D.CompId =@CompId) AND (M.OrderStyleRefId =@OrderStyleRefId) and left(I.ItemCode,2)='10'  --Fabric
--group by  M.ItemCode, I.ItemName,C.ColorName,S.SizeName,U.UnitName