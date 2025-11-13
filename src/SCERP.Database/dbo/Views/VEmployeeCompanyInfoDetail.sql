	CREATE view [dbo].[VEmployeeCompanyInfoDetail]
	AS
	SELECT     EM.Name AS EmployeeName, 
	EM.EmployeeCardId AS EmployeeCardId, 
	EM.PhotographPath AS PhotographPath,
	DEG.Title AS Designation, 
	COM.Name AS CompanyName, 
	Br.Name AS BranchName, 
	U.Name AS UnitName, 
	EMPCINFO.FromDate AS FromDate, 
	EMPCINFO.ToDate AS ToDate,
	DEPT.Name AS DepartmentName,
	SEC.Name AS SectionName,
	LINE.Name AS LineName,
	employeeType.Id AS EmployeeTypeId,
	employeeType.Title AS EmployeeTypeTitle,
	EM.EmployeeId AS EmployeeId, 
	Br.Id AS BranchId,
	COM.Id AS CompanyId,
	BUD.BranchUnitDepartmentId AS BranchUnitDepartmentId,
	BU.BranchUnitId AS BranchUnitId,
	UD.UnitDepartmentId AS UnitDepartmentId, 
	U.UnitId AS UnitId,
	DEPT.Id AS DepartmentId,
	DEG.Id AS DesignationId,
	DSEC.DepartmentSectionId AS DepartmentSectionId,
	DLine.DepartmentLineId AS DepartmentLineId,
	employeeGrade.Id AS EmployeeGradeId,
	employeeGrade.Name AS EmployeeGradeName,
	EMPCINFO.EmployeeCompanyInfoId AS EmployeeCompanyInfoId,
	EM.[Status],
	EmployeeSalary.GrossSalary 
	FROM Employee AS EM LEFT JOIN
		 (SELECT EmployeeCompanyInfoId, EmployeeId, FromDate, ToDate, DesignationId,DepartmentLineId,DepartmentSectionId, BranchUnitDepartmentId,IsActive, 
		         ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS RowNum
		  FROM EmployeeCompanyInfo AS EMCINFO
	      WHERE CAST(EMCINFO.FromDate AS Date) <= CURRENT_TIMESTAMP AND EMCINFO.IsActive = 1) EMPCINFO ON EM.EmployeeId = EMPCINFO.EmployeeId 
	INNER JOIN EmployeeDesignation AS DEG ON EMPCINFO.DesignationId = DEG.Id 
	LEFT JOIN EmployeeGrade AS employeeGrade ON DEG.GradeId = employeeGrade.Id
    LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
	INNER JOIN BranchUnitDepartment AS BUD ON EMPCINFO.BranchUnitDepartmentId = BUD.BranchUnitDepartmentId 
	INNER JOIN BranchUnit AS BU ON BUD.BranchUnitId = BU.BranchUnitId 
	INNER JOIN UnitDepartment AS UD ON BUD.UnitDepartmentId = UD.UnitDepartmentId 
	INNER JOIN Unit AS U ON BU.UnitId = U.UnitId 
	INNER JOIN Department AS DEPT ON UD.DepartmentId = DEPT.Id 
	INNER JOIN Branch AS Br ON BU.BranchId = Br.Id 
	INNER JOIN Company AS COM ON Br.CompanyId = COM.Id
	LEFT JOIN (SELECT  EmployeeId,BasicSalary, GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSalary     
      FROM EmployeeSalary AS EmployeeSalary
      WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= convert(varchar(10),GETDATE(),120)) OR (convert(varchar(10),GETDATE(),120) IS NULL)) AND EmployeeSalary.IsActive=1) EmployeeSalary
      ON EM.EmployeeId = EmployeeSalary.EmployeeId AND rowNumSalary = 1
	LEFT JOIN DepartmentSection AS DSEC ON EMPCINFO.DepartmentSectionId=DSEC.DepartmentSectionId
	LEFT JOIN Section AS SEC ON DSEC.SectionId=SEC.SectionId
	LEFT JOIN DepartmentLine AS DLine ON EMPCINFO.DepartmentLineId=DLine.DepartmentLineId
	LEFT JOIN Line AS LINE ON DLine.LineId=LINE.LineId
	WHERE EMPCINFO.RowNum = 1 AND EMPCINFO.IsActive=1 AND EM.IsActive=1