CREATE TABLE [dbo].[CommBbLcInfo] (
    [BbLcId]                 INT              IDENTITY (1, 1) NOT NULL,
    [LcRefId]                INT              NOT NULL,
    [BbLcNo]                 NVARCHAR (50)    NULL,
    [BbLcDate]               DATETIME         NULL,
    [SupplierCompanyRefId]   INT              NULL,
    [BbLcAmount]             DECIMAL (18, 2)  NULL,
    [BbLcQuantity]           DECIMAL (18, 2)  NULL,
    [MatureDate]             DATETIME         NULL,
    [ExpiryDate]             DATETIME         NULL,
    [ExtensionDate]          DATETIME         NULL,
    [BbLcIssuingBank]        NVARCHAR (100)   NULL,
    [BbLcIssuingBankAddress] NVARCHAR (500)   NULL,
    [IssuingBankId]          INT              NULL,
    [ReceivingBank]          NVARCHAR (100)   NULL,
    [ReceivingBankAddress]   NVARCHAR (500)   NULL,
    [BbLcType]               INT              NULL,
    [Beneficiary]            NVARCHAR (50)    NULL,
    [PartialShipment]        INT              NULL,
    [Description]            NVARCHAR (MAX)   NULL,
    [IfdbcNo]                NVARCHAR (100)   NULL,
    [IfdbcDate]              DATETIME         NULL,
    [IfdbcValue]             DECIMAL (18, 2)  NULL,
    [PcsSanctionAmount]      INT              NULL,
    [PaymentDate]            DATETIME         NULL,
    [BtmeaNo]                NVARCHAR (50)    NULL,
    [BtmeaDate]              DATETIME         NULL,
    [BeNo]                   NVARCHAR (50)    NULL,
    [BeDate]                 DATETIME         NULL,
    [Vat]                    DECIMAL (18, 4)  NULL,
    [CreatedDate]            DATETIME         NULL,
    [CreatedBy]              UNIQUEIDENTIFIER NULL,
    [EditedDate]             DATETIME         NULL,
    [EditedBy]               UNIQUEIDENTIFIER NULL,
    [IsActive]               BIT              NOT NULL,
    [CompId]                 VARCHAR (3)      NULL,
    [ItemType]               INT              NULL,
    [SalseContactId]         INT              NULL,
    CONSTRAINT [PK_CommBbLcInfo] PRIMARY KEY CLUSTERED ([BbLcId] ASC),
    CONSTRAINT [FK_CommBbLcInfo_COMMLcInfo] FOREIGN KEY ([LcRefId]) REFERENCES [dbo].[COMMLcInfo] ([LcId]),
    CONSTRAINT [FK_CommBbLcInfo_Mrc_SupplierCompany] FOREIGN KEY ([SupplierCompanyRefId]) REFERENCES [dbo].[Mrc_SupplierCompany] ([SupplierCompanyId])
);


GO
CREATE TRIGGER [dbo].[TRIG_BBLC_MailSend]

ON dbo.CommBbLcInfo
		
FOR INSERT, UPDATE, DELETE
		
AS
	BEGIN
	
		DECLARE @PersonName		NVARCHAR(100)
		DECLARE @MailAddress	NVARCHAR(100)
		DECLARE @Subject		NVARCHAR(200)
		DECLARE @Body			NVARCHAR(MAX)
	
		DECLARE @MasterLcNo		NVARCHAR(50)
		DECLARE @BuyerName		NVARCHAR(50)
	    DECLARE @BBLcNo			NVARCHAR(50)
		DECLARE @BBLcDate		DATETIME
		DECLARE @Supplier		NVARCHAR(50)
		DECLARE @BBLcAmount		DECIMAL(18,2)
		DECLARE @BBLcQauntity	DECIMAL(18,2)


		DECLARE Mail_cursor CURSOR FOR

			SELECT PersonName, MailAddress			  
			FROM MailSend 
			WHERE MailSend.MailType = 'bblc' AND MailSend.IsActive = 1 ;

		OPEN Mail_cursor;
			FETCH NEXT FROM Mail_cursor
			INTO @PersonName, @MailAddress

		WHILE @@FETCH_STATUS = 0

		   BEGIN
			    
			    SELECT TOP(1) 
					@MasterLcNo =	ISNULL(COMMLcInfo.LcNo, 'Not Found')
				   ,@BuyerName =	ISNULL(OM_Buyer.BuyerName, 'Not Found')
				   ,@BBLcNo =		ISNULL(BbLcNo, 'Not Found')
				   ,@BBLcDate=		BbLcDate
				   ,@Supplier =		ISNULL(Mrc_SupplierCompany.CompanyName, 'Not Found')
				   ,@BBLcAmount =	ISNULL(BbLcAmount, 0)
				   ,@BBLcQauntity = ISNULL(BbLcQuantity, 0)

				FROM CommBbLcInfo 
				LEFT JOIN COMMLcInfo ON COMMLcInfo.LcId = CommBbLcInfo.LcRefId
				LEFT JOIN OM_Buyer ON OM_Buyer.BuyerId = COMMLcInfo.BuyerId AND OM_Buyer.CompId = '001'
				LEFT JOIN Mrc_SupplierCompany ON Mrc_SupplierCompany.SupplierCompanyId = CommBbLcInfo.SupplierCompanyRefId AND Mrc_SupplierCompany.IsActive = 1
				WHERE  CommBbLcInfo.IsActive = 1
				ORDER BY BbLcId DESC
								
				SET @Subject = 'A New BBLC has been Added/Updated to SC ERP'

			    SET @Body = 'Dear '								+ @PersonName + ',' 
							+ CHAR(13) + CHAR(13)				+ 'A New BBLC has been Added/Updated to SC ERP'
				 + CHAR(13) + CHAR(13) + 'Buyer Name     : '	+ @BuyerName
				            + CHAR(13) + 'Master LC No : '		+ @MasterLcNo
							+ CHAR(13) + 'BB LC No     : '		+ @BBLcNo
							+ CHAR(13) + 'Supplier Name : '		+ CAST(@Supplier AS NVARCHAR(20))
							+ CHAR(13) + 'BB LC Date   : '		+ CAST(@BBLcDate AS NVARCHAR(12))
							+ CHAR(13) + 'BB LC Amount : '		+ CAST(@BBLcAmount AS NVARCHAR(20))											
		     									
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
