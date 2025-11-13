
CREATE procedure [dbo].[spProdStyleWisePrintEmbStatus]
@orderNo varchar(12),
@compId varchar(3)
as 

select VOM_BuyOrdStyle.BuyerName,
VOM_BuyOrdStyle.RefNo as OrderName,
VOM_BuyOrdStyle.StyleName,OM_Color.ColorName,
(select SUM(QuantityP) from VBuyOrdShipDetail where ColorRefId=OM_Color.ColorRefId  and OrderStyleRefId= VOM_BuyOrdStyle.OrderStyleRefId) as Qty,

(select SUM(Total-RejectQty) from VwCuttingBatch where ComponentRefId='001' and ColorRefId=OM_Color.ColorRefId and OrderStyleRefId= VOM_BuyOrdStyle.OrderStyleRefId) as CutQty,

OM_Component.ComponentName as Part,

ISNULL((select SUM(PDD.Quantity)  from PROD_ProcessDelivery as PD
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
where PD.ProcessRefId='005' and PD.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId   and PDD.ColorRefId=PROD_CuttingSequence.ColorRefId  and PDD.CuttingTagId=PROD_CuttingTag.CuttingTagId),0) as TTLPrintSent,

ISNULL((select SUM(PROD_ProcessReceiveDetail.ReceivedQty-(PROD_ProcessReceiveDetail.FabricReject+PROD_ProcessReceiveDetail.ProcessReject)) as RcvQty from PROD_ProcessReceive
inner join PROD_ProcessReceiveDetail on PROD_ProcessReceive.ProcessReceiveId=PROD_ProcessReceiveDetail.ProcessReceiveId
inner join PROD_CuttingBatch as CB on PROD_ProcessReceiveDetail.CuttingBatchId=CB.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId='005'  and CB.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId  and PROD_ProcessReceiveDetail.CuttingTagId=PROD_CuttingTag.CuttingTagId and PROD_ProcessReceiveDetail.ColorRefId=PROD_CuttingSequence.ColorRefId),0) as TTLPrintRQty,


ISNULL((select SUM(PROD_ProcessReceiveDetail.ProcessReject)  from PROD_ProcessReceive
inner join PROD_ProcessReceiveDetail on PROD_ProcessReceive.ProcessReceiveId=PROD_ProcessReceiveDetail.ProcessReceiveId
inner join PROD_CuttingBatch as CB on PROD_ProcessReceiveDetail.CuttingBatchId=CB.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId='005'  and CB.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId  and PROD_ProcessReceiveDetail.CuttingTagId=PROD_CuttingTag.CuttingTagId and PROD_ProcessReceiveDetail.ColorRefId=PROD_CuttingSequence.ColorRefId),0) as TTLPrintReject,


ISNULL((select SUM(PROD_ProcessReceiveDetail.FabricReject)  from PROD_ProcessReceive
inner join PROD_ProcessReceiveDetail on PROD_ProcessReceive.ProcessReceiveId=PROD_ProcessReceiveDetail.ProcessReceiveId
inner join PROD_CuttingBatch as CB on PROD_ProcessReceiveDetail.CuttingBatchId=CB.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId='005'  and CB.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId  and PROD_ProcessReceiveDetail.CuttingTagId=PROD_CuttingTag.CuttingTagId and PROD_ProcessReceiveDetail.ColorRefId=PROD_CuttingSequence.ColorRefId),0) as TTLPrintFabricReject,


ISNULL((select SUM(PDD.Quantity)  from PROD_ProcessDelivery as PD
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
where PD.ProcessRefId='006'    and PDD.ColorRefId=PROD_CuttingSequence.ColorRefId and  PDD.CuttingTagId=PROD_CuttingTag.CuttingTagId),0) as TTLEmbSent,

ISNULL((select SUM(PROD_ProcessReceiveDetail.ProcessReject) as RcvQty from PROD_ProcessReceive
inner join PROD_ProcessReceiveDetail on PROD_ProcessReceive.ProcessReceiveId=PROD_ProcessReceiveDetail.ProcessReceiveId
inner join PROD_CuttingBatch as CB on PROD_ProcessReceiveDetail.CuttingBatchId=CB.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId='006'  and CB.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and  PROD_ProcessReceiveDetail.CuttingTagId=PROD_CuttingTag.CuttingTagId and PROD_ProcessReceiveDetail.ColorRefId=PROD_CuttingSequence.ColorRefId),0) as TTLEmbReject,

ISNULL((select SUM(PROD_ProcessReceiveDetail.FabricReject) as RcvQty from PROD_ProcessReceive
inner join PROD_ProcessReceiveDetail on PROD_ProcessReceive.ProcessReceiveId=PROD_ProcessReceiveDetail.ProcessReceiveId
inner join PROD_CuttingBatch as CB on PROD_ProcessReceiveDetail.CuttingBatchId=CB.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId='006'  and CB.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and  PROD_ProcessReceiveDetail.CuttingTagId=PROD_CuttingTag.CuttingTagId and PROD_ProcessReceiveDetail.ColorRefId=PROD_CuttingSequence.ColorRefId),0) as TTLEmbFabricReject,


ISNULL((select SUM(PROD_ProcessReceiveDetail.ReceivedQty-(PROD_ProcessReceiveDetail.FabricReject+PROD_ProcessReceiveDetail.ProcessReject)) as RcvQty from PROD_ProcessReceive
inner join PROD_ProcessReceiveDetail on PROD_ProcessReceive.ProcessReceiveId=PROD_ProcessReceiveDetail.ProcessReceiveId
inner join PROD_CuttingBatch as CB on PROD_ProcessReceiveDetail.CuttingBatchId=CB.CuttingBatchId
where PROD_ProcessReceive.ProcessRefId='006'  and CB.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and  PROD_ProcessReceiveDetail.CuttingTagId=PROD_CuttingTag.CuttingTagId and PROD_ProcessReceiveDetail.ColorRefId=PROD_CuttingSequence.ColorRefId),0) as TTLEmbRQty,
(
select SUM(PROD_SewingInputProcessDetail.InputQuantity) from PROD_SewingInputProcess 
inner join  PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId
where PROD_SewingInputProcess.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and PROD_SewingInputProcess.ColorRefId=PROD_CuttingSequence.ColorRefId) as InputQty



from PROD_CuttingTag
inner join PROD_CuttingSequence on PROD_CuttingTag.CuttingSequenceId=PROD_CuttingSequence.CuttingSequenceId
inner join VOM_BuyOrdStyle on  PROD_CuttingSequence.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and PROD_CuttingSequence.CompId=VOM_BuyOrdStyle.CompId
inner join OM_Color on PROD_CuttingSequence.ColorRefId=OM_Color.ColorRefId and PROD_CuttingSequence.CompId=OM_Color.CompId
inner join OM_Component  on PROD_CuttingTag.ComponentRefId=OM_Component.ComponentRefId and PROD_CuttingTag.CompId=OM_Component.CompId
inner join PROD_CuttingProcessStyleActive as SINCUT on VOM_BuyOrdStyle.OrderStyleRefId=SINCUT.OrderStyleRefId and VOM_BuyOrdStyle.CompId=SINCUT.CompId
where PROD_CuttingTag.IsSolid=0 and (PROD_CuttingTag.IsPrint=1 or PROD_CuttingTag.IsEmbroidery=1) and   VOM_BuyOrdStyle.CompId=@compId  and VOM_BuyOrdStyle.OrderNo=@orderNo





