




CREATE view [dbo].[VStorePurchaseRequisition]
AS
SELECT 
SPREQ.StorePurchaseRequisitionId,
SPREQ.MaterialRequisitionId,
SPREQ.ApprovalStatusId,
SPREQ.RequisitionTypeId,
SPREQ.PurchaseTypeId,
SPREQ.RequisitionNo,
SPREQ.RequisitionDate,
SPREQ.ProcessId,
EMP1.Name AS SubmittedToEmployeeName,
SPREQ.SubmittedTo,
EMP2.Name AS ModifiedByEmployeeName,
SPREQ.ModifiedBy,
EMP3.Name AS PreparedByEmployeeName,
SPREQ.PreparedBy,
REQTYPE.Title AS RequsitionTypeTitle,
PURCTYPE.Title AS PurchaseTypeTitle,
APPSTATUS.StatusName as ApprovalStausName,
SPREQ.ApprovalDate,
SPREQ.Remarks,
MATREQ.RequisitionNoteNo,
MATREQ.RequisitionNoteDate,
MATREQ.BranchUnitDepartmentId,
MATREQ.DepartmentSectionId,
MATREQ.DepartmentLineId,
BR.CompanyId AS CompanyId,
COMP.Name AS CompanyName,
BR.Id AS BranchId,
BU.BranchUnitId,
BR.Name AS BranchName,
UNIT.Name AS UnitName,
DEPT.Name AS DepartmentName,
SEC.Name AS SectionName,
LINE.Name AS LineName
FROM Inventory_StorePurchaseRequisition AS SPREQ
INNER JOIN Inventory_MaterialRequisition AS MATREQ ON SPREQ.MaterialRequisitionId=MATREQ.MaterialRequisitionId
INNER JOIN Inventory_RequsitionType AS REQTYPE ON SPREQ.RequisitionTypeId=REQTYPE.RequisitionTypeId
INNER JOIN Inventory_PurchaseType AS PURCTYPE ON SPREQ.PurchaseTypeId=PURCTYPE.PurchaseTypeId
INNER JOIN Inventory_ApprovalStatus AS APPSTATUS ON SPREQ.ApprovalStatusId=APPSTATUS.ApprovalStatusId
LEFT JOIN Employee AS EMP1 ON SPREQ.SubmittedTo=EMP1.EmployeeId 
LEFT JOIN Employee AS EMP2 ON SPREQ.ModifiedBy=EMP2.EmployeeId 
LEFT JOIN Employee AS EMP3 ON SPREQ.PreparedBy=EMP3.EmployeeId 
INNER JOIN BranchUnitDepartment AS BUD ON MATREQ.BranchUnitDepartmentId=BUD.BranchUnitDepartmentId
INNER JOIN BranchUnit AS BU ON BUD.BranchUnitId=BU.BranchUnitId
INNER JOIN Branch AS BR ON BU.BranchId=BR.Id
INNER JOIN Company AS COMP ON BR.CompanyId=COMP.Id
INNER JOIN Unit AS UNIT ON BU.UnitId=UNIT.UnitId
INNER JOIN UnitDepartment  AS UNITDEPT ON BUD.UnitDepartmentId=UNITDEPT.UnitDepartmentId
INNER JOIN Department AS DEPT ON UNITDEPT.DepartmentId=DEPT.Id
left JOIN DepartmentSection AS DEPTSEC ON MATREQ.DepartmentSectionId=DEPTSEC.DepartmentSectionId
left JOIN Section AS SEC ON DEPTSEC.SectionId=SEC.SectionId
left JOIN DepartmentLine AS DEPTLINE ON MATREQ.DepartmentLineId=DEPTLINE.DepartmentLineId
left JOIN Line AS LINE ON DEPTLINE.LineId=LINE.LineId
where SPREQ.IsActive='true' and MATREQ.IsActive='true' and MATREQ.IsSentToStore=1
AND (EMP1.[Status]=1 or EMP1.[Status] is null)
AND (EMP2.[Status]=1  or EMP2.[Status] is null)
AND (EMP3.[Status]=1 or EMP3.[Status] is null)






