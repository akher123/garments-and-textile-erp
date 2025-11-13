
-- ===========================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <14-Sep-15 2:09:40 PM>
-- Description:	<> EXEC [SPGetSkillMatrixDetailProcessName] '1', 'EBFC205F-54E4-48DE-8A7B-0032958803C6'
-- ===========================================================================================================


CREATE PROCEDURE [dbo].[SPGetSkillMatrixDetailProcessName]
						
									
							@MachineTypeId		INT			
						   ,@EmployeeId			UNIQUEIDENTIFIER
							
AS

BEGIN
	
	SET NOCOUNT ON;
			 
					    SELECT 
					--SkillMatrixDetailId
					--	  ,SkillMatrixId
						   SkillMatrixProcessName.ProcessId
						  ,SkillMatrixProcessName.MachineTypeId
						  ,SkillMatrixProcessName.ProcessName
						  ,SkillMatrixDetail.ProcessSmv
						  ,SkillMatrixDetail.ProcessGrade
						  ,SkillMatrixDetail.AverageCycle
						  ,SkillMatrixProcessName.StandardProcessSmv

					FROM SkillMatrixProcessName 
					LEFT JOIN SkillMatrixDetail ON SkillMatrixProcessName.ProcessId = SkillMatrixDetail.ProcessId
					WHERE SkillMatrixProcessName.IsActive = 1 
						  --AND SkillMatrixDetail.IsActive = 1
					
END