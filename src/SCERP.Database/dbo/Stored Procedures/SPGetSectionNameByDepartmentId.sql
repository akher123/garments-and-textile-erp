
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGetLineNameByDepartmentId 1
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPGetSectionNameByDepartmentId]
	   
				@DepartmentId INT
			  
AS
BEGIN
				
				SELECT Section.SectionId, Section.Name FROM UnitDepartment
				INNER JOIN BranchUnitDepartment ON BranchUnitDepartment.UnitDepartmentId = UnitDepartment.UnitDepartmentId
				INNER JOIN DepartmentSection ON DepartmentSection.BranchUnitDepartmentId = BranchUnitDepartment.BranchUnitDepartmentId
				INNER JOIN Section ON Section.SectionId = DepartmentSection.SectionId
				WHERE UnitDepartment.DepartmentId = @DepartmentId
				AND BranchUnitDepartment.IsActive = 1
				AND DepartmentSection.IsActive = 1
				AND Section.IsActive = 1

				UNION ALL

				SELECT        - 1 AS Expr1, '<All>' AS Expr2
				ORDER BY Section.SectionId

				SET NOCOUNT ON;
END