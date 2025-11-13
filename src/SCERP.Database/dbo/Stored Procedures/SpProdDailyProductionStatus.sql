CREATE procedure [dbo].[SpProdDailyProductionStatus]
@CompId varchar(3),
@FilterDate datetime
as
select 
BOST.BuyerName as BuyerName,
BOST.RefNo as OrderName,
BOST.StyleName as StyleName,

CLR.ColorName as ColorName,
ISNULL((select SUM(QuantityP) from VBuyOrdShipDetail
where OrderStyleRefId=SH.OrderStyleRefId and CompId=SH.CompId and ColorRefId=SHD.ColorRefId),0) as OrderQty ,

ISNULL((select SUM(BD1.Quantity)-(
ISNULL((select sum(RA.RejectQty) FROM PROD_RejectAdjustment AS RA
inner join PROD_CuttingBatch AS ICB ON RA.CuttingBatchId=ICB.CuttingBatchId and RA.CompId=ICB.CompId
where   Convert(date,ICB.CuttingDate)=Convert(date,@FilterDate)and   ICB.ColorRefId=SHD.ColorRefId and ICB.ComponentRefId='001' and ICB.OrderStyleRefId=SH.OrderStyleRefId and RA.CompId=SH.CompId),0)
) FROM PROD_BundleCutting as BD1
inner join PROD_CuttingBatch AS CB1 ON BD1.CuttingBatchId=CB1.CuttingBatchId and BD1.CompId=SH.CompId
where   Convert(date,CB1.CuttingDate)=Convert(date,@FilterDate) and  CB1.ComponentRefId='001'  and CB1.OrderStyleRefId=SH.OrderStyleRefId and BD1.ColorRefId=SHD.ColorRefId 
group by BD1.OrderStyleRefId,BD1.ColorRefId),0)as Cutting,
--ISNULL((select SUM(Total-RejectQty) from VwCuttingBatch where OrderStyleRefId=SH.OrderStyleRefId and CompId=SH.CompId and ColorRefId=SHD.ColorRefId and ComponentRefId='001' and Convert(date,CuttingDate)=Convert(date,@FilterDate)),0) as Cutting,

ISNULL((select SUM(BD1.Quantity)-(
ISNULL((select sum(RA.RejectQty) FROM PROD_RejectAdjustment AS RA
inner join PROD_CuttingBatch AS ICB ON RA.CuttingBatchId=ICB.CuttingBatchId and RA.CompId=ICB.CompId
where    ICB.ColorRefId=SHD.ColorRefId and ICB.ComponentRefId='001' and ICB.OrderStyleRefId=SH.OrderStyleRefId and RA.CompId=SH.CompId),0)
) FROM PROD_BundleCutting as BD1
inner join PROD_CuttingBatch AS CB1 ON BD1.CuttingBatchId=CB1.CuttingBatchId and BD1.CompId=SH.CompId
where  CB1.ComponentRefId='001'  and CB1.OrderStyleRefId=SH.OrderStyleRefId and BD1.ColorRefId=SHD.ColorRefId 
group by BD1.OrderStyleRefId,BD1.ColorRefId),0)as CumCutting,

--ISNULL((select SUM(Total-RejectQty) from VwCuttingBatch where OrderStyleRefId=SH.OrderStyleRefId and CompId=SH.CompId and ComponentRefId='001' and ColorRefId=SHD.ColorRefId),0) as CumCutting,
ISNULL((
select SUM(PROD_ProcessDeliveryDetail.Quantity) from PROD_ProcessDelivery
inner join PROD_ProcessDeliveryDetail on PROD_ProcessDelivery.ProcessDeliveryId=PROD_ProcessDeliveryDetail.ProcessDeliveryId
inner join PROD_CuttingBatch on PROD_ProcessDeliveryDetail.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=SH.OrderStyleRefId and PROD_CuttingBatch.CompId=SH.CompId and PROD_CuttingBatch.ColorRefId=SHD.ColorRefId and PROD_ProcessDelivery.ProcessRefId='005' and  PROD_CuttingBatch.ComponentRefId='001'),0) as PrintSend,



