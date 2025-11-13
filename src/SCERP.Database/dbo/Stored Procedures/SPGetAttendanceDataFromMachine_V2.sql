-- =============================================
-- Author:		Golam Rabbi
-- Create date: 2015.04.29
-- Description:	<> exec [SPGetAttendanceDataFromMachine_V2] '04/20/2015','04/26/2015'
-- =============================================

CREATE PROCEDURE [dbo].[SPGetAttendanceDataFromMachine_V2]

			@FromDate DateTime = NULL,
			@ToDate DateTime = NULL

AS
BEGIN
	
			SET NOCOUNT ON;
			
		SET NOCOUNT ON;
			
			IF EXISTS (SELECT name FROM sys.servers WHERE (server_id != 0) AND [name] = 'SCERP_Attendance_Linked_Server2')
			BEGIN
				EXEC sp_dropserver @server='SCERP_Attendance_Linked_Server2'
			END
		
			EXEC sp_addlinkedserver
			@server='SCERP_Attendance_Linked_Server2',	
			@srvproduct='',     
			@provider='sqlncli',		
			@datasrc='180.92.239.110',  
			@location='',
			@provstr='',
			@catalog='Plummy_Attendance' 					  
			EXEC sp_addlinkedsrvlogin
			@rmtsrvname = '192.168.10.50',
			@useself = 'false',
			@rmtuser = 'sa',            
			@rmtpassword = 'sa!23$'		 
			EXEC sp_serveroption '192.168.10.50', 'rpc out', true;
									
 

	BEGIN TRAN T1;
								     
		INSERT INTO EmployeeDailyAttendance
		(
			 [EmployeeCardId]
			,[TransactionDateTime]
			,[IsFromMachine]
			,[Remarks]
			,[CreatedDate]
			,[CreatedBy]
			,[EditedDate]
			,[EditedBy]
			,[IsActive]
		)
		SELECT 
			 Userid
			,CheckTime
			, 1
			,'From Machine'
			,CURRENT_TIMESTAMP
			,NULL
			,NULL
			,NULL
			,1
		FROM SC_Server.Plummy_Attendance.dbo.Checkinout AS MachineData
		WHERE MachineData.IsExported = 0
		AND (CAST(MachineData.CheckTime AS date) >= CAST(@FromDate AS date) OR @FromDate IS NULL)
		AND (CAST(MachineData.CheckTime AS date) <= CAST(@ToDate AS date) OR @ToDate IS NULL)	  

		UPDATE  SC_Server.Plummy_Attendance.dbo.Checkinout 
		SET IsExported = 1
		--FROM SC_Server.SCERPDB_PILOT.dbo.EmployeeDailyAttendance AS MachineData
		WHERE IsExported = 0
		AND (CAST(CheckTime AS date) >= CAST(@FromDate AS date) OR @FromDate IS NULL)
		AND (CAST(CheckTime AS date) <= CAST(@ToDate AS date) OR @ToDate IS NULL)	 

	COMMIT TRAN T1;

END




