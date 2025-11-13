
CREATE procedure [dbo].[InvSpLoanSummary]
 @FromDate datetime ,
 @ToDate datetime,
 @SupplierId int
as

Truncate Table  Inv_LoanBalanceReport
INSERT INTO Inv_LoanBalanceReport (SupplierId, ItemId, RQty, IQty, TRQty, TIQty )
SELECT    distinct    SupplierId, ItemId, 0 AS RQty, 0 AS IQty, ISNULL(SUM( CAST( Quantity AS bigint)), 0) AS TRQty, 0 AS TIQty
FROM            VItemReceive
WHERE        (ReceiveType = 3) AND (Convert(date,TransactionDate) <=Convert(date, @ToDate))
GROUP BY SupplierId, ItemId

INSERT INTO Inv_LoanBalanceReport
                         (SupplierId, ItemId, RQty, IQty, TRQty, TIQty)
SELECT     distinct   I.SupplierId, ISL.ItemId, 0 AS RQty, 0 AS IQty, 0 AS TRQty, 0 AS TIQty
FROM            Inventory_MaterialIssue AS I INNER JOIN
                         Inventory_StoreLedger AS ISL ON I.MaterialIssueId = ISL.MaterialIssueId
WHERE     ISL.TransactionType=2 and (I.IsActive = 1) AND (I.IType IN (3, 4)) 
AND ( Convert(date,ISL.TransactionDate) <=  Convert(date,@ToDate)) 
AND ISL.ItemId  
NOT IN (SELECT  S.ItemId  FROM  Inv_LoanBalanceReport AS S where SupplierId=@SupplierId  )

update Inv_LoanBalanceReport set RQty = isnull(( Select Sum(Quantity) from VItemReceive WHERE 
ReceiveType = 3 AND Convert(date,TransactionDate) >= Convert(date,@FromDate) AND Convert(date,TransactionDate) <=Convert(date, @ToDate) and SupplierId=Inv_LoanBalanceReport.SupplierId and ItemId=Inv_LoanBalanceReport.ItemId ),0) 


update Inv_LoanBalanceReport set TIQty=isnull(( SELECT SUM(ISL.Quantity) FROM   Inventory_MaterialIssue AS I INNER JOIN
Inventory_StoreLedger AS ISL ON I.MaterialIssueId = ISL.MaterialIssueId
WHERE     ISL.TransactionType=2 and   (I.IsActive = 1) AND (I.IType IN (3, 4)) AND ( Convert(date,ISL.TransactionDate)  <=  Convert(date,@ToDate)) AND (I.SupplierId = Inv_LoanBalanceReport.SupplierId) AND (ISL.ItemId = Inv_LoanBalanceReport.ItemId) ),0)

update Inv_LoanBalanceReport set IQty=isnull(( SELECT SUM(ISL.Quantity) FROM   Inventory_MaterialIssue AS I INNER JOIN
Inventory_StoreLedger AS ISL ON I.MaterialIssueId = ISL.MaterialIssueId
WHERE  ISL.TransactionType=2 and      (I.IsActive = 1) AND (I.IType IN (3, 4)) AND ( Convert(date,ISL.TransactionDate ) >=  Convert(date,@FromDate)) AND ( Convert(date,ISL.TransactionDate ) <=  Convert(date,@ToDate)) AND (I.SupplierId = Inv_LoanBalanceReport.SupplierId) AND (ISL.ItemId = Inv_LoanBalanceReport.ItemId) ),0)


Delete From Inv_LoanBalanceReport where RQty=0 and IQty=0 and (TRQty - TIQty)*(TRQty - TIQty) < 0.1

Select SL.ItemId, SL.SupplierId, SL.RQty as ReciveQty, SL.IQty as IssuQty, SL.TIQty-SL.TRQty as Balance, I.ItemCode, I.ItemName, 
SUPL.CompanyName , I.UnitName 
From Inv_LoanBalanceReport as SL
inner join VInvItem as I on SL.ItemId=I.ItemId
inner join Mrc_SupplierCompany as SUPL on SL.SupplierId=SUPL.SupplierCompanyId
where   (SL.SupplierId=@SupplierId or @SupplierId=-1) order by SL.SupplierId












