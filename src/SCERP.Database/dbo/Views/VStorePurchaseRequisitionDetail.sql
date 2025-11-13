


CREATE view [dbo].[VStorePurchaseRequisitionDetail]
AS

SELECT        SPRDetail.StorePurchaseRequisitionId, SPRDetail.Description, SPRDetail.Quantity, 
                         SPRDetail.PresentRate, SPRDetail.StorePurchaseRequisitionDetailId,
                         SPRDetail.DesiredDate, SPRDetail.FunctionalArea, 
                         SPRDetail.StockInHand, SPRDetail.LastUnitPrice, SPRDetail.EstimatedYearlyRequirement, 
                         SPRDetail.ModifiedRequiredQuantity, SPRDetail.ApprovedQuantity, SPRDetail.ApprovalDate, 
                         SPRDetail.RemarksOfRequisitionApprovalPerson, SPRDetail.RemarksOfPurchaseApprovalPerson, 
                         SPRDetail.Quotation, SPRDetail.ApprovedPurchase, Inventory_Item.ItemName, Inventory_Item.ItemCode, 
                         MeasurementUnit.UnitName AS MeasurementUnit, Inventory_Brand.Name AS BrandName, Inventory_Size.Title AS SizeName, Country.CountryName AS Origin,
					     Inventory_ApprovalStatus.StatusName AS ApprovalStatus, SPRDetail.IsReceived,
						 SPRDetail.SizeId,SPRDetail.BrandId,SPRDetail.OriginId,SPRDetail.ApprovalStatusId,SPRDetail.ItemId
FROM            Inventory_StorePurchaseRequisitionDetail as SPRDetail INNER JOIN
                         Inventory_Item ON SPRDetail.ItemId = Inventory_Item.ItemId INNER JOIN
                         MeasurementUnit ON Inventory_Item.MeasurementUinitId = MeasurementUnit.UnitId INNER JOIN
                         Inventory_ApprovalStatus ON SPRDetail.ApprovalStatusId = Inventory_ApprovalStatus.ApprovalStatusId LEFT OUTER JOIN
                         Inventory_Brand ON SPRDetail.BrandId = Inventory_Brand.BrandId LEFT OUTER JOIN
                         Inventory_Size ON SPRDetail.SizeId = Inventory_Size.SizeId LEFT OUTER JOIN
                         Country ON SPRDetail.OriginId = Country.Id
WHERE          (SPRDetail.IsActive = 1)



