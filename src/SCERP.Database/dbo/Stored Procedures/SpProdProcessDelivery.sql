create procedure [dbo].[SpProdProcessDelivery]

@ProcessDeliveryId bigint,
@CompId varchar(3)
as
select  
PD.RefNo as ChalanSL,
PD.InvDate as ChalanDate,
PD.Remarks,
P.Name as FactoryName,
P.[Address] as FAddress,
P.Email,
P.ContactPersonName,
P.ContactPhone,
P.Phone,
CB.JobNo,
CB.CuttingBatchRefId,
BOST.BuyerName,
BOST.RefNo as OrderNo,
BOST.StyleName,
CLR.ColorName,
SZ.SizeName ,
PDD.Quantity,
CM.ComponentName as TagName,
E.Name as Employee,
C.Name as CompanyName,
C.FullAddress
from PROD_ProcessDelivery as PD
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
inner join Party as P on PD.PartyId=P.PartyId
inner join OM_Size as SZ on PDD.SizeRefId=SZ.SizeRefId and PDD.CompId=SZ.CompId
inner join PROD_CuttingBatch as CB on PDD.CuttingBatchId=CB.CuttingBatchId
inner join OM_Color as CLR on CB.ColorRefId=CLR.ColorRefId and CB.CompId=CLR.CompId
inner join PROD_CuttingTag as CT on PDD.CuttingTagId=CT.CuttingTagId
inner join OM_Component as CM on CT.ComponentRefId=CM.ComponentRefId and CT.CompId=CM.CompId
inner join VOM_BuyOrdStyle as BOST on PD.OrderNo=BOST.OrderNo and PD.BuyerRefId=BOST.BuyerRefId and  PD.OrderStyleRefId=BOST.OrderStyleRefId and PD.OrderStyleRefId=CB.OrderStyleRefId and PD.CompId=BOST.CompId
inner join Employee as E on PD.PreparedBy=E.EmployeeId
inner join Company as C on PD.CompId=C.CompanyRefId
where PD.ProcessDeliveryId=@ProcessDeliveryId and PD.CompId=@CompId
order by CB.JobNo