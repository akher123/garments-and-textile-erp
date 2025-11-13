create procedure InvReporDepartmetWiseSpSprStatus
@FromDate datetime,
@ToDate datetime,
@PurchaseTypeId int,
@DepartmentId int
as
SELECT DISTINCT SPR.RequisitionNo, SPRD.ApprovedQuantity AS ReqQty, SPR.RequisitionDate AS RqDate, I.ItemCode, I.ItemName, M.UnitName, ISNULL
                             ((SELECT        SUM(SL.Quantity) AS RQty
                                 FROM            Inventory_ItemStore AS IST INNER JOIN
                                                          Inventory_QualityCertificate AS QC ON IST.ItemStoreId = QC.ItemStoreId INNER JOIN
                                                          Inventory_GoodsReceivingNote AS GRN ON QC.QualityCertificateId = GRN.QualityCertificateId INNER JOIN
                                                          Inventory_StoreLedger AS SL ON GRN.GoodsReceivingNotesId = SL.GoodsReceivingNoteId INNER JOIN
                                                          Mrc_SupplierCompany AS SUP ON IST.SupplierId = SUP.SupplierCompanyId
                                 WHERE        (SL.ItemId = SPRD.ItemId) AND (IST.RequisitionNo = SPR.RequisitionNo) AND (IST.IsActive = 1) AND (SL.IsActive = 1)
                                 GROUP BY SL.ItemId), 0) AS RecivedQty
FROM            Inventory_StorePurchaseRequisition AS SPR INNER JOIN
                         Inventory_StorePurchaseRequisitionDetail AS SPRD ON SPR.StorePurchaseRequisitionId = SPRD.StorePurchaseRequisitionId INNER JOIN
                         Inventory_Item AS I ON SPRD.ItemId = I.ItemId INNER JOIN
                         MeasurementUnit AS M ON I.MeasurementUinitId = M.UnitId INNER JOIN
                         Inventory_PurchaseType AS PT ON SPR.PurchaseTypeId = PT.PurchaseTypeId INNER JOIN
						   Inventory_MaterialRequisition AS MaterialRequisition ON SPR.MaterialRequisitionId = MaterialRequisition.MaterialRequisitionId INNER JOIN
                         BranchUnitDepartment AS BRANCHUNITDEPT ON BRANCHUNITDEPT.BranchUnitDepartmentId = MaterialRequisition.BranchUnitDepartmentId INNER JOIN
                         BranchUnit AS BRANCHUNIT ON BRANCHUNITDEPT.BranchUnitId = BRANCHUNIT.BranchUnitId LEFT OUTER JOIN
                         UnitDepartment AS UD ON BRANCHUNITDEPT.UnitDepartmentId = UD.UnitDepartmentId LEFT OUTER JOIN
                         Unit AS UNIT ON BRANCHUNIT.UnitId = UNIT.UnitId LEFT OUTER JOIN
                         Department AS DEPT ON UD.DepartmentId = DEPT.Id LEFT OUTER JOIN
                         DepartmentSection AS DeptSection ON MaterialRequisition.DepartmentSectionId = DeptSection.DepartmentSectionId LEFT OUTER JOIN
                         Section AS Section ON DeptSection.SectionId = Section.SectionId INNER JOIN
                         Branch AS BRANCH ON BRANCHUNIT.BranchId = BRANCH.Id INNER JOIN
                         Company AS COMPANY ON BRANCH.CompanyId = COMPANY.Id
WHERE        SPR.IsActive = 1 AND (PT.PurchaseTypeId = @PurchaseTypeId or  @PurchaseTypeId=-1) AND (SPR.RequisitionDate BETWEEN @FromDate AND @ToDate) and (DEPT.Id = @DepartmentId) 