
CREATE view [dbo].[VwCompConsumptionOrderStyle]
as
select 
BO.MerchandiserId,
OS.CompId,
ISNULL(S.StyleName,'---') as StyleName ,
ISNULL(Sn.SeasonName,'---') as SeasonName ,
ISNULL(I.ItemName,'---') as ItemName ,
I.ItemCode,
BO.RefNo,
BO.BuyerRef,
OS.EFD as ShipDate,
OS.OrderNo,
OS.OrderStyleRefId,
OS.Quantity,
(select count(*)from dbo.OM_CompConsumption  as C where  C.OrderStyleRefId=OS.OrderStyleRefId and C.CompId=OS.CompId) as TotalComponet,
(select ISNULL(SUM(CD.TotalQty),0) from dbo.OM_ConsumptionDetail as CD inner join dbo.OM_Consumption as C on C.OrderStyleRefId=OS.OrderStyleRefId and CD.ConsRefId=C.ConsRefId and CD.CompId=C.CompId and left(C.ItemCode,2)='10'where C.CompId=OS.CompId) as TotalFabQty
from dbo.OM_BuyOrdStyle as OS
Inner join dbo.OM_Style as S on OS.StyleRefId=s.StylerefId  and OS.CompId=s.CompID
inner join dbo.OM_BuyerOrder as BO on OS.OrderNo=BO.OrderNo and  OS.CompId=BO.CompId
inner join dbo.Inventory_Item as I on I.ItemId=S.ItemId
left join dbo.OM_Season as Sn on OS.SeasonRefId=Sn.SeasonRefId and OS.CompId=Sn.CompId

where OS.ActiveStatus=1 and BO.Closed='O'