ISNULL((select SUM(ReceivedQty-FabricReject-ProcessReject) from PROD_ProcessReceiveDetail
inner join PROD_ProcessReceive on PROD_ProcessReceiveDetail.ProcessReceiveId= PROD_ProcessReceive.ProcessReceiveId
inner join PROD_CuttingBatch on PROD_ProcessReceiveDetail.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=SH.OrderStyleRefId and PROD_CuttingBatch.CompId=SH.CompId and PROD_CuttingBatch.ColorRefId=SHD.ColorRefId and PROD_ProcessReceive.ProcessRefId='005'  and  PROD_CuttingBatch.ComponentRefId='001' ),0) as PrintRcv,


ISNULL((
select SUM(PROD_ProcessDeliveryDetail.Quantity) from PROD_ProcessDelivery
inner join PROD_ProcessDeliveryDetail on PROD_ProcessDelivery.ProcessDeliveryId=PROD_ProcessDeliveryDetail.ProcessDeliveryId
inner join PROD_CuttingBatch on PROD_ProcessDeliveryDetail.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=SH.OrderStyleRefId and PROD_CuttingBatch.CompId=SH.CompId and PROD_CuttingBatch.ColorRefId=SHD.ColorRefId and PROD_ProcessDelivery.ProcessRefId='006'  and  PROD_CuttingBatch.ComponentRefId='001' ),0) as EmboSend,


--ISNULL((select SUM(TotalQuantity) from VwProcessDelivery where OrderStyleRefId=SH.OrderStyleRefId and VwProcessDelivery.ColorRefId=SHD.ColorRefId and CompId=SH.CompId  and ProcessRefId='006' ),0) as EmboSend,

ISNULL((select SUM(ReceivedQty-FabricReject-ProcessReject) from PROD_ProcessReceiveDetail
inner join PROD_ProcessReceive on PROD_ProcessReceiveDetail.ProcessReceiveId= PROD_ProcessReceive.ProcessReceiveId
inner join PROD_CuttingBatch on PROD_ProcessReceiveDetail.CuttingBatchId=PROD_CuttingBatch.CuttingBatchId
where PROD_CuttingBatch.OrderStyleRefId=SH.OrderStyleRefId and PROD_CuttingBatch.CompId=SH.CompId and PROD_CuttingBatch.ColorRefId=SHD.ColorRefId and PROD_ProcessReceive.ProcessRefId='006'   and  PROD_CuttingBatch.ComponentRefId='001'),0) as EmboRcv,

ISNULL((
select SUM(PROD_SewingInputProcessDetail.InputQuantity) from PROD_SewingInputProcess
inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId  where PROD_SewingInputProcess.OrderStyleRefId=SH.OrderStyleRefId and PROD_SewingInputProcess.CompId=SH.CompId and PROD_SewingInputProcess.ColorRefId=SHD.ColorRefId and   Convert(date,PROD_SewingInputProcess.InputDate)= Convert(date, @FilterDate)),0) as SewInput,

ISNULL((
select SUM(PROD_SewingInputProcessDetail.InputQuantity) from PROD_SewingInputProcess
inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId  where PROD_SewingInputProcess.OrderStyleRefId=SH.OrderStyleRefId and PROD_SewingInputProcess.CompId=SH.CompId and PROD_SewingInputProcess.ColorRefId=SHD.ColorRefId),0) as CumSewInput,

ISNULL((select SUM(PROD_SewingOutPutProcessDetail.Quantity) from PROD_SewingOutPutProcess
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
 where PROD_SewingOutPutProcess.OrderStyleRefId=SH.OrderStyleRefId and PROD_SewingOutPutProcess.CompId=SH.CompId and PROD_SewingOutPutProcess.ColorRefId=SHD.ColorRefId and  Convert(date,PROD_SewingOutPutProcess.OutputDate)=Convert(date,@FilterDate)),0) as SewOutput ,


