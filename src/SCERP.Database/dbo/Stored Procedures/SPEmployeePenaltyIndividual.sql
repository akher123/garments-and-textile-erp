
-- =================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2018-10-10>
-- Description:	<> EXEC [dbo].[SPEmployeePenaltyIndividual] '6BEB83B8-AA98-4457-8EC3-0343ECEC6426', '2018-10-12'
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[SPEmployeePenaltyIndividual]

								 
									 @EmployeeId	UNIQUEIDENTIFIER
									,@UpToDate		DATETIME 
				

AS

BEGIN
									DECLARE @EmployeeCardId VARCHAR(50)
									
									SELECT TOP(1) @EmployeeCardId = EmployeeCardId FROM Employee WHERE EmployeeId = @EmployeeId
					

									SELECT [EmployeeCardId]
										  ,Year
										  ,PenaltyAmount
										  ,[HrmPenaltyType].Type
										  FROM

									(SELECT 
										   [EmployeeCardId]
										  ,YEAR([PenaltyDate]) AS Year
										  ,[PenaltyTypeId]
										  ,SUM([Penalty]) AS PenaltyAmount
      
									  FROM [dbo].[HrmPenalty]
									  WHERE EmployeeCardId = @EmployeeCardId
									  GROUP BY [EmployeeCardId], [PenaltyTypeId], YEAR([PenaltyDate])
									  ) AS Table1 LEFT JOIN [HrmPenaltyType] ON [HrmPenaltyType].PenaltyTypeId = Table1.PenaltyTypeId  	

END