

CREATE procedure  spInAccessoriesEditRcvBalance
 @AdvanceMaterialIssueId bigint
 as
SELECT 
MR.AdvanceMaterialIssueId,
MRD.AdvanceMaterialIssueDetailId,
MRD.PurchaseOrderDetailId, 
MRD.ItemCode, 
MRD.ItemId,
MRD.ItemName,
MRD.ColorRefId, 
MRD.SizeRefId, 
MRD.ColorName, 
MRD.GSizeRefId, 
MRD.ColorName,
MRD.SizeName,
MRD.FColorName ,
MRD.GSizeName, 
MRD.FColorRefId, 
        
MRD.IssueRate as Rate, 
MRD.IssueQty,
(select  SUM(ReceivedQty - ISNULL(RejectedQty, 0)) from Inventory_MaterialReceiveAgainstPoDetail where PurchaseOrderDetailId=MRD.PurchaseOrderDetailId) AS TotalRcvQty,
 ISNULL((select sum(IssueQty) from Inventory_AdvanceMaterialIssueDetail where PurchaseOrderDetailId=MRD.PurchaseOrderDetailId),0) AS ToalIssueQty
FROM  VwAdvanceMaterialIssueDetail AS MRD INNER JOIN
Inventory_AdvanceMaterialIssue AS MR ON MRD.AdvanceMaterialIssueId = MR.AdvanceMaterialIssueId
WHERE   MR.AdvanceMaterialIssueId=@AdvanceMaterialIssueId   
GROUP BY MR.AdvanceMaterialIssueId,
MRD.AdvanceMaterialIssueDetailId, MRD.ItemCode, MRD.ItemName,MRD.ItemId, MRD.ColorRefId, MRD.SizeRefId,MRD.FColorRefId,  MRD.FColorName, MRD.GSizeRefId, MRD.ColorName, MRD.SizeName, MRD.FColorName, MRD.GSizeName, MRD.PurchaseOrderDetailId, MRD.IssueQty,
                         MRD.IssueRate



