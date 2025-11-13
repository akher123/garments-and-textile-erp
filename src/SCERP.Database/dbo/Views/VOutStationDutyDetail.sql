


CREATE view [dbo].[VOutStationDutyDetail]
AS
	SELECT    OSD.OutStationDutyId,
	OSD.EmployeeId,
	VEMCD.EmployeeCardId,
	VEMCD.EmployeeName,
	 OSD.DutyDate,
	 OSD.Location,
	 OSD.Purpose,VEMCD.Designation,
	 VEMCD.CompanyName,
	 VEMCD.BranchName,
	 VEMCD.UnitName,
	 VEMCD.DepartmentName,
	 VEMCD.SectionName,
	 VEMCD.LineName ,
	 VEMCD.EmployeeTypeTitle,
	 VEMCD.CompanyId,
	 VEMCD.BranchId,
	 VEMCD.BranchUnitId,
	 VEMCD.BranchUnitDepartmentId,
	 VEMCD.DepartmentSectionId,
	 VEMCD.DepartmentLineId,
	 VEMCD.EmployeeTypeId
	 from OutStationDuty as OSD 
	 inner join [dbo].[VEmployeeCompanyInfoDetail] as VEMCD on OSD.EmployeeId=VEMCD.EmployeeId
	 where OSD.IsActive='true'


