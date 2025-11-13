
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGetLineNameByDepartmentId 1
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPGetDesignationByGradeId]
	   

				@EmployeeGradeId INT
			  
AS
BEGIN
				SELECT EmployeeDesignation.Id, EmployeeDesignation.Title FROM EmployeeDesignation
				WHERE EmployeeDesignation.GradeId = @EmployeeGradeId AND EmployeeDesignation.IsActive = 1

				UNION ALL

				SELECT   - 1 AS Expr1, '<All>' AS Expr2
				ORDER BY EmployeeDesignation.Id	

				SET NOCOUNT ON;
END




