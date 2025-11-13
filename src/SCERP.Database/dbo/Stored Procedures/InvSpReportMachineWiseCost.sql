CREATE procedure [dbo].[InvSpReportMachineWiseCost]
		@MachineId int,
        @FromDate datetime ,
        @ToDate datetime
as
select I.ItemCode,I.ItemName, SUM(SL.Quantity) as Qty,SUM(SL.UnitPrice) as Rate,(SUM(SL.Quantity) *SUM(SL.UnitPrice)) as Amt,U.UnitName  ,M.Name as MachineName from Inventory_StoreLedger as SL
inner join Inventory_Item as I on SL.ItemId=I.ItemId
inner join MeasurementUnit as U on I.MeasurementUinitId=U.UnitId
inner join Inventory_MaterialIssue as MI on SL.MaterialIssueId=MI.MaterialIssueId  
inner join Production_Machine as M on MI.MachineId=M.MachineId
where SL.TransactionType='2' and MI.IType=1 and SL.IsActive=1 and MI.IsActive=1 and  ((SL.TransactionDate >= @FromDate)  and (SL.TransactionDate <= @ToDate)) and (M.MachineId=@MachineId or @MachineId='-1')
group by M.Name ,I.ItemName, I.ItemCode ,U.UnitName
order by I.ItemCode 


--exec [InvSpReportMachineWiseCost] -1,'2018-08-01','2018-08-31'