
create View [dbo].[VwSewingInputDetailStatus]
AS
select BOS.SizeRefId,
S.SizeName,
Convert(int,SUM(BOS.QuantityP)) as OrderQty, 
BOS.OrderStyleRefId,
BOS.CompId,
BOS.SizeRow,
BOS.ColorRefId,
C.ColorName,
'' AS OrderShipRefId,
(select ISNULL(SUM(SD.InputQuantity),0) from PROD_SewingInputProcess as SI
inner join PROD_SewingInputProcessDetail as SD on SI.SewingInputProcessId=SD.SewingInputProcessId
where  SI.OrderStyleRefId=BOS.OrderStyleRefId and SI.ColorRefId=BOS.ColorRefId and SD.SizeRefId=BOS.SizeRefId and SI.CompId=BOS.CompId) as InputQuantity,
ISNULL((select SUM(BC.Quantity)-(ISNULL((select SUM(RA.RejectQty) from PROD_RejectAdjustment as RA
inner join PROD_CuttingBatch as CB1 on RA.CuttingBatchId=CB1.CuttingBatchId
where  CB1.ComponentRefId='001' and CB1.OrderStyleRefId=BOS.OrderStyleRefId and CB1.ColorRefId=BOS.ColorRefId and RA.SizeRefId=BOS.SizeRefId and RA.CompId=BOS.CompId),0)) from PROD_CuttingBatch as CB
inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId
where  CB.OrderStyleRefId=BOS.OrderStyleRefId and CB.ColorRefId=BOS.ColorRefId and BC.SizeRefId=BOS.SizeRefId and CB.ComponentRefId='001'
),0) as BankQty
from VBuyOrdShipDetail as BOS
inner join OM_Size as S on BOS.SizeRefId=S.SizeRefId and BOS.CompId=S.CompId
inner join OM_Color as C on BOS.ColorRefId=C.ColorRefId and BOS.CompId=C.CompId
group by   BOS.SizeRefId,
S.SizeName,
BOS.OrderStyleRefId,
BOS.CompId,
BOS.SizeRow,
BOS.ColorRefId,
C.ColorName
