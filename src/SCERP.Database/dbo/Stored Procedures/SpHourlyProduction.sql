CREATE PROCEDURE [dbo].[SpHourlyProduction] 
@ViewDate datetime

AS
BEGIN

SELECT ISNULL(M.Name,'Total') as LINE, HP.TotalQty as PRODUCTION,HP.H01 as [9am-10am] ,HP.H02 as  [10am-11am], HP.H03 as  [11am-12am],HP.H04 as  [12pm-1pm],HP.H05 as  [2pm-3pm],HP.H06 as  [3pm-4pm],HP.H07 as  [4pm-5pm],HP.H08 as  [5pm-6pm],HP.H09 as  [6pm-7pm],HP.H10 as  [7pm-8pm],
(select top(1) Remarks from PROD_MachingInterruption where  MachineId=M.MachineId and CompId=M.CompId and ProcessRefId='007' and convert(date,InterrupDate)=convert(date,@ViewDate)) as Remarks FROM PROD_HourlyProductionReport as HP
left join Production_Machine as M on HP.LineId=M.MachineId and M.IsActive=1 and HP.CompId=M.CompId
where HP.CompId='001'
order by M.MachineId,HP.DataType

END






