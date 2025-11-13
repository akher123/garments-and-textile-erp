CREATE procedure [dbo].[SpInventoryStyleShipmentChallan]
@StyleShipmentId bigint
as

select SH.StyleShipmentRefId as ChallanRefNo,
 B.BuyerName,
 BO.RefNo as OrderName,
 ST.StyleName,
 RTRIM(LTRIM(I.ItemName)) as ItemName, 
 SH.VehicleNo,
 SH.InvoiceDate,
 SH.InvoiceNo,
 SH.Messrs,
 SH.[Address],
 SH.DepoName,
 SH.DriverCellNo,
 SH.ThroughCellNo,
 SH.DriverLicenceNo,
 SH.LockNo,
 SH.DriverNid,
SH.ShipmentMode,
SH.DriverName,
SH.ShipDate,
SUM(SD.ShipmentQty) as ShipmentQty ,
SH.Remarks,
SH.Through,ISNULL((select Name from Employee where EmployeeId= SH.ApprovedBy),'--')as ApprovedBy ,ISNULL((select Name from Employee where EmployeeId= SH.PrepairedBy),'--')as PrepairedBy from Inventory_StyleShipment as SH
inner join Inventory_StyleShipmentDetail as SD on SH.StyleShipmentId=SD.StyleShipmentId
inner join OM_BuyOrdStyle as BOST on  SH.CompId=BOST.CompId and BOST.OrderStyleRefId=SD.OrderStyleRefId
inner join OM_BuyerOrder as BO on BOST.OrderNo=BO.OrderNo and SH.CompId=BO.CompId
inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Style as ST on BOST.StyleRefId=ST.StylerefId and BOST.CompId=ST.CompID
inner join Inventory_Item as I on ST.ItemId=I.ItemId
where SH.StyleShipmentId=@StyleShipmentId
group by SH.ThroughCellNo,SH.ShipDate, SH.StyleShipmentRefId,SH.Through,SH.ApprovedBy,SH.PrepairedBy , B.BuyerName,BO.RefNo ,SH.DriverName,ST.StyleName, SH.Messrs,SH.Remarks, I.ItemName, SH.VehicleNo,SH.InvoiceNo,SH.[Address],SH.DepoName,SH.DriverCellNo,SH.InvoiceDate ,SH.DriverLicenceNo,SH.LockNo,SH.DriverNid,ShipmentMode





--exec SpInventoryStyleShipmentChallan 1



