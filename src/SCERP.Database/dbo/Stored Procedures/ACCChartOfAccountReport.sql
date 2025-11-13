
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <04/04/2015>
-- Description:	<> exec ACCChartOfAccountReport
-- =============================================
CREATE PROCEDURE [dbo].[ACCChartOfAccountReport]
						
						 
AS

BEGIN
	
	SET NOCOUNT ON;

				SELECT T1.ControlCode AS MainCode
					  ,T1.ControlName AS MainHead

					  ,T2.ControlCode AS GroupCode
					  ,T2.ControlName AS GroupName

					  ,T3.ControlCode AS SubGroupCode
					  ,T3.ControlName AS SubGroupName

					  ,T4.ControlCode AS ControlCode
					  ,T4.ControlName AS ControlName

					  ,T5.AccountCode AS GLCode
					  ,T5.AccountName AS GLName


					  FROM [dbo].[Acc_ControlAccounts] AS T1 
					  INNER JOIN  [dbo].[Acc_ControlAccounts] AS T2 ON T2.ParentCode = T1.ControlCode AND T2.IsActive = 1
					  INNER JOIN  [dbo].[Acc_ControlAccounts] AS T3 ON T3.ParentCode = T2.ControlCode AND T3.IsActive = 1
					  INNER JOIN  [dbo].[Acc_ControlAccounts] AS T4 ON T4.ParentCode = T3.ControlCode AND T4.IsActive = 1
					  INNER JOIN  [dbo].Acc_GLAccounts AS T5 ON T5.ControlCode = T4.ControlCode AND T5.IsActive = 1
					  ORDER BY T1.ControlCode, T2.ControlCode,T3.ControlCode,T4.ControlCode, T5.AccountCode																			
																						
END