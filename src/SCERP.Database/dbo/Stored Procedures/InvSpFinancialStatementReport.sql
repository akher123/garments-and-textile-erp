CREATE procedure 	[dbo].[InvSpFinancialStatementReport]

@FromDate datetime ,
@ToDate datetime
 as 
 truncate table Inv_FinancialStatementReport
INSERT INTO Inv_FinancialStatementReport
                      (GroupId, SubGroupId, OAmt, RAmt, IAmt)
SELECT   GroupId, SubGroupId, 0 AS OAmt, 0 AS RAmt, 0 AS IAmt
FROM         Inventory_SubGroup

Update Inv_FinancialStatementReport Set OAmt= isnull(( SELECT SUM(SL.Amount) FROM Inventory_StoreLedger as  SL
inner join Inventory_Item as I on SL.ItemId=I.ItemId
WHERE SL.TransactionType = 1 AND (Convert(date,SL.TransactionDate )< Convert(date,@FromDate))  AND (I.SubGroupId = Inv_FinancialStatementReport.SubGroupId) ), 0)
Update Inv_FinancialStatementReport Set RAmt= isnull(( SELECT SUM(SL.Amount) FROM Inventory_StoreLedger as  SL
inner join Inventory_Item as I on SL.ItemId=I.ItemId
WHERE SL.TransactionType = 1 AND (Convert(date,SL.TransactionDate ) >=  Convert(date,@FromDate)) and (Convert(date,SL.TransactionDate ) < = Convert(date,@ToDate ))  AND (I.SubGroupId = Inv_FinancialStatementReport.SubGroupId) ), 0)

Update Inv_FinancialStatementReport Set IAmt= isnull(( SELECT SUM(SL.Amount) FROM Inventory_StoreLedger as  SL
inner join Inventory_Item as I on SL.ItemId=I.ItemId
WHERE SL.TransactionType = 2 AND (Convert(date,SL.TransactionDate ) >=  Convert(date,@FromDate)) and (Convert(date,SL.TransactionDate ) <=Convert(date,@ToDate ))  AND (I.SubGroupId = Inv_FinancialStatementReport.SubGroupId) ), 0)

 select G.GroupName,SG.SubGroupName, FS.OAmt,FS.RAmt,FS.IAmt from Inv_FinancialStatementReport as FS

 inner join Inventory_Group as G on FS.GroupId=G.GroupId

 inner join Inventory_SubGroup as SG on FS.SubGroupId=SG.SubGroupId

 where  (FS.OAmt>0  or FS.RAmt>0 or FS.IAmt>0)






