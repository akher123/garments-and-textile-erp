CREATE procedure [dbo].[SpProdProcessReceive]
@OrderStyleRefId varchar(7),
@ColorRefId varchar(4),
@CuttingTagId bigint,
@ProcessRefId varchar(3),
@CompId varchar(3)
as 
select 
PR.RefNo,
PR.InvoiceNo as RChallanNo ,
PR.InvoiceDate AS ChallanDate,
CB.JobNo,
CB.CuttingBatchRefId,
(select SUM(Quantity) from PROD_ProcessDeliveryDetail  as PDD
inner join PROD_ProcessDelivery as PD on PDD.ProcessDeliveryId=PD.ProcessDeliveryId
where PDD.CuttingBatchId= PRD.CuttingBatchId and PDD.CuttingTagId=PRD.CuttingTagId   and PD.ProcessRefId=PR.ProcessRefId and PDD.CompId=PRD.CompId) as SendingQty,
PRD.ReceivedQty,
PRD.InvocieQty,
PRD.FabricReject,
PRD.ProcessReject
from PROD_ProcessReceive as PR
inner join PROD_ProcessReceiveDetail as PRD on PR.ProcessReceiveId=PRD.ProcessReceiveId
inner join PROD_CuttingBatch as CB on PRD.CuttingBatchId=CB.CuttingBatchId and PR.CompId=CB.CompId
inner join PROD_CuttingTag as CT on PRD.CuttingTagId=CT.CuttingTagId
where PR.ProcessRefId=@ProcessRefId and  CT.CuttingTagId=@CuttingTagId and CB.ColorRefId=@ColorRefId and CB.OrderStyleRefId=@OrderStyleRefId and CB.CompId=@CompId 
order by PRD.ProcessReceiveDetailId  



