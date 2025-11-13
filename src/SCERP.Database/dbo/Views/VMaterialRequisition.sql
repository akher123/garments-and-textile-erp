
CREATE view [dbo].[VMaterialRequisition]
AS
SELECT 
EMP1.Name AS SubmittedToEmployeeName,
MATREQ.SubmittedTo,
EMP2.Name AS ModifiedByEmployeeName,
MATREQ.ModifiedBy,
EMP3.Name AS PreparedByEmployeeName,
MATREQ.CreatedBy,
EMP4.Name AS CreatedByEmployeeName,
MATREQ.PreparedBy,
MATREQ.IsModifiedByStore ,
MATREQ.IsSentToStore,
MATREQ.MaterialRequisitionId,
MATREQ.SendingDate,
MATREQ.Remarks,
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
FROM Inventory_MaterialRequisition AS MATREQ
LEFT JOIN Employee AS EMP1 ON MATREQ.SubmittedTo=EMP1.EmployeeId 
LEFT JOIN Employee AS EMP2 ON MATREQ.ModifiedBy=EMP2.EmployeeId 
LEFT JOIN Employee AS EMP3 ON MATREQ.PreparedBy=EMP3.EmployeeId 
LEFT JOIN Employee AS EMP4 ON MATREQ.CreatedBy=EMP4.EmployeeId
INNER JOIN BranchUnitDepartment AS BUD ON MATREQ.BranchUnitDepartmentId=BUD.BranchUnitDepartmentId
INNER JOIN BranchUnit AS BU ON BUD.BranchUnitId=BU.BranchUnitId
INNER JOIN Branch AS BR ON BU.BranchId=BR.Id
INNER JOIN Company AS COMP ON BR.CompanyId=COMP.Id
INNER JOIN Unit AS UNIT ON BU.UnitId=UNIT.UnitId
INNER JOIN UnitDepartment  AS UNITDEPT ON BUD.UnitDepartmentId=UNITDEPT.UnitDepartmentId
INNER JOIN Department AS DEPT ON UNITDEPT.DepartmentId=DEPT.Id
LEFT JOIN DepartmentSection AS DEPTSEC ON MATREQ.DepartmentSectionId=DEPTSEC.DepartmentSectionId
LEFT JOIN Section AS SEC ON DEPTSEC.SectionId=SEC.SectionId
LEFT JOIN DepartmentLine AS DEPTLINE ON MATREQ.DepartmentLineId=DEPTLINE.DepartmentLineId
LEFT JOIN Line AS LINE ON DEPTLINE.LineId=LINE.LineId
where MATREQ.IsActive='true' 
--AND (EMP1.[Status]=1 or EMP1.[Status] is null)
--AND (EMP2.[Status]=1  or EMP2.[Status] is null)
--AND (EMP3.[Status]=1 or EMP3.[Status] is null)
--AND (EMP3.[Status]=1 or EMP4.[Status] is null)




