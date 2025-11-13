CREATE procedure SpProdDyeingJobOrderReort
@DyeingJobOrderId bigint
as

select 
j.JobRefId,
j.JobDate,
j.DeliveryDate,
j.WorkOrderNo,
j.BuyerName,
j.OrderName,
j.StyleName,
(select Name from Party where PartyId=j.PartyId and CompId=j.CompId) as PartyName,

( select top(1) ItemName from Inventory_Item where ItemId=jd.ItemId and CompId=jd.CompId ) as ItemName,
( select top(1) ComponentName from OM_Component where ComponentRefId=jd.ComponentRefId and CompId=Jd.CompId) as ComponentName,
(select top(1) ColorName from OM_Color where ColorRefId=jd.ColorRefId and CompId=jd.CompId) as ColorName,
 (select top(1) SizeName from OM_Size where SizeRefId=jd.FdSizeRefId and CompId=jd.CompId) as FdName,
 (select top(1) SizeName from OM_Size where SizeRefId=jd.MdSizeRefId and CompId=jd.CompId) as MdName,
 jd.Gsm,
 jd.Rate,
 jd.Quantity,
  j.Remarks as RemarksP,
 jd.Remarks,
 '' as PreparedByEmployee,
 '' as ApprovedByEmployee,
 ( select Name from Company where CompanyRefId=j.CompId )as CompanyName,
  ( select FullAddress from Company where CompanyRefId=j.CompId )as FullAddress
 from PROD_DyeingJobOrder as j
 inner join PROD_DyeingJobOrderDetail as jd on j.DyeingJobOrderId=jd.DyeingJobOrderId
 where j.DyeingJobOrderId=@DyeingJobOrderId





 --exec SpProdDyeingJobOrderReort 3