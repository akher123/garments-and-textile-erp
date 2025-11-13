CREATE PROCEDURE [dbo].[InvSpAdvanceReportStock]

						@SubGroupId int,
						@GroupId int,
                        @FromDate datetime ,
                        @ToDate datetime
AS
BEGIN
	
	SET NOCOUNT ON;
	truncate table  Inventory_RStock

INSERT INTO Inventory_RStock
                      (ItemId, ItemCode, OQty, OAmt, RQty, RAmt, IQty, IAmt)
SELECT     ItemId, ItemCode, 0 AS OQty, 0 AS OAmt, 0 AS RQty, 0 AS RAmt, 0 AS IQty, 0 AS IAmt
FROM         Inventory_Item as I
inner join Inventory_SubGroup as Sg on I.SubGroupId=Sg.SubGroupId
inner join Inventory_Group as g on Sg.GroupId=g.GroupId
WHERE    (Sg.SubGroupId = @SubGroupId or @SubGroupId=-1)and g.GroupId=@GroupId and I.ItemType=1

Update Inventory_RStock Set OQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
WHERE TransactionType = 1 AND (TransactionDate < @FromDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)
Update Inventory_RStock Set OAmt= isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
WHERE TransactionType = 1 AND (TransactionDate < @FromDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)


Update Inventory_RStock Set OQty=OQty- isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
WHERE TransactionType = 2 AND (TransactionDate < @FromDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)

Update Inventory_RStock Set OAmt=OAmt- isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
WHERE TransactionType = 2 AND (TransactionDate < @FromDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)


Update Inventory_RStock Set RQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
WHERE TransactionType = 1 AND (TransactionDate >= @FromDate) and (TransactionDate <=@ToDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)
Update Inventory_RStock Set RAmt= isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
WHERE TransactionType = 1 AND (TransactionDate >= @FromDate) and (TransactionDate <=@ToDate)  AND (ItemId = Inventory_RStock.ItemId) ), 0)


Update Inventory_RStock Set IQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
WHERE TransactionType = 2 AND (TransactionDate >= @FromDate)  and (TransactionDate <= @ToDate) AND (ItemId = Inventory_RStock.ItemId) ), 0)

Update Inventory_RStock Set IAmt= isnull(( SELECT SUM(Rate*Quantity) FROM Inventory_StockRegister
WHERE TransactionType = 2 AND (TransactionDate >= @FromDate)  and (TransactionDate <= @ToDate) AND (ItemId = Inventory_RStock.ItemId) ), 0)

select R.*, I.ItemName,GN.Name as GenericName,MU.UnitName,SG.SubGroupName,G.GroupName From Inventory_RStock as R, Inventory_Item as I 
inner join Inventory_SubGroup as SG on SG.SubGroupId=I.SubGroupId
inner join Inventory_Group as G on G.GroupId=SG.GroupId
inner join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
left join Inventory_GenericName as GN on I.GenericNameId=GN.GenericNameId
where R.ItemCode=I.ItemCode and I.ItemType=1 and (R.OQty>0  or R.RQty>0 or R.IQty>0)
order by I.ItemName

END