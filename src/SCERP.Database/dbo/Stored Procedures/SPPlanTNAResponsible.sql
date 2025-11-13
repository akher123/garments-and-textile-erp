-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <28/01/2016>
-- Description:	<> exec SPPlanTNAResponsible '','001'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPPlanTNAResponsible]
			
							
						    @PersonName		NVARCHAR(50)	
						   ,@CompanyId		NVARCHAR(3)						 


AS
BEGIN
	
			SET NOCOUNT ON;

					SELECT PLAN_TNA.Id
						  ,PLAN_TNA.CompId
						  ,PLAN_TNA.OrderStyleRefId
						  ,PLAN_TNA.ActivityId
						  ,OM_Buyer.BuyerName
						  ,OM_Style.StyleName
						  ,OM_BuyerOrder.RefNo AS OrderNo
						  ,CONVERT(VARCHAR(10),OM_BuyerOrder.OrderDate, 103) AS OrderDate
						  ,OM_Merchandiser.EmpName AS MerchandiserName
						  ,PLAN_Activity.ActivityName
						  ,PLAN_TNA.LeadTime
						  ,CONVERT(VARCHAR(10),PLAN_TNA.PlannedStartDate , 103) AS PlannedStartDate
						  ,CONVERT(VARCHAR(10),PLAN_TNA.PlannedEndDate, 103) AS PlannedEndDate
						  ,CONVERT(VARCHAR(10),PLAN_TNA.ActualStartDate, 103) AS ActualStartDate
						  ,CONVERT(VARCHAR(10),PLAN_TNA.ActrualEndDate, 103) AS ActrualEndDate
						  ,PLAN_TNA.ResponsiblePerson
						  ,PLAN_ResponsiblePerson.ResponsiblePersonName
						  ,PLAN_TNA.NotifyBeforeDays
						  ,PLAN_TNA.Remarks
						  ,PLAN_TNA.CreatedDate
						  ,PLAN_TNA.CreatedBy
						  ,PLAN_TNA.EditedDate
						  ,PLAN_TNA.EditedBy
						  ,PLAN_TNA.IsActive

						  FROM PLAN_TNA
						  LEFT JOIN PLAN_Activity ON PLAN_Activity.Id = PLAN_TNA.ActivityId AND PLAN_Activity.CompId = @CompanyId
						  LEFT JOIN PLAN_ResponsiblePerson ON PLAN_ResponsiblePerson.ResponsiblePersonId = PLAN_TNA.ResponsiblePerson
						  LEFT JOIN OM_BuyOrdStyle	ON PLAN_TNA.OrderStyleRefId = OM_BuyOrdStyle.OrderStyleRefId AND OM_BuyOrdStyle.CompId = @CompanyId		  								
						  LEFT JOIN OM_BuyerOrder ON OM_BuyerOrder.OrderNo = OM_BuyOrdStyle.OrderNo AND OM_BuyerOrder.CompId = @CompanyId
						  LEFT JOIN OM_Buyer ON OM_Buyer.BuyerRefId = OM_BuyerOrder.BuyerRefId AND OM_Buyer.CompId = OM_BuyerOrder.CompId AND OM_Buyer.CompId = @CompanyId
						  LEFT JOIN OM_Style ON OM_Style.StylerefId = OM_BuyOrdStyle.StyleRefId AND OM_Style.CompID = @CompanyId
						  LEFT JOIN OM_Merchandiser ON OM_Merchandiser.EmpId = OM_BuyerOrder.MerchandiserId AND OM_Merchandiser.CompId = @CompanyId
						 
						  WHERE PLAN_TNA.IsActive = 1
						  AND (PLAN_ResponsiblePerson.ResponsiblePersonName LIKE '%'+ @PersonName +'%' OR @PersonName = '')	
						  AND PLAN_TNA.CompId = @CompanyId							  
						  											  							
END
