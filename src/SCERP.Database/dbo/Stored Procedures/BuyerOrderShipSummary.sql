CREATE procedure BuyerOrderShipSummary
@EmployeeId uniqueidentifier ,
@CompId varchar(3),
@FromDate datetime,
@ToDate datetime
as
select B.BuyerName,
BO.RefNo as OrderName,
BO.OrderNo,
I.ItemName,
ST.StyleName,
SH.ShipDate,
SH.OrderShipRefId,
M.EmpName,
ISNULL((select top(1)PortOfLoadingName from OM_PortOfLoading where PortOfLoadingRefId=SH.PortOfLoadingRefId and CompId=SH.CompId),'--') as PortOfLoadingName,
ISNULL((select SUM(QuantityP) from OM_BuyOrdShipDetail where OrderShipRefId=SH.OrderShipRefId and CompId=SH.CompId ),0) as Quantity ,
ISNULL(OST.Rate,0) as Rate,
(select count(distinct ColorRefId) from OM_BuyOrdShipDetail where OrderShipRefId=SH.OrderShipRefId and CompId=SH.CompId ) as NoColor ,
(select count(distinct SizeRefId) from OM_BuyOrdShipDetail where OrderShipRefId=SH.OrderShipRefId and CompId=SH.CompId ) as NoSize 
from OM_BuyerOrder as BO
inner join OM_BuyOrdStyle as OST on BO.OrderNo=OST.OrderNo and BO.CompId=OST.CompId
inner join OM_Style as ST on OST.StyleRefId=ST.StylerefId and OST.CompId=ST.CompID
inner join Inventory_Item as I on ST.ItemId=I.ItemId
inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_BuyOrdShip as SH on OST.OrderStyleRefId=SH.OrderStyleRefId and SH.CompId=OST.CompId
inner join OM_Merchandiser as M on BO.MerchandiserId=M.EmpId and BO.CompId=M.CompId
where  OST.ActiveStatus=1 and BO.CompId=@CompId and SH.ShipDate between  @FromDate and @ToDate and BO.MerchandiserId in (select MerchandiserRefId from UserMerchandiser
where EmployeeId=@EmployeeId and CompId=@CompId)


