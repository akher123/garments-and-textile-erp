-- ==============================================
-- Author:		<>
-- Create date: <26/01/2019>
-- Description:	<> SPGetEmployeeIdByCardId '0835'
-- ==============================================

CREATE PROCEDURE [dbo].[SPGetEmployeeIdByCardId]
	

					@employeeCardId NVARCHAR(100)

AS

BEGIN
	
					SET NOCOUNT ON;

					SET XACT_ABORT ON;

					SELECT EmployeeId FROM Employee
					WHERE SUBSTRING(EmployeeCardId, PATINDEX('%[^0 ]%', EmployeeCardId + ' '), LEN(EmployeeCardId)) = SUBSTRING(@employeeCardId, PATINDEX('%[^0 ]%', @employeeCardId + ' '), LEN(@employeeCardId))
					AND Employee.IsActive = 1

END