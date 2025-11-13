CREATE procedure [dbo].[SpProdProcessDeliveryReport]
@OrderStyleRefId varchar(7),
@ColorRefId varchar(4),
@CuttingTagId bigint,
@ProcessRefId varchar(3),
@CompId varchar(3)

as 
select  
PD.RefNo as ChalanNo,
PD.InvDate as SendDate,
CB.JobNo,
CB.CuttingBatchRefId,
SUM(PDD.Quantity) as SendQty
from PROD_ProcessDelivery as PD
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
inner join PROD_CuttingBatch as CB on PDD.CuttingBatchId=CB.CuttingBatchId
inner join PROD_CuttingTag as CT on PDD.CuttingTagId=CT.CuttingTagId
 where PD.ProcessRefId=@ProcessRefId and   CT.CuttingTagId=@CuttingTagId and CB.ColorRefId=@ColorRefId and   CB.OrderStyleRefId=@OrderStyleRefId and CB.CompId=@CompId 
group by PDD.CuttingTagId, PD.RefNo ,PD.InvDate,CB.JobNo,CB.CuttingBatchRefId





