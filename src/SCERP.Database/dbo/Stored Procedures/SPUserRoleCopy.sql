
-- ==================================================
-- Author : Yasir Arafat
-- Create date: 2018-09-29
-- Description:	EXEC SPUserRoleCopy  'anik','arefin'
-- ==================================================

CREATE PROCEDURE [dbo].[SPUserRoleCopy]

															 
								 @OldUserName NVARCHAR(20)			
								,@NewUserName NVARCHAR(20)

AS

BEGIN	
					SET NOCOUNT ON;
										
										
							  DELETE FROM [UserRole]
							  WHERE [UserName] = @NewUserName	
																																																												  
							  INSERT INTO [dbo].[UserRole]
											   ([UserName]
											   ,[ModuleFeatureId]
											   ,[AccessLevel]
											   ,[CDT]
											   ,[CreatedBy]
											   ,[EDT]
											   ,[EditedBy]
											   ,[IsActive])
										SELECT 
											   @NewUserName
											  ,[ModuleFeatureId]
											  ,[AccessLevel]
											  ,[CDT]
											  ,[CreatedBy]
											  ,[EDT]
											  ,[EditedBy]
											  ,[IsActive]
										  FROM [dbo].[UserRole]		
										 WHERE [UserName] = @OldUserName									
	
END