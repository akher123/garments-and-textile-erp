
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGetLineNameByDepartmentId 1
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPGetLineNameByDepartmentId]
	   
				@DepartmentId INT
			  
AS
BEGIN
	
				SELECT Line.LineId, Line.Name FROM UnitDepartment
				INNER JOIN BranchUnitDepartment ON BranchUnitDepartment.UnitDepartmentId = UnitDepartment.UnitDepartmentId
				INNER JOIN DepartmentLine ON DepartmentLine.BranchUnitDepartmentId = BranchUnitDepartment.BranchUnitDepartmentId
				INNER JOIN Line ON Line.LineId = DepartmentLine.LineId
				WHERE UnitDepartment.DepartmentId = @DepartmentId	
				AND BranchUnitDepartment.IsActive = 1
				AND DepartmentLine.IsActive = 1
				AND Line.IsActive = 1	

				UNION ALL

				SELECT   - 1 AS Expr1, '<All>' AS Expr2
				ORDER BY Line.LineId

				SET NOCOUNT ON;
END




