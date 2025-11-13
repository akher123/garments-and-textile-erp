CREATE procedure [dbo].[SpProdUpdateCuttBank]
@OrderStyleRefId varchar(7),
@CompId varchar(3)
as

delete PROD_CutBank where OrderStyleRefId=@OrderStyleRefId and CompId=@CompId
INSERT INTO PROD_CutBank (OrderStyleRefId,ColorRefId,SizeRefId,ComponentRefId,OrderQty,CutFQty,BankQty,BalanceQty,CompId,ComponentType)
select distinct OS.OrderStyleRefId,SD.ColorRefId,SD.SizeRefId ,CS.ComponentRefId,SD.QuantityP as OrderQty,0 as CutFQty,0 as BankQty,0 as BalancQty,OS.CompId,'S' as ComponentType 
from OM_BuyOrdShip as OS
inner join OM_BuyOrdShipDetail as SD on OS.OrderShipRefId=SD.OrderShipRefId and OS.CompId=SD.CompId
inner join PROD_CuttingSequence as CS on OS.OrderStyleRefId=CS.OrderStyleRefId and OS.CompId=CS.CompId
where OS.OrderStyleRefId=@OrderStyleRefId and OS.CompId=@CompId
--group by  OS.OrderStyleRefId,SD.ColorRefId,SD.SizeRefId,CS.ComponentRefId,OS.CompId

--update PROD_CutBank set CutFQty= ISNULL((select SUM(Quantity) from PROD_BundleCutting
--where OrderStyleRefId=PROD_CutBank.OrderStyleRefId and ColorRefId=PROD_CutBank.ColorRefId and ComponentRefId=PROD_CutBank.ComponentRefId and SizeRefId=PROD_CutBank.SizeRefId and PROD_CutBank.ComponentType='S' ),0)
update PROD_CutBank set CutFQty= ((ISNULL((select SUM(Quantity) from PROD_BundleCutting
where OrderStyleRefId=PROD_CutBank.OrderStyleRefId and ColorRefId=PROD_CutBank.ColorRefId and ComponentRefId=PROD_CutBank.ComponentRefId and SizeRefId=PROD_CutBank.SizeRefId and PROD_CutBank.ComponentType='S' ),0)
-ISNULL((select SUM(RejectQty) from PROD_RejectAdjustment as RA
inner join PROD_CuttingBatch  as CB on RA.CuttingBatchId=CB.CuttingBatchId
where CB.OrderStyleRefId=PROD_CutBank.OrderStyleRefId and CB.ColorRefId=PROD_CutBank.ColorRefId and CB.ComponentRefId=PROD_CutBank.ComponentRefId and RA.SizeRefId=PROD_CutBank.SizeRefId and  PROD_CutBank.ComponentType='S' ),0)))


INSERT INTO PROD_CutBank
                         ( OrderStyleRefId, ColorRefId, SizeRefId, OrderQty, CutFQty, BankQty, BalanceQty, CompId, ComponentType, ComponentRefId)
SELECT        PROD_CutBank_1.OrderStyleRefId, PROD_CutBank_1.ColorRefId, PROD_CutBank_1.SizeRefId, PROD_CutBank_1.OrderQty, PROD_CutBank_1.CutFQty, PROD_CutBank_1.BankQty, 
                         PROD_CutBank_1.BalanceQty, PROD_CutBank_1.CompId, 'T' AS ComponentType, VwCuttBankTag.PartRefId AS ComponentRefId
FROM            PROD_CutBank AS PROD_CutBank_1 INNER JOIN
                         VwCuttBankTag ON PROD_CutBank_1.OrderStyleRefId = VwCuttBankTag.OrderStyleRefId AND PROD_CutBank_1.ColorRefId = VwCuttBankTag.ColorRefId AND 
                         PROD_CutBank_1.ComponentRefId = VwCuttBankTag.ComponentRefId AND PROD_CutBank_1.CompId = VwCuttBankTag.CompId
