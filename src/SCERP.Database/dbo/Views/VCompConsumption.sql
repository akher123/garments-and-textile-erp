

CREATE view [dbo].[VCompConsumption]
as 
select CMP.CompConsumptionId,CMP.CompId,CMP.ConsRefId,CMP.OrderStyleRefId, CMP.EnDate,CMP.[Description],CMP.NParts,CMP.ComponentSlNo,CMP.FabricType,CMP.ComponentRefId,
(select top(1)[Description] from OM_FabricType where CMP.FabricType=FabricType)as FabricTypeName ,
(select top(1)ComponentName from OM_Component where CMP.ComponentRefId=ComponentRefId and CMP.CompId=CompId)as ComponentName ,
(select top(1) CompType from OM_Component where CMP.ComponentRefId=ComponentRefId and CMP.CompId=CompId)as CompType ,
(select top(1)ItemName from VConsumption where CMP.ConsRefId=ConsRefId and CMP.CompId=CompId)as FabricName, 
(select sum(ISNULL(TQty,0)) from OM_CompConsumptionDetail as COPD where  COPD.ConsRefId=CMP.ConsRefId and COPD.CompomentSlNo=CMP.ComponentSlNo) as TCompQty
 from OM_CompConsumption as CMP




