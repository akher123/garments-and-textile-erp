

CREATE procedure [dbo].[SpOmFabricWorkOrderSheet]
@OrderStyleRefId varchar(7),
@CompId varchar(3)
as
select  cp.Name as CompanyName,cp.FullAddress, BOST.BuyerName,BOST.RefNo as OrderName,BOST.StyleName , ConsD.ItemName,ConsD.UnitName,ConsD.GColorName,ConsD.GSizeName,ConsD.QuantityP as OrderQty,  ConsD.PColorName,ConsD.PSizeName,ConsD.TQty,ConsD.TableWidth,ConsD.BaseColorName,ConsD.PAllow,ConsD.PPQty,ConsD.PDzCons ,BOST.Merchandiser from VCompConsumptionDetail as ConsD
inner join VOM_BuyOrdStyle as BOST on ConsD.OrderStyleRefId=BOST.OrderStyleRefId and ConsD.CompId=BOST.CompId
inner join Company as cp on ConsD.CompId=Cp.CompanyRefId
where ConsD.OrderStyleRefId=@OrderStyleRefId and ConsD.CompId=@CompId


