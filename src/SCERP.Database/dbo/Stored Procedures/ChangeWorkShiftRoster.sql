-- ===============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <11/09/2019>
-- Description:	<> EXEC ChangeWorkShiftRoster '2019-09-01', '2019-09-10', '', ''
-- ===============================================================================
CREATE PROCEDURE [dbo].[ChangeWorkShiftRoster]
					

							   @FromDate				DATE
							  ,@ToDate					DATE	
							  ,@EmployeeCardIdFrom		NVARCHAR(20)
							  ,@EmployeeCardIdTo		NVARCHAR(20)										 
																	 						   
AS

BEGIN
	
				 SET NOCOUNT ON;
							   
							   DECLARE @EmployeeIdFrom UNIQUEIDENTIFIER
							   DECLARE @EmployeeIdTo   UNIQUEIDENTIFIER


							   SELECT @EmployeeIdFrom = EmployeeId FROM Employee WHERE EmployeeCardId = @EmployeeCardIdFrom
							   AND IsActive = 1

							   SELECT @EmployeeIdTo = EmployeeId FROM Employee WHERE EmployeeCardId = @EmployeeCardIdTo
							   AND IsActive = 1
							   							  

							   DELETE FROM [EmployeeWorkShift]
							   WHERE CAST(ShiftDate AS DATE) BETWEEN @FromDate AND  @ToDate
							   AND EmployeeId = @EmployeeIdTo										 
						

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
							   @EmployeeIdTo
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
							  ,[IsActive]
						  FROM [dbo].[EmployeeWorkShift]
						  WHERE CAST(ShiftDate AS DATE) BETWEEN @FromDate AND @ToDate
						  AND EmployeeId = @EmployeeIdFrom
							 					  					  														  						  											  							
END