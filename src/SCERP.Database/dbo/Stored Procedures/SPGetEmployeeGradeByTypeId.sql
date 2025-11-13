
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGetLineNameByDepartmentId 1
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeGradeByTypeId]
	   
				@EmployeeTypeId INT
			  
AS
BEGIN	
				SELECT EmployeeGrade.Id, EmployeeGrade.Name  FROM EmployeeGrade
				WHERE EmployeeGrade.EmployeeTypeId = @EmployeeTypeId AND EmployeeGrade.IsActive = 1

				UNION ALL

				SELECT   - 1 AS Expr1, '<All>' AS Expr2
				ORDER BY EmployeeGrade.Id

				SET NOCOUNT ON;
END




