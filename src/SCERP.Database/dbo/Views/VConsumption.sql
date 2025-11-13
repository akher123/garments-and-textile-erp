
CREATE view [dbo].[VConsumption] 
as 
select 
C.CompId,
OS.OrderNo,
BO.RefNo as OrderName,
ST.StyleName,
C.OrderStyleRefId,
CG.ConsGroupName,
CT.ConsTypeName,
C.ConsumptionId,
C.ConsRefId,
C.ConsDate,
C.Quantity,
C.ItemCode,
c.ConsGroup,
c.ConsTypeRefId,
C.SupplierId,
S.CompanyName as SupplierName,
C.ItemDescription,
C.Rate,
ISNULL(I.ItemName,'---')  as ItemName,
ISNULL(MU.UnitName,'---') as Unit
from OM_Consumption as C
inner join OM_BuyOrdStyle as OS on C.OrderStyleRefId=OS.OrderStyleRefId and C.CompId=OS.CompId
inner join OM_BuyerOrder  as BO on OS.OrderNo=BO.OrderNo and OS.CompId=BO.CompId
inner join OM_ConsumptionGroup as CG on C.ConsGroup=CG.ConsGroup
inner join OM_ConsumptionType as CT on C.ConsTypeRefId=CT.ConsTypeRefId
inner join OM_Style as ST on OS.StyleRefId=ST.StylerefId and OS.CompId=ST.CompID

left join Inventory_Item as I on C.ItemCode=I.ItemCode
left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
left join Mrc_SupplierCompany as S on C.SupplierId= S.SupplierCompanyId




