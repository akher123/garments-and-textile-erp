CREATE procedure [dbo].[spMisUserActivity]

as
DECLARE @MonthId int ,@DayId int ,@YearId int;
set @MonthId=MONTH(GETDATE())
set @DayId=Day(GETDATE())
set @YearId=year(GETDATE())
truncate table MIS_UserActivity

/* Merchandising Order */
INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Merchandising' AS ModuleName, 'Order' AS ActivityName, CompId, 1 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          OM_BuyerOrder AS D
                            WHERE      (CompId = '001') AND (YEAR(OrderDate) = @YearId) AND (MONTH(OrderDate) = @MonthId) AND (DAY(OrderDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          OM_BuyerOrder AS M
                            WHERE      (CompId = '001') AND (YEAR(OrderDate) = @YearId) AND (MONTH(OrderDate) =@MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM         OM_BuyerOrder
WHERE     (CompId = '001') AND (YEAR(OrderDate) = @YearId)
GROUP BY CompId

/* Commercial LC */
INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Commercial' AS ModuleName, 'L/C' AS ActivityName, '001' AS CompId, 2 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          COMMLcInfo AS D
                            WHERE      (YEAR(LcDate) = @YearId) AND (MONTH(LcDate) = @MonthId) AND (DAY(LcDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          COMMLcInfo AS M
                            WHERE      (YEAR(LcDate) = @YearId) AND (MONTH(LcDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM         COMMLcInfo
WHERE     (YEAR(LcDate) =@YearId)

/* Commercial BBLC */
INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Commercial' AS ModuleName, 'BBLC' AS ActivityName, '001' AS CompId, 3 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          CommBbLcInfo AS D
                            WHERE      (YEAR(BbLcDate) = @YearId) AND (MONTH(BbLcDate) = @MonthId) AND (DAY(BbLcDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          CommBbLcInfo AS M
                            WHERE      (YEAR(BbLcDate) = @YearId) AND (MONTH(BbLcDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM         CommBbLcInfo
WHERE     (YEAR(BbLcDate) =@YearId)

/* Commercial Export */
INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Commercial' AS ModuleName, 'Invoice' AS ActivityName, '001' AS CompId, 4 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          CommExport AS D
                            WHERE      (YEAR(ExportDate) = @YearId) AND (MONTH(ExportDate) = @MonthId) AND (DAY(ExportDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          CommExport AS M
                            WHERE      (YEAR(ExportDate) = @YearId) AND (MONTH(ExportDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM         CommExport
WHERE     (YEAR(ExportDate) =@YearId)

/* Accounts Voucher */
INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Accounts' AS ModuleName, 'Voucher' AS ActivityName, '001' AS CompId, 5 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          Acc_VoucherMaster AS D
                            WHERE      (YEAR(VoucherDate) = @YearId) AND (MONTH(VoucherDate) = @MonthId) AND (DAY(VoucherDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          Acc_VoucherMaster AS M
                            WHERE      (YEAR(VoucherDate) = @YearId) AND (MONTH(VoucherDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM         Acc_VoucherMaster
WHERE     (YEAR(VoucherDate) =@YearId)

/* Inventory Receive */
INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Store (Inventory)' AS ModuleName, 'Receive' AS ActivityName, '001' AS CompId, 6 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          Inventory_ItemStore AS D
                            WHERE      (YEAR(InvoiceDate) = @YearId) AND (MONTH(InvoiceDate) = @MonthId) AND (DAY(InvoiceDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          Inventory_ItemStore AS M
                            WHERE      (YEAR(InvoiceDate) = @YearId) AND (MONTH(InvoiceDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM         Inventory_ItemStore
WHERE     (YEAR(InvoiceDate) =@YearId)

/* Inventory Issue */
INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Store (Inventory)' AS ModuleName, 'Issue' AS ActivityName, '001' AS CompId, 7 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          Inventory_MaterialIssue AS D
                            WHERE      (YEAR(IssueReceiveDate) = @YearId) AND (MONTH(IssueReceiveDate) = @MonthId) AND (DAY(IssueReceiveDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM          Inventory_MaterialIssue AS M
                            WHERE      (YEAR(IssueReceiveDate) = @YearId) AND (MONTH(IssueReceiveDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM         Inventory_MaterialIssue
WHERE     (YEAR(IssueReceiveDate) =@YearId)


/* Knitting Program */
INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Knitting' AS ModuleName, 'Grey Knitting Program' AS ActivityName, '001' AS CompId, 8 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PLAN_Program AS D
                            WHERE      (YEAR(PrgDate) = @YearId)and ProcessRefId='002' AND (MONTH(PrgDate) = @MonthId) AND (DAY(PrgDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PLAN_Program AS M
                            WHERE      (YEAR(PrgDate) = @YearId )and ProcessRefId='002' AND (MONTH(PrgDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PLAN_Program
WHERE     (YEAR(PrgDate) =@YearId and ProcessRefId='002')


INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Kniting' AS ModuleName, 'Dyed Yarn Program' AS ActivityName, '001' AS CompId, 8 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PLAN_Program AS D
                            WHERE      (YEAR(PrgDate) = @YearId)and ProcessRefId='001' AND (MONTH(PrgDate) = @MonthId) AND (DAY(PrgDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PLAN_Program AS M
                            WHERE      (YEAR(PrgDate) = @YearId )and ProcessRefId='001' AND (MONTH(PrgDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PLAN_Program
WHERE     (YEAR(PrgDate) =@YearId and ProcessRefId='001')


INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Kniting' AS ModuleName, 'Collar & Cuff Program' AS ActivityName, '009' AS CompId, 8 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PLAN_Program AS D
                            WHERE      (YEAR(PrgDate) = @YearId)and ProcessRefId='009' AND (MONTH(PrgDate) = @MonthId) AND (DAY(PrgDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PLAN_Program AS M
                            WHERE      (YEAR(PrgDate) = @YearId )and ProcessRefId='009' AND (MONTH(PrgDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PLAN_Program
WHERE     (YEAR(PrgDate) =@YearId and ProcessRefId='009')


INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Kniting' AS ModuleName, 'ROLL RECEIVED' AS ActivityName, '001' AS CompId, 27 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_KnittingRoll AS D
                            WHERE      (YEAR(RollDate) = @YearId) AND (MONTH(RollDate) = @MonthId) AND (DAY(RollDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_KnittingRoll AS M
                            WHERE      (YEAR(RollDate) = @YearId) AND (MONTH(RollDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PROD_KnittingRoll
WHERE     (YEAR(PROD_KnittingRoll.RollDate) =@YearId)



/* Dyeing Batch */
INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Dyeing' AS ModuleName, 'Batch' AS ActivityName, '001' AS CompId, 9 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           Pro_Batch AS D
                            WHERE      (YEAR(BatchDate) = @YearId) AND (MONTH(BatchDate) = @MonthId) AND (DAY(BatchDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           Pro_Batch AS M
                            WHERE      (YEAR(BatchDate) = @YearId) AND (MONTH(BatchDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          Pro_Batch
WHERE     (YEAR(BatchDate) =@YearId)

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Dyeing' AS ModuleName, 'Dyeing Job Order' AS ActivityName, '001' AS CompId, 17 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_DyeingJobOrder AS D
                            WHERE      (YEAR(JobDate) = @YearId) AND (MONTH(JobDate) = @MonthId) AND (DAY(JobDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_DyeingJobOrder AS M
                            WHERE      (YEAR(JobDate) = @YearId) AND (MONTH(JobDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PROD_DyeingJobOrder
WHERE     (YEAR(PROD_DyeingJobOrder.JobDate) =@YearId)

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Dyeing' AS ModuleName, 'Finish Fab Challan To Store' AS ActivityName, '001' AS CompId, 18 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_DyeingSpChallan AS D
                            WHERE      (YEAR(ChallanDate) = @YearId) AND (MONTH(ChallanDate) = @MonthId) AND (DAY(ChallanDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_DyeingSpChallan AS M
                            WHERE      (YEAR(ChallanDate) = @YearId) AND (MONTH(ChallanDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PROD_DyeingSpChallan
WHERE     (YEAR(PROD_DyeingSpChallan.ChallanDate) =@YearId)

/* Cutting Job */
INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Cutting' AS ModuleName, 'Job Cut' AS ActivityName, '001' AS CompId, 10 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_CuttingBatch AS D
                            WHERE      (YEAR(CuttingDate) = @YearId) AND (MONTH(CuttingDate) = @MonthId) AND (DAY(CuttingDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_CuttingBatch AS M
                            WHERE      (YEAR(CuttingDate) = @YearId) AND (MONTH(CuttingDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PROD_CuttingBatch
WHERE     (YEAR(CuttingDate) =@YearId)

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Cutting' AS ModuleName, 'PRINT SENT' AS ActivityName, '001' AS CompId, 26 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_ProcessDelivery AS D
                            WHERE      (YEAR(InvDate) = @YearId) AND (MONTH(D.InvDate) = @MonthId) AND (DAY(D.InvDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_ProcessDelivery AS M
                            WHERE      (  M.ProcessRefId='005'and  YEAR(M.InvDate) = @YearId) AND (MONTH(M.InvDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PROD_ProcessDelivery
WHERE     (YEAR(PROD_ProcessDelivery.InvDate) =@YearId and PROD_ProcessDelivery.ProcessRefId='005')


INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Cutting' AS ModuleName, 'EMBROIDERY SENT' AS ActivityName, '001' AS CompId, 25 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_ProcessDelivery AS D
                            WHERE      ( D.ProcessRefId='006'and YEAR(InvDate) = @YearId) AND (MONTH(D.InvDate) = @MonthId) AND (DAY(D.InvDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_ProcessDelivery AS M
                            WHERE      (  M.ProcessRefId='006'and  YEAR(M.InvDate) = @YearId) AND (MONTH(M.InvDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PROD_ProcessDelivery
WHERE     (YEAR(PROD_ProcessDelivery.InvDate) =@YearId and PROD_ProcessDelivery.ProcessRefId='006')


INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Cutting' AS ModuleName, 'PRINT RECEIVED' AS ActivityName, '001' AS CompId, 24 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_ProcessReceive AS D
                            WHERE      (YEAR(InvoiceDate) = @YearId) AND (MONTH(D.InvoiceDate) = @MonthId) AND (DAY(D.InvoiceDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_ProcessReceive AS M
                            WHERE      (  M.ProcessRefId='005'and  YEAR(M.InvoiceDate) = @YearId) AND (MONTH(M.InvoiceDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PROD_ProcessReceive
WHERE     (YEAR(PROD_ProcessReceive.InvoiceDate) =@YearId and PROD_ProcessReceive.ProcessRefId='005')


INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Cutting' AS ModuleName, 'EMBROIDERY RECEIVED' AS ActivityName, '001' AS CompId, 23 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_ProcessReceive AS D
                            WHERE      ( D.ProcessRefId='006'and YEAR(D.InvoiceDate) = @YearId) AND (MONTH(D.InvoiceDate) = @MonthId) AND (DAY(D.InvoiceDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_ProcessReceive AS M
                            WHERE      (  M.ProcessRefId='006'and  YEAR(M.InvoiceDate) = @YearId) AND (MONTH(M.InvoiceDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PROD_ProcessReceive
WHERE     (YEAR(PROD_ProcessReceive.InvoiceDate) =@YearId and PROD_ProcessReceive.ProcessRefId='006')





/* IE NPT */
INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'IE' AS ModuleName, 'NPT' AS ActivityName, '001' AS CompId, 13 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            PROD_NonProductiveTime AS D
                            WHERE      (YEAR(EntryDate) = @YearId) AND (MONTH(EntryDate) = @MonthId) AND (DAY(EntryDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            PROD_NonProductiveTime AS M
                            WHERE      (YEAR(EntryDate) = @YearId) AND (MONTH(EntryDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           PROD_NonProductiveTime
WHERE     (YEAR(EntryDate) =@YearId)



INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'IE' AS ModuleName, 'Sewing Output' AS ActivityName, '001' AS CompId, 11 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_SewingOutPutProcess AS D
                            WHERE      (YEAR(OutputDate) = @YearId) AND (MONTH(OutputDate) = @MonthId) AND (DAY(OutputDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM           PROD_SewingOutPutProcess AS M
                            WHERE      (YEAR(OutputDate) = @YearId) AND (MONTH(OutputDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM          PROD_SewingOutPutProcess
WHERE     (YEAR(OutputDate) =@YearId)

/*  FINISHING */


INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'FINISHING' AS ModuleName, 'IRON' AS ActivityName, '001' AS CompId, 27 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            PROD_FinishingProcess AS D
                            WHERE      (YEAR(D.InputDate) = @YearId) AND (MONTH(D.InputDate) = @MonthId) AND (DAY(D.InputDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            PROD_FinishingProcess AS M
                            WHERE      (YEAR(M.InputDate) = @YearId) AND (MONTH(M.InputDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           PROD_FinishingProcess
WHERE     (YEAR(PROD_FinishingProcess.InputDate) =@YearId and FType=1)

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'FINISHING' AS ModuleName, 'POLY' AS ActivityName, '001' AS CompId, 28AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            PROD_FinishingProcess AS D
                            WHERE      (YEAR(D.InputDate) = @YearId) AND (MONTH(D.InputDate) = @MonthId) AND (DAY(D.InputDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            PROD_FinishingProcess AS M
                            WHERE      (YEAR(M.InputDate) = @YearId) AND (MONTH(M.InputDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           PROD_FinishingProcess
WHERE     (YEAR(PROD_FinishingProcess.InputDate) =@YearId and FType=2)


/*  Merchandising */

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Merchandising' AS ModuleName, 'Fabric Work Order' AS ActivityName, '001' AS CompId, 14 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            OM_FabricOrder AS D
                            WHERE      (YEAR(D.OrderDate) = @YearId) AND (MONTH(D.OrderDate) = @MonthId) AND (DAY(D.OrderDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            OM_FabricOrder AS M
                            WHERE      (YEAR(M.OrderDate) = @YearId) AND (MONTH(M.OrderDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           OM_FabricOrder
WHERE     (YEAR(OM_FabricOrder.OrderDate) =@YearId)


INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Merchandising' AS ModuleName, 'Yarn Booking' AS ActivityName, '001' AS CompId, 15 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            CommPurchaseOrder AS D
                            WHERE      (  D.PType='Y' and   YEAR(D.PurchaseOrderDate) = @YearId) AND (MONTH(D.PurchaseOrderDate) = @MonthId) AND (DAY(D.PurchaseOrderDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            CommPurchaseOrder AS M
                            WHERE      ( M.PType='Y'and   YEAR(M.PurchaseOrderDate) = @YearId) AND (MONTH(M.PurchaseOrderDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           CommPurchaseOrder
WHERE     (YEAR(CommPurchaseOrder.PurchaseOrderDate) =@YearId and CommPurchaseOrder.PType='Y')

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Merchandising' AS ModuleName, 'Accessories Work Order' AS ActivityName, '001' AS CompId, 16 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            CommPurchaseOrder AS D
                            WHERE      (  D.PType='A' and   YEAR(D.PurchaseOrderDate) = @YearId) AND (MONTH(D.PurchaseOrderDate) = @MonthId) AND (DAY(D.PurchaseOrderDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            CommPurchaseOrder AS M
                            WHERE      ( M.PType='A'and   YEAR(M.PurchaseOrderDate) = @YearId) AND (MONTH(M.PurchaseOrderDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           CommPurchaseOrder
WHERE     (YEAR(CommPurchaseOrder.PurchaseOrderDate) =@YearId and CommPurchaseOrder.PType='A')

/*  Store */

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Store (Inventory)' AS ModuleName, 'Received Register' AS ActivityName, '001' AS CompId, 19 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_MaterialReceived AS D
                            WHERE      (    YEAR(D.ReceivedDate) = @YearId) AND (MONTH(D.ReceivedDate) = @MonthId) AND (DAY(D.ReceivedDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_MaterialReceived AS M
                            WHERE      (   YEAR(M.ReceivedDate) = @YearId) AND (MONTH(M.ReceivedDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           Inventory_MaterialReceived
WHERE     (YEAR(Inventory_MaterialReceived.ReceivedDate) =@YearId )



INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Store (Inventory)' AS ModuleName, 'Returnable Gate pass' AS ActivityName, '001' AS CompId, 20AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Maintenance_ReturnableChallan AS D
                            WHERE      (    YEAR(D.ChallanDate) = @YearId) AND (MONTH(D.ChallanDate) = @MonthId) AND (DAY(D.ChallanDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Maintenance_ReturnableChallan AS M
                            WHERE      (   YEAR(M.ChallanDate) = @YearId) AND (MONTH(M.ChallanDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           Maintenance_ReturnableChallan
WHERE     (YEAR(Maintenance_ReturnableChallan.ChallanDate) =@YearId )


INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Store (Inventory)' AS ModuleName, 'General Gate pass' AS ActivityName, '001' AS CompId, 21 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            GatePass AS D
                            WHERE      (    YEAR(D.ChallanDate) = @YearId) AND (MONTH(D.ChallanDate) = @MonthId) AND (DAY(D.ChallanDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            GatePass AS M
                            WHERE      (   YEAR(M.ChallanDate) = @YearId) AND (MONTH(M.ChallanDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           GatePass
WHERE     (YEAR(GatePass.ChallanDate) =@YearId )

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Store (Inventory)' AS ModuleName, 'Yarn Received' AS ActivityName, '001' AS CompId, 21 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_MaterialReceiveAgainstPo AS D
                            WHERE      (    YEAR(D.GrnDate) = @YearId) AND (MONTH(D.GrnDate) = @MonthId) AND (DAY(D.GrnDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_MaterialReceiveAgainstPo AS M
                            WHERE      (   YEAR(M.GrnDate) = @YearId) AND (MONTH(M.GrnDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           Inventory_MaterialReceiveAgainstPo
WHERE     (YEAR(Inventory_MaterialReceiveAgainstPo.GrnDate) =@YearId )


INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Store (Inventory)' AS ModuleName, 'Yarn Delivery' AS ActivityName, '001' AS CompId, 22 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_AdvanceMaterialIssue AS D
                            WHERE      (    YEAR(D.IRNoteDate) = @YearId) AND (MONTH(D.IRNoteDate) = @MonthId) AND (DAY(D.IRNoteDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_AdvanceMaterialIssue AS M
                            WHERE      (   YEAR(M.IRNoteDate) = @YearId) AND (MONTH(M.IRNoteDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           Inventory_AdvanceMaterialIssue
WHERE     (YEAR(Inventory_AdvanceMaterialIssue.IRNoteDate) =@YearId )

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Store (Inventory)' AS ModuleName, 'Shipment' AS ActivityName, '001' AS CompId, 12 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_StyleShipment AS D
                            WHERE      (YEAR(D.InvoiceDate) = @YearId) AND (MONTH(D.InvoiceDate) = @MonthId) AND (DAY(D.InvoiceDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_StyleShipment AS M
                            WHERE      (YEAR(M.InvoiceDate) = @YearId) AND (MONTH(M.InvoiceDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           Inventory_StyleShipment
WHERE     (YEAR(Inventory_StyleShipment.InvoiceDate) =@YearId)

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Store (Inventory)' AS ModuleName, 'Finish Fab Delivery' AS ActivityName, '001' AS CompId, 12 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_FinishFabricIssue AS D
                            WHERE      (YEAR(ChallanDate) = @YearId) AND (MONTH(ChallanDate) = @MonthId) AND (DAY(ChallanDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_FinishFabricIssue AS M
                            WHERE      (YEAR(ChallanDate) = @YearId) AND (MONTH(ChallanDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           Inventory_FinishFabricIssue
WHERE     (YEAR(Inventory_FinishFabricIssue.ChallanDate) =@YearId)

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'Store (Inventory)' AS ModuleName, 'BILL' AS ActivityName, '001' AS CompId, 12 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_FinishFabricIssue AS D
                            WHERE      (YEAR(ChallanDate) = @YearId) AND (MONTH(ChallanDate) = @MonthId) AND (DAY(ChallanDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            Inventory_FinishFabricIssue AS M
                            WHERE      (YEAR(ChallanDate) = @YearId) AND (MONTH(ChallanDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           Inventory_FinishFabricIssue
WHERE     (YEAR(Inventory_FinishFabricIssue.ChallanDate) =@YearId)

/*  HRM */

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'HRM' AS ModuleName, 'OSD' AS ActivityName, '001' AS CompId, 12 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            OutStationDuty AS D
                            WHERE      (YEAR(DutyDate) = @YearId) AND (MONTH(DutyDate) = @MonthId) AND (DAY(DutyDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            OutStationDuty AS M
                            WHERE      (YEAR(DutyDate) = @YearId) AND (MONTH(DutyDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           OutStationDuty
WHERE     (YEAR(OutStationDuty.DutyDate) =@YearId)


INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'HRM' AS ModuleName, 'Employee Leave' AS ActivityName, '001' AS CompId, 12 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            EmployeeLeave AS D
                            WHERE      (YEAR(SubmitDate) = @YearId) AND (MONTH(SubmitDate) = @MonthId) AND (DAY(SubmitDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            EmployeeLeave AS M
                            WHERE      (YEAR(SubmitDate) = @YearId) AND (MONTH(SubmitDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           EmployeeLeave
WHERE     (YEAR(EmployeeLeave.SubmitDate) =@YearId)

INSERT INTO MIS_UserActivity
                      (ModuleName, ActivityName, CompId, SlNo, ToDayData, MonthlyData, YearlyData)
SELECT     'HRM' AS ModuleName, 'Penalty' AS ActivityName, '001' AS CompId, 12 AS SLNo,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            HrmPenalty AS D
                            WHERE      (YEAR(D.PenaltyDate) = @YearId) AND (MONTH(D.PenaltyDate) = @MonthId) AND (DAY(D.PenaltyDate) = @DayId)) AS ToDayData,
                          (SELECT     COUNT(*) AS YearlyData
                            FROM            HrmPenalty AS M
                            WHERE      (YEAR(M.PenaltyDate) = @YearId) AND (MONTH(M.PenaltyDate) = @MonthId)) AS MonthlyData, COUNT(*) AS YearlyData
FROM           HrmPenalty
WHERE     (YEAR(HrmPenalty.PenaltyDate) =@YearId)



select * from MIS_UserActivity















