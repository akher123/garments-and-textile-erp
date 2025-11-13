
create procedure SpOrderStyleDetailForBOM
@CompId varchar(3),
@OrderStyleRefId varchar(7)
as 

select BOS.EFD as ShipDate,BO.BuyerName, BO.RefNo,BO.OrderNo,BO.OrderDate,BO.EmpName,BO.Fab,BO.Quantity as OrderQty,BO.OTypeName, BOS.BuyerArt as Article,I.ItemName,Stl.StyleName,BOS.Quantity as StyleQty, 
C.ColorName,S.SizeName,SUM(OSD.QuantityP) as Quantity from OM_BuyOrdShipDetail as OSD

inner join OM_BuyOrdShip as OS on OSD.OrderShipRefId=OS.OrderShipRefId and OSD.CompId=OS.CompId

left join OM_Color  as C on OSD.ColorRefId=C.ColorRefId and  OSD.CompId=C.CompId

left join OM_Size as S on OSD.SizeRefId=S.SizeRefId and OSD.CompId=S.CompId
inner join OM_BuyOrdStyle as BOS on OS.OrderStyleRefId=BOS.OrderStyleRefId and OS.CompId=BOS.CompId
inner join OM_Style as Stl on BOS.StyleRefId=stl.StylerefId and BOS.CompId=stl.CompID
inner join Inventory_Item as I on Stl.ItemId=I.ItemId
inner join VBuyerOrder as BO on OS.OrderNo=BO.OrderNo and OS.CompId=BO.CompId
where OSD.CompId=@CompId and OS.OrderStyleRefId=@OrderStyleRefId

group by BOS.EFD , BO.BuyerName, BO.RefNo,BO.OrderNo,BO.OrderDate,BO.EmpName,BO.Fab,BO.Quantity,BO.OTypeName,I.ItemName, BOS.BuyerArt , C.ColorName,S.SizeName,Stl.StyleName,BOS.Quantity
