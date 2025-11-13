
CREATE procedure [dbo].[SpMonthlyShipmentSummary]
@FromDate datetime,
@ToDate dateTime ,
@CompId varchar(3)
as
--select B.BuyerName,LEFT(DATENAME(MONTH,SH.ShipDate),3)+'-'+RIGHT(DATENAME(YEAR,SH.ShipDate),2) as ShipDate,SUM(SHD.ShipmentQty) as ShipmentQty,SUM(SHD.ShipmentQty*ST.Rate) as Amount from Inventory_StyleShipment as SH
--inner join Inventory_StyleShipmentDetail as SHD on SH.StyleShipmentId=SHD.StyleShipmentId
--inner join OM_BuyOrdStyle as ST on SHD.OrderStyleRefId=ST.OrderStyleRefId and SHD.CompId=ST.CompId
--inner join OM_Buyer as B on SH.BuyerRefId=B.BuyerRefId and SH.CompId=B.CompId
--where Convert(Date,SH.InvoiceDate)>=Convert(Date,@FromDate) and Convert(Date,SH.InvoiceDate)<=Convert(Date,@ToDate) and SH.CompId=@CompId
--group by B.BuyerName,SH.ShipDate
--order by SH.ShipDate

--select B.BuyerName,SUM(SHD.ShipmentQty) as ShipmentQty,SUM(SHD.ShipmentQty*ST.Rate) as Amount 
--from Inventory_StyleShipment as SH
--inner join Inventory_StyleShipmentDetail as SHD on SH.StyleShipmentId=SHD.StyleShipmentId
--inner join OM_BuyOrdStyle as ST on SHD.OrderStyleRefId=ST.OrderStyleRefId and SHD.CompId=ST.CompId
--inner join OM_Buyer as B on SH.BuyerRefId=B.BuyerRefId and SH.CompId=B.CompId
--where  Convert(Date,SH.InvoiceDate)>=Convert(Date,@FromDate) and Convert(Date,SH.InvoiceDate)<=Convert(Date,@ToDate) and SH.CompId=@CompId
--group by B.BuyerName 
select VOM_BuyOrdStyle.BuyerName,SUM(Inventory_StyleShipmentDetail.ShipmentQty) as ShipmentQty,SUM(Inventory_StyleShipmentDetail.ShipmentQty*VOM_BuyOrdStyle.Rate) as Amount  from Inventory_StyleShipmentDetail 
inner join Inventory_StyleShipment on Inventory_StyleShipmentDetail.StyleShipmentId=Inventory_StyleShipment.StyleShipmentId
inner join VOM_BuyOrdStyle on Inventory_StyleShipmentDetail.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId
where  Convert(Date,Inventory_StyleShipment.ShipDate)>=Convert(Date,@FromDate) and Convert(Date,Inventory_StyleShipment.ShipDate)<=Convert(Date,@ToDate) and Inventory_StyleShipment.CompId=@CompId

group by VOM_BuyOrdStyle.BuyerName

--exec SpMonthlyShipmentSummary '2016-01-01','2017-01-01','001'




