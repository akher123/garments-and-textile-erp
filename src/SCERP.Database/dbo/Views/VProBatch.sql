





CREATE view [dbo].[VProBatch]
as
SELECT        
dbo.Pro_Batch.BatchId,
dbo.Pro_Batch.BtType,
dbo.Pro_Batch.BatchNo,
dbo.Pro_Batch.BtRefNo,
dbo.Pro_Batch.BatchQty,
dbo.Pro_Batch.BatchDate,
dbo.Pro_Batch.BatchStatus,
dbo.Pro_Batch.CompId,
dbo.Pro_Batch.ItemId,
I.ItemName,
I.ItemCode,
dbo.Pro_Batch.Gsm,
dbo.Pro_Batch.GrColorRefId,
GC.ColorName as GColorName,
dbo.Pro_Batch.GSizeRefId,
GS.SizeName as GSizeName,
dbo.Pro_Batch.FColorRefId,
FC.ColorName as FColorName,
dbo.Pro_Batch.FSizeRefId,
FS.SizeName as FSizeName,
dbo.Pro_Batch.CostRate,
dbo.Pro_Batch.BillRate,
dbo.Pro_Batch.ShadePerc,
dbo.Pro_Batch.OrderStyleRefId,
dbo.Pro_Batch.ApprovedLdNo,
dbo.Pro_Batch.Remarks,
dbo.Pro_Batch.LoadingDateTime,
dbo.Pro_Batch.UnLoadingDateTime,
BST.BuyerName,
BST.RefNo as OrderName,
BST.StyleName,
Pro_Batch.BuyerRefId,
BST.OrderNo,
dbo.Pro_Batch.ConsumptionGroupId,
dbo.Party.PartyId,
dbo.Party.Name as PartyName,
dbo.Party.PartyRefNo,
dbo.Production_Machine.Name AS MachineName,
dbo.Production_Machine.MachineId, 
dbo.Color.ColorId, dbo.Color.ColorRef, 
dbo.Color.ColorName

		FROM		 dbo.Pro_Batch INNER JOIN
					 dbo.Party ON dbo.Pro_Batch.PartyId = dbo.Party.PartyId INNER JOIN
					 dbo.Production_Machine ON dbo.Pro_Batch.MachineId = dbo.Production_Machine.MachineId left JOIN
					 dbo.Color ON dbo.Pro_Batch.ColorId = dbo.Color.ColorId
					 left join dbo.OM_Color as GC on dbo.Pro_Batch.GrColorRefId=GC.ColorRefId and dbo.Pro_Batch.CompId=GC.CompId
					 left join dbo.OM_Color as FC on dbo.Pro_Batch.FColorRefId=FC.ColorRefId and dbo.Pro_Batch.CompId=FC.CompId
					 left join dbo.OM_Size as GS on dbo.Pro_Batch.GSizeRefId=GS.SizeRefId and dbo.Pro_Batch.CompId=GS.CompId
					 left join dbo.OM_Size as FS on dbo.Pro_Batch.FSizeRefId=FS.SizeRefId and  dbo.Pro_Batch.CompId=FS.CompId
					 left join dbo.Inventory_Item as I on dbo.Pro_Batch.ItemId=I.ItemId and  dbo.Pro_Batch.CompId=I.CompId
					 left join VOM_BuyOrdStyle as BST on dbo.Pro_Batch.OrderStyleRefId=BST.OrderStyleRefId and dbo.Pro_Batch.BuyerRefId=BST.BuyerRefId and dbo.Pro_Batch.CompId=BST.CompId 
			 where  dbo.Pro_Batch.IsActive=1









