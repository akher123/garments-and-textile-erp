CREATE procedure [dbo].[SpinventoryShipStatus]
@buyerRefId varchar(4)=null,
@CompId varchar(3)
as
select B.BuyerName,BO.RefNo as OrderName,ST.StyleName,
RTRIM(LTRIM(I.ItemName))as ItemName,SUM(SD.ShipmentQty) as ShipQty,
SUM( SD.ShipmentQty*OST.Rate) as ShipValue ,Sum( distinct OST.Quantity) as OrdQty,Sum( distinct OST.Quantity*OST.Rate) as OrdValue 
 from Inventory_StyleShipmentDetail as SD
inner join OM_BuyOrdStyle as OST on SD.OrderStyleRefId=OST.OrderStyleRefId and SD.CompId=OST.CompId
inner join OM_BuyerOrder as BO on OST.OrderNo=BO.OrderNo and OST.CompId=BO.CompId
inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Style as ST on OST.StyleRefId=St.StylerefId and OST.CompId=ST.CompID
inner join Inventory_Item as I on ST.ItemId=I.ItemId
where (BO.BuyerRefId=@buyerRefId or @buyerRefId is null) and SD.CompId=@CompId
group by B.BuyerName,BO.RefNo ,ST.StyleName,I.ItemName







 
