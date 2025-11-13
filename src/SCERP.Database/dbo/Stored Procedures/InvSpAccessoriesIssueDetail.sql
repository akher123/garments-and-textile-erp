create procedure InvSpAccessoriesIssueDetail
@FromDate datetime,
@ToDate datetime,
@CompId varchar(3)
as

select MI.IRefId,MI.IRNoteNo,MI.IRNoteDate,
(select BuyerName from OM_Buyer where BuyerRefId=MI.BuyerRefId and CompId=MI.CompId ) as BuyerName,
(select RefNo from VOM_BuyOrdStyle where OrderStyleRefId=MI.OrderStyleRefId and CompId=MI.CompId) as OrderNo,
(select StyleName from VOM_BuyOrdStyle where OrderStyleRefId=MI.OrderStyleRefId and CompId=MI.CompId) as StyleName,
(select ColorName from OM_Color where ColorRefId=MID.FColorRefId and CompId=MID.CompId ) as GColorName,
(select SizeName from OM_Size where SizeRefId=MID.GSizeRefId and CompId=MID.CompId ) as GSizeName,
(select ColorName from OM_Color where ColorRefId=MID.ColorRefId and CompId=MID.CompId ) as PColorName,
(select SizeName from OM_Size where SizeRefId=MID.SizeRefId and CompId=MID.CompId ) as PSizeName,
(select ItemName from VInvItem where ItemId=MID.ItemId ) as ItemName,
(select UnitName from VInvItem where ItemId=MID.ItemId ) as UnitName,
MID.IssueQty,
MID.IssueRate
 from Inventory_AdvanceMaterialIssue as MI
inner join Inventory_AdvanceMaterialIssueDetail as MID on MI.AdvanceMaterialIssueId=MID.AdvanceMaterialIssueId
where MI.IType=5 and MI.StoreId=2 and MI.IRNoteDate between @FromDate and @ToDate 
order by MI.IRefId 
 