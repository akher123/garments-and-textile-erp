CREATE proc [dbo].[SpPlanStyleWiseSmv] 
@CompId varchar(3)
as
select 
BO.RefNo as OrderNo,
ST.StyleName,
B.BuyerName as Buyer,
OST.Quantity,
Mrc.EmpName as Merchandiser,
ISNULL(smv.StMv,0) as StMv,
round(ISNULL(OST.Quantity,0) * ISNULL(smv.StMv,0),0) as TotalJob 

from OM_BuyerOrder AS BO
inner join OM_BuyOrdStyle as OST on BO.OrderNo=OST.OrderNo and BO.CompId=OST.CompId
inner join OM_Style as ST on OST.StyleRefId =ST.StylerefId
inner join OM_Buyer AS B ON BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Merchandiser as Mrc on BO.MerchandiserId=Mrc.EmpId and BO.CompId=Mrc.CompId 
left join PROD_StanderdMinValue as smv on OST.OrderStyleRefId=smv.OrderStyleRefId and OST.CompId=smv.CompId
where BO.CompId=@CompId
order by BO.BuyerOrderId 