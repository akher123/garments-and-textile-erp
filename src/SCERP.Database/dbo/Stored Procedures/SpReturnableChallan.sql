
CREATE PROCEDURE [dbo].[SpReturnableChallan]
@ReceiveType int,
@FromDate datetime,
@ToDate datetime,
@ChallanType char(1),
@CompId varchar(3)
AS
BEGIN 

SELECT C.ReturnableChallanId,C.ReturnableChallanRefId,C.CompId,C.Messrs,C.[Address],C.RefferancePerson,C.ChallanDate,D.Remarks,
 STUFF((SELECT ',' + ISNULL(Maintenance_ReturnableChallanReceiveMaster.ChallanNo,'--')
            FROM Maintenance_ReturnableChallanReceiveMaster where Maintenance_ReturnableChallanReceiveMaster.ReturnableChallanId=C.ReturnableChallanId  AND CompId=C.CompId
            FOR XML PATH('')) ,1,1,'') AS Designation ,

D.ItemName,D.Unit,C.Department,D.DeliveryQty,D.BatchNo,D.Buyer,D.OrderNo,D.StyleNo,D.Color,D.RollQty, ISNULL((Select Sum(ReceiveQty) from Maintenance_ReturnableChallanReceive Where ReturnableChallanDetailId=D.ReturnableChallanDetailId AND CompId=D.CompId),0) AS ReceiveQty
FROM Maintenance_ReturnableChallan AS C
INNER JOIN Maintenance_ReturnableChallanDetail AS D
ON C.ReturnableChallanId=D.ReturnableChallanId AND C.CompId=D.CompId
WHERE C.ChllanType=@ChallanType and ((CONVERT(varchar(10), C.ChallanDate, 120) )>=@FromDate AND (CONVERT(varchar(10), C.ChallanDate, 120)) <=@ToDate) AND C.CompId=@CompId AND ((D.DeliveryQty-ISNULL((Select Sum(ReceiveQty) from Maintenance_ReturnableChallanReceive Where ReturnableChallanDetailId=D.ReturnableChallanDetailId AND CompId=D.CompId),0))>0 or @ReceiveType=1)
END





--EXEC SpReturnableChallan  @ReceiveType=0, @FromDate='2017-05-04', @ToDate='2017-06-05',@ChallanType='M' , @CompId='001'






