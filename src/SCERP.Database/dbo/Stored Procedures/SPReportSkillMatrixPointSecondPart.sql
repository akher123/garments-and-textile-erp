
-- ===========================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <14-Sep-15 2:09:40 PM>
-- Description:	<> EXEC [SPReportSkillMatrixPointSecondPart]	'0835'
-- ===========================================================================================================

CREATE PROCEDURE [dbo].[SPReportSkillMatrixPointSecondPart]
			
							 				
							 @employeeCardId			NVARCHAR(100) = NULL

AS

BEGIN
	
	SET NOCOUNT ON;
			 
						SELECT [PointId]
							  ,[EmployeeId]
							  ,[EmployeeCardId]
							  ,[ExperienceYear]
							  ,[ExperiencePoint]
							  ,[ExperienceWPoint]
							  ,[MultiMCTtl]
							  ,[MultiMCPoint]
							  ,[MultiMCWPoint]
							  ,[MultiProcessTtl]
							  ,[MultiProcessPoint]
							  ,[MultiProcessWPoint]
							  ,[ProcessGradePoint]
							  ,[ProcessGradeWPoint]
							  ,[AttitudeGrade]
							  ,[AttitudePoint]
							  ,[AttitudeWPoint]
							  ,[PerformancePercentage]
							  ,[PerformancePoint]
							  ,[PerformanceWPoint]
							  ,[TotalPoints]
							  ,[Grading]
							  ,[ProcessedWages]
							  ,[CreatedDate]
							  ,[CreatedBy]
							  ,[EditedDate]
							  ,[EditedBy]
							  ,[IsActive]
						  FROM [dbo].[SkillMatrixPointTable]
						  WHERE EmployeeCardId = @employeeCardId								
						  ORDER BY EmployeeCardId

END