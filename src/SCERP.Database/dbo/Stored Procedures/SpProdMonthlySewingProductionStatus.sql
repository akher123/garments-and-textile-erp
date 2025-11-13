CREATE procedure SpProdMonthlySewingProductionStatus
@YearId int ,
@MonthId int ,
@CompId varchar(3)
as 
select Convert(varchar(10), PROD_SewingOutPutProcess.LineId)+'--'+Production_Machine.Name as Name,SUM(PROD_SewingOutPutProcessDetail.Quantity) as Quantity from PROD_SewingOutPutProcess
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
inner join Production_Machine  on PROD_SewingOutPutProcess.LineId=Production_Machine.MachineId 
where Year(PROD_SewingOutPutProcess.OutputDate)=@YearId and month(PROD_SewingOutPutProcess.OutputDate)=@MonthId and PROD_SewingOutPutProcess.CompId=@CompId and Production_Machine.IsActive=1
group by Production_Machine.Name, PROD_SewingOutPutProcess.LineId 
order by PROD_SewingOutPutProcess.LineId