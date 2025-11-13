
CREATE procedure [dbo].[SpDailyProductionStatusSumary]
@CompId varchar(3),
@FilterDate datetime
as
truncate table  PROD_CuttingSummaryReportTable

insert into PROD_CuttingSummaryReportTable (CompId, ViewDate, OrderStyleRefId, ColorRefId,
 OrdQty, TDCuttQty, TTLCuttQty, PanelRejectQty, FinalCuttQty, FinalCutPrc, PrintSent,
  PrintRcv, PrintRcBlance, EmbSent, EmbRcv, EmbRcvBalance,  TDaySewInput, TTLSewInput,
   CuttBank, TDaySewOut, TTLSewOut, SewingWIP)
  select @CompId as  CompId, @FilterDate as  ViewDate, SH.OrderStyleRefId, SHD.ColorRefId,SUM(SHD.QuantityP) OrdQty, 0 as  TDCuttQty, 0 as  TTLCuttQty,0 as  PanelRejectQty,0 as  FinalCuttQty,0 as  FinalCutPrc,0 as  PrintSent,0 as  PrintRcv,0 as  PrintRcBlance,0 as  EmbSent,0 as  EmbRcv,0 as  EmbRcvBalance, 
 0 as TDaySewInput,0 as  TTLSewInput,0 as  CuttBank,0 as  TDaySewOut,0 as  TTLSewOut,0 as  SewingWIP  FROM OM_BuyOrdShipDetail AS SHD 
INNER JOIN OM_BuyOrdShip AS SH ON SHD.CompId = SH.CompId AND SHD.OrderShipRefId = SH.OrderShipRefId
inner join PROD_CuttingProcessStyleActive as SINCUT on SH.OrderStyleRefId=SINCUT.OrderStyleRefId and SH.CompId=SINCUT.CompId
inner join PROD_CuttingBatch as CB on SINCUT.OrderStyleRefId=CB.OrderStyleRefId and SINCUT.CompId=CB.CompId
where  SH.CompId=@CompId  and CB.ApprovalStatus='A' and Convert(date,SINCUT.StartDate)<=Convert(date,@FilterDate) and (convert(date,SINCUT.EndDate) is null or Convert(date,SINCUT.EndDate)>=Convert(date,@FilterDate)) 
group by SH.OrderStyleRefId,SHD.ColorRefId

update PROD_CuttingSummaryReportTable set OrdQty=(select SUM(QuantityP) from VBuyOrdShipDetail where OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and CompId=PROD_CuttingSummaryReportTable.CompId)
update PROD_CuttingSummaryReportTable set TDCuttQty=ISNULL((select SUM(ISNULL(Quantity,0)) from PROD_CuttingBatch
inner join PROD_BundleCutting on PROD_CuttingBatch.CuttingBatchId=PROD_BundleCutting.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and PROD_CuttingBatch.ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and PROD_CuttingBatch.ComponentRefId='001' and PROD_CuttingBatch.CompId=PROD_CuttingSummaryReportTable.CompId and Convert(date,PROD_CuttingBatch.CuttingDate)=Convert(date,PROD_CuttingSummaryReportTable.ViewDate)),0)

update PROD_CuttingSummaryReportTable set TTLCuttQty=ISNULL((select SUM(ISNULL(Quantity,0)) from PROD_CuttingBatch
inner join PROD_BundleCutting on PROD_CuttingBatch.CuttingBatchId=PROD_BundleCutting.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and PROD_CuttingBatch.ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and PROD_CuttingBatch.CompId=PROD_CuttingSummaryReportTable.CompId and PROD_CuttingBatch.ComponentRefId='001' and Convert(date,PROD_CuttingBatch.CuttingDate)<=Convert(date,PROD_CuttingSummaryReportTable.ViewDate)),0)

