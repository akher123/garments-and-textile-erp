CREATE procedure [dbo].[InvSpReportBatchCost]
@GroupId int,
@BtRefNo varchar(8)
as
 select sum(ISNULL(SL.Quantity,0)) as Qty ,Sum(ISNULL(SL.Amount,0))as Amt,I.ItemCode,I.ItemName,SG.SubGroupName,g.GroupName,B.PartyName,B.MachineName,MIR.IssueReceiveNoteNo as IRNNO,MIR.IssueReceiveNoteDate as IRNDate, B.BatchQty from Inventory_MaterialIssue as MI
 inner join VProBatch as B on MI.BtRefNo=B.BtRefNo 
 inner join Inventory_StoreLedger as SL on MI.MaterialIssueId=SL.MaterialIssueId
 inner join Inventory_MaterialIssueRequisition as MIR on MI.MaterialIssueRequisitionId=MIR.MaterialIssueRequisitionId
 inner join Inventory_Item as I on SL.ItemId=I.ItemId
 inner join Inventory_SubGroup as SG on I.SubGroupId=SG.SubGroupId
 inner join Inventory_Group as g on SG.GroupId=g.GroupId
 where MI.IType=2 and SL.TransactionType=2  and B.BatchNo=@BtRefNo  and g.GroupId=@GroupId
 group by I.ItemCode,B.BatchNo,B.BtRefNo,I.ItemName,SG.SubGroupName,g.GroupName,B.PartyName,B.MachineName ,MIR.IssueReceiveNoteNo,MIR.IssueReceiveNoteDate,B.BatchQty
 order  by B.BtRefNo