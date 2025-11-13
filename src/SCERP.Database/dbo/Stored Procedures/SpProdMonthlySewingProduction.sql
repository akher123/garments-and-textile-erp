CREATE procedure [dbo].[SpProdMonthlySewingProduction]
@YearId int ,
@MonthId int ,
@CompId varchar(3)
as
--select SOP.OutputDate, SUM(SOPD.Quantity) as Quantity from PROD_SewingOutPutProcess as SOP 
--inner join PROD_SewingOutPutProcessDetail as SOPD on SOP.SewingOutPutProcessId=SOPD.SewingOutPutProcessId
--where year(SOP.OutputDate)=@YearId and month(SOP.OutputDate)=@MonthId and SOP.CompId=@CompId and SOP.LineId>0
--group by SOP.OutputDate
--order by SOP.OutputDate

select CAST(SOP.OutputDate AS DATE) as OutputDate, SUM(SOPD.Quantity) as OutQty,

(select SUM(SIPD.InputQuantity) from PROD_SewingInputProcess as SIP
inner join PROD_SewingInputProcessDetail as SIPD on SIP.SewingInputProcessId=SIPD.SewingInputProcessId
where year(SIP.InputDate)=year(SOP.OutputDate)and month(SIP.InputDate)=month(SOP.OutputDate) and DAY(SIP.InputDate)=DAY(SOP.OutputDate)) as InQty
 from PROD_SewingOutPutProcess as SOP 
inner join PROD_SewingOutPutProcessDetail as SOPD on SOP.SewingOutPutProcessId=SOPD.SewingOutPutProcessId
where  year(SOP.OutputDate)=@YearId and month(SOP.OutputDate)=@MonthId and SOP.CompId=@CompId and SOP.LineId>0
group by  CAST(SOP.OutputDate AS DATE)  ,year(SOP.OutputDate),month(SOP.OutputDate),DAY(SOP.OutputDate)
order by CAST(SOP.OutputDate AS DATE) 

