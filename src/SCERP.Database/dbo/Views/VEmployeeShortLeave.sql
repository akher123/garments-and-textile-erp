


CREATE view [dbo].[VEmployeeShortLeave]
AS
	SELECT    ESD.Id,
	 ESD.EmployeeId,
	 ESD.ReasonType,
	 ESD.ReasonDescription,
	 ESD.[Date],
	 ESD.FromTime,
	 ESD.ToTime,
	 ESD.TotalHours,
	 VEMCD.EmployeeGradeName,
	 VEMCD.Designation,
	 VEMCD.EmployeeCardId,
	 VEMCD.EmployeeName,
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
	 FROM EmployeeShortLeave AS ESD 
	 INNER JOIN [dbo].[VEmployeeCompanyInfoDetail] AS VEMCD ON ESD.EmployeeId=VEMCD.EmployeeId
	 WHERE ESD.IsActive='true'


