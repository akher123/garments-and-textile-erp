create procedure spGetEmbWorkOrderDetails
@EmbWorkOrderId int 
as 
select 
dtl.EmbWorkOrderId,
dtl.EmbWorkOrderDetailId,

( select top(1) BuyerName from OM_Buyer where CompId=dtl.CompId and BuyerRefId=dtl.BuyerRefId) as Buyer,
( select top(1) RefNo from OM_BuyerOrder where CompId=dtl.CompId and OrderNo=dtl.OrderNo) as [Order],
( select top(1) OM_Style.StyleName from OM_BuyOrdStyle 
 inner join OM_Style on OM_BuyOrdStyle.StyleRefId=OM_Style.StylerefId and OM_BuyOrdStyle.CompId=OM_Style.CompID
 where OM_BuyOrdStyle.CompId=dtl.CompId and OM_BuyOrdStyle.OrderStyleRefId=dtl.OrderStyleRefId ) as Style,
( select top(1) ColorName from OM_Color where ColorRefId=dtl.GColorRefId and CompId=dtl.CompId) as GColorName,
(select top(1) ComponentName from OM_Component where CompId=dtl.CompId and ComponentRefId=dtl.ComponentRefId) as  Sequance,
dtl.FabricType as [FabricType],
dtl.EmbellishmentType ,
dtl.FinishColor, 
dtl.FinishSize ,
dtl.ItemName,
dtl.Qty,
dtl.Rate,
dtl.Qty*dtl.Rate as Amount,
dtl.Remarks
from OM_EmbWorkOrderDetail as dtl

where dtl.EmbWorkOrderId=@EmbWorkOrderId
