CREATE procedure spTnaStatus
@CompId varchar(3),
@IndicationKey int
as



select OM_TNA.TnaRowId,
 VOM_BuyOrdStyle.BuyerName,
 VOM_BuyOrdStyle.RefNo as OrderNo,
 VOM_BuyOrdStyle.StyleName,
 Quantity,
 VOM_BuyOrdStyle.Merchandiser,
 OM_TNA.SerialId,
 OM_TNA.ActivityName,
 OM_TNA.PSDate,
 OM_TNA.PEDate,
 OM_TNA.Rmks,
 OM_TNA.XWho,
 OM_TNA.XWhen,
 Convert(varchar,OM_TNA.ASDate,103) as ASDate,
 Convert(varchar,OM_TNA.AEDate,101) as AEDate,
 OM_TNA.Responsible,OM_TNA.UpdateRemarks from OM_TNA
 inner join VOM_BuyOrdStyle on OM_TNA.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and OM_TNA.CompId=VOM_BuyOrdStyle.CompId
 where   VOM_BuyOrdStyle.ActiveStatus=1 and
 OM_TNA.CompId=@CompId
 and  DATEDIFF(DAY,GETDATE(),dbo.fnDateConvert(OM_TNA.PSDate))  in (@IndicationKey)
  order by OM_TNA.SerialId 


