CREATE procedure spLoanReturnChallanReport

@MaterialIssueId int
as

SELECT 
MATISSUE.BtRefNo as LoanRefNo,
SC.CompanyName as Supplyer,
EMP1.Name AS StoreEmployeeName,
MATISSUE.IssueReceiveNo,
MATISSUE.IssueReceiveDate,
MATISSUE.Note as Remarks,
MATISSUEREQ.IssueReceiveNoteNo,
MATISSUEREQ.IssueReceiveNoteDate,
MEASUNIT.UnitName As MeasurementUnitName,
ITEM.ItemName,
MATISSDET.IssuedQuantity,
MATISSDET.Remarks as Note,
'' as Through,
SC.[Address] as SuplierAddress,
'' as  Designation,
'' as PhoneNo,
'' as  VheicalNo
FROM Inventory_MaterialIssue AS MATISSUE
inner join  Inventory_MaterialIssueDetail AS MATISSDET on MATISSUE.MaterialIssueId=MATISSDET.MaterialIssueId
Inner join Inventory_Item AS ITEM ON MATISSDET.ItemId=ITEM.ItemId
LEFT join Production_Machine AS MACHINE ON MACHINE.MachineId=MATISSDET.MachineId
Inner join MeasurementUnit AS MEASUNIT ON ITEM.MeasurementUinitId=MEASUNIT.UnitId
INNER JOIN Inventory_MaterialIssueRequisition AS MATISSUEREQ ON MATISSUE.MaterialIssueRequisitionId=MATISSUEREQ.MaterialIssueRequisitionId
inner JOIN Employee AS EMP1 ON MATISSUE.PreparedByStore=EMP1.EmployeeId 
inner join Mrc_SupplierCompany as SC on MATISSUE.SupplierId=SC.SupplierCompanyId
where MATISSUE.IsActive='true' 
AND (EMP1.[Status]=1 or EMP1.[Status] is null) and MATISSDET.MaterialIssueId=@MaterialIssueId

