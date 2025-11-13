CREATE procedure [dbo].[SpProdGetIronPolyFinishingStatus]
@CompId varchar(3),
@OrderStyleRefId varchar(7),
@ColorRefId varchar(4),
@FinishingProcessId bigint
as
select BOS.SizeRefId,
S.SizeName,
ISNULL(Convert(int,SUM(BOS.QuantityP)),0) as OrderQty, 
BOS.OrderStyleRefId,
BOS.CompId,
BOS.SizeRow,
BOS.ColorRefId,
C.ColorName,
BOS.SizeRow,
ISNULL((select SUM(BC.Quantity)-(ISNULL((select SUM(RA.RejectQty) from PROD_RejectAdjustment as RA
inner join PROD_CuttingBatch as CB1 on RA.CuttingBatchId=CB1.CuttingBatchId
where  CB1.ComponentRefId='001' and CB1.OrderStyleRefId=BOS.OrderStyleRefId and CB1.ColorRefId=BOS.ColorRefId and RA.SizeRefId=BOS.SizeRefId and RA.CompId=BOS.CompId),0)) from PROD_CuttingBatch as CB
inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId
where  CB.OrderStyleRefId=BOS.OrderStyleRefId and CB.ColorRefId=BOS.ColorRefId and BC.SizeRefId=BOS.SizeRefId and CB.ComponentRefId='001'
),0) as TotalCuttQty,
ISNULL((select SUM(SHD.QuantityP)  from OM_BuyOrdShip as SH
inner join OM_BuyOrdShipDetail as SHD on SH.OrderShipRefId=SHD.OrderShipRefId and SH.CompId=SHD.CompId
where SH.OrderStyleRefId=BOS.OrderStyleRefId and SHD.ColorRefId=BOS.ColorRefId and SHD.SizeRefId=BOS.SizeRefId and SH.CompId=BOS.CompId),0) as TtlOrderQty,

ISNULL((select SUM(SIPD.InputQuantity) from PROD_SewingInputProcess as SIP
inner join PROD_SewingInputProcessDetail as SIPD on SIP.SewingInputProcessId=SIPD.SewingInputProcessId
where SIP.OrderStyleRefId=BOS.OrderStyleRefId and SIP.ColorRefId=BOS.ColorRefId and SIPD.SizeRefId=BOS.SizeRefId and SIP.CompId=BOS.CompId),0)as TtlSwInputQty,
ISNULL((select ISNULL(SUM(SOD.Quantity),0) from PROD_SewingOutPutProcess as SO 
inner join PROD_SewingOutPutProcessDetail as SOD on SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
where   SO.ColorRefId=BOS.ColorRefId and SO.OrderStyleRefId=BOS.OrderStyleRefId and SOD.SizeRefId=BOS.SizeRefId and SO.CompId=BOS.CompId),0) AS TtlSwOutQty,
ISNULL((select SUM(FPD.InputQuantity) from PROD_FinishingProcess  as FP
inner join PROD_FinishingProcessDetail as FPD on FP.FinishingProcessId=FPD.FinishingProcessId 
where FP.FType=1 and FP.FinishingProcessId=@FinishingProcessId and FP.CompId=BOS.CompId and FP.ColorRefId=BOS.ColorRefId  and FP.OrderStyleRefId=BOS.OrderStyleRefId  and FPD.SizeRefId=BOS.SizeRefId),0) as InputQuantity,

ISNULL((select SUM(FPD.InputQuantity) from PROD_FinishingProcess  as FP
inner join PROD_FinishingProcessDetail as FPD on FP.FinishingProcessId=FPD.FinishingProcessId 
where FP.FType=1  and FP.CompId=BOS.CompId and FP.ColorRefId=BOS.ColorRefId and FP.OrderStyleRefId=BOS.OrderStyleRefId  and FPD.SizeRefId=BOS.SizeRefId),0) as TinQuantity

from VBuyOrdShipDetail as BOS
inner join OM_Size as S on BOS.SizeRefId=S.SizeRefId and BOS.CompId=S.CompId
inner join OM_Color as C on BOS.ColorRefId=C.ColorRefId and BOS.CompId=C.CompId
where BOS.ColorRefId=@ColorRefId and BOS.OrderStyleRefId=@OrderStyleRefId and BOS.CompId=@CompId
group by   BOS.SizeRefId,
S.SizeName,
BOS.OrderStyleRefId,
BOS.CompId,
BOS.SizeRow,
BOS.ColorRefId,
C.ColorName
order by BOS.SizeRow