WHERE PROD_CutBank_1.CompId = @CompId AND       (PROD_CutBank_1.OrderStyleRefId = @OrderStyleRefId)


DELETE FROM PROD_CutBank
WHERE     CompId=@CompId AND   OrderStyleRefId = @OrderStyleRefId AND ComponentType = 'S' AND ( ColorRefId + ComponentRefId IN
                             (SELECT        ColorRefId + ComponentRefId AS Expr1
                               FROM            VwCuttBankTag
                               WHERE        OrderStyleRefId = @OrderStyleRefId AND CompId = @CompId ))

update PROD_CutBank SET PrintRcvQty=0,PrintRejQty=0, EmbRcvQty=0 ,EmbRejQty=0,FabricRejQty=0 ,SolidQty=0
  WHERE        OrderStyleRefId = @OrderStyleRefId AND CompId = @CompId
--PRINT QTY UPDATE

update PROD_CutBank set CutFQty=ISNULL((
select SUM(PRD.ReceivedQty)-(SUM(PRD.ProcessReject)+SUM(PRD.FabricReject)) from PROD_ProcessReceive as PR 

inner join PROD_ProcessReceiveDetail as PRD on PR.ProcessReceiveId=PRD.ProcessReceiveId
inner join PROD_CuttingTag as CT on PRD.CuttingTagId=CT.CuttingTagId

where PRD.ColorRefId=PROD_CutBank.ColorRefId and  PR.ProcessRefId='005' and  CT.ComponentRefId=PROD_CutBank.ComponentRefId and CT.IsPrint=1 and PRD.SizeRefId=PROD_CutBank.SizeRefId and PROD_CutBank.ComponentType='T'),0)

--PRINT RCV
update PROD_CutBank set PrintRcvQty=ISNULL((
select SUM(PRD.ReceivedQty) from PROD_ProcessReceive as PR 
inner join PROD_ProcessReceiveDetail as PRD on PR.ProcessReceiveId=PRD.ProcessReceiveId
inner join PROD_CuttingTag as CT on PRD.CuttingTagId=CT.CuttingTagId
where PRD.ColorRefId=PROD_CutBank.ColorRefId and PR.ProcessRefId='005' and CT.ComponentRefId=PROD_CutBank.ComponentRefId and CT.IsPrint=1 and PRD.SizeRefId=PROD_CutBank.SizeRefId and PROD_CutBank.ComponentType='T'),0)
--EBROIDERY RCV
update PROD_CutBank set EmbRcvQty=ISNULL((
select SUM(PRD.ReceivedQty) from PROD_ProcessReceive as PR 
inner join PROD_ProcessReceiveDetail as PRD on PR.ProcessReceiveId=PRD.ProcessReceiveId
inner join PROD_CuttingTag as CT on PRD.CuttingTagId=CT.CuttingTagId
where PRD.ColorRefId=PROD_CutBank.ColorRefId and PR.ProcessRefId='006' and CT.ComponentRefId=PROD_CutBank.ComponentRefId and CT.IsEmbroidery=1 and PRD.SizeRefId=PROD_CutBank.SizeRefId and PROD_CutBank.ComponentType='T'),0)

--Print Reject
update PROD_CutBank set PrintRejQty=ISNULL((
select SUM(PRD.ProcessReject) from PROD_ProcessReceive as PR 
inner join PROD_ProcessReceiveDetail as PRD on PR.ProcessReceiveId=PRD.ProcessReceiveId
inner join PROD_CuttingTag as CT on PRD.CuttingTagId=CT.CuttingTagId
where PRD.ColorRefId=PROD_CutBank.ColorRefId and PR.ProcessRefId='005' and CT.ComponentRefId=PROD_CutBank.ComponentRefId and CT.IsPrint=1 and PRD.SizeRefId=PROD_CutBank.SizeRefId and PROD_CutBank.ComponentType='T'),0)

