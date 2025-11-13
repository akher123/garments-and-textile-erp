
-- ==============================================================================================================================================
-- Author:		<Md.Akheruzzaman>
-- Create date: <02/11/2015>
-- Description:	<> exec [InvSpReportDailyStock] '02', '02001','2016-01-30','-1'
-- ==============================================================================================================================================

CREATE PROCEDURE [dbo].[InvSpReportDailyStock]
	                    @GroupId int,
						@SubGroupId int,
                        @Date datetime ,
                        @GenericNameId int

AS
BEGIN
	
	SET NOCOUNT ON;

		truncate table        Inventory_RStock

INSERT INTO Inventory_RStock
                      (ItemId, ItemCode, OQty, OAmt, RQty, RAmt, IQty, IAmt)
SELECT     ItemId, ItemCode, 0 AS OQty, 0 AS OAmt, 0 AS RQty, 0 AS RAmt, 0 AS IQty, 0 AS IAmt
FROM         VInvItem
WHERE    ( SubGroupId = @SubGroupId or @SubGroupId='-1') and (GroupId =@GroupId)

Update Inventory_RStock Set OQty= isnull(( SELECT SUM(STL.Quantity) FROM Inventory_StoreLedger as STL
WHERE STL.TransactionType = 1 AND (STL.TransactionDate < @Date)  AND (STL.ItemId = Inventory_RStock.ItemId) ), 0)
Update Inventory_RStock Set OAmt= isnull(( SELECT SUM(Inventory_StoreLedger.Amount) FROM Inventory_StoreLedger 
WHERE Inventory_StoreLedger.TransactionType = 1 AND (Convert(date,Inventory_StoreLedger.TransactionDate) < Convert(date,@Date))  AND (Inventory_StoreLedger.ItemId = Inventory_RStock.ItemId) ), 0)


Update Inventory_RStock Set OQty=OQty- isnull(( SELECT SUM(Inventory_StoreLedger.Quantity) FROM Inventory_StoreLedger
WHERE Inventory_StoreLedger.TransactionType = 2 AND (Convert(date,Inventory_StoreLedger.TransactionDate)< Convert(date,@Date))  AND (Inventory_StoreLedger.ItemId = Inventory_RStock.ItemId) ), 0)

Update Inventory_RStock Set OAmt=OAmt- isnull(( SELECT SUM(Inventory_StoreLedger.Amount) FROM Inventory_StoreLedger
WHERE Inventory_StoreLedger.TransactionType = 2 AND (Convert(date,Inventory_StoreLedger.TransactionDate) < Convert(date,@Date))  AND (Inventory_StoreLedger.ItemId = Inventory_RStock.ItemId) ), 0)


Update Inventory_RStock Set RQty= isnull(( SELECT SUM(Inventory_StoreLedger.Quantity) FROM Inventory_StoreLedger
WHERE Inventory_StoreLedger.TransactionType = 1 AND (Convert(date,Inventory_StoreLedger.TransactionDate)>= Convert(date,@Date)) and (Inventory_StoreLedger.TransactionDate <=Convert(date,@Date))  AND (Inventory_StoreLedger.ItemId = Inventory_RStock.ItemId) ), 0)
Update Inventory_RStock Set RAmt= isnull(( SELECT SUM(Inventory_StoreLedger.Amount) FROM Inventory_StoreLedger
WHERE Inventory_StoreLedger.TransactionType = 1 AND (Convert(date,Inventory_StoreLedger.TransactionDate) >= Convert(date,@Date)) and (Inventory_StoreLedger.TransactionDate <=Convert(date,@Date))  AND (Inventory_StoreLedger.ItemId = Inventory_RStock.ItemId) ), 0)


Update Inventory_RStock Set IQty= isnull(( SELECT SUM(Inventory_StoreLedger.Quantity) FROM Inventory_StoreLedger
WHERE Inventory_StoreLedger.TransactionType = 2 AND (Convert(date,Inventory_StoreLedger.TransactionDate)>= Convert(date,@Date))  and (Inventory_StoreLedger.TransactionDate <= Convert(date,@Date)) AND (Inventory_StoreLedger.ItemId = Inventory_RStock.ItemId) ), 0)

Update Inventory_RStock Set IAmt= isnull(( SELECT SUM(Inventory_StoreLedger.Amount) FROM Inventory_StoreLedger
WHERE Inventory_StoreLedger.TransactionType = 2 AND (Convert(date,Inventory_StoreLedger.TransactionDate) >= Convert(date,@Date))  and (Inventory_StoreLedger.TransactionDate <= Convert(date,@Date)) AND (ItemId = Inventory_RStock.ItemId) ), 0)


/*Commented by Golam Rabbi on 2016.04.06*/
--Update Inventory_RStock Set OutConsQty= isnull(( SELECT SUM(Quantity) FROM Inventory_MaterialIssue as MI

--inner join Inventory_StoreLedger as SL on MI.MaterialIssueId=SL.MaterialIssueId

--WHERE TransactionType = 2 and MI.IType=3 and MI.IType=4  AND (TransactionDate >= @Date)  and (TransactionDate <= @Date) AND (ItemId = Inventory_RStock.ItemId) ), 0)

/*Updated by Golam Rabbi on 2016.04.06*/
Update Inventory_RStock Set OutConsQty= isnull(( SELECT SUM(MI.Quantity) FROM Inventory_MaterialIssue as MI
inner join Inventory_StoreLedger as SL on MI.MaterialIssueId=SL.MaterialIssueId
WHERE SL.TransactionType = 2 and MI.IType IN (3,4)
AND (convert(date,SL.TransactionDate) >= Convert(date,@Date))  and (Convert(date,SL.TransactionDate) <= Convert(date,@Date)) AND (SL.ItemId = Inventory_RStock.ItemId) ), 0)

select R.*, I.ItemName,GN.Name as GenericName,MU.UnitName,SG.SubGroupName,G.GroupName From Inventory_RStock as R, Inventory_Item as I 
inner join Inventory_SubGroup as SG on SG.SubGroupId=I.SubGroupId
inner join Inventory_Group as G on G.GroupId=SG.GroupId
left join Inventory_GenericName as GN on I.GenericNameId=GN.GenericNameId and I.CompId=GN.CompId
inner join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
where R.ItemCode=I.ItemCode and (R.OQty>0  or R.RQty>0 or R.IQty>0) and (GN.GenericNameId=@GenericNameId or @GenericNameId='-1' )
order by  I.ItemCode 
END





