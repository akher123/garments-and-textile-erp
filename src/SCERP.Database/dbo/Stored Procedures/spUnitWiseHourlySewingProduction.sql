CREATE procedure spUnitWiseHourlySewingProduction
@CompId varchar(3),
@FilterDate datetime
as

select P.ProcessorName as UnitName,H.HourName, SUM(SOD.Quantity) as Qty,SO.OutputDate from PROD_SewingOutPutProcess as SO
inner join PROD_Hour as H on SO.HourId=H.HourId
inner join Production_Machine as L on SO.LineId=L.MachineId
inner join PROD_Processor as P on L.ProcessorRefId=P.ProcessorRefId
inner join PROD_SewingOutPutProcessDetail  as SOD on SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
where Convert(date,SO.OutputDate)=Convert(date,@FilterDate) and SO.CompId=@CompId
group by  P.ProcessorName,H.HourRefId ,H.HourName ,SO.OutputDate

