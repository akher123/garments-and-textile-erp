create procedure [dbo].[spInReDyeingFabricIssue]
@PartyId bigint,
@FromDate datetime,
@ToDate datetime
as
select
( select Name from Party where PartyId=RP.PartyId) as  PartyName,
(select top(1) BuyerName from OM_Buyer where BuyerRefId=Bt.BuyerRefId and CompId=Bt.CompId) as BuyerName,
( select top (1)RefNo from VOM_BuyOrdStyle where OrderNo=Bt.OrderNo and CompId=Bt.CompId) as OrderName,
( select top (1)StyleName from VOM_BuyOrdStyle where OrderStyleRefId=Bt.OrderStyleRefId and CompId=Bt.CompId) as StyleName,
Bt.BatchNo as BatchNo,
(select ItemName from Inventory_Item where ItemId=Btd.ItemId) as FabricType,
( select top(1) ComponentName from OM_Component where ComponentRefId=Btd.ComponentRefId and CompId=Btd.CompId) as Component,
Btd.GSM as GSM,
( select ColorName from OM_Color where ColorRefId=Bt.GrColorRefId and CompId=Bt.CompId) as ColorName,
(select UnitName from VInvItem where ItemId=Btd.ItemId) as Unit,
( select top(1) SizeName from OM_Size where SizeRefId=Btd.MdiaSizeRefId and CompId=Btd.CompId) as McDia,
( select top(1) SizeName from OM_Size where SizeRefId=Btd.FdiaSizeRefId and CompId=Btd.CompId) as FDia,
RP.Remarks, 
RP.ChallanDate,
RP.RefNo,
RP.ChallanNo,
RPD.FinishQty
from Inventory_ReDyeingFabricIssue as RP
inner join Inventory_ReDyeingFabricIssueDetail as RPD on RP.RedyeingFabricIssueId=RPD.RedyeingFabricIssueId
inner join Pro_Batch as Bt on RPD.BatchId=Bt.BatchId
inner join PROD_BatchDetail as Btd on RPD.BatchDetailId=Btd.BatchDetailId and RPD.BatchId=Btd.BatchId

where (RP.PartyId=@PartyId or @PartyId='-1') and  Convert(date,RP.ChallanDate)>=Convert(date,@FromDate) and Convert(date,RP.ChallanDate)<=Convert(date,@ToDate)

