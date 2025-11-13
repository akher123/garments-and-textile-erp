
CREATE Procedure [dbo].[InvSpReportProductionCost]
                 @FromDate datetime ,
				 @ToDate datetime
AS


select  

VProBatch.BatchNo,
VProBatch.BtRefNo, 
VProBatch.BuyerName,
SUM(VProBatch.BatchQty) as  BatchQty,
ISNULL( VProBatch.OrderName,
VProBatch.Gsm) as OrderName ,
VProBatch.StyleName,
VProBatch.BillRate,
VProBatch.ShadePerc,
 VProBatch.BatchDate,

  VProBatch.MachineName,
  VProBatch.PartyName,
  VProBatch.GColorName as ColorName,

((SELECT        ISNULL(SUM(SL.Amount), 0) AS Expr1
                               FROM            Inventory_StoreLedger AS SL INNER JOIN
                                                         Inventory_MaterialIssue AS MIS ON SL.MaterialIssueId = MIS.MaterialIssueId INNER JOIN
                                                         Inventory_Item AS ITM ON SL.ItemId = ITM.ItemId
                                                            WHERE        (MIS.IType = 2 ) and MIS.ToppingType=1 and (convert(date,SL.TransactionDate) >= Convert(date,@FromDate)) and   (Convert(date,SL.TransactionDate) <= Convert(date,@ToDate))  AND (SL.TransactionType = 2) AND (MIS.BtRefNo = VProBatch.BtRefNo) AND (LEFT(ITM.ItemCode, 5) = '01001')
							   
							   ))AS TCCost,

                             ((SELECT        ISNULL(SUM(SL.Amount), 0) AS Expr1
                               FROM            Inventory_StoreLedger AS SL INNER JOIN
                                                         Inventory_MaterialIssue AS MIS ON SL.MaterialIssueId = MIS.MaterialIssueId INNER JOIN
                                                         Inventory_Item AS ITM ON SL.ItemId = ITM.ItemId
                               WHERE        (MIS.IType = 2 )and  MIS.ToppingType=1 and (convert(date,SL.TransactionDate) >= Convert(date,@FromDate)) and   (Convert(date,SL.TransactionDate) <= Convert(date,@ToDate))  AND (SL.TransactionType = 2) AND (MIS.BtRefNo = VProBatch.BtRefNo) AND (LEFT(ITM.ItemCode, 5) = '01002')
							   )) AS TDCost,
							   ( STUFF((SELECT distinct  ',' + bd.ItemName 
										  FROM VwProdBatchDetail bd
										  WHERE  bd.BatchId =VProBatch.BatchId
										  FOR XML PATH('')), 1, 1, '') ) AS [FabricType]

from Inventory_MaterialIssue

inner join VProBatch on Inventory_MaterialIssue.BtRefNo=VProBatch.BtRefNo
where convert(date ,Inventory_MaterialIssue.IssueReceiveDate)>=Convert(date,@FromDate) and Convert(date,Inventory_MaterialIssue.IssueReceiveDate)<=Convert(date,@ToDate) and Inventory_MaterialIssue.IType=2    and Inventory_MaterialIssue.ToppingType=1
group by  VProBatch.BatchNo,
VProBatch.BtRefNo,
VProBatch.BatchQty,
 VProBatch.BuyerName,
 VProBatch.OrderName,
 VProBatch.Gsm,
 VProBatch.StyleName,
 VProBatch.BillRate,
 VProBatch.ShadePerc,
 VProBatch.BatchQty,
  VProBatch.BatchDate,
  VProBatch.MachineName,
  VProBatch.PartyName,
   VProBatch.GColorName,
   VProBatch.BatchId



  