CREATE procedure [dbo].[GetBuyerTnaTemplate]

@BuyerRefId char(3),
@TempTypeId int,
@CompId char(3)
as
select @CompId as CompId, @BuyerRefId as BuyerRefId,TNA.ActivityId, @TempTypeId as TemplateTypeId,TNA.ShortName,
(select top(1) BuyerName from OM_Buyer where BuyerRefId=@BuyerRefId and CompId=@CompId) as BuyerName,
TNA.Name as Activity,
TNA.Responsible,
ISNULL((select top(1) SerialNo from OM_BuyerTnaTemplate where ActivityId=TNA.ActivityId and BuyerRefId=@BuyerRefId and TemplateTypeId=@TempTypeId and CompId=@CompId),0) as SerialNo,
(select top(1) Duration from OM_BuyerTnaTemplate where ActivityId=TNA.ActivityId and BuyerRefId=@BuyerRefId and TemplateTypeId=@TempTypeId and CompId=@CompId) as Duration,
(select top(1) RSerialNo from OM_BuyerTnaTemplate where ActivityId=TNA.ActivityId and BuyerRefId=@BuyerRefId and TemplateTypeId=@TempTypeId and CompId=@CompId) as RSerialNo,
(select top(1) FDuration from OM_BuyerTnaTemplate where ActivityId=TNA.ActivityId and BuyerRefId=@BuyerRefId and TemplateTypeId=@TempTypeId and CompId=@CompId) as FDuration,
(select top(1) RType from OM_BuyerTnaTemplate where ActivityId=TNA.ActivityId and BuyerRefId=@BuyerRefId and TemplateTypeId=@TempTypeId and CompId=@CompId) as RType,
(select top(1) Remarks from OM_BuyerTnaTemplate where ActivityId=TNA.ActivityId and BuyerRefId=@BuyerRefId and TemplateTypeId=@TempTypeId and CompId=@CompId) as Remarks

from OM_TnaActivity as TNA
left join OM_BuyerTnaTemplate as bt on TNA.ActivityId=bt.ActivityId
where  bt.BuyerRefId=@BuyerRefId
order by bt.SerialNo 

--exec GetBuyerTnaTemplate '087',1,'001'





