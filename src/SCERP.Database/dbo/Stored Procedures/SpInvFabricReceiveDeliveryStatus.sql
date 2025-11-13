CREATE Procedure [dbo].[SpInvFabricReceiveDeliveryStatus]
@PartyId bigint,
@FromDate datetime,
@ToDate datetime
as
--select 
--( select Name from Party where PartyId=Bt.PartyId) as  PartyName,
--(select top(1) BuyerName from OM_Buyer where BuyerRefId=Bt.BuyerRefId and CompId=Bt.CompId) as BuyerName,
--( select top (1)RefNo from VOM_BuyOrdStyle where OrderNo=Bt.OrderNo and CompId=Bt.CompId) as OrderName,
--( select top (1)StyleName from VOM_BuyOrdStyle where OrderStyleRefId=Bt.OrderStyleRefId and CompId=Bt.CompId) as StyleName,
--Bt.BatchNo as BatchNo,
--(select ItemName from Inventory_Item where ItemId=Btd.ItemId) as FabricType,
--( select top(1) ComponentName from OM_Component where ComponentRefId=Btd.ComponentRefId and CompId=Btd.CompId) as Component,
--Btd.GSM as GSM,
--( select ColorName from OM_Color where ColorRefId=Bt.GrColorRefId and CompId=Bt.CompId) as ColorName,
--(select UnitName from VInvItem where ItemId=Btd.ItemId) as Unit,
--( select top(1) SizeName from OM_Size where SizeRefId=Btd.MdiaSizeRefId and CompId=Btd.CompId) as McDia,
--( select top(1) SizeName from OM_Size where SizeRefId=Btd.FdiaSizeRefId and CompId=Btd.CompId) as FDia,
--Btd.Quantity as BatchQty,
--ISNULL(( select SUM(RcvQty) from Inventory_FinishFabDetailStore
-- where BatchDetailId=Btd.BatchDetailId and BatchId=Btd.BatchId),0) as TtlFinishRcvQty,
--(Btd.Quantity-ISNULL(( select SUM(RcvQty) from Inventory_FinishFabDetailStore
-- where BatchDetailId=Btd.BatchDetailId and BatchId=Btd.BatchId),0)) as RcvBalanceQty,
--ISNULL(( select SUM(FabQty) from Inventory_FinishFabricIssueDetail
--where BatchDetailId=Btd.BatchDetailId and BatchId=Btd.BatchId ),0) as DeliveryQty,
--ISNULL(( select SUM(GreyWt) from Inventory_FinishFabricIssueDetail
--where BatchDetailId=Btd.BatchDetailId and BatchId=Btd.BatchId ),0) as BillQty,
--(select top(1) Remarks from Inventory_FinishFabDetailStore where BatchId=Btd.BatchId and BatchDetailId=Btd.BatchDetailId) as Location,
--Btd.Remarks as Remarks
--from Pro_Batch as Bt
--inner join PROD_BatchDetail as Btd on Bt.BatchId=Btd.BatchId
--where (Bt.PartyId=@PartyId or @PartyId='-1') and Bt.BatchId in (select BatchId from Inventory_FinishFabStore
--inner join Inventory_FinishFabDetailStore on Inventory_FinishFabStore.FinishFabStoreId=Inventory_FinishFabDetailStore.FinishFabStoreId
--where  Convert(date,Inventory_FinishFabStore.InvoiceDate)>=Convert(date,@FromDate) and Convert(date,Inventory_FinishFabStore.InvoiceDate)<=Convert(date,@ToDate))

select 
( select Name from Party where PartyId=Bt.PartyId) as  PartyName,
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
Btd.Quantity as BatchQty,
ISNULL(( select SUM(RcvQty) from Inventory_FinishFabDetailStore
 where BatchDetailId=Btd.BatchDetailId and BatchId=Bt.BatchId),0) as TtlFinishRcvQty,
(SUM(Btd.Quantity)-ISNULL(( select SUM(RcvQty) from Inventory_FinishFabDetailStore
 where BatchDetailId=Btd.BatchDetailId and BatchId=Bt.BatchId),0)) as RcvBalanceQty,
ISNULL(SUM(Inventory_FinishFabricIssueDetail.FabQty),0) as DeliveryQty,
ISNULL(SUM(Inventory_FinishFabricIssueDetail.GreyWt),0) as BillQty,
(select SUM(FinishWeight) from PROD_DyeingSpChallanDetail where  BatchDetailId=Btd.BatchDetailId) as ChallanQty,
(select top(1) Remarks from Inventory_FinishFabDetailStore where BatchId=Bt.BatchId and BatchDetailId=Btd.BatchDetailId) as Location,
Btd.Remarks as Remarks
from Pro_Batch as Bt
inner join  PROD_BatchDetail as Btd on Bt.BatchId=Btd.BatchId
inner join Inventory_FinishFabricIssueDetail on Btd.BatchDetailId=Inventory_FinishFabricIssueDetail.BatchDetailId
inner join Inventory_FinishFabricIssue on Inventory_FinishFabricIssueDetail.FinishFabricIssueId=Inventory_FinishFabricIssue.FinishFabIssueId
where (Bt.PartyId=@PartyId or @PartyId='-1') and  Convert(date,Inventory_FinishFabricIssue.ChallanDate)>=Convert(date,@FromDate) and Convert(date,Inventory_FinishFabricIssue.ChallanDate)<=Convert(date,@ToDate)

group by Bt.PartyId,Bt.BuyerRefId,Bt.CompId,Bt.OrderNo,Bt.OrderStyleRefId,Bt.BtRefNo,Bt.BatchNo,Btd.ItemId,Btd.ComponentRefId,Btd.CompId,Btd.GSM,Bt.GrColorRefId,Btd.FdiaSizeRefId,Btd.MdiaSizeRefId,Btd.Quantity,Btd.BatchDetailId,Bt.BatchId,Btd.Remarks






--where (Bt.PartyId=@PartyId or @PartyId='-1') and Bt.BatchId in (select BatchId from Inventory_FinishFabricIssue
--inner join Inventory_FinishFabricIssueDetail on Inventory_FinishFabricIssue.FinishFabIssueId=Inventory_FinishFabricIssueDetail.FinishFabricIssueId
--where  Convert(date,Inventory_FinishFabricIssue.ChallanDate)>=Convert(date,@FromDate) and Convert(date,Inventory_FinishFabricIssue.ChallanDate)<=Convert(date,@ToDate))

















 
















