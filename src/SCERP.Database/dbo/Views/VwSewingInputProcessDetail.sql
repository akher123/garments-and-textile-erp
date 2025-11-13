
CREATE View [dbo].[VwSewingInputProcessDetail]
AS
Select SI.SewingInputProcessId,SI.CompId,SI.OrderStyleRefId,Si.ColorRefId,Si.LineId,SI.InputDate,SD.InputQuantity,SD.SizeRefId,OMS.SizeName,
(
select top(1) OrderQty from PROD_CutBank

where ColorRefId=SI.ColorRefId and SizeRefId=SD.SizeRefId and OrderStyleRefId=SI.OrderStyleRefId and CompId=SI.CompId) as OrderQty,
(
select SUM( BankQty) from PROD_CutBank

where ColorRefId=SI.ColorRefId and SizeRefId=SD.SizeRefId and OrderStyleRefId=SI.OrderStyleRefId and CompId=SI.CompId 
group by SizeRefId
) as BankQty,
IsNull((select Sum(InputQuantity)From PROD_SewingInputProcess 
inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId
where PROD_SewingInputProcessDetail.SizeRefId=SD.SizeRefId and  PROD_SewingInputProcess.ColorRefId=SI.ColorRefId and PROD_SewingInputProcess.CompId=SI.CompId
GROUP BY SizeRefId),0) as TotalInput,
BSTS.SizeRow
from PROD_SewingInputProcess AS SI
INNER JOIN PROD_SewingInputProcessDetail AS SD
ON SI.SewingInputProcessId=SD.SewingInputProcessId AND SI.CompId=SD.CompId
INNER JOIN OM_Size AS OMS
ON SD.SizeRefId=OMS.SizeRefId and SD.CompId=OMS.CompId

inner join OM_BuyOrdStyleSize as BSTS on SD.SizeRefId=BSTS.SizeRefId and SI.OrderStyleRefId=BSTS.OrderStyleRefId and SI.CompId=BSTS.CompId