update PROD_CuttingSummaryReportTable set PanelRejectQty=ISNULL((select SUM(PROD_RejectAdjustment.RejectQty) from PROD_RejectAdjustment 
inner join PROD_CuttingBatch on PROD_RejectAdjustment.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and PROD_CuttingBatch.ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and PROD_CuttingBatch.CompId=PROD_CuttingSummaryReportTable.CompId and PROD_CuttingBatch.ComponentRefId='001' and  Convert(date,PROD_CuttingBatch.CuttingDate)<=Convert(date,PROD_CuttingSummaryReportTable.ViewDate)),0)

update PROD_CuttingSummaryReportTable set FinalCuttQty=PROD_CuttingSummaryReportTable.TTLCuttQty-PROD_CuttingSummaryReportTable.PanelRejectQty

update PROD_CuttingSummaryReportTable set FinalCutPrc=(PROD_CuttingSummaryReportTable.FinalCuttQty*100)/OrdQty

update PROD_CuttingSummaryReportTable set PrintSent=ISNULL((
select SUM(PROD_ProcessDeliveryDetail.Quantity) from PROD_ProcessDelivery
inner join PROD_ProcessDeliveryDetail on PROD_ProcessDelivery.ProcessDeliveryId=PROD_ProcessDeliveryDetail.ProcessDeliveryId
inner join PROD_CuttingBatch on PROD_ProcessDeliveryDetail.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and PROD_CuttingBatch.CompId=PROD_CuttingSummaryReportTable.CompId and PROD_CuttingBatch.ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and PROD_ProcessDelivery.ProcessRefId='005' and Convert(date,PROD_ProcessDelivery.InvDate)<=Convert(date,PROD_CuttingSummaryReportTable.ViewDate) ),0) 


update PROD_CuttingSummaryReportTable set PrintRcv=ISNULL((select SUM(ReceivedQty-FabricReject-ProcessReject) from PROD_ProcessReceiveDetail
inner join PROD_ProcessReceive on PROD_ProcessReceiveDetail.ProcessReceiveId= PROD_ProcessReceive.ProcessReceiveId
inner join PROD_CuttingBatch on PROD_ProcessReceiveDetail.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and PROD_CuttingBatch.CompId=PROD_CuttingSummaryReportTable.CompId and PROD_CuttingBatch.ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and PROD_ProcessReceive.ProcessRefId='005' and Convert(date,PROD_ProcessReceive.InvoiceDate)<=Convert(date,PROD_CuttingSummaryReportTable.ViewDate)),0)

update PROD_CuttingSummaryReportTable set PrintRcBlance=PROD_CuttingSummaryReportTable.PrintRcv-PROD_CuttingSummaryReportTable.PrintSent

update PROD_CuttingSummaryReportTable set EmbSent=ISNULL((
select SUM(PROD_ProcessDeliveryDetail.Quantity) from PROD_ProcessDelivery
inner join PROD_ProcessDeliveryDetail on PROD_ProcessDelivery.ProcessDeliveryId=PROD_ProcessDeliveryDetail.ProcessDeliveryId
inner join PROD_CuttingBatch on PROD_ProcessDeliveryDetail.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and PROD_CuttingBatch.CompId=PROD_CuttingSummaryReportTable.CompId and PROD_CuttingBatch.ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and PROD_ProcessDelivery.ProcessRefId='006' and Convert(date,PROD_ProcessDelivery.InvDate)<=Convert(date,PROD_CuttingSummaryReportTable.ViewDate) ),0) 

update PROD_CuttingSummaryReportTable set EmbRcv=ISNULL((select SUM(ReceivedQty-FabricReject-ProcessReject) from PROD_ProcessReceiveDetail
inner join PROD_ProcessReceive on PROD_ProcessReceiveDetail.ProcessReceiveId= PROD_ProcessReceive.ProcessReceiveId
inner join PROD_CuttingBatch on PROD_ProcessReceiveDetail.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and PROD_CuttingBatch.CompId=PROD_CuttingSummaryReportTable.CompId and PROD_CuttingBatch.ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and PROD_ProcessReceive.ProcessRefId='006' and Convert(date,PROD_ProcessReceive.InvoiceDate)<=Convert(date,PROD_CuttingSummaryReportTable.ViewDate) ),0)

