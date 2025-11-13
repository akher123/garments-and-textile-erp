CREATE PROCEDURE spInvStockValue
 @FromDate datetime ,
 @ToDate datetime
AS
BEGIN
	
	SET NOCOUNT ON;

		truncate table        Inventory_RStock

INSERT INTO Inventory_RStock
                      (ItemId, ItemCode, OQty, OAmt, RQty, RAmt, IQty, IAmt)
SELECT     ItemId, ItemCode, 0 AS OQty, 0 AS OAmt, 0 AS RQty, 0 AS RAmt, 0 AS IQty, 0 AS IAmt
FROM         Inventory_Item 

--and I.ItemType=1

Update Inventory_RStock Set OQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StoreLedger
WHERE TransactionType = 1 AND (TransactionDate < @FromDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)
Update Inventory_RStock Set OAmt= isnull(( SELECT SUM(Amount) FROM Inventory_StoreLedger
WHERE TransactionType = 1 AND (TransactionDate < @FromDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)


Update Inventory_RStock Set OQty=OQty- isnull(( SELECT SUM(Quantity) FROM Inventory_StoreLedger
WHERE TransactionType = 2 AND (TransactionDate < @FromDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)

Update Inventory_RStock Set OAmt=OAmt- isnull(( SELECT SUM(Amount) FROM Inventory_StoreLedger
WHERE TransactionType = 2 AND (TransactionDate < @FromDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)


Update Inventory_RStock Set RQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StoreLedger
WHERE TransactionType = 1 AND (TransactionDate >= @FromDate) and (TransactionDate <=@ToDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)
Update Inventory_RStock Set RAmt= isnull(( SELECT SUM(Amount) FROM Inventory_StoreLedger
WHERE TransactionType = 1 AND (TransactionDate >= @FromDate) and (TransactionDate <=@ToDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)


Update Inventory_RStock Set IQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StoreLedger
WHERE TransactionType = 2 AND (TransactionDate >= @FromDate)  and (TransactionDate <= @ToDate) AND (ItemId = Inventory_RStock.ItemId) ), 0)

Update Inventory_RStock Set IAmt= isnull(( SELECT SUM(Amount) FROM Inventory_StoreLedger
WHERE TransactionType = 2 AND (TransactionDate >= @FromDate)  and (TransactionDate <= @ToDate) AND (ItemId = Inventory_RStock.ItemId) ), 0)

select ROUND(SUM(R.OAmt),2) as TOAmt, ROUND(SUM(R.RAmt),2) as TRAmt,ROUND(SUM(R.IAmt),2) as IAmt,RTRIM(LTRIM(SG.SubGroupName)) as SubGroupName,RTRIM(LTRIM(G.GroupName)) as GroupName From Inventory_RStock as R, Inventory_Item as I 
inner join Inventory_SubGroup as SG on SG.SubGroupId=I.SubGroupId
inner join Inventory_Group as G on G.GroupId=SG.GroupId
where R.ItemCode=I.ItemCode and (R.OQty>0  or R.RQty>0 or R.IQty>0) 
--and I.ItemType=1 
group by SG.SubGroupName,G.GroupName

END






