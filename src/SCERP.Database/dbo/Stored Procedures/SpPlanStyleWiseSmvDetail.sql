-- =============================================
-- Author:		<Author,,Md.Akheruzzaman>
-- Create date: <Create Date,05/03/2016,>
-- Description:	<SMV Detail Style Wise>
-- =============================================
CREATE PROCEDURE SpPlanStyleWiseSmvDetail 
@CompId varchar(3),
@FromDate datetime=null,
@ToDate datetime=null
AS
BEGIN

	SET NOCOUNT ON;
				select 
				BO.RefNo as OrderNo,
				ST.StyleName,
				B.BuyerName as Buyer,
				OST.Quantity,
				Mrc.EmpName as Merchandiser,
				ISNULL(smv.StMv,0) as StMv,
				round(ISNULL(OST.Quantity,0) * ISNULL(smv.StMv,0),0) as TotalJob, 
				round(ISNULL(OST.Quantity,0) * ISNULL(smvd.StMvD,0),0) as TotalJobDtl ,
				ISNULL(smvd.StMvD,0) as StMvD,
				SP.SubProcessName 
				from OM_BuyerOrder AS BO
				inner join OM_BuyOrdStyle as OST on BO.OrderNo=OST.OrderNo and BO.CompId=OST.CompId
				inner join OM_Style as ST on OST.StyleRefId =ST.StylerefId
				inner join OM_Buyer AS B ON BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
				inner join OM_Merchandiser as Mrc on BO.MerchandiserId=Mrc.EmpId and BO.CompId=Mrc.CompId 
				left join PROD_StanderdMinValue as smv on OST.OrderStyleRefId=smv.OrderStyleRefId and OST.CompId=smv.CompId
				left join PROD_StanderdMinValDetail as smvd on smv.StanderdMinValueId=smvd.StanderdMinValueId
				left join PROD_SubProcess as SP on smvd.SubProcessRefId=SP.SubProcessRefId and SP.CompId=smvd.CompId
				where BO.CompId=@CompId and (OST.EFD between @FromDate and @ToDate)
 				order by BO.BuyerOrderId 
END
