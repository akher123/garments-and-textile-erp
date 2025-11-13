

	CREATE view [dbo].[VEmployeeWorkShiftDetail]
	AS
	SELECT   
	WS.Name AS WorkShiftName,
	ews.ShiftDate AS ShiftDate,
	ews.Remarks AS Remarks,
    EM.Name AS EmployeeName, 
	EM.EmployeeCardId AS EmployeeCardId, 
	DEG.Title AS Designation, 
	empType.Id AS EmployeeTypeId,
	empType.Title AS EmployeeTypeTitle,
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
	UD.UnitDepartmentId AS UnitDepartmentId, 
	DSEC.DepartmentSectionId AS DepartmentSectionId,
	DLine.DepartmentLineId AS DepartmentLineId,
	EMPCINFO.EmployeeCompanyInfoId AS EmployeeCompanyInfoId,
	BUWS.BranchUnitWorkShiftId AS BranchUnitWorkShiftId,
	ews.EmployeeWorkShiftId AS EmployeeWorkShiftId
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
	LEFT JOIN EmployeeWorkShift ews ON ews.EmployeeId = EM.EmployeeId
	INNER JOIN BranchUnitWorkShift buws on ews.BranchUnitWorkShiftId = buws.BranchUnitWorkShiftId
	INNER JOIN WorkShift ws ON buws.WorkShiftId = ws.WorkShiftId						 	
	LEFT JOIN DepartmentSection AS DSEC ON EMPCINFO.DepartmentSectionId=DSEC.DepartmentSectionId
	LEFT JOIN Section AS SEC ON DSEC.SectionId=SEC.SectionId
	LEFT JOIN DepartmentLine AS DLine ON EMPCINFO.DepartmentLineId=DLine.DepartmentLineId
	LEFT JOIN Line AS LINE ON DLine.LineId=LINE.LineId
	WHERE EMPCINFO.RowNum = 1 
		  AND EMPCINFO.IsActive=1 
	      AND ews.IsActive=1 
	      AND EM.IsActive=1 and ews.[Status]=1



