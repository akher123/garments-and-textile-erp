CREATE procedure [dbo].[SpInvFinishFabStore]
@FinishFabStoreId bigint ,
@DyeingSpChallanId bigint
as
select CD.DyeingSpChallanDetailId,b.PartyName, B.BuyerName,B.OrderName,B.StyleName,

 B.BatchNo,B.BtRefNo, CD.BatchId,CD.BatchDetailId,I.ItemName as FabType,GSP.GroupName as SubProcessName, CD.FinishWeight as FabQty,CD.GreyWeight as GreyWt,
ISNULL((select SUM(ISNULL(FFD.RcvQty,0)-ISNULL(FFD.RejQty,0)) from Inventory_FinishFabDetailStore as FFD
inner join Inventory_FinishFabStore as FF on FFD.FinishFabStoreId= FF.FinishFabStoreId
where FF.DyeingSpChallanId=CD.DyeingSpChallanId and FFD.BatchDetailId=CD.BatchDetailId ),0) as RcvQty ,
ISNULL((select SUM(ISNULL(FFD.RcvQty,0)-ISNULL(FFD.RejQty,0)) from Inventory_FinishFabDetailStore as FFD
inner join Inventory_FinishFabStore as FF on FFD.FinishFabStoreId= FF.FinishFabStoreId
where FF.DyeingSpChallanId=CD.DyeingSpChallanId and FFD.BatchDetailId=CD.BatchDetailId and  FF.FinishFabStoreId=@FinishFabStoreId),0) as Qty ,
ISNULL(CD.CcuffQty,0) as CcuffQty,
(select FFD.Remarks from Inventory_FinishFabDetailStore as FFD
inner join Inventory_FinishFabStore as FF on FFD.FinishFabStoreId= FF.FinishFabStoreId
where FF.DyeingSpChallanId=CD.DyeingSpChallanId and FFD.BatchDetailId=CD.BatchDetailId and  FF.FinishFabStoreId=@FinishFabStoreId) as Remarks 



from PROD_DyeingSpChallanDetail as CD
inner join PROD_BatchDetail as BD on CD.BatchDetailId=BD.BatchDetailId 
inner join VProBatch as B on BD.BatchId=B.BatchId
inner join Inventory_Item as I on BD.ItemId=I.ItemId
inner join PROD_GroupSubProcess as GSP on CD.SpGroupId=GSP.GroupSubProcessId
where CD.DyeingSpChallanId=@DyeingSpChallanId

--(CD.GreyWeight-ISNULL((select SUM(GreyWt) from Inventory_FinishFabDetailStore where BatchDetailId=CD.BatchDetailId),0)) as GreyWt,
--exec SpInvFinishFabStore 0,7
