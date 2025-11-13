CREATE procedure [dbo].[InvSpPartyWiseYarnDeliveryReport]
@FromDate datetime,
@ToDate datetime ,
@ColorName varchar(max),
@xStatus int
as


select
MI.ProgramRefId,
(select top(1) OM_Buyer.BuyerName from PLAN_Program 
inner join OM_Buyer on PLAN_Program.BuyerRefId=PLAN_Program.BuyerRefId
where PLAN_Program.ProgramRefId=MI.ProgramRefId and PLAN_Program.CompId=C.CompanyRefId ) as BuyerName,
MI.IRefId,
MI.IRNoteDate,
sz.SizeName as CountName,
B.Name as Brand,
I.ItemName,
MU.UnitName,
clr.ColorName,
C.Name as CompanyName,
C.FullAddress,
P.[Address] as PartyAddress ,
P.Name as ParyName,
SUM(MID.IssueQty) as TQty,
SUM(MID.IssueRate*MID.IssueQty) as TAmt
from Inventory_AdvanceMaterialIssueDetail as MID
inner join Inventory_AdvanceMaterialIssue as MI on MID.AdvanceMaterialIssueId=MI.AdvanceMaterialIssueId and MID.CompId=MI.CompId
left join Party as P on MI.PartyId=P.PartyId and MI.CompId=P.CompId
inner join Inventory_Item as I on MID.ItemId=I.ItemId  
left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId 
left join OM_Color as clr on MID.ColorRefId=clr.ColorRefId and MID.CompId=clr.CompId
left join OM_Size as sz on MID.SizeRefId=sz.SizeRefId and MID.CompId=sz.CompId
left join Inventory_Brand as B on clr.ColorCode=B.BrandId
inner join Company as C on MI.CompId=C.CompanyRefId

where ((MI.IRNoteDate >=@FromDate OR @FromDate IS NULL) and ( MI.IRNoteDate <=@ToDate OR @ToDate IS NULL))and ( Clr.ColorName=@ColorName  or @xStatus=1) and ( sz.SizeName=@ColorName  or @xStatus=2) 
group by MI.ProgramRefId,
MI.IRefId, 
I.ItemName,
MU.UnitName,
clr.ColorName,
C.Name ,
C.FullAddress,
P.[Address] ,
P.Name ,
MI.IRNoteDate,
sz.SizeName ,
B.Name,
C.CompanyRefId

