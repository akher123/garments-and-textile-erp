CREATE procedure [dbo].[InvSpReporBatchCostParyWise]
@FromDate datetime,
@ToDate datetime,
@PartyId int
as
select sum(ISNULL(SL.Quantity,0)) as Qty ,Sum(ISNULL(SL.Amount,0))as Amt,I.ItemCode,I.ItemName,SG.SubGroupName,g.GroupName,B.PartyName,B.MachineName,B.BatchDate,P.Name as Party, B.BtRefNo,B.BatchNo, B.BatchQty from Inventory_MaterialIssue as MI
 inner join VProBatch as B on MI.BtRefNo=B.BtRefNo 
 inner join Inventory_StoreLedger as SL on MI.MaterialIssueId=SL.MaterialIssueId
 inner join Inventory_MaterialIssueRequisition as MIR on MI.MaterialIssueRequisitionId=MIR.MaterialIssueRequisitionId
 inner join Inventory_Item as I on SL.ItemId=I.ItemId
 inner join Inventory_SubGroup as SG on I.SubGroupId=SG.SubGroupId
 inner join Inventory_Group as g on SG.GroupId=g.GroupId
 inner join Party as P on B.PartyId=P.PartyId
 where MI.IType=2 and SL.TransactionType=2 and (MI.IssueReceiveDate BETWEEN  @FromDate and  @ToDate )and B.PartyId=@PartyId  and SL.IsActive=1
 group by I.ItemCode,B.BatchNo,B.BtRefNo,I.ItemName,SG.SubGroupName,g.GroupName,B.PartyName,B.MachineName ,B.BatchQty,P.Name,B.BatchDate
 order  by B.BtRefNo