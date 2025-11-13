
-- ===========================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2018-01-03>
-- Description:	<> EXEC [SPSaveWorkShiftRoster] 'Dyeing', 'Group-1','','Ashik Shak','2017-12-31'
-- ===========================================================================================================

CREATE PROCEDURE [dbo].[SPSaveWorkShiftRoster]
						

									  @UnitName				NVARCHAR(50)
									 ,@GroupName			NVARCHAR(50)		
									 ,@EmployeeCardId		NVARCHAR(50)					
									 									
AS

BEGIN
	
	SET NOCOUNT ON;

							DECLARE @EmployeeId	UNIQUEIDENTIFIER = NULL

							SELECT @EmployeeId = EmployeeId FROM Employee WHERE EmployeeCardId = @EmployeeCardId

							IF(@EmployeeId IS NOT NULL)
							BEGIN

								DELETE FROM [WorkGroupRoster]
								WHERE [EmployeeCardId] = @EmployeeCardId
								 
			 					INSERT INTO [dbo].[WorkGroupRoster]
												 ([UnitName]
												 ,[GroupName]
												 ,[EmployeeId]
												 ,[EmployeeCardId]
												 ,[CreatedDate]
												 ,[CreatedBy]
												 ,[EditedDate]
												 ,[EditedBy]
												 ,[IsActive])
										  
										  VALUES (@UnitName
												 ,@GroupName
												 ,@EmployeeId
												 ,@EmployeeCardId
												 ,CURRENT_TIMESTAMP
												 ,NULL
												 ,CURRENT_TIMESTAMP
												 ,NULL
												 ,1)
									

										SELECT [WorkGroupRosterId]
											  ,[UnitName]
											  ,[GroupName]
											  ,[EmployeeId]
											  ,[EmployeeCardId]
											  ,[CreatedDate]
											  ,[CreatedBy]
											  ,[EditedDate]
											  ,[EditedBy]
											  ,[IsActive]
										  FROM [dbo].[WorkGroupRoster]	
										  WHERE [UnitName] = @UnitName 
											AND [GroupName] = @GroupName
											AND [EmployeeCardId] = @EmployeeCardId 	
									
											   
							END
																												  						
END