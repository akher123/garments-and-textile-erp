



CREATE view [dbo].[VYarnConsumption]
as
select 
(SELECT top(1) ColorName FROM OM_Color WHERE ColorRefId=YN.GrColorRefId AND CompId=YN.CompId) as GColorName,
(SELECT top(1) ColorName FROM OM_Color WHERE ColorRefId=YN.KColorRefId AND CompId=YN.CompId) as  KColorName,
(SELECT top(1) SizeName FROM OM_Size WHERE SizeRefId=YN.KSizeRefId AND CompId=YN.CompId) as KSizeName,
I.ItemCode,
I.ItemName,
MU.UnitName,
YN.YarnConsumptionId,
YN.YCRef,
YN.CompId,
YN.ConsRefId,
YN.CPercent,
YN.DReq,
YN.GrColorRefId,
YN.KColorRefId,
YN.KSizeRefId,
YN.KQty,
YN.PLoss,
YN.WMtr,
C.OrderStyleRefId,
YN.Rate,
YN.SupplierId,
S.CompanyName as SupplierName,
YN.PiRefId
from OM_YarnConsumption as YN
inner join OM_Consumption as C on YN.ConsRefId=C.ConsRefId and YN.CompId=C.CompId
left join Mrc_SupplierCompany as S on YN.SupplierId=S.SupplierCompanyId 
--INNER join OM_Color as GC on YN.GrColorRefId=GC.ColorRefId and YN.CompId=GC.CompId
--INNER join OM_Color as KC on YN.KColorRefId=KC.ColorRefId and YN.CompId=KC.CompId
--INNER join OM_Size as KS on YN.KSizeRefId=KS.SizeRefId and YN.CompId=KS.CompId
INNER join Inventory_Item as I on YN.ItemCode=I.ItemCode 
LEFT join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId

