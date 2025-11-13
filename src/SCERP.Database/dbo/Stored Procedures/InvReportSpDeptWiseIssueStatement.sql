CREATE procedure [dbo].[InvReportSpDeptWiseIssueStatement]
@FromDate datetime,
@ToDate datetime,
--@GroupId int,
@UnitId int,
@DepartmentId int,
@SectionId int 
as 

select  COMPANY.FullAddress as AddressName,
 COMPANY.Name as Compabny,
 MI.IssueReceiveNo,
 MIR.IssueReceiveNoteNo,
 D.Name as DepartmentName,
(select Section.Name from DepartmentSection 
inner join Section on DepartmentSection.SectionId=Section.SectionId
where DepartmentSection.BranchUnitDepartmentId=MIR.BranchUnitDepartmentId and DepartmentSectionId=MIR.DepartmentSectionId) as SectionName,
U.Name as DptUnit,
 ((select Line.Name from DepartmentLine 
inner join Line on DepartmentLine.LineId=Line.LineId
where DepartmentLine.BranchUnitDepartmentId=MIR.BranchUnitDepartmentId  and DepartmentLineId=MIR.DepartmentLineId))as Line,
 I.ItemCode,
 I.ItemName,
 I.UnitName,
 I.GroupId,
 I.GroupName,
 I.SubGroupName,
 SL.TransactionDate,
 SL.Quantity as Qty
 from Inventory_StoreLedger as SL 
inner join Inventory_MaterialIssue as MI on SL.MaterialIssueId=MI.MaterialIssueId
inner join Inventory_MaterialIssueRequisition as MIR on MI.MaterialIssueRequisitionId=MIR.MaterialIssueRequisitionId
inner join VInvItem as I on SL.ItemId=I.ItemId
inner join BranchUnitDepartment as BUD on MIR.BranchUnitDepartmentId=BUD.BranchUnitDepartmentId
inner join BranchUnit as BU on BUD.BranchUnitId=BU.BranchUnitId
inner join UnitDepartment as UD on BUD.UnitDepartmentId=UD.UnitDepartmentId
inner join Unit as U on BU.UnitId=U.UnitId
inner join Department as D on UD.DepartmentId=D.Id

inner join Branch as B on BU.BranchId=B.Id
INNER JOIN Company AS COMPANY ON B.CompanyId = COMPANY.Id

where SL.TransactionType=2 and MI.IType=1 and SL.IsActive=1  AND (SL.TransactionDate BETWEEN @FromDate AND @ToDate) and (BUD.BranchUnitDepartmentId = @DepartmentId) and (MIR.DepartmentSectionId=@SectionId or @SectionId=-1)
order by SL.TransactionDate  , I.ItemCode



--AND I.GroupId in( @GroupId)








