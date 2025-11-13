--exec [[InvBuyerWiseYarnStockDetail]] 23,19,'2016-02-01','2016-06-16'
CREATE PROCEDURE [dbo].[InvBuyerWiseYarnStockDetail]
                        @FromDate datetime ,
                        @ToDate datetime
						as
	truncate table  Inventory_RStock

INSERT INTO Inventory_RStock
                      (ItemId, ItemCode, OQty, OAmt, RQty, RAmt, IQty, IAmt,SizeRefId,ColorRefId,BrandId,BuyerRefId)
select distinct   SR.ItemId, ItemCode, 0 AS OQty, 0 AS OAmt, 0 AS RQty, 0 AS RAmt, 0 AS IQty, 0 AS IAmt,SR.SizeRefId,SR.ColorRefId,C.ColorCode as BrandId,B.BuyerRefId from Inventory_MaterialReceiveAgainstPo as MR
inner join Inventory_StockRegister as SR on MR.MaterialReceiveAgstPoId=SR.SourceId

inner join OM_Buyer as B on MR.BuyerId=B.BuyerId
inner join OM_Color as C on SR.ColorRefId=C.ColorRefId and SR.CompId=C.CompId
inner join Inventory_Item as I on SR.ItemId=I.ItemId
inner join Inventory_SubGroup as Sg on I.SubGroupId=Sg.SubGroupId
inner join Inventory_Group as g on Sg.GroupId=g.GroupId
where SR.StoreId=1  and SR.ActionType=3 and  g.GroupId=19 and I.ItemType=1

Update Inventory_RStock Set OQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
inner join Inventory_MaterialReceiveAgainstPo as YR on Inventory_StockRegister.SourceId=YR.MaterialReceiveAgstPoId
inner join OM_Buyer as B on YR.BuyerId=B.BuyerId
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId

WHERE B.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1  and  TransactionType = 1 AND (Convert(date,TransactionDate) < Convert(date, @FromDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)

Update Inventory_RStock Set OAmt= isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
inner join Inventory_MaterialReceiveAgainstPo as YR on Inventory_StockRegister.SourceId=YR.MaterialReceiveAgstPoId
inner join OM_Buyer as B on YR.BuyerId=B.BuyerId
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId
WHERE  B.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1  and TransactionType = 1 AND (Convert(date,TransactionDate) < Convert(date, @FromDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)

Update Inventory_RStock Set OQty=OQty- isnull(( SELECT SUM(Inventory_StockRegister.Quantity) FROM Inventory_StockRegister
inner join Inventory_AdvanceMaterialIssue as YI on Inventory_StockRegister.SourceId=YI.AdvanceMaterialIssueId
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId
WHERE  YI.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1 and  TransactionType = 2 AND (Convert(date,TransactionDate) <Convert(date, @FromDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)

Update Inventory_RStock Set OAmt=OAmt- isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
inner join Inventory_AdvanceMaterialIssue as YI on Inventory_StockRegister.SourceId=YI.AdvanceMaterialIssueId
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId
WHERE YI.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1 and  TransactionType = 2 AND (Convert(date,TransactionDate) < Convert(date, @FromDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)


Update Inventory_RStock Set RQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
inner join Inventory_MaterialReceiveAgainstPo as YR on Inventory_StockRegister.SourceId=YR.MaterialReceiveAgstPoId
inner join OM_Buyer as B on YR.BuyerId=B.BuyerId
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId
WHERE B.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1  and TransactionType = 1 AND (Convert(date,TransactionDate) >= Convert(date, @FromDate)) and (TransactionDate <=Convert(date, @ToDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)

Update Inventory_RStock Set RAmt= isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
inner join Inventory_MaterialReceiveAgainstPo as YR on Inventory_StockRegister.SourceId=YR.MaterialReceiveAgstPoId
inner join OM_Buyer as B on YR.BuyerId=B.BuyerId
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId
WHERE B.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1  and  TransactionType = 1 AND (Convert(date,TransactionDate) >= Convert(date, @FromDate)) and (TransactionDate <=Convert(date, @ToDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)


Update Inventory_RStock Set IQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
inner join Inventory_AdvanceMaterialIssue as YI on Inventory_StockRegister.SourceId=YI.AdvanceMaterialIssueId
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId
WHERE  YI.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1 and TransactionType = 2 AND (Convert(date,TransactionDate) >= Convert(date, @FromDate))  and (TransactionDate <= Convert(date, @ToDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)


Update Inventory_RStock Set IAmt= isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
inner join Inventory_AdvanceMaterialIssue as YI on Inventory_StockRegister.SourceId=YI.AdvanceMaterialIssueId
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId
WHERE YI.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1 and TransactionType = 2 AND (Convert(date,TransactionDate) >=Convert(date, @FromDate))  and (TransactionDate <= Convert(date, @ToDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)

select B.BuyerName, R.ItemId,R.ItemCode, R.OQty, R.OAmt, R.RQty, R.RAmt, R.IQty,IAmt,C.ColorName,SZ.SizeName,BR.Name as Brand, I.ItemName,GN.Name as GenericName,MU.UnitName,SG.SubGroupName,G.GroupName From Inventory_RStock as R
inner join  Inventory_Item as I  on R.ItemId=I.ItemId
inner join  OM_Color as C on R.ColorRefId=C.ColorRefId 
inner join OM_Size as SZ on R.SizeRefId=SZ.SizeRefId
inner join Inventory_Brand as BR on R.BrandId=BR.BrandId 
inner join Inventory_SubGroup as SG on SG.SubGroupId=I.SubGroupId
inner join Inventory_Group as G on G.GroupId=SG.GroupId
inner join OM_Buyer as B on R.BuyerRefId=B.BuyerRefId
inner join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId

left join Inventory_GenericName as GN on I.GenericNameId=GN.GenericNameId

where R.ItemCode=I.ItemCode and I.ItemType=1 
and (R.OQty+R.RQty+ R.IQty>0)
order by I.ItemName









