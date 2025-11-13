CREATE view [dbo].[VwSewingFindInputStatus]
as 
select BOS.SizeRefId,
S.SizeName,
Convert(int,SUM(BOS.QuantityP)) as OrderQty, 
BOS.OrderStyleRefId,
BOS.CompId,
BOS.ColorRefId,
C.ColorName,
'' AS OrderShipRefId,
(select ISNULL(SUM(SD.InputQuantity),0) from PROD_SewingInputProcess as SI
inner join PROD_SewingInputProcessDetail as SD on SI.SewingInputProcessId=SD.SewingInputProcessId
where  SI.OrderStyleRefId=BOS.OrderStyleRefId and SI.ColorRefId=BOS.ColorRefId and SD.SizeRefId=BOS.SizeRefId and SI.CompId=BOS.CompId) as TotalInput,
(select ISNULL(SUM(SOD.Quantity),0) from PROD_SewingOutPutProcess as SO 
inner join PROD_SewingOutPutProcessDetail as SOD on SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
where   SO.ColorRefId=BOS.ColorRefId and SO.OrderStyleRefId=BOS.OrderStyleRefId and SOD.SizeRefId=BOS.SizeRefId and SO.CompId=BOS.CompId) as TotalOutput
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

