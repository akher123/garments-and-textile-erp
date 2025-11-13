CREATE procedure [dbo].[spInReDyeingFabricStatus]
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
'' as Remarks, 
'' as ChallanDate,
'' as RefNo,
'' as GatEntryNo,
'' as ChallanNo,
sum(RPD.RQty) as RQty,
ISNULL((
select SUM( RFID.FinishQty) from Inventory_ReDyeingFabricIssueDetail as RFID 
where  RFID.BatchDetailId=Btd.BatchDetailId and  RFID.BatchId=RPD.BatchId),0) as FinishQty,

ISNULL((select SUM(RFID.ReprocessQty) from Inventory_ReDyeingFabricIssue  as RFI
inner join Inventory_ReDyeingFabricIssueDetail as RFID on RFI.ReDyeingFabricIssueId=RFID.ReDyeingFabricIssueId
where RFID.BatchDetailId=RPD.BatchDetailId and RFID.BatchId=RPD.BatchId and RFI.PartyId=RP.PartyId),0) as ProcessedQty
from Inventory_ReDyeingFabricReceive as RP
inner join Inventory_ReDyeingFabricReceiveDetail as RPD on RP.ReDyeingFabricReceiveId=RPD.ReDyeingFabricReceiveId
inner join Pro_Batch as Bt on RPD.BatchId=Bt.BatchId
inner join PROD_BatchDetail as Btd on RPD.BatchDetailId=Btd.BatchDetailId and RPD.BatchId=Btd.BatchId

where (RP.PartyId=@PartyId or @PartyId='-1') and  Convert(date,RP.ReceiveDate)>=Convert(date,@FromDate) and Convert(date,RP.ReceiveDate)<=Convert(date,@ToDate)

group by --'' as Remarks, 
--RP.ChallanDate,
--RP.RefNo,
--RP.GatEntryNo,
--RP.ChallanNo,
RP.PartyId,
Bt.BuyerRefId,
Bt.CompId, 
Bt.OrderNo,
Bt.OrderStyleRefId,
Bt.BatchNo,
Btd.ItemId,
Btd.ComponentRefId,
Btd.CompId,
Btd.GSM,
Bt.GrColorRefId,
Btd.MdiaSizeRefId,
Btd.FdiaSizeRefId,
RPD.BatchDetailId,
RPD.BatchId,
Btd.BatchDetailId



