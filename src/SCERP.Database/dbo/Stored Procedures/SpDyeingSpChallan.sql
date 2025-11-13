CREATE procedure [dbo].[SpDyeingSpChallan]
@DyeingSpChallanId bigint
as
select 
SPC.DyeingSpChallanRefId,
SPC.ChallanNo,
SPC.ChallanDate,
SPC.ExpDate,
SPC.Remarks,
b.PartyName as PartyName,
p.Name as Factory,
p.Email,
p.[Address] as PAddress,
P.Phone,
P.ContactPersonName as CpName,
P.ContactPhone as CpPhone,
b.BtRefNo,
b.BatchNo,
b.BuyerName,
ISNULL(b.OrderName,b.Gsm) as OrderName,
b.StyleName,
b.GColorName,
GSP.GroupName,
bd.ItemName ,
bd.FdiaSizeName,
bd.GSM,
bd.ComponentName,
SPCD.GreyWeight as  Quantity,
ISNULL(SPCD.CcuffQty,0) as  CcuffQty,
SPCD.Remarks as RemarksDetail,
cb.Name as CreatedEmployee,
ISNULL(ab.Name,'Not Approved') as ApprovedEmployee,
SPCD.FinishWeight
from PROD_DyeingSpChallan as SPC
inner join Party as P on SPC.ParyId=P.PartyId
inner join PROD_DyeingSpChallanDetail as SPCD on SPC.DyeingSpChallanId=SPCD.DyeingSpChallanId 
inner join PROD_GroupSubProcess as GSP on SPCD.SpGroupId=GSP.GroupSubProcessId
inner join VwProdBatchDetail as bd on SPCD.BatchDetailId=bd.BatchDetailId and SPCD.BatchId=bd.BatchId
inner join VProBatch as b on SPCD.BatchId=b.BatchId
inner join Employee as Cb on SPC.CreatedBy=Cb.EmployeeId
left join Employee as ab on SPC.ApprovedBy=ab.EmployeeId
where SPC.DyeingSpChallanId=@DyeingSpChallanId







