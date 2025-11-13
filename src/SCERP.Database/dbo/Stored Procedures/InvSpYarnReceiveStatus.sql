CREATE procedure [dbo].[InvSpYarnReceiveStatus]
@SupplierId bigint,
@FromDate date=NULL,
@ToDate date=NULL,
@CompId varchar(3)
as 
select PoNo, Spl.SupplierCompanyId, MR.RType,MR.LcNo, MR.MRRDate as RDate,
 Spl.CompanyName  as Supplyer,
 (select top(1) Party.Name from PLAN_Program 
inner join Party ON PLAN_Program.PartyId=Party.PartyId
where ProgramRefId=PoNo) as PartyName,
 (CASE
   WHEN B.OrderStyleRefId IS NOT NULL THEN  B.BuyerName 
   ELSE (SELECT BuyerName FROM OM_BUYER WHERE BuyerId=MR.BuyerId)  
   END) as BuyerName,
  (CASE WHEN B.OrderStyleRefId IS NULL THEN (ISNULL((select top(1) RefNo from OM_BuyerOrder where OrderNo=MR.OrderNo),MR.OrderNo)) ELSE  B.RefNo END) as OrderNo,
 (CASE WHEN B.OrderStyleRefId IS NULL THEN MR.StyleNo ELSE  B.StyleName END) as StyleNo,
 MR.RefNo, MR.InvoiceNo ,I.ItemName,I.UnitName,REPLACE(I.SubGroupName,' ','') as SubGroupName,C.ColorName as LotNo,S.SizeName as YarnCount,Br.Name as Brand,SR.ReceivedQty,SR.RejectedQty,SR.ReceivedRate AS Rate,((SR.ReceivedQty-SR.RejectedQty)*SR.ReceivedRate) as Amount,CP.Name as CompanyName,CP.FullAddress ,CP.ImagePath,
 (select top(1) ColorName from OM_Color where ColorRefId=SR.FColorRefId ) as FColorName
 
 from Inventory_MaterialReceiveAgainstPo as MR 
inner join Inventory_MaterialReceiveAgainstPoDetail as SR on MR.MaterialReceiveAgstPoId=SR.MaterialReceiveAgstPoId
inner join Mrc_SupplierCompany as Spl on MR.SupplierId=Spl.SupplierCompanyId
inner join VInvItem as I on SR.ItemId=I.ItemId 
inner join OM_Color as C on SR.ColorRefId=C.ColorRefId and SR.CompId=C.CompId
inner join Inventory_Brand as Br on C.ColorCode=Br.BrandId
inner join OM_Size as S on SR.SizeRefId=S.SizeRefId and SR.CompId=S.CompId
LEFT join VOM_BuyOrdStyle as B on SR.OrderStyleRefId=B.OrderStyleRefId and SR.CompId=B.CompId 
--inner join OM_Buyer as B on MR.BuyerId=B.BuyerId 
inner join Company as CP on MR.CompId=CP.CompanyRefId
where MR.StoreId=1 and  SR.CompId=@CompId and (MR.SupplierId=@SupplierId or @SupplierId='-1')  and (CAST( MR.MRRDate AS DATE) >=@FromDate OR @FromDate IS NULL) and (CAST( MR.MRRDate AS DATE)<=@ToDate OR @ToDate IS NULL)
order by  MR.MRRDate




