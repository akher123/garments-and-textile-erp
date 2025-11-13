create procedure spRejectYarnIssureReport
@RejectYarnIssueId bigint
as

select 
I.RefId,
I.IssueDate,
I.ChallanNo,
(select Name from Party where PartyId=I.PartyId) as PartyName,
(select ItemName from Inventory_Item where ItemId=MI.ItemId  and CompId=MI.CompId) as ItemName,
(select ColorName from OM_Color where ColorRefId=MI.FColorRefId and CompId=MI.CompId) as ColorName,
B.Name as Brand,
(select SizeName from OM_Size where SizeRefId=MI.SizeRefId  and CompId=MI.CompId) as YarnCount,
C.ColorName as YarnLot,
ID.Qty
from Inventory_RejectYarnIssue as I
inner join   Inventory_RejectYarnIssueDetail as ID on I.RejectYarnIssueId=ID.RejectYarnIssueId
inner  join Inventory_MaterialReceiveAgainstPoDetail as MI on ID.MaterialReceiveDetailId=MI.MaterialReceiveAgstPoDetailId
left join OM_Color as C on MI.ColorRefId=C.ColorRefId and MI.CompId=C.CompId
left join Inventory_Brand as B on C.ColorCode=B.BrandId
where I.RejectYarnIssueId=@RejectYarnIssueId