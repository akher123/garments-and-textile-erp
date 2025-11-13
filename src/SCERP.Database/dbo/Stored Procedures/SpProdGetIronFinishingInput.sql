CREATE procedure [dbo].[SpProdGetIronFinishingInput]
@CompId varchar(3),
@OrderStyleRefId varchar(7),
@ColorRefId varchar(4),
@FinishingProcessId bigint
as
select 
SOP.CompId,
SOP.OrderStyleRefId,
SOPD.SizeRefId,
SOP.ColorRefId,
BS.SizeRow, 
S.SizeName,
0 AS TotalCuttQty,
ISNULL((select SUM(SHD.QuantityP)  from OM_BuyOrdShip as SH
inner join OM_BuyOrdShipDetail as SHD on SH.OrderShipRefId=SHD.OrderShipRefId and SH.CompId=SHD.CompId
where SH.OrderStyleRefId=SOP.OrderStyleRefId and SHD.ColorRefId=SOP.ColorRefId and SHD.SizeRefId=SOPD.SizeRefId and SH.CompId=SOP.CompId),0) as TtlOrderQty,

(select SUM(SIPD.InputQuantity) from PROD_SewingInputProcess as SIP
inner join PROD_SewingInputProcessDetail as SIPD on SIP.SewingInputProcessId=SIPD.SewingInputProcessId
where SIP.OrderStyleRefId=SOP.OrderStyleRefId and SIP.ColorRefId=SOP.ColorRefId and SIPD.SizeRefId=SOPD.SizeRefId and SIP.CompId=SOP.CompId)as TtlSwInputQty,

SUM(SOPD.Quantity) as TtlSwOutQty,
ISNULL((select SUM(FPD.InputQuantity) from PROD_FinishingProcess  as FP
inner join PROD_FinishingProcessDetail as FPD on FP.FinishingProcessId=FPD.FinishingProcessId 
where FP.FType=1 and FP.FinishingProcessId=@FinishingProcessId and FP.CompId=SOP.CompId and FP.ColorRefId=SOP.ColorRefId  and FP.OrderStyleRefId=SOP.OrderStyleRefId and FP.OrderNo=SOP.OrderNo and FPD.SizeRefId=SOPD.SizeRefId),0) as InputQuantity,

ISNULL((select SUM(FPD.InputQuantity) from PROD_FinishingProcess  as FP
inner join PROD_FinishingProcessDetail as FPD on FP.FinishingProcessId=FPD.FinishingProcessId 
where FP.FType=1  and FP.CompId=SOP.CompId and FP.ColorRefId=SOP.ColorRefId and FP.OrderStyleRefId=SOP.OrderStyleRefId and FP.OrderNo=SOP.OrderNo and FPD.SizeRefId=SOPD.SizeRefId),0) as TinQuantity

from PROD_SewingOutPutProcess as SOP
inner join PROD_SewingOutPutProcessDetail as SOPD on SOP.SewingOutPutProcessId=SOPD.SewingOutPutProcessId
inner join OM_Size as S on SOPD.SizeRefId=S.SizeRefId and SOPD.CompId=S.CompId
inner join OM_BuyOrdStyleSize as BS on SOPD.SizeRefId=BS.SizeRefId and SOP.OrderStyleRefId=BS.OrderStyleRefId and SOP.CompId=BS.CompId
where SOP.ColorRefId=@ColorRefId and SOP.OrderStyleRefId=@OrderStyleRefId and SOP.CompId=@CompId
group by  BS.SizeRow, SOP.ColorRefId,SOP.OrderStyleRefId, S.SizeName,SOP.CompId,SOP.OrderStyleRefId,SOP.OrderNo,SOPD.SizeRefId
order by BS.SizeRow

--Exec SpProdGetIronFinishingInput '001','ST00095','0011',1



