CREATE procedure [dbo].[spStyleActivity]
@CompId varchar(3),
@EmployeeId uniqueidentifier,
@BuyerRefId varchar(3)=null,
@SearchString varchar(100)=NULL
as
select 
BO.CompId,
M.EmpName as Merchandiser, 
BO.MerchandiserId,
BO.BuyerRefId,
B.BuyerName,
BO.RefNo,
BO.OrderNo ,
BOST.OrderStyleRefId,
ST.StyleName,
I.ItemName,
BO.Quantity as OrdQty ,
(select max(ShipDate) from OM_BuyOrdShip as SHIP where SHIP.OrderNo=BO.OrderNo and SHIP.CompId=BO.CompId) as ShipDate,
CAST((select count(*)from dbo.OM_BuyOrdShip  as SHIP where  SHIP.OrderStyleRefId=BOST.OrderStyleRefId and SHIP.CompId=BOST.CompId) as bit) as IsAssort,
CAST((select count(*) from OM_Consumption where left(ItemCode,2)='03' and OrderStyleRefId=BOST.OrderStyleRefId and CompId=BOST.CompId) as bit) as IsAccsCons,
CAST((select count(*) from OM_FabricOrderDetail as D where D.OrderStyleRefId=BOST.OrderStyleRefId and D.CompId=BOST.CompId) as bit) as IsFabCons,
(select ISNULL(SUM(CD.TQty),0) from dbo.OM_CompConsumptionDetail as CD   where CD.CompId=BOST.CompId and CD.OrderStyleRefId=BOST.OrderStyleRefId) as FabConsQty,
CAST((select count(*)from CommPurchaseOrder  as PO where  PO.OrderStyleRefId=BOST.OrderStyleRefId and PO.CompId=BOST.CompId and PType='A')as bit) IsDIn,
CAST((select count(*)from CommPurchaseOrder  as PO where  PO.OrderStyleRefId=BOST.OrderStyleRefId and PO.CompId=BOST.CompId and PType='Y') as bit) as IsYarnCons,
CAST(0 as bit) as IsProcsSeq,
CAST(0 as bit) as IsProdProg,
CAST(0 as bit) as IsKIn,
CAST(0 as bit) as IsKOut,

CAST(0 as bit) as IsDOut,
CAST(0 as bit) as IsCIn,
CAST(0 as bit) as IsCOut,
CAST(0 as bit) as IsSIn,
CAST(0 as bit) as IsSOut,
CAST(0 as bit) as IsFIn,
CAST(0as bit) as IsFOut,
ISNULL((select sum(ShQty)  from OM_BuyOrdShipDetail as BOSH inner join OM_BuyOrdShip as OSH on BOSH.OrderShipRefId=OSH.OrderShipRefId and BOSH.CompId=OSH.CompId where   BOSH.CompId=BOST.CompId and OSH.OrderStyleRefId=BOST.OrderStyleRefId),0) as ShipQty
from OM_BuyerOrder as BO
inner join dbo.OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Merchandiser as M on BO.MerchandiserId=M.EmpId and BO.CompId=M.CompId
inner join dbo.OM_BuyOrdStyle as BOST on BO.OrderNo=BOST.OrderNo and BO.CompId=BOST.CompId
inner join dbo.OM_Style as ST on BOST.StyleRefId=ST.StylerefId and BOST.CompId=ST.CompID
left join Inventory_Item as I on ST.ItemId=I.ItemId  and ST.CompID=I.CompId
where  BO.Closed='O' and BOST.ActiveStatus=1  and (BO.BuyerRefId=@BuyerRefId or @BuyerRefId='') and BO.CompId=@CompId and (ST.StyleName+''+BO.RefNo+''+B.BuyerName like '%' + @SearchString + '%'  or @SearchString is null) 
and BO.MerchandiserId in (select MerchandiserRefId from UserMerchandiser where EmployeeId=@EmployeeId)



--exec spStyleActivity '001',null,null,null







