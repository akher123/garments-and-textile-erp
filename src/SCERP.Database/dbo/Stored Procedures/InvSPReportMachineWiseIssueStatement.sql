CREATE procedure [dbo].[InvSPReportMachineWiseIssueStatement]
 @FromDate datetime ,
 @ToDate datetime,
 @MachineId int
as 
select MIR.IssueReceiveNoteNo as IRNNO,
 MI.IssueReceiveNo as IRefNo,
 MI.IssueReceiveDate as IDate,
 MIR.IssueReceiveNoteDate,
 M.Name as Matchin,I.ItemCode,
 I.ItemName,
 SUM(SL.Quantity) as IQty ,
-- SUM(SL.UnitPrice*SL.Quantity) as Value,
SUM(SL.Amount) as Value,
   --COALESCE(SUM(SL.UnitPrice*SL.Quantity)/ SUM(nullif(SL.Quantity,0)),0) as IRate,
    COALESCE(SUM(SL.Amount)/ SUM(nullif(SL.Quantity,0)),0) as IRate,
MU.UnitName 
from Inventory_MaterialIssue as MI 
inner join Inventory_MaterialIssueRequisition as MIR on MI.MaterialIssueRequisitionId=MIR.MaterialIssueRequisitionId
inner join Inventory_StoreLedger as SL on MI.MaterialIssueId=SL.MaterialIssueId
inner join Inventory_Item as I on SL.ItemId=I.ItemId 
inner join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
inner join Production_Machine as M on MI.MachineId=M.MachineId 
where MI.IType=1 and   MI.IsActive=1 and SL.TransactionType=2 and ((Convert(date,SL.TransactionDate) >=Convert(date, @FromDate))  and (Convert(date,SL.TransactionDate) <= convert(date,@ToDate))) and (MI.MachineId=@MachineId or @MachineId='-1') 
--and  LEFT(I.ItemCode, 5) in( '01002','01001')
group by  I.ItemName,MIR.IssueReceiveNoteNo, MI.IssueReceiveNo ,MI.IssueReceiveDate, MIR.IssueReceiveNoteDate ,M.Name ,I.ItemCode,MU.UnitName 
order by MI.IssueReceiveDate, MIR.IssueReceiveNoteDate




