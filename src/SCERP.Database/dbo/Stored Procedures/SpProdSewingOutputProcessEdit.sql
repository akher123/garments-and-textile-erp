CREATE procedure [dbo].[SpProdSewingOutputProcessEdit]
@SewingOutPutProcessId bigint ,
@CompId varchar(3)
AS
declare @orderStyleRefId varchar(7),@ColorRefId varchar(4)
set @orderStyleRefId=(select top(1) OrderStyleRefId from PROD_SewingOutPutProcess where SewingOutPutProcessId=@SewingOutPutProcessId )
set @ColorRefId=(select top(1) ColorRefId from PROD_SewingOutPutProcess where SewingOutPutProcessId=@SewingOutPutProcessId)
Select 

SD.SizeRefId,
S.SizeName as  SizeName,

--(select top(1) OrderQty from PROD_CutBank where ColorRefId=SD.ColorRefId and SizeRefId=SD.SizeRefId and OrderStyleRefId=SH.OrderStyleRefId and CompId=SH.CompId) as OrderQty,
sum(SD.QuantityP) as OrderQty,
IsNull((select Sum(InputQuantity)From PROD_SewingInputProcess 
inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId
where PROD_SewingInputProcessDetail.SizeRefId=SD.SizeRefId and  PROD_SewingInputProcess.ColorRefId=SD.ColorRefId and PROD_SewingInputProcess.CompId=SH.CompId
GROUP BY SizeRefId),0) as TotalInput,

IsNull((select Sum(Quantity)From PROD_SewingOutPutProcess 
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
where PROD_SewingOutPutProcessDetail.SizeRefId=SD.SizeRefId and  PROD_SewingOutPutProcess.ColorRefId=SD.ColorRefId and PROD_SewingOutPutProcess.CompId=SH.CompId
GROUP BY SizeRefId),0) as TotalOutput,

ISNULL((select PROD_SewingOutPutProcessDetail.Quantity from PROD_SewingOutPutProcess
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
where PROD_SewingOutPutProcess.SewingOutPutProcessId=@SewingOutPutProcessId and PROD_SewingOutPutProcess.ColorRefId=SD.ColorRefId  and PROD_SewingOutPutProcess.OrderStyleRefId=SH.OrderStyleRefId and  PROD_SewingOutPutProcess.CompId=SD.CompId and PROD_SewingOutPutProcessDetail.SizeRefId=SD.SizeRefId),0) as OutputQuantity

from OM_BuyOrdShip as SH 
inner join OM_BuyOrdShipDetail as SD on SH.OrderShipRefId=SD.OrderShipRefId and SH.CompId=SD.CompId
inner join OM_Size as S on SD.SizeRefId=S.SizeRefId and SD.CompId=S.CompId
inner join OM_BuyOrdStyleSize as BS on SH.OrderStyleRefId=BS.OrderStyleRefId and SD.CompId=BS.CompId and SD.SizeRefId=BS.SizeRefId
where SH.OrderStyleRefId =@orderStyleRefId and SD.ColorRefId=@ColorRefId and SH.CompId=@CompId
group by SD.SizeRefId,
S.SizeName,SD.ColorRefId,SH.CompId,SH.OrderStyleRefId,SD.CompId


