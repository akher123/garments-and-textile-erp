CREATE PROCEDURE [dbo].[InvBuyerWiseYarnStockSummary]
                        @FromDate datetime ,
                        @ToDate datetime
						as
truncate table  Inventory_RStock

INSERT INTO Inventory_RStock
                      ( OQty, OAmt, RQty, RAmt, IQty, IAmt,BuyerRefId)
select distinct    0 AS OQty, 0 AS OAmt, 0 AS RQty, 0 AS RAmt, 0 AS IQty, 0 AS IAmt,B.BuyerRefId from Inventory_MaterialReceiveAgainstPo as MR
inner join Inventory_StockRegister as SR on MR.MaterialReceiveAgstPoId=SR.SourceId
inner join OM_Buyer as B on MR.BuyerId=B.BuyerId
inner join Inventory_Item as I on SR.ItemId=I.ItemId
inner join Inventory_SubGroup as Sg on I.SubGroupId=Sg.SubGroupId
inner join Inventory_Group as g on Sg.GroupId=g.GroupId
where SR.StoreId=1  and SR.ActionType=3 and  g.GroupId=19 and I.ItemType=1

Update Inventory_RStock Set OQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
inner join Inventory_MaterialReceiveAgainstPo as YR on Inventory_StockRegister.SourceId=YR.MaterialReceiveAgstPoId
inner join OM_Buyer as B on YR.BuyerId=B.BuyerId
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId

WHERE B.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1  and  TransactionType = 1 AND (Convert(date,TransactionDate) < Convert(date, @FromDate))), 0)

Update Inventory_RStock Set OAmt= isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
inner join Inventory_MaterialReceiveAgainstPo as YR on Inventory_StockRegister.SourceId=YR.MaterialReceiveAgstPoId
inner join OM_Buyer as B on YR.BuyerId=B.BuyerId
WHERE  B.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1  and TransactionType = 1 AND (Convert(date,TransactionDate) < Convert(date, @FromDate))), 0)

Update Inventory_RStock Set OQty=OQty- isnull(( SELECT SUM(Inventory_StockRegister.Quantity) FROM Inventory_StockRegister
inner join Inventory_AdvanceMaterialIssue as YI on Inventory_StockRegister.SourceId=YI.AdvanceMaterialIssueId
WHERE  YI.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1 and  TransactionType = 2 AND (Convert(date,TransactionDate) <Convert(date, @FromDate))), 0)

Update Inventory_RStock Set OAmt=OAmt- isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
inner join Inventory_AdvanceMaterialIssue as YI on Inventory_StockRegister.SourceId=YI.AdvanceMaterialIssueId
WHERE YI.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1 and  TransactionType = 2 AND (Convert(date,TransactionDate) < Convert(date, @FromDate)) ), 0)


Update Inventory_RStock Set RQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
inner join Inventory_MaterialReceiveAgainstPo as YR on Inventory_StockRegister.SourceId=YR.MaterialReceiveAgstPoId
inner join OM_Buyer as B on YR.BuyerId=B.BuyerId
WHERE B.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1  and TransactionType = 1 AND (Convert(date,TransactionDate) >= Convert(date, @FromDate)) and (TransactionDate <=Convert(date, @ToDate))), 0)

Update Inventory_RStock Set RAmt= isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
inner join Inventory_MaterialReceiveAgainstPo as YR on Inventory_StockRegister.SourceId=YR.MaterialReceiveAgstPoId
inner join OM_Buyer as B on YR.BuyerId=B.BuyerId
WHERE B.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1  and  TransactionType = 1 AND (Convert(date,TransactionDate) >= Convert(date, @FromDate)) and (TransactionDate <=Convert(date, @ToDate))), 0)


Update Inventory_RStock Set IQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
inner join Inventory_AdvanceMaterialIssue as YI on Inventory_StockRegister.SourceId=YI.AdvanceMaterialIssueId
WHERE  YI.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1 and TransactionType = 2 AND (Convert(date,TransactionDate) >= Convert(date, @FromDate))  and (TransactionDate <= Convert(date, @ToDate)) ), 0)

Update Inventory_RStock Set IAmt= isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
inner join Inventory_AdvanceMaterialIssue as YI on Inventory_StockRegister.SourceId=YI.AdvanceMaterialIssueId
WHERE YI.BuyerRefId=Inventory_RStock.BuyerRefId and Inventory_StockRegister.StoreId=1 and TransactionType = 2 AND (Convert(date,TransactionDate) >=Convert(date, @FromDate))  and (TransactionDate <= Convert(date, @ToDate))), 0)

select B.BuyerName,  R.OQty, R.OAmt, R.RQty, R.RAmt, R.IQty,IAmt From Inventory_RStock as R
inner join OM_Buyer as B on R.BuyerRefId=B.BuyerRefId

where R.BuyerRefId=B.BuyerRefId
and (R.OQty+R.RQty+ R.IQty>0)
order by B.BuyerName



--exec InvBuyerWiseYarnStockSummary '2018-01-01','2018-01-18'