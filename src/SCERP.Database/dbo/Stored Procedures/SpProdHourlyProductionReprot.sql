CREATE procedure [dbo].[SpProdHourlyProductionReprot]
@OutputDate datetime,
@CompId varchar(3)
as 
select
BOST.BuyerName,
BOST.RefNo as OrderName,
BOST.StyleName,
C.ColorName,
(M.MachineRefId+'--'+ M.Name) as Line,
--OP.ManPower,
 (select SUM(ManPower) from PROD_SewingOutPutProcess

where OrderStyleRefId=OP.OrderStyleRefId and ColorRefId=OP.ColorRefId and LineId=OP.LineId and CAST(OutputDate AS DATE)=CAST(OP.OutputDate AS DATE))/( select count(ManPower) from PROD_SewingOutPutProcess

where OrderStyleRefId=OP.OrderStyleRefId and ColorRefId=OP.ColorRefId and LineId=OP.LineId and CAST(OutputDate AS  DATE)=CAST(OP.OutputDate AS DATE)) as ManPower,
(H.HourRefId+'--'+H.HourName)  as HourName,
(select top(1) StMv from PROD_StanderdMinValue where OrderStyleRefId=OP.OrderStyleRefId and CompId=OP.CompId) as Smv
,8 as RunningHoure,
--SUM(OPD.Quantity) as Quantity,
(Select SUM(Quantity)  from PROD_SewingOutPutProcessDetail where SewingOutPutProcessId=OP.SewingOutPutProcessId  ) AS Quantity,
OP.OutputDate,
'' as  Remarks,
CP.Name as CompanyName,
CP.FullAddress
from PROD_SewingOutPutProcess as OP
inner join Production_Machine as M on OP.LineId=M.MachineId and OP.CompId=M.CompId
inner join PROD_Hour as H on OP.HourId=H.HourId and OP.CompId=H.CompId
inner join OM_Color as C on OP.ColorRefId=C.ColorRefId and OP.CompId=C.CompId
inner join VOM_BuyOrdStyle as BOST on   OP.OrderStyleRefId=BOST.OrderStyleRefId and OP.CompId=BOST.CompId
inner join Company as CP on OP.CompId=CP.CompanyRefId
where CAST(OP.OutputDate AS DATE)=CAST(@OutputDate AS DATE) and OP.CompId=@CompId and M.IsActive=1
--group by  BOST.BuyerName,BOST.RefNo,BOST.StyleName, C.ColorName, M.Name ,OP.ManPower,H.HourName,OP.CompId,OP.OrderStyleRefId,CP.Name,CP.FullAddress,OP.OutputDate,H.HourRefId,H.HourName,M.MachineId

