

CREATE VIEW [dbo].[VwReceiveDetail]
AS
SELECT R.*,D.ReturnableChallanId,D.ItemName FROM Maintenance_ReturnableChallanReceive AS R
INNER JOIN Maintenance_ReturnableChallanDetail D
ON R.ReturnableChallanDetailId=D.ReturnableChallanDetailId AND R.CompId=D.CompId

