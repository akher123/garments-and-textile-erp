CREATE procedure [dbo].[GeYarnDeliveryChallan]
@CompId varchar(3),
@AdvanceMaterialIssueId bigint
as
select MID.AdvanceMaterialIssueDetailId,MID.AdvanceMaterialIssueId,MID.ColorRefId,MID.CompId,MID.IssueQty,MID.IssueRate,MID.ItemId,
MID.FColorRefId,GC.ColorName as FColorName, B.Name as Brand,MID.QtyInBag,MID.Wrapper,MID.SizeRefId,MID.GColorRefId,
FC.ColorName as  GColorName,
ISNULL((select top(1)Attention from PLAN_Program where ProgramRefId=MI.ProgramRefId),'---')  as Attention,
ISNULL((select OM_Buyer.BuyerName from OM_Buyer where OM_Buyer.BuyerRefId=MI.BuyerRefId and OM_Buyer.CompId=MI.CompId),'---') as BuyerName,
MI.StoreId,MI.IRefId,MI.IRNoteDate,MI.IRNoteNo,MI.IssuedBy,MI.OrderNo,MI.StyleNo,MI.SlipNo,MI.Remarks,MI.IType,(select top(1) Name from Employee where EmployeeId=MI.IssuedBy) as Employee,(select top(1) Name from Employee where EmployeeId=MI.RefPerson) as RefEmployee ,(select top(1) Designation from VEmployee where EmployeeId=MI.RefPerson)as Designation,
ISNULL((select top(1) Name from Employee where EmployeeId=MI.ApprovedBy),'Not Approved') as ApprovedByName,
I.ItemName,MU.UnitName, I.ItemCode,clr.ColorName,sz.SizeName,MID.GSizeRefId,MID.PurchaseOrderDetailId,
(Select SizeName from OM_Size where SizeRefId=MID.GSizeRefId and CompId=MID.CompId) as GSizeName,
 C.Name as CompanyName,C.FullAddress,P.[Address] as PartyAddress ,P.Name as ParyName,MI.VehicleNo as VehecleNo,MI.DriverName as DriverName,0 as CurrencyId,MI.ProgramRefId as CurrencyName from Inventory_AdvanceMaterialIssueDetail as MID
inner join Inventory_AdvanceMaterialIssue as MI on MID.AdvanceMaterialIssueId=MI.AdvanceMaterialIssueId and MID.CompId=MI.CompId
left join Party as P on MI.PartyId=P.PartyId and MI.CompId=P.CompId
inner join Inventory_Item as I on MID.ItemId=I.ItemId  
left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId 
left join OM_Color as clr on MID.ColorRefId=clr.ColorRefId and clr.CompId=MI.CompId
left join OM_Size as sz on MID.SizeRefId=sz.SizeRefId and MI.CompId=sz.CompId
left join OM_Color as  FC on MID.FColorRefId=FC.ColorRefId and FC.CompId=MID.CompId
left join OM_Color as  GC on MID.GColorRefId=GC.ColorRefId and GC.CompId=MID.CompId
--inner join Employee as E on MI.IssuedBy=E.EmployeeId
--left join VEmployee as RE on MI.RefPerson=RE.EmployeeId
left join Inventory_Brand as B on clr.ColorCode=B.BrandId
inner join Company as C on MI.CompId=C.CompanyRefId

where MID.CompId=@CompId and MID.AdvanceMaterialIssueId=@AdvanceMaterialIssueId



--exec [GeYarnDeliveryChallan] '001','598144'




