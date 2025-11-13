CREATE procedure [dbo].[SpSizeAndLineWizeOutputSumamaryReport]
@OrderStyleRefId varchar(7),
@ColorRefId varchar(4),
@CompId varchar(3)
as
select
BOST.BuyerName,
BOST.StyleName,
BOST.RefNo as OrderName,
SUM(OSD.QuantityP) as OrderQty,
Convert(varchar(5), OSD.SizeRow )+'--'+S.SizeName as SizeName,
C.ColorName ,
(select ISNULL(SUM(SD.InputQuantity),0) as TInputQty from PROD_SewingInputProcess as SI 
inner join PROD_SewingInputProcessDetail as SD on SI.SewingInputProcessId=SD.SewingInputProcessId
where SI.OrderStyleRefId=OS.OrderStyleRefId and SI.ColorRefId=OSD.ColorRefId and SD.SizeRefId=OSD.SizeRefId and SD.CompId=OSD.CompId) as TInputQty,
(select ISNULL(SUM(SOPD.Quantity),0)  from PROD_SewingOutPutProcess as SOP
inner join PROD_SewingOutPutProcessDetail as SOPD on SOP.SewingOutPutProcessId=SOPD.SewingOutPutProcessId
where SOP.OrderStyleRefId=OS.OrderStyleRefId and SOP.ColorRefId=OSD.ColorRefId and SOPD.SizeRefId=OSD.SizeRefId and SOPD.CompId=OSD.CompId) as ToutPutQty,
0 as SewingRejectQty,
(select  ISNULL(count(distinct SOP.LineId),0)  from PROD_SewingOutPutProcess as SOP
where SOP.OrderStyleRefId=OS.OrderStyleRefId and SOP.ColorRefId=OSD.ColorRefId  and SOP.CompId=OS.CompId) as LineNumber,
(select ISNULL(count(SOP.HourId),0)  from PROD_SewingOutPutProcess as SOP
where SOP.OrderStyleRefId=OS.OrderStyleRefId and SOP.ColorRefId=OSD.ColorRefId  and SOP.CompId=OS.CompId) as RuningHr,
(select top(1) StMv from PROD_StanderdMinValue where OrderStyleRefId=OS.OrderStyleRefId and CompId=OS.CompId) as Smv
from OM_BuyOrdShip as OS
inner join OM_BuyOrdShipDetail as OSD on OS.OrderShipRefId=OSD.OrderShipRefId and OS.CompId=OSD.CompId
inner join OM_Size as S on OSD.SizeRefId=S.SizeRefId and OSD.CompId=S.CompId
inner join OM_Color as C on OSD.ColorRefId=C.ColorRefId and OSD.CompId=C.CompId
inner join VOM_BuyOrdStyle as BOST on OS.OrderStyleRefId=BOST.OrderStyleRefId and OS.CompId=BOST.CompId
where OS.OrderStyleRefId=@OrderStyleRefId and OSD.ColorRefId=@ColorRefId and OS.CompId=@CompId
group by BOST.BuyerName,
BOST.StyleName,
BOST.RefNo ,
OSD.SizeRow,C.ColorName,S.SizeName,OS.OrderStyleRefId,OSD.ColorRefId,OSD.SizeRefId,OSD.CompId,OS.CompId

