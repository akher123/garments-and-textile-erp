CREATE procedure [dbo].[spInvAccessoriesRcvSummary] 
@OrderStyleRefId varchar(7),
@CompId varchar(3)
as 


SELECT        MRD.ItemCode, 
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
MRD.PurchaseOrderDetailId,
             
MRD.ReceivedRate as Rate, 
 SUM(MRD.ReceivedQty - ISNULL(MRD.RejectedQty, 0)) AS TotalRcvQty, ISNULL((select sum(IssueQty) from Inventory_AdvanceMaterialIssueDetail where PurchaseOrderDetailId=MRD.PurchaseOrderDetailId),0) AS ToalIssueQty
FROM            VwMaterialReceiveAgainstPoDetail AS MRD INNER JOIN
                         Inventory_MaterialReceiveAgainstPo AS MR ON MRD.MaterialReceiveAgstPoId = MR.MaterialReceiveAgstPoId
WHERE        (MRD.RType = 'P') and MR.OrderStyleRefId=@OrderStyleRefId and MRD.CompId=@CompId
GROUP BY MRD.ItemCode, MRD.ItemName,MRD.ItemId, MRD.ColorRefId, MRD.SizeRefId,MRD.FColorRefId,  MRD.FColorName, MRD.GSizeRefId, MRD.ColorName, MRD.SizeName, MRD.FColorName, MRD.GSizeName, MRD.PurchaseOrderDetailId, 
                         MRD.ReceivedRate


		--exec spInvAccessoriesRcvSummary 'ST00537','001'	
		
		