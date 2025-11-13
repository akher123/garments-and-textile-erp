





CREATE view [dbo].[VMaterialIssueRequisition]
AS 
SELECT 
EMP1.Name AS SubmittedToEmployeeName,
MATISREQ.SubmittedTo,
EMP2.Name AS PreparedByEmployeeName,
MATISREQ.CreatedBy,
MATISREQ.PreparedBy,
MATISREQ.IsModifiedByStore ,
MATISREQ.IsSentToStore,
MATISREQ.MaterialIssueRequisitionId,
MATISREQ.SendingDate,
MATISREQ.Remarks,
MATISREQ.IssueReceiveNoteNo,
MATISREQ.IssueReceiveNoteDate,
MATISREQ.BranchUnitDepartmentId,
MATISREQ.DepartmentSectionId,
MATISREQ.DepartmentLineId,
BR.CompanyId AS CompanyId,
COMP.Name AS CompanyName,
BR.Id AS BranchId,
BU.BranchUnitId,
BR.Name AS BranchName,
UNIT.Name AS UnitName,
DEPT.Name AS DepartmentName,
SEC.Name AS SectionName,
LINE.Name AS LineName
FROM Inventory_MaterialIssueRequisition AS MATISREQ
LEFT JOIN Employee AS EMP1 ON MATISREQ.SubmittedTo=EMP1.EmployeeId 
LEFT JOIN Employee AS EMP2 ON MATISREQ.PreparedBy=EMP2.EmployeeId 
LEFT JOIN Employee AS EMP3 ON MATISREQ.CreatedBy=EMP3.EmployeeId 
INNER JOIN BranchUnitDepartment AS BUD ON MATISREQ.BranchUnitDepartmentId=BUD.BranchUnitDepartmentId
INNER JOIN BranchUnit AS BU ON BUD.BranchUnitId=BU.BranchUnitId
INNER JOIN Branch AS BR ON BU.BranchId=BR.Id
INNER JOIN Company AS COMP ON BR.CompanyId=COMP.Id
INNER JOIN Unit AS UNIT ON BU.UnitId=UNIT.UnitId
INNER JOIN UnitDepartment  AS UNITDEPT ON BUD.UnitDepartmentId=UNITDEPT.UnitDepartmentId
INNER JOIN Department AS DEPT ON UNITDEPT.DepartmentId=DEPT.Id
LEFT JOIN DepartmentSection AS DEPTSEC ON MATISREQ.DepartmentSectionId=DEPTSEC.DepartmentSectionId
LEFT JOIN Section AS SEC ON DEPTSEC.SectionId=SEC.SectionId
LEFT JOIN DepartmentLine AS DEPTLINE ON MATISREQ.DepartmentLineId=DEPTLINE.DepartmentLineId
LEFT JOIN Line AS LINE ON DEPTLINE.LineId=LINE.LineId
where MATISREQ.IsActive=1 
AND (EMP1.[Status]=1 or EMP1.[Status] is null)
AND (EMP2.[Status]=1  or EMP2.[Status] is null)
AND (EMP3.[Status]=1 or EMP3.[Status] is null)



