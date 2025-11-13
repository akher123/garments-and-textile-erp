CREATE TABLE [dbo].[CommExport] (
    [ExportId]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [ExportRefId]      CHAR (7)         NOT NULL,
    [ExportNo]         VARCHAR (25)     NULL,
    [ExportDate]       DATE             NULL,
    [InvoiceNo]        VARCHAR (25)     NULL,
    [InvoiceDate]      DATE             NULL,
    [InvoiceQuantity]  DECIMAL (18, 5)  CONSTRAINT [DF_CommExport_InvoiceQuantity] DEFAULT ((0)) NULL,
    [InvoiceValue]     DECIMAL (18, 5)  NULL,
    [BankRefNo]        VARCHAR (25)     NULL,
    [BankRefDate]      DATE             NULL,
    [RealizedValue]    DECIMAL (18, 5)  NULL,
    [RealizedDate]     DATE             NULL,
    [ShortFallAmount]  DECIMAL (18, 5)  NULL,
    [ShortFallReason]  NVARCHAR (100)   NULL,
    [BillOfLadingNo]   VARCHAR (25)     NULL,
    [BillOfLadingDate] DATE             NULL,
    [SBNo]             VARCHAR (25)     NULL,
    [SBNoDate]         DATE             NULL,
    [LcId]             INT              NOT NULL,
    [CompId]           VARCHAR (3)      NOT NULL,
    [FcAmount]         DECIMAL (18, 5)  NULL,
    [UdNoLocal]        NVARCHAR (50)    NULL,
    [UdNoForeign]      NVARCHAR (50)    NULL,
    [UdDateLocal]      DATETIME         NULL,
    [UdDateForeign]    DATETIME         NULL,
    [PaymentMode]      NVARCHAR (50)    NULL,
    [IncoTerm]         NVARCHAR (50)    NULL,
    [ShipmentMode]     NVARCHAR (50)    NULL,
    [PortOfLanding]    NVARCHAR (50)    NULL,
    [PortOfDischarge]  NVARCHAR (50)    NULL,
    [FinalDestination] NVARCHAR (50)    NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NULL,
    [CreateDate]       DATETIME         NULL,
    [EditedBy]         UNIQUEIDENTIFIER NULL,
    [EditedDate]       DATE             NULL,
    [SalseContactId]   INT              NULL,
    CONSTRAINT [PK_CommExport] PRIMARY KEY CLUSTERED ([ExportId] ASC),
    CONSTRAINT [FK_CommExport_COMMLcInfo] FOREIGN KEY ([LcId]) REFERENCES [dbo].[COMMLcInfo] ([LcId])
);


GO
CREATE TRIGGER [dbo].[TRIG_EXPORT_MailSend]

ON dbo.CommExport
		
FOR INSERT, UPDATE, DELETE
		
AS
	BEGIN
	
		DECLARE @PersonName		NVARCHAR(100)
		DECLARE @MailAddress	NVARCHAR(100)
		DECLARE @Subject		NVARCHAR(200)
		DECLARE @Body			NVARCHAR(MAX)
	
	    DECLARE @InvoiceNo		NVARCHAR(50)
		DECLARE @InvoiceDate	DATETIME
		DECLARE @InvoiceValue	DECIMAL(18,2)
		DECLARE @LCNo			NVARCHAR(50)		
		DECLARE @BuyerName		NVARCHAR(50)

		DECLARE Mail_cursor CURSOR FOR

			SELECT PersonName, MailAddress			  
			FROM MailSend 
			WHERE MailSend.MailType = 'export' AND MailSend.IsActive = 1 ;

		OPEN Mail_cursor;
			FETCH NEXT FROM Mail_cursor
			INTO @PersonName, @MailAddress

		WHILE @@FETCH_STATUS = 0

		   BEGIN
			    
			    SELECT TOP(1) 
					@InvoiceNo = ISNULL(InvoiceNo,'Not Found')
				   ,@InvoiceDate = InvoiceDate			
				   ,@InvoiceValue = ISNULL(InvoiceValue, 0)
				   ,@LCNo =	ISNULL(COMMLcInfo.LcNo,'Not Found')
				   ,@BuyerName = ISNULL(OM_Buyer.BuyerName,'Not Found')

				FROM CommExport 
				LEFT JOIN COMMLcInfo ON COMMLcInfo.LcId = CommExport.LcId
				LEFT JOIN OM_Buyer ON OM_Buyer.BuyerId = COMMLcInfo.BuyerId	
				ORDER BY ExportId DESC
								
				SET @Subject = 'A New Exp has been Added/Updated to SC ERP'
			    SET @Body = 'Dear '							+ @PersonName + ',' 
							+ CHAR(13) + CHAR(13)			+ 'A New Exp has been Added/Updated to SC ERP'
				 + CHAR(13) + CHAR(13) + 'Invoice NO     : '		+ @InvoiceNo
							+ CHAR(13) + 'Invoice Date   : '		+ CAST(@InvoiceDate AS NVARCHAR(12))
							+ CHAR(13) + 'Invoice Value  : '		+ CAST(@InvoiceValue AS NVARCHAR(20))
		     				+ CHAR(13) + 'LC No            : '		+ CAST(@LCNo AS NVARCHAR(20))
							+ CHAR(13) + 'Buyer Name   : '		+ CAST(@BuyerName AS NVARCHAR(20))

				 + CHAR(13) + CHAR(13) + 'Best Regards,'
				 + CHAR(13) + '- SCERP Authority'
									
   				EXEC msdb.dbo.sp_send_dbmail
					@profile_name = 'SCERP'
				   ,@recipients = @MailAddress
				   ,@subject = @Subject
				   ,@body = @Body	
			   
			   FETCH NEXT FROM Mail_cursor
			   INTO @PersonName, @MailAddress 
			   		 
		   END 
	
		CLOSE Mail_cursor
		DEALLOCATE Mail_cursor
			
	END
