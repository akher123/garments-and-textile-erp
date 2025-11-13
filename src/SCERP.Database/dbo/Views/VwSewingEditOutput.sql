

CREATE View [dbo].[VwSewingEditOutput]
AS
Select SO.SewingOutPutProcessId,
SO.CompId,
SO.OrderStyleRefId,
SO.ColorRefId,
SD.Quantity AS OutputQuantity,
SD.SizeRefId,
OMS.SizeName,
(select SUM(QuantityP) as OrderQty from VBuyOrdShipDetail where OrderStyleRefId=SO.OrderStyleRefId and ColorRefId=SO.ColorRefId and SizeRefId=SD.SizeRefId and CompId=SO.CompId) as OrderQty,
(select  TotalInput from VwSewingFindInput 

where  ColorRefId=SO.ColorRefId and OrderStyleRefId=SO.OrderStyleRefId and SizeRefId=SD.SizeRefId) as TotalInput ,
IsNull((select Sum(Quantity)From PROD_SewingOutPutProcess 
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
where PROD_SewingOutPutProcessDetail.SizeRefId=SD.SizeRefId and  PROD_SewingOutPutProcess.ColorRefId=SO.ColorRefId and PROD_SewingOutPutProcess.CompId=SO.CompId
GROUP BY SizeRefId),0) as TotalOutput

from PROD_SewingOutPutProcess AS SO
INNER JOIN PROD_SewingOutPutProcessDetail AS SD
ON SO.SewingOutPutProcessId=SD.SewingOutPutProcessId AND SO.CompId=SD.CompId
INNER JOIN OM_Size AS OMS
ON SD.SizeRefId=OMS.SizeRefId and SD.CompId=OMS.CompId


