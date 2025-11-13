CREATE procedure [dbo].[SpProdLineAndSizeWizeSewingInputDetailReport]
@OrderStyleRefId varchar(7),
@ColorRefId varchar(4),
@CompId varchar(3)
as
select  (M.MachineRefId+'--'+ M.Name) as Line,
Convert(varchar(10),(select top(1) SizeRow from VBuyOrdShipDetail

where OrderStyleRefId=SI.OrderStyleRefId and ColorRefId=SI.ColorRefId and SizeRefId=SIPD.SizeRefId and CompId=SI.CompId))+'--'+ S.SizeName as SizeName,
(select count(HourId) from PROD_SewingInputProcess

where OrderStyleRefId=SI.OrderStyleRefId and ColorRefId=SI.ColorRefId and LineId=SI.LineId and InputDate=SI.InputDate) as RunningHoure,
SUM(SIPD.InputQuantity) as Quantity,
SI.InputDate from PROD_SewingInputProcess as SI 
inner join PROD_SewingInputProcessDetail as SIPD on SI.SewingInputProcessId=SIPD.SewingInputProcessId
inner join Production_Machine as M on SI.LineId=M.MachineId
inner join PROD_Hour as H on SI.HourId=H.HourId
inner join OM_Size as S on SIPD.SizeRefId=S.SizeRefId and SI.CompId=S.CompId
inner join VOM_BuyOrdStyle as BOST on   SI.OrderStyleRefId=BOST.OrderStyleRefId and SI.CompId=BOST.CompId
where SI.OrderStyleRefId=@OrderStyleRefId and SI.ColorRefId=@ColorRefId and SI.CompId=@CompId
group by M.MachineRefId,M.Name,SI.OrderStyleRefId,SI.ColorRefId,SI.LineId,SI.InputDate,S.SizeName,SIPD.SizeRefId,SI.CompId


