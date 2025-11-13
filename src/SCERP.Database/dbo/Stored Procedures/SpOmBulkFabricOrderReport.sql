CREATE procedure [dbo].[SpOmBulkFabricOrderReport]
@CompId varchar(3),
@FabricOrderId int
as 

select
FO.FabricOrderRefId,
FO.Remarks as Terms,
FO.OrderDate,
FO.ExpDate,
SCP.CompanyName as Supplier,
SCP.[Address],
SCP.ContactName,
SCP.Phone,
cp.Name as CompanyName,
cp.FullAddress ,
BST.BuyerName,
BST.RefNo as OrderName,
BST.StyleName,
BST.Merchandiser, 
(convert(varchar(2), (select SizeRow from OM_BuyOrdStyleSize as SST

where SST.SizeRefId=CCD.GSizeRefId and SST.OrderStyleRefId=CC.OrderStyleRefId and SST.CompId=CC.CompId ) ) +'--'+ GS.SizeName) as GSizeName ,
GC.ColorName as GColorName,
PC.ColorName as PColorName,
PS.SizeName  as  PSizeName,
BC.ColorName BaseColorName,
TW.SizeName as  TableWidth,
Comp.ComponentName,
ISNULL(I.ItemName,'----') as ItemName,
ISNULL(MU.UnitName,'----') as UnitName,
CCD.PAllow,
(isnull(CCD.PPQty,0.0)*12) as PDzCons,
CCD.PPQty,
CCD.TQty,
CCD.GSM,
CCD.ProcessLoss as DyeingWsPrc,
CCD.ApprovedLD as ApprovedLD,
CS.ItemDescription as Composition,
IIF(Comp.CompType=3, CCD.RequiredQty*2, CCD.RequiredQty) as RequiredQty,
ISNULL(CCD.TQty/(1-(CCD.ProcessLoss*0.01)),0) as GreyQty,
ISNULL(CCD.QuantityP,0.00) as QuantityP,
CC.[Description] as Remarks
from OM_CompConsumption as CC
inner join OM_CompConsumptionDetail as CCD on  CC.OrderStyleRefId=CCD.OrderStyleRefId and  CC.ConsRefId=CCD.ConsRefId and  CC.ComponentSlNo = CCD.CompomentSlNo
inner join OM_Component as Comp on CC.ComponentRefId=Comp.ComponentRefId and CC.CompId=Comp.CompId
inner join OM_Consumption as CS on CCD.ConsRefId=CS.ConsRefId and CCD.CompID=CS.CompId
inner join Inventory_Item as I on CS.ItemCode=I.ItemCode and CS.CompId=I.CompId
inner join VOM_BuyOrdStyle as BST on CC.OrderStyleRefId=BST.OrderStyleRefId and CC.CompId=BST.CompId
left join OM_Color as GC on CCD.GColorRefId=GC.ColorRefId and CCD.CompID=GC.CompId
left join OM_Color as PC on CCD.PColorRefId=PC.ColorRefId and CCD.CompID=PC.CompId
left join OM_Color as BC on CCD.BaseColorRefId=BC.ColorRefId and CCD.CompID=BC.CompId
left join OM_Size as GS on CCD.GSizeRefId=GS.SizeRefId and CCD.CompID=GS.CompId
left join OM_Size as PS on CCD.PSizeRefId=PS.SizeRefId and CCD.CompID=PS.CompId
left join OM_Size as TW on CCD.TableWidthID=TW.SizeRefId and CCD.CompID=TW.CompId
left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId

inner join OM_FabricOrderDetail as FOD on CC.OrderStyleRefId=FOD.OrderStyleRefId and CC.CompId=FOD.CompId
left join OM_BuyOrdStyleSize as SZ on CC.OrderStyleRefId=SZ.OrderStyleRefId and GS.SizeRefId=SZ.SizeRefId and FOD.CompId=SZ.CompId
inner join OM_FabricOrder as FO on FO.FabricOrderId=FOD.FabricOrderId and CC.CompId=FO.CompId
inner join Mrc_SupplierCompany as scp on FO.SupplierId=SCP.SupplierCompanyId 
inner join Company as cp on CC.CompId=cp.CompanyRefId
where FO.FabricOrderId=@FabricOrderId and FO.CompId=@CompId and CCD.PPQty>0 and ISNULL(CCD.TQty/(1-(CCD.ProcessLoss*0.01)),0)>0
order by SZ.SizeRow

