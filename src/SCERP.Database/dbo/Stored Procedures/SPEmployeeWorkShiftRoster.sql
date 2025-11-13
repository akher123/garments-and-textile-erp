
-- ==============================================================
-- Author:		<Yasir Arafat>
-- Create date: <2017-12-18>
-- Description:	<> EXEC [SPEmployeeWorkShiftRoster]  '2020-04-28'
-- ==============================================================

CREATE PROCEDURE [dbo].[SPEmployeeWorkShiftRoster]
						
							  
								@Date		DATETIME
							
															
AS

BEGIN
	
	SET NOCOUNT ON;
							DECLARE @BranchUnitWorkShiftId  INT
							DECLARE @WorkShiftRosterId		INT
							DECLARE @UnitName				NVARCHAR(50)
							DECLARE @GroupName				NVARCHAR(50)
							DECLARE @ShiftName				NVARCHAR(50)
							DECLARE @StartDate				DATETIME


							DECLARE Roster_cursor CURSOR FOR  
							 
							SELECT	   [WorkShiftRosterId]
									  ,[UnitName]
									  ,[GroupName]
									  ,[ShiftName]
									  ,[FromDate] 
									  ,[BranchUnitWorkShiftId]  
							FROM [dbo].[WorkShiftRoster]  
							WHERE IsActive = 1

							OPEN Roster_cursor   
							FETCH NEXT FROM Roster_cursor INTO @WorkShiftRosterId, @UnitName, @GroupName, @ShiftName, @StartDate, @BranchUnitWorkShiftId  

							WHILE @@FETCH_STATUS = 0   
							BEGIN   
									IF(@UnitName = 'Dyeing' AND DATENAME(dw, @Date) = 'Thursday' AND @ShiftName = 'Night' AND DATEDIFF(DAY, @StartDate, @Date) > 1)
									BEGIN 
											UPDATE WorkShiftRoster
											SET  ShiftName = 'Day'
												,BranchUnitWorkShiftId = 47
												,FromDate = DATEADD(DAY, 0, @Date)
											WHERE WorkShiftRosterId = @WorkShiftRosterId

											SET @BranchUnitWorkShiftId = 11
									END

								    IF(@UnitName = 'Dyeing' AND DATENAME(dw, @Date) = 'Friday' AND @ShiftName = 'Day' AND DATEDIFF(DAY, @StartDate, @Date) > 1)
									BEGIN 
											UPDATE WorkShiftRoster
											SET  ShiftName = 'Night'
												,BranchUnitWorkShiftId = 11
												,FromDate = DATEADD(DAY, 0, @Date)
											WHERE WorkShiftRosterId = @WorkShiftRosterId

											SET @BranchUnitWorkShiftId = 50
									END								 
																	
									INSERT INTO [dbo].[EmployeeWorkShift]
													   ([EmployeeId]
													   ,[BranchUnitWorkShiftId]
													   ,[ShiftDate]															
													   ,[Remarks]
													   ,[Status]
													   ,[CreatedDate]																														
													   ,[IsActive])
													   												       
											SELECT	   [EmployeeId]
													  ,@BranchUnitWorkShiftId
													  ,@Date													
													  ,'Roster Auto Shift Assign'
													  ,1
													  ,GETDATE()
													  ,1
												  FROM [dbo].[WorkGroupRoster]
												  WHERE UnitName = @UnitName AND GroupName = @GroupName AND @StartDate < @Date
								   

								   --  Insert workshift to history  --
								   INSERT INTO [dbo].[WorkShiftRosterHistory]
													([Date]
													,[UnitName]
													,[GroupName]
													,[ShiftName]
													,[BranchUnitWorkShiftId]
													,[CreatedDate]
													,[CreatedBy]
													,[EditedDate]
													,[EditedBy]
													,[IsActive])
												SELECT 
													 @Date
													,@UnitName
													,@GroupName
													,@ShiftName		
													,@BranchUnitWorkShiftId
													,[CreatedDate]
													,[CreatedBy]
													,[EditedDate]
													,[EditedBy]
													,[IsActive]
												FROM [dbo].[WorkShiftRoster]
												WHERE IsActive = 1 AND UnitName = @UnitName AND GroupName = @GroupName AND ShiftName = @ShiftName						   
								   	
								   FETCH NEXT FROM Roster_cursor INTO @WorkShiftRosterId, @UnitName, @GroupName, @ShiftName, @StartDate, @BranchUnitWorkShiftId    
							END   

							CLOSE Roster_cursor   
							DEALLOCATE Roster_cursor						
					
END