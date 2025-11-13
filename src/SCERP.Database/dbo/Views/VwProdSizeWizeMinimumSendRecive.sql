


CREATE view [dbo].[VwProdSizeWizeMinimumSendRecive]
as
select T.PartyId, T.ProcessRefId, T.CompId,T.OrderStyleRefId, T.ColorRefId,T.SizeRefId,T.OrderQty,MIN(T.Quantity) as Quantity,MIN(T.RejectQty) as RejectQty,MIN(T.ReceiveQty) as ReceiveQty
 from (  select PD.PartyId, PD.ProcessRefId, CB.CompId,  CB.OrderStyleRefId, PDD.CuttingTagId,CB.ColorRefId,
ISNULL((select SUM(OM_BuyOrdShipDetail.QuantityP) from OM_BuyOrdShip
inner join OM_BuyOrdShipDetail on OM_BuyOrdShip.OrderShipRefId=OM_BuyOrdShipDetail.OrderShipRefId 
where OM_BuyOrdShipDetail.ColorRefId=CB.ColorRefId and OM_BuyOrdShipDetail.SizeRefId=PDD.SizeRefId  and OM_BuyOrdShip.OrderStyleRefId=CB.OrderStyleRefId and OM_BuyOrdShip.CompId=CB.CompId and OM_BuyOrdShipDetail.SizeRefId=PDD.SizeRefId),0) as OrderQty
,SUM(PDD.Quantity) as Quantity,PDD.SizeRefId,

ISNULL((select (SUM(PROD_ProcessReceiveDetail.ReceivedQty)-(SUM(PROD_ProcessReceiveDetail.ProcessReject)+SUM(PROD_ProcessReceiveDetail.FabricReject))) from  PROD_ProcessReceiveDetail  
inner join PROD_ProcessReceive on PROD_ProcessReceiveDetail.ProcessReceiveId=PROD_ProcessReceive.ProcessReceiveId
inner join PROD_CuttingBatch on PROD_ProcessReceiveDetail.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId=PD.ProcessRefId and  PROD_CuttingBatch.OrderStyleRefId=CB.OrderStyleRefId and PROD_ProcessReceiveDetail.ColorRefId=CB.ColorRefId and PROD_ProcessReceiveDetail.SizeRefId=PDD.SizeRefId and PROD_ProcessReceiveDetail.CompId=CB.CompId and PROD_ProcessReceiveDetail.CuttingTagId=PDD.CuttingTagId
),0) as ReceiveQty,
ISNULL((select (SUM(PROD_ProcessReceiveDetail.ProcessReject)+SUM(PROD_ProcessReceiveDetail.FabricReject)) from  PROD_ProcessReceiveDetail  
inner join PROD_ProcessReceive on PROD_ProcessReceiveDetail.ProcessReceiveId=PROD_ProcessReceive.ProcessReceiveId
inner join PROD_CuttingBatch on PROD_ProcessReceiveDetail.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId=PD.ProcessRefId and PROD_CuttingBatch.OrderStyleRefId=CB.OrderStyleRefId and PROD_ProcessReceiveDetail.ColorRefId=CB.ColorRefId and PROD_ProcessReceiveDetail.SizeRefId=PDD.SizeRefId and PROD_ProcessReceiveDetail.CompId=CB.CompId and PROD_ProcessReceiveDetail.CuttingTagId=PDD.CuttingTagId 
),0) as RejectQty
from PROD_CuttingBatch as CB
inner join PROD_ProcessDeliveryDetail as PDD on CB.CuttingBatchId=PDD.CuttingBatchId
inner join PROD_ProcessDelivery as PD on PDD.ProcessDeliveryId=PD.ProcessDeliveryId
inner join PROD_CuttingTag  on PDD.CuttingTagId=PROD_CuttingTag.CuttingTagId

group by  PD.PartyId, PD.ProcessRefId, CB.OrderStyleRefId, PDD.CuttingTagId,CB.ColorRefId,CB.OrderStyleRefId,CB.CompId,PDD.SizeRefId
) as T 

group by  T.PartyId, T.ProcessRefId,  T.CompId, T.OrderStyleRefId, T.ColorRefId,T.SizeRefId,T.OrderQty,T.SizeRefId






