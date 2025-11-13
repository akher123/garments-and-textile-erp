



CREATE view [dbo].[VwAdvanceMaterialIssueDetail]
as
select MID.AdvanceMaterialIssueDetailId,MID.AdvanceMaterialIssueId,MID.ColorRefId,MID.CompId,MID.IssueQty,MID.IssueRate,MID.ItemId,
MID.FColorRefId,FC.ColorName as FColorName, B.Name as Brand,MID.QtyInBag,MID.Wrapper,MID.SizeRefId,MID.GColorRefId,GC.ColorName as  GColorName,
ISNULL((select OM_Buyer.BuyerName from OM_Buyer where OM_Buyer.BuyerRefId=MI.BuyerRefId and OM_Buyer.CompId=MI.CompId),'---') as BuyerName,
ISNULL((select top(1)Attention from PLAN_Program where ProgramRefId=MI.ProgramRefId),'---')  as Attention,
MI.StoreId,MI.IRefId,MI.IRNoteDate,MI.IRNoteNo,MI.IssuedBy,MI.OrderNo,MI.StyleNo,MI.SlipNo,MI.Remarks,MI.IType,(select top(1) Name from Employee where EmployeeId=MI.IssuedBy) as Employee,(select top(1) Name from Employee where EmployeeId=MI.RefPerson) as RefEmployee ,(select top(1) Designation from VEmployee where EmployeeId=MI.RefPerson)as Designation,
ISNULL((select top(1) Name from Employee where EmployeeId=MI.ApprovedBy),'Not Approved') as ApprovedByName,
I.ItemName,MU.UnitName, I.ItemCode,clr.ColorName,sz.SizeName,MID.GSizeRefId,MID.PurchaseOrderDetailId,
(Select SizeName from OM_Size where SizeRefId=MID.GSizeRefId and CompId=MID.CompId) as GSizeName,
 C.Name as CompanyName,C.FullAddress,P.[Address] as PartyAddress ,P.Name as ParyName,MI.VehicleNo as VehecleNo,MI.DriverName as DriverName,0 as CurrencyId,MI.ProgramRefId as CurrencyName from Inventory_AdvanceMaterialIssueDetail as MID
inner join Inventory_AdvanceMaterialIssue as MI on MID.AdvanceMaterialIssueId=MI.AdvanceMaterialIssueId and MID.CompId=MI.CompId
left join Party as P on MI.PartyId=P.PartyId and MI.CompId=P.CompId
inner join Inventory_Item as I on MID.ItemId=I.ItemId  
left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId 
left join OM_Color as clr on clr.ColorRefId=MID.ColorRefId and clr.CompId= MID.CompId
left join OM_Size as sz on MID.SizeRefId=sz.SizeRefId and MID.CompId=sz.CompId
left join OM_Color as  FC on FC.ColorRefId=MID.FColorRefId and FC.CompId=MID.CompId
left join OM_Color as  GC on GC.ColorRefId=MID.GColorRefId and GC.CompId=MID.CompId
--inner join Employee as E on MI.IssuedBy=E.EmployeeId
--left join VEmployee as RE on MI.RefPerson=RE.EmployeeId
left join Inventory_Brand as B on B.BrandId=clr.ColorCode
inner join Company as C on MI.CompId=C.CompanyRefId





