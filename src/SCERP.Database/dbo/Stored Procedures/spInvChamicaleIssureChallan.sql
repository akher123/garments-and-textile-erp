CREATE procedure spInvChamicaleIssureChallan
@MaterialIssueId int 
as
select B.BatchNo,B.BatchQty,B.BtRefNo,B.BatchDate,MID.IssuedItemRate,MID.IssuedQuantity,MI.IssueReceiveDate,MI.ToppingType,MI.IssueReceiveNo,(select Name from Employee where EmployeeId=MI.PreparedByStore) as Creator,B.PartyName,
(select ItemName from Inventory_Item where ItemId=MID.ItemId) as ItemName,
(select ItemCode from Inventory_Item where ItemId=MID.ItemId) as ItemCode,
(select UnitName from VInvItem where ItemId=MID.ItemId) as UnitName from Inventory_MaterialIssue as MI 
inner join Inventory_MaterialIssueDetail as MID on MI.MaterialIssueId=MID.MaterialIssueId
inner join VProBatch as B on MI.BtRefNo=B.BtRefNo
where MI.MaterialIssueId=@MaterialIssueId


