CREATE procedure [dbo].[SpProdProcessReceiveBalance]
@ProcessRefId varchar(3),
@CuttingBatchId bigint,
@CuttingTagId bigint,
@CompId varchar(3)
as 
select CB.CuttingBatchRefId,PDD.CuttingTagId, CB.CuttingBatchId,C.ColorRefId,SZ.SizeRefId, C.ColorName,SZ.SizeName,SUM(PDD.Quantity) as SendQuantity,
ISNULL((select  sum(PRD.ReceivedQty)  
from PROD_ProcessReceiveDetail as PRD
inner join PROD_ProcessReceive as PR on PRD.ProcessReceiveId=PR.ProcessReceiveId
where PRD.CuttingBatchId=PDD.CuttingBatchId and PRD.CuttingTagId=PDD.CuttingTagId and PRD.ColorRefId=PDD.ColorRefId and PRD.SizeRefId=PDD.SizeRefId and PRD.CompId=PDD.CompId and PR.ProcessRefId=PD.ProcessRefId),0) as RecvQuantity from PROD_ProcessDelivery as PD 
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
inner join PROD_CuttingBatch as CB on PDD.CuttingBatchId=CB.CuttingBatchId
inner join OM_Color as C on PDD.ColorRefId=C.ColorRefId and PDD.CompId=C.CompId
inner join OM_Size as SZ on PDD.SizeRefId=SZ.SizeRefId and PDD.CompId=SZ.CompId
inner join PROD_CuttingTag as CT on PDD.CuttingTagId=CT.CuttingTagId
inner join OM_Component as CM on CT.ComponentRefId=CM.ComponentRefId and PDD.CompId=CM.CompId
inner join OM_BuyOrdStyleSize as BSZ on CB.OrderStyleRefId=BSZ.OrderStyleRefId and SZ.SizeRefId=BSZ.SizeRefId and CB.CompId=BSZ.CompId
where PD.ProcessRefId=@ProcessRefId and PDD.CuttingBatchId=@CuttingBatchId and PD.CompId=@CompId and PDD.CuttingTagId=@CuttingTagId
group by BSZ.SizeRow, C.ColorRefId,SZ.SizeRefId, CB.CuttingBatchId,CB.CuttingBatchRefId,CM.ComponentName,C.ColorName,SZ.SizeName,PDD.ColorRefId,PDD.SizeRefId,PDD.CompId,PDD.CuttingTagId,PDD.CuttingBatchId,PD.ProcessRefId
order by BSZ.SizeRow


