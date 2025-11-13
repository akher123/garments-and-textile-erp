
CREATE view [dbo].[vwTnaAlert]
as
select 
OM_TNA.CompId,
VOM_BuyOrdStyle.BuyerRefId,
VOM_BuyOrdStyle.OrderNo as OrderRefId,
VOM_BuyOrdStyle.OrderStyleRefId,
OM_TNA.TnaRowId,
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
 DATEDIFF(DAY,GETDATE(),dbo.fnDateConvert(OM_TNA.PSDate)) as DateDiffernce, ( case 
 when DATEDIFF(DAY,GETDATE(),dbo.fnDateConvert(OM_TNA.PSDate)) in (4, 5 ) then 'Y'
 when DATEDIFF(DAY,GETDATE(),dbo.fnDateConvert(OM_TNA.PSDate)) in (3, 2, 1, 0 ) then 'A'
 when DATEDIFF(DAY,GETDATE(),dbo.fnDateConvert(OM_TNA.PSDate)) in (-2, -1 ) then 'R' 
 else 'D' end) as AlertType,

 dbo.fnDateConvert(OM_TNA.ASDate) as ASDate,
  dbo.fnDateConvert(OM_TNA.AEDate) as AEDate,
 --Convert(varchar,OM_TNA.ASDate,103) as ASDate,
-- Convert(varchar,OM_TNA.AEDate,101) as AEDate,
 OM_TNA.Responsible,OM_TNA.UpdateRemarks from OM_TNA
 inner join VOM_BuyOrdStyle on OM_TNA.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and OM_TNA.CompId=VOM_BuyOrdStyle.CompId
  where   VOM_BuyOrdStyle.ActiveStatus=1 and ( DATEDIFF(DAY,dbo.fnDateConvert(OM_TNA.ASDate),GETDATE())<0 or dbo.fnDateConvert(OM_TNA.ASDate) is null) and  DATEDIFF(DAY,GETDATE(),dbo.fnDateConvert(OM_TNA.PSDate)) <=5