update PROD_CuttingSummaryReportTable set EmbRcvBalance=PROD_CuttingSummaryReportTable.EmbRcv-PROD_CuttingSummaryReportTable.EmbSent


update PROD_CuttingSummaryReportTable set TDaySewInput=ISNULL((
select SUM(PROD_SewingInputProcessDetail.InputQuantity) from PROD_SewingInputProcess
inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId  where PROD_SewingInputProcess.OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and PROD_SewingInputProcess.CompId=PROD_CuttingSummaryReportTable.CompId and PROD_SewingInputProcess.ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and   Convert(date,PROD_SewingInputProcess.InputDate)= Convert(date, PROD_CuttingSummaryReportTable.ViewDate)),0) 

update PROD_CuttingSummaryReportTable set TTLSewInput=ISNULL((
select SUM(PROD_SewingInputProcessDetail.InputQuantity) from PROD_SewingInputProcess
inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId  where PROD_SewingInputProcess.OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and PROD_SewingInputProcess.CompId=PROD_CuttingSummaryReportTable.CompId and PROD_SewingInputProcess.ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and   Convert(date,PROD_SewingInputProcess.InputDate)<= Convert(date, PROD_CuttingSummaryReportTable.ViewDate)),0) 

update PROD_CuttingSummaryReportTable set TDaySewOut=ISNULL((select SUM(PROD_SewingOutPutProcessDetail.Quantity) from PROD_SewingOutPutProcess
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
 where PROD_SewingOutPutProcess.OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and PROD_SewingOutPutProcess.CompId=PROD_CuttingSummaryReportTable.CompId and PROD_SewingOutPutProcess.ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and  Convert(date,PROD_SewingOutPutProcess.OutputDate)=Convert(date,PROD_CuttingSummaryReportTable.ViewDate)),0)

update PROD_CuttingSummaryReportTable set TTLSewOut=ISNULL((select SUM(PROD_SewingOutPutProcessDetail.Quantity) from PROD_SewingOutPutProcess
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
 where PROD_SewingOutPutProcess.OrderStyleRefId=PROD_CuttingSummaryReportTable.OrderStyleRefId and PROD_SewingOutPutProcess.CompId=PROD_CuttingSummaryReportTable.CompId and PROD_SewingOutPutProcess.ColorRefId=PROD_CuttingSummaryReportTable.ColorRefId and  Convert(date,PROD_SewingOutPutProcess.OutputDate)<=Convert(date,PROD_CuttingSummaryReportTable.ViewDate) ),0) 

 select BST.BuyerName,BST.RefNo as OrderNo,BST.StyleName,C.ColorName,CSR.OrdQty, CSR.TDCuttQty, CSR.TTLCuttQty, CSR.PanelRejectQty, CSR.FinalCuttQty, CSR.FinalCutPrc, CSR.PrintSent, CSR.PrintRcv, CSR.PrintRcBlance, CSR.EmbSent, CSR.EmbRcv, CSR.EmbRcvBalance, 
                         CSR.TDaySewInput, CSR.TTLSewInput, CSR.CuttBank, CSR.TDaySewOut, CSR.TTLSewOut, CSR.SewingWIP
 from PROD_CuttingSummaryReportTable as CSR
 inner join VOM_BuyOrdStyle as BST on CSR.OrderStyleRefId=BST.OrderStyleRefId and CSR.CompId=BST.CompId
 inner join OM_Color as C on CSR.ColorRefId=C.ColorRefId and CSR.CompId=C.CompId

 order by BST.BuyerName,BST.RefNo ,BST.StyleName,C.ColorName

