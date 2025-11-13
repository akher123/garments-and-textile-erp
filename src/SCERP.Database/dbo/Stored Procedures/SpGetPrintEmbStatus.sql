CREATE procedure SpGetPrintEmbStatus
@OrderStyleRefId varchar(7),
@ColorRefId varchar(4),
@ProcessRefId varchar(3)
as 
select BOS.SizeRefId,
S.SizeName,
Convert(int,SUM(BOS.QuantityP)) as OrderQty, 
BOS.OrderStyleRefId,
BOS.CompId,
BOS.ColorRefId,
C.ColorName,
ISNULL((select SUM(PD.Quantity) from PROD_ProcessDeliveryDetail AS PD
INNER JOIN PROD_ProcessDelivery as P ON PD.ProcessDeliveryId=P.ProcessDeliveryId
INNER JOIN PROD_CuttingBatch AS CB ON PD.CuttingBatchId=CB.CuttingBatchId
WHERE PD.ColorRefId=BOS.ColorRefId and PD.SizeRefId=BOS.SizeRefId AND CB.OrderStyleRefId=BOS.OrderStyleRefId AND P.ProcessRefId=@ProcessRefId),0) as SentQty,
ISNULL((select SUM(PR.InvocieQty) from PROD_ProcessReceiveDetail AS PR
INNER JOIN PROD_ProcessReceive as R ON PR.ProcessReceiveId=R.ProcessReceiveId
INNER JOIN PROD_CuttingBatch AS CB ON PR.CuttingBatchId=CB.CuttingBatchId
WHERE PR.ColorRefId=BOS.ColorRefId and PR.SizeRefId=BOS.SizeRefId AND CB.OrderStyleRefId=BOS.OrderStyleRefId and R.ProcessRefId=@ProcessRefId),0) as InvQty,

ISNULL((select SUM(PR.FabricReject) from PROD_ProcessReceiveDetail AS PR
INNER JOIN PROD_ProcessReceive as R ON PR.ProcessReceiveId=R.ProcessReceiveId
INNER JOIN PROD_CuttingBatch AS CB ON PR.CuttingBatchId=CB.CuttingBatchId
WHERE PR.ColorRefId=BOS.ColorRefId and PR.SizeRefId=BOS.SizeRefId AND CB.OrderStyleRefId=BOS.OrderStyleRefId and R.ProcessRefId=@ProcessRefId),0) as FabReject,

ISNULL((select SUM(PR.ProcessReject) from PROD_ProcessReceiveDetail AS PR
INNER JOIN PROD_ProcessReceive as R ON PR.ProcessReceiveId=R.ProcessReceiveId
INNER JOIN PROD_CuttingBatch AS CB ON PR.CuttingBatchId=CB.CuttingBatchId
WHERE PR.ColorRefId=BOS.ColorRefId and PR.SizeRefId=BOS.SizeRefId AND CB.OrderStyleRefId=BOS.OrderStyleRefId and R.ProcessRefId=@ProcessRefId),0) as ProcesReject
from VBuyOrdShipDetail as BOS
inner join OM_Size as S on BOS.SizeRefId=S.SizeRefId and BOS.CompId=S.CompId
inner join OM_Color as C on BOS.ColorRefId=C.ColorRefId and BOS.CompId=C.CompId
where BOS.OrderStyleRefId=@OrderStyleRefId AND BOS.ColorRefId=@ColorRefId 
group by   BOS.SizeRefId,
S.SizeName,
BOS.OrderStyleRefId,
BOS.CompId,
BOS.SizeRow,
BOS.ColorRefId,
C.ColorName
order by BOS.SizeRow
