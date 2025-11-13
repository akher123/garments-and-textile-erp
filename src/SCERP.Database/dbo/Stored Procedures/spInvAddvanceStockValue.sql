CREATE PROCEDURE [dbo].[spInvAddvanceStockValue]
 @FromDate datetime ,
 @ToDate datetime,
 @CompId varchar(3)
AS
BEGIN
	
	SET NOCOUNT ON;

		truncate table        Inventory_RStock

INSERT INTO Inventory_RStock
                      (ItemId, ItemCode, OQty, OAmt, RQty, RAmt, IQty, IAmt,SizeRefId,ColorRefId,BrandId,CompId)
select distinct   SR.ItemId, ItemCode, 0 AS OQty, 0 AS OAmt, 0 AS RQty, 0 AS RAmt, 0 AS IQty, 0 AS IAmt,SR.SizeRefId,SR.ColorRefId,C.ColorCode as BrandId,@CompId from Inventory_MaterialReceiveAgainstPo as MR
inner join Inventory_StockRegister as SR on MR.MaterialReceiveAgstPoId=SR.SourceId
inner join OM_Color as C on SR.ColorRefId=C.ColorRefId and SR.CompId=C.CompId
inner join Inventory_Item as I on SR.ItemId=I.ItemId
inner join Inventory_SubGroup as Sg on I.SubGroupId=Sg.SubGroupId
inner join Inventory_Group as g on Sg.GroupId=g.GroupId
where SR.StoreId=1 and SR.ActionType=3 and g.GroupId=19 and I.ItemType=1

