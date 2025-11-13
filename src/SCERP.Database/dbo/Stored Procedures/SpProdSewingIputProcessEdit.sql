CREATE  procedure [dbo].[SpProdSewingIputProcessEdit]
@SewingInputProcessId bigint ,
@CompId varchar(3)
as 
declare @orderStyleRefId varchar(7),@ColorRefId varchar(4)
set @orderStyleRefId=(select top(1) OrderStyleRefId from PROD_SewingInputProcess where SewingInputProcessId=@SewingInputProcessId)
set @ColorRefId=(select top(1) ColorRefId from PROD_SewingInputProcess where SewingInputProcessId=@SewingInputProcessId)
select 
SD.CompId,
SH.OrderStyleRefId,
SD.SizeRefId,
SD.ColorRefId,
SUM(SD.QuantityP) as OrderQty,
--(select top(1) OrderQty from PROD_CutBank where ColorRefId=SD.ColorRefId and SizeRefId=SD.SizeRefId and OrderStyleRefId=SH.OrderStyleRefId and CompId=SH.CompId) as OrderQty,
(select SUM( BankQty) from PROD_CutBank where ColorRefId=SD.ColorRefId and SizeRefId=SD.SizeRefId and OrderStyleRefId=SH.OrderStyleRefId and CompId=SH.CompId group by SizeRefId) as BankQty,
IsNull((select Sum(InputQuantity)From PROD_SewingInputProcess 
inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId
where PROD_SewingInputProcessDetail.SizeRefId=SD.SizeRefId and  PROD_SewingInputProcess.ColorRefId=SD.ColorRefId and PROD_SewingInputProcess.CompId=SH.CompId
GROUP BY SizeRefId),0) as TotalInput,
BS.SizeRow as SizeRow, 
ISNULL((select PROD_SewingInputProcessDetail.InputQuantity from PROD_SewingInputProcess
inner join PROD_SewingInputProcessDetail on PROD_SewingInputProcess.SewingInputProcessId=PROD_SewingInputProcessDetail.SewingInputProcessId
where PROD_SewingInputProcess.SewingInputProcessId=@SewingInputProcessId and PROD_SewingInputProcess.ColorRefId=SD.ColorRefId  and PROD_SewingInputProcess.OrderStyleRefId=SH.OrderStyleRefId and  PROD_SewingInputProcess.CompId=SD.CompId and PROD_SewingInputProcessDetail.SizeRefId=SD.SizeRefId),0) as InputQuantity,
S.SizeName as  SizeName
from OM_BuyOrdShip as SH 
inner join OM_BuyOrdShipDetail as SD on SH.OrderShipRefId=SD.OrderShipRefId and SH.CompId=SD.CompId
inner join OM_Size as S on SD.SizeRefId=S.SizeRefId and SD.CompId=S.CompId
inner join OM_BuyOrdStyleSize as BS on SH.OrderStyleRefId=BS.OrderStyleRefId and SD.CompId=BS.CompId and SD.SizeRefId=BS.SizeRefId
where SH.OrderStyleRefId =@orderStyleRefId and SD.ColorRefId=@ColorRefId and SH.CompId=@CompId
group by SD.CompId,
SH.OrderStyleRefId,
SD.SizeRefId,
SD.ColorRefId,SH.CompId,BS.SizeRow,S.SizeName