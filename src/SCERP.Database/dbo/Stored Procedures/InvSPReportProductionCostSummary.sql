CREATE procedure [dbo].[InvSPReportProductionCostSummary]
                @FromDate datetime ,
				 @ToDate datetime ,
				 @PartyId int
AS

--select  
--  SUM(Pro_Batch.BatchQty) as GreyQty, (select Name from Party where PartyId=Pro_Batch.PartyId) as  PartyName, 
  
--((SELECT        ISNULL(SUM(SL.UnitPrice*SL.Quantity), 0) AS Expr1
--                               FROM            Inventory_StoreLedger AS SL INNER JOIN
--                                                         Inventory_MaterialIssue AS MIS ON SL.MaterialIssueId = MIS.MaterialIssueId 
														 
--														INNER JOIN  Pro_Batch as BT on MIS.BtRefNo=BT.BtRefNo
--														  inner join Inventory_Item AS ITM ON SL.ItemId = ITM.ItemId
--                                                            WHERE        (MIS.IType = 2 ) and MIS.ToppingType in(1,2,3) and BT.PartyId=Pro_Batch.PartyId and (convert(date,MIS.IssueReceiveDate) >= Convert(date,@FromDate)) and   (Convert(date,MIS.IssueReceiveDate) <= Convert(date,@ToDate))  AND (SL.TransactionType = 2) AND (LEFT(ITM.ItemCode, 5) = '01001')
							   
--							   ))AS TCCost,

--                             ((SELECT        ISNULL(SUM(SL.UnitPrice*SL.Quantity), 0) AS Expr1
--                               FROM            Inventory_StoreLedger AS SL INNER JOIN
--                                                         Inventory_MaterialIssue AS MIS ON SL.MaterialIssueId = MIS.MaterialIssueId 
--														 INNER JOIN Pro_Batch as BT on MIS.BtRefNo=BT.BtRefNo inner join 
--                                                         Inventory_Item AS ITM ON SL.ItemId = ITM.ItemId
--                               WHERE        (MIS.IType = 2 )and MIS.ToppingType in(1,2,3) and BT.PartyId=Pro_Batch.PartyId and (convert(date,MIS.IssueReceiveDate) >= Convert(date,@FromDate)) and   (Convert(date,MIS.IssueReceiveDate) <= Convert(date,@ToDate))  AND (SL.TransactionType = 2)  AND (LEFT(ITM.ItemCode, 5) = '01002')
--							   )) AS TDCost,
--							  0 as  TCost

--from Inventory_MaterialIssue
--inner join Pro_Batch on Inventory_MaterialIssue.BtRefNo=Pro_Batch.BtRefNo
--where convert(date ,Inventory_MaterialIssue.IssueReceiveDate)>=Convert(date,@FromDate) and Convert(date,Inventory_MaterialIssue.IssueReceiveDate)<=Convert(date,@ToDate) and Inventory_MaterialIssue.IType=2    and Inventory_MaterialIssue.ToppingType=1 
--group by Pro_Batch.PartyId



select  
  SUM(Pro_Batch.BatchQty) as GreyQty, (select Name from Party where PartyId=Pro_Batch.PartyId) as  PartyName, 
  
((SELECT        ISNULL(SUM(SL.Amount), 0) AS Expr1
                               FROM            Inventory_StoreLedger AS SL INNER JOIN
                                                         Inventory_MaterialIssue AS MIS ON SL.MaterialIssueId = MIS.MaterialIssueId 
														 
														INNER JOIN  Pro_Batch as BT on MIS.BtRefNo=BT.BtRefNo
														  inner join Inventory_Item AS ITM ON SL.ItemId = ITM.ItemId
                                                            WHERE        (MIS.IType = 2 ) and MIS.ToppingType in(1,2,3,4) and BT.PartyId=Pro_Batch.PartyId and (convert(date,SL.TransactionDate) >= Convert(date,@FromDate)) and   (Convert(date,SL.TransactionDate) <= Convert(date,@ToDate))  AND (SL.TransactionType = 2) AND (LEFT(ITM.ItemCode, 5) = '01001')
							   
							   ))AS TCCost,

                             ((SELECT        ISNULL(SUM(SL.Amount), 0) AS Expr1
                               FROM            Inventory_StoreLedger AS SL INNER JOIN
                                                         Inventory_MaterialIssue AS MIS ON SL.MaterialIssueId = MIS.MaterialIssueId 
														 INNER JOIN Pro_Batch as BT on MIS.BtRefNo=BT.BtRefNo inner join 
                                                         Inventory_Item AS ITM ON SL.ItemId = ITM.ItemId
                               WHERE        (MIS.IType = 2 )and MIS.ToppingType in(1,2,3,4) and BT.PartyId=Pro_Batch.PartyId and (convert(date,SL.TransactionDate) >= Convert(date,@FromDate)) and   (Convert(date,SL.TransactionDate) <= Convert(date,@ToDate))  AND (SL.TransactionType = 2)  AND (LEFT(ITM.ItemCode, 5) = '01002')
							   )) AS TDCost,
							  0 as  TCost

from Inventory_MaterialIssue
inner join Pro_Batch on Inventory_MaterialIssue.BtRefNo=Pro_Batch.BtRefNo
where Inventory_MaterialIssue.MaterialIssueId in (select MaterialIssueId from Inventory_StoreLedger where convert(date ,Inventory_StoreLedger.TransactionDate)>=Convert(date,@FromDate) and Convert(date,Inventory_StoreLedger.TransactionDate)<=Convert(date,@ToDate)) and Inventory_MaterialIssue.IType=2    and Inventory_MaterialIssue.ToppingType=1 
--where convert(date ,Inventory_MaterialIssue.IssueReceiveDate)>=Convert(date,@FromDate) and Convert(date,Inventory_MaterialIssue.IssueReceiveDate)<=Convert(date,@ToDate) and Inventory_MaterialIssue.IType=2    and Inventory_MaterialIssue.ToppingType=1 
group by Pro_Batch.PartyId

