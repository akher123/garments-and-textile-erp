-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <26/04/2015>
-- Description:	<> exec SPGetAttendanceDataFromMachine '04/20/2015','04/26/2015'
-- =============================================

CREATE PROCEDURE [dbo].[SPGetAttendanceDataFromMachine]

			@FromDate DateTime = NULL,
			@ToDate DateTime = NULL

AS
BEGIN
	
			SET NOCOUNT ON;
			
			IF EXISTS (SELECT name FROM sys.servers WHERE (server_id != 0) AND [name] = 'SCERP_Attendance_Linked_Server')
			BEGIN
				EXEC sp_dropserver @server='SCERP_Attendance_Linked_Server'
			END
		
			EXEC sp_addlinkedserver
			@server='SCERP_Attendance_Linked_Server',	
			@srvproduct='',     
			@provider='sqlncli',		
			@datasrc='180.92.239.110',  
			@location='',
			@provstr='',
			@catalog='Plummy_Attendance' 
								  
			EXEC sp_addlinkedsrvlogin
			@rmtsrvname = '180.92.239.110',
			@useself = 'false',
			@rmtuser = 'sa',            
			@rmtpassword = 'sa!23$'	
				 
			EXEC sp_serveroption '180.92.239.110', 'rpc out', true;
									
	BEGIN TRAN T1;
								     
		INSERT INTO EmployeeDailyAttendance
		(
			 [EmployeeId]
			,[EmployeeCardId]
			,[TransactionDateTime]
			,[FunctionKey]
			,[IsFromMachine]
			,[Remarks]
			,[CreatedDate]								
			,[IsActive]
		)
		SELECT 
			 Employee.EmployeeId
			,Employee.EmployeeCardId
			,MachineData.CheckTime
			,1
			,1
			,'From Machine'
			,Current_timestamp						
			,1

		FROM SC_Server.Plummy_Attendance.dbo.Checkinout AS MachineData
		JOIN Employee ON Employee.EmployeeCardId = MachineData.UserId
		COLLATE DATABASE_DEFAULT
		WHERE IsExported = 0 
		AND (CAST(MachineData.CheckTime AS date) >= CAST(@FromDate AS date) OR @FromDate IS NULL)
		AND (CAST(MachineData.CheckTime AS date) <= CAST(@ToDate AS date) OR @ToDate IS NULL)	  

		
		UPDATE  MachineData
		SET IsExported = 1
		FROM SC_Server.Plummy_Attendance.dbo.Checkinout AS MachineData
		WHERE IsExported = 0
		AND (CAST(MachineData.CheckTime AS date) >= CAST(@FromDate AS date) OR @FromDate IS NULL)
		AND (CAST(MachineData.CheckTime AS date) <= CAST(@ToDate AS date) OR @ToDate IS NULL)	 

		SELECT 1 As Status

	COMMIT TRAN T1;

END

