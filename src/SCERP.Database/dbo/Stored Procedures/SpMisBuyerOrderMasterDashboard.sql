-- ==============================================================================================================================================
-- Author:		<Fazlay Rabby>
-- Create date: <11/02/2017>
-- Description:	<> exec [SpMisBuyerOrderMasterDashboard] '012', '', ''
-- ==============================================================================================================================================
CREATE procedure [dbo].[SpMisBuyerOrderMasterDashboard]
@BuyerRefId varchar(7)=null
,@OrderNo varchar(12)
,@OrderStyleRefId varchar(7)

as 



SELECT     BS.OrderStyleRefId as Ref,
 O.RefNo AS [Order No],
  CONVERT(varchar(10), O.RefDate, 103) AS [Order Date],
   S.StyleName AS [Style No], 
   I.ItemName AS Item,
    BS.Quantity AS [Order Qty],
	 ISNULL(BS.Rate, 0)  AS FOB, 
	 ISNULL(BS.Amt, 0) AS [Total Value], 
					  
					  
					 --round(isnull(( SELECT SUM(CostRate) / 12 AS FabCost FROM OM_CostOrdStyle  WHERE CompId = BS.CompId 

					  round(isnull((select  SUM(CO.CostRate) / 12 AS FabCost from OM_CostOrdStyle as CO
					  inner join OM_CostDefination as CD on CO.CostRefId=CD.CostRefId
					  where CD.CostGroup='FAB' and CO.OrderStyleRefId=BS.OrderStyleRefId),0)* BS.Quantity,3) AS [Fab Cost],

					   (select SUM(STP.PayAount) as PayAount from Acc_StylePayment as STP
inner join OM_CostGroup as CT on STP.CostGroup='FAB'
where OrderStyleRefId=BS.OrderStyleRefId) AS [Payment Current Status],
					 
					--  round(isnull(( SELECT SUM(CostRate) / 12 AS FabCost FROM OM_CostOrdStyle WHERE CompId = BS.CompId AND OrderStyleRefId = BS.OrderStyleRefId AND (CostRefId IN ('08', '11')) ),0)* BS.Quantity,3) as [Print/Embo Cost],
					  round(isnull((select  SUM(CO.CostRate) / 12 AS FabCost from OM_CostOrdStyle as CO
					  inner join OM_CostDefination as CD on CO.CostRefId=CD.CostRefId
					  where CD.CostGroup='EMB' and CO.OrderStyleRefId=BS.OrderStyleRefId),0)* BS.Quantity,3) as [Print/Embo Cost],

					   (select SUM(STP.PayAount) as PayAount from Acc_StylePayment as STP
inner join OM_CostGroup as CT on STP.CostGroup='EMB'
where OrderStyleRefId=BS.OrderStyleRefId)  AS [Payment Current Status], 
					  --round(isnull(( SELECT SUM(CostRate) / 12 AS FabCost FROM OM_CostOrdStyle WHERE CompId = BS.CompId AND OrderStyleRefId = BS.OrderStyleRefId AND  (CostRefId IN ('04', '05')) ),0)* BS.Quantity,3) AS [Acc Cost], 
					    
						 round(isnull((select  SUM(CO.CostRate) / 12 AS FabCost from OM_CostOrdStyle as CO
					  inner join OM_CostDefination as CD on CO.CostRefId=CD.CostRefId
					  where CD.CostGroup='ACC' and CO.OrderStyleRefId=BS.OrderStyleRefId),0)* BS.Quantity,3) as [Acc Cost],


						 (select SUM(STP.PayAount) as PayAount from Acc_StylePayment as STP
inner join OM_CostGroup as CT on STP.CostGroup='ACC'
where OrderStyleRefId=BS.OrderStyleRefId)  AS [Payment Current Status], 
					 
					 -- round(isnull(( SELECT SUM(CostRate) / 12 AS FabCost FROM OM_CostOrdStyle WHERE CompId =BS.CompId AND OrderStyleRefId = BS.OrderStyleRefId AND (CostRefId IN ('09')) ),0)* BS.Quantity,3) AS CM, 
                    	
						 round(isnull((select  SUM(CO.CostRate) / 12 AS FabCost from OM_CostOrdStyle as CO
					  inner join OM_CostDefination as CD on CO.CostRefId=CD.CostRefId
					  where CD.CostGroup='CM' and CO.OrderStyleRefId=BS.OrderStyleRefId),0)* BS.Quantity,3) as CM,


					(select Convert(int,round(SUM(TQty),0))  from VCompConsumptionDetail
where OrderStyleRefId=BS.OrderStyleRefId) as FabQty,
(select Convert(int,round(SUM(GreyQty),0))   from VCompConsumptionDetail
where OrderStyleRefId=BS.OrderStyleRefId) as YarnQty,
(select sum(R.Quantity)  from PROD_KnittingRoll as R 
inner join PLAN_Program as P  on R.ProgramId=P.ProgramId
where  P.OrderStyleRefId=BS.OrderStyleRefId and P.ProcessRefId in ('002','009')) as KnittingQty,
(select sum(BatchQty) from Pro_Batch
where OrderStyleRefId=BS.OrderStyleRefId) as DyeingQty,
0 as FinishFabQty,
					  (select ISNULL((select SUM(Quantity) from PROD_CuttingBatch as CB 
inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId
where CB.ComponentRefId='001' and CB.OrderStyleRefId=BS.OrderStyleRefId and CB.CompId=CB.CompId),0)-(ISNULL((select SUM(RJ.RejectQty)from PROD_RejectAdjustment as RJ 
inner join PROD_CuttingBatch as CTB on RJ.CuttingBatchId=CTB.CuttingBatchId
where CTB.OrderStyleRefId=BS.OrderStyleRefId) ,0))) AS [Cutting Qty.], 

(select  ISNULL((select SUM(SOPD.Quantity) from PROD_SewingOutPutProcess as SOP 
inner join PROD_SewingOutPutProcessDetail as SOPD on SOP.SewingOutPutProcessId=SOPD.SewingOutPutProcessId
where SOP.OrderStyleRefId=BS.OrderStyleRefId and SOP.CompId=BS.CompId),0) as SewingQty) AS [Sewing Qty.],
(
select ISNULL((select SUM(PFD.InputQuantity) from PROD_FinishingProcess as PF
inner join PROD_FinishingProcessDetail as PFD on PF.FinishingProcessId=PFD.FinishingProcessId
where PF.FType=2 and PF.OrderStyleRefId=BS.OrderStyleRefId and  PF.CompId=BS.CompId),0)) AS [Finishing Qty], CONVERT(varchar(10), BS.EFD, 103)  AS [Ship Date], CONVERT(varchar(10), (select MAX(ShipDate) from OM_BuyOrdShip
where OrderStyleRefId=BS.OrderStyleRefId), 103) AS ETD, BS.despatchQty AS [Ship Qty], ISNULL(BS.despatchQty * BS.Rate, 0) 
                      AS [Ship Value], BS.Quantity - BS.despatchQty AS [BL Qty], ISNULL(BS.Amt, 0) - ISNULL(BS.despatchQty * BS.Rate, 0) AS [BL Value]

FROM         OM_BuyOrdStyle AS BS INNER JOIN
                      OM_BuyerOrder AS O ON BS.CompId = O.CompId AND BS.OrderNo = O.OrderNo INNER JOIN
                      OM_Style AS S ON BS.CompId = S.CompID AND BS.StyleRefId = S.StylerefId INNER JOIN
                      Inventory_Item AS I ON S.ItemId = I.ItemId AND S.CompID = I.CompId
WHERE     (O.BuyerRefId = @BuyerRefId)
          AND ((BS.OrderNo = @OrderNo) OR (@OrderNo=''))
		  AND ((BS.OrderStyleRefId = @OrderStyleRefId) OR (@OrderStyleRefId=''))
		  AND BS.ActiveStatus=1
ORDER BY [Ship Date] DESC


