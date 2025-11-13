CREATE PROCEDURE [dbo].[SpOMCostSheet]
@CompId varchar(3),
@OrderStyleRefId Varchar(7)
as 
select  
CS.CostRefId,
CG.Name as CostGroup,
CD.CostName,
CS.CostRate,
CS.Qty,
CS.Unit,
(ISNULL(OST.Quantity*OST.Rate,0))as Fob,
B.BuyerName,
BO.RefNo as OrderName,
ST.StyleName,
I.ItemName,
BO.Fab,
OST.Quantity,
OST.Rate,
M.EmpName as Merchandiser,
BO.OrderDate,
OST.EFD as ShipDate
from OM_CostOrdStyle AS CS
inner join OM_CostDefination as CD on CS.CostRefId=CD.CostRefId
inner join OM_CostGroup as CG on CD.CostGroup=CG.GroupCode and CS.CompId=CG.COmpId
inner join OM_BuyOrdStyle as OST on CS.OrderStyleRefId=OST.OrderStyleRefId and CS.CompId=OST.CompId
inner join OM_BuyerOrder as BO on OST.OrderNo=BO.OrderNo and OST.CompId=OST.CompId
inner join OM_Merchandiser as M on BO.MerchandiserId=M.EmpId and BO.CompId=M.CompId
inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Style as ST on OST.StyleRefId=St.StylerefId and OST.CompId=ST.CompID
inner join Inventory_Item as I on ST.ItemId=I.ItemId
where CS.OrderStyleRefId=@OrderStyleRefId and CS.CompId=@CompId
order by CG.CostGroupId