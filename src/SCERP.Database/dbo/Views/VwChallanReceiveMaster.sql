
CREATE VIEW [dbo].[VwChallanReceiveMaster]
AS
Select  M.ReturnableChallanReceiveMasterId,M.RetChallanMasterRefId,M.ChallanNo,M.CompId,R.ReturnableChallanReceiveId,R.ReturnableChallanDetailId,R.RejectQty,R.ReceiveQty,R.Amount,
D.ItemName,D.Unit,D.DeliveryQty,D.Remarks,D.Buyer,D.OrderNo,D.StyleNo,D.Color,D.BatchNo,D.RollQty,
(Select ISNULL(SUM(R.ReceiveQty),0) AS ReceiveQty from Maintenance_ReturnableChallanReceive AS R Where CompId=D.CompId AND ReturnableChallanDetailId=D.ReturnableChallanDetailId) AS TotalReceiveQty,
(Select ISNULL(SUM(R.RejectQty),0) AS RejectQty from Maintenance_ReturnableChallanReceive AS R Where CompId=D.CompId AND ReturnableChallanDetailId=D.ReturnableChallanDetailId) AS TotalRejectQty,
(D.DeliveryQty - ((Select ISNULL(SUM(R.ReceiveQty),0) AS ReceiveQty from Maintenance_ReturnableChallanReceive AS R Where CompId=D.CompId AND ReturnableChallanDetailId=D.ReturnableChallanDetailId)-((Select ISNULL(SUM(R.RejectQty),0) AS RejectQty from Maintenance_ReturnableChallanReceive AS R Where CompId=D.CompId AND ReturnableChallanDetailId=D.ReturnableChallanDetailId)))) AS RemainingQty
 from Maintenance_ReturnableChallanReceiveMaster As M 
INNER JOIN Maintenance_ReturnableChallanReceive AS R 
ON M.ReturnableChallanReceiveMasterId=R.ReturnableChallanReceiveMasterId AND M.CompId=R.CompId
INNER JOIN Maintenance_ReturnableChallanDetail AS D
ON D.ReturnableChallanDetailId=R.ReturnableChallanDetailId AND D.CompId=R.CompId
