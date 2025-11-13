CREATE procedure [dbo].[spMonthlyRateUpdate]
@ReportDate datetime
as
BEGIN
declare @yearMonth INT;
set @yearMonth=CONVERT(int,(YEAR(@ReportDate) * 100 + MONTH(@ReportDate)));

truncate table Inventory_TempCostRate

INSERT INTO Inventory_TempCostRate
                         (ItemId, RQty, RAmt, IQty, IAmt, BQty, BAmt, BRate)
SELECT DISTINCT ItemId, 0 AS RQty, 0 AS RAmt, 0 AS IQty, 0 AS IAmt, 0 AS BQty, 0 AS BAmt, 0 AS BRate
FROM            Inventory_StoreLedger
WHERE        (YEAR(TransactionDate) * 100 + MONTH(TransactionDate) <= @yearMonth)


Update Inventory_TempCostRate set RQty = isnull(( SELECT        ISNULL(SUM(Quantity), 0) AS Expr1
FROM            Inventory_StoreLedger
WHERE        (YEAR(TransactionDate) * 100 + MONTH(TransactionDate) <= @yearMonth) AND (ItemId = Inventory_TempCostRate.ItemId) AND (TransactionType = 1)),0)


Update Inventory_TempCostRate set RAmt = isnull(( SELECT ISNULL(SUM(Quantity*UnitPrice), 0) AS Expr1
FROM            Inventory_StoreLedger
WHERE        (YEAR(TransactionDate) * 100 + MONTH(TransactionDate) <= @yearMonth) AND (ItemId = Inventory_TempCostRate.ItemId) AND (TransactionType = 1)),0)

Update Inventory_TempCostRate set IQty = isnull(( SELECT        ISNULL(SUM(Quantity), 0) AS Expr1
FROM            Inventory_StoreLedger
WHERE        (YEAR(TransactionDate) * 100 + MONTH(TransactionDate) < @yearMonth) AND (ItemId = Inventory_TempCostRate.ItemId) AND (TransactionType = 2)),0)


Update Inventory_TempCostRate set IAmt = isnull(( SELECT ISNULL(SUM(Quantity*UnitPrice), 0) AS Expr1
FROM            Inventory_StoreLedger
WHERE        (YEAR(TransactionDate) * 100 + MONTH(TransactionDate) < @yearMonth) AND (ItemId = Inventory_TempCostRate.ItemId) AND (TransactionType = 2)),0)

Update Inventory_TempCostRate set BQty = RQty - IQty
Update Inventory_TempCostRate set BAmt = RAmt - IAmt

Update Inventory_TempCostRate set BRate = BAmt/BQty where BQty <> 0


update  Inventory_StoreLedger set UnitPrice= isnull(( select top 1 BRate from Inventory_TempCostRate where ItemId=Inventory_StoreLedger.ItemId ),0)
where Inventory_StoreLedger.TransactionType=2 and (YEAR(Inventory_StoreLedger.TransactionDate) * 100 + MONTH(Inventory_StoreLedger.TransactionDate) = @yearMonth)


update  Inventory_StoreLedger set Amount= UnitPrice *Quantity 
where Inventory_StoreLedger.TransactionType=2 and (YEAR(Inventory_StoreLedger.TransactionDate) * 100 + MONTH(Inventory_StoreLedger.TransactionDate) = @yearMonth)
END


--exec [dbo].[spMonthlyRateUpdate] '2017-11-01'