-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <16/10/2019>
-- Description:	<> EXEC SPAssignWorkShiftRoster  '700070', '2019-10-01', 'Night'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPAssignWorkShiftRoster]
					

							   @EmployeeCardId   		NVARCHAR(20)
							  ,@Date					DATE
							  ,@ShiftName			    NVARCHAR(20)													 													 
																	 						   
AS

BEGIN
	
				 SET NOCOUNT ON;
							   
							
							  DECLARE @GroupName		NVARCHAR(20)
							  DECLARE @EmployeeId		UNIQUEIDENTIFIER


							  SELECT @EmployeeId = EmployeeId FROM Employee WHERE EmployeeCardId = @EmployeeCardId AND IsActive = 1
							  SELECT @GroupName = GroupName FROM WorkShiftRosterHistory WHERE Date = @Date AND ShiftName = @ShiftName
							  
							  PRINT @EmployeeId
							  														 						
							  -- Delete and Insert workshift --
							  DELETE FROM EmployeeWorkShift WHERE ShiftDate BETWEEN @Date AND GETDATE() AND EmployeeId = @EmployeeId
							  							 
							  INSERT INTO [dbo].[EmployeeWorkShift]
								   ([EmployeeId]
								   ,[BranchUnitWorkShiftId]
								   ,[ShiftDate]
								   ,[StartDate]
								   ,[EndDate]
								   ,[Remarks]
								   ,[Status]
								   ,[CreatedDate]
								   ,[CreatedBy]
								   ,[EditedDate]
								   ,[EditedBy]
								   ,[IsActive])					
							  SELECT 
							       @EmployeeId
							      ,[BranchUnitWorkShiftId]
								  ,[Date]
								  ,NULL
								  ,NULL
								  ,'Auto Shift Assign'
								  ,1
								  ,[CreatedDate]
								  ,[CreatedBy]
								  ,[EditedDate]
								  ,[EditedBy]
								  ,[IsActive]
							  FROM [dbo].[WorkShiftRosterHistory]
							  WHERE Date BETWEEN @Date AND GETDATE() AND GroupName = @GroupName


							  -- Delete and Assign employee to a group --

							  DELETE FROM WorkGroupRoster WHERE EmployeeId = @EmployeeId

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
							  SELECT 
								   'Dyeing'
								   ,@GroupName
								   ,@EmployeeId
								   ,@EmployeeCardId
								   ,GETDATE()
								   ,NULL
								   ,GETDATE()
								   ,NULL
								   ,1								   
												 					  					  														  						  											  							
END