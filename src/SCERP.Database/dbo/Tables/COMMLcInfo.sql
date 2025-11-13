CREATE TABLE [dbo].[COMMLcInfo] (
    [LcId]                   INT              IDENTITY (1, 1) NOT NULL,
    [LcNo]                   NVARCHAR (50)    NULL,
    [LcDate]                 DATETIME         NULL,
    [BuyerId]                BIGINT           NULL,
    [LcAmount]               DECIMAL (18, 2)  NULL,
    [LcQuantity]             DECIMAL (18, 2)  NULL,
    [MatureDate]             DATETIME         NULL,
    [ExpiryDate]             DATETIME         NULL,
    [ExtensionDate]          DATETIME         NULL,
    [ShipmentDate]           DATETIME         NULL,
    [LcIssuingBank]          NVARCHAR (100)   NULL,
    [LcIssuingBankAddress]   NVARCHAR (500)   NULL,
    [ReceivingBank]          NVARCHAR (100)   NULL,
    [ReceivingBankAddress]   NVARCHAR (500)   NULL,
    [ReceivingBankId]        INT              NULL,
    [SalesContactNo]         NVARCHAR (100)   NULL,
    [LcType]                 INT              NULL,
    [Beneficary]             NVARCHAR (50)    NULL,
    [PartialShipment]        INT              NULL,
    [Description]            NVARCHAR (MAX)   NULL,
    [AppliedDate]            DATETIME         NULL,
    [IncentiveClaimValue]    FLOAT (53)       NULL,
    [NewMarketCliam]         VARCHAR (150)    NULL,
    [BTMACertificate]        VARCHAR (10)     NULL,
    [BkmeaCertificat]        VARCHAR (10)     NULL,
    [FirstAuditStatus]       VARCHAR (10)     NULL,
    [CertificateOvservation] VARCHAR (10)     NULL,
    [FinalClaimAmount]       FLOAT (53)       NULL,
    [ReceiveAmount]          FLOAT (53)       NULL,
    [ReceiveDate]            DATETIME         NULL,
    [CashIncentiveRemarks]   VARCHAR (250)    NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    [RStatus]                CHAR (1)         NULL,
    [CommissionPrc]          FLOAT (53)       NULL,
    [CommissionsAmount]      FLOAT (53)       NULL,
    [FreightAmount]          FLOAT (53)       NULL,
    [UdEoNo]                 VARCHAR (100)    NULL,
    [FileNo]                 VARCHAR (100)    NULL,
    [GroupLcId]              INT              NULL,
    CONSTRAINT [PK_CommercialLcInfo] PRIMARY KEY CLUSTERED ([LcId] ASC)
);


GO
CREATE TRIGGER [dbo].[TRIG_LC_MailSend]

ON dbo.COMMLcInfo
		
FOR INSERT, UPDATE, DELETE
		
AS
	BEGIN
	
		DECLARE @PersonName		NVARCHAR(100)
		DECLARE @MailAddress	NVARCHAR(100)
		DECLARE @Subject		NVARCHAR(200)
		DECLARE @Body			NVARCHAR(MAX)
	
	    DECLARE @LcNo			NVARCHAR(50)
		DECLARE @LcDate			DATETIME
		DECLARE @Buyer			NVARCHAR(50)
		DECLARE @LcAmount		DECIMAL(18,2)
		DECLARE @LcQauntity		DECIMAL(18,2)


		DECLARE Mail_cursor CURSOR FOR

			SELECT PersonName, MailAddress			  
			FROM MailSend 
			WHERE MailSend.MailType = 'lc' AND MailSend.IsActive = 1 ;

		OPEN Mail_cursor;
			FETCH NEXT FROM Mail_cursor
			INTO @PersonName, @MailAddress

		WHILE @@FETCH_STATUS = 0

		   BEGIN
			    
			    SELECT TOP(1) 
						@LcNo = LcNo 
					   ,@LcDate= LcDate
					   ,@Buyer = ISNULL(OM_Buyer.BuyerName, 'Not Found')
					   ,@LcAmount = ISNULL(LcAmount, 0)
					   ,@LcQauntity = ISNULL(LcQuantity, 0)

				FROM COMMLcInfo 
				LEFT JOIN OM_Buyer ON OM_Buyer.BuyerId = COMMLcInfo.BuyerId
				ORDER BY LcId DESC
								
				SET @Subject = 'A New LC has been Added/Updated to SC ERP'
			    SET @Body = 'Dear '							+ @PersonName + ',' 
							+ CHAR(13) + CHAR(13)			+ 'A New LC has been Added/Updated to SC ERP'
				 + CHAR(13) + CHAR(13) + 'LC NO     : '		+ @LcNo
							+ CHAR(13) + 'Buyer Name : '	+ CAST(@Buyer AS NVARCHAR(50))
							+ CHAR(13) + 'LC Date   : '		+ CAST(@LcDate AS NVARCHAR(12))
							+ CHAR(13) + 'LC Amount : '		+ CAST(@LcAmount AS NVARCHAR(20))
		     				+ CHAR(13) + 'LC Quantity : '	+ CAST(@LcQauntity AS NVARCHAR(20))
				 + CHAR(13) + CHAR(13) + '- SCERP Authority'
									
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
