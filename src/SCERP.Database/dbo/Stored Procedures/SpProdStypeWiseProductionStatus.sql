CREATE procedure [dbo].[SpProdStypeWiseProductionStatus]
@OrderStyleRefId varchar(7),
@CompId varchar(3)
as
select 
M.EmpName as Merchandiser,
B.BuyerName,
BO.RefNo as OrderNo,
ST.StyleName,
I.ItemName,
C.ColorName,
S.SizeName,
SH.ShipDate,
(
select top(1) [CountryName] from Country where Id=SH.CountryId) AS Country,
SUM(SHD.QuantityP) as OrderQty,
ISNULL((select SUM(Quantity) from PROD_CuttingBatch as CB 
inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId
where CB.ComponentRefId='001' and CB.ColorRefId=SHD.ColorRefId and BC.SizeRefId=SHD.SizeRefId and CB.OrderStyleRefId=BST.OrderStyleRefId and CB.CompId=SHD.CompId and CB.StyleRefId =SH.OrderShipRefId),0)-(ISNULL((select SUM(RJ.RejectQty)from PROD_RejectAdjustment as RJ 
inner join PROD_CuttingBatch as CTB on RJ.CuttingBatchId=CTB.CuttingBatchId
where CTB.OrderStyleRefId=BST.OrderStyleRefId and CTB.ColorRefId=SHD.ColorRefId  and RJ.SizeRefId=SHD.SizeRefId and RJ.CompId=SHD.CompId  and CTB.StyleRefId=SH.OrderShipRefId) ,0)) as CuttQty,
ISNULL((select SUM(SIPD.InputQuantity) from PROD_SewingInputProcess as SIP 
inner join PROD_SewingInputProcessDetail as SIPD on SIP.SewingInputProcessId=SIPD.SewingInputProcessId
where SIP.OrderStyleRefId=BST.OrderStyleRefId and SIP.ColorRefId =SHD.ColorRefId and SIPD.SizeRefId=SHD.SizeRefId and SIP.CompId=SHD.CompId and SIP.OrderShipRefId=SH.OrderShipRefId),0) as SewingInputQty,
ISNULL((select SUM(SOPD.Quantity) from PROD_SewingOutPutProcess as SOP 
inner join PROD_SewingOutPutProcessDetail as SOPD on SOP.SewingOutPutProcessId=SOPD.SewingOutPutProcessId
where SOP.OrderStyleRefId=BST.OrderStyleRefId and SOP.ColorRefId =SHD.ColorRefId and SOPD.SizeRefId=SHD.SizeRefId and SOP.CompId=SHD.CompId and SOP.OrderShipRefId=SH.OrderShipRefId),0) as SewingQty,

ISNULL((select SUM(IRFD.InputQuantity) from PROD_FinishingProcess as IRF
inner join PROD_FinishingProcessDetail as IRFD on IRF.FinishingProcessId=IRFD.FinishingProcessId
where IRF.FType=1 and IRF.ColorRefId=SHD.ColorRefId and IRF.OrderStyleRefId=BST.OrderStyleRefId and IRFD.SizeRefId=SHD.SizeRefId and IRF.CompId=SHD.CompId and IRF.OrderShipRefId=SH.OrderShipRefId),0) as IronQty,

ISNULL((select SUM(PFD.InputQuantity) from PROD_FinishingProcess as PF
inner join PROD_FinishingProcessDetail as PFD on PF.FinishingProcessId=PFD.FinishingProcessId
where PF.FType=2 and PF.ColorRefId=SHD.ColorRefId and PF.OrderStyleRefId=BST.OrderStyleRefId and PFD.SizeRefId=SHD.SizeRefId and PF.CompId=SHD.CompId and PF.OrderShipRefId=SH.OrderShipRefId),0) as PolyQty

from OM_BuyOrdShip as SH
inner join OM_BuyOrdShipDetail as SHD on SH.OrderShipRefId=SHD.OrderShipRefId and SH.CompId=SHD.CompId
inner join OM_BuyOrdStyle as BST on SH.OrderStyleRefId=BST.OrderStyleRefId and SH.CompId=BST.CompId
inner join OM_BuyerOrder as BO on BST.OrderNo=BO.OrderNo and BST.CompId=BO.CompId
inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Style as ST on BST.StyleRefId=ST.StyleRefId and BST.CompId=ST.CompID
inner join OM_Color as C on SHD.ColorRefId=C.ColorRefId and SHD.CompId=C.CompId
inner join OM_Size as S on SHD.SizeRefId=S.SizeRefId and SHD.CompId=S.CompId
inner join Inventory_Item as I on ST.ItemId=I.ItemId
inner join OM_Merchandiser as M on BO.MerchandiserId=M.EmpId and BO.CompId=M.CompId
where SH.OrderStyleRefId=@OrderStyleRefId and SH.CompId=@CompId
group by  B.BuyerName,BO.RefNo ,ST.StyleName,C.ColorName,S.SizeName,SHD.SizeRow,I.ItemName,M.EmpName,BST.OrderStyleRefId,SHD.ColorRefId,SHD.SizeRefId,SHD.CompId,SH.OrderShipRefId,SH.ShipDate,SH.CountryId
order by SHD.SizeRow






