




CREATE VIEW [dbo].[VwReturnableChallanReceive]
AS
SELECT
D.CompId,
D.ReturnableChallanDetailId,
D.ReturnableChallanId,D.ItemName,
D.Unit,
D.DeliveryQty,
D.ReceiveQty,
D.RejectQty,
D.BatchNo,
D.RollQty,
D.Buyer,D.StyleNo,D.OrderNo,D.Color,
(Select ISNULL(SUM(R.ReceiveQty),0) AS ReceiveQty from Maintenance_ReturnableChallanReceive AS R Where CompId=D.CompId AND ReturnableChallanDetailId=D.ReturnableChallanDetailId) AS TotalReceiveQty,
(Select ISNULL(SUM(R.RejectQty),0) AS RejectQty from Maintenance_ReturnableChallanReceive AS R Where CompId=D.CompId AND ReturnableChallanDetailId=D.ReturnableChallanDetailId) AS TotalRejectQty,
(D.DeliveryQty - ((Select ISNULL(SUM(R.ReceiveQty),0) AS ReceiveQty from Maintenance_ReturnableChallanReceive AS R Where CompId=D.CompId AND ReturnableChallanDetailId=D.ReturnableChallanDetailId)-((Select ISNULL(SUM(R.RejectQty),0) AS RejectQty from Maintenance_ReturnableChallanReceive AS R Where CompId=D.CompId AND ReturnableChallanDetailId=D.ReturnableChallanDetailId)))) AS RemainingQty,
(Select ISNULL(SUM(R.Amount),0) AS Amount from Maintenance_ReturnableChallanReceive AS R Where CompId=D.CompId AND ReturnableChallanDetailId=D.ReturnableChallanDetailId) AS Amount
 FROM  Maintenance_ReturnableChallanDetail AS D





