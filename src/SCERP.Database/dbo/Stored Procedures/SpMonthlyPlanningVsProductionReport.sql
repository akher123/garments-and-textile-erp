CREATE procedure SpMonthlyPlanningVsProductionReport
@yearId int,
@monthId int
as

select Production_Machine.Name as Line,
ISNULL((select SUM(PROD_SewingOutPutProcessDetail.Quantity) from PROD_SewingOutPutProcess
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
where PROD_SewingOutPutProcess.LineId=Production_Machine.MachineId and month(PROD_SewingOutPutProcess.OutputDate)=@monthId and year(PROD_SewingOutPutProcess.OutputDate)=@yearId),0) as AchiveQty,
ISNULL((
select SUM(PLAN_TargetProduction.TotalTargetQty) from PLAN_TargetProduction where PLAN_TargetProduction.LineId=Production_Machine.MachineId and month(PLAN_TargetProduction.StartDate)=@monthId and year(PLAN_TargetProduction.StartDate)=@yearId),0) as TargetQty 

from Production_Machine 
inner join PROD_Processor  on Production_Machine.ProcessorRefId=PROD_Processor.ProcessorRefId
where PROD_Processor.ProcessRefId='007' and Production_Machine.CompId='001' and Production_Machine.IsActive=1


select * from [User] where EmployeeId='3728EF42-768F-4247-8CC3-378BCF324705'



