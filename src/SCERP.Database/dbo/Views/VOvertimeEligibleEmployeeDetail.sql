
	CREATE VIEW [dbo].[VOvertimeEligibleEmployeeDetail]
	AS
	SELECT    
	EM.Name AS EmployeeName, 
	EM.EmployeeCardId AS EmployeeCardId, 
	OELEMP.OvertimeDate AS OvertimeDate,
	OELEMP.OvertimeHour AS OvertimeHour,
	OELEMP.Remarks AS Remarks,
	empType.Id AS EmployeeTypeId,
	empType.Title AS EmployeeTypeTitle,
	DEG.Title AS Designation, 
	COM.Name AS CompanyName, 
	Br.Name AS BranchName, 
	U.Name AS UnitName, 
	DEPT.Name AS DepartmentName,
	SEC.Name AS SectionName,
	LINE.Name AS LineName,
	EM.EmployeeId AS EmployeeId, 
	Br.Id AS BranchId,
	COM.Id AS CompanyId,
	BUD.BranchUnitDepartmentId AS BranchUnitDepartmentId,
	BU.BranchUnitId AS BranchUnitId,
	DEG.Id AS DesignationId,
	DSEC.DepartmentSectionId AS DepartmentSectionId,
	DLine.DepartmentLineId AS DepartmentLineId,
    OELEMP.OvertimeEligibleEmployeeId AS OvertimeEligibleEmployeeId
	FROM Employee AS EM LEFT JOIN
		 (SELECT EmployeeCompanyInfoId, EmployeeId, FromDate, ToDate, DesignationId,DepartmentLineId,DepartmentSectionId, BranchUnitDepartmentId,IsActive, 
		         ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS RowNum
		  FROM EmployeeCompanyInfo AS EMCINFO
	      WHERE CAST(EMCINFO.FromDate AS Date) <= CURRENT_TIMESTAMP) EMPCINFO ON EM.EmployeeId = EMPCINFO.EmployeeId 
	INNER JOIN EmployeeDesignation AS DEG ON EMPCINFO.DesignationId = DEG.Id 
	LEFT JOIN EmployeeGrade AS empGrade ON  empGrade.Id = DEG.GradeId
	LEFT JOIN employeeType empType ON empGrade.EmployeeTypeId = empType.Id
	INNER JOIN BranchUnitDepartment AS BUD ON EMPCINFO.BranchUnitDepartmentId = BUD.BranchUnitDepartmentId 
	INNER JOIN BranchUnit AS BU ON BUD.BranchUnitId = BU.BranchUnitId 
	INNER JOIN UnitDepartment AS UD ON BUD.UnitDepartmentId = UD.UnitDepartmentId 
	INNER JOIN Unit AS U ON BU.UnitId = U.UnitId 
	INNER JOIN Department AS DEPT ON UD.DepartmentId = DEPT.Id 
	INNER JOIN Branch AS Br ON BU.BranchId = Br.Id 
	INNER JOIN Company AS COM ON Br.CompanyId = COM.Id
	INNER JOIN OvertimeEligibleEmployee AS OELEMP ON EM.EmployeeId=OELEMP.EmployeeId
	LEFT JOIN DepartmentSection AS DSEC ON EMPCINFO.DepartmentSectionId=DSEC.DepartmentSectionId
	LEFT JOIN Section AS SEC ON DSEC.SectionId=SEC.SectionId
	LEFT JOIN DepartmentLine AS DLine ON EMPCINFO.DepartmentLineId=DLine.DepartmentLineId
	LEFT JOIN Line AS LINE ON DLine.LineId=LINE.LineId
	WHERE EMPCINFO.RowNum = 1 
	AND EMPCINFO.IsActive=1 
	AND EM.IsActive=1 
	AND OELEMP.IsActive=1 
	AND OELEMP.[Status]=1
	--AND EM.[Status]=1 






