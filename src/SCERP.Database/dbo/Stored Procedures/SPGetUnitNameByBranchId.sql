
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGetUnitNameByBranchId 1
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPGetUnitNameByBranchId]
	   
				@BranchId INT
			  
AS
BEGIN
	
				SELECT        Unit.UnitId, Unit.Name
				FROM          BranchUnit INNER JOIN Unit ON BranchUnit.UnitId = Unit.UnitId                         
				WHERE		  BranchUnit.BranchId = @BranchId 
				AND			  BranchUnit.IsActive = 1
				AND			  Unit.IsActive = 1		
				
				UNION ALL

				SELECT        - 1 AS Expr1, '<All>' AS Expr2
				ORDER BY Unit.UnitId		

				SET NOCOUNT ON;
END




