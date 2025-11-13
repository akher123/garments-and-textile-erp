CREATE procedure [dbo].[spOrderClosingStatus]
@buyerRefId varchar(4),
@orderNo varchar(12),
@orderStyleRefId varchar(7)
as
truncate table [dbo].[RPTProductionStatus]


INSERT INTO [dbo].[RPTProductionStatus]
           ([OrderStyleRefId],
		    [FinishFabQty],
			[Allowance],
			[PLoss],
			[YarnRequiredQty]
           ,[YBookingQty]
           ,[YRcvQty]
           ,[YDeliveryQty]
           ,[YShortQty]
           ,[GRcvQty]
           ,[GDeliveryQty]
           ,[GShortQty]
          
		   ,[FRcvQty]
           ,[FDeliveryQty]
           ,[FShortQty]
           ,[FAllowedPLoss]
           ,[FActualPLoss]
           ,[CRcvQty]
           ,[CCuttingQty]
           ,[CDeliveryQty]
           ,[CRejectQty]
           ,[CWastageQty]
           ,[EDeliveryQty]
           ,[ERcvQty]
           ,[ERejectQty]
           ,[EMissingQty]
           ,[PRcvQty]
           ,[PRejectQty]
           ,[PMissingQty]
           ,[SRcvQty]
           ,[SDeliveryQty]
           ,[SRejectQty]
           ,[SMissingQty]
           ,[FPRcvQty]
           ,[FPDelivery]
           ,[FIRcvQty]
           ,[FIDeliveryQty]
           ,[ShipStyleQty]
           ,[ShipQty])
		   select OrderStyleRefId, 0 AS [FinishFabQty],0 AS [Allowance],0 AS [PLoss],0 AS [YarnRequiredQty]
		   ,0 as [YBookingQty],0 as [YRcvQty],0 as [YDeliveryQty],0 as [YShortQty],0 as [GRcvQty],
		   0 as [GDeliveryQty],0 as [GShortQty],0 as [FRcvQty],0 as [FDeliveryQty],0 as [FShortQty],0 as [FAllowedPLoss],
		   0 as [FActualPLoss],  0 as [CRcvQty],  0 as [CCuttingQty],  0 as [CDeliveryQty],  0 as [CRejectQty],
		   0 as [CWastageQty],  0 as [EDeliveryQty],  0 as [ERcvQty],  0 as [ERejectQty],  0 as [EMissingQty],
		   0 as [PRcvQty],  0 as [PRejectQty],  0 as [PMissingQty],  0 as [SRcvQty],  0 as [SDeliveryQty],
		   0 as [SRejectQty],  0 as [SMissingQty],  0 as [FPRcvQty],  0 as [FPDelivery],  0 as [FIRcvQty],
		   0 as [FIDeliveryQty],  0 as [ShipStyleQty],  0 as [ShipQty]
		   from OM_BuyOrdStyle as ST
		   inner join OM_BuyerOrder as BO on ST.OrderNo=BO.OrderNo and ST.CompId=BO.CompId
		   where BO.SCont='N' AND (BO.BuyerRefId=@buyerRefId or @buyerRefId='-1')  and (BO.RefNo=@orderNo OR @orderNo='-1') AND (ST.OrderStyleRefId=@orderStyleRefId OR @orderStyleRefId='-1')

          update [RPTProductionStatus]  set [YarnRequiredQty]=ROUND((select 
									ISNULL(SUM((CCD.TQty/(1-CCD.ProcessLoss*0.01))),0.0) from OM_CompConsumptionDetail as CCD
									inner join OM_Consumption as CS on CCD.ConsRefId=CS.ConsRefId and CCD.CompID=CS.CompId
									where CS.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId  and CS.CompId='001'),2)

          update [RPTProductionStatus]  set [FinishFabQty]=ROUND((select 
									SUM(CCD.TQty) from OM_CompConsumptionDetail as CCD
									inner join OM_Consumption as CS on CCD.ConsRefId=CS.ConsRefId and CCD.CompID=CS.CompId
									where CS.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId  and CS.CompId='001'),0)


     --YARN SECTION--
     update [RPTProductionStatus]  set [YBookingQty]=ROUND(ISNULL(( select SUM(WMtr) from VYarnConsumption where OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId  and CompId='001'),0),2)
    
	 update [RPTProductionStatus]  set [YRcvQty]=Round(ISNULL((  select SUM(ReceivedQty-RejectedQty) from Inventory_MaterialReceiveAgainstPoDetail where OrderStyleRefId =[RPTProductionStatus].OrderStyleRefId  and CompId='001'),0),2)

	 update [RPTProductionStatus]  set [YDeliveryQty]=ROUND(ISNULL((   select SUM(SR.Quantity) from Inventory_AdvanceMaterialIssue as YI
     inner join Inventory_StockRegister as SR on YI.AdvanceMaterialIssueId=SR.SourceId
     inner join PLAN_Program as P on YI.ProgramRefId=P.ProgramRefId
     where YI.ProcessRefId='002'  and SR.TransactionType='2' and IType='2' and P.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId),0),2)

	   update [RPTProductionStatus] set [YShortQty]=ROUND([RPTProductionStatus].[YRcvQty]-[RPTProductionStatus].[YDeliveryQty],2)
    --YARN SECTION END--
	 --GREY SECTION START--
	     update [RPTProductionStatus]  set [GRcvQty]=ROUND(ISNULL((select SUM(KR.Quantity) from PROD_KnittingRoll as KR
         inner join PLAN_Program as P on KR.ProgramId=P.ProgramId
         where P.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId ),0),2)

	   update [RPTProductionStatus]  set [GDeliveryQty]=Round(ISNULL((  select SUM(RID.RollQty) from PROD_KnittingRollIssue as RI
	   inner join PROD_KnittingRollIssueDetail as RID on RI.KnittingRollIssueId=RID.KnittingRollIssueId
		where RI.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId),0),2)
		   
       update [RPTProductionStatus] set [GShortQty]=Round(ISNULL([GRcvQty]-[GDeliveryQty],0),0)
	 --GREY SECTION END--
	  --FINISH FAB SECTION START--
	 update [RPTProductionStatus]  set [FRcvQty]=Round(ISNULL((   select SUM(FRD.RcvQty) from Inventory_FinishFabStore as FR
 inner join Inventory_FinishFabDetailStore as FRD on FR.FinishFabStoreId=FRD.FinishFabStoreId
 inner join Pro_Batch as B on FRD.BatchId=B.BatchId
 where B.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId),0),2)

 	 update [RPTProductionStatus]  set [FDeliveryQty]=Round(ISNULL(( 

 select SUM(FID.FabQty)from Inventory_FinishFabricIssue as FI
 inner join Inventory_FinishFabricIssueDetail as FID on FI.FinishFabIssueId=FID.FinishFabricIssueId
  inner join Pro_Batch as B on FID.BatchId=B.BatchId
 where B.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId),0),2)

	  update [RPTProductionStatus] set [FShortQty]=Round([FRcvQty]-[FDeliveryQty],2)	   
  --FINISH FAB SECTION END--
  --CUTTING SECTION--

 	   update [RPTProductionStatus]  set [CRcvQty]=Round(ISNULL((  select SUM(RID.RollQty) from PROD_KnittingRollIssue as RI
	   inner join PROD_KnittingRollIssueDetail as RID on RI.KnittingRollIssueId=RID.KnittingRollIssueId
		where RI.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId),0),2)

        update [RPTProductionStatus]  set [CCuttingQty]=Round(ISNULL((   select SUM(BC.Quantity) from PROD_CuttingBatch as CB
        inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId
        where CB.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and CB.ComponentRefId='001'),0),2)

			update [RPTProductionStatus]  set [CDeliveryQty]=Round(ISNULL((select SUM(SIPD.InputQuantity) from PROD_SewingInputProcess as SIP
                inner join PROD_SewingInputProcessDetail as SIPD on SIP.SewingInputProcessId=SIPD.SewingInputProcessId
                where SIP.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId),0),2)

		         update [RPTProductionStatus]  set [CRejectQty]=Round(ISNULL(( select SUM(RJ.RejectQty) from PROD_CuttingBatch as CB
                 inner join PROD_RejectAdjustment as RJ on CB.CuttingBatchId=RJ.CuttingBatchId
	             where CB.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and CB.ComponentRefId='001'),0),2)

			update [RPTProductionStatus]  set [CWastageQty]=Round(ISNULL((  select SUM(CFR.CuttingWit+CFR.CuttingWit) from PROD_CuttFabReject as CFR
            inner join Pro_Batch as B on CFR.BatchId=B.BatchId
            where B.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId),0),2)
			  --CUTTING SECTION END--

			   --Emboidery Section Start--
		
  update [RPTProductionStatus]  set [EDeliveryQty]=Round(ISNULL(( select SUM(PDD.Quantity) from PROD_ProcessDelivery as PD 
 inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
where PD.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and  PD.ProcessRefId='005'),0),2)

  update [RPTProductionStatus]  set [ERcvQty]=Round(ISNULL(( select SUM(PRD.ReceivedQty) from PROD_CuttingBatch as CB
 inner join PROD_ProcessReceiveDetail as PRD on CB.CuttingBatchId=PRD.CuttingBatchId
 inner join PROD_ProcessReceive as PR on PRD.ProcessReceiveId=PR.ProcessReceiveId
 where CB.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and PR.ProcessRefId='005'
 ),0),2)

   update [RPTProductionStatus]  set [ERejectQty]=Round(ISNULL((  select SUM(PRD.FabricReject+PRD.ProcessReject) from PROD_CuttingBatch as CB
 inner join PROD_ProcessReceiveDetail as PRD on CB.CuttingBatchId=PRD.CuttingBatchId
  inner join PROD_ProcessReceive as PR on PRD.ProcessReceiveId=PR.ProcessReceiveId
 where CB.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and PR.ProcessRefId='005'),0),2)
	
			   
       update [RPTProductionStatus] set [EMissingQty]=Round(ISNULL((  select SUM(PRD.InvocieQty) from PROD_CuttingBatch as CB
 inner join PROD_ProcessReceiveDetail as PRD on CB.CuttingBatchId=PRD.CuttingBatchId
  inner join PROD_ProcessReceive as PR on PRD.ProcessReceiveId=PR.ProcessReceiveId
 where CB.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and PR.ProcessRefId='005')-[ERcvQty],0),0)
    --Emboidery Section End--

				   --Print Section Start--
		
  update [RPTProductionStatus]  set [PDelivery]=Round(ISNULL(( select SUM(PDD.Quantity) from PROD_ProcessDelivery as PD 
 inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
where PD.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and PD.ProcessRefId='006'),0),2)

  update [RPTProductionStatus]  set [PRcvQty]=Round(ISNULL(( select SUM(PRD.ReceivedQty) from PROD_CuttingBatch as CB
 inner join PROD_ProcessReceiveDetail as PRD on CB.CuttingBatchId=PRD.CuttingBatchId
 inner join PROD_ProcessReceive as PR on PRD.ProcessReceiveId=PR.ProcessReceiveId
 where CB.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and PR.ProcessRefId='006'
 ),0),2)

   update [RPTProductionStatus]  set [PRejectQty]=Round(ISNULL((  select SUM(PRD.FabricReject+PRD.ProcessReject) from PROD_CuttingBatch as CB
 inner join PROD_ProcessReceiveDetail as PRD on CB.CuttingBatchId=PRD.CuttingBatchId
  inner join PROD_ProcessReceive as PR on PRD.ProcessReceiveId=PR.ProcessReceiveId
 where CB.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and PR.ProcessRefId='006'),0),2)
	
			   
       update [RPTProductionStatus] set [PMissingQty]=Round(ISNULL((  select SUM(PRD.InvocieQty) from PROD_CuttingBatch as CB
 inner join PROD_ProcessReceiveDetail as PRD on CB.CuttingBatchId=PRD.CuttingBatchId
  inner join PROD_ProcessReceive as PR on PRD.ProcessReceiveId=PR.ProcessReceiveId
 where CB.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and PR.ProcessRefId='006')-[ERcvQty],0),0)
    --Print Section End--
	--Sewing Section Start--

	update [RPTProductionStatus]  set [SRcvQty]=Round(ISNULL((select SUM(SIPD.InputQuantity) from PROD_SewingInputProcess as SIP
                inner join PROD_SewingInputProcessDetail as SIPD on SIP.SewingInputProcessId=SIPD.SewingInputProcessId
                where SIP.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId),0),2)
 update [RPTProductionStatus]  set [SDeliveryQty]=Round(ISNULL((select SUM(SOPD.Quantity) from PROD_SewingOutPutProcess as SOP
                inner join PROD_SewingOutPutProcessDetail as SOPD on SOP.SewingOutPutProcessId=SOPD.SewingOutPutProcessId
                where SOP.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId),0),2)
   --Sewing Section End--


   	--IRON Section Start--
	   
  update [RPTProductionStatus]  set [FIRcvQty]=Round(ISNULL((select SUM(FPD.InputQuantity) from PROD_FinishingProcess as FP
                inner join PROD_FinishingProcessDetail as FPD on FP.FinishingProcessId=FPD.FinishingProcessId
                where FP.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and FP.FType=1),0),2)

  update [RPTProductionStatus]  set [FPRcvQty]=Round(ISNULL((select SUM(FPD.InputQuantity) from PROD_FinishingProcess as FP
                inner join PROD_FinishingProcessDetail as FPD on FP.FinishingProcessId=FPD.FinishingProcessId
                where FP.OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId and FP.FType=2),0),2)

   --IRON Section End--
   --Shipment Section Start--

  update [RPTProductionStatus]  set [ShipQty]=Round(ISNULL((select SUM(ShipmentQty) from Inventory_StyleShipmentDetail where OrderStyleRefId=[RPTProductionStatus].OrderStyleRefId ),0),2)
--Shipment Section Start--

    select ST.BuyerName,ST.StyleName,ST.Quantity,ST.Merchandiser,ST.RefNo as OrderName,
	( select ItemName from VStyle where StylerefId=ST.StyleRefId and CompID=ST.CompId) as ItemName
	,PR.*,ST.Quantity-PR.ShipQty as ShortShipQty,0 as ExtraShipQty, Round(ISNULL(PR.ShipQty*ST.Rate,0),2) as Amount
	from [RPTProductionStatus] as PR
	inner join VOM_BuyOrdStyle as ST on PR.OrderStyleRefId=ST.OrderStyleRefId
	inner join OM_BuyOrdStyle as BOST on ST.OrderStyleRefId=BOST.OrderStyleRefId

 --  exec [spOrderClosingStatus] '2017-10-16'




 