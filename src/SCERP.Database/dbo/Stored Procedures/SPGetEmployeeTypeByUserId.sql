
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGetEmployeeTypeByUserId 'superadmin'
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeTypeByUserId]
	   			
			   @UserName NVARCHAR(100)

AS
BEGIN
	
				SELECT Distinct EmployeeType.Id, EmployeeType.Title 
				FROM EmployeeType 
				INNER JOIN UserPermissionForEmployeeLevel ON  EmployeeType.Id = UserPermissionForEmployeeLevel.EmployeeTypeId		 				
				WHERE UserPermissionForEmployeeLevel.UserName = @UserName
				AND UserPermissionForEmployeeLevel.IsActive = 1
				AND EmployeeType.IsActive = 1

				UNION ALL

				SELECT   - 1 AS Expr1, '<All>' AS Expr2
				ORDER BY EmployeeType.Id

				SET NOCOUNT ON;

				SET NOCOUNT ON;
END




