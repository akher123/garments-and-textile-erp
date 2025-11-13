-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================


CREATE PROCEDURE [dbo].[InvSpStokDetailReport]
						@SubGroupCode nvarchar(50),
                        @FromDate datetime ,
                        @ToDate datetime,
						@GenericNameId int ,
						@GroupId int
AS
BEGIN
	
	SET NOCOUNT ON;

		truncate table        Inventory_RStock

INSERT INTO Inventory_RStock
  (ItemId, ItemCode, OQty, OAmt, RQty, RAmt, IQty, IAmt ,OutConsQty)
SELECT     ItemId, ItemCode, 0 AS OQty, 0 AS OAmt, 0 AS RQty, 0 AS RAmt, 0 AS IQty, 0 AS IAmt ,0 as OutConsQty
FROM         VInvItem
WHERE    ( SubGroupCode = @SubGroupCode or @SubGroupCode='-1') and GroupId=@GroupId

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


Update Inventory_RStock Set IQty= isnull(( SELECT SUM(SL.Quantity) FROM Inventory_StoreLedger as SL
inner join Inventory_MaterialIssue as MI on SL.MaterialIssueId=MI.MaterialIssueId
WHERE SL.TransactionType = 2 and MI.IType in (1,2) AND (SL.TransactionDate >= @FromDate)  and (SL.TransactionDate <= @ToDate) AND (SL.ItemId = Inventory_RStock.ItemId) ), 0)

Update Inventory_RStock Set IAmt= isnull(( SELECT SUM(SL.Amount) FROM Inventory_StoreLedger as SL
inner join Inventory_MaterialIssue as MI on SL.MaterialIssueId=MI.MaterialIssueId
WHERE SL.TransactionType = 2 and MI.IType in (1,2)  AND (SL.TransactionDate >= @FromDate)  and (SL.TransactionDate <= @ToDate) AND (SL.ItemId = Inventory_RStock.ItemId) ), 0)


/*Commented by Golam Rabbi on 2016.04.06*/
--Update Inventory_RStock 
--Set OutConsQty = isnull((SELECT SUM(Quantity) FROM Inventory_MaterialIssue as MI

--inner join Inventory_StoreLedger as SL on MI.MaterialIssueId=SL.MaterialIssueId

--WHERE TransactionType = 2 and MI.IType=3 and MI.IType=4  AND (TransactionDate >= @FromDate)  and (TransactionDate <= @ToDate) AND (ItemId = Inventory_RStock.ItemId) ), 0)

/*Updated by Golam Rabbi on 2016.04.06*/
Update Inventory_RStock 
Set OutConsQty = isnull((SELECT SUM(SL.Quantity) FROM Inventory_MaterialIssue as MI
inner join Inventory_StoreLedger as SL on MI.MaterialIssueId=SL.MaterialIssueId
WHERE TransactionType = 2 and MI.IType IN (3,4)  AND (TransactionDate >= @FromDate)  and (TransactionDate <= @ToDate) AND (ItemId = Inventory_RStock.ItemId) ), 0)

Update Inventory_RStock 
Set OutAmt = isnull((SELECT SUM(SL.Amount) FROM Inventory_MaterialIssue as MI
inner join Inventory_StoreLedger as SL on MI.MaterialIssueId=SL.MaterialIssueId
WHERE TransactionType = 2 and MI.IType IN (3,4)  AND (TransactionDate >= @FromDate)  and (TransactionDate <= @ToDate) AND (ItemId = Inventory_RStock.ItemId) ), 0)

select R.*, I.ItemName,GN.Name as GenericName,MU.UnitName,SG.SubGroupName,G.GroupName From Inventory_RStock as R, Inventory_Item as I 
inner join Inventory_SubGroup as SG on SG.SubGroupId=I.SubGroupId
left join Inventory_GenericName as GN on I.GenericNameId=GN.GenericNameId and I.GenericNameId=GN.GenericNameId and I.CompId=GN.CompId
inner join Inventory_Group as G on G.GroupId=SG.GroupId
inner join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
where R.ItemCode=I.ItemCode  and (GN.GenericNameId=@GenericNameId or @GenericNameId='-1' ) and (R.OQty>0  or R.RQty>0 or R.IQty>0)
--and I.ItemType=1
order by GN.Name
END





