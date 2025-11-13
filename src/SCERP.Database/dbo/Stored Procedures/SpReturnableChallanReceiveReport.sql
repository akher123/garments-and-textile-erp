
CREATE PROCEDURE [dbo].[SpReturnableChallanReceiveReport]
@ReturnableChallanReceiveMasterId bigint,
@CompId varchar(3)
AS
BEGIN
Select CD.Unit,CD.ItemName,RC.Messrs,RC.ReturnableChallanRefId,RC.[Address],RC.RefferancePerson,Rc.Designation,RC.Department,RC.Phone,RM.CompId,RM.ChallanNo,RM.ReceiveDate,SUM(R.Amount) as TotalAmount,R.ReceiveQty
 from Maintenance_ReturnableChallanReceiveMaster AS RM 
INNER JOIN Maintenance_ReturnableChallanReceive AS R
ON RM.ReturnableChallanReceiveMasterId=R.ReturnableChallanReceiveMasterId AND RM.CompId=R.CompId
INNER JOIN Maintenance_ReturnableChallan AS RC
ON RM.ReturnableChallanId=RC.ReturnableChallanId AND RM.CompId=RC.CompId
INNER JOIN Maintenance_ReturnableChallanDetail AS CD
ON R.ReturnableChallanDetailId=CD.ReturnableChallanDetailId AND RC.CompId=R.CompId
WHERE RM.ReturnableChallanReceiveMasterId=@ReturnableChallanReceiveMasterId AND RM.CompId=@CompId
group by CD.Unit,CD.ItemName,RC.ReturnableChallanRefId,RC.Messrs,RC.[Address],RC.RefferancePerson,Rc.Designation,RC.Department,RC.Phone,RM.CompId,RM.ChallanNo,RM.ReceiveDate,R.ReceiveQty
END



