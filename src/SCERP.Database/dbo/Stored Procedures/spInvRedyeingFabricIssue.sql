create procedure spInvRedyeingFabricIssue
@RedyeingFabricIssueId bigint
as

select P.Name as PartyName,
P.[Address],
P.ContactPersonName,
P.ContactPhone,
P.Email,P.Phone,
FI.RefNo ,
FI.ChallanNo,
FI.ChallanDate,
FI.Remarks ,
FI.DriverName,
FI.DriverPhone,
FI.VehicleType,
B.BuyerName as BuyerName,
ISNULL(B.OrderName, B.Gsm) as OrderName,
B.StyleName,
B.GColorName as ColorName,
B.BatchNo,
B.BtRefNo,
(select top(1) PROD_GroupSubProcess.GroupName from PROD_DyeingSpChallanDetail
inner join PROD_GroupSubProcess on PROD_DyeingSpChallanDetail.SpGroupId=PROD_GroupSubProcess.GroupSubProcessId
where PROD_DyeingSpChallanDetail.BatchId=FID.BatchId and PROD_DyeingSpChallanDetail.BatchDetailId=FID.BatchDetailId ) as SubProcesName,
--ISNULL(B.BillRate,0) as BillRate,
(select top(1) Rate from PROD_BatchDetail where BatchDetailId=FID.BatchDetailId and BatchId=FID.BatchId) as BillRate,
(select Name from Employee where EmployeeId=FI.CreatedBy ) as CreatedByName,
ISNULL((select Name from Employee where EmployeeId=FI.ApprovedBy),'Not Approved') as ApprovedByName,
(
select top(1) GSM from PROD_BatchDetail where BatchDetailId=FID.BatchDetailId) as GSM,
(select ItemName from VwProdBatchDetail where BatchDetailId=FID.BatchDetailId) as ItemName,

FID.ReprocessQty,
(select top(1) MdiaSizeName from VwProdBatchDetail where BatchDetailId=FID.BatchDetailId) as MdiaSizeName,
(select FdiaSizeName from VwProdBatchDetail where BatchDetailId=FID.BatchDetailId) as FinishDia,
FID.FinishQty as FinishWt,
ISNULL((select Rate from PROD_BatchDetail where BatchDetailId=FID.BatchDetailId),0)*ISNULL(FID.ReprocessQty,0) as Amt,
FID.NoRole,
FID.Remarks as [Description]
from  Inventory_RedyeingFabricIssue as FI 
inner join Party as P on FI.PartyId=P.PartyId
inner join Inventory_RedyeingFabricIssueDetail as FID on FI.RedyeingFabricIssueId=FID.RedyeingFabricIssueId
inner join VProBatch as B on FID.BatchId=B.BatchId
where FI.RedyeingFabricIssueId=@RedyeingFabricIssueId