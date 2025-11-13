create procedure SpOmThreadConsumptionReport
@ThreadConsumptionId int 
as

select cons.BuyerName,
cons.OrderName,
cons.StyleName,
cons.ItemName as ItemNameG,
cons.EntryDate,cons.Remarks as RemarksT,
(select Name from Employee where EmployeeId=cons.ApprovedBy) as ApporoverName,
(select Name from Employee where EmployeeId=cons.CreatedBy) as CreatorName
,cons.SizeName,consD.* from VwThreadConsumption as cons
inner join OM_ThreadConsumptionDetail as consD on cons.ThreadConsumptionId=consD.ThreadConsumptionId

where cons.ThreadConsumptionId=@ThreadConsumptionId
