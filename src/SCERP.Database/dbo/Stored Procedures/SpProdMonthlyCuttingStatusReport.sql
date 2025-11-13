
CREATE  procedure [dbo].[SpProdMonthlyCuttingStatusReport]
		
			@CompId varchar(3),
			@FromDate datetime,
			@ToDate dateTime
AS
select 
BOST.BuyerName,
BOST.RefNo as OrderNo,
BOST.StyleName ,
(select SUM(QuantityP) from VBuyOrdShipDetail where OrderStyleRefId=CB.OrderStyleRefId and ColorRefId=CB.ColorRefId) as Quantity,
SUM(BC.Quantity) AS CuttingQuantity, 
ISNULL((select sum(RA.RejectQty) FROM PROD_RejectAdjustment AS RA
inner join PROD_CuttingBatch AS ICB ON RA.CuttingBatchId=ICB.CuttingBatchId and RA.CompId=ICB.CompId
where   ICB.CuttingDate>=@FromDate and ICB.CuttingDate<=@ToDate   and  ICB.ColorRefId=CB.ColorRefId and ICB.ComponentRefId='001' and ICB.OrderStyleRefId=CB.OrderStyleRefId and RA.CompId=CB.CompId),0)  as RejectQty
,C.ColorName,
ISNULL((
select SUM(InputQuantity) from PROD_SewingInputProcess
inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId
where PROD_SewingInputProcess.OrderStyleRefId=CB.OrderStyleRefId and PROD_SewingInputProcess.ColorRefId=CB.ColorRefId and    PROD_SewingInputProcess.InputDate>=@FromDate and PROD_SewingInputProcess.InputDate<=@ToDate   ),0) as InputQty,
0 AS CutWIP,
ISNULL((
select SUM(Quantity) from PROD_SewingOutPutProcess
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
where PROD_SewingOutPutProcess.OrderStyleRefId=CB.OrderStyleRefId and PROD_SewingOutPutProcess.ColorRefId=CB.ColorRefId  and  CAST(PROD_SewingOutPutProcess.OutputDate AS DATE)>= CAST(@FromDate AS DATE) and CAST(PROD_SewingOutPutProcess.OutputDate AS DATE)<=CAST(@ToDate AS DATE)),0) AS OutputQty,
0 AS LineWIP
from PROD_CuttingBatch as CB
inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId
inner join VOM_BuyOrdStyle as BOST on CB.OrderStyleRefId=BOST.OrderStyleRefId and CB.CompId=BOST.CompId
inner join OM_Color as C on CB.ColorRefId=C.ColorRefId and CB.CompId=C.CompId
where CB.CuttingDate>=@FromDate and CB.CuttingDate<=@ToDate  and CB.ComponentRefId='001'
group by BOST.BuyerName,BOST.RefNo,BOST.StyleName, C.ColorName, CB.ColorRefId,CB.OrderStyleRefId,CB.CompId

 

  --exec [SpProdMonthlyCuttingStatusReport] '001','2016-11-01','2016-11-13'