ISNULL((select SUM(PROD_SewingOutPutProcessDetail.Quantity) from PROD_SewingOutPutProcess
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
 where PROD_SewingOutPutProcess.OrderStyleRefId=SH.OrderStyleRefId and PROD_SewingOutPutProcess.CompId=SH.CompId and PROD_SewingOutPutProcess.ColorRefId=SHD.ColorRefId ),0) as CumSewOutput,


 ISNULL((select SUM(InputQuantity) from PROD_FinishingProcess
inner join PROD_FinishingProcessDetail on PROD_FinishingProcess.FinishingProcessId=PROD_FinishingProcessDetail.FinishingProcessId
where PROD_FinishingProcess.OrderStyleRefId=SH.OrderStyleRefId and PROD_FinishingProcess.CompId=SH.CompId and PROD_FinishingProcess.ColorRefId=SHD.ColorRefId and Convert(date,PROD_FinishingProcess.InputDate)=Convert(date,@FilterDate) and FType=1),0) as IronInput ,

ISNULL((select SUM(PROD_FinishingProcessDetail.InputQuantity) from PROD_FinishingProcess
inner join PROD_FinishingProcessDetail on PROD_FinishingProcess.FinishingProcessId=PROD_FinishingProcessDetail.FinishingProcessId
where PROD_FinishingProcess.OrderStyleRefId=SH.OrderStyleRefId and PROD_FinishingProcess.CompId=SH.CompId and PROD_FinishingProcess.ColorRefId=SHD.ColorRefId  and FType=1),0) as CumIronInput,

ISNULL((select SUM(InputQuantity) from PROD_FinishingProcess
inner join PROD_FinishingProcessDetail on PROD_FinishingProcess.FinishingProcessId=PROD_FinishingProcessDetail.FinishingProcessId
where PROD_FinishingProcess.OrderStyleRefId=SH.OrderStyleRefId and PROD_FinishingProcess.CompId=SH.CompId and PROD_FinishingProcess.ColorRefId=SHD.ColorRefId and Convert(date,PROD_FinishingProcess.InputDate)=Convert(date,@FilterDate) and FType=2),0) as PolyInput,


ISNULL((select SUM(InputQuantity) from PROD_FinishingProcess
inner join PROD_FinishingProcessDetail on PROD_FinishingProcess.FinishingProcessId=PROD_FinishingProcessDetail.FinishingProcessId
where PROD_FinishingProcess.OrderStyleRefId=SH.OrderStyleRefId and PROD_FinishingProcess.CompId=SH.CompId and PROD_FinishingProcess.ColorRefId=SHD.ColorRefId  and FType=2),0) as CumPolyInput,
0 as ShipQty,COM.Name as CompanyName,COM.FullAddress

 FROM OM_BuyOrdShipDetail AS SHD 
INNER JOIN OM_BuyOrdShip AS SH ON SHD.CompId = SH.CompId AND SHD.OrderShipRefId = SH.OrderShipRefId
inner join OM_Color as CLR on SHD.ColorRefId=CLR.ColorRefId and SHD.CompId=CLR.CompId
inner join VOM_BuyOrdStyle as BOST on SH.OrderStyleRefId=BOST.OrderStyleRefId and SH.CompId=BOST.CompId
inner join PROD_CuttingProcessStyleActive as SINCUT on BOST.OrderStyleRefId=SINCUT.OrderStyleRefId and BOST.CompId=SINCUT.CompId
inner join PROD_CuttingBatch as CB on SINCUT.OrderStyleRefId=CB.OrderStyleRefId and SINCUT.CompId=CB.CompId
left join Company as COM on SHD.CompId=COM.CompanyRefId

where  SH.CompId=@CompId and CB.ApprovalStatus='A'   and Convert(date,SINCUT.StartDate)<=Convert(date,@FilterDate) and (SINCUT.EndDate is null or Convert(date,SINCUT.EndDate)>=Convert(date,@FilterDate)) 

group by BOST.BuyerName,BOST.RefNo,BOST.StyleName,CLR.ColorName,SH.OrderStyleRefId,SHD.ColorRefId,SH.CompId,COM.Name,COM.FullAddress




--exec [SpProdDailyProductionStatus] '001','2018-05-27'


