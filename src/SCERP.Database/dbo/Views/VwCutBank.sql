
CREATE VIEW [dbo].[VwCutBank]
AS
select CB.*,BOS.BuyerRefId,BOS.BuyerName,BOS.StyleName,BO.RefNo AS OrderRefId,OMC.ColorName,OMS.SizeName,COM.ComponentName 
from PROD_CutBank AS CB
INNER JOIN VOM_BuyOrdStyle AS BOS
ON CB.OrderStyleRefId=BOS.OrderStyleRefId AND CB.CompId=BOS.CompId
INNER JOIN OM_BuyerOrder AS BO
ON BOS.OrderNo=BO.OrderNo AND BOS.CompId=BO.CompId
INNER JOIN OM_Color AS OMC
ON CB.ColorRefId=OMC.ColorRefId AND CB.CompId=OMC.CompId
INNER JOIN OM_Size AS OMS
ON CB.SizeRefId=OMS.SizeRefId AND CB.CompId=OMS.CompId
INNER JOIN OM_Component COM
ON CB.ComponentRefId=COM.ComponentRefId AND CB.CompId=COM.CompId


