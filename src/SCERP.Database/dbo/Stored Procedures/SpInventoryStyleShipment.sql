CREATE procedure [dbo].[SpInventoryStyleShipment]
@OrderStyleRefId varchar(7),
@StyleShipmentId bigint,
@CompId varchar(3)
as
if(@StyleShipmentId>0)
Begin
select  SH.CompId, SH.OrderStyleRefId,
(ISNULL((select  SUM(ShipmentQty)-(SUM(SHD.QuantityP)-SUM(SHD.QuantityP)) from Inventory_StyleShipmentDetail
where OrderStyleRefId=SH.OrderStyleRefId and ColorRefId=SHD.ColorRefId and SizeRefId=SHD.SizeRefId and CompId=SHD.CompId and Inventory_StyleShipmentDetail.StyleShipmentId=@StyleShipmentId ),0))as ShipmentQty,
SHD.SizeRefId,
(select top(1) SizeName from OM_Size where SizeRefId=SHD.SizeRefId and CompId=SHD.CompId ) as SizeName,
(select top(1) ColorName from OM_Color where ColorRefId=SHD.ColorRefId and CompId=SHD.CompID )as ColorName,
SHD.ColorRefId 
from OM_BuyOrdShip as SH
inner join OM_BuyOrdShipDetail as SHD on SH.OrderShipRefId=SHD.OrderShipRefId
where SH.OrderStyleRefId=@OrderStyleRefId and SH.CompId=@CompId  and SHD.QuantityP>0
group by SH.CompId,SH.OrderStyleRefId,SHD.SizeRefId,SHD.ColorRefId,SHD.CompId
END
else
Begin 
select SH.CompId, SH.OrderStyleRefId,
(SUM(SHD.QuantityP)-ISNULL((select SUM(ShipmentQty) from Inventory_StyleShipmentDetail
where OrderStyleRefId=SH.OrderStyleRefId and ColorRefId=SHD.ColorRefId and SizeRefId=SHD.SizeRefId and CompId=SHD.CompId and Inventory_StyleShipmentDetail.StyleShipmentId=@StyleShipmentId ),0))as ShipmentQty,
SHD.SizeRefId,
(select top(1) SizeName from OM_Size where SizeRefId=SHD.SizeRefId and CompId=SHD.CompId ) as SizeName,
(select top(1) ColorName from OM_Color where ColorRefId=SHD.ColorRefId and CompId=SHD.CompID )as ColorName,
SHD.ColorRefId 
from OM_BuyOrdShip as SH
inner join OM_BuyOrdShipDetail as SHD on SH.OrderShipRefId=SHD.OrderShipRefId
where SH.OrderStyleRefId=@OrderStyleRefId and SH.CompId=@CompId  and SHD.QuantityP>0
group by SH.CompId,SH.OrderStyleRefId,SHD.SizeRefId,SHD.ColorRefId,SHD.CompId
END
