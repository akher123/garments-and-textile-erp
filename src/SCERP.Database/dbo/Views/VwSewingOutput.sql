CREATE VIEW [dbo].[VwSewingOutput]
AS
Select SO.SewingOutPutProcessId,
SO.CompId,
SO.OrderStyleRefId,
SO.ColorRefId,
SO.LineId,
SO.OutputDate,
SO.HourId,
M.MachineId,
M.Name AS MachineName,
H.HourName,
SD.Quantity AS OutputQuantity,
SD.SizeRefId,
OMS.SizeName,
V.BuyerName,
V.RefNo AS OrderName,
V.StyleName,
OMC.ColorName,
(select top(1) OrderQty from PROD_CutBank where ColorRefId=SO.ColorRefId and SizeRefId=SD.SizeRefId and OrderStyleRefId=SO.OrderStyleRefId and CompId=SO.CompId) as OrderQty,
IsNull((select Sum(InputQuantity)From PROD_SewingInputProcess 
inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId
where PROD_SewingInputProcessDetail.SizeRefId=SD.SizeRefId and  PROD_SewingInputProcess.ColorRefId=SO.ColorRefId and PROD_SewingInputProcess.CompId=SO.CompId
GROUP BY SizeRefId),0) as TotalInput,
IsNull((select Sum(Quantity)From PROD_SewingOutPutProcess 
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
where PROD_SewingOutPutProcessDetail.SizeRefId=SD.SizeRefId and  PROD_SewingOutPutProcess.ColorRefId=SO.ColorRefId and PROD_SewingOutPutProcess.CompId=SO.CompId
GROUP BY SizeRefId),0) as TotalOutput
 from PROD_SewingOutPutProcess AS SO
INNER JOIN PROD_SewingOutPutProcessDetail AS SD
ON SO.SewingOutPutProcessId=SD.SewingOutPutProcessId 
INNER JOIN OM_Size AS OMS
ON SD.SizeRefId=OMS.SizeRefId and SD.CompId=OMS.CompId
INNER JOIN Production_Machine AS M
ON SO.LineId=M.MachineId 
INNER JOIN PROD_Hour AS H
ON SO.HourId=H.HourId 
INNER JOIN VOM_BuyOrdStyle AS V
ON SO.OrderStyleRefId=V.OrderStyleRefId AND SO.OrderNo=V.OrderNo AND SO.CompId=V.CompId
INNER JOIN OM_Color AS OMC
ON SO.ColorRefId=OMC.ColorRefId AND SO.CompId=OMC.CompId

