
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGetCompanyNameByUserId 'superadmin'
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPGetCompanyNameByUserId]
	   			
			   @UserName NVARCHAR(100)

AS
BEGIN
	
				SELECT Distinct Company.Id, Company.Name 
				FROM Company INNER JOIN UserPermissionForDepartmentLevel			 
				ON  Company.Id = UserPermissionForDepartmentLevel.CompanyId
				WHERE UserPermissionForDepartmentLevel.UserName = @UserName
				AND UserPermissionForDepartmentLevel.IsActive = 1
				AND Company.IsActive = 1

				UNION ALL
				SELECT        - 1 AS Expr1, '<All>' AS Expr2
				ORDER BY Company.Id

				SET NOCOUNT ON;
END




