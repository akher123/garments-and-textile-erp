
CREATE view [dbo].[VwDyeingJobOrderDetail]
as
select 
jd.*,
( select top(1) ItemName from Inventory_Item where ItemId=jd.ItemId and CompId=jd.CompId ) as ItemName,
( select top(1) ComponentName from OM_Component where ComponentRefId=jd.ComponentRefId and CompId=Jd.CompId) as ComponentName,
(select top(1) ColorName from OM_Color where ColorRefId=jd.ColorRefId and CompId=jd.CompId) as ColorName,
 (select top(1) SizeName from OM_Size where SizeRefId=jd.FdSizeRefId and CompId=jd.CompId) as FdName,
 (select top(1) SizeName from OM_Size where SizeRefId=jd.MdSizeRefId and CompId=jd.CompId) as MdName
 from PROD_DyeingJobOrderDetail as jd







