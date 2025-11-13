-- ==================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>(To date is always process date)
-- Description:	<> EXEC Utility_EmployeeDonotHaveWorkShift '2020-09-01', '2020-09-22'
-- ==================================================================================

CREATE PROCEDURE [dbo].[Utility_EmployeeDonotHaveWorkShift]
			
									 
							@FromDate		DATETIME
						   ,@ToDate			DATETIME
						   
AS

BEGIN
	
			SET NOCOUNT ON;
					  												 
							DECLARE @CountDate DATE = @fromdate

							DECLARE @EmployeeShift TABLE
							(
								   ShiftDate	  DATE
								  ,EmployeeId	  UNIQUEIDENTIFIER
								  ,EmployeCardId  NVARCHAR(10)
								  ,JoinDate		  DATE
								  ,QuitDate		  DATE
							)

							WHILE @CountDate <= @toDate
							BEGIN
									INSERT INTO @EmployeeShift(ShiftDate, EmployeeId, EmployeCardId, JoinDate, QuitDate)
									SELECT @CountDate, EmployeeId, EmployeeCardId, JoiningDate, QuitDate FROM Employee		
									WHERE ((CAST(QuitDate AS DATE) >= @CountDate) OR QuitDate IS NULL)
									AND CAST(JoiningDate AS DATE) <= @CountDate									
									AND EmployeeCardId NOT IN('999998','999999')
									AND IsActive = 1
									AND EmployeeId NOT IN(

										SELECT EmployeeId FROM EmployeeWorkShift WHERE CAST(ShiftDate AS DATE) = @CountDate AND IsActive = 1
									)

									SET @CountDate = DATEADD(DAY, 1, @CountDate)
							END

							SELECT EmployeCardId, EmployeeId, ShiftDate, JoinDate, QuitDate FROM @EmployeeShift ORDER BY EmployeCardId, ShiftDate	

							--------------------------------------------------------------

							DECLARE @CheckDate			DATETIME
							DECLARE @EmployeeCardId		NVARCHAR(10)
							DECLARE @EmployeeId			UNIQUEIDENTIFIER
							DECLARE @WorkShiftId		INT
							

							DECLARE @AssignShift AS CURSOR

							SET @AssignShift = CURSOR FOR

							SELECT EmployeCardId, ShiftDate								  
							FROM @EmployeeShift 
							WHERE LEN(EmployeCardId) < 5
							ORDER BY EmployeCardId, ShiftDate	

							OPEN @AssignShift
							FETCH NEXT FROM @AssignShift INTO @EmployeeCardId, @CheckDate

							WHILE @@FETCH_STATUS = 0
							BEGIN							

								 SELECT @EmployeeId = EmployeeId FROM Employee WHERE EmployeeCardId = @EmployeeCardId
								 SELECT @WorkShiftId = BranchUnitWorkShiftId FROM [EmployeeWorkShift] WHERE EmployeeId = @EmployeeId AND ShiftDate =  DATEADD(Day, -1, @CheckDate)

								 INSERT INTO [dbo].[EmployeeWorkShift]
												   ([EmployeeId]
												   ,[BranchUnitWorkShiftId]
												   ,[ShiftDate]
												   ,[StartDate]
												   ,[EndDate]
												   ,[Remarks]
												   ,[Status]
												   ,[CreatedDate]
												   ,[IsActive])
										VALUES(
												   @EmployeeId
												  ,@WorkShiftId
												  ,@CheckDate
												  ,@CheckDate
												  ,@CheckDate
												  ,'Auto Shift Assign'  		
												  ,1
												  ,GETDATE()			
												  ,1
											 )
										  
							FETCH NEXT FROM @AssignShift INTO @EmployeeCardId, @CheckDate
							END

							CLOSE @AssignShift
							DEALLOCATE @AssignShift													 					  					  														  						  											  							
END