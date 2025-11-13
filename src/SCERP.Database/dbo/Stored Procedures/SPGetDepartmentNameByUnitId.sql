
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGetDepartmentNameByUnitId 1
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPGetDepartmentNameByUnitId]
	   

				@UnitId INT

AS
BEGIN
	
				SELECT	Department.Id, Department.Name	   
				FROM    UnitDepartment INNER JOIN Department ON UnitDepartment.DepartmentId = Department.Id     				                        
				WHERE	UnitDepartment.UnitId = @UnitId
				AND     UnitDepartment.IsActive = 1
				AND		Department.IsActive = 1

				UNION ALL

				SELECT        - 1 AS Expr1, '<All>' AS Expr2
				ORDER BY Department.Id


				SET NOCOUNT ON;
END