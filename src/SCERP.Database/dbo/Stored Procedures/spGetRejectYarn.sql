CREATE procedure [dbo].[spGetRejectYarn]
@RejectYarnIssueId int

as

IF(@RejectYarnIssueId=0)
select RD.MaterialReceiveAgstPoDetailId as MaterialReceiveDetailId, R.RefNo,R.MRRNO,R.MRRDate,R.InvoiceNo,R.ReceiveRegNo,R.GateEntryNo,R.PoNo,
(select ItemName from Inventory_Item where ItemId=RD.ItemId  and CompId=RD.CompId) as ItemName,
(select ColorName from OM_Color where ColorRefId=RD.FColorRefId and CompId=RD.CompId) as ColorName,
B.Name as Brand,
(select SizeName from OM_Size where SizeRefId=RD.SizeRefId  and CompId=RD.CompId) as YarnCount,
C.ColorName as YarnLot,
(select CompanyName from Mrc_SupplierCompany where SupplierCompanyId=R.SupplierId ) as SupplierName,
RD.ReceivedQty,
ISNULL((ISNULL(RD.RejectedQty,0)-ISNULL((select ISNULL(SUM(Inventory_RejectYarnIssueDetail.Qty),0) from Inventory_RejectYarnIssueDetail where MaterialReceiveDetailId=RD.MaterialReceiveAgstPoDetailId),0)),0) as RejectedQty ,
RD.ReceivedRate,
ISNULL(Cast(RY.Qty as float),0) as Qty
from Inventory_MaterialReceiveAgainstPo as R
inner join Inventory_MaterialReceiveAgainstPoDetail as RD on R.MaterialReceiveAgstPoId=RD.MaterialReceiveAgstPoId
left join OM_Color as C on RD.ColorRefId=C.ColorRefId and RD.CompId=C.CompId
left join Inventory_Brand as B on C.ColorCode=B.BrandId
left  join Inventory_RejectYarnIssueDetail as RY on RD.MaterialReceiveAgstPoDetailId=RY.MaterialReceiveDetailId
where R.StoreId=1 and   ISNULL((ISNULL(RD.RejectedQty,0)-ISNULL((select ISNULL(SUM(Inventory_RejectYarnIssueDetail.Qty),0) from Inventory_RejectYarnIssueDetail where MaterialReceiveDetailId=RD.MaterialReceiveAgstPoDetailId),0)),0)>0   and (RY.RejectYarnIssueId=@RejectYarnIssueId or @RejectYarnIssueId=0)
ELSE
select RD.MaterialReceiveAgstPoDetailId as MaterialReceiveDetailId, R.RefNo,R.MRRNO,R.MRRDate,R.InvoiceNo,R.ReceiveRegNo,R.GateEntryNo,R.PoNo,
(select ItemName from Inventory_Item where ItemId=RD.ItemId  and CompId=RD.CompId) as ItemName,
(select ColorName from OM_Color where ColorRefId=RD.FColorRefId and CompId=RD.CompId) as ColorName,
B.Name as Brand,
(select SizeName from OM_Size where SizeRefId=RD.SizeRefId  and CompId=RD.CompId) as YarnCount,
C.ColorName as YarnLot,
(select CompanyName from Mrc_SupplierCompany where SupplierCompanyId=R.SupplierId ) as SupplierName,
RD.ReceivedQty,
ISNULL((ISNULL(RD.RejectedQty,0)-ISNULL((select ISNULL(SUM(Inventory_RejectYarnIssueDetail.Qty),0) from Inventory_RejectYarnIssueDetail where MaterialReceiveDetailId=RD.MaterialReceiveAgstPoDetailId),0)),0) as RejectedQty ,
RD.ReceivedRate,
ISNULL(Cast(RY.Qty as float),0) as Qty
from Inventory_MaterialReceiveAgainstPo as R
inner join Inventory_MaterialReceiveAgainstPoDetail as RD on R.MaterialReceiveAgstPoId=RD.MaterialReceiveAgstPoId
left join OM_Color as C on RD.ColorRefId=C.ColorRefId and RD.CompId=C.CompId
left join Inventory_Brand as B on C.ColorCode=B.BrandId
inner  join Inventory_RejectYarnIssueDetail as RY on RD.MaterialReceiveAgstPoDetailId=RY.MaterialReceiveDetailId
where R.StoreId=1  and (RY.RejectYarnIssueId=@RejectYarnIssueId or @RejectYarnIssueId=0)
--exec spGetRejectYarn 7


--select * from Inventory_RejectYarnIssue