--Embroidery Reject
update PROD_CutBank set EmbRejQty=ISNULL((
select SUM(PRD.ProcessReject) from PROD_ProcessReceive as PR 
inner join PROD_ProcessReceiveDetail as PRD on PR.ProcessReceiveId=PRD.ProcessReceiveId
inner join PROD_CuttingTag as CT on PRD.CuttingTagId=CT.CuttingTagId
where PRD.ColorRefId=PROD_CutBank.ColorRefId and PR.ProcessRefId='006' and CT.ComponentRefId=PROD_CutBank.ComponentRefId and CT.IsEmbroidery=1 and PRD.SizeRefId=PROD_CutBank.SizeRefId and PROD_CutBank.ComponentType='T'),0)

--Fabric Reject
update PROD_CutBank set FabricRejQty=ISNULL((
select SUM(PRD.FabricReject) from PROD_ProcessReceive as PR 
inner join PROD_ProcessReceiveDetail as PRD on PR.ProcessReceiveId=PRD.ProcessReceiveId
inner join PROD_CuttingTag as CT on PRD.CuttingTagId=CT.CuttingTagId
where PRD.ColorRefId=PROD_CutBank.ColorRefId and PR.ProcessRefId='005' and CT.ComponentRefId=PROD_CutBank.ComponentRefId and CT.IsPrint=1 and PRD.SizeRefId=PROD_CutBank.SizeRefId and PROD_CutBank.ComponentType='T'),0)

update PROD_CutBank set FabricRejQty=FabricRejQty+ISNULL((
select SUM(PRD.ProcessReject) from PROD_ProcessReceive as PR 
inner join PROD_ProcessReceiveDetail as PRD on PR.ProcessReceiveId=PRD.ProcessReceiveId
inner join PROD_CuttingTag as CT on PRD.CuttingTagId=CT.CuttingTagId
where PRD.ColorRefId=PROD_CutBank.ColorRefId and PR.ProcessRefId='006'and CT.ComponentRefId=PROD_CutBank.ComponentRefId and CT.IsEmbroidery=1 and PRD.SizeRefId=PROD_CutBank.SizeRefId and PROD_CutBank.ComponentType='T'),0)

----EMBROIDERY  QTY UPDATE


update PROD_CutBank set EmbRcvQty=ISNULL((
select SUM(PRD.ReceivedQty)-(SUM(PRD.ProcessReject)+SUM(PRD.FabricReject)) from PROD_ProcessReceive as PR 
inner join PROD_ProcessReceiveDetail as PRD on PR.ProcessReceiveId=PRD.ProcessReceiveId
inner join PROD_CuttingTag as CT on PRD.CuttingTagId=CT.CuttingTagId
where PRD.ColorRefId=PROD_CutBank.ColorRefId and  PR.ProcessRefId='006' and  CT.ComponentRefId=PROD_CutBank.ComponentRefId and CT.IsEmbroidery=1 and PROD_CutBank.PrintRcvQty=0  and PRD.SizeRefId=PROD_CutBank.SizeRefId and PROD_CutBank.ComponentType='T'),0)

Update PROD_CutBank set BankQty = isnull(( SELECT        MIN(CutFQty-FabricRejQty-PrintRejQty) AS Expr1
FROM            PROD_CutBank as A
WHERE        CompId = PROD_CutBank.CompId AND OrderStyleRefId = PROD_CutBank.OrderStyleRefId  
AND ColorRefId = PROD_CutBank.ColorRefId AND SizeRefId = PROD_CutBank.SizeRefId and ComponentRefId= PROD_CutBank.ComponentRefId),0) where CompId=@CompId and OrderStyleRefId = @OrderStyleRefId

Update PROD_CutBank Set BalanceQty=CutFQty-BankQty where CompId=@CompId and OrderStyleRefId = @OrderStyleRefId









