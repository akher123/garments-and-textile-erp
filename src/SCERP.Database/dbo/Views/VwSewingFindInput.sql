




 CREATE view [dbo].[VwSewingFindInput]
 as 
select  
SI.CompId,
SI.OrderStyleRefId,
 SI.ColorRefId,
SUM(SIDT.InputQuantity) as TotalInput,
SIDT.SizeRefId,
S.SizeName ,
SI.OrderShipRefId,
(select SUM(QuantityP) as OrderQty from VBuyOrdShipDetail where OrderStyleRefId=SI.OrderStyleRefId and ColorRefId=SI.ColorRefId and SizeRefId=SIDT.SizeRefId and CompId=SI.CompId and OrderShipRefId=SI.OrderShipRefId) as OrderQty,
(select ISNULL(SUM(SOD.Quantity),0) from PROD_SewingOutPutProcess as SO 
inner join PROD_SewingOutPutProcessDetail as SOD on SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
where SO.OrderShipRefId= SI.OrderShipRefId and  SO.ColorRefId=SI.ColorRefId and SO.OrderStyleRefId=SI.OrderStyleRefId and SOD.SizeRefId=SIDT.SizeRefId and SO.CompId=SI.CompId) as TotalOutput,
(select ISNULL(SUM(SOD.QcRejectQty),0) from PROD_SewingOutPutProcess as SO 
inner join PROD_SewingOutPutProcessDetail as SOD on SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
where   SO.ColorRefId=SI.ColorRefId and SO.OrderStyleRefId=SI.OrderStyleRefId and SOD.SizeRefId=SIDT.SizeRefId and SO.CompId=SI.CompId) as QcRejectQty
from PROD_SewingInputProcess as SI 
inner join PROD_SewingInputProcessDetail as SIDT on SI.SewingInputProcessId=SIDT.SewingInputProcessId 
inner join OM_Size as S on SIDT.SizeRefId=S.SizeRefId and SIDT.CompId=S.CompId
--where SI.ColorRefId='0142' and SI.OrderStyleRefId='ST00102'
group by SIDT.SizeRefId,S.SizeName,SI.OrderStyleRefId,SI.ColorRefId,SIDT.SizeRefId,SI.CompId,SI.OrderShipRefId



