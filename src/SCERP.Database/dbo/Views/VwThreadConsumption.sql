
CREATE view [dbo].[VwThreadConsumption]
as
select com.* ,
(select BuyerName from OM_Buyer where BuyerRefId=com.BuyerRefId and CompId=com.CompId) as BuyerName
,ST.RefNo as OrderName,ST.StyleName,ST.ItemName,
(select SizeName from OM_Size where SizeRefId=com.SizeRefId and CompId=com.CompId) as SizeName
from OM_ThreadConsumption as com
inner join VOMBuyOrdStyle as ST on com.OrderStyleRefId=ST.OrderStyleRefId and com.CompId=ST.CompId
