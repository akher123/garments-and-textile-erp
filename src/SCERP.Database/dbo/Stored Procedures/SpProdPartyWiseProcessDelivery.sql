CREATE procedure [dbo].[SpProdPartyWiseProcessDelivery]
@ProcessRefId varchar(3),
@PartyId bigint,
@CompId varchar(3),
@CuttingBatchRefId varchar(6)

as



select CB.BuyerName,CB.OrderName as OrderNo,CB.StyleName,CB.ColorName, CB.CuttingBatchId,CB.JobNo, CB.CuttingBatchRefId,PDD.CuttingTagId,CM.ComponentName as TagName,SUM(PDD.Quantity) as SendQuantity,
CB.ColorRefId,ISNULL((select  sum(ReceivedQty)  from PROD_ProcessReceiveDetail as PRD
inner join PROD_ProcessReceive as PR on PRD.ProcessReceiveId=PR.ProcessReceiveId
where PRD.CuttingBatchId=PDD.CuttingBatchId and PRD.CuttingTagId=PDD.CuttingTagId  and PRD.CompId=PDD.CompId and PR.ProcessRefId=PD.ProcessRefId),0) as RecvQuantity from PROD_ProcessDelivery as PD 
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
inner join VwCuttingBatch as CB on PDD.CuttingBatchId=CB.CuttingBatchId
inner join PROD_CuttingTag as CT on PDD.CuttingTagId=CT.CuttingTagId
inner join OM_Component as CM on CT.ComponentRefId=CM.ComponentRefId and PDD.CompId=CM.CompId
where PD.ProcessRefId=@ProcessRefId  and PD.CompId=@CompId and  PD.PartyId=@PartyId and ((CB.CuttingBatchRefId LIKE '%'+@CuttingBatchRefId+'%' or @CuttingBatchRefId='NULL' )OR (CB.BuyerName LIKE '%'+@CuttingBatchRefId+'%' or @CuttingBatchRefId='NULL' )OR (CB.OrderName LIKE '%'+@CuttingBatchRefId+'%' or @CuttingBatchRefId='NULL' )OR (CB.StyleName LIKE '%'+@CuttingBatchRefId+'%' or @CuttingBatchRefId='NULL' ))
group by  CB.ColorRefId, CB.BuyerName,CB.OrderName,CB.StyleName,CB.ColorName,PD.ProcessRefId,  CB.CuttingBatchId,CB.JobNo,CB.CuttingBatchRefId,CM.ComponentName,PDD.CompId,PDD.CuttingTagId,PDD.CuttingBatchId

