-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <31/03/2015>
-- Description:	<> exec SPShortLeaveSummary '03/01/2015', '03/31/2015'
-- =============================================
CREATE PROCEDURE [dbo].[SPShortLeaveSummary]
						  @BranchUnitDepartmentId INT,
						  @DepartmentSectionId INT = NULL,
						  @DepartmentLineId INT = NULL,
						  @EmployeeCardNo NVARCHAR(100) = NULL,
						  @FromDate DATETIME,
						  @ToDate DATETIME,
						  @UserName NVARCHAR(100)
AS
BEGIN
	
	SET NOCOUNT ON;

			DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);

			INSERT INTO @ListOfEmployeeTypeIds
			SELECT DISTINCT EmployeeTypeId FROM UserPermissionForEmployeeLevel
			WHERE UserName = @UserName;

			Declare @StartDate Datetime = Current_timestamp

			SELECT		  Employee.EmployeeId
						 ,Employee.EmployeeCardId
						 ,Employee.Name 
						 ,EmployeeDesignation.Title AS Designation	
						 ,EmployeeGrade.Name AS Grade														 
						 ,EmployeeType.Title AS EmployeeType
						 ,Company.Name AS CompanyName
						 ,Company.FullAddress AS CompanyAddress
						 ,Branch.Name AS Branch
						 ,Unit.Name AS Unit	
						 ,Department.Name AS Department
						 ,Section.Name AS Section	
						 ,Line.Name	AS Line																							
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) AS JoiningDate	
						 ,CONVERT(VARCHAR(10), EmployeeShortLeave.Date, 103) AS ShortLeaveDate														 																																						
						 ,CONVERT(INT, EmployeeShortLeave.ReasonType) ReasonType
						 ,[ReasonName]=
							CASE
								WHEN EmployeeShortLeave.ReasonType = 1 THEN 'Personal'
								WHEN EmployeeShortLeave.ReasonType = 2 THEN 'Official'
								ELSE ''
							END
						 ,EmployeeShortLeave.FromTime
						 ,EmployeeShortLeave.ToTime
						 ,EmployeeShortLeave.TotalHours
						 ,BranchUnitDepartment.BranchUnitDepartmentId AS DepartmentId
						 ,DepartmentSection.DepartmentSectionId AS SectionId
						 ,DepartmentLine.DepartmentLineId AS LineId
					     ,CONVERT(VARCHAR(10),@FromDate, 103) AS FromDate
						 ,CONVERT(VARCHAR(10),@ToDate, 103) AS ToDate
						 				
	FROM				EmployeeShortLeave	LEFT OUTER JOIN 																										
						Employee ON EmployeeShortLeave.EmployeeId = Employee.EmployeeId AND Employee.IsActive = 1 												 					                       
						LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo 
						WHERE (CAST(employeeCompanyInfo.FromDate AS Date) <= @StartDate) 
							  AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
							  ON employee.EmployeeId = employeeCompanyInfo.EmployeeId
						INNER JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
						INNER JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
						INNER JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
						INNER JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
						INNER JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
						INNER JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
						INNER JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
						INNER JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
						INNER JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
						INNER JOIN Company  AS company ON branch.CompanyId=company.Id															
						LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
						LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
						LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
						LEFT JOIN Line line on departmentLine.LineId = line.LineId
						WHERE branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId
						AND ((departmentSection.DepartmentSectionId = @DepartmentSectionId) OR (@DepartmentSectionId IS NULL))
						AND ((departmentLine.DepartmentLineId = @DepartmentLineId) OR (@DepartmentLineId IS NULL))
						AND (employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds))
						AND employeeCompanyInfo.rowNum = 1 
						AND EmployeeShortLeave.IsActive = 1 
						AND EmployeeShortLeave.[Date] >= @FromDate 
						AND EmployeeShortLeave.[Date] <= @ToDate	
						AND ((Employee.EmployeeCardId = @EmployeeCardNo) OR (@EmployeeCardNo IS NULL))	
						AND (Employee.IsActive=1)
						AND (Employee.[Status] = 1)		
						ORDER BY EmployeeCardId ASC												   
END