Update Inventory_RStock Set OQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId
WHERE TransactionType = 1 AND (Convert(date,TransactionDate) < Convert(date, @FromDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)

Update Inventory_RStock Set OQty=OQty- isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId
WHERE TransactionType = 2 AND (Convert(date,TransactionDate) <Convert(date, @FromDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)

Update Inventory_RStock Set RQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId
WHERE  TransactionType = 1 AND (Convert(date,TransactionDate) >= Convert(date, @FromDate)) and (TransactionDate <=Convert(date, @ToDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)


Update Inventory_RStock Set IQty= isnull(( SELECT SUM(Quantity) FROM Inventory_StockRegister
inner join OM_Color as CLR on Inventory_StockRegister.ColorRefId=CLR.ColorRefId
WHERE TransactionType = 2 AND (Convert(date,TransactionDate) >= Convert(date, @FromDate))  and (TransactionDate <= Convert(date, @ToDate))  AND (ItemId = Inventory_RStock.ItemId) AND (Inventory_StockRegister.ColorRefId = Inventory_RStock.ColorRefId) AND (Inventory_StockRegister.SizeRefId = Inventory_RStock.SizeRefId) AND (CLR.ColorCode = Inventory_RStock.BrandId)), 0)


select ROUND(SUM(R.OQty),2) as TOQty,
 ROUND(SUM(R.RQty),2) as TRQty,
 ROUND(SUM(R.IQty),2) as IQty,

(ROUND(ISNULL((  select SUM(Inventory_StockRegister.Quantity) from Inventory_StockRegister
inner join Inventory_AdvanceMaterialIssue on Inventory_AdvanceMaterialIssue.AdvanceMaterialIssueId=Inventory_StockRegister.SourceId
 where Inventory_StockRegister.ActionType=6 and Inventory_StockRegister.TransactionType=2 and    (Convert(date,Inventory_StockRegister.TransactionDate )>= Convert(date,@FromDate))  and (Convert(date,Inventory_StockRegister.TransactionDate) <= Convert(date,@ToDate))
),0),2)) as ReturnToPartyQty,

     (ROUND(ISNULL((   select SUM(SR.Quantity) from Inventory_AdvanceMaterialIssue as YI
     inner join Inventory_StockRegister as SR on YI.AdvanceMaterialIssueId=SR.SourceId
     inner join PLAN_Program as P on YI.ProgramRefId=P.ProgramRefId
     where YI.ProcessRefId ='002'  and SR.TransactionType='2' and IType='2' AND (Convert(date,TransactionDate )>= Convert(date,@FromDate))  and (Convert(date,TransactionDate) <= Convert(date,@ToDate))  ),0),2)) as KRcvQty,

	 (ROUND(ISNULL((   select SUM(SR.Quantity) from Inventory_AdvanceMaterialIssue as YI
     inner join Inventory_StockRegister as SR on YI.AdvanceMaterialIssueId=SR.SourceId
     inner join PLAN_Program as P on YI.ProgramRefId=P.ProgramRefId
     where YI.ProcessRefId ='009'  and SR.TransactionType='2' and IType='2' AND (Convert(date,TransactionDate) >=Convert(date, @FromDate))  and (Convert(date,TransactionDate)<= Convert(date,@ToDate))  ),0),2)) as CLRCUFFRcvQty,
	  (ROUND(ISNULL((   select SUM(SR.Quantity) from Inventory_AdvanceMaterialIssue as YI
     inner join Inventory_StockRegister as SR on YI.AdvanceMaterialIssueId=SR.SourceId
     inner join PLAN_Program as P on YI.ProgramRefId=P.ProgramRefId
     where YI.ProcessRefId ='001'  and SR.TransactionType='2' and IType='2' AND (Convert(date,TransactionDate) >=Convert(date, @FromDate))  and (Convert(date,TransactionDate) <= Convert(date,@ToDate))  ),0),2)) as YDRcvQty,

	 (ROUND(ISNULL((select SUM(KR.Quantity) from PROD_KnittingRoll as KR
         inner join PLAN_Program as P on KR.ProgramId=P.ProgramId
         where  (Convert(date,KR.RollDate) >=Convert(date, @FromDate))  and (Convert(date,KR.RollDate) <= Convert(date,@ToDate))),0),2)) as GreyQty,

	 (Round(ISNULL((  select SUM(RID.RollQty) from PROD_KnittingRollIssue as RI
	   inner join PROD_KnittingRollIssueDetail as RID on RI.KnittingRollIssueId=RID.KnittingRollIssueId
		where   (Convert(date,RI.IssueDate) >= Convert(date,@FromDate))  and (Convert(date,RI.IssueDate) <= Convert(date,@ToDate))),0),2)) as BatchGreyRcvQty, 
(ROUND(ISNULL((select SUM(BTD.Quantity) from Pro_Batch as BT
inner join PROD_BatchDetail as BTD on BT.BatchId=BTD.BatchId
where BT.PartyId=1 and (Convert(date,BT.BatchDate) >=Convert(date, @FromDate))  and (Convert(date,BT.BatchDate) <=Convert(date, @ToDate))),0),2)) as BatchQty,

(Round(ISNULL((select SUM(FRD.RcvQty) from Inventory_FinishFabStore as FR
 inner join Inventory_FinishFabDetailStore as FRD on FR.FinishFabStoreId=FRD.FinishFabStoreId
 inner join Pro_Batch as B on FRD.BatchId=B.BatchId
 where  B.PartyId=1 and  (Convert(date,FR.InvoiceDate )>= Convert(date,@FromDate))  and (Convert(date,FR.InvoiceDate) <= Convert(date,@ToDate))),0),2))as FinshQty,
(Round(ISNULL(( 
select SUM(FID.FabQty)from Inventory_FinishFabricIssue as FI
inner join Inventory_FinishFabricIssueDetail as FID on FI.FinishFabIssueId=FID.FinishFabricIssueId
inner join Pro_Batch as B on FID.BatchId=B.BatchId
where  B.PartyId=1 and  (Convert(date,FI.ChallanDate) >=Convert(date, @FromDate))  and (Convert(date,FI.ChallanDate) <=Convert(date, @ToDate))),0),2)) as  IssueToCutQty 
,CP.Name AS ComapnyName,CP.FullAddress ,CP.ImagePath 
From Inventory_RStock as R
inner join  Inventory_Item as I ON  R.ItemCode=I.ItemCode  
inner join Inventory_SubGroup as SG on SG.SubGroupId=I.SubGroupId
inner join Inventory_Group as G on G.GroupId=SG.GroupId
inner join Company AS CP ON R.CompId=CP.CompanyRefId
where R.CompId=@CompId and (R.OQty>0  or R.RQty>0 or R.IQty>0) and G.GroupId=19
--and I.ItemType=1 
group by CP.Name,CP.FullAddress,CP.ImagePath

END




