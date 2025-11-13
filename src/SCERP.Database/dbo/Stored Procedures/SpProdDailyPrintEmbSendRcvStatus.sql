CREATE procedure SpProdDailyPrintEmbSendRcvStatus
@CompId varchar(3),
@FilterDate datetime
as

select VOM_BuyOrdStyle.BuyerName,
VOM_BuyOrdStyle.RefNo as OrderName,
VOM_BuyOrdStyle.StyleName,OM_Color.ColorName,
OM_Component.ComponentName as Part,
ISNULL((select  SUM(PDD.Quantity) from PROD_ProcessDelivery as PD
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
where PD.ProcessRefId='005' and PD.InvDate=@FilterDate and PD.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId  and PDD.CuttingTagId=PROD_CuttingTag.CuttingTagId
),0) as TDPrintSent,
ISNULL((select SUM(PDD.Quantity)  from PROD_ProcessDelivery as PD
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
where PD.ProcessRefId='005' and PD.InvDate<=@FilterDate and PD.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId   and PDD.ColorRefId=PROD_CuttingSequence.ColorRefId  and PDD.CuttingTagId=PROD_CuttingTag.CuttingTagId),0) as TTLPrintSent,

ISNULL((select SUM(PROD_ProcessReceiveDetail.ReceivedQty-(PROD_ProcessReceiveDetail.FabricReject+PROD_ProcessReceiveDetail.ProcessReject)) as RcvQty from PROD_ProcessReceive
inner join PROD_ProcessReceiveDetail on PROD_ProcessReceive.ProcessReceiveId=PROD_ProcessReceiveDetail.ProcessReceiveId
inner join PROD_CuttingBatch as CB on PROD_ProcessReceiveDetail.CuttingBatchId=CB.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId='005' and  PROD_ProcessReceive.InvoiceDate=@FilterDate and CB.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId  and PROD_ProcessReceiveDetail.CuttingTagId=PROD_CuttingTag.CuttingTagId and PROD_ProcessReceiveDetail.ColorRefId=PROD_CuttingSequence.ColorRefId),0) as TDPrintRQty,

ISNULL((select SUM(PROD_ProcessReceiveDetail.ReceivedQty-(PROD_ProcessReceiveDetail.FabricReject+PROD_ProcessReceiveDetail.ProcessReject)) as RcvQty from PROD_ProcessReceive
inner join PROD_ProcessReceiveDetail on PROD_ProcessReceive.ProcessReceiveId=PROD_ProcessReceiveDetail.ProcessReceiveId
inner join PROD_CuttingBatch as CB on PROD_ProcessReceiveDetail.CuttingBatchId=CB.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId='005' and  PROD_ProcessReceive.InvoiceDate<=@FilterDate and CB.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId  and PROD_ProcessReceiveDetail.CuttingTagId=PROD_CuttingTag.CuttingTagId and PROD_ProcessReceiveDetail.ColorRefId=PROD_CuttingSequence.ColorRefId),0) as TTLPrintRQty,

ISNULL((select SUM(PDD.Quantity)  from PROD_ProcessDelivery as PD
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
where PD.ProcessRefId='006' and PD.InvDate=@FilterDate   and PDD.ColorRefId=PROD_CuttingSequence.ColorRefId and PDD.CuttingTagId=PROD_CuttingTag.CuttingTagId),0) as TDEmbSent,

ISNULL((select SUM(PDD.Quantity)  from PROD_ProcessDelivery as PD
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
where PD.ProcessRefId='006' and PD.InvDate<=@FilterDate   and PDD.ColorRefId=PROD_CuttingSequence.ColorRefId and  PDD.CuttingTagId=PROD_CuttingTag.CuttingTagId),0) as TTLEmbSent,

ISNULL((select SUM(PROD_ProcessReceiveDetail.ReceivedQty-(PROD_ProcessReceiveDetail.FabricReject+PROD_ProcessReceiveDetail.ProcessReject)) as RcvQty from PROD_ProcessReceive
inner join PROD_ProcessReceiveDetail on PROD_ProcessReceive.ProcessReceiveId=PROD_ProcessReceiveDetail.ProcessReceiveId
inner join PROD_CuttingBatch as CB on PROD_ProcessReceiveDetail.CuttingBatchId=CB.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId='006' and  PROD_ProcessReceive.InvoiceDate=@FilterDate  and CB.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and  PROD_ProcessReceiveDetail.CuttingTagId=PROD_CuttingTag.CuttingTagId and PROD_ProcessReceiveDetail.ColorRefId=PROD_CuttingSequence.ColorRefId),0) as TDEmbRQty,

ISNULL((select SUM(PROD_ProcessReceiveDetail.ReceivedQty-(PROD_ProcessReceiveDetail.FabricReject+PROD_ProcessReceiveDetail.ProcessReject)) as RcvQty from PROD_ProcessReceive
inner join PROD_ProcessReceiveDetail on PROD_ProcessReceive.ProcessReceiveId=PROD_ProcessReceiveDetail.ProcessReceiveId
inner join PROD_CuttingBatch as CB on PROD_ProcessReceiveDetail.CuttingBatchId=CB.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId='006' and  PROD_ProcessReceive.InvoiceDate<=@FilterDate  and CB.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and  PROD_ProcessReceiveDetail.CuttingTagId=PROD_CuttingTag.CuttingTagId and PROD_ProcessReceiveDetail.ColorRefId=PROD_CuttingSequence.ColorRefId),0) as TTLEmbRQty

from PROD_CuttingTag
inner join PROD_CuttingSequence on PROD_CuttingTag.CuttingSequenceId=PROD_CuttingSequence.CuttingSequenceId
inner join VOM_BuyOrdStyle on  PROD_CuttingSequence.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and PROD_CuttingSequence.CompId=VOM_BuyOrdStyle.CompId
inner join OM_Color on PROD_CuttingSequence.ColorRefId=OM_Color.ColorRefId and PROD_CuttingSequence.CompId=OM_Color.CompId
inner join OM_Component  on PROD_CuttingTag.ComponentRefId=OM_Component.ComponentRefId and PROD_CuttingTag.CompId=OM_Component.CompId
inner join PROD_CuttingProcessStyleActive as SINCUT on VOM_BuyOrdStyle.OrderStyleRefId=SINCUT.OrderStyleRefId and VOM_BuyOrdStyle.CompId=SINCUT.CompId
where PROD_CuttingTag.IsSolid=0  and   VOM_BuyOrdStyle.CompId=@CompId   and SINCUT.StartDate<=@FilterDate and (SINCUT.EndDate is null or SINCUT.EndDate>=@FilterDate) 











