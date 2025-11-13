
CREATE VIEW [dbo].[VwReturnableChallan]
AS
SELECT C.ReturnableChallanId,C.ReturnableChallanRefId,C.CompId,C.Messrs,C.[Address],C.ChllanType,D.BatchNo,D.RollQty, C.RefferancePerson,C.Designation,C.ChallanDate,D.Buyer,D.StyleNo,D.OrderNo,D.Color, D.Remarks,C.EmployeeCardId ,C.Phone,
D.ItemName,D.Unit,D.DeliveryQty,D.ReceiveQty,(Select E.Name from Employee AS E Where E.EmployeeId=C.PreparedBy) AS PreparedBy,
(Select E.Name from Employee AS E Where E.EmployeeId=C.ApprovedBy) AS ApprovedBy
FROM Maintenance_ReturnableChallan AS C
INNER JOIN Maintenance_ReturnableChallanDetail AS D
ON C.ReturnableChallanId=D.ReturnableChallanId AND C.CompId=D.CompId
