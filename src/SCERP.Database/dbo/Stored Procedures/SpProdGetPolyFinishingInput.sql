CREATE procedure [dbo].[SpProdGetPolyFinishingInput]
@CompId varchar(3),
@OrderStyleRefId varchar(7),
@ColorRefId varchar(4),
@FinishingProcessId bigint
as
select 
PFP.CompId,
PFP.OrderStyleRefId,
PFPD.SizeRefId,
PFP.ColorRefId,
BS.SizeRow, 
S.SizeName,
ISNULL((select SUM(SHD.QuantityP)  from OM_BuyOrdShip as SH
inner join OM_BuyOrdShipDetail as SHD on SH.OrderShipRefId=SHD.OrderShipRefId and SH.CompId=SHD.CompId
where SH.OrderStyleRefId=PFP.OrderStyleRefId and SHD.ColorRefId=PFP.ColorRefId and SHD.SizeRefId=PFPD.SizeRefId and SH.CompId=PFP.CompId),0) as TtlOrderQty,

(select SUM(SIPD.InputQuantity) from PROD_SewingInputProcess as SIP
inner join PROD_SewingInputProcessDetail as SIPD on SIP.SewingInputProcessId=SIPD.SewingInputProcessId
where SIP.OrderStyleRefId=PFP.OrderStyleRefId and SIP.ColorRefId=PFP.ColorRefId and SIPD.SizeRefId=PFPD.SizeRefId and SIP.CompId=PFP.CompId)as TtlSwInputQty,


ISNULL((select SUM(PROD_FinishingProcessDetail.InputQuantity) from PROD_FinishingProcess 
inner join PROD_FinishingProcessDetail  on PROD_FinishingProcess.FinishingProcessId=PROD_FinishingProcessDetail.FinishingProcessId
where PROD_FinishingProcess.FinishingProcessId=@FinishingProcessId and PROD_FinishingProcess.FType=2 and PROD_FinishingProcess.CompId=PFP.CompId and PROD_FinishingProcess.ColorRefId=PFP.ColorRefId and PROD_FinishingProcess.OrderStyleRefId=PFP.OrderStyleRefId and PROD_FinishingProcessDetail.SizeRefId=PFPD.SizeRefId),0) as InputQuantity,

ISNULL((select SUM(PROD_FinishingProcessDetail.InputQuantity) from PROD_FinishingProcess 
inner join PROD_FinishingProcessDetail  on PROD_FinishingProcess.FinishingProcessId=PROD_FinishingProcessDetail.FinishingProcessId
where  PROD_FinishingProcess.FType=2 and PROD_FinishingProcess.CompId=PFP.CompId and PROD_FinishingProcess.ColorRefId=PFP.ColorRefId and PROD_FinishingProcess.OrderStyleRefId=PFP.OrderStyleRefId and PROD_FinishingProcessDetail.SizeRefId=PFPD.SizeRefId),0) as TinQuantity,


SUM(PFPD.InputQuantity) as TtlSwOutQty from PROD_FinishingProcess as PFP
inner join PROD_FinishingProcessDetail as PFPD on PFP.FinishingProcessId=PFPD.FinishingProcessId
inner join OM_Size as S on PFPD.SizeRefId=S.SizeRefId and PFPD.CompId=S.CompId
inner join OM_BuyOrdStyleSize as BS on PFPD.SizeRefId=BS.SizeRefId and PFP.OrderStyleRefId=BS.OrderStyleRefId and PFP.CompId=BS.CompId
where PFP.FType=1 and PFP.CompId=@CompId and PFP.OrderStyleRefId=@OrderStyleRefId and PFP.ColorRefId=@ColorRefId
group by   BS.SizeRow, PFP.ColorRefId,PFP.OrderStyleRefId, S.SizeName,PFP.CompId,PFP.OrderStyleRefId,PFP.OrderNo,PFPD.SizeRefId
order by BS.SizeRow


--EXEC SpProdGetPolyFinishingInput '001','ST00095' , '0011' ,'0'


