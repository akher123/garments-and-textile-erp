

CREATE view [dbo].[VLoanGiven]
AS
SELECT 
 MATISSUE.MaterialIssueId,
 MATISSUE.SupplierId,
MATISSUE.MaterialIssueRequisitionId,
MATISSUE.BtRefNo as LoanRefNo,
MATISSUE.IType,
SC.CompanyName as Supplyer,
EMP1.Name AS StoreEmployeeName,
MATISSUE.IssueReceiveNo,
MATISSUE.IssueReceiveDate,
MATISSUE.Note as Remarks,
MATISSUEREQ.IssueReceiveNoteNo,
MATISSUEREQ.IssueReceiveNoteDate
FROM Inventory_MaterialIssue AS MATISSUE
INNER JOIN Inventory_MaterialIssueRequisition AS MATISSUEREQ ON MATISSUE.MaterialIssueRequisitionId=MATISSUEREQ.MaterialIssueRequisitionId
inner JOIN Employee AS EMP1 ON MATISSUE.PreparedByStore=EMP1.EmployeeId 
inner join Mrc_SupplierCompany as SC on MATISSUE.SupplierId=SC.SupplierCompanyId
where MATISSUE.IsActive='true'  
AND (EMP1.[Status]=1 or EMP1.[Status] is null) and MATISSUE.IType=4



















