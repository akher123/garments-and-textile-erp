-- ==============================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <25/03/2019>
-- Description:	<> EXEC SPFemaleConsentLetter -1, -1, -1, -1, -1, '', '2019-03-25', '2019-03-25'
-- =============================================================================================================

CREATE PROCEDURE [dbo].[SPFemaleConsentLetter]
			

							 @CompanyId						INT = -1
							,@BranchId						INT = -1
							,@BranchUnitId					INT = -1	
							,@BranchUnitDepartmentId		INT = -1
							,@DepartmentSectionId			INT = -1
							,@EmployeeCardId				NVARCHAR(100) = ''
							,@DisagreeDate					DATETIME
							,@EffectiveDate					DATETIME

AS
	
BEGIN
	
			SET NOCOUNT ON;
			

			SELECT		  
						  CAST(ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS NVARCHAR(10)) AS SerialNo
						 ,Company.NameInBengali AS CompanyNameInBengali
						 ,Company.FullAddressInBengali
						 ,Employee.EmployeeCardId
						 ,Employee.NameInBengali 
						 ,EmployeeDesignation.TitleInBengali AS DesignationInBengali	
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) JoiningDate						 
						 ,CONVERT(VARCHAR(10),@DisagreeDate, 103) DisagreeDate												 																																			
						 																									
FROM			    	Employee AS  employee

						LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @EffectiveDate) OR (@EffectiveDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
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
						LEFT JOIN Gender ON Gender.GenderId = Employee.GenderId
						
						WHERE employee.IsActive = 1 
						AND employee.[Status] = 1
						AND employeeCompanyInfo.rowNum = 1 	
						AND Gender.GenderId = 2	
						AND EmployeeType.Id IN(4,5)
							
						AND (company.Id = @CompanyId OR @CompanyId = -1)
						AND (branch.Id = @BranchId OR @BranchId = -1 )
						AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = -1)
						AND (branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId = -1 )
						AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId = -1)
						AND (Employee.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId ='')

						ORDER BY EmployeeCardId
END