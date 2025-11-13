-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================

--- SELECT dbo.fnGetHeadOfDepartmentDesignation(16,'2015-02-01')

CREATE FUNCTION [dbo].[fnGetHeadOfDepartmentDesignation]
(
	 @DepartmentId INT,
	 @UpToDate DATETIME

)
RETURNS NVARCHAR(100)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @EmployeeDesignation NVARCHAR(100)

	SELECT 
									
			@EmployeeDesignation = EmployeeDesignation.Title

			FROM 
			
			HeadOfDepartment hod 
			INNER JOIN Employee emp ON hod.EmployeeId = emp.EmployeeId
			LEFT JOIN 
	
			(SELECT EmployeeId, PunchCardNo, BranchUnitDepartmentId, DesignationId,
					ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
			FROM EmployeeCompanyInfo AS employeeCompanyInfo
			WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @UpToDate) OR (@UpToDate IS NULL))
			AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
			ON emp.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1  
			LEFT JOIN EmployeeDesignation ON employeeCompanyInfo.DesignationId = EmployeeDesignation.Id AND EmployeeDesignation.IsActive = 1 
			LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId 
			LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
			LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
			LEFT JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
			LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
			WHERE
			(	
			 unitDepartment.DepartmentId = @DepartmentId
			 AND (emp.[Status] = 1)
			 AND (emp.IsActive = 1)			
			 AND (employeeCompanyInfo.rowNum = 1)
			)			 				   		
			ORDER BY emp.EmployeeCardId

	RETURN @EmployeeDesignation

END
