CREATE procedure [dbo].[SpProdReceiveBalanceReport]
@OrderStyleRefId varchar(7),
@ProcessRefId varchar(3),
@CompId varchar(3)
as

truncate table RPTProdReceiveBalance

INSERT INTO RPTProdReceiveBalance
                      (SL, ProcessRefId, CompId, Factory, PartyId, CompanyName, FullAddress, OrderStyleRefId, IsPrint, IsEmbroidery, BuyerName, OrderNo, StyleName, JobNo, CutRefNo, 
                      InvoiceNo, InvDate, Part, ColorName, ColorRefId, Quantity, SendingQty, ChallanNo, ChallanDate, ChalanQty, ReceiveQty, ProcessRejectQty, FabricRejectQty, 
                      TSendQty, TRecQty,TProcessRectQty,TFabRejectQty)
SELECT     row_number() OVER (ORDER BY ColorName, Part, CutRefNo, InvoiceNo, ChallanNo) AS SL, ProcessRefId, CompId, Factory, PartyId, CompanyName, FullAddress, OrderStyleRefId, IsPrint, IsEmbroidery, BuyerName, OrderNo, StyleName, JobNo, CutRefNo, 
                      InvoiceNo, InvDate, Part, ColorName, ColorRefId, Quantity, SendingQty, ChallanNo, ChallanDate, ChalanQty, ReceiveQty, ProcessRejectQty, FabricRejectQty, 
                      0 AS TSendQty, 0 AS TRecQty, 0 AS TProcessRectQty ,0 AS TFabRejectQty
FROM         VwProdReceiveBalance
WHERE     (OrderStyleRefId = @OrderStyleRefId and CompId=@CompId and ProcessRefId=@ProcessRefId)
ORDER BY ColorName, Part, CutRefNo, InvoiceNo, ChallanNo


Update RPTProdReceiveBalance set  TSendQty = SendingQty where SL= ( SELECT top 1 SL
FROM         RPTProdReceiveBalance as A
WHERE     CutRefNo = RPTProdReceiveBalance.CutRefNo AND  ColorName = RPTProdReceiveBalance.ColorName AND Part = RPTProdReceiveBalance.Part )

Update RPTProdReceiveBalance set  TRecQty = isnull((SELECT     SUM(ReceiveQty) AS TSendQty
FROM         RPTProdReceiveBalance as B where CutRefNo = RPTProdReceiveBalance.CutRefNo AND  ColorName = RPTProdReceiveBalance.ColorName AND Part = RPTProdReceiveBalance.Part ),0) where SL= ( SELECT top 1 SL
FROM         RPTProdReceiveBalance as A
WHERE     CutRefNo = RPTProdReceiveBalance.CutRefNo AND  ColorName = RPTProdReceiveBalance.ColorName AND Part = RPTProdReceiveBalance.Part )

Update RPTProdReceiveBalance set  TProcessRectQty = isnull((SELECT     SUM(ProcessRejectQty) AS TProcessRectQty
FROM         RPTProdReceiveBalance as B where CutRefNo = RPTProdReceiveBalance.CutRefNo AND  ColorName = RPTProdReceiveBalance.ColorName AND Part = RPTProdReceiveBalance.Part ),0) where SL= ( SELECT top 1 SL
FROM         RPTProdReceiveBalance as A
WHERE     CutRefNo = RPTProdReceiveBalance.CutRefNo AND  ColorName = RPTProdReceiveBalance.ColorName AND Part = RPTProdReceiveBalance.Part )

Update RPTProdReceiveBalance set  TFabRejectQty = isnull((SELECT     SUM(FabricRejectQty) AS TFabRejectQty
FROM         RPTProdReceiveBalance as B where CutRefNo = RPTProdReceiveBalance.CutRefNo AND  ColorName = RPTProdReceiveBalance.ColorName AND Part = RPTProdReceiveBalance.Part ),0) where SL= ( SELECT top 1 SL
FROM         RPTProdReceiveBalance as A
WHERE     CutRefNo = RPTProdReceiveBalance.CutRefNo AND  ColorName = RPTProdReceiveBalance.ColorName AND Part = RPTProdReceiveBalance.Part )

select * from RPTProdReceiveBalance

--where PartyId='10094' and JobNo='7'


