create procedure spInvAccessoriesIssueChallan
@AdvanceMaterialIssueId bigint
as
SELECT 
P.Name as Party,
P.[Address],
P.Email,
P.ContactPersonName,
P.ContactPhone,
MR.IRefId,
MR.IRNoteDate,
BO.BuyerName,BO.RefNo as OrderNo,BO.StyleName,
MRD.ItemCode, 
MRD.ItemName,
MRD.ColorName, 
MRD.SizeName,
MRD.FColorName ,
MRD.GSizeName, 
MRD.IssueQty,
MR.Remarks,
'' as ApprovedBy,
'' as IRNoteNo,
(select Name from Employee where EmployeeId=MR.IssuedBy) as Employee
FROM  VwAdvanceMaterialIssueDetail AS MRD INNER JOIN
Inventory_AdvanceMaterialIssue AS MR ON MRD.AdvanceMaterialIssueId = MR.AdvanceMaterialIssueId
inner join VOM_BuyOrdStyle as BO on MR.OrderStyleRefId=BO.OrderStyleRefId and MR.CompId=BO.CompId
inner join Party as P on MR.PartyId=P.PartyId
WHERE   MR.AdvanceMaterialIssueId=@AdvanceMaterialIssueId  
GROUP BY P.Name ,
P.[Address],
P.Email,
P.ContactPersonName,
P.ContactPhone, MR.IRefId,MR.IRNoteDate, BO.BuyerName,BO.RefNo,BO.StyleName, MR.AdvanceMaterialIssueId,
MRD.AdvanceMaterialIssueDetailId, MRD.ItemCode, MRD.ItemName,  MRD.FColorName,  MRD.ColorName, MRD.SizeName, MRD.FColorName, MRD.GSizeName, MRD.IssueQty,
                         MRD.IssueRate,MR.Remarks,MR.IssuedBy


					

		