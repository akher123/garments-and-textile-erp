




CREATE view [dbo].[VwMaterialReceiveAgainstPoDetail]
as
						SELECT        
						MRD.MaterialReceiveAgstPoDetailId,
						MR.MaterialReceiveAgstPoId,
						MRD.ItemId, 
						MRD.ColorRefId,
						MRD.SizeRefId,
						MRD.ReceivedQty,
						MRD.ReceivedRate,
						MRD.RejectedQty, 
						MRD.DiscountQty, 
						MRD.PurchaseOrderDetailId,
				
						CLR.ColorName,
						SZ.SizeName, 
						I.ItemName,
						I.ItemCode,
						U.UnitName, 
						MR.RefNo,
						MR.CompId,
						MR.SupplierId,
						MR.StoreId, 
						MR.MRRNo, 
						MR.MRRDate, 
						MR.VoucherNo,
						MR.InvoiceNo, 
						MR.InvoiceDate, 
						MR.ReceiveRegNo, 
						MR.ReceiveRegDate,
						MR.GateEntryNo,
						MR.GateEntryDate,
						MR.PoNo,
						 MR.RType,
						MR.PoDate,
						 MR.TotalAmount,
						MR.PayRef,
						MR.TotalDiscount,
						MR.NetAmount,
						MR.Remarks, 
						MR.EmployeeId,
						MR.GrnStatus, 
						SPL.CompanyName as Supplier,
						dbo.Company.Name as CompanyName,
						dbo.Company.FullAddress,
						EMP.Name as EmployeeName,
						MR.GrnRemarks,
					    MR.LcNo,
						MRD.FColorRefId,
						FC.ColorName as FColorName,
						BR.Name as Brand,
						SPL.[Address] as SupplierAddress,
						MR.GrnDate,
						MR.QcDate,
						MRD.GSizeRefId,
						MRD.Location,
						((select SUM(Quantity) from CommPurchaseOrderDetail where PurchaseOrderDetailId=MRD.PurchaseOrderDetailId)-(select Sum(ReceivedQty)-SUM(ISNULL(RejectedQty,0)) from Inventory_MaterialReceiveAgainstPoDetail where PurchaseOrderDetailId=MRD.PurchaseOrderDetailId)) as BalanceQty,

						(select Sum(ReceivedQty)-SUM(ISNULL(RejectedQty,0)) from Inventory_MaterialReceiveAgainstPoDetail where PurchaseOrderDetailId=MRD.PurchaseOrderDetailId) as TotalRcvQty,
						(select top(1) SizeName from OM_Size where SizeRefId=MRD.GSizeRefId and CompId=MRD.CompId) as GSizeName
FROM            dbo.Inventory_MaterialReceiveAgainstPoDetail AS MRD INNER JOIN
                         dbo.Inventory_MaterialReceiveAgainstPo AS MR ON MRD.MaterialReceiveAgstPoId = MR.MaterialReceiveAgstPoId AND MRD.CompId = MR.CompId INNER JOIN
                         dbo.Inventory_Item AS I ON MRD.ItemId = I.ItemId left JOIN
						 dbo.MeasurementUnit as U on I.MeasurementUinitId=U.UnitId left JOIN
                         dbo.OM_Color AS CLR ON MRD.ColorRefId = CLR.ColorRefId AND MRD.CompId = CLR.CompId left JOIN
						 Inventory_Brand as BR on CLR.ColorCode=BR.BrandId
                         left join  dbo.OM_Size AS SZ ON MRD.SizeRefId = SZ.SizeRefId AND MRD.CompId = SZ.CompId 
						 left join dbo.OM_Color AS FC ON MRD.FColorRefId = FC.ColorRefId AND MRD.CompId = FC.CompId 
						 left JOIN  dbo.Mrc_SupplierCompany AS SPL ON MR.SupplierId = SPL.SupplierCompanyId 
						 INNER JOIN
                         dbo.Company ON MRD.CompId = dbo.Company.CompanyRefId INNER JOIN
                         dbo.Employee AS EMP ON MR.EmployeeId = EMP.EmployeeId


