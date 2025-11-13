

CREATE view [dbo].[VMaterialIssue]
AS
SELECT 
MATISSUE.MaterialIssueId,
MATISSUE.MaterialIssueRequisitionId,
MATISSUE.BtRefNo,
MATISSUE.ToppingType,
MATISSUE.Quantity,
Bt.BatchNo,
MATISSUE.IType,
pt.Name as PartyName,
EMP1.Name AS StoreEmployeeName,
EMP2.Name AS PreparedByEmployeeName,
MATISSUE.IssueReceiveNo,
MATISSUE.IssueReceiveDate,
MATISSUE.PreparedByStore,
MATISSUEREQ.PreparedBy,
M.MachineId ,
M.Name as MachineName,
MATISSUE.Note,
MATISSUEREQ.IssueReceiveNoteNo,
MATISSUEREQ.IssueReceiveNoteDate,
MATISSUEREQ.BranchUnitDepartmentId,
MATISSUEREQ.DepartmentSectionId,
MATISSUEREQ.DepartmentLineId,
BR.CompanyId AS CompanyId,
COMP.Name AS CompanyName,
BR.Id AS BranchId,
BU.BranchUnitId,
BR.Name AS BranchName,
UNIT.Name AS UnitName,
DEPT.Name AS DepartmentName,
SEC.Name AS SectionName,
LINE.Name AS LineName
FROM Inventory_MaterialIssue AS MATISSUE

INNER JOIN Inventory_MaterialIssueRequisition AS MATISSUEREQ ON MATISSUE.MaterialIssueRequisitionId=MATISSUEREQ.MaterialIssueRequisitionId
LEFT JOIN Employee AS EMP1 ON MATISSUE.PreparedByStore=EMP1.EmployeeId 
LEFT JOIN Employee AS EMP2 ON MATISSUEREQ.PreparedBy=EMP2.EmployeeId 
INNER JOIN BranchUnitDepartment AS BUD ON MATISSUEREQ.BranchUnitDepartmentId=BUD.BranchUnitDepartmentId
INNER JOIN BranchUnit AS BU ON BUD.BranchUnitId=BU.BranchUnitId
INNER JOIN Branch AS BR ON BU.BranchId=BR.Id
INNER JOIN Company AS COMP ON BR.CompanyId=COMP.Id
INNER JOIN Unit AS UNIT ON BU.UnitId=UNIT.UnitId
INNER JOIN UnitDepartment  AS UNITDEPT ON BUD.UnitDepartmentId=UNITDEPT.UnitDepartmentId
INNER JOIN Department AS DEPT ON UNITDEPT.DepartmentId=DEPT.Id
LEFT JOIN DepartmentSection AS DEPTSEC ON MATISSUEREQ.DepartmentSectionId=DEPTSEC.DepartmentSectionId
LEFT JOIN Section AS SEC ON DEPTSEC.SectionId=SEC.SectionId
LEFT JOIN DepartmentLine AS DEPTLINE ON MATISSUEREQ.DepartmentLineId=DEPTLINE.DepartmentLineId
LEFT JOIN Line AS LINE ON DEPTLINE.LineId=LINE.LineId
left join Pro_Batch as Bt on MATISSUE.BtRefNo=Bt.BtRefNo
left join Party as Pt on Bt.PartyId=Pt.PartyId
left join Production_Machine as M on MATISSUE.MachineId=M.MachineId
where MATISSUE.IsActive='true' and MATISSUEREQ.IsActive='true' and MATISSUEREQ.IsSentToStore=1 

--AND (EMP1.[Status]=1 or EMP1.[Status] is null)
--AND (EMP2.[Status]=1 or EMP2.[Status] is null)

