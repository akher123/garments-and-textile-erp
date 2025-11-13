CREATE Procedure [dbo].[InvSpReportToppingProductionCost]
                 @FromDate datetime ,
				  @ToDate datetime ,
				 @ToppingType int
AS



select  

VProBatch.BatchNo,
VProBatch.BtRefNo, 
VProBatch.BuyerName,
SUM(Inventory_MaterialIssue.Quantity) as BatchQty,
ISNULL( VProBatch.OrderName,
VProBatch.Gsm) as OrderName ,
VProBatch.StyleName,
VProBatch.BillRate,
VProBatch.ShadePerc,
 VProBatch.BatchDate,
  VProBatch.MachineName,
  VProBatch.PartyName,
  VProBatch.GColorName as ColorName,
   ToppingType =
      CASE Inventory_MaterialIssue.ToppingType
         WHEN 2 THEN 'Topping'
         WHEN 3 THEN 'Redyeing'
         WHEN 4 THEN 'Rewashing'
      END,

((SELECT        ISNULL(SUM(SL.Amount), 0) AS Expr1
                               FROM            Inventory_StoreLedger AS SL INNER JOIN
                                                         Inventory_MaterialIssue AS MIS ON SL.MaterialIssueId = MIS.MaterialIssueId INNER JOIN
                                                         Inventory_Item AS ITM ON SL.ItemId = ITM.ItemId
                                                            WHERE        (MIS.IType = 2 ) and MIS.ToppingType=@ToppingType  and (convert(date,SL.TransactionDate) >= Convert(date,@FromDate)) and   (Convert(date,SL.TransactionDate) <= Convert(date,@ToDate))  AND (SL.TransactionType = 2) AND (MIS.BtRefNo = VProBatch.BtRefNo) AND (LEFT(ITM.ItemCode, 5) = '01001')
							   
							   ))AS TCCost,

                             ((SELECT        ISNULL(SUM(SL.Amount), 0) AS Expr1
                               FROM            Inventory_StoreLedger AS SL INNER JOIN
                                                         Inventory_MaterialIssue AS MIS ON SL.MaterialIssueId = MIS.MaterialIssueId INNER JOIN
                                                         Inventory_Item AS ITM ON SL.ItemId = ITM.ItemId
                               WHERE        (MIS.IType = 2 )and  MIS.ToppingType=@ToppingType and (convert(date,SL.TransactionDate) >= Convert(date,@FromDate)) and   (Convert(date,SL.TransactionDate) <= Convert(date,@ToDate))  AND (SL.TransactionType = 2) AND (MIS.BtRefNo = VProBatch.BtRefNo) AND (LEFT(ITM.ItemCode, 5) = '01002')
							   )) AS TDCost

from Inventory_MaterialIssue
inner join VProBatch on Inventory_MaterialIssue.BtRefNo=VProBatch.BtRefNo
where convert(date ,Inventory_MaterialIssue.IssueReceiveDate)>=Convert(date,@FromDate) and Convert(date,Inventory_MaterialIssue.IssueReceiveDate)<=Convert(date,@ToDate) and Inventory_MaterialIssue.IType=2    and Inventory_MaterialIssue.ToppingType=@ToppingType
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
 Inventory_MaterialIssue.ToppingType

   
