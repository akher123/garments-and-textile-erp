
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGetBranchNameByCompanyId 1
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPGetBranchNameByCompanyId]
	   

				@CompanyId INT

AS
BEGIN
	
				SELECT        Id, Name
				FROM            Branch
				WHERE        (CompanyId = @CompanyId) AND Branch.IsActive = 1

					UNION ALL

				SELECT        - 1 AS Expr1, '<All>' AS Expr2
				ORDER BY  Id

				SET NOCOUNT ON;
END




