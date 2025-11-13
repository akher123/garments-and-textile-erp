
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <16/01/2016>
-- Description:	<> exec SPCRMDocumentationReport 0, '01/01/2015', NULL,''
-- =============================================

CREATE PROCEDURE [dbo].[SPCRMDocumentationReport]


						   @ModuleId		INT = 0						  
						  ,@FromDate		DATETIME =  NULL
						  ,@ToDate			DATETIME = NULL
						  ,@SearchString	NVARCHAR(100) = ''
AS

BEGIN
	
						SET NOCOUNT ON;

						SELECT [CRMDocumentationReport].[Id]
							  ,[CRMDocumentationReport].[RefNo]
							  ,[CRMDocumentationReport].[ReportName]
							  ,[CRMDocumentationReport].[Description]
							  ,[CRMDocumentationReport].[Literature]
							  ,[CRMDocumentationReport].[ModuleId]
							  ,[Module].[ModuleName]

							  ,[CRMDocumentationReport].[PhotographPath]

							  ,[CRMDocumentationReport].[ResponsiblePerson]
							  ,CONVERT(VARCHAR(10), [CRMDocumentationReport].[LastUpdateDate], 103) AS [LastUpdateDate]
							  ,[CRMDocumentationReport].[LastUpdateBy]
							  ,[CRMDocumentationReport].[CreatedDate]
							  ,[CRMDocumentationReport].[CreatedBy]
							  ,[CRMDocumentationReport].[EditedDate]
							  ,[CRMDocumentationReport].[EditedBy]
							  ,[CRMDocumentationReport].[IsActive]
							  ,CONVERT(VARCHAR(10), @FromDate, 103) AS FromDate
							  ,CONVERT(VARCHAR(10), @ToDate, 103) AS ToDate

							  FROM [dbo].[CRMDocumentationReport]
							  INNER JOIN Module ON Module.Id = [CRMDocumentationReport].ModuleId

							  WHERE ([CRMDocumentationReport].ModuleId = @ModuleId OR @ModuleId = 0)								   
							  AND (	  
									   [CRMDocumentationReport].RefNo LIKE '%'+ @SearchString +'%'
									OR [CRMDocumentationReport].ReportName LIKE '%'+ @SearchString +'%'
									OR [CRMDocumentationReport].Description LIKE '%'+ @SearchString +'%'
									OR [CRMDocumentationReport].Literature LIKE '%'+ @SearchString +'%'
									OR [Module].[ModuleName] LIKE '%'+ @SearchString +'%'
									OR @SearchString =''
									)
							  AND (CAST([CRMDocumentationReport].LastUpdateDate AS date) >= CAST(@FromDate AS date) OR @FromDate IS NULL)
							  AND (CAST([CRMDocumentationReport].LastUpdateDate AS date) <= CAST(@ToDate AS date) OR @ToDate IS NULL)
							  AND [CRMDocumentationReport].IsActive = 1 																																									 													
END








