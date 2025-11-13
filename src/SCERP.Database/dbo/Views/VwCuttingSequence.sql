

CREATE VIEW [dbo].[VwCuttingSequence]
AS
Select CS.*,C.ComponentName,Clr.ColorName from PROD_CuttingSequence AS CS
INNER JOIN OM_Component AS C
ON CS.ComponentRefId=C.ComponentRefId and CS.CompId=C.CompId

INNER JOIN OM_Color as Clr on CS.ColorRefId=Clr.ColorRefId and CS.CompId=Clr.CompId



