CREATE procedure SpProdPrintEmbroieryBalanceSummary
@CompId varchar(3),
@OrderStyleRefId varchar(7)
as 
select iif(PD.ProcessRefId='005','PRINT','EMBROIDERY') as ProcessName,
 p.Name as Party, 
 CB.BuyerName,
 CB.OrderName,
 CB.StyleName,
 CB.OrderStyleRefId,
 SUM(PDD.Quantity) as SendQty,
 ISNULL((select (SUM(ReceivedQty)-(SUM(ProcessReject)+SUM(FabricReject)))  from PROD_ProcessReceive 
inner join PROD_ProcessReceiveDetail on PROD_ProcessReceive.ProcessReceiveId=PROD_ProcessReceiveDetail.ProcessReceiveId
inner join PROD_CuttingBatch on PROD_ProcessReceiveDetail.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=PD.OrderStyleRefId and PROD_ProcessReceive.ProcessRefId=PD.ProcessRefId),0)as RcvQty
 from PROD_ProcessDelivery as PD 
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
inner join Party as P on PD.PartyId=P.PartyId
inner join VwCuttingBatch as CB on PDD.CuttingBatchId=CB.CuttingBatchId
--where PD.CompId=@CompId and PD.OrderStyleRefId=@OrderStyleRefId
group by PD.ProcessRefId, p.Name, CB.BuyerName,CB.OrderName,CB.StyleName,PD.OrderStyleRefId, CB.OrderStyleRefId


