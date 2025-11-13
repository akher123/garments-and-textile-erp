

CREATE view [dbo].[VCompConsumptionDetail]
as
select 
GC.ColorName as GColorName,
GS.SizeName as GSizeName,
PC.ColorName as PColorName,
PS.SizeName  as  PSizeName,
BC.ColorName BaseColorName,
TW.SizeName as  TableWidth,
I.ItemCode,
ISNULL(I.ItemName,'----') as ItemName,
ISNULL(MU.UnitName,'----') as UnitName,
CCD.CompId,
CCD.CompConsumptionDetailId,
CCD.ConsRefId,
CCD.GColorRefId,
CCD.GSizeRefId,
CCD.PColorRefId,
CCD.PSizeRefId,
CCD.BaseColorRefId,
CCD.PAllow,
(isnull(CCD.PPQty,0.0)*12) as PDzCons,
CCD.PPQty,
CCD.TQty,
CCD.GSM,
CCD.[Length],
CCD.OrderStyleRefId,
CCD.TableWidthID,
CCD.[Weight],
CCD.Width,
CCD.CompomentSlNo,
CCD.LayQty,
CCD.ProcessLoss,
CS.ConsGroup,
CS.ConsTypeRefId,
ISNULL((CCD.TQty/(1-CCD.ProcessLoss*0.01)),0.0) as GreyQty,
ISNULL(CCD.QuantityP,0.00) as QuantityP,
ISNULL((select SUM(KQty) from OM_YarnConsumption 
where ConsRefId=CCD.ConsRefId  and GrColorRefId=CCD.GColorRefId and CompId= CCD.CompID ),0.0) AS YConsQty,
CCD.ApprovedLD,
CCD.RequiredQty as RequiredQty,
(
select top(1) cp.CompType from OM_CompConsumption as CC 
inner join OM_Component as cp on CC.ComponentRefId=Cp.ComponentRefId and CC.CompId=Cp.CompId
where CC.ComponentSlNo=CCD.CompomentSlNo and CC.ConsRefId=CCD.ConsRefId and CC.CompId=CCD.CompID) as CompType,
OSS.SizeRow
from OM_CompConsumptionDetail as CCD
left join OM_Color as GC on CCD.GColorRefId=GC.ColorRefId and CCD.CompID=GC.CompId
left join OM_Color as PC on CCD.PColorRefId=PC.ColorRefId and CCD.CompID=PC.CompId
left join OM_Color as BC on CCD.BaseColorRefId=BC.ColorRefId and CCD.CompID=BC.CompId

left join OM_Size as GS on CCD.GSizeRefId=GS.SizeRefId and CCD.CompID=GS.CompId

left join OM_Size as PS on CCD.PSizeRefId=PS.SizeRefId and CCD.CompID=PS.CompId

left join OM_Size as TW on CCD.TableWidthID=TW.SizeRefId and CCD.CompID=TW.CompId

inner join OM_Consumption as CS on CCD.ConsRefId=CS.ConsRefId and CCD.CompID=CS.CompId
left join OM_BuyOrdStyleSize as OSS on CS.OrderStyleRefId=OSS.OrderStyleRefId and GS.SizeRefId=OSS.SizeRefId and OSS.CompId=GS.CompId
inner join Inventory_Item as I on CS.ItemCode=I.ItemCode 

left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId



