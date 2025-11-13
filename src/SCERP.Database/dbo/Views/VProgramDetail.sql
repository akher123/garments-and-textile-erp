
CREATE view [dbo].[VProgramDetail]
as 
select PD.ProgramDetailId,
P.ProgramRefId as PrgramRefId,
PD.CompId,
PD.ProgramId,
PD.Quantity,
PD.ColorRefId,
PD.SizeRefId,
(select top(1) SizeName from OM_Size where SizeRefId=PD.SizeRefId) as SizeName,
(select top(1) ColorName from OM_Color where ColorRefId=PD.ColorRefId) as ColorName,
(select top(1) SizeName from OM_Size where SizeRefId=PD.FinishSizeRefId) as FinishSizeName,
PD.FinishSizeRefId,
PD.LotRefId,

ISNULL((select  top(1) Lot.ColorName from  OM_Color as Lot
inner join Inventory_Brand as Br on  Lot.ColorCode=CAST( Br.BrandId AS varchar) and Lot.TypeId='02'
where Lot.ColorRefId=PD.LotRefId),PD.Remarks) as LotName,
ISNULL((select top(1) Br.Name from  OM_Color as Lot
inner join Inventory_Brand as Br on  Lot.ColorCode=CAST( Br.BrandId AS varchar)  and  Lot.TypeId='02'
where Lot.ColorRefId=PD.LotRefId),PD.Remarks) as BrandName,

I.ItemCode,
I.ItemId,
PD.Remarks,
PD.SleeveLength,
PD.GSM,
PD.YRatio,
PD.NoOfCone,
PD.Rate,
PD.ComponentRefId,
(select  top(1) ComponentName from OM_Component where ComponentRefId=PD.ComponentRefId and CompId=PD.CompId) as ComponentName,
I.ItemName,
U.UnitName,
I.MeasurementUinitId,
PD.MType,P.ProcessRefId from PLAN_ProgramDetail as PD
inner join PLAN_Program as P on PD.ProgramId=P.ProgramId and PD.CompId=P.CompId
--inner join OM_Size as S on PD.SizeRefId=S.SizeRefId and PD.CompId=S.CompId
--left join OM_Color as C on PD.ColorRefId=C.ColorRefId and PD.CompId=C.CompId
--left join OM_Color as Lot on PD.LotRefId=Lot.ColorRefId and PD.CompId=P.CompId 
--left join Inventory_Brand as Br on Lot.ColorCode=Br.BrandId 
--left join OM_Size  as FS on PD.FinishSizeRefId=FS.SizeRefId and PD.CompId=FS.CompId 
left join Inventory_Item as I on PD.ItemCode=I.ItemCode 
left join MeasurementUnit as U on I.MeasurementUinitId=U.UnitId



