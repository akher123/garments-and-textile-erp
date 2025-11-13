--EXECUTE SpPrintEmbroiderySummary @CuttingBatchRefId='C00002',@CompId='001',@BuyerRefId='030',@OrderNo='PFL/ORD00046',@OrderStyleRefId='ST00093',@ColorRefId='0013'
--EXECUTE SpPrintEmbroiderySummary @CuttingBatchRefId='000000',@CompId='001',@BuyerRefId='000',@OrderNo='000000000000',@OrderStyleRefId='0000000',@ColorRefId='0000'
CREATE PROCEDURE [dbo].[SpPrintEmbroiderySummary]
(
@CuttingBatchRefId varchar(6),
@BuyerRefId varchar(3),
@OrderNo varchar(12),
@OrderStyleRefId varchar(7),
@ColorRefId varchar(7),
@CompId varchar(3)
)
AS
BEGIN
SELECT CuttingBatchRefId,x.CompId,CompanyName,FullAddress,CuttingDate,JobNo,x.BuyerRefId,BuyerName,x.OrderNo,OrderName,x.OrderStyleRefId,StyleName,ColorRefId,ColorName,ComponentRefId,Sequence,CuttingSequenceId,CuttingTagId,TagRefId,TagName,EmblishmentStatus,FinalCut,TotalSend, PartyName= STUFF((SELECT ', ' + PartyName 
    FROM VwPrintEmbroideryBalance WHERE CuttingBatchRefId=x.CuttingBatchRefId AND TagRefId=x.TagRefId AND EmblishmentStatus = x.EmblishmentStatus 
     ORDER BY CuttingBatchRefId
     FOR XML PATH, TYPE).value('.[1]',
 'nvarchar(max)'),1,1,'')
FROM VwPrintEmbroideryBalance AS x 
inner join PROD_CuttingProcessStyleActive as SINCUT on X.OrderStyleRefId=SINCUT.OrderStyleRefId and x.CompId=SINCUT.CompId
Where (x.CompId=@CompId AND (x.CuttingBatchRefId=@CuttingBatchRefId OR @CuttingBatchRefId='000000') AND (FinalCut-TotalSend)!=0 AND(x.BuyerRefId=@BuyerRefId OR @BuyerRefId='000') AND (x.OrderNo=@OrderNo OR @OrderNo='000000000000')AND (x.OrderStyleRefId=@OrderStyleRefId OR @OrderStyleRefId='0000000') AND (ColorRefId=@ColorRefId OR @ColorRefId='0000')) and CONVERT(date,SINCUT.StartDate)<= CONVERT(date, GETDATE()) and (SINCUT.EndDate is null or  CONVERT(date,SINCUT.EndDate)>= CONVERT(date,GETDATE())) 
GROUP BY CuttingBatchRefId,x.CompId,CompanyName,FullAddress,CuttingDate,JobNo,x.BuyerRefId,BuyerName,x.OrderNo,OrderName,x.OrderStyleRefId,StyleName,ColorRefId,ColorName,ComponentRefId,Sequence,CuttingSequenceId,CuttingTagId,TagRefId,TagName,EmblishmentStatus,FinalCut,TotalSend
ORDER BY CuttingBatchRefId;

END