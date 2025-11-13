create procedure spLineCapacityUtilization
as

Delete From PLAN_CapacityUtilization where OutputDate = '2017-12-05'

INSERT INTO PLAN_CapacityUtilization
                         (CompId, LineId, OutputDate, UsedHour, NoMachine, OutputMin)
SELECT        CompId, MachineId AS LineId, '2017-12-05' AS OutputDate, 0 AS UsedHour, NoMachine, 0 AS OutpurMin
FROM            Production_Machine
WHERE        (ProcessorRefId IN ('003', '004', '007')) AND (CompId = '001') AND (IsActive = 1)

Update PLAN_CapacityUtilization set UsedHour= isnull(( select count(distinct SOP1.HourId) as Nhurs from PROD_SewingOutPutProcess as SOP1 
inner join PROD_SewingOutPutProcessDetail as SOPD1 on SOP1.SewingOutPutProcessId=SOPD1.SewingOutPutProcessId
where SOP1.OutputDate=PLAN_CapacityUtilization.OutputDate 
and   SOP1.LineId=PLAN_CapacityUtilization.LineId and SOPD1.Quantity> 0 ),0) where OutputDate ='2017-12-05'

Update PLAN_CapacityUtilization set OutputMin= isnull(( select SUM(SOPD.Quantity*SMV.StMv) as TTLAM
  from PROD_SewingOutPutProcess as SOP 
inner join PROD_SewingOutPutProcessDetail as SOPD on SOP.SewingOutPutProcessId=SOPD.SewingOutPutProcessId
inner join PROD_StanderdMinValue as SMV on SOP.OrderStyleRefId=SMV.OrderStyleRefId
where SOP.OutputDate=PLAN_CapacityUtilization.OutputDate and SOPD.Quantity>0
and SOP.LineId=PLAN_CapacityUtilization.LineId ),0) where OutputDate ='2017-12-05'


