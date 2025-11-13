CREATE procedure [dbo].[InvSPReportBrandWiseRateHistry]
	                    @FromDate datetime,
                        @ToDate datetime ,
					    @BrandId integer 
						as
select 
SL.TransactionDate,
GRN.GRNNumber as RefNo,
IST.InvoiceNo,
IST.RequisitionNo as SPRNO,
ITM.ItemCode,
ITM.ItemName,
ISNULL(S.Title,'---') as Size,
ISNULL(S.SizeId,0) as SizeId,
ISNULL(B.Name,'---') as Brand,
ISNULL(B.BrandId,0)as BrandId,
ISNULL(Org.CountryName,'---') as Origin,
ISNULL(Org.Id,0) as OrignId,
SL.UnitPrice as Rate ,
 'Plummy Fashions Limited' AS CompanyName,
 'North Norshingpur, Fatullah, Narayanganj'as FullAddress
from Inventory_StoreLedger as SL
inner join Inventory_GoodsReceivingNote as GRN on SL.GoodsReceivingNoteId=GRN.GoodsReceivingNotesId
inner join Inventory_QualityCertificate as QC on GRN.QualityCertificateId=QC.QualityCertificateId
inner join Inventory_ItemStore as IST on QC.ItemStoreId=IST.ItemStoreId
inner join Inventory_Item as ITM on SL.ItemId=ITM.ItemId
left join Inventory_Brand as B on SL.BrandId=B.BrandId
Left join Inventory_Size as S on SL.SizeId=S.SizeId
left join Country as Org on SL.OriginId=org.Id
where SL.TransactionType=1 and  (SL.TransactionDate >= @FromDate) and (SL.TransactionDate <=@ToDate) and ( B.BrandId=@BrandId)
order by GRN.GRNNumber ,IST.RequisitionNo,ITM.ItemCode